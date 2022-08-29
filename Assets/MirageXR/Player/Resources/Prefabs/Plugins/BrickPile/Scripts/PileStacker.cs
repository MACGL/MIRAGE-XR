using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class PileStacker : MonoBehaviour
{
    [SerializeField] private TMP_Text low;
    [SerializeField] private TMP_Text high;
    [SerializeField] private Slider slider;


    private int brickCount;
    public int BrickCount
    {
        get
        {
            return brickCount;
        }
        set
        {
            brickCount = value;
            BrickCountChanged(value);
        }
    }

    [SerializeField] private int maxXCount;
    [SerializeField] private int maxYCount;
    [SerializeField] private int maxZCount;

    [SerializeField] private GameObject brickPreFab;
    float margin = .2f;

    private GameObject container;
    private GameObject[] spawnedBricksArray;
    private int lastFrameBricks = 0;
    private int currentPhysicalBricks;
    private MaterialPropertyBlock propertyBlock;

    private void Start()
    {
        slider.onValueChanged.AddListener((value) => BrickCount = (int)(slider.value * (int.Parse(high.text) - int.Parse(low.text))));

        spawnedBricksArray = new GameObject[maxXCount * maxZCount];
        container = Instantiate(new GameObject(), transform);

        float remapX = 1f / maxXCount;
        float remapY = 1f / maxYCount;
        float remapZ = 1f / maxZCount;

        brickPreFab.transform.localScale = new Vector3(remapX, remapY, remapZ);

        for (int i = 0; i < maxZCount; i++)
            for(int j = 0; j < maxXCount; j++)
            {
                GameObject g = Instantiate(brickPreFab, container.transform);
                g.transform.localPosition = new Vector3(j * remapX + remapX / 2, 0, i * remapZ + remapZ / 2);
                g.SetActive(false);
                spawnedBricksArray[i * maxXCount + j] = g;
            }
    }

    private void BrickCountChanged(int newValue)
    {
        int pileTotal = maxXCount * maxYCount * maxZCount;

        int pileCount = newValue / pileTotal;
        int yCount = (newValue - (pileCount * pileTotal)) / (maxXCount * maxZCount);
        int zCount = (newValue - (pileCount * pileTotal + yCount * maxXCount * maxZCount)) / maxXCount;
        int xCount = newValue - (pileCount * pileTotal + yCount * maxXCount * maxZCount + zCount * maxXCount);

        CreateCube(pileCount, yCount);

        container.transform.localPosition = new Vector3(pileCount + pileCount * margin + (yCount % 2 == 0 ? 1 : 0), 
            (float)yCount / maxYCount + brickPreFab.transform.localScale.y / 2, 
            0);
        container.transform.localRotation = yCount % 2 == 0 ? Quaternion.Euler(0, -90, 0) : Quaternion.identity;

        currentPhysicalBricks = zCount * maxXCount + xCount;
        if (propertyBlock == null)
            propertyBlock = new MaterialPropertyBlock();

        for (int i = 0; i < spawnedBricksArray.Length; i++)
        {
            if(i < currentPhysicalBricks)
            {
                if (!spawnedBricksArray[i].activeSelf)
                {
                    propertyBlock.SetFloat("_Run", lastFrameBricks < newValue ? 1f : 0f);
                    propertyBlock.SetFloat("_CurrentTime", Time.time);
                    spawnedBricksArray[i].GetComponentInChildren<MeshRenderer>().SetPropertyBlock(propertyBlock);
                }
                spawnedBricksArray[i].SetActive(true);
            }
            else
            {
                spawnedBricksArray[i].SetActive(false);
            }
        }

        lastFrameBricks = newValue;
    }

    private List<Vector3> GetVerts(int p, float height = 10)
    {
        List<Vector3> verticies = new List<Vector3>();

        Vector3[] corners =
            {
                new Vector3 (p + p * margin, 0, 0),
                new Vector3 (p + p * margin + 1, 0, 0),
                new Vector3 (p + p * margin + 1, 0, 1),
                new Vector3 (p + p * margin, 0, 1),
                new Vector3 (p + p * margin, height / maxYCount, 0),
                new Vector3 (p + p * margin + 1, height / maxYCount, 0),
                new Vector3 (p + p * margin + 1, height / maxYCount, 1),
                new Vector3 (p + p * margin, height / maxYCount, 1)
            };

        int ring = 4;

        for (int i = 0; i < ring; i++)
        {
            verticies.Add(corners[i]);
            verticies.Add(corners[(i + 1) % ring]);
            verticies.Add(corners[(i + 1) % ring + ring]);
            verticies.Add(corners[(i + ring)]);
        }

        if(height % 2 == 1)
        {
            verticies.AddRange(corners.Skip(4).Take(4));
        }
        else
        {
            verticies.AddRange(corners.Skip(5).Take(3));
            verticies.Add(corners[4]);
        }

        return verticies;
    }

    private List<Vector2> GetUVs(float height = 10)
    {
        List<Vector2> uvs = new List<Vector2>();
        int ring = 4;

        for (int i = 0; i < ring; i++)
        {
            uvs.AddRange(GetUVSquareFromPosition(new Vector2(i * .25f, 0), height / maxYCount));
        }

        uvs.AddRange(GetUVSquareFromPosition(new Vector2(.25f, .25f)));

        return uvs;
    }

    private Vector2[] GetUVSquareFromPosition(Vector2 position, float height = 1)
    {
        Vector2[] square = new Vector2[4];

        square[0] = position;
        square[1] = position + new Vector2(.25f, 0);
        square[2] = position + new Vector2(.25f, .25f * height);
        square[3] = position + new Vector2(0, .25f * height);

        return square;
    }

    private void CreateCube(int piles = 0, float height = 10)
    {
        List<Vector3> verticies = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();
        for (int p = 0; p < piles; p++)
        {
            verticies.AddRange(GetVerts(p));
            uvs.AddRange(GetUVs());
        }
        if(height > 0)
        {
            verticies.AddRange(GetVerts(piles, height));
            uvs.AddRange(GetUVs(height));
        }

        for (int i = 0; i < verticies.Count; i += 4)
        {
            triangles.Add(i);
            triangles.Add(i + 2);
            triangles.Add(i + 1);
            triangles.Add(i);
            triangles.Add(i + 3);
            triangles.Add(i + 2);
        }

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = verticies.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }
}

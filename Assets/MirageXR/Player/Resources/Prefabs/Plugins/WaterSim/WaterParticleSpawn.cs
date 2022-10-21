using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterParticleSpawn : MonoBehaviour
{
    [SerializeField] private GameObject waterPrefab;

    private int maxParticles = 3000;
    private int particlesPerSqrUnitPerSec = 100;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnWater());
    }

    private IEnumerator SpawnWater()
    {
        for(int i = 0; i < maxParticles; i++)
        {
            Instantiate(waterPrefab, transform.position + new Vector3(transform.lossyScale.x * Random.Range(-.5f, .5f), -waterPrefab.transform.lossyScale.y * .5f, transform.lossyScale.z * Random.Range(-.5f, .5f)), Quaternion.identity);
            yield return new WaitForSeconds(1f / (particlesPerSqrUnitPerSec * transform.localScale.x * transform.localScale.z));
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using i5.Toolkit.Core.Utilities;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class RainSpawner : MonoBehaviour
{
    [SerializeField] private float dropsPerSecond;
    [SerializeField] private GameObject rainDropPrefab;
    [SerializeField] private TMP_Text text;
    
    private float timeSinceLastDrop = 0;
    float timeBetweenDrops;

    private void Start()
    {
        timeBetweenDrops = 1 / dropsPerSecond;
        text.text = dropsPerSecond.ToString();
    }

    private void Update()
    {
        timeBetweenDrops = 1 / dropsPerSecond;
        timeSinceLastDrop += Time.deltaTime;
        if (timeSinceLastDrop > timeBetweenDrops)
        {
            int amount = Mathf.FloorToInt(timeSinceLastDrop / timeBetweenDrops);
            var extends = transform.lossyScale / 2f;
            for (int i = 0; i < amount; i++)
            {
                GameObject newDrop = Instantiate(
                        rainDropPrefab,
                  transform.position + new Vector3(
                        Random.Range(-extends.x, extends.x), 
                        Random.Range(-extends.y, extends.y),
                        Random.Range(-extends.z, extends.z)), 
                            Quaternion.identity);
                //newDrop.transform.SetParent(transform);
            }

            timeSinceLastDrop -= amount * timeBetweenDrops;
        }
    }

    public void SetDropsPerSecond(float amount)
    {
        dropsPerSecond = amount;
        timeBetweenDrops = 1 / dropsPerSecond;
        text.text = dropsPerSecond.ToString();
    }

}

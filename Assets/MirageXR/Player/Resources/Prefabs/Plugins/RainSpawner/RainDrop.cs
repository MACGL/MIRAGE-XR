using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class RainDrop : MonoBehaviour
{
    [SerializeField] private float  lifeTime = 10f;
    private float timeAlive;
    void Update()
    {
        timeAlive += Time.deltaTime;
        if(lifeTime <= timeAlive) Destroy(gameObject);
    }
}

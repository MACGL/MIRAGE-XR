using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSettings : MonoBehaviour
{
    [SerializeField, Range(0, 100)] private int angleInPercent;
    [SerializeField] private GameObject LaneContainer;

    private void Update()
    {
        LaneContainer.transform.rotation = Quaternion.Euler(0, 0, (float)-angleInPercent / 100 * 45);
    }
}

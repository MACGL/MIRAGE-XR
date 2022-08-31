using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleThroughModels : MonoBehaviour
{
    [SerializeField] private float tickTime = 2f;
    private int _currentModelID = 0;
    private int _dir = 1;
    
    // Start is called before the first frame update
    private IEnumerator Start()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        transform.localScale = new Vector3(1/transform.lossyScale.x, 
                                            1/transform.lossyScale.y, 
                                            1/transform.lossyScale.z);
        YieldInstruction wait = new WaitForSeconds(tickTime);
        while (true)
        {
            yield return wait;
            transform.GetChild(_currentModelID).gameObject.SetActive(false);
            _currentModelID+=_dir;
            if (_currentModelID >= transform.childCount - 1 || _currentModelID <= 0)
            {
                _dir = -_dir;
            }
            transform.GetChild(_currentModelID).gameObject.SetActive(true);
        }
    }
}

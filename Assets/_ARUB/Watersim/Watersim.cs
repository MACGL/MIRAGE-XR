using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watersim : MonoBehaviour
{
    [SerializeField] private float fps = 60f;

    private YieldInstruction wait;
    // Start is called before the first frame update
    void Start()
    {
        Setup();
        StartCoroutine(Play());
    }

    IEnumerator Play()
    {
        int currentIndex = 0;
        int prevIndex = 0;
        transform.GetChild(currentIndex).gameObject.SetActive(true);
        
        while (true)
        {
            yield return wait;
            currentIndex++;
            if (currentIndex >= transform.childCount) currentIndex = 0;
            transform.GetChild(prevIndex).gameObject.SetActive(false);
            transform.GetChild(currentIndex).gameObject.SetActive(true);
            prevIndex = currentIndex;
        }
    }

    private void Setup()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive((false));
        }

        wait = new WaitForSeconds(1 / fps);
    }
}

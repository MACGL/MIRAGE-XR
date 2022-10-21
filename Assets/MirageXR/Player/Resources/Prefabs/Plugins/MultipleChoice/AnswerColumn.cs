using System;
using System.Collections;
using System.Collections.Generic;
using MirageXR;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class AnswerColumn : MonoBehaviour
{
    [SerializeField] private InputField inputField;
    [SerializeField] private Button button;
    [SerializeField] private bool correct;

    public void ButtonPressed()
    {
        Debug.Log("Button pressed");
        if (correct) GetComponent<Image>().color = Color.green;
        else GetComponent<Image>().color = Color.red;
    }
}

using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class Tour : MonoBehaviour
{
    private static Tour _singleton;
    public static Tour Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else
            {
                Debug.Log($"{nameof(Tour)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    private ModelData currentModel;

    [SerializeField] private TourData tourData;

    public Transform contentContainer;

    private bool calibrating;

    [SerializeField] private TMP_Dropdown modelsDropdown;
    [SerializeField] private GameObject editPanel;


    [SerializeField] private Button calibrateButton;
    [SerializeField] private Button upButton;
    [SerializeField] private Button downButton;
    [SerializeField] private TMP_Dropdown floorsDropdown;

    [SerializeField] private TMP_InputField inputName;
    [SerializeField] private Button inputModel;
    [SerializeField] private TMP_InputField inputFloors;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        tourData = DataHandler.GetData<TourData>();

        modelsDropdown.ClearOptions();
        modelsDropdown.AddOptions(tourData.models.Select(o => o.name).ToList());
        modelsDropdown.onValueChanged.AddListener(ModelChanged);

        calibrateButton.onClick.AddListener(PressedCalibrateHeight);
        upButton.onClick.AddListener(PressedUp);
        downButton.onClick.AddListener(PressedDown);
        floorsDropdown.onValueChanged.AddListener(FloorChanged);

        if (tourData.models.Count > 0)
        {
            currentModel = tourData.models[0];
            SetFloors();
            SetInputfields();
        }

        floorsDropdown.onValueChanged.Invoke(0);
    }

    private void Update()
    {
        if(calibrating)
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                Camera.main.transform.position += Vector3.up * Input.mouseScrollDelta.y;
            }
        }
    }

    #region BUTTON INPUTS

    public void PressedUp()
    {
        Debug.Log($"[Nick] {floorsDropdown.value}");
        if(floorsDropdown.value < currentModel.floorHeights.Count - 1)
            floorsDropdown.value++;
    }

    public void PressedDown()
    {
        Debug.Log($"[Nick] {floorsDropdown.value}");
        if (floorsDropdown.value > 0)
            floorsDropdown.value--;
    }

    public void PressedCalibrateHeight()
    {
        if (calibrating)
        {
            upButton.interactable = true;
            downButton.interactable = true;
            floorsDropdown.interactable = true;

            calibrateButton.GetComponentInChildren<TMP_Text>().text = "Calibrate";
            tourData.models[modelsDropdown.value].floorHeights[floorsDropdown.value] = Camera.main.transform.position.y;
            DataHandler.SetData(tourData);
            calibrating = false;
        }
        else
        {
            upButton.interactable = false;
            downButton.interactable = false;
            floorsDropdown.interactable = false;

            calibrateButton.GetComponentInChildren<TMP_Text>().text = "Save";
            calibrating = true;
        }
    }

    public void PressedEdit()
    {
        editPanel.SetActive(true);
    }

    public void AddModel()
    {
        ModelData newEntry = new ModelData($"Model {tourData.models.Count}");

        tourData.models.Add(newEntry);
        modelsDropdown.AddOptions(new List<string> { newEntry.name });
        modelsDropdown.value = tourData.models.Count - 1;
        DataHandler.SetData(tourData);
    }

    public void SelectModel()
    {
        //FileBrowser.ShowSaveDialog((p) => { Debug.Log($"[Nick] {p}"); }, () => { Debug.Log($"[Nick] cancel"); }, FileBrowser.PickMode.Files); 
    }

    public void Save()
    {
        ModelDataChanged(inputName.text, int.Parse(inputFloors.text));
        editPanel.SetActive(false);
    }

    #endregion

    private void SetFloors(bool edited = false)
    {
        string[] floorDescriptions = new string[currentModel.floorHeights.Count];
        if (edited)
        {
            int parsedInt;
            if(int.TryParse(inputFloors.text, out parsedInt))
                floorDescriptions = new string[parsedInt];
        }

        for (int i = 0; i < currentModel.floorHeights.Count; i++)
        {
            if (i == 0)
                floorDescriptions[i] = $"Erdgeschoss";
            else if (i == currentModel.floorHeights.Count - 1)
                floorDescriptions[i] = $"Dachgeschoss";
            else
                floorDescriptions[i] = $"{i}. Etage";
        }
        floorsDropdown.ClearOptions();
        floorsDropdown.AddOptions(floorDescriptions.ToList());
    }

    private void FloorChanged(int value)
    {
        Vector3 currentPos = Camera.main.transform.position;
        Camera.main.transform.position = new Vector3(currentPos.x, currentModel.floorHeights[value], currentPos.z);
    }

    private void SetInputfields()
    {
        inputName.text = currentModel.name;
        inputFloors.text = currentModel.floorHeights.Count.ToString();
    }

    private void ModelChanged(int value)
    {
        Debug.Log($"[Nick] Model changed: {tourData.models[value].name}");
        for(int i = contentContainer.childCount - 1; i >= 0; i--)
            Destroy(contentContainer.GetChild(i).gameObject);

        currentModel = tourData.models[value];

        if(currentModel.model != null)
            Instantiate(currentModel.model, contentContainer);
        SetFloors();    
        SetInputfields();
    }

    public void ModelDataChanged(string name, int floors)
    {
        currentModel.name = name;
        if (floors < currentModel.floorHeights.Count)
            currentModel.floorHeights = currentModel.floorHeights.Take(floors).ToList();
        else if(floors > currentModel.floorHeights.Count)
            for (int i = 0; i <= floors - currentModel.floorHeights.Count; i++)
                currentModel.floorHeights.Add(0);

        tourData.models[modelsDropdown.value] = currentModel;
        SetFloors();
        DataHandler.SetData(tourData);
    }
}
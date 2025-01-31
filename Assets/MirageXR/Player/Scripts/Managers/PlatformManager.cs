﻿ using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

namespace MirageXR
{
    public class PlatformManager : MonoBehaviour
    {
        [Serializable]
        public class LoadObject
        {
            public GameObject prefab;
            public Transform pathToLoad;
        }
        
        [Tooltip("If you want to test AR in the editor enable this.")]
        [SerializeField] bool forceWorldSpaceUi = false;
        [SerializeField] bool forceToTabletView = false;
        [SerializeField] private LoadObject[] _worldSpaceObjects;
        [SerializeField] private LoadObject[] _screenSpaceObjects;
        
        private float distanceToCamera = 0.5f;
        private float offsetYFromCamera = 0.5f;
        
        private bool _worldSpaceUi;
        private string _playerScene = "Player";
        private string _recorderScene = "recorder";
        private string _commonScene = "common";
        private string _activitySelectionScene = "ActivitySelection";
        private Camera _mainCamera;

        public static PlatformManager Instance { get; private set; }

        public bool WorldSpaceUi => _worldSpaceUi;

        public string PlayerSceneName => _playerScene;
        public string RecorderSceneName => _recorderScene;

        public string CommonSceneName => _commonScene;

        public string ActivitySelectionScene => _activitySelectionScene;

        private void Awake()
        {
            //Singleton
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("Platform: " + Application.platform);

            if (Application.platform == RuntimePlatform.WSAPlayerX86 || Application.platform == RuntimePlatform.WSAPlayerARM)
            {
                foreach (var arcm in Resources.FindObjectsOfTypeAll<ARCameraManager>()) Destroy(arcm);      //TODO: remove Resources.FindObjectsOfTypeAll
                foreach (var arm in Resources.FindObjectsOfTypeAll<ARManager>()) Destroy(arm);
                foreach (var ars in Resources.FindObjectsOfTypeAll<ARSession>()) Destroy(ars);
            }
        }

        public Vector3 GetTaskStationPosition()
        {
            return _mainCamera.transform.TransformPoint(Vector3.forward);
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            _mainCamera = Camera.main;

#if UNITY_ANDROID || UNITY_IOS
            _worldSpaceUi = forceWorldSpaceUi;
#else
            _worldSpaceUi = true;
#endif
            if (_worldSpaceUi)
            {
                if (_worldSpaceObjects != null)
                {
                    foreach (var worldSpaceObject in _worldSpaceObjects)
                    {
                        InstantiateObject(worldSpaceObject);
                    }
                }
            }
            else
            {
                if (_screenSpaceObjects != null)
                {
                    foreach (var screenSpaceObject in _screenSpaceObjects)
                    {
                        InstantiateObject(screenSpaceObject);
                    }
                }
            }
        }

        private static void InstantiateObject(LoadObject loadObject)
        {
            if (loadObject.pathToLoad)
            {
                Instantiate(loadObject.prefab, loadObject.pathToLoad);
            }
            else
            {
                Instantiate(loadObject.prefab);
            }
        }
        
        public enum DeviceFormat
        {
            Phone,
            Tablet,
            Unknown
        }

        public static DeviceFormat GetDeviceFormat()
        {
            if (Instance != null && Instance.forceToTabletView) return DeviceFormat.Tablet;
#if UNITY_IOS && !UNITY_EDITOR
            return UnityEngine.iOS.Device.generation.ToString().Contains("iPad") ? DeviceFormat.Tablet : DeviceFormat.Phone;
#elif UNITY_ANDROID && !UNITY_EDITOR
            const float minTabletSize = 6.5f;
            return GetDeviceDiagonalSizeInInches() > minTabletSize ? DeviceFormat.Tablet : DeviceFormat.Phone;
#elif UNITY_WSA && !UNITY_EDITOR
            return DeviceFormat.Unknown;
#else
            return Screen.width > Screen.height ? DeviceFormat.Tablet : DeviceFormat.Unknown;
#endif
        }

        private static float GetDeviceDiagonalSizeInInches()
        {
            var screenWidth = Screen.width / Screen.dpi;
            var screenHeight = Screen.height / Screen.dpi;
            var diagonalInches = Mathf.Sqrt (Mathf.Pow (screenWidth, 2) + Mathf.Pow (screenHeight, 2));
 
            Debug.Log ("Getting device inches: " + diagonalInches);
 
            return diagonalInches;
        }
    }
}

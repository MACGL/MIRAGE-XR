﻿using System.Collections;
using UnityEngine;


namespace MirageXR
{
    public class GazeCalibration : MonoBehaviour
    {
        [SerializeField] private GameObject loading;
        [SerializeField] private CalibrationTool calibrationTool;
        [SerializeField] private GameObject calibrationModel;
        [SerializeField] private GameObject lookHereText;
        [SerializeField] private GameObject calibratingText;

        private Camera cam;
        private Animator loadingAnimator;

        private void OnEnable()
        {
            cam = Camera.main;
            loadingAnimator = loading.GetComponent<Animator>();

            DisableCalibration();
        }



        void DisableCalibration()
        {
            //reset the animation to the first frame
            if (loadingAnimator.GetBool("Load"))
                loadingAnimator.Play("loading", 0, 0);

            lookHereText.SetActive(true);
            calibratingText.SetActive(false);
            loadingAnimator.SetBool("Load", false);
            loading.SetActive(false);
        }


        // Update is called once per frame
        void FixedUpdate()
        {

            //if you remove !UiManager.Instance you will get an error on editor
            if (!UiManager.Instance || UiManager.Instance.IsCalibrated)
            {
                if (calibrationModel.activeInHierarchy)
                    calibrationModel.SetActive(false);
                return;
            }

            DetectCalibration();

        }


         void  DetectCalibration()
        {
            RaycastHit hit;

            if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject == calibrationModel)
                    StartCoroutine(PlayCalibration());
                else
                    DisableCalibration();
            }
        }



        IEnumerator PlayCalibration()
        {
            calibrationModel.SetActive(true);

            yield return new WaitForSeconds(1);

            loading.SetActive(true);
            lookHereText.SetActive(false);
            calibratingText.SetActive(true);
            loadingAnimator.SetBool("Load", true);

        }

    }


}

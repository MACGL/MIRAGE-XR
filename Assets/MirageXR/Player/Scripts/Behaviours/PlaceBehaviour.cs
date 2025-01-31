﻿using UnityEngine;

namespace MirageXR
{
    public class PlaceBehaviour : MonoBehaviour
    {
        public Place Place { get; set; }

        private void OnEnable()
        {
            EventManager.OnClearAll += Delete;
        }

        private void OnDisable()
        {
            EventManager.OnClearAll -= Delete;
        }

        private void Delete()
        {
            Destroy(gameObject);
        }
    }
}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AISandbox
{
    [RequireComponent(typeof(IBoid))]
    public class Player : MonoBehaviour
    {
        #region Variables
        private IBoid _boid;
        #endregion

        #region Getters_Setters

        #endregion

        #region Unity
        private void Awake()
        {
            _boid = GetComponent<IBoid>();
        }

        private void Update()
        {
            Vector2 steering = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            _boid.Steering = steering;
        }
        #endregion

        #region Custom

        #endregion
    }
}
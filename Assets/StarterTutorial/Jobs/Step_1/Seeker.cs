using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial.Jobs_Step1
{
    public class Seeker : MonoBehaviour
    {
        public Vector3 Direction { get; set; }

        public void Update()
        {
            transform.localPosition += Direction * Time.deltaTime;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ECSStudy.StarterTutorial.Jobs_Step1
{
    public class Target : MonoBehaviour
    {
        public Vector3 Direction { get; set; }

        public void Update()
        {
            transform.localPosition += Direction * Time.deltaTime;
        }
    }
}
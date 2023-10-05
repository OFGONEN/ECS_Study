using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ECSStudy.StarterTutorial.Jobs_Step1
{
    public class FindNearest : MonoBehaviour
    {
        private void Update()
        {
            Vector3 nearestTargetPosition = default;
            float nearestDistSq = float.MaxValue;
            foreach (var targetTransform in Spawner.TargetTransforms)
            {
                Vector3 offset = targetTransform.localPosition - transform.localPosition;
                float distSq = offset.sqrMagnitude;
                if (distSq < nearestDistSq)
                {
                    nearestDistSq = distSq;
                    nearestTargetPosition = targetTransform.localPosition;
                }
            }

            Debug.DrawLine(transform.localPosition, nearestTargetPosition);
        }
    }
}
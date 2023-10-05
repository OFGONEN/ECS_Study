using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace ECSStudy.StarterTutorial.Jobs_Step2
{
    public class FindNearest : MonoBehaviour
    {
        private NativeArray<float3> TargetPositions;
        private NativeArray<float3> SeekerPositions;
        private NativeArray<float3> NearestTargetPositions;

        private void Start()
        {
            Spawner spawner = FindObjectOfType<Spawner>();

            TargetPositions = new NativeArray<float3>(spawner.NumTargets, Allocator.Persistent);
            SeekerPositions = new NativeArray<float3>(spawner.NumSeekers, Allocator.Persistent);
            NearestTargetPositions = new NativeArray<float3>(spawner.NumSeekers, Allocator.Persistent);
        }

        private void OnDestroy()
        {
            TargetPositions.Dispose();
            SeekerPositions.Dispose();
            NearestTargetPositions.Dispose();
        }

        private void Update()
        {
            for (int i = 0; i < TargetPositions.Length; i++)
                TargetPositions[i] = Spawner.TargetTransforms[i].position;
            
            for (int i = 0; i < SeekerPositions.Length; i++)
                SeekerPositions[i] = Spawner.SeekerTransforms[i].position;

            FindNearestJob findNearestJob = new FindNearestJob
            {
                TargetPositions = TargetPositions,
                SeekerPositions = SeekerPositions,
                NearestTargetPositions = NearestTargetPositions
            };

            JobHandle findHandle = findNearestJob.Schedule();
            findHandle.Complete(); // Complete the Job Now

            for (int i = 0; i < SeekerPositions.Length; i++)
                Debug.DrawLine(SeekerPositions[i], NearestTargetPositions[i]);
        }
    }
}
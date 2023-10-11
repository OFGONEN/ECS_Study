using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Tutorial.Jobs_Step2;

namespace Tutorial.Jobs_Step4
{
    public class FindNearest : MonoBehaviour
    {
        NativeArray<float3> TargetPositions;
        NativeArray<float3> SeekerPositions;
        NativeArray<float3> NearestTargetPositions;

        public void Start()
        {
            Jobs_Step2.Spawner spawner = GetComponent<Jobs_Step2.Spawner>();
            TargetPositions = new NativeArray<float3>(spawner.NumTargets, Allocator.Persistent);
            SeekerPositions = new NativeArray<float3>(spawner.NumSeekers, Allocator.Persistent);
            NearestTargetPositions = new NativeArray<float3>(spawner.NumSeekers, Allocator.Persistent);
        }

        public void OnDestroy()
        {
            TargetPositions.Dispose();
            SeekerPositions.Dispose();
            NearestTargetPositions.Dispose();
        }

        public void Update()
        {
            for (int i = 0; i < TargetPositions.Length; i++)
            {
                TargetPositions[i] = Jobs_Step2.Spawner.TargetTransforms[i].localPosition;
            }

            for (int i = 0; i < SeekerPositions.Length; i++)
            {
                SeekerPositions[i] = Jobs_Step2.Spawner.SeekerTransforms[i].localPosition;
            }

            SortJob<float3, AxisXComparer> sortJob = TargetPositions.SortJob(new AxisXComparer { });

            FindNearestJob findJob = new FindNearestJob
            {
                TargetPositions = TargetPositions,
                SeekerPositions = SeekerPositions,
                NearestTargetPositions = NearestTargetPositions,
            };

            JobHandle sortHandle = sortJob.Schedule();
            JobHandle findHandle = findJob.Schedule(SeekerPositions.Length, 100, sortHandle);
            findHandle.Complete();

            for (int i = 0; i < SeekerPositions.Length; i++)
            {
                Debug.DrawLine(SeekerPositions[i], NearestTargetPositions[i]);
            }
        }
    }
}
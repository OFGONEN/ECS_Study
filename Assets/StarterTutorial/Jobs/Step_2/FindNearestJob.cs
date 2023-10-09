using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Tutorial.Jobs_Step2
{
    
    [BurstCompile]
    public struct FindNearestJob : IJob
    {
        [ReadOnly] public NativeArray<float3> TargetPositions;
        [ReadOnly] public NativeArray<float3> SeekerPositions;
        public NativeArray<float3> NearestTargetPositions;


        [BurstCompile]
        public void Execute()
        {
            for (int i = 0; i < SeekerPositions.Length; i++)
            {
                float3 seekerPosition = SeekerPositions[i];
                float nearestDistSq = float.MaxValue;
                for (int j = 0; j < TargetPositions.Length; j++)
                {
                    float3 targetPos = TargetPositions[j];
                    float distSq = math.distancesq(seekerPosition, targetPos);

                    if (distSq < nearestDistSq)
                    {
                        nearestDistSq = distSq;
                        NearestTargetPositions[i] = nearestDistSq;
                    }
                }
                
            }
        }
    }
}
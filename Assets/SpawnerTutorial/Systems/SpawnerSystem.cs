using System.ComponentModel;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Tutorial.Spawner
{
    [BurstCompile]
    public partial struct SpawnerSystem : ISystem
    {
        public void OnCreate(ref SystemState state) { }
        public void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb_parallel = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();


            var processSpawnerJobHandle = new ProcessSpawnerJob
            {
                ECB = ecb_parallel,
                ElapsedTime = SystemAPI.Time.ElapsedTime,
            }.ScheduleParallel(state.Dependency);

            state.Dependency = processSpawnerJobHandle;

            // foreach (var spawner in SystemAPI.Query<RefRW<Spawner>>())
            // {
            //     ProcessSpawner(ref state, spawner);
            // }
        }

        // private void ProcessSpawner(ref SystemState state, RefRW<Spawner> spawner)
        // {
        //     if (spawner.ValueRO.NextSpawnTime < SystemAPI.Time.ElapsedTime)
        //     {
        //         Entity newEntity = state.EntityManager.Instantiate(spawner.ValueRO.Prefab);
        //         
        //         state.EntityManager.SetComponentData(newEntity, LocalTransform.FromPosition(spawner.ValueRO.SpawnPosition));
        //
        //         spawner.ValueRW.NextSpawnTime = (float)SystemAPI.Time.ElapsedTime + spawner.ValueRO.SpawnRate;
        //     }
        // }
    }
    
    [BurstCompile]
    public partial struct ProcessSpawnerJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ECB;
        public double ElapsedTime;

        private void Execute([ChunkIndexInQuery] int chunkIndex, ref Spawner spawner)
        {
            if (spawner.NextSpawnTime < ElapsedTime)
            {
                Entity newEntity = ECB.Instantiate(chunkIndex, spawner.Prefab);
                ECB.SetComponent(chunkIndex, newEntity, LocalTransform.FromPosition(spawner.SpawnPosition));

                spawner.NextSpawnTime = (float)ElapsedTime + spawner.SpawnRate;
            }
        }
    }
}
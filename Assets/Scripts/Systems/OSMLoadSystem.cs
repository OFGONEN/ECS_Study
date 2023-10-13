using System.Net.Mail;
using System.Xml;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace Curvit
{
    [UpdateInGroup(typeof(InitializationSystemGroup), OrderFirst = true)]
    public partial class OSMLoadSystem : SystemBase
    {
        private NativeArray<CurvitEntityData> nodesEntityDataArray;
        protected override void OnCreate()
        {
            RequireForUpdate<OSMLoadPathData>();
        }

        protected override void OnUpdate()
        {
            var entity = SystemAPI.GetSingletonEntity<OSMLoadPathData>();
            var component = EntityManager.GetComponentData<OSMLoadPathData>(entity);
            
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(component.LoadPathData.ToString());
            
            EntityManager.DestroyEntity(entity);
            
            var entityCommandBuffer = SystemAPI.GetSingleton<EndInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged).AsParallelWriter();
            
            var xmlNodeOsm = xmlDocument.SelectSingleNode("osm");
            var xmlNodeListNode = xmlNodeOsm.SelectNodes("node");

            nodesEntityDataArray = new NativeArray<CurvitEntityData>(xmlNodeListNode.Count, Allocator.TempJob);

            for (int i = 0; i < xmlNodeListNode.Count; i++)
            {
                var node = xmlNodeListNode[i];
                var id = node.Attributes["id"].Value;
                float3 position = new float3();
                
                foreach (XmlNode tag in node.SelectNodes("tag"))
                {
                    switch (tag.Attributes["k"].Value)
                    {
                        case "ele":
                            position.y = float.Parse(tag.Attributes["v"].Value);
                            break;
                        case "local_x":
                            position.x = float.Parse(tag.Attributes["v"].Value);
                            break;
                        case "local_y":
                            position.z = float.Parse(tag.Attributes["v"].Value);
                            break;
                    }
                }

                nodesEntityDataArray[i] = new CurvitEntityData
                {
                    Id = int.Parse(node.Attributes["id"].Value),
                    Position = position
                };
            }

            var nodeConstructionJobHandle = new NodeConstructionJob
            {
                nodeDataArray = nodesEntityDataArray,
                ECB = entityCommandBuffer
            }.Schedule(nodesEntityDataArray.Length, nodesEntityDataArray.Length / 4, Dependency);

            Dependency = nodeConstructionJobHandle;
        }
    }

    [BurstCompile]
    public partial struct NodeConstructionJob : IJobParallelFor
    {
        [ReadOnly]
        [DeallocateOnJobCompletion]
        public NativeArray<CurvitEntityData> nodeDataArray;
        public EntityCommandBuffer.ParallelWriter ECB;
        
        [BurstCompile]
        public void Execute(int index)
        {
            var entity = ECB.CreateEntity(index);
            ECB.AddComponent(index,entity, nodeDataArray[index]);
        }
    }
}
using System.Xml;
using Unity.Entities;
using UnityEngine;

namespace Curvit
{
    [RequireMatchingQueriesForUpdate]
    public partial class OSMLoadSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity entity, in OSMLoadPathData osmLoadPathData) =>
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(osmLoadPathData.LoadPathData.ToString());
                
                EntityManager.DestroyEntity(entity);
            })
                .WithStructuralChanges()
                .WithoutBurst()
                .Run();
        }
    }
}
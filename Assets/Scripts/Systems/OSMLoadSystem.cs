using System.Xml;
using Unity.Entities;
using UnityEngine;

namespace Curvit
{
    public partial class OSMLoadSystem : SystemBase
    {
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
        }
    }
}
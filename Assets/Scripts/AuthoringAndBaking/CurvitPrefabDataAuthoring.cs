using System.Collections;
using System.Collections.Generic;
using Curvit;
using Unity.Entities;
using UnityEngine;

namespace Curvit
{
    public class CurvitPrefabDataAuthoring : MonoBehaviour
    {
        public GameObject nodePrefab;
        public GameObject wayPrefab;
        public GameObject laneletPrefab;

        class CurvitPrefabDataBaker : Baker<CurvitPrefabDataAuthoring>
        {
            public override void Bake(CurvitPrefabDataAuthoring authoring)
            {
                DependsOn(authoring.nodePrefab);
                DependsOn(authoring.wayPrefab);
                DependsOn(authoring.laneletPrefab);

                if (authoring.nodePrefab == null || authoring.wayPrefab == null || authoring.laneletPrefab == null)
                    return;

                var nodeEntity = GetEntity(authoring.nodePrefab);
                var wayEntity = GetEntity(authoring.wayPrefab);
                var laneletEntity = GetEntity(authoring.laneletPrefab);

                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new CurvitPrefabData
                {
                    nodeEntityPrefab = nodeEntity,
                    wayEntityPrefab = wayEntity,
                    laneletEntityPrefab = laneletEntity
                });
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Curvit;

public class LoaderInterface : MonoBehaviour
{
    public string XMLPath;
    
    [ContextMenu("Load XML")]
    public void LoadXML()
    {
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        var fixedString = new OSMLoadPathData
        {
            LoadPathData = XMLPath
        };

        var entity = entityManager.CreateEntity();
        entityManager.AddComponentData(entity, fixedString);
    }
}
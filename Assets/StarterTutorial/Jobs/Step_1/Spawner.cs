using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ECSStudy.StarterTutorial.Jobs_Step1
{
    public class Spawner : MonoBehaviour
    {
        public static Transform[] TargetTransforms;

        public GameObject SeekerPrefab;
        public GameObject TargetPrefab;
        public int NumberOfSeekers;
        public int NumberOfTargets;
        public Vector2 Bounds;

        private void Start()
        {
            Random.InitState(123);

            for (int i = 0; i < NumberOfSeekers; i++)
            {
                GameObject go = GameObject.Instantiate(SeekerPrefab);
                Seeker seeker = go.GetComponent<Seeker>();
                Vector2 dir = Random.insideUnitCircle;
                seeker.Direction = new Vector3(dir.x, 0, dir.y);
                go.transform.localPosition = new Vector3(
                    Random.Range(0, Bounds.x), 0, Random.Range(0, Bounds.y));
            }

            TargetTransforms = new Transform[NumberOfTargets];
            for (int i = 0; i < NumberOfTargets; i++)
            {
                GameObject go = GameObject.Instantiate(TargetPrefab);
                Target target = go.GetComponent<Target>();
                Vector2 dir = Random.insideUnitCircle;
                target.Direction = new Vector3(dir.x, 0, dir.y);
                TargetTransforms[i] = go.transform;
                go.transform.localPosition = new Vector3(
                    Random.Range(0, Bounds.x), 0, Random.Range(0, Bounds.y));
            }
        }
    }
}
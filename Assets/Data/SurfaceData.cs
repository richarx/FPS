using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Data
{
    [CreateAssetMenu(fileName = "SurfaceData", menuName = "ScriptableObjects/SurfaceData")]
    public class SurfaceData : ScriptableObject
    {
        public enum SurfaceType
        {
            Ground,
            Wall,
            Enemy
        }

        public List<GameObject> wallImpactPrefabs;
        public List<GameObject> enemyImpactPrefabs;

        public GameObject GetRandomImpactFromSurface(SurfaceType type)
        {
            List<GameObject> list;

            switch (type)
            {
                case SurfaceType.Ground:
                case SurfaceType.Wall:
                    list = wallImpactPrefabs;
                    break;
                case SurfaceType.Enemy:
                    list = enemyImpactPrefabs;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            int index = Random.Range(0, list.Count);
            return list[index];
        }
    }
}

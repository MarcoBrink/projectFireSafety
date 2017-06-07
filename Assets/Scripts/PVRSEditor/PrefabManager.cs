using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public static class PrefabManager
    {
        public static GameObject[] Prefabs { get; private set; }

        static PrefabManager()
        {
            Prefabs = Resources.LoadAll<GameObject>("Prefabs/");
        }

        public static GameObject GetPrefab(string prefabName)
        {
            GameObject foundPrefab = null;

            foreach (GameObject prefab in Prefabs)
            {
                if (prefab.name == prefabName)
                {
                    foundPrefab = prefab;
                    break;
                }
            }

            if (foundPrefab == null)
            {
                Debug.Log("Prefab not found.");
            }

            return foundPrefab;
        }
    }
}

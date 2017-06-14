using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public static class PrefabManager
    {
        public struct Prefab
        {
            public string Name;
            public Sprite Thumbnail;
            public GameObject Item;
        }

        public static Prefab[] Prefabs { get; private set; }

        static PrefabManager()
        {
            Prefabs = LoadPrefabs();
        }

        public static GameObject GetPrefab(string prefabName)
        {
            GameObject foundPrefab = null;

            foreach (Prefab prefab in Prefabs)
            {
                if (prefab.Name == prefabName)
                {
                    foundPrefab = prefab.Item;
                    break;
                }
            }

            if (foundPrefab == null)
            {
                Debug.Log("Prefab not found.");
            }

            return foundPrefab;
        }

        private static Prefab GetPrefab(GameObject item)
        {
            Prefab prefab;
            prefab.Name = item.name;
            prefab.Item = item;
            Texture2D thumb = Resources.Load<Texture2D>("Thumbnails/" + item.name);
            prefab.Thumbnail = Sprite.Create(thumb, new Rect(0, 0, 512, 512), Vector2.zero);
            return prefab;
        }

        private static Prefab[] LoadPrefabs()
        {
            List<Prefab> prefabs = new List<Prefab>();
            GameObject[] gameObjects = Resources.LoadAll<GameObject>("Prefabs/");

            foreach (GameObject item in gameObjects)
            {
                Prefab prefab = GetPrefab(item);
                prefabs.Add(prefab);
            }

            return prefabs.ToArray();
        }
    }
}

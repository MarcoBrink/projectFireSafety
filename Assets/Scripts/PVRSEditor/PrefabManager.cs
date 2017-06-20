using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public static class PrefabManager
    {
        /// <summary>
        /// A structure for a prefab object, contains a name, the thumbnail for references and
        /// the object itself for in-game use
        /// </summary>
        public struct Prefab
        {
            public string Name;
            public Sprite Thumbnail;
            public GameObject Item;
        }

        /// <summary>
        /// An array of prefabs that can currently be accessed
        /// </summary>
        public static Prefab[] Prefabs { get; private set; }

        /// <summary>
        /// Insert all prefabs retrieved bij LoadPrefabs() into the array
        /// </summary>
        static PrefabManager()
        {
            Prefabs = LoadPrefabs();
        }

        /// <summary>
        /// Get the item from a prefab by name
        /// </summary>
        /// <param name="prefabName">the name of the prefab you're looking for</param>
        /// <returns></returns>
        public static GameObject GetPrefab(string prefabName)
        {
            //These are not the Prefabs you're looking for
            GameObject foundPrefab = null;

            foreach (Prefab prefab in Prefabs)
            {
                if (prefab.Name == prefabName)
                {
                    //Unless we found the right one
                    foundPrefab = prefab.Item;
                    return foundPrefab;
                }
            }

            return foundPrefab;
        }

        /// <summary>
        /// Create and get a prefab on the spot
        /// </summary>
        /// <param name="item">The item you want to put into the prefab</param>
        /// <returns></returns>
        private static Prefab GetPrefab(GameObject item)
        {
            Prefab prefab;
            prefab.Name = item.name;
            prefab.Item = item;
            Texture2D thumb = Resources.Load<Texture2D>("Thumbnails/" + item.name);
            prefab.Thumbnail = Sprite.Create(thumb, new Rect(0, 0, 512, 512), Vector2.zero);
            return prefab;
        }

        /// <summary>
        /// Get all game objects from the prefab location, wrap it in a prefab and add it to the prefab list
        /// </summary>
        /// <returns>A list of prefabs created from all saved game objects</returns>
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

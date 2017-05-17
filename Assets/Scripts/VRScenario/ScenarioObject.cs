using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.VRScenario
{
    public class ScenarioObject
    {
        private GameObject _object;
        private Vector3 _position;
        private Quaternion _rotation;

        public Vector3 Position
        {
            get
            {
                if (this._object != null)
                {
                    this._position = this._object.transform.position;
                }

                return this._position;
            }
        }

        public Quaternion Rotation
        {
            get
            {
                if (this._object != null)
                {
                    this._rotation = this._object.transform.rotation;
                }

                return this._rotation;
            }
        }

        public int PrefabID { get; set; }

        public ScenarioObject(int prefabID, GameObject gameObject)
        {
            this.PrefabID = prefabID;
            this._object = gameObject;
        }

        public ScenarioObject()
        {

        }

        public ScenarioObjectData GetData()
        {
            return new ScenarioObjectData(this);
        }
    }

    [System.Serializable]
    public class ScenarioObjectData
    {
        

        public int PrefabID { get; private set; }

        public ScenarioObjectData(ScenarioObject scenarioObject)
        {
            this.PrefabID = scenarioObject.PrefabID;
        }

        public ScenarioObject GetScenarioObject()
        {
            return new ScenarioObject();
        }
    }

    [System.Serializable]
    public class Vector3Data
    {
        private float X;
        private float YPosition;
        private float ZPosition;
    }
}
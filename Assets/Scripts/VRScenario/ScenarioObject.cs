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

            set
            {
                if (this._object != null)
                {
                    this._object.transform.position = value;
                }

                this._position = value;
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

            set
            {
                if (this._object != null)
                {
                    this._object.transform.rotation = value;
                }

                this._rotation = value;
            }
        }

        public string PrefabName { get; set; }

        public ScenarioObject(string prefabName, GameObject gameObject)
        {
            this.PrefabName = prefabName;
            this._object = gameObject;
        }

        public ScenarioObject(string prefabName, Vector3 position, Quaternion rotation)
        {
            this._object = null;
            this._position = position;
            this._rotation = rotation;
            this.PrefabName = prefabName;
        }

        public ScenarioObjectData GetData()
        {
            return new ScenarioObjectData(this);
        }

        public void SetObjectReference(GameObject gameObject)
        {
            this._object = gameObject;
        }
    }

    [System.Serializable]
    public class ScenarioObjectData
    {

        private Vector3Data Position;
        private QuaternionData Rotation;
        private string PrefabName;

        public ScenarioObjectData(ScenarioObject scenarioObject)
        {
            this.Position = new Vector3Data(scenarioObject.Position);
            this.Rotation = new QuaternionData(scenarioObject.Rotation);
            this.PrefabName = scenarioObject.PrefabName;
        }

        public ScenarioObject GetScenarioObject()
        {
            Vector3 position = this.Position.GetVector3();
            Quaternion rotation = this.Rotation.GetQuaternion();
            ScenarioObject scenarioObject = new ScenarioObject(this.PrefabName, position, rotation);
            return scenarioObject;
        }
    }

    [System.Serializable]
    public class Vector3Data
    {
        private float X;
        private float Y;
        private float Z;

        public Vector3Data(Vector3 vector)
        {
            this.X = vector.x;
            this.Y = vector.y;
            this.Z = vector.z;
        }

        public Vector3 GetVector3()
        {
            return new Vector3(this.X, this.Y, this.Z);
        }
    }

    [System.Serializable]
    public class QuaternionData
    {
        private float W;
        private float X;
        private float Y;
        private float Z;

        public QuaternionData(Quaternion quaternion)
        {
            this.W = quaternion.w;
            this.X = quaternion.x;
            this.Y = quaternion.y;
            this.Z = quaternion.z;
        }

        public Quaternion GetQuaternion()
        {
            return new Quaternion(this.X, this.Y, this.Z, this.W);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.VRScenario
{
    public class ScenarioObject
    {
        private GameObject _object;

        public Vector3 Position
        {
            get
            {
                return _object.transform.position;
            }
        }

        public Quaternion Rotation
        {
            get
            {
                return _object.transform.rotation;
            }
        }

        public int PrefabID { get; set; }

        public ScenarioObject(int prefabID, GameObject gameObject)
        {
            this.PrefabID = prefabID;
            this._object = gameObject;
        }

        public ScenarioObjectData GetData()
        {
            return new ScenarioObjectData(this);
        }
    }

    [System.Serializable]
    public class ScenarioObjectData
    {
        private float XPosition;
        private float YPosition;
        private float ZPosition;

        public int PrefabID { get; private set; }

        public ScenarioObjectData(ScenarioObject scenarioObject)
        {
            this.XPosition = scenarioObject.Position.x;
            this.YPosition = scenarioObject.Position.y;
            this.ZPosition = scenarioObject.Position.z;

            this.PrefabID = scenarioObject.PrefabID;
        }

        public Vector3 GetPosition()
        {
            return new Vector3(this.XPosition, this.YPosition, this.ZPosition);
        }
    }
}
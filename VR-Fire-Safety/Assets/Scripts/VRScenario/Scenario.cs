using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.VRScenario
{
    public class Scenario
    {
        public List<GameObject> Prefabs { get; set; }

        public Vector2 Dimensions { get; set; }

        public List<ScenarioObject> Objects { get; private set; }

        public Scenario(Vector2 dimensions)
        {
            this.Dimensions = dimensions;
        }

        public Scenario(ScenarioData data)
        {

        }

        public void AddObject(int prefabID, Vector3 position)
        {
            Object.Instantiate(Prefabs[prefabID]);
        }
    }

    [System.Serializable]
    public class ScenarioData
    {
        private float XDimension { get; set; }

        private float YDimension { get; set; }

        private ScenarioObject[] Objects { get; set; }

        public ScenarioData(Scenario scenario)
        {
            this.XDimension = scenario.Dimensions.x;
            this.YDimension = scenario.Dimensions.y;

            this.Objects = scenario.Objects.ToArray();
        }

        public Vector2 GetDimensions()
        {
            return new Vector2(this.XDimension, this.YDimension);
        }
    }
}

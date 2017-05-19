using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.VRScenario
{
    public class Scenario
    {
        public Vector2 Dimensions { get; set; }
        public List<ScenarioObject> Objects { get; set; }

        public Scenario(Vector2 dimensions)
        {
            this.Dimensions = dimensions;
            this.Objects = new List<ScenarioObject>();
        }

        public Scenario(Vector2 dimensions, ScenarioObject[] objects)
        {
            this.Dimensions = dimensions;
            this.Objects = new List<ScenarioObject>();

            foreach (ScenarioObject scenarioObject in objects)
            {
                this.Objects.Add(scenarioObject);
            }
        }
    }

    [System.Serializable]
    public class ScenarioData
    {
        private Vector2Data Dimensions;
        private ScenarioObjectData[] Objects;

        public ScenarioData(Scenario scenario)
        {
            this.Dimensions = new Vector2Data(scenario.Dimensions);

            List<ScenarioObject> objects = scenario.Objects;
            int objectCount = objects.Count;
            this.Objects = new ScenarioObjectData[objectCount];

            for (int index = 0; index < objectCount; index++)
            {
                this.Objects[index] = new ScenarioObjectData(objects[index]);
            }
        }

        public Scenario GetScenario()
        {
            Vector2 dimensions = this.Dimensions.GetVector2();

            List<ScenarioObject> objects = new List<ScenarioObject>();

            foreach (ScenarioObjectData scenarioObject in this.Objects)
            {
                objects.Add(scenarioObject.GetScenarioObject());
            }

            Scenario scenario = new Scenario(dimensions, objects.ToArray());
            return scenario;
        }
    }

    [System.Serializable]
    public class Vector2Data
    {
        private float X;
        private float Y;

        public Vector2Data(Vector2 vector)
        {
            this.X = vector.x;
            this.Y = vector.y;
        }

        public Vector2 GetVector2()
        {
            return new Vector2(this.X, this.Y);
        }
    }

}

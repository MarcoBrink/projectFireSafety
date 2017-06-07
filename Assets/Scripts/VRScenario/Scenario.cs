using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.VRScenario
{
    /// <summary>
    /// The main class that stores all data about a scenario.
    /// </summary>
    public class Scenario
    {
        /// <summary>
        /// The name of the scenario, this is used to name the file and otherwise.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The x and y dimensions of the scenario.
        /// </summary>
        public Vector2 Dimensions { get; set; }

        /// <summary>
        /// The objects stored in the scenario.
        /// </summary>
        public List<ScenarioObject> Objects { get; set; }

        /// <summary>
        /// The constructor for a Scenario.
        /// </summary>
        /// <param name="name">The name of the new scenario.</param>
        /// <param name="dimensions">The dimensions of the new scenario.</param>
        public Scenario(string name, Vector2 dimensions)
        {
            this.Name = name;
            this.Dimensions = dimensions;
            this.Objects = new List<ScenarioObject>();
        }

        /// <summary>
        /// Converts data from a ScenarioObject to a scenario.
        /// </summary>
        /// <param name="name">The name of the scenario.</param>
        /// <param name="dimensions">The dimensions of the scenario.</param>
        /// <param name="objects">The objects in the scenario.</param>
        public Scenario(string name, Vector2 dimensions, ScenarioObject[] objects)
        {
            this.Name = name;
            this.Dimensions = dimensions;
            this.Objects = new List<ScenarioObject>();

            foreach (ScenarioObject scenarioObject in objects)
            {
                this.Objects.Add(scenarioObject);
            }
        }

        /// <summary>
        /// Add an object to the Scenario for keeping track and saving.
        /// </summary>
        /// <param name="gameObject">The object to add to the scenario.</param>
        public void AddObject(string prefabName, GameObject gameObject)
        {
            gameObject.tag = "Scenario Object";
            ScenarioObject newObject = new ScenarioObject(prefabName, gameObject);
            Objects.Add(newObject);
        }
    }

    /// <summary>
    /// Serializable scenario data.
    /// </summary>
    [System.Serializable]
    public class ScenarioData
    {
        /// <summary>
        /// The name of the scenario.
        /// </summary>
        private string Name;

        /// <summary>
        /// The dimensions of the scenario.
        /// </summary>
        private Vector2Data Dimensions;

        /// <summary>
        /// The objects in the scenario.
        /// </summary>
        private ScenarioObjectData[] Objects;

        /// <summary>
        /// The constructor for Scenario data for serialization.
        /// </summary>
        /// <param name="scenario">The scenario to convert.</param>
        public ScenarioData(Scenario scenario)
        {
            // Set the name and convert the dimensions to a serializable format.
            this.Name = scenario.Name;
            this.Dimensions = new Vector2Data(scenario.Dimensions);

            // Convert all objects to serializable data and save them in an array to save space.
            List<ScenarioObject> objects = scenario.Objects;
            int objectCount = objects.Count;
            this.Objects = new ScenarioObjectData[objectCount];

            for (int index = 0; index < objectCount; index++)
            {
                this.Objects[index] = new ScenarioObjectData(objects[index]);
            }
        }

        /// <summary>
        /// Convert this data to a Scenario.
        /// </summary>
        /// <returns>The corresponding Scenario.</returns>
        public Scenario GetScenario()
        {
            Scenario scenario = null;

            // Only return the scenario if the data is valid.
            if (IsValid())
            {
                // Get a usable Vector2 for dimensions.
                Vector2 dimensions = this.Dimensions.GetVector2();

                // Create a new list for the scenario objects and fill it.
                List<ScenarioObject> objects = new List<ScenarioObject>();

                foreach (ScenarioObjectData scenarioObject in this.Objects)
                {
                    objects.Add(scenarioObject.GetScenarioObject());
                }

                // Create a scenario with the given data and return it.
                scenario = new Scenario(this.Name, dimensions, objects.ToArray());
            }
            
            return scenario;
        }

        /// <summary>
        /// Validate the ScenarioData.
        /// </summary>
        /// <returns>False if the ScenarioData is invalid.</returns>
        private bool IsValid()
        {
            bool valid = false;

            // The data is invalid if any of its contents is null.
            if (Name != null && Dimensions != null && Objects != null)
            {
                valid = true;
            }
            else if (Objects.Length > 0)
            {
                foreach (ScenarioObjectData item in Objects)
                {
                    if (item == null)
                    {
                        valid = false;
                        break;
                    }
                }
            }

            return valid;
        }
    }

    /// <summary>
    /// Serializable Vector2 data.
    /// </summary>
    [System.Serializable]
    public class Vector2Data
    {
        /// <summary>
        /// The x value.
        /// </summary>
        private float X;

        /// <summary>
        /// The y value.
        /// </summary>
        private float Y;

        /// <summary>
        /// Constructor for Vector2 data for serialization.
        /// </summary>
        /// <param name="vector">The vector to convert.</param>
        public Vector2Data(Vector2 vector)
        {
            this.X = vector.x;
            this.Y = vector.y;
        }

        /// <summary>
        /// Convert this data to a usable Vector2.
        /// </summary>
        /// <returns>The corresponding Vector2.</returns>
        public Vector2 GetVector2()
        {
            return new Vector2(this.X, this.Y);
        }
    }

}

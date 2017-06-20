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
        /// The environment the scenario exists in.
        /// </summary>
        public GameObject Environment { get; set; }

        /// <summary>
        /// The objects stored in the scenario.
        /// </summary>
        public List<ScenarioObject> Objects { get; set; }

        /// <summary>
        /// The constructor for a Scenario.
        /// </summary>
        /// <param name="name">The name of the new scenario.</param>
        /// <param name="dimensions">The dimensions of the new scenario.</param>
        public Scenario(string name, GameObject environment)
        {
            this.Name = name;
            this.Environment = environment;
            this.Objects = new List<ScenarioObject>();
        }

        /// <summary>
        /// Converts data from a ScenarioObject to a scenario.
        /// </summary>
        /// <param name="name">The name of the scenario.</param>
        /// <param name="dimensions">The dimensions of the scenario.</param>
        /// <param name="objects">The objects in the scenario.</param>
        public Scenario(string name, GameObject environment, ScenarioObject[] objects)
        {
            this.Name = name;
            this.Environment = environment;
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

        /// <summary>
        /// Remove and discard a given Scenario Object from the Scenario.
        /// </summary>
        /// <param name="scenarioObject"></param>
        public void RemoveObject(ScenarioObject scenarioObject)
        {
            // You can't remove null.
            if (scenarioObject != null)
            {
                if (Objects.Remove(scenarioObject))
                {
                    // Destroy and discard the object.
                    scenarioObject.Destroy();
                    scenarioObject = null;
                }
            }
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
        private string Environment;

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
            this.Environment = scenario.Environment.name;

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
                // Get a usable GameObject for the environment.
                GameObject environment = GetEnvironment();

                // Create a new list for the scenario objects and fill it.
                List<ScenarioObject> objects = new List<ScenarioObject>();

                foreach (ScenarioObjectData scenarioObject in this.Objects)
                {
                    objects.Add(scenarioObject.GetScenarioObject());
                }

                // Create a scenario with the given data and return it.
                scenario = new Scenario(this.Name, environment, objects.ToArray());
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
            if (Name != null && Environment != null && Objects != null)
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

        /// <summary>
        /// Get the correct environment from the resources folder.
        /// </summary>
        /// <returns>The environment as a game object.</returns>
        private GameObject GetEnvironment()
        {
            return Resources.Load<GameObject>("Omgevingen/" + this.Environment);
        }
    }
}

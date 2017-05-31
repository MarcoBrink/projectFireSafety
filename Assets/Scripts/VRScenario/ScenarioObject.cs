using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.VRScenario
{
    /// <summary>
    /// This class is used to hold data about objects in a scenario so they can be saved, loaded and manipulated.
    /// </summary>
    public class ScenarioObject
    {
        /// <summary>
        /// The hidden inner reference to the object.
        /// </summary>
        private GameObject _object;
        
        /// <summary>
        /// The hidden inner position.
        /// </summary>
        private Vector3 _position;

        /// <summary>
        /// The hidden inner rotation.
        /// </summary>
        private Quaternion _rotation;

        /// <summary>
        /// The position of the scenario object. Linked to its GameObject.
        /// </summary>
        public Vector3 Position
        {
            get
            {
                // If there is an object reference, the inner position is updated to its position.
                if (this._object != null)
                {
                    this._position = this._object.transform.position;
                }

                return this._position;
            }

            set
            {
                // If there is an object reference, its position is updated.
                if (this._object != null)
                {
                    this._object.transform.position = value;
                }

                this._position = value;
            }
        }

        /// <summary>
        /// The rotation of the scenario object.
        /// </summary>
        public Quaternion Rotation
        {
            get
            {
                // If there is an object reference, the inner rotation is updated to its rotation.
                if (this._object != null)
                {
                    this._rotation = this._object.transform.rotation;
                }

                return this._rotation;
            }

            set
            {
                // If there is an object reference, its rotation is updated.
                if (this._object != null)
                {
                    this._object.transform.rotation = value;
                }

                this._rotation = value;
            }
        }

        /// <summary>
        /// Check if the scenario object has a reference to a GameObject.
        /// </summary>
        public bool HasObjectReference
        {
            get
            {
                bool hasReference = false;

                // If the object isn't null, it has a reference.
                if (_object != null)
                {
                    hasReference = true;
                }

                return hasReference;
            }
        }

        /// <summary>
        /// The object referenced by the scenario object.
        /// </summary>
        public GameObject Object
        {
            get
            {
                // This will return null when there is no object reference.
                return this._object;
            }
            set
            {
                this._object = value;
            }
        }

        /// <summary>
        /// The name of the prefab used by this object.
        /// </summary>
        public string PrefabName { get; set; }

        /// <summary>
        /// The constructor for a scenario Object.
        /// </summary>
        /// <param name="prefabName">The prefab used by the scenario object.</param>
        /// <param name="gameObject">The object for this scenario object.</param>
        public ScenarioObject(string prefabName, GameObject gameObject)
        {
            this.PrefabName = prefabName;
            this._object = gameObject;
        }

        /// <summary>
        /// The constructor for a scenario object.
        /// </summary>
        /// <param name="prefabName">The name of the prefab used by the scenario object.</param>
        /// <param name="position">The position of the scenario object.</param>
        /// <param name="rotation">The rotation of the scenario object.</param>
        public ScenarioObject(string prefabName, Vector3 position, Quaternion rotation)
        {
            // No object reference is set for this constructor.
            this._object = null;

            // Instead, the position and rotation are set.
            this._position = position;
            this._rotation = rotation;

            this.PrefabName = prefabName;
        }

        /// <summary>
        /// Destroy the gameObject linked to this scenario object. After this the scenario object can be discarded.
        /// </summary>
        public void Destroy()
        {
            GameObject.Destroy(this._object);
        }
    }

    /// <summary>
    /// Serializable data for scenario objects.
    /// </summary>
    [System.Serializable]
    public class ScenarioObjectData
    {
        /// <summary>
        /// The position of the scenario object.
        /// </summary>
        private Vector3Data Position;

        /// <summary>
        /// The rotation of the scenario object.
        /// </summary>
        private QuaternionData Rotation;

        /// <summary>
        /// The name of the prefab used by the scenario object.
        /// </summary>
        private string PrefabName;

        /// <summary>
        /// The constructor for scenario object data.
        /// </summary>
        /// <param name="scenarioObject">The object to convert to data.</param>
        public ScenarioObjectData(ScenarioObject scenarioObject)
        {
            // Convert the scenario object's properties to serializable data.
            this.Position = new Vector3Data(scenarioObject.Position);
            this.Rotation = new QuaternionData(scenarioObject.Rotation);

            this.PrefabName = scenarioObject.PrefabName;
        }

        /// <summary>
        /// Convert this data to a scenario object.
        /// </summary>
        /// <returns>The corresponding scenario object.</returns>
        public ScenarioObject GetScenarioObject()
        {
            // Convert the data back to usable values.
            Vector3 position = this.Position.GetVector3();
            Quaternion rotation = this.Rotation.GetQuaternion();

            // Put these values in a new scenario object and return it.
            ScenarioObject scenarioObject = new ScenarioObject(this.PrefabName, position, rotation);
            return scenarioObject;
        }
    }

    /// <summary>
    /// Serializable Vector3 data.
    /// </summary>
    [System.Serializable]
    public class Vector3Data
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
        /// The z value.
        /// </summary>
        private float Z;

        /// <summary>
        /// The constructor for Vector3 data for serialization.
        /// </summary>
        /// <param name="vector">The Vector3 to convert to data.</param>
        public Vector3Data(Vector3 vector)
        {
            // Save the vector's data.
            this.X = vector.x;
            this.Y = vector.y;
            this.Z = vector.z;
        }

        /// <summary>
        /// Convert this data to a usable Vector3.
        /// </summary>
        /// <returns></returns>
        public Vector3 GetVector3()
        {
            // Add the saved values to a new Vector3.
            return new Vector3(this.X, this.Y, this.Z);
        }
    }

    /// <summary>
    /// Serializable Quaternion data.
    /// </summary>
    [System.Serializable]
    public class QuaternionData
    {
        /// <summary>
        /// The Quaternion's w value.
        /// </summary>
        private float W;

        /// <summary>
        /// The Quaternion's x value.
        /// </summary>
        private float X;

        /// <summary>
        /// The Quaternion's y value.
        /// </summary>
        private float Y;

        /// <summary>
        /// The Quaternion's z value.
        /// </summary>
        private float Z;

        /// <summary>
        /// The constructor for Quaternion data for serialization.
        /// </summary>
        /// <param name="quaternion">The Quaternion to convert to serializable data.</param>
        public QuaternionData(Quaternion quaternion)
        {
            // Save the Quaternion's data.
            this.W = quaternion.w;
            this.X = quaternion.x;
            this.Y = quaternion.y;
            this.Z = quaternion.z;
        }

        /// <summary>
        /// Convert this data to a usable Quaternion.
        /// </summary>
        /// <returns>The quaternion with this data.</returns>
        public Quaternion GetQuaternion()
        {
            return new Quaternion(this.X, this.Y, this.Z, this.W);
        }
    }
}
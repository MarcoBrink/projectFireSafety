using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using Assets.Scripts.VRScenario;

namespace Assets.Scripts.Saving
{
    /// <summary>
    /// This class is used for saving and loading scenario's.
    /// Needs some rewriting.
    /// </summary>
    public static class SaveLoad
    {
        /// <summary>
        /// The directory where scenarios are saved.
        /// </summary>
        private static string SaveDirectory = Application.persistentDataPath + "/Scenarios";

        /// <summary>
        /// The file type for scenarios. Currently allows lowercase and uppercase for the filetype.
        /// </summary>
        private static string[] ScenarioFileType = new string[]{"pvrs", "PVRS"};

        /// <summary>
        /// For saving and loading to work at all, the directory needs to exist.
        /// </summary>
        static SaveLoad()
        {
            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }
        }

        /// <summary>
        /// Get the names of all currently saved scenarios.
        /// </summary>
        /// <returns>An array of strings that describe the names of all scenarios.</returns>
        public static string[] GetSavedScenarios()
        {
            // Get the files first.
            string[] foundFiles = Directory.GetFiles(SaveDirectory);
            List<string> validFiles = new List<string>();

            // Check the files for validity. Needs more checks later, security checks for instance.
            foreach (string path in foundFiles)
            {
                if (Path.GetExtension(path) == ".pvrs")
                {
                    string filename = Path.GetFileNameWithoutExtension(path);
                    validFiles.Add(filename);
                }
            }

            return validFiles.ToArray();
        }

        /// <summary>
        /// Load a saved scenario by filename. Works because it allows only one directory and therefore no duplicate files.
        /// </summary>
        /// <param name="fileName">The name of the scenario to load.</param>
        /// <returns>A scenario object with all required data.</returns>
        public static Scenario LoadSavedScenario(string fileName)
        {
            Scenario scenario = null;

            // Original path for the filename.
            string path = GetFilePath(fileName);

            // Debug code to indicate that the file doesn't exist. Needs some real warning.
            if (!File.Exists(path))
            {
                Debug.Log("Scenario file doesn't exist;" + path);
            }
            else
            {
                FileStream stream = null;

                try
                {
                    // Open the file.
                    stream = File.Open(path, FileMode.Open);
                    BinaryFormatter formatter = new BinaryFormatter();

                    // Deserialize the data.
                    ScenarioData data = (ScenarioData)formatter.Deserialize(stream);

                    // Convert the data to a scenario object.
                    scenario = data.GetScenario();
                }
                catch (Exception ex)
                {
                    // Debug code for file loading, needs permanent solution.
                    Debug.Log(ex.Message);
                }
                finally
                {
                    // Stream needs to be closed and disposed to prevent memory leaks.
                    stream.Close();
                    stream.Dispose();
                }
            }

            return scenario;
        }

        /// <summary>
        /// Save a scenario with the given filename.
        /// </summary>
        /// <param name="filename">The name with which to save the scenario.</param>
        /// <param name="scenario">The scenario object to save.</param>
        public static void SaveScenario(string filename, Scenario scenario)
        {
            // The scenario object needs to be converted to serializable data first.
            ScenarioData data = new ScenarioData(scenario);

            FileStream stream = null;

            try
            {
                stream = File.Create(GetFilePath(filename));
                BinaryFormatter formatter = new BinaryFormatter();

                formatter.Serialize(stream, data);
            }
            catch (Exception ex)
            {
                // Debug code for saving errors, needs permanent solution.
                Debug.Log(ex.Message);
            }
            finally
            {
                // The stream needs to be closed and disposed to prevent memory leaks.
                stream.Close();
                stream.Dispose();
            }
        }

        /// <summary>
        /// Get the path of a scenario by filename.
        /// </summary>
        /// <param name="fileName">The name of the scenario.</param>
        /// <returns>The path on disk for the scenario.</returns>
        private static string GetFilePath(string fileName)
        {
            return SaveDirectory + "/" + fileName + ".pvrs";
        }
    }
}
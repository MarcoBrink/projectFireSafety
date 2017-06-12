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
        /// Test authorisation string.
        /// </summary>
        private static string Auth = "Provrex";

        /// <summary>
        /// The current scenario.
        /// </summary>
        public static Scenario CurrentScenario;

        /// <summary>
        /// The path of the current scenario.
        /// </summary>
        public static string CurrentPath;

        /// <summary>
        /// The directory where scenarios are saved.
        /// </summary>
        public static string SaveDirectory = Application.persistentDataPath + "/Scenarios";

        /// <summary>
        /// The file type for scenarios. Currently allows lowercase and uppercase for the filetype.
        /// </summary>
        private static string ScenarioFileType = ".pvrs";

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
        /// <param name="dir">The directory to search.</param>
        /// <returns>An array of strings that describe the names of all scenarios.</returns>
        public static string[] GetSavedScenarios(string dir)
        {
            // Get the files first.
            string[] foundFiles = Directory.GetFiles(dir);
            List<string> validFiles = new List<string>();

            // Check the files for validity. Needs more checks later, security checks for instance.
            foreach (string path in foundFiles)
            {
                if (Path.GetExtension(path) == ScenarioFileType)
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
        public static bool LoadSavedScenario(string filePath, out Scenario scenario)
        {
            bool valid = false;
            scenario = null;

            if (File.Exists(filePath))
            {
                if (Path.GetExtension(filePath) == ScenarioFileType)
                {
                    FileStream stream = null;

                    try
                    {
                        // Open the file.
                        stream = File.Open(filePath, FileMode.Open);
                        BinaryFormatter formatter = new BinaryFormatter();

                        // Deserialize the data.
                        PVRS data = (PVRS)formatter.Deserialize(stream);

                        // Convert the data to a scenario object.
                        scenario = data.GetScenario(Auth);

                        valid = true;
                        CurrentPath = filePath;
                    }
                    catch (Exception ex)
                    {
                        // Debug code for file loading, needs permanent solution.
                        Debug.Log(ex.Message);

                        valid = false;
                    }
                    finally
                    {
                        // Stream needs to be closed and disposed to prevent memory leaks.
                        stream.Close();
                        stream.Dispose();
                    }
                }
            }

            if (!valid)
            {
                scenario = null;
            }

            return valid;
        }

        /// <summary>
        /// Save a scenario with the given filename.
        /// </summary>
        /// <param name="filename">The name with which to save the scenario.</param>
        /// <param name="scenario">The scenario object to save.</param>
        public static bool SaveScenario(Scenario scenario, string Path)
        {
            bool successful = false;

            // The scenario object needs to be converted to serializable data first.
            PVRS data = new PVRS(Auth, scenario);

            FileStream stream = null;

            try
            {
                stream = File.Create(Path);
                BinaryFormatter formatter = new BinaryFormatter();

                formatter.Serialize(stream, data);
                CurrentPath = Path;
                successful = true;
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

            return successful;
        }

        /// <summary>
        /// Get the path of a scenario by filename.
        /// </summary>
        /// <param name="fileName">The name of the scenario.</param>
        /// <returns>The path on disk for the scenario.</returns>
        public static string GetFilePath(string fileName)
        {
            return SaveDirectory + "/" + fileName + ScenarioFileType;
        }
    }
}
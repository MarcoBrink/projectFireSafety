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

            CurrentPath = SaveDirectory + "/Default";
        }

        /// <summary>
        /// Get all saved scenarios in a given directory.
        /// </summary>
        /// <param name="dir">The directory to search.</param>
        /// <param name="files">The array to copy the files to.</param>
        /// <returns>A bool indicating the success of the operation.</returns>
        public static bool GetSavedScenarios(string dir, out string[] files)
        {
            bool foundAny = false;
            files = null;

            string[] foundFiles;

            // Get the files first.
            try
            {
                foundFiles = Directory.GetFiles(dir);
            }
            catch (Exception)
            {
                // The search was a failure because either the directory doesn't exist or some other error occurred.
                foundFiles = null;
                foundAny = false;
            }

            // Only continue if any files were found.
            if (foundFiles != null)
            {
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

                if (validFiles.Count != 0)
                {
                    files = validFiles.ToArray();
                    foundAny = true;
                }
            }
            return foundAny;
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

            string extendedPath = filePath + ScenarioFileType;

            if (File.Exists(extendedPath))
            {
                FileStream stream = null;

                try
                {
                    // Open the file.
                    stream = File.Open(extendedPath, FileMode.Open);
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
        public static bool SaveScenario(Scenario scenario, string path)
        {
            bool successful = false;

            // The scenario object needs to be converted to serializable data first.
            PVRS data = new PVRS(Auth, scenario);

            FileStream stream = null;

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                stream = File.Create(path + ScenarioFileType);
                BinaryFormatter formatter = new BinaryFormatter();

                formatter.Serialize(stream, data);
                CurrentPath = path;
                successful = true;
            }
            catch (Exception ex)
            {
                // Debug code for saving errors, needs permanent solution.
                Debug.Log(ex.Message);
                successful = false;
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
        /// Check if a file already exists.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool FileExists(string path)
        {
            bool exists = false;

            if (File.Exists(path + ".pvrs"))
            {
                exists = true;
            }

            return exists;
        }

        /// <summary>
        /// Check if a folder already exists.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns></returns>
        public static bool FolderExists(string path)
        {
            bool exists = false;

            if (Directory.Exists(path))
            {
                exists = true;
            }

            return exists;
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

        /// <summary>
        /// Get the file name without the extension.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <returns>The file name without the extension.</returns>
        public static string GetFileName(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using Assets.Scripts.VRScenario;

namespace Assets.Scripts.Saving
{
    public static class SaveLoad
    {
        private static string SaveDirectory = Application.persistentDataPath + "/Scenarios";
        private static string[] ScenarioFileType = new string[]{"pvrs", "PVRS"};

        static SaveLoad()
        {
            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }
        }

        public static string[] GetSavedScenarios()
        {
            string[] foundFiles = Directory.GetFiles(SaveDirectory);
            List<string> validFiles = new List<string>();

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

        public static Scenario LoadSavedScenario(string fileName)
        {
            Scenario scenario = null;

            string path = GetFilePath(fileName);

            if (!File.Exists(path))
            {
                Debug.Log("Scenario file doesn't exist;" + path);
            }
            else
            {
                FileStream stream = null;

                try
                {
                    stream = File.Open(path, FileMode.Open);
                    BinaryFormatter formatter = new BinaryFormatter();

                    ScenarioData data = (ScenarioData)formatter.Deserialize(stream);
                    scenario = data.GetScenario();
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                }
                finally
                {
                    stream.Close();
                    stream.Dispose();
                }
            }

            return scenario;
        }

        public static void SaveScenario(string filename, Scenario scenario)
        {
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
                Debug.Log(ex.Message);
            }
            finally
            {
                stream.Close();
                stream.Dispose();
            }
        }

        private static string GetFilePath(string fileName)
        {
            return SaveDirectory + "/" + fileName + ".pvrs";
        }
    }
}
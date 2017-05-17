using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad
{
    //private static string ScenarioPath = Application.persistentDataPath + "/Scenarios";

    //static SaveLoad()
    //{
    //    if (!Directory.Exists(ScenarioPath))
    //    {
    //        Directory.CreateDirectory(ScenarioPath);
    //    }
    //}

    //public static List<Scenario> GetSavedScenarios()
    //{
    //    List<Scenario> savedScenarios = new List<Scenario>();

    //    string[] files = null;

    //    try
    //    {
    //        files = Directory.GetFiles(ScenarioPath);
    //    }
    //    catch (DirectoryNotFoundException)
    //    {
    //        Debug.Log("Failed to find directory.");
    //    }

    //    FileStream stream = null;

    //    foreach (string file in files)
    //    {
    //        try
    //        {
    //            stream = File.Open(file, FileMode.Open);
    //            BinaryFormatter binaryFormatter = new BinaryFormatter();

    //            Scenario scenario = (Scenario)binaryFormatter.Deserialize(stream);
    //            savedScenarios.Add(scenario);
    //        }
    //        catch (System.Exception)
    //        {
    //            Debug.Log("Deserialization Error.");
    //        }
    //        finally
    //        {
    //            if (stream != null)
    //            {
    //                stream.Close();
    //                stream.Dispose();
    //            }
    //        }
    //    }

    //    return savedScenarios;
    //}

    //public static void SaveScenario(string FileName, Scenario current)
    //{
    //    FileStream stream = null;

    //    try
    //    {
    //        BinaryFormatter binaryFormatter = new BinaryFormatter();
    //        stream = File.Create(ScenarioPath + "/" + FileName + ".PVRS");
    //        binaryFormatter.Serialize(stream, current);
    //    }
    //    catch (System.Exception)
    //    {
    //        Debug.Log("Saving Error.");
    //    }
    //    finally
    //    {
    //        stream.Close();
    //        stream.Dispose();
    //    }
    //}
}

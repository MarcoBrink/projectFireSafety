
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class GetSaves : MonoBehaviour {

    public static string[] GetSavedScenarios()
    {
       // string[] foundFiles = Directory.GetFiles(SaveDirectory);
        List<string> validFiles = new List<string>();

      //  foreach (string path in foundFiles)
        {
        //    if (Path.GetExtension(path) == ".pvrs")
            {
      //          string filename = Path.GetFileNameWithoutExtension(path);
        //        validFiles.Add(filename);
            }
        }

        return validFiles.ToArray();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class Test2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                List<List<List<object>>> data = new List<List<List<object>>>();

                CubeScript[] allCubes = FindObjectsOfType<CubeScript>();
                Component[] cubeComponents;
                PropertyInfo[] componentFields;
                for (int i = 0; i < allCubes.Length; i++)
                {
                    List<List<object>> componentData = new List<List<object>>();

                    cubeComponents = allCubes[i].gameObject.GetComponents<Component>();

                    for (int j = 0; j < cubeComponents.Length; j++)
                    {
                        List<object> fieldData = new List<object>();
                        
                        componentFields = cubeComponents[j].GetType().GetProperties();

                        for (int k = 0; k < componentFields.Length; k++)
                        {
                            object obj = cubeComponents[j];
                            fieldData.Add(componentFields[k].GetValue(obj, null));
                        }

                        componentData.Add(fieldData);
                    }

                    data.Add(componentData);
                }

                foreach (List<List<object>> meme in data)
                {
                    string debugString = "";
                    foreach (List<object> mem in meme)
                    {
                        foreach (object me in mem)
                        {
                            debugString += me.GetType().ToString();
                            debugString += me;
                            Debug.Log(debugString);
                        }
                    }
                }
            }   
        }
	}
}

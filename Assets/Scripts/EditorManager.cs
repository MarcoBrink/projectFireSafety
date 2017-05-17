using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.VRScenario;
using Assets.Scripts.Saving;

public class EditorManager : MonoBehaviour
{

    private Scenario CurrentScenario;
    private string LastSaved;
    public List<GameObject> prefabs;
    public string CurrentPrefabName = "TestCube";

    // Use this for initialization
    void Start () {
        Uri[] savedScenarios = SaveLoad.GetSavedScenarios();

        if (savedScenarios.Length == 0)
        {
            CurrentScenario = new Scenario(new Vector2(50, 50));
        }
        else
        {
            string fileName = SaveLoad.GetFileName(savedScenarios[0]);
            CurrentScenario = SaveLoad.LoadSavedScenario(fileName);
            LastSaved = fileName;
            Debug.Log(fileName);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                SaveLoad.SaveScenario("test", CurrentScenario);
                LastSaved = "test";
            }
            else if (Input.GetKeyDown(KeyCode.F5))
            {
                SaveLoad.SaveScenario(LastSaved, CurrentScenario);
            }
            else if (Input.GetKeyDown(KeyCode.F9))
            {
                SaveLoad.LoadSavedScenario(LastSaved);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Test");

            float dist = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            Vector3 MousePos = Input.mousePosition;

            Vector3 trans = new Vector3(MousePos.x, MousePos.y, dist);

            Vector3 pos = Camera.main.ScreenToWorldPoint(trans);
            Quaternion rot = new Quaternion();

            GameObject prefab = GetPrefab(CurrentPrefabName);

            GameObject newObject = Instantiate(prefab, pos, rot);

            ScenarioObject newScenarioObject = new ScenarioObject(CurrentPrefabName, newObject);

            CurrentScenario.Objects.Add(newScenarioObject);
        }
    }

    GameObject GetPrefab(string prefabName)
    {
        GameObject foundPrefab = null;

        foreach (GameObject prefab in prefabs)
        {
            if (prefab.name == prefabName)
            {
                foundPrefab = prefab;
                break;
            }
        }

        if (foundPrefab == null)
        {
            Debug.Log("Prefab not found.");
        }

        return foundPrefab;
    }

    void LoadScenario()
    {
        GameObject[] currentObjects = GameObject.FindGameObjectsWithTag("Scenario Object");

        foreach (GameObject currentObject in currentObjects)
        {
            Destroy(currentObject);
        }

        foreach (ScenarioObject scenarioObject in CurrentScenario.Objects)
        {
            if (!scenarioObject.HasObjectReference)
            {
                GameObject prefab = GetPrefab(scenarioObject.PrefabName);
                Vector3 position = scenarioObject.Position;
                Quaternion rotation = scenarioObject.Rotation;

                GameObject newObject = Instantiate(prefab, position, rotation);
                scenarioObject.SetObjectReference(newObject);
            }
        }
    }
}

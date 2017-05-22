using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.VRScenario;
using Assets.Scripts.Saving;
using Assets.Scripts;

public class EditorManager : MonoBehaviour
{

    private Scenario CurrentScenario;
    public List<GameObject> Prefabs;
    public EditorCursor CursorPrefab;
    private EditorCursor Cursor;

    // Use this for initialization
    void Start ()
    {
        Cursor = Instantiate(CursorPrefab);

        string[] savedScenarios = SaveLoad.GetSavedScenarios();

        if (savedScenarios.Length == 0)
        {
            CurrentScenario = new Scenario("Test", new Vector2(50, 50));
        }
        else
        {
            CurrentScenario = SaveLoad.LoadSavedScenario("test");
        }

        LoadScenario();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                SaveLoad.SaveScenario("test", CurrentScenario);
            }
            else if (Input.GetKeyDown(KeyCode.F5))
            {
                SaveLoad.SaveScenario(CurrentScenario.Name, CurrentScenario);
            }
            else if (Input.GetKeyDown(KeyCode.F9))
            {
                CurrentScenario = SaveLoad.LoadSavedScenario(CurrentScenario.Name);
                LoadScenario();
            }
        }
    }

    void LoadScenario()
    {
        GameObject[] currentObjects = GameObject.FindGameObjectsWithTag("Scenario Object");

        foreach (GameObject currentObject in currentObjects)
        {
            Destroy(currentObject);
        }

        GameObject floor = GameObject.FindGameObjectWithTag("Floor");

        foreach (ScenarioObject scenarioObject in CurrentScenario.Objects)
        {
            if (!scenarioObject.HasObjectReference)
            {
                GameObject prefab = PrefabManager.GetPrefab(scenarioObject.PrefabName);
                Vector3 position = scenarioObject.Position;
                Quaternion rotation = scenarioObject.Rotation;

                GameObject newObject = Instantiate(prefab, position, rotation);
                scenarioObject.Object = newObject;
            }
        }
    }
}

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
    private EditorCursor EditorCursor;
    private Dictionary<string, EditorMode> Modes;
    private EditorMode CurrentMode;

    // Use this for initialization
    void Start ()
    {
        EditorCursor = Instantiate(CursorPrefab);

        Modes = new Dictionary<string, EditorMode>();
        Modes.Add("Cursor", new EditorCursorMode(EditorCursor));
        Modes.Add("Move", new MoveMode(Camera.main));

        ChangeEditorMode("Cursor");

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
	void Update ()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            if (!(CurrentMode is MoveMode))
            {
                ChangeEditorMode("Cursor");
            }
        }
        else
        {
            if (!(CurrentMode is EditorCursorMode))
            {
                ChangeEditorMode("Move");
            }
        }

        CurrentMode.Update();
    }

    private void LoadScenario()
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

    private void ChangeEditorMode(string modeName)
    {
        if (CurrentMode != null)
        {
            CurrentMode.Disable();
        }

        CurrentMode = Modes[modeName];

        CurrentMode.Enable();
    }
}

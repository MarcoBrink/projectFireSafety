using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.VRScenario;
using Assets.Scripts.Saving;
using Assets.Scripts;
using Assets.Scripts.PVRSEditor;

/// <summary>
/// A manager class for the editor. This MonoBehaviour is responsible for running the editor functionality.
/// </summary>
public class EditorManager : MonoBehaviour
{
    // Assigned in inspector.
    private Canvas Objectmenu;

    /// <summary>
    /// The current scenario.
    /// </summary>
    public static Scenario CurrentScenario;

    /// <summary>
    /// The prefab used for the cursor, set in the Unity Editor.
    /// </summary>
    public EditorCursor CursorPrefab;

    /// <summary>
    /// A list of all existing editor modes.
    /// </summary>
    private Dictionary<string, IEditorMode> Modes;

    /// <summary>
    /// The current mode.
    /// </summary>
    private IEditorMode CurrentMode;

    private string SaveName = "newTest";

    /// <summary>
    /// This method initializes the editor.
    /// </summary>
    void Start ()
    {
        //The modes need to be made next, so they can be used by the editor.
        Modes = new Dictionary<string, IEditorMode>();
        IEditorMode cursorMode = new EditorCursorMode(CursorPrefab);
        Modes.Add(cursorMode.ToString(), cursorMode);
        IEditorMode movementMode = new MoveMode(Camera.main);
        Modes.Add(movementMode.ToString(), movementMode);
        IEditorMode selectionMode = new SelectionMode(CursorPrefab);
        Modes.Add(selectionMode.ToString(), selectionMode);

        // The initial mode; trivial and could (or even should) be changed to something dynamic later.
        ChangeEditorMode("Cursor");

        // Some loading test code, needs to be changed.
        #region TestLoading
        string[] savedScenarios = SaveLoad.GetSavedScenarios(SaveLoad.SaveDirectory);

        if (savedScenarios.Length == 0)
        {
            CurrentScenario = new Scenario("Test", new Vector2(50, 50));
        }
        else
        {
            CurrentScenario = SaveLoad.LoadSavedScenario(savedScenarios[0]);
        }
        #endregion

        // After the current scenario is loaded.
        LoadScenario(CurrentScenario);
    }

    /// <summary>
    /// GUI code for the Editor, currently used for debug.
    /// </summary>
    private void OnGUI()
    {
        GUI.BeginGroup(new Rect(25, 25, 200, 150));
        //SaveName = GUI.TextField(new Rect(5, 5, 190, 40), SaveName);
        if (GUI.Button(new Rect(5, 55, 190, 40), "Save"))
        {
            SaveLoad.SaveScenario(SaveName, CurrentScenario);
        }
        if (GUI.Button(new Rect(5, 110, 190, 40), "Load"))
        {
            Scenario loaded = SaveLoad.LoadSavedScenario(SaveName);
            LoadScenario(loaded);
        }
        GUI.EndGroup();

        CurrentMode.OnGUI();
    }

    /// <summary>
    /// The update method is called once per frame. It checks for general input and calls the current mode's Update method.
    /// </summary>
    void Update ()
    {
        // Tab is currently a fixed key for switching modes, should be changed later.
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (CurrentMode is MoveMode)
            {
                ChangeEditorMode("Selection");
            }
            else if (CurrentMode is EditorCursorMode)
            {
                ChangeEditorMode("Move");
            }
            else if (CurrentMode is SelectionMode)
            {
                ChangeEditorMode("Cursor");
            }
        }
        CurrentMode.Update();
    }

    /// <summary>
    /// This method is used to load a scenario from a file. Removes all current objects and spawns saved ones.
    /// </summary>
    private void LoadScenario(Scenario scenario)
    {
        // All scenario objects will have this tag, so they can be properly removed by the editor manager.
        GameObject[] currentObjects = GameObject.FindGameObjectsWithTag("Scenario Object");

        foreach (GameObject currentObject in currentObjects)
        {
            Destroy(currentObject);
        }

        // Temp code, does nothing.
        GameObject floor = GameObject.FindGameObjectWithTag("Floor");

        // All objects are spawned in a loop.
        foreach (ScenarioObject scenarioObject in scenario.Objects)
        {
            if (!scenarioObject.HasObjectReference)
            {
                // Get the right prefab by its name.
                GameObject prefab = PrefabManager.GetPrefab(scenarioObject.PrefabName);

                // Get the right position and rotation.
                Vector3 position = scenarioObject.Position;
                Quaternion rotation = scenarioObject.Rotation;

                //Instantiate an instance of the right prefab with these characteristics and hand a reference to the scenario object.
                GameObject newObject = Instantiate(prefab, position, rotation);
                newObject.tag = "Scenario Object";
                scenarioObject.Object = newObject;
            }
        }

        CurrentScenario = scenario;
    }

    /// <summary>
    /// Change the current mode to the given mode.
    /// </summary>
    /// <param name="modeName">The name of the mode to switch to. Should be that mode's ToString()</param>
    private void ChangeEditorMode(string modeName)
    {
        // The current mode needs to be disabled, if there is one.
        if (CurrentMode != null)
        {
            CurrentMode.Disable();
        }

        CurrentMode = Modes[modeName];

        // The new mode needs to be enabled on startup.
        CurrentMode.Enable();
    }

    

    public void ToggleObject()
    {
        Objectmenu = GetComponent<Canvas>();
        Objectmenu.enabled = !Objectmenu.enabled;
    }
}

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.VRScenario;
using Assets.Scripts.Saving;
using Assets.Scripts;

/// <summary>
/// A manager class for the editor. This MonoBehaviour is responsible for running the editor functionality.
/// </summary>
public class EditorManager : MonoBehaviour
{
    /// <summary>
    /// The current scenario.
    /// </summary>
    private Scenario CurrentScenario;

    /// <summary>
    /// The prefab used for the cursor, set in the Unity Editor.
    /// </summary>
    public EditorCursor CursorPrefab;

    /// <summary>
    /// The cursor used by the editor.
    /// </summary>
    private EditorCursor EditorCursor;

    /// <summary>
    /// A list of all existing editor modes.
    /// </summary>
    private Dictionary<string, IEditorMode> Modes;

    /// <summary>
    /// The current mode.
    /// </summary>
    private IEditorMode CurrentMode;

    /// <summary>
    /// This method initializes the editor.
    /// </summary>
    void Start ()
    {
        // The cursor needs to be instantiated first, because it is needed for a mode.
        EditorCursor = Instantiate(CursorPrefab);

        //The modes need to be made next, so they can be used by the editor.
        Modes = new Dictionary<string, IEditorMode>();
        IEditorMode cursorMode = new EditorCursorMode(EditorCursor);
        Modes.Add(cursorMode.ToString(), cursorMode);
        IEditorMode movementMode = new MoveMode(Camera.main);
        Modes.Add(movementMode.ToString(), movementMode);

        // The initial mode; trivial and could (or even should) be changed to something dynamic later.
        ChangeEditorMode("Cursor");

        // Some loading test code, needs to be changed.
        #region TestLoading
        string[] savedScenarios = SaveLoad.GetSavedScenarios();

        if (savedScenarios.Length == 0)
        {
            CurrentScenario = new Scenario("Test", new Vector2(50, 50));
        }
        else
        {
            CurrentScenario = SaveLoad.LoadSavedScenario("test");
        }
        #endregion

        // After the current scenario is loaded.
        LoadScenario();
    }

    /// <summary>
    /// Some test GUI code, needs to be removed later.
    /// </summary>
    private void OnGUI()
    {
        // Make some buttons and a label pertaining to the current mode.
        GUI.Label(new Rect(10, 10, 150, 30), "Mode: " + CurrentMode.ToString());
        if (GUI.Button(new Rect(10, 40, 150, 30), "Cursor Mode"))
        {
            ChangeEditorMode("Cursor");
        }
        if (GUI.Button(new Rect(10, 75, 150, 30), "Movement Mode"))
        {
            ChangeEditorMode("Move");
        }
    }

    /// <summary>
    /// The update method is called once per frame. It checks for general input and calls the current mode's Update method.
    /// </summary>
    void Update ()
    {
        // Tab is currently a fixed key for switching modes, should be changed later.
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!(CurrentMode is MoveMode))
            {
                ChangeEditorMode("Move");
            }
            else if(!(CurrentMode is EditorCursorMode))
            {
                ChangeEditorMode("Cursor");
            }
        }
        CurrentMode.Update();
    }

    /// <summary>
    /// This method is used to load a scenario from a file. Removes all current objects and spawns saved ones.
    /// </summary>
    private void LoadScenario()
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
        foreach (ScenarioObject scenarioObject in CurrentScenario.Objects)
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
                scenarioObject.Object = newObject;
            }
        }
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
}

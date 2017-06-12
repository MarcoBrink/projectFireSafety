using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts.VRScenario;
using Assets.Scripts.Saving;
using Assets.Scripts;
using Assets.Scripts.PVRSEditor;

/// <summary>
/// A manager class for the editor. This MonoBehaviour is responsible for running the editor functionality.
/// </summary>
public class EditorManager : MonoBehaviour
{
    /// <summary>
    /// The current scenario. Equal to the scenario stored by SaveLoad.
    /// </summary>
    public static Scenario CurrentScenario
    {
        get
        {
            return SaveLoad.CurrentScenario;
        }
        private set
        {
            SaveLoad.CurrentScenario = value;
        }
    }

    /// <summary>
    /// The Canvases used by the UI. Assigned in Unity Editor.
    /// </summary>
    public GameObject[] Menus;

    /// <summary>
    /// Used to fill the object view.
    /// </summary>
    public ObjectPanel PanelPrefab;

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

    /// <summary>
    /// The name of the previous mode to return to when going out of movemode.
    /// </summary>
    private string PreviousMode;

    /// <summary>
    /// This method initializes the editor.
    /// </summary>
    void Start ()
    {
        //The modes need to be made, so they can be used by the editor.
        CreateModes();

        // The object menu needs to be hidden.
        HideMenus();

        // The object menu needs to be populated with prefabs.
        PopulateObjectMenu();

        // Some loading test code, needs to be changed.
        #region TestLoading
        if (CurrentScenario == null)
        {
            Scenario newScenario = new Scenario("new", new Vector2(200, 400));
            CurrentScenario = newScenario;
        }
        #endregion

        // After the current scenario is loaded.
        LoadScenario(SaveLoad.CurrentScenario);

        ChangeEditorMode("Cursor");
    }

    /// <summary>
    /// The update method is called once per frame. It checks for general input and calls the current mode's Update method.
    /// </summary>
    void Update ()
    {
        // Tab is the fixed key for switching to move mode.
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (CurrentMode is MoveMode)
            {
                ChangeEditorMode(PreviousMode);
            }
            else
            {
                PreviousMode = CurrentMode.ToString();
                ChangeEditorMode("Move");
            }
        }
        if (CurrentMode != null)
        {
            CurrentMode.Update();
        }
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

        SaveLoad.CurrentScenario = scenario;
    }

    /// <summary>
    /// Change the editor mode according to the ID of the dropdown menu.
    /// </summary>
    public void DropDownChangeEditorMode()
    {
        int modeID = FindObjectOfType<UnityEngine.UI.Dropdown>().value;
        string mode = "";

        switch (modeID)
        {
            case 0:
                mode = "Cursor";
                break;
            case 1:
                mode = "Selection";
                break;
        }

        ChangeEditorMode(mode);

    }

    /// <summary>
    /// Change the current mode to the given mode.
    /// </summary>
    /// <param name="modeName">The name of the mode to switch to. Should be that mode's ToString()</param>
    public void ChangeEditorMode(string modeName)
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

    /// <summary>
    /// Toggle a menu in the GUI.
    /// </summary>
    /// <param name="menuIndex">The index of the menu in the Menus array.</param>
    public void ToggleMenu(int menuIndex)
    {
        GameObject Menu = Menus[menuIndex];
        if (Menu.transform.localScale != Vector3.zero)
        {
            Menu.transform.localScale = Vector3.zero;
        }
        else
        {
            Menu.transform.localScale = Vector3.one;
        }
    }

    /// <summary>
    /// Return to the main menu.
    /// </summary>
    public void ToMainMenu()
    {
        // Properly save the current scenario.
        SaveScenario();

        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Menu"));
    }

    /// <summary>
    /// Save the current scenario.
    /// </summary>
    public void SaveScenario()
    {
        // Disable the current mode to save changes.
        CurrentMode.Disable();

        SaveLoad.SaveScenario(SaveLoad.CurrentScenario, SaveLoad.GetFilePath(CurrentScenario.Name));

        // Restore the current mode.
        CurrentMode.Enable();
    }

    /// <summary>
    /// Hide all menus at start except the main one.
    /// </summary>
    private void HideMenus()
    {
        for (int i = 0; i < Menus.Length; i++)
        {
            ToggleMenu(i);
        }
    }

    /// <summary>
    /// Load the last saved version of the current scenario.
    /// </summary>
    public void LoadCurrentScenario()
    {
        Scenario scenario;
        if (SaveLoad.LoadSavedScenario(SaveLoad.CurrentPath, out scenario))
        {
            LoadScenario(scenario);
        }
    }

    /// <summary>
    /// Change the prefab in the editor cursor.
    /// Also switches to cursor mode.
    /// </summary>
    /// <param name="name">The name of the prefab.</param>
    public void ChangePrefab(string name)
    {
        if (!(CurrentMode is EditorCursorMode))
        {
            ChangeEditorMode("Cursor");
        }

        EditorCursorMode cursMode = CurrentMode as EditorCursorMode;
        cursMode.CurrentPrefabName = name;
    }

    /// <summary>
    /// Instantiate all modes, used in EditorManager.Start().
    /// </summary>
    private void CreateModes()
    {
        Modes = new Dictionary<string, IEditorMode>();
        IEditorMode cursorMode = new EditorCursorMode(CursorPrefab);
        Modes.Add(cursorMode.ToString(), cursorMode);
        IEditorMode movementMode = new MoveMode(Camera.main);
        Modes.Add(movementMode.ToString(), movementMode);
        IEditorMode selectionMode = new SelectionMode(CursorPrefab);
        Modes.Add(selectionMode.ToString(), selectionMode);
    }

    /// <summary>
    /// Populate the object menu with all prefabs in Resources/Prefabs.
    /// </summary>
    private void PopulateObjectMenu()
    {
        UnityEngine.UI.ScrollRect scrollView = FindObjectOfType<UnityEngine.UI.ScrollRect>();
        foreach (GameObject prefab in PrefabManager.Prefabs)
        {
            ObjectPanel panel = Instantiate(PanelPrefab);
            panel.ObjectPrefab = prefab.name;
            panel.transform.SetParent(scrollView.content, false);
        }
    }
}

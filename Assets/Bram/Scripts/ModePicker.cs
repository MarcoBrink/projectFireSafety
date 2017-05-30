using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.VRScenario;
using Assets.Scripts.Saving;
using Assets.Scripts;

public class ModePicker : MonoBehaviour
{

    private Scenario CurrentScenario;
    public List<GameObject> Prefabs;
    public EditorCursor CursorPrefab;
    private EditorCursor EditorCursor;
    private Dictionary<string, EditorMode> Modes;
    private EditorMode CurrentMode;

    // Use this for initialization
    void Start()
    {
        EditorCursor = Instantiate(CursorPrefab);

        Modes = new Dictionary<string, EditorMode>();
        Modes.Add("Cursor", new EditorCursorMode(EditorCursor));
        Modes.Add("Move", new MoveMode(Camera.main));

        ChangeEditorMode("Cursor");    
    }

    // Update is called once per frame
    
    public void CursorMode()
    {
        if (!(CurrentMode is MoveMode))
        {
            ChangeEditorMode("Cursor");
        }
    }
    public void MoveMode()
    {
        if (!(CurrentMode is EditorCursorMode))
        {
            ChangeEditorMode("Move");
        }
        

    CurrentMode.Update();
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

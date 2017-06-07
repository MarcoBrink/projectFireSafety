using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.VRScenario;
using Assets.Scripts.Saving;
using Assets.Scripts;
using Assets.Scripts.PVRSEditor;

public class ModePicker : MonoBehaviour
{

    private Scenario CurrentScenario;
    public List<GameObject> Prefabs;
    public EditorCursor CursorPrefab;
    private EditorCursor EditorCursor;
    private Dictionary<string, IEditorMode> Modes;
    private IEditorMode CurrentMode;

    // Use this for initialization
    void Start()
    {
        EditorCursor = Instantiate(CursorPrefab);

        Modes = new Dictionary<string, IEditorMode>();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using Assets.Scripts.VRScenario;

namespace Assets.Scripts.PVRSEditor
{
    public class SelectionMode : IEditorMode
    {
        private ScenarioObject selectedObject;
        private EditorCursor mockCursor;

        /// <summary>
        /// Called when the mode is enabled.
        /// </summary>
        public void Enable()
        {

        }

        /// <summary>
        /// Called by the EditorManager's Update method.
        /// </summary>
        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                
            }
        }

        /// <summary>
        /// Called when the mode is disabled.
        /// </summary>
        public void Disable()
        {

        }

        /// <summary>
        /// Used for GUI events.
        /// </summary>
        public void OnGUI()
        {

        }

        /// <summary>
        /// Select an object in the level.
        /// </summary>
        /// <param name="newObject">The Scenario Object connected to the selected object.</param>
        public void SelectObject(ScenarioObject newObject)
        {
            if (selectedObject != null)
            {
                
            }
            this.selectedObject = newObject;
        }

        /// <summary>
        /// Get the scenario object that is at the cursor.
        /// </summary>
        /// <returns></returns>
        private ScenarioObject GetObjectAtCursor()
        {
            ScenarioObject foundObject = null;

            // The object is found using a raycast.
            Vector3 mousePos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject.tag == "Scenario Object")
                {
                    // The scenario object corresponding to the game object needs to be found.
                    foreach (ScenarioObject scenarioObject in EditorManager.CurrentScenario.Objects)
                    {
                        if (scenarioObject.Object.Equals(hit.transform.gameObject))
                        {
                            foundObject = scenarioObject;
                            break;
                        }
                    }
                }
            }

            return foundObject;
        }
    }
}

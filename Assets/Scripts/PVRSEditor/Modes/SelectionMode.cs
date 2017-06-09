using UnityEngine;
using Assets.Scripts;
using Assets.Scripts.VRScenario;

namespace Assets.Scripts.PVRSEditor
{
    public class SelectionMode : IEditorMode
    {
        /// <summary>
        /// The cursor used to change the object.
        /// </summary>
        private EditorCursor ECursor;

        /// <summary>
        /// The prefab for the Editor Cursor. Passed by the EditorManager.
        /// </summary>
        private EditorCursor ECursorPrefab;

        /// <summary>
        /// The currently selected object.
        /// </summary>
        private ScenarioObject SelectedObject;

        /// <summary>
        /// Constructor for selection mode.
        /// </summary>
        /// <param name="eCursorPrefab">The prefab for an editor cursor.</param>
        public SelectionMode(EditorCursor eCursorPrefab)
        {
            this.ECursorPrefab = eCursorPrefab;
        }

        /// <summary>
        /// Called when the mode is enabled.
        /// </summary>
        public void Enable()
        {
            // The cursor needs to be visible for this mode to work.
            if (Cursor.visible == false)
            {
                Cursor.visible = true;
            }

            // The cursor needs to be freed for this mode to work.
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }

        /// <summary>
        /// Called by the EditorManager's Update method.
        /// </summary>
        public void Update()
        {
            // Don't process movement of the selected item if no item is selected.
            if (ECursor != null)
            {
                // Move only if the input has changed, this is more efficiënt.
                if (Input.GetAxis("Horizontal") != 0F)
                {
                    ECursor.Move("Horizontal");
                }

                if (Input.GetAxis("Vertical") != 0F)
                {
                    ECursor.Move("Vertical");
                }

                if (Input.GetAxis("UpDown") != 0F)
                {
                    ECursor.Move("UpDown");
                }

                // Only rotate when actual rotation took place.
                if (Input.GetAxis("RotateX") != 0F || Input.GetAxis("RotateY") != 0F || Input.GetAxis("RotateZ") != 0F)
                {
                    ECursor.Rotate();
                }
            }

            if (Input.GetMouseButtonDown(0) && !(bool)UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                ScenarioObject cursorObject = GetObjectAtCursor();
                if (cursorObject != null)
                {
                    SelectObject(cursorObject);
                }
                else
                {
                    DeselectObject();
                }
            }
        }

        /// <summary>
        /// Called when the mode is disabled.
        /// </summary>
        public void Disable()
        {
            // Deselect any currently selected objects.
            DeselectObject();
        }

        /// <summary>
        /// Select an object in the level.
        /// </summary>
        /// <param name="newObject">The Scenario Object connected to the selected object.</param>
        private void SelectObject(ScenarioObject newObject)
        {
            if (SelectedObject == null || !SelectedObject.Equals(newObject))
            {
                // The currently selected object needs to be deselected first. DeselectObject handles nulls itself.
                DeselectObject();

                this.SelectedObject = newObject;

                // Use a cursor to track the position.
                this.ECursor = Object.Instantiate(ECursorPrefab);
                ECursor.ChangePrefab(newObject.PrefabName);
                ECursor.Position = newObject.Position;
                ECursor.Rotation = newObject.Rotation;
                ECursor.transform.localScale = Vector3.one;

                // If the object has any colliders, they need to be disabled for now.
                Collider[] objectColliders = newObject.Object.GetComponents<Collider>();
                foreach (Collider col in objectColliders)
                {
                    col.enabled = false;
                }

                // Setting the scale to zero effectively hides the object.
                newObject.Object.transform.localScale = Vector3.zero;

                // There was a problem with rigidbodies that continued to sleep even when something they were resting on was selected.
                // All colliders within a range of 35 are selected.
                Collider[] colliders = Physics.OverlapBox(ECursor.Position, Vector3.one * 35);

                // Then, each collider's GameObject is checked for a rigidbody.
                foreach (Collider collider in colliders)
                {
                    Rigidbody colliderRB = collider.gameObject.GetComponent<Rigidbody>();
                    if(colliderRB != null)
                    {
                        // If the collider has a rigidbody, it is told to wake up so physics properly apply.
                        colliderRB.WakeUp();
                    }
                }

                // If the object has a rigidbody, it needs to be disabled to prevent it from interfering with other objects and itself.
                Rigidbody rb = SelectedObject.Object.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.useGravity = false;
                }
            }
        }

        /// <summary>
        /// Deselect the currently selected object.
        /// </summary>
        private void DeselectObject()
        {
            if (SelectedObject != null)
            {
                // Save the position and rotation of the cursor.
                Vector3 newPos = ECursor.Position;
                Quaternion newRot = ECursor.Rotation;

                // The cursor is no longer needed.
                Object.Destroy(ECursor.gameObject);
                
                // Set the object's position and rotation to the right values and show it by resetting its scale to one.
                SelectedObject.Position = newPos;
                SelectedObject.Rotation = newRot;
                SelectedObject.Object.transform.localScale = Vector3.one;

                // If the object has any colliders, they need to be enabled again.
                Collider[] objectColliders = SelectedObject.Object.GetComponents<Collider>();
                foreach (Collider col in objectColliders)
                {
                    col.enabled = true;
                }

                // If the object has a rigidbody, it needs to be reset to work properly.
                Rigidbody rb = SelectedObject.Object.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                }

                // The object is no longer selected.
                SelectedObject = null;
            }
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

        /// <summary>
        /// The ToString method for this mode. Returns its name.
        /// </summary>
        /// <returns>The name of this mode.</returns>
        public override string ToString()
        {
            return "Selection";
        }
    }
}

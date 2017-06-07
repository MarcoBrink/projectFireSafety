using UnityEngine;

namespace Assets.Scripts.PVRSEditor
{
    /// <summary>
    /// This editor mode allows the user to manipulate the cursor so they can place items.
    /// </summary>
    class EditorCursorMode : IEditorMode
    {
        /// <summary>
        /// The editor cursor.
        /// </summary>
        private EditorCursor ECursor;

        /// <summary>
        /// The name of the current prefab. Can be set to change the prefab.
        /// </summary>
        private string CurrentPrefabName
        {
            get
            {
                return ECursor.CurrentPrefab.name;
            }
            set
            {
                ECursor.ChangePrefab(value);
            }
        }

        /// <summary>
        /// The constructor for EditorCursorMode. Needs the cursor to move.
        /// </summary>
        /// <param name="cursor">The cursor prefab.</param>
        public EditorCursorMode(EditorCursor cursor)
        {
            this.ECursor = GameObject.Instantiate(cursor);

            // The cursor is hidden first by scaling it down to zero.
            ECursor.transform.localScale = Vector3.zero;
        }

        /// <summary>
        /// The Enable script for this mode. Shows and unlocks the cursor, just in case.
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

            // Show the cursor by resetting its scale to one.
            ECursor.transform.localScale = Vector3.one;
        }

        /// <summary>
        /// The Disable script for this mode.
        /// </summary>
        public void Disable()
        {
            // Hide the cursor by setting the scale to zero.
            ECursor.transform.localScale = Vector3.zero;
        }

        /// <summary>
        /// The Update script for this mode. Gets input and moves the cursor if needed.
        /// </summary>
        public void Update()
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

            if (Input.GetAxis("Place") != 0F)
            {
                PlaceObject();
            }

            // Only rotate if actual rotation took place.
            if (Input.GetAxis("RotateX") != 0F || Input.GetAxis("RotateY") != 0F || Input.GetAxis("RotateZ") != 0F)
            {
                ECursor.Rotate();
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (!ECursor.IsAtMouse(Mathf.Infinity))
                {
                    ECursor.MoveToMouse();
                }
            }

            
        }

        /// <summary>
        /// Some test GUI code, needs to be removed later.
        /// </summary>
        public void OnGUI()
        {
            
        }

        /// <summary>
        /// The ToString method for this mode. Returns its name.
        /// </summary>
        /// <returns>The name of this mode.</returns>
        public override string ToString()
        {
            return "Cursor";
        }

        /// <summary>
        /// Place an object in the scenario.
        /// The object is placed at the cursor coördinates.
        /// </summary>
        public void PlaceObject()
        {
            // The cursor handles instantiating the object at the right place.
            GameObject newObject = ECursor.PlaceItemAtCursor();

            if (newObject != null)
            {
                // The object needs to be properly registered with the Scenario.
                EditorManager.CurrentScenario.AddObject(CurrentPrefabName, newObject);
            }
        }
    }
}

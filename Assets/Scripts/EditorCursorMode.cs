using UnityEngine;

namespace Assets.Scripts
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
        /// The constructor for EditorCursorMode. Needs the cursor to move.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        public EditorCursorMode(EditorCursor cursor)
        {
            this.ECursor = cursor;
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
        }

        /// <summary>
        /// The Disable script for this mode. It doesn't need to do anything, so it doesn't do anything.
        /// </summary>
        public void Disable() { }

        /// <summary>
        /// The Update script for this mode. Gets input and moves the cursor if needed.
        /// </summary>
        public void Update()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0F)
            {
                
            }

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
        }

        /// <summary>
        /// The ToString method for this mode. Returns its name.
        /// </summary>
        /// <returns>The name of this mode.</returns>
        public override string ToString()
        {
            return "Cursor";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class EditorCursorMode : EditorMode
    {
        private EditorCursor ECursor;
        private float Range = 25F;

        public EditorCursorMode(EditorCursor cursor)
        {
            this.ECursor = cursor;
        }

        public void Enable()
        {

        }

        public void Disable()
        {

        }

        public void Update()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0F)
            {
                Range += scroll;
            }
            if (Input.GetMouseButton(0) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                ECursor.MoveToMouse(Range);
            }
        }
    }
}

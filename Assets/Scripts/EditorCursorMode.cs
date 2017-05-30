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
                
            }

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
    }
}

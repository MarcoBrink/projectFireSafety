using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// This editor mode allows the user to use first person freecam controls in the editor.
    /// </summary>
    public class MoveMode : IEditorMode
    {
        /// <summary>
        /// The current X rotation of the camera.
        /// </summary>
        private float XRotation;

        /// <summary>
        /// The maximum X rotation of the camera.
        /// </summary>
        private float MaxXRotation = 60;

        /// <summary>
        /// The minimum X rotation of the camera.
        /// </summary>
        private float MinXRotation = -60;

        /// <summary>
        /// The current Y rotation of the camera.
        /// </summary>
        private float YRotation;

        /// <summary>
        /// The movement speed.
        /// </summary>
        private float MoveSpeed = 0.5F;

        /// <summary>
        /// The transform of the main camera, used for movement.
        /// </summary>
        private Transform CameraTransform;

        /// <summary>
        /// Constructor for MoveMode. Takes a camera to use.
        /// </summary>
        /// <param name="cam"></param>
        public MoveMode(Camera cam)
        {
            this.XRotation = 0;
            this.YRotation = 0;
            // only the transform of the camera is needed, so this saves a bit of effort.
            this.CameraTransform = cam.transform;
        }

        /// <summary>
        /// Called when the mode is enabled. Hides and locks the cursor.
        /// </summary>
        public void Enable()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        /// <summary>
        /// Called when the mode is disabled. Unlocks and shows the cursor.
        /// </summary>
        public void Disable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        /// <summary>
        /// The update script for this mode. Looks at the camera and then moves according to input.
        /// </summary>
        public void Update()
        {
            // Look first, then move. This means movement is possibly more synced to the direction the camer is facing.
            Look();
            Move();
        }

        /// <summary>
        /// Look in the direction of cursor movement.
        /// </summary>
        private void Look()
        {
            // To get the direction to turn to, the axes of the mouse are needed.
            float XAxis = Input.GetAxis("Mouse Y");
            float YAxis = Input.GetAxis("Mouse X");

            if (XAxis != 0)
            {
                // Rotation on the X axis is limited to the min and max X because making full backflips with the camera is disoriënting.
                XRotation -= XAxis * 5;
                XRotation = Mathf.Clamp(XRotation, MinXRotation, MaxXRotation);
            }
            if (YAxis != 0)
            {
                YRotation += YAxis * 5;
            }

            Vector3 newAngles = new Vector3(XRotation, YRotation, 0);
            CameraTransform.rotation = Quaternion.Slerp(CameraTransform.rotation, Quaternion.Euler(newAngles), 500 * Time.deltaTime);
        }

        /// <summary>
        /// Move according to user input.
        /// </summary>
        private void Move()
        {
            // Move left/right.
            MoveCamera("Vertical");

            // Move forwards/backwards.
            MoveCamera("Horizontal");

            // Move up/down.
            MoveCamera("UpDown");
        }

        /// <summary>
        /// Move the camera according to input on the given axis.
        /// </summary>
        /// <param name="axis">The name of the axis.</param>
        private void MoveCamera(string axis)
        {
            // Do nothing if the axis is zero, this is more efficiënt.
            if (Input.GetAxis(axis) != 0F)
            {
                Vector3 moveDir = Vector3.zero;

                // Get the right direction of movement.
                if (axis == "Vertical")
                {
                    moveDir = CameraTransform.forward;
                }
                else if (axis == "Horizontal")
                {
                    moveDir = CameraTransform.right;
                }
                else if (axis == "UpDown")
                {
                    moveDir = CameraTransform.up;
                }

                // Apply the movement to the camera.
                CameraTransform.Translate(moveDir * Input.GetAxis(axis), Space.World);
            }
        }

        /// <summary>
        /// This mode's override for ToString().
        /// </summary>
        /// <returns>The name of this mode.</returns>
        public override string ToString()
        {
            return "Move";
        }
    }
}

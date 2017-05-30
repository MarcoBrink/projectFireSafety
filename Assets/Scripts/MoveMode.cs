using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class MoveMode : EditorMode
    {
        private float XRotation;
        private float MaxXRotation = 60;
        private float MinXRotation = -60;
        private float YRotation;
        private float MoveSpeed = 0.5F;
        private Transform CameraTransform;

        public MoveMode(Camera cam)
        {
            this.XRotation = 0;
            this.YRotation = 0;
            this.CameraTransform = cam.transform;
        }

        public void Enable()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void Disable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void Update()
        {
            Look();
            Move();
        }

        private void Look()
        {
            float XAxis = Input.GetAxis("Mouse Y");
            float YAxis = Input.GetAxis("Mouse X");

            if (XAxis != 0)
            {
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

        private void Move()
        {
            float vertical = Input.GetAxis("Vertical");
            if (vertical != 0F)
            {
                CameraTransform.Translate(CameraTransform.forward * MoveSpeed * Input.GetAxis("Vertical"), Space.World);
            }

            float horizontal = Input.GetAxis("Horizontal");
            if (horizontal != 0F)
            {
                CameraTransform.Translate(CameraTransform.right * MoveSpeed * Input.GetAxis("Horizontal"), Space.World);
            }

            if (Input.GetKey(KeyCode.Q))
            {
                CameraTransform.Translate(CameraTransform.up * MoveSpeed, Space.World);
            }
            if (Input.GetKey(KeyCode.E))
            {
                CameraTransform.Translate(CameraTransform.up * -MoveSpeed, Space.World);
            }
        }
    }
}

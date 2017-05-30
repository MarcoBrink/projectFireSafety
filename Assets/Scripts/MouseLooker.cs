using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class MouseLooker
    {
        private float XRotation;
        private float MaxXRotation = 60;
        private float MinXRotation = -60;
        private float YRotation;
        private Transform CameraTransform;

        public MouseLooker(Camera camera)
        {
            this.XRotation = 0;
            this.YRotation = 0;
            this.CameraTransform = camera.transform;
        }

        public void Look()
        {
            float XAxis = Input.GetAxis("Mouse Y");
            float YAxis = Input.GetAxis("Mouse X");

            if (XAxis != 0)
            {
                XRotation += XAxis * 5;
                XRotation = Mathf.Clamp(XRotation, MinXRotation, MaxXRotation);
            }
            if (YAxis != 0)
            {
                YRotation -= YAxis * 5;
            }

            Vector3 newAngles = new Vector3(XRotation, YRotation, 0);
            CameraTransform.rotation = Quaternion.Slerp(CameraTransform.rotation, Quaternion.Euler(newAngles), 500 * Time.deltaTime);
        }
    }
}

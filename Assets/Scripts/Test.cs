using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    class Test : MonoBehaviour
    {
        private Rigidbody ThisObject;
        private bool Selected;

        // Initialisation for the test script.
        void Start()
        {
            ThisObject = GetComponent<Rigidbody>();
            Selected = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (!this.Selected)
            {
                if (!ThisObject.isKinematic)
                {
                    ThisObject.isKinematic = true;
                }
                if (!ThisObject.detectCollisions)
                {
                    ThisObject.detectCollisions = true;
                }
            }
        }

        private void OnMouseDown()
        {
            this.Selected = true;
            ThisObject.isKinematic = false;
            ThisObject.detectCollisions = false;
        }

        private void OnMouseDrag()
        {
            float dist = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            Vector3 MousePos = Input.mousePosition;

            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll != 0)
            {
                dist += scroll;
            }

            Vector3 trans = new Vector3(MousePos.x, MousePos.y, dist);

            transform.position = Camera.main.ScreenToWorldPoint(trans);
        }

        private void OnMouseUpAsButton()
        {
            this.Selected = false;
            ThisObject.isKinematic = true;
            ThisObject.detectCollisions = true;
        }
    }
}

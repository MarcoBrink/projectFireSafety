using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class EditorCursor : MonoBehaviour
{
    public bool CanPlace
    {
        get
        {
            bool canPlace = false;

            if (transform.childCount != 0)
            {
                GameObject cursorObject = transform.GetChild(0).gameObject;
                WireframeScript wireFrame = cursorObject.GetComponent<WireframeScript>();
                if (wireFrame.lineColor == Color.green)
                {
                    canPlace = true;
                }
            }

            return canPlace;
        }
        // A private set because Unity's version of C# doesn't support read-only properties.
        private set { }
    }
    private GameObject CurrentPrefab;

	/// <summary>
    /// The start method for the EditorCursor shows the first prefab.
    /// </summary>
	void Start ()
    {
        string firstPrefabName = PrefabManager.Prefabs[0].name;
        ChangePrefab(firstPrefabName);
    }

    private void Update()
    {
        
    }

    

    public bool IsAtMouse()
    {
        bool atMouse = false;

        Camera mainCamera = Camera.main;
        Vector3 mousePos = Input.mousePosition;

        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit, 500F))
        {
            if (hit.transform.root.gameObject.Equals(this.gameObject))
            {
                atMouse = true;
            }
        }

        return atMouse;
    }

    public void MoveToMouse(float range)
    {
        Camera mainCamera = Camera.main;
        Vector3 mousePos = Input.mousePosition;

        Vector3 newPos = new Vector3();

        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit, range))
        {
            newPos = hit.point;
        }
        else
        {
            mousePos.z = range;
            newPos = mainCamera.ScreenToWorldPoint(mousePos);
        }

        this.transform.position = newPos;
    }

    public void ChangePrefab(string prefabName)
    {
        if (transform.childCount != 0)
        {
            Destroy(transform.GetChild(0));
        }
        

        GameObject prefab = PrefabManager.GetPrefab(prefabName);
        if (prefab == null)
        {
            Debug.Log("Prefab Change Failed: No Prefab");
        }
        else
        {
            CurrentPrefab = prefab;

            GameObject copy = Instantiate(CurrentPrefab, this.transform.position, this.transform.rotation, this.transform);
            Component[] copyComponents = copy.GetComponents<Component>();
            foreach (Component component in copyComponents)
            {
                if (!(component is MeshFilter) && !(component is Collider) && !(component is Renderer) && !(component is Transform))
                {
                    Destroy(component);
                }
            }

            copy.transform.SetPositionAndRotation(transform.position, transform.rotation);

            WireframeScript wireframe = copy.AddComponent<WireframeScript>();
            wireframe.render_mesh_normally = true;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class EditorCursor : MonoBehaviour
{
    private bool CanPlace;
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

    void OnTriggerStay(Collider other)
    {
        WireframeScript wireframe = GetComponentInChildren<WireframeScript>();
        wireframe.lineColor = Color.red;
        CanPlace = false;
    }

    void OnTriggerExit(Collider other)
    {
        WireframeScript wireframe = GetComponentInChildren<WireframeScript>();
        wireframe.lineColor = Color.green;
        CanPlace = true;
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

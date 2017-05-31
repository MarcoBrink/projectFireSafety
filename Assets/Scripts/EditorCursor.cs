using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

/// <summary>
/// The cursor used by the editor. Spawns objects.
/// </summary>
public class EditorCursor : MonoBehaviour
{
    /// <summary>
    /// The speed at which the cursor moves.
    /// </summary>
    private float MoveSpeed = 0.125F;

    /// <summary>
    /// The bounds of the current prefab.
    /// </summary>
    private Bounds prefabBounds
    {
        get
        {
            Bounds bounds = new Bounds();

            if (transform.childCount != 0)
            {
                // The current bounds can be found on the stripped-down version of the prefab.
                bounds = transform.GetChild(0).GetComponent<Collider>().bounds;
            }

            return bounds;
        }
        // Set does nothing because this is technically read-only, but Unity's version of C# doesn't support read-only properties.
        set { }
    }

    /// <summary>
    /// Whether or not the selected object can be placed here.
    /// </summary>
    public bool CanPlace
    {
        get
        {
            bool canPlace = false;

            if (transform.childCount != 0)
            {
                // Collisions are kept track of in the WireFrameScript.
                GameObject cursorObject = transform.GetChild(0).gameObject;
                WireframeScript wireFrame = cursorObject.GetComponent<WireframeScript>();
                if (!wireFrame.collides)
                {
                    canPlace = true;
                }
            }

            return canPlace;
        }
        // A private set because Unity's version of C# doesn't support read-only properties.
        private set { }
    }

    /// <summary>
    /// The current prefab used by the cursor. Used for placement.
    /// </summary>
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

    /// <summary>
    /// Move along the given axis based on input.
    /// </summary>
    /// <param name="axis">The input axis to check for movement.</param>
    public void Move(string axis)
    {
        Camera mainCam = Camera.main;
        Vector3 transformDir = Vector3.zero;
        if (axis == "Horizontal")
        {
            transformDir = mainCam.transform.right;
            transformDir.y = 0;
        }
        else if (axis == "Vertical")
        {
            transformDir = mainCam.transform.forward;
            transformDir.y = 0;
        }
        else if (axis == "UpDown")
        {
            transformDir = Vector3.up;
        }
        transform.Translate(transformDir * MoveSpeed * Input.GetAxis(axis), Space.World);
    }

    /// <summary>
    /// Rotate the cursor along the given axis.
    /// </summary>
    /// <param name="axis">The axis to rotate along.</param>
    public void Rotate()
    {
        Vector3 mousePos = Input.mousePosition;

        mousePos.z = Camera.main.transform.position.z - transform.position.z;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

        float angle = -Mathf.Atan2(transform.position.z - mouseWorldPos.z, transform.position.x - mouseWorldPos.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, angle, 0), 200 * Time.deltaTime);

        //Vector3 euler = Vector3.zero;

        //if (axis == 'x')
        //{
        //    euler.x = Input.GetAxisRaw("Mouse ScrollWheel") * 20F;
        //}
        //else if (axis == 'y')
        //{
        //    euler.y = Input.GetAxisRaw("Mouse ScrollWheel") * 20F;
        //}
        //else if (axis == 'z')
        //{
        //    euler.z = Input.GetAxisRaw("Mouse ScrollWheel") * 20F;
        //}
        
        //transform.Rotate(test);
    }

    /// <summary>
    /// Reset the rotation to zero.
    /// </summary>
    public void ResetRotation()
    {
        transform.rotation = new Quaternion();
    }

    
    /// <summary>
    /// Check if the cursor is at the current mouse position, within the specified range.
    /// </summary>
    /// <param name="range">The range within which the cursor should be to qualify.</param>
    /// <returns>True if the cursor is at the mouse position, false if it isn't.</returns>
    public bool IsAtMouse(float range)
    {
        bool atMouse = false;

        Camera mainCamera = Camera.main;
        Vector3 mousePos = Input.mousePosition;

        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit, range))
        {
            if (hit.transform.root.gameObject.Equals(this.gameObject))
            {
                atMouse = true;
            }
        }

        return atMouse;
    }

    /// <summary>
    /// Get the position fo the cursor after collision with the given collider.
    /// </summary>
    /// <param name="collider">The collider with which the cursor has collided.</param>
    /// <returns>The new position for the cursor after the collision.</returns>
    private Vector3 GetCollisionPos(Collider collider)
    {
        Vector3 cameraPos  = Camera.main.transform.position;
        Vector3 collisionPos = collider.ClosestPointOnBounds(cameraPos);

        Vector3 head = collisionPos - cameraPos;
        float dist = head.magnitude;
        Vector3 dir = head / dist;
        Debug.Log(dir.ToString());

        Ray ray = new Ray(transform.position, dir);
        float hitdist;
        collider.bounds.IntersectRay(ray, out hitdist);

        // The cursor prefab object has layer 8.
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo, hitdist + 1F, layerMask);

        Vector3 offset = Vector3.zero;
        offset.x = -dir.x * prefabBounds.extents.x;
        offset.y = -dir.y * prefabBounds.extents.y;
        offset.z = -dir.z * prefabBounds.extents.z;

        Vector3 finalPos = collisionPos + offset;
        return finalPos;
    }

    /// <summary>
    /// Change to the prefab with the given name.
    /// </summary>
    /// <param name="prefabName">The name of the prefab.</param>
    public void ChangePrefab(string prefabName)
    {
        if (transform.childCount != 0)
        {
            Destroy(transform.GetChild(0));
        }
        
        // Retrieve the prefab, debug code needs to be changed later.
        GameObject prefab = PrefabManager.GetPrefab(prefabName);
        if (prefab == null)
        {
            Debug.Log("Prefab Change Failed: No Prefab");
        }
        else
        {
            CurrentPrefab = prefab;

            // The prefab is copied and stripped of all components that aren't needed for the editor cursor. Prevents odd behaviour.
            GameObject copy = Instantiate(CurrentPrefab, this.transform.position, this.transform.rotation, this.transform);
            Component[] copyComponents = copy.GetComponents<Component>();
            foreach (Component component in copyComponents)
            {
                if (!(component is MeshFilter) && !(component is Collider) && !(component is Renderer) && !(component is Transform))
                {
                    Destroy(component);
                }
                if (component is Collider)
                {
                    Collider collider = component as Collider;
                    collider.isTrigger = true;
                }
            }

            // The cursor is placed at the right position.
            copy.transform.SetPositionAndRotation(transform.position, transform.rotation);

            // The wireframe script is added to the object to make it clear whether or not it can be placed.
            WireframeScript wireframe = copy.AddComponent<WireframeScript>();
            wireframe.render_mesh_normally = true;

            // The cursor is placed in layer 8, which is ignored by several raycasts in other related code.
            copy.layer = 8;
        }
    }

    /// <summary>
    /// Place the selected prefab at the cursor.
    /// </summary>
    /// <returns>The placed object. Null if it couldn't place the object.</returns>
    public GameObject PlaceItemAtCursor()
    {
        GameObject placed = null;

        // Obviously, the item can't be placed if this boolean is set to false.
        if (CanPlace)
        {
            placed = Instantiate(CurrentPrefab, transform.position, transform.rotation);
        }
        
        return placed;
    }
}

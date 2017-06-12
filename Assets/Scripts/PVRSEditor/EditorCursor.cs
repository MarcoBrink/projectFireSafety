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
    /// The position of the cursor.
    /// </summary>
    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
        set
        {
            transform.position = value;
        }
    }

    /// <summary>
    /// The rotation of the cursor.
    /// </summary>
    public Quaternion Rotation
    {
        get
        {
            return this.transform.rotation;
        }
        set
        {
            this.transform.rotation = value;
        }
    }


    /// <summary>
    /// The bounds of the current prefab.
    /// </summary>
    /// Used to place the cursor correctly of the surface of other objects.
    private Bounds PrefabBounds
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

                // This is stored in the collisionscript.
                CursorCollision collision = cursorObject.GetComponent<CursorCollision>();
                if (!collision.Collides)
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
    public GameObject CurrentPrefab;

	/// <summary>
    /// The start method for the EditorCursor shows the first prefab.
    /// </summary>
	void Start ()
    {
        string firstPrefabName = PrefabManager.Prefabs[0].name;
        if (CurrentPrefab == null)
        {
            ChangePrefab(firstPrefabName);
        }
    }

    /// <summary>
    /// Move along the given axis based on input.
    /// </summary>
    /// <param name="axis">The input axis to check for movement.</param>
    public void Move(string axis)
    {
        // Shortcut for main camera and start
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
        // The vector to store all angles to rotate at.
        Vector3 rotationEuler = Vector3.zero;
        rotationEuler.x += Input.GetAxis("RotateX") * 1.5F;
        rotationEuler.y += Input.GetAxis("RotateY") * 1.5F;
        rotationEuler.z += Input.GetAxis("RotateZ") * 1.5F;

        // The main camera transform is used to get relative rotation.
        Transform mainCamTrans = Camera.main.transform;

        // Rotate along each angle relative to the world.
        transform.Rotate(mainCamTrans.forward, rotationEuler.x, Space.World);
        transform.Rotate(mainCamTrans.up, rotationEuler.y, Space.World);
        transform.Rotate(mainCamTrans.right, rotationEuler.z, Space.World);
    }

    /// <summary>
    /// Reset the rotation to zero.
    /// </summary>
    public void ResetRotation()
    {
        transform.rotation = new Quaternion();
    }

    /// <summary>
    /// Move the cursor to the mouse.
    /// </summary>
    public void MoveToMouse()
    {
        Camera mainCamera = Camera.main;
        Vector3 mousePos = Input.mousePosition;

        // The cursor is in layer 8.
        int layerMask = 0 >> 8;
        layerMask = ~layerMask;

        // Get the position of any obstacles in the way.
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            // Place the item on the surface of the object.
            Collider collider = hit.collider;
            Vector3 pos = GetCollisionPos(hit);
            this.transform.position = pos;
        }
        else
        {
            this.transform.position = mainCamera.transform.position + ray.direction * 15F;
        }
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
    private Vector3 GetCollisionPos(RaycastHit hit)
    {
        Transform mainCam = Camera.main.transform;
        Vector3 collisionPos = hit.point;

        Vector3 dir = hit.normal;

        Vector3 offset = Vector3.zero;
        offset.x = dir.x * PrefabBounds.extents.x + 0.01F;
        offset.y = dir.y * PrefabBounds.extents.y + 0.01F;
        offset.z = dir.z * PrefabBounds.extents.z + 0.01F;

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
            Destroy(transform.GetChild(0).gameObject);
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

            copy.AddComponent<CursorCollision>();

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
            placed.tag = "Scenario Object";
        }
        
        return placed;
    }
}

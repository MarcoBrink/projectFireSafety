using UnityEngine;

public class CursorCollision : MonoBehaviour
{
    public bool Collides;
    private Material[] ObjectMaterials;
    private Collider[] Colliders;

    private void Start()
    {
        ObjectMaterials = GetComponent<Renderer>().materials;
        Colliders = GetComponents<Collider>();
        ChangeColor(Color.green);
        Collides = false;
    }

    private void Update()
    {
        // The cursor's own layer is 8, so that should be ignored.
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        bool colliding = false;

        foreach (Collider collider in Colliders)
        {
            Collider[] hits = Physics.OverlapBox(collider.bounds.center, collider.bounds.extents, collider.transform.rotation, layerMask);

            if (hits.Length != 0 && colliding == false)
            {
                colliding = true;
            }
        }

        if (colliding && Collides == false)
        {
            ChangeColor(Color.red);
            Collides = true;
        }
        else if (!colliding && Collides == true)
        {
            ChangeColor(Color.green);
            Collides = false;
        }
    }

    /// <summary>
    /// Change the color of the object.
    /// </summary>
    /// <param name="color">The color to change it to.</param>
    private void ChangeColor(Color color)
    {
        foreach (Material mat in ObjectMaterials)
        {
            mat.color = color;
        }
    }
}

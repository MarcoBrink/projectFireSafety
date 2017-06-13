using UnityEngine;

public class CursorCollision : MonoBehaviour
{
    public bool Collides;
    private Material[] ObjectMaterials;
    private Collider ThisCollider;

    private void Start()
    {
        ObjectMaterials = GetComponent<Renderer>().materials;
        ThisCollider = GetComponent<Collider>();
        ChangeColor(Color.green);
        Collides = false;
    }

    private void Update()
    {
        // The cursor's own layer is 8, so that should be ignored.
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        Collider[] hits = Physics.OverlapBox(transform.position, ThisCollider.bounds.extents, transform.rotation, layerMask);

        if (hits.Length != 0)
        {
            Collides = true;
            ChangeColor(Color.red);
        }
        else if (hits.Length == 0)
        {
            Collides = false;
            ChangeColor(Color.green);
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

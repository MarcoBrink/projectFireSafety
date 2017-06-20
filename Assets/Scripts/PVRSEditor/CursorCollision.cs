using UnityEngine;

public class CursorCollision : MonoBehaviour
{
    /// <summary>
    /// A boolean to set if an object is currently in collision
    /// </summary>
    public bool Collides;
    //Arrays of materials and colliders
    private Material[] ObjectMaterials;
    private Collider[] Colliders;

    /// <summary>
    /// As soon as an object of this class is instanced, set all its properties
    /// </summary>
    private void Start()
    {
        ObjectMaterials = GetComponent<Renderer>().materials;
        Colliders = GetComponents<Collider>();
        ChangeColor(Color.green);
        Collides = false;
    }

    /// <summary>
    /// Tasks to repeat every frame
    /// </summary>
    private void Update()
    {
        // The cursor's own layer is 8, so that should be ignored.
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        //Assume there is no collision
        bool colliding = false;

        //Check if there is any collision
        foreach (Collider collider in Colliders)
        {
            Collider[] hits = Physics.OverlapBox(collider.bounds.center, collider.bounds.extents, collider.transform.rotation, layerMask);

            if (hits.Length != 0 && colliding == false)
            {
                //And only stop assuming no collision if you're certain there is.
                colliding = true;
            }
        }

        //Once collision has been confirmed or debunked, set the color accordingly
        if (colliding && !Collides)
        {
            ChangeColor(Color.red);
            Collides = true;
        }
        else if (!colliding && Collides)
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

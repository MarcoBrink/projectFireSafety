using UnityEngine;
using System.Collections;

public class WireframeScript : MonoBehaviour
{

    public bool render_mesh_normally = true;
    public Color lineColor = Color.green;
    public Color backgroundColor = new Color(0.0f, 0.5f, 0.5f);
    public bool ZWrite = true;
    public bool AWrite = true;
    public bool blend = true;
    public float lineWidth = 1;
    public int size = 0;

    private Vector3[] lines;
    private ArrayList lines_List;
    public Material lineMaterial;

    void Start()
    {
        if (lineMaterial == null)
        {
            Shader shader = Shader.Find("Custom/shaderfile");
            lineMaterial = new Material(shader);
        }
        lineMaterial.hideFlags = HideFlags.HideAndDontSave;
        lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
        lines_List = new ArrayList();

        MeshFilter filter = gameObject.GetComponent<MeshFilter>();
        Mesh mesh = filter.mesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        for (int i = 0; i + 2 < triangles.Length; i += 3)
        {
            lines_List.Add(vertices[triangles[i]]);
            lines_List.Add(vertices[triangles[i + 1]]);
            lines_List.Add(vertices[triangles[i + 2]]);
        }

        lines = (Vector3[])lines_List.ToArray(typeof(Vector3));
        lines_List.Clear();//free memory from the arraylist
        size = lines.Length;
    }

    // to simulate thickness, draw line as a quad scaled along the camera's vertical axis.
    void DrawQuad(Vector3 p1, Vector3 p2)
    {
        float thisWidth = 1.0f / Screen.width * lineWidth * 0.5f;
        Vector3 edge1 = Camera.main.transform.position - (p2 + p1) / 2.0f;    //vector from line center to camera
        Vector3 edge2 = p2 - p1;    //vector from point to point
        Vector3 perpendicular = Vector3.Cross(edge1, edge2).normalized * thisWidth;

        GL.Vertex(p1 - perpendicular);
        GL.Vertex(p1 + perpendicular);
        GL.Vertex(p2 + perpendicular);
        GL.Vertex(p2 - perpendicular);
    }

    Vector3 to_world(Vector3 vec)
    {
        return gameObject.transform.TransformPoint(vec);
    }

    void OnRenderObject()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.enabled = render_mesh_normally;
        if (lines == null || lines.Length < lineWidth)
        {
            print("No lines");
        }
        else
        {
            lineMaterial.SetPass(0);

            if (lineWidth == 1)
            {
                GL.Begin(GL.LINES);
                GL.Color(lineColor);
                for (int i = 0; i + 2 < lines.Length; i += 3)
                {
                    Vector3 vec1 = to_world(lines[i]);
                    Vector3 vec2 = to_world(lines[i + 1]);
                    Vector3 vec3 = to_world(lines[i + 2]);

                    GL.Vertex(vec1);
                    GL.Vertex(vec2);
                    GL.Vertex(vec2);
                    GL.Vertex(vec3);
                    GL.Vertex(vec3);
                    GL.Vertex(vec1);
                }
            }
            else
            {
                GL.Begin(GL.QUADS);
                GL.Color(lineColor);
                for (int i = 0; i + 2 < lines.Length; i += 3)
                {
                    Vector3 vec1 = to_world(lines[i]);
                    Vector3 vec2 = to_world(lines[i + 1]);
                    Vector3 vec3 = to_world(lines[i + 2]);
                    DrawQuad(vec1, vec2);
                    DrawQuad(vec2, vec3);
                    DrawQuad(vec3, vec1);
                }
            }
            GL.End();
        }
    }

    void OnTriggerExit(Collider other)
    {
        lineColor = Color.green;
    }

    void OnTriggerStay(Collider other)
    {
        lineColor = Color.red;
    }
}

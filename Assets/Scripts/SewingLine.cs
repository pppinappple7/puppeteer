using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SewingLine : MonoBehaviour
{
    [SerializeField] GameObject[] sewPoints;
    [SerializeField] Material threadMaterial;
    [SerializeField] float width = 0.3f;
    [SerializeField] float surfaceOffset = 0.01f;
    [SerializeField] float textureSpeed = 1f;

    Mesh mesh;
    Vector2[] baseUV = new Vector2[0];
    Vector2[] uv = new Vector2[0];
    float offset;

    void Awake()
    {
        mesh = new Mesh();
        mesh.name = "SewingLineMesh";
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = threadMaterial;
    }

    void Update()
    {
        offset += textureSpeed * Time.deltaTime;

        for (int i = 0; i < uv.Length; i++)
            uv[i].x = baseUV[i].x + offset;

        mesh.uv = uv;
    }

    public void Refresh()
    {
        int count = sewPoints.Length;

        if (count < 2)
        {
            mesh.Clear();
            return;
        }

        Vector3[] vertices = new Vector3[count * 2];
        baseUV = new Vector2[count * 2];
        uv = new Vector2[count * 2];

        System.Collections.Generic.List<int> triangles = new();

        float distance = 0;

        for (int i = 0; i < count; i++)
        {
            Vector3 prev = i > 0
                ? sewPoints[i].transform.position - sewPoints[i - 1].transform.position
                : sewPoints[1].transform.position - sewPoints[0].transform.position;

            Vector3 next = i < count - 1
                ? sewPoints[i + 1].transform.position - sewPoints[i].transform.position
                : prev;

            prev.y = 0;
            next.y = 0;

            prev.Normalize();
            next.Normalize();

            Vector3 side1 = Vector3.Cross(Vector3.up, prev).normalized;
            Vector3 side2 = Vector3.Cross(Vector3.up, next).normalized;

            Vector3 side = (side1 + side2).normalized;

            float scale = 1f / Mathf.Max(Vector3.Dot(side, side2), 0.1f);
            scale = Mathf.Min(scale, 2.5f);

            Vector3 center = sewPoints[i].transform.position + Vector3.up * surfaceOffset;
            Vector3 offsetSide = side * width * 0.5f * scale;

            vertices[i * 2] = transform.InverseTransformPoint(center - offsetSide);
            vertices[i * 2 + 1] = transform.InverseTransformPoint(center + offsetSide);

            if (i > 0)
                distance += Vector3.Distance(
                    sewPoints[i - 1].transform.position,
                    sewPoints[i].transform.position
                );

            baseUV[i * 2] = new Vector2(distance, 0);
            baseUV[i * 2 + 1] = new Vector2(distance, 1);

            if (i < count - 1 && sewPoints[i + 1].activeSelf)
            {
                int v = i * 2;

                triangles.Add(v);
                triangles.Add(v + 2);
                triangles.Add(v + 1);

                triangles.Add(v + 1);
                triangles.Add(v + 2);
                triangles.Add(v + 3);
            }
        }

        uv = (Vector2[])baseUV.Clone();

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}
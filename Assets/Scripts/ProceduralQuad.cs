using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ProceduralQuad : MonoBehaviour
{
    [SerializeField] Material targetMaterial;
    [SerializeField] float width = 1;
    [SerializeField] float height = 1;

#if UNITY_EDITOR
    private void OnValidate() => Refresh();
#endif

    [ContextMenu("Refresh")]
    void Refresh()
    {
        MeshFilter meshFilter;
        MeshRenderer meshRenderer;

        if (!gameObject.TryGetComponent(out meshFilter))
            meshFilter = gameObject.AddComponent<MeshFilter>();

        if (!gameObject.TryGetComponent(out meshRenderer))
            meshRenderer = gameObject.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(-width/2, 0, 0),
            new Vector3(width/2, 0, 0),
            new Vector3(-width/2, height, 0),
            new Vector3(width/2, height, 0),
        };

        mesh.vertices = vertices;

        int[] triangles = new int[6]
        {
            2, 3, 1,
            0, 2, 1
        };

        mesh.triangles = triangles;

        Vector3[] normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
        };

        mesh.normals = normals;

        Vector2[] uvs = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };

        mesh.uv = uvs;

        meshFilter.mesh = mesh;
        meshRenderer.material = targetMaterial;
    }
}


using System;
using UnityEngine;

public class ProceduralQuad : MonoBehaviour
{
    public Material targetMaterial;
    public float width = 1;
    public float height = 1;
    
#if UNITY_EDITOR
    void OnValidate()
    {
        MeshRenderer meshRenderer;
        MeshFilter meshFilter;

        if (!gameObject.TryGetComponent(out meshRenderer))
            meshRenderer = gameObject.AddComponent<MeshRenderer>();

        if (!gameObject.TryGetComponent(out meshFilter))
            meshFilter = gameObject.AddComponent<MeshFilter>();

        Material selectedMaterial = targetMaterial ? targetMaterial : new Material(Shader.Find("Standard"));
        
        meshRenderer.sharedMaterial = selectedMaterial;

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(-width / 2, 0, 0),
            new Vector3(width / 2, 0, 0),
            new Vector3(-width / 2, height, 0),
            new Vector3(width / 2, height, 0)
        };
        mesh.vertices = vertices;

        int[] tris = new int[6]
        {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
        };
        mesh.triangles = tris;

        Vector3[] normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        mesh.normals = normals;

        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        mesh.uv = uv;

        meshFilter.mesh = mesh;
    }
    
#endif
}


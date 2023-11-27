
using System;
using UnityEngine;

[ExecuteAlways]
public class ProceduralQuad : MonoBehaviour
{
    static readonly int MainTex = Shader.PropertyToID("_MainTex");
    
    [SerializeField] Material targetMaterial;
    
    [SerializeField] float width = 1;
    [SerializeField] float height = 1;

    [SerializeField] Texture2D texture;


    void OnDidApplyAnimationProperties()
    {
        Refresh();
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        Refresh();
    }
    
#endif

    void Refresh()
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
        
        if(!Application.isPlaying)
            meshRenderer.sharedMaterial.SetTexture(MainTex, texture);
        else
            meshRenderer.material.SetTexture(MainTex, texture);
    }
}


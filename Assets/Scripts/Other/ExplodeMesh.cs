using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeMesh : MonoBehaviour {

    [SerializeField]
    Mesh BrickWoodMesh;

    [SerializeField]
    Mesh BrickRockMesh;

    [SerializeField]
    Mesh BrickLogMesh;

    [SerializeField]
    Material BrickWoodMaterial;

    [SerializeField]
    Material BrickRockMaterial;

    [SerializeField]
    Material BrickLogMaterial; 

    public static List<GameObject> GetExplodingMeshes(Material material, Mesh mesh)
    {
        List<GameObject> meshes = new List<GameObject>();

        Vector3[] verts = mesh.vertices;
        Vector3[] normals = mesh.normals;
        Vector2[] uvs = mesh.uv;  

        int[] indices = mesh.GetTriangles(0);
 
        for (int i = 0; i < indices.Length; i += 3)
        {
            Vector3[] newVerts = new Vector3[3];
            Vector3[] newNormals = new Vector3[3];
            Vector2[] newUvs = new Vector2[3];
            for (int n = 0; n < 3; n++)
            {
                int index = indices[i + n];
                newVerts[n] = verts[index];
                newUvs[n] = uvs[index];
                    newNormals[n] = normals[index];
            }

            Mesh newMesh = new Mesh();
            newMesh.vertices = newVerts;
            newMesh.normals = newNormals;
            newMesh.uv = newUvs;
      
            newMesh.triangles = new int[] { 0, 1, 2, 2, 1, 0 };

            GameObject GO = new GameObject("Triangle " + (i / 3));           
            
            GO.AddComponent<MeshFilter>().mesh = newMesh;
            meshes.Add(GO);
         
         }
        return meshes;      
    }
}

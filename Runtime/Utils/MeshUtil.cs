using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;

namespace Voxell
{
  public static class MeshUtil
  {
    /// <summary>Creates a deep copy of a mesh.</summary>
    /// <param name="originMesh">source of mesh to copy from</param>
    /// <param name="newMesh">location of copied mesh</param>
    public static void DeepCopyMesh(in Mesh originMesh, out Mesh newMesh)
    {
      newMesh = new Mesh();
      newMesh.vertices = originMesh.vertices;
      newMesh.triangles = originMesh.triangles;
      newMesh.uv = originMesh.uv;
      newMesh.normals = originMesh.normals;
      newMesh.colors = originMesh.colors;
      newMesh.tangents = originMesh.tangents;
    }

    /// <summary>Get native array of mesh vertices.</summary>
    /// <param name="meshData">mesh data</param>
    /// <param name="allocator">allocation type</param>
    public static void NativeGetVertices(
      in Mesh.MeshData meshData,
      out NativeArray<float3> na_vertices, Allocator allocator
    )
    {
      int vertexCount = meshData.vertexCount;
      NativeArray<Vector3> na_verticesVec3 = new NativeArray<Vector3>(vertexCount, allocator);
      meshData.GetVertices(na_verticesVec3);
      na_vertices = na_verticesVec3.Reinterpret<float3>();
    }

    /// <summary>Get native array of mesh normals.</summary>
    /// <param name="meshData">mesh data</param>
    /// <param name="allocator">allocation type</param>
    public static void NativeGetNormals(
      in Mesh.MeshData meshData,
      out NativeArray<float3> na_normals, Allocator allocator)
    {
      int indexCount = meshData.vertexCount;
      NativeArray<Vector3> na_normalsVec3 = new NativeArray<Vector3>(indexCount, allocator);
      meshData.GetNormals(na_normalsVec3);
      na_normals = na_normalsVec3.Reinterpret<float3>();
    }
    
    /// <summary>Get native array of triangle indices.</summary>
    /// /// <param name="meshData">mesh data</param>
    /// <param name="allocator">allocation type</param>
    public static void NativeGetIndices(
      in Mesh.MeshData meshData,
      out NativeArray<int> na_triangles, Allocator allocator, int submesh=0)
    {
      int indexCount = meshData.GetSubMesh(submesh).indexCount;
      na_triangles = new NativeArray<int>(indexCount, allocator);
      meshData.GetIndices(na_triangles, submesh);
    }

    public static void SeparateTriangles(
      ref Vector3[] verts,
      ref ushort[] indices,
      ref Color[] colors,
      ref Vector2[] uvs,
      out Mesh mesh
    )
    {
      int totalTris = indices.Length/3;
      Vector3[] newVerts = new Vector3[indices.Length];
      ushort[] newIndices = new ushort[indices.Length];
      Color[] newColors = new Color[indices.Length];
      Vector2[] newUvs = new Vector2[indices.Length];

      for (int t=0; t < totalTris; t++)
      {
        int t0 = indices[t*3];
        int t1 = indices[t*3 + 1];
        int t2 = indices[t*3 + 2];

        newVerts[t*3] = verts[t0];
        newVerts[t*3 + 1] = verts[t1];
        newVerts[t*3 + 2] = verts[t2];

        newColors[t*3] = colors[t0];
        newColors[t*3 + 1] = colors[t1];
        newColors[t*3 + 2] = colors[t2];

        newUvs[t*3] = uvs[t0];
        newUvs[t*3 + 1] = uvs[t1];
        newUvs[t*3 + 2] = uvs[t2];
      }

      for (ushort i=0; i < indices.Length; i++) newIndices[i] = i;

      mesh = new Mesh();
      mesh.SetVertices(newVerts);
      mesh.SetIndices(newIndices, MeshTopology.Triangles, 0);
      mesh.SetColors(newColors);
      mesh.SetUVs(0, newUvs);
    }
  }
}
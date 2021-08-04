/*
This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software Foundation,
Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301, USA.

The Original Code is Copyright (C) 2020 Voxell Technologies.
All rights reserved.
*/

using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;

namespace Voxell
{
  public static class MeshUtil
  {
    /// <summary>
    /// Creates a deep copy of a mesh
    /// </summary>
    /// <param name="originMesh">source of mesh to copy from</param>
    /// <param name="newMesh">location of copied mesh</param>
    public static void DeepCopyMesh(ref Mesh originMesh, out Mesh newMesh)
    {
      newMesh = new Mesh();
      newMesh.vertices = originMesh.vertices;
      newMesh.triangles = originMesh.triangles;
      newMesh.uv = originMesh.uv;
      newMesh.normals = originMesh.normals;
      newMesh.colors = originMesh.colors;
      newMesh.tangents = originMesh.tangents;
    }

    /// <summary>
    /// Get native array of mesh vertices
    /// </summary>
    /// <param name="meshData">mesh data</param>
    /// <param name="allocator">allocation type</param>
    /// <returns></returns>
    public static NativeArray<float3> NativeGetVertices(in Mesh.MeshData meshData, Allocator allocator)
    {
      int vertexCount = meshData.vertexCount;
      var vertices = new NativeArray<float3>(vertexCount, allocator);
      meshData.GetVertices(vertices.Reinterpret<Vector3>());
      return vertices;
    }

    /// <summary>
    /// Get native array of mesh normals
    /// </summary>
    /// <param name="meshData">mesh data</param>
    /// <param name="allocator">allocation type</param>
    /// <returns></returns>
    public static NativeArray<float3> NativeGetNormals(in Mesh.MeshData meshData, Allocator allocator)
    {
      int indexCount = meshData.vertexCount;
      var normals = new NativeArray<float3>(indexCount, allocator);
      meshData.GetNormals(normals.Reinterpret<Vector3>());
      return normals;
    }
    
    /// <summary>
    /// Get native array of triangle indices
    /// </summary>
    /// /// <param name="meshData">mesh data</param>
    /// <param name="allocator">allocation type</param>
    public static NativeArray<int> NativeGetIndices(in Mesh.MeshData meshData, Allocator allocator, int submesh=0)
    {
      int indexCount = meshData.GetSubMesh(submesh).indexCount;
      var triangles = new NativeArray<int>(indexCount, allocator);
      meshData.GetIndices(triangles, submesh);
      return triangles;
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
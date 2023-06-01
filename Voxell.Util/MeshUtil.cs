using System.Runtime.CompilerServices;
using UnityEngine.Rendering;
using Unity.Mathematics;
using Unity.Collections;

namespace Voxell
{
    using UnityEngine;
    public static class MeshUtil
    {
        /// <summary>VertexAttributeFormat size in bytes.</summary>
        /// <remarks>Refer to: https://docs.unity3d.com/ScriptReference/Rendering.VertexAttributeFormat.html</remarks>
        private static readonly uint[] VertexAttributeFormatSize = new uint[]
        {
            // 8 bytes per bit:
            32u / 8u, // float32
            16u / 8u, // float16
            8u  / 8u, // unorm8
            8u  / 8u, // snorm8
            16u / 8u, // unorm16
            16u / 8u, // snorm16
            8u  / 8u, // uint8
            8u  / 8u, // sint8
            16u / 8u, // uint16
            16u / 8u, // sint16
            32u / 8u, // uint32
            32u / 8u, // sint32
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint GetVertexAttributeFormatSize(VertexAttributeFormat format)
        {
            return VertexAttributeFormatSize[(int)format];
        }

        /// <summary>Get native array of mesh vertices.</summary>
        /// <param name="meshData">mesh data</param>
        /// <param name="allocator">allocation type</param>
        public static NativeArray<float3> GetNativeVertices(in Mesh.MeshData meshData, Allocator allocator)
        {
            int vertexCount = meshData.vertexCount;
            NativeArray<Vector3> na_vertices = new NativeArray<Vector3>(vertexCount, allocator);
            meshData.GetVertices(na_vertices);
            return na_vertices.Reinterpret<float3>();
        }

        /// <summary>Get native array of mesh normals.</summary>
        /// <param name="meshData">mesh data</param>
        /// <param name="allocator">allocation type</param>
        public static NativeArray<float3> GetNativeNormals(in Mesh.MeshData meshData, Allocator allocator)
        {
            int vertexCount = meshData.vertexCount;
            NativeArray<Vector3> na_normals = new NativeArray<Vector3>(vertexCount, allocator);
            meshData.GetNormals(na_normals);
            return na_normals.Reinterpret<float3>();
        }

        /// <summary>Get native array of mesh normals.</summary>
        /// <param name="meshData">mesh data</param>
        /// <param name="allocator">allocation type</param>
        public static NativeArray<float4> GetNativeTangents(in Mesh.MeshData meshData, Allocator allocator)
        {
            int vertexCount = meshData.vertexCount;
            NativeArray<Vector4> na_tangents = new NativeArray<Vector4>(vertexCount, allocator);
            meshData.GetTangents(na_tangents);
            return na_tangents.Reinterpret<float4>();
        }

        /// <summary>Get native array of uv coordinates.</summary>
        /// <param name="meshData">mesh data</param>
        /// <param name="allocator">allocation type</param>
        public static NativeArray<float2> GetNativeUVs(in Mesh.MeshData meshData, int channel, Allocator allocator)
        {
            int vertexCount = meshData.vertexCount;
            NativeArray<Vector2> na_vertices = new NativeArray<Vector2>(vertexCount, allocator);
            meshData.GetUVs(channel, na_vertices);
            return na_vertices.Reinterpret<float2>();
        }

        /// <summary>Get native array of vertex colors.</summary>
        /// <param name="meshData">mesh data</param>
        /// <param name="allocator">allocation type</param>
        public static NativeArray<float4> GetNativeColors(in Mesh.MeshData meshData, Allocator allocator)
        {
            int vertexCount = meshData.vertexCount;
            NativeArray<Color> na_colors = new NativeArray<Color>(vertexCount, allocator);
            meshData.GetColors(na_colors);
            return na_colors.Reinterpret<float4>();
        }

        /// <summary>Get native array of triangle indices.</summary>
        /// <param name="meshData">mesh data</param>
        /// <param name="allocator">allocation type</param>
        public static NativeArray<int> GetNativeIndices(in Mesh.MeshData meshData, Allocator allocator, int submesh=0)
        {
            int indexCount = meshData.GetSubMesh(submesh).indexCount;
            NativeArray<int> na_triangles = new NativeArray<int>(indexCount, allocator);
            meshData.GetIndices(na_triangles, submesh);
            return na_triangles;
        }

        /// <summary>Get native array of triangle indices.</summary>
        /// <param name="meshData">mesh data</param>
        /// <param name="allocator">allocation type</param>
        public static NativeArray<int> GetAllNativeIndices(in Mesh.MeshData meshData, Allocator allocator)
        {
            NativeList<int> na_trianglesList = new NativeList<int>(Allocator.Temp);

            for (int sm = 0; sm < meshData.subMeshCount; sm++)
            {
                int indexCount = meshData.GetSubMesh(sm).indexCount;

                NativeArray<int> na_subTriangles = new NativeArray<int>(indexCount, Allocator.Temp);
                meshData.GetIndices(na_subTriangles, sm);

                na_trianglesList.AddRange(na_subTriangles);
            }

            NativeArray<int> na_triangles = new NativeArray<int>(na_trianglesList.Length, allocator, NativeArrayOptions.UninitializedMemory);
            na_triangles.CopyFrom(na_trianglesList.AsArray());
            return na_triangles;
        }

        /// <summary>Creates a deep copy of a mesh.</summary>
        /// <param name="originMesh">source of mesh to copy from</param>
        /// <param name="targetMesh">location of copied mesh</param>
        public static Mesh DeepCopyMesh(in Mesh originMesh)
        {
            Mesh targetMesh = new Mesh();
            targetMesh.vertices = originMesh.vertices;
            targetMesh.triangles = originMesh.triangles;
            targetMesh.uv = originMesh.uv;
            targetMesh.normals = originMesh.normals;
            targetMesh.colors = originMesh.colors;
            targetMesh.tangents = originMesh.tangents;

            int subMeshCount = originMesh.subMeshCount;
            targetMesh.subMeshCount = subMeshCount;
            for (int s = 0; s < subMeshCount; s++)
                targetMesh.SetSubMesh(s, originMesh.GetSubMesh(s));

            return targetMesh;
        }

        // TODO: turn this into job based
        /// <summary>
        /// Reverse the triangle order of the mesh to flip the mesh
        /// </summary>
        /// <param name="mesh">mesh to be reversed</param>
        public static void ReverseTriangleOrder(ref Mesh mesh)
        {
            for (int m = 0; m < mesh.subMeshCount; m++)
            {
                int[] triangles = mesh.GetTriangles(m);
                for (int t = 0; t < triangles.Length; t += 3)
                {
                    int temp = triangles[t + 0];
                    triangles[t + 0] = triangles[t + 1];
                    triangles[t + 1] = temp;
                }
                mesh.SetTriangles(triangles, m);
            }

            Vector3[] normals = mesh.normals;
            for (int n = 0; n < normals.Length; n++) normals[n] = -normals[n];
            mesh.SetNormals(normals);
        }
    }
}

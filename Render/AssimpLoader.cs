using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;
using OpenTK.Mathematics;
using static System.Formats.Asn1.AsnWriter;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Match3Example.Render
{
    static class AssimpLoader
    {
        public static float[] GetMeshFromFile(string filepath)
        {
            List<float> mesh = new List<float>();

            AssimpContext context = new AssimpContext();
            Assimp.Scene scene = context.ImportFile(filepath, PostProcessSteps.FlipUVs | PostProcessSteps.Triangulate);

            Vector3D[] vertices = Array.Empty<Vector3D>();
            Vector3D[] normals = Array.Empty<Vector3D>();
            Vector3D[] UV = Array.Empty<Vector3D>();
            int[] indies = Array.Empty<int>();

            if (scene.MeshCount > 0)
            {
                vertices = scene.Meshes[0].Vertices.ToArray();
                normals = scene.Meshes[0].Normals.ToArray();
                UV = scene.Meshes[0].HasTextureCoords(0) ? scene.Meshes[0].TextureCoordinateChannels[0].ToArray() : UV;
                indies = scene.Meshes[0].GetIndices();
            }
            else
            {
                throw new Exception("No meshes in file");
            }

            for (int i = 0; i < indies.Length; i++)
            {
                int index = indies[i];
                Vector3D ver = vertices[index];
                Vector3D nor = normals[index];
                Vector3D uv = UV[index];

                mesh.Add(ver.X);
                mesh.Add(ver.Y);
                mesh.Add(ver.Z);

                mesh.Add(nor.X);
                mesh.Add(nor.Y);
                mesh.Add(nor.Z);

                mesh.Add(uv.X);
                mesh.Add(uv.Y);
            }

            return mesh.ToArray();
        }
    }
}

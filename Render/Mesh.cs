using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Match3Example.Render
{
    internal class Mesh
    {
        private float[] vertices;

        private int VBO;
        private int VAO;

        public Mesh(float[] vertices)
        {
            this.vertices = vertices;

            InitializeVBO();
        }

        void InitializeVBO()
        {
            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            //VERTEX POSITION
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            //VERTEX NORMAL
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            //VERTEX UV
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
            GL.EnableVertexAttribArray(2);
        }

        public void Render(Shader shader, Transforms transforms)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BindVertexArray(VAO);

            shader.SetUniformMatix4("model", transforms.GetModelMatrix());

            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length / 8);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3Example
{
    internal class Element
    {
        public int ID;
        public Texture texture;
        public Mesh mesh;

        public Element(Mesh mesh, Texture texture)
        {
            this.mesh = mesh;
            this.texture = texture;
        }

        public void Render(Shader shader, Transforms transforms)
        {
            texture.Use(shader);
            mesh.Render(shader, transforms);
        }
    }
}

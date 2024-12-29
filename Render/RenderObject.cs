using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3Example.Render
{
    internal class RenderObject
    {
        public Transforms transforms;
        public Mesh mesh;
        public Texture texture;

        public RenderObject(Mesh mesh, Texture texture, Transforms transforms)
        {
            this.mesh = mesh;
            this.texture = texture;
            this.transforms = transforms;
        }

        public virtual void Render(Shader shader)
        {
            texture.Use(shader);
            mesh.Render(shader, transforms);
        }
    }
}

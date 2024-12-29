using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3Example
{
    internal class Element
    {
        private static int _elementsCount = 0;
        public static int elementsCount {  get { return _elementsCount; } }

        public int ID;
        public Texture texture;
        public Mesh mesh;

        public Element(Mesh mesh, Texture texture)
        {
            ID = _elementsCount;
            _elementsCount++;
            this.mesh = mesh;
            this.texture = texture;
        }

        public void Render(Shader shader, Transforms transforms)
        {
            texture.Use(shader);
            mesh.Render(shader, transforms);
        }

        public static void ResetElementsCount()
        {
            _elementsCount = 0;
        }
    }
}

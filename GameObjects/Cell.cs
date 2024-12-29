using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;
using Match3Example.Render;

namespace Match3Example.GameObjects
{
    internal class Cell
    {
        public Element element = null;
        public Transforms transforms;

        public Cell(Vector3 position)
        {
            transforms = new Transforms(position, Vector3.Zero, Vector3.One * 0.7f);
        }

        public Cell(Vector3 position, Element element)
        {
            transforms = new Transforms(position, Vector3.Zero, Vector3.One * 0.7f);
            this.element = element;
        }

        public void Render(Shader shader)
        {
            if (element == null)
                return;

            element.Render(shader, transforms);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Match3Example.Inputs
{
    static class MouseInput
    {
        public static Vector2 MousePosition
        {
            get { return Viewport.Instance.MousePosition; }
        }

        public static Vector2 NormalizedMousePosition
        {
            get
            {
                float mouseNDC_X = MousePosition.X * 2 / Viewport.Instance.ClientSize.X - 1;
                float mouseNDC_Y = 1 - MousePosition.Y * 2 / Viewport.Instance.ClientSize.Y;
                return new Vector2(mouseNDC_X, mouseNDC_Y);
            }
        }

        public static MouseState state
        {
            get
            {
                return Viewport.Instance.MouseState;
            }
        }
    }
}

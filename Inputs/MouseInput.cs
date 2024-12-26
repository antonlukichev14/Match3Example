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

        public static Vector3 ScreenToCameraOrtWorldPosition(Camera camera, float z_posiiton)
        {
            Vector3 mouseNDC = new Vector3(NormalizedMousePosition.X, NormalizedMousePosition.Y, z_posiiton);

            Matrix4 invProjectionView = Matrix4.Invert(camera.projection * camera.view);

            Vector4 clipCoords = new Vector4(mouseNDC.X, mouseNDC.Y, -1.0f, 1.0f);
            Vector4 worldCoords = invProjectionView * clipCoords;
            worldCoords /= worldCoords.W;

            Vector3 worldPosition = new Vector3(worldCoords.X, worldCoords.Y, worldCoords.Z);
            return worldPosition;
        }
    }
}

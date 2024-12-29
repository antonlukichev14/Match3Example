using Match3Example.Render;
using Match3Example.Scenes;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3Example
{
    internal class Camera
    {
        private Scene cameraScene;

        private Matrix4 _projection;
        private float _fov;

        public Vector3 position = new Vector3(0f, 0f, 0f);
        private Vector3 front = new Vector3(0.0f, -1.0f, 0.0f);
        private Vector3 up = new Vector3(0.0f, 0.0f, 1.0f);

        public Camera(Scene scene, float fov)
        {
            cameraScene = scene;
            this.fov = fov;
        }

        public Matrix4 view
        {
            get
            {
                return GetView();
            }
        }
        public Matrix4 projection
        {
            get
            {
                return _projection;
            }
        }
        public float fov
        {
            get
            {
                return _fov;
            }
            set
            {
                _fov = value;
                SetProjection();
            }
        }

        public void SetProjection()
        {
            Vector2 clientSize = new Vector2((float)Viewport.Instance.ClientSize.X, (float)Viewport.Instance.ClientSize.Y);
            clientSize.Normalize();
            _projection = Matrix4.CreateOrthographic((float)clientSize.X * fov, (float)clientSize.Y * fov, -50f, 50.0f);
        }

        private Matrix4 GetView()
        {
            return Matrix4.LookAt(position, position + front, up);
        }

        public void Use(Shader shader)
        {
            shader.SetUniformMatix4("view", view);
            shader.SetUniformMatix4("projection", projection);
        }

        public float noclipSpeed = 5.0f;

        public void Move(Vector2 velocity)
        {
            position += up * velocity.X * noclipSpeed * (float)cameraScene.deltaTime;
            position -= Vector3.Normalize(Vector3.Cross(front, up)) * noclipSpeed * velocity.Y * (float)cameraScene.deltaTime;
        }

        public static Vector2 WorldToScreenPosition(Camera camera, Vector3 worldPosition)
        {
            Vector4 clipCoords = new Vector4(worldPosition, 1.0f) * camera.view * camera.projection;

            Vector3 ndc = new Vector3(clipCoords.X / clipCoords.W, clipCoords.Y / clipCoords.W, clipCoords.Z / clipCoords.W);
            Vector2 screenPosition = new Vector2((ndc.X + 1) / 2 * Viewport.Instance.ClientSize.X, (ndc.Y + 1) / 2 * Viewport.Instance.ClientSize.Y);

            return screenPosition;
        }
    }
}

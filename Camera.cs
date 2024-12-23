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
        private Matrix4 _projection;
        private float _fov;

        public Vector3 position = new Vector3(0f, 0f, 0f);
        private Vector3 front = new Vector3(0.0f, -1.0f, 0.0f);
        private Vector3 up = new Vector3(0.0f, 0.0f, 1.0f);

        public Camera(float fov)
        {
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
            Vector2 clientSize = new Vector2((float)Game.instance.ClientSize.X, (float)Game.instance.ClientSize.Y);
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
            position += up * velocity.X * noclipSpeed * Game.instance.deltaTime;
            position -= Vector3.Normalize(Vector3.Cross(front, up)) * noclipSpeed * velocity.Y * Game.instance.deltaTime;
        }
    }
}

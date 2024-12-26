using Assimp;
using Match3Example.Inputs;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3Example
{
    internal class Collider2DAABB
    {
        public Vector2 ColliderVertical;
        public Vector2 ColliderHorizontal;

        public Collider2DAABB(Vector2 ColliderPosition, Vector2 ColliderScale) 
        {
            ColliderVertical = new Vector2(ColliderPosition.Y + ColliderScale.Y, ColliderPosition.Y - ColliderScale.Y);
            ColliderHorizontal = new Vector2(ColliderPosition.X + ColliderScale.X, ColliderPosition.X - ColliderScale.X);
        }

        public Collider2DAABB(float up, float down, float left, float right)
        {
            ColliderVertical.X = up;
            ColliderVertical.Y = down;
            ColliderHorizontal.X = left;
            ColliderHorizontal.Y = right;
        }

        public void Render()
        {
            throw new NotImplementedException();
        }

        public bool PointCollison(Vector2 point)
        {
            if (point.X < ColliderHorizontal.X && point.X > ColliderHorizontal.Y && point.Y < ColliderVertical.X && point.Y > ColliderVertical.Y)
                return true;
            return false;
        }

        public bool ScreenPointCollison(Camera camera, Vector2 point)
        {
            Vector2 ScreenAABBpoint1 = Camera.WorldToScreenPosition(camera, new Vector3(ColliderHorizontal.X, 0, ColliderVertical.X));
            Vector2 ScreenAABBpoint2 = Camera.WorldToScreenPosition(camera, new Vector3(ColliderHorizontal.Y, 0, ColliderVertical.Y));

            if (point.X < ScreenAABBpoint2.X && point.X > ScreenAABBpoint1.X && point.Y < ScreenAABBpoint1.Y && point.Y > ScreenAABBpoint2.Y)
                return true;
            return false;
        }

        public Vector2 GetScreenAABBpoint(Camera camera, int index)
        {
            switch (index)
            {
                case 0:
                    return Camera.WorldToScreenPosition(camera, new Vector3(ColliderHorizontal.X, 0, ColliderVertical.X));
                    break;
                case 1:
                    return Camera.WorldToScreenPosition(camera, new Vector3(ColliderHorizontal.Y, 0, ColliderVertical.Y));
                    break;
                case 2:
                    return Camera.WorldToScreenPosition(camera, new Vector3(ColliderHorizontal.X, 0, ColliderVertical.Y));
                    break;
                case 3:
                    return Camera.WorldToScreenPosition(camera, new Vector3(ColliderHorizontal.Y, 0, ColliderVertical.X));
                    break;
                default:
                    throw new Exception();
                    break;
            }
        }
    }
}

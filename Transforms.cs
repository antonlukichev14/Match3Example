﻿using OpenTK.Mathematics;

namespace Match3Example
{
    class Transforms
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;

        public Transforms()
        {
            position = Vector3.Zero;
            rotation = Vector3.Zero;
            scale = Vector3.One;
        }

        public Transforms(Vector3 position)
        {
            this.position = position;
            rotation = Vector3.Zero;
            scale = Vector3.One;
        }

        public Transforms(Vector3 position, Vector3 rotation)
        {
            this.position = position;
            this.rotation = rotation;
            scale = Vector3.One;
        }

        public Transforms(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }

        public Matrix4 GetModelMatrix()
        {
            Matrix4 matrixscale = Matrix4.CreateScale(scale.X, scale.Y, scale.Z);

            Matrix4 matrixposition = Matrix4.CreateTranslation(position.X, position.Y, position.Z);

            return matrixscale * matrixposition * CreateRotationMatrixFromEuler(rotation);
        }

        public static Matrix4 CreateRotationMatrixFromEuler(Vector3 eulerAngles)
        {
            // Получаем углы в радианах
            float pitch = MathHelper.DegreesToRadians(eulerAngles.X); // Вращение вокруг оси X
            float yaw = MathHelper.DegreesToRadians(eulerAngles.Y);   // Вращение вокруг оси Y
            float roll = MathHelper.DegreesToRadians(eulerAngles.Z);  // Вращение вокруг оси Z

            // Создаем матрицы поворота
            Matrix4 rotationX = Matrix4.CreateRotationX(pitch);
            Matrix4 rotationY = Matrix4.CreateRotationY(yaw);
            Matrix4 rotationZ = Matrix4.CreateRotationZ(roll);

            // Объединяем матрицы
            return rotationZ * rotationY * rotationX;
        }

        static public Transforms Default() { return new Transforms(); }
    }
}

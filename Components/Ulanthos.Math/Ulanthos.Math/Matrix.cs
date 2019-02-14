using System;
using System.IO;
using System.Runtime.InteropServices;
using Ulanthos.Helpers;

namespace Ulanthos.Math
{
    /// <summary>
    /// Defines a 4x4 matrix.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    public struct Matrix : IEquatable<Matrix>
    {
        #region Properties

        /// <summary>
        /// First row, First column.
        /// </summary>
        [FieldOffset(0)]
        public float M11;

        /// <summary>
        /// First row, Second column.
        /// </summary>
        [FieldOffset(1)]
        public float M12;

        /// <summary>
        /// First row, Third column.
        /// </summary>
        [FieldOffset(2)]
        public float M13;

        /// <summary>
        /// First row, Forth column.
        /// </summary>
        [FieldOffset(3)]
        public float M14;

        /// <summary>
        /// Second row, First column.
        /// </summary>
        [FieldOffset(4)]
        public float M21;

        /// <summary>
        /// Second row, Second column.
        /// </summary>
        [FieldOffset(5)]
        public float M22;

        /// <summary>
        /// Second row, Third column.
        /// </summary>
        [FieldOffset(6)]
        public float M23;

        /// <summary>
        /// Second row, Forth column.
        /// </summary>
        [FieldOffset(7)]
        public float M24;

        /// <summary>
        /// Third row, First column.
        /// </summary>
        [FieldOffset(8)]
        public float M31;

        /// <summary>
        /// Third row, Second column.
        /// </summary>
        [FieldOffset(9)]
        public float M32;

        /// <summary>
        /// Third row, Third column.
        /// </summary>
        [FieldOffset(10)]
        public float M33;

        /// <summary>
        /// Third row, Forth column.
        /// </summary>
        [FieldOffset(11)]
        public float M34;

        /// <summary>
        /// Forth row, First column.
        /// </summary>
        [FieldOffset(12)]
        public float M41;

        /// <summary>
        /// Forth row, Second column.
        /// </summary>
        [FieldOffset(13)]
        public float M42;

        /// <summary>
        /// Forth row, Third column.
        /// </summary>
        [FieldOffset(14)]
        public float M43;

        /// <summary>
        /// Forth row, Forth column.
        /// </summary>
        [FieldOffset(15)]
        public float M44;

        /// <summary>
        /// The translation amount for the Matrix.
        /// </summary>
        public Vector3 Translation
        {
            get { return new Vector3(M41, M42, M43); }
            set
            {
                M41 = value.X;
                M42 = value.Y;
                M43 = value.Z;
            }
        }

        /// <summary>
        /// The scaling amount for the vector.
        /// </summary>
        public Vector3 Scale
        {
            get { return new Vector3(M11, M22, M33); }
            set
            {
                M11 = value.X;
                M22 = value.Y;
                M33 = value.Z;
            }
        }

        /// <summary>
        /// The forward values of the Matrix as a Vector.
        /// </summary>
        public Vector3 Forward { get { return new Vector3(M31, M32, M33); } }

        /// <summary>
        /// The up values of the Matrix as a Vector.
        /// </summary>
        public Vector3 Up { get { return new Vector3(M21, M22, M23); } }

        /// <summary>
        /// The right values of the Matrix as a Vector.
        /// </summary>
        public Vector3 Right { get { return new Vector3(M11, M12, M13); } }

        /// <summary>
        /// Returns the determinant of the Matrix. 
        /// </summary>
        public float Determinant
        {
            get
            {
                float m44x33m43x34 = (M44 * M33) - (M43 * M34);
                float m32x44m42x34 = (M32 * M44) - (M42 * M34);
                float m32x43m42x33 = (M32 * M43) - (M42 * M33);
                float m31x44m41x34 = (M31 * M44) - (M41 * M34);
                float m31x43m41x33 = (M31 * M43) - (M41 * M33);
                float m31x42m41x32 = (M31 * M42) - (M41 * M32);
                return (((((((M22 * m44x33m43x34) - (M23 * m32x44m42x34)) +
                            (M24 * m32x43m42x33)) * M11) - ((((M21 * m44x33m43x34) -
                                                              (M23 * m31x44m41x34)) +
                                                             (M24 * m31x43m41x33)) * M12)) +
                         ((((M21 * m32x44m42x34) - (M22 * m31x44m41x34)) +
                           (M24 * m31x42m41x32)) * M13)) - ((((M21 * m32x43m42x33) -
                                                              (M22 * m31x43m41x33)) +
                                                             (M23 * m31x42m41x32)) * M14));
            }
        }

        /// <summary>
        /// Returns the inverse of this Matrix.
        /// </summary>
        public Matrix Inverse
        {
            get
            {
                float m33x44m34x43 = (M33 * M44) - (M34 * M43);
                float m32x44m34x42 = (M32 * M44) - (M34 * M42);
                float m32x43m33x42 = (M32 * M43) - (M33 * M42);
                float m31x44m34x41 = (M31 * M44) - (M34 * M41);
                float m31x43m33x41 = (M31 * M43) - (M33 * M41);
                float m31x42m32x41 = (M31 * M42) - (M32 * M41);
                float m22m23m24 = ((M22 * m33x44m34x43) - (M23 * m32x44m34x42)) +
                                  (M24 * m32x43m33x42);
                float m21m23m24 = -(((M21 * m33x44m34x43) - (M23 * m31x44m34x41)) +
                                    (M24 * m31x43m33x41));
                float m21m22m24 = ((M21 * m32x44m34x42) - (M22 * m31x44m34x41)) +
                                  (M24 * m31x42m32x41);
                float m21m22m23 = -(((M21 * m32x43m33x42) - (M22 * m31x43m33x41)) +
                                    (M23 * m31x42m32x41));
                float inverseDet = 1f / ((((M11 * m22m23m24) + (M12 * m21m23m24)) +
                                          (M13 * m21m22m24)) + (M14 * m21m22m23));
                float m23x44m24x43 = (M23 * M44) - (M24 * M43);
                float m22x44m24x42 = (M22 * M44) - (M24 * M42);
                float m22x43m23x42 = (M22 * M43) - (M23 * M42);
                float m21x44m24x41 = (M21 * M44) - (M24 * M41);
                float m21x43m23x41 = (M21 * M43) - (M23 * M41);
                float m21x42m22x41 = (M21 * M42) - (M22 * M41);
                float m23x34m24x33 = (M23 * M34) - (M24 * M33);
                float m22x34m24x32 = (M22 * M34) - (M24 * M32);
                float m22x33m23x32 = (M22 * M33) - (M23 * M32);
                float m21x34m23x31 = (M21 * M34) - (M24 * M31);
                float m21x33m23x31 = (M21 * M33) - (M23 * M31);
                float m21x32m22x31 = (M21 * M32) - (M22 * M31);

                return new Matrix(m22m23m24 * inverseDet,
                    -(((M12 * m33x44m34x43) - (M13 * m32x44m34x42)) +
                      (M14 * m32x43m33x42)) * inverseDet,
                    (((M12 * m23x44m24x43) - (M13 * m22x44m24x42)) +
                     (M14 * m22x43m23x42)) * inverseDet,
                    -(((M12 * m23x34m24x33) - (M13 * m22x34m24x32)) +
                      (M14 * m22x33m23x32)) * inverseDet,
                    m21m23m24 * inverseDet,
                    (((M11 * m33x44m34x43) - (M13 * m31x44m34x41)) +
                     (M14 * m31x43m33x41)) * inverseDet,
                    -(((M11 * m23x44m24x43) - (M13 * m21x44m24x41)) +
                      (M14 * m21x43m23x41)) * inverseDet,
                    (((M11 * m23x34m24x33) - (M13 * m21x34m23x31)) +
                     (M14 * m21x33m23x31)) * inverseDet,
                    m21m22m24 * inverseDet,
                    -(((M11 * m32x44m34x42) - (M12 * m31x44m34x41)) +
                      (M14 * m31x42m32x41)) * inverseDet,
                    (((M11 * m22x44m24x42) - (M12 * m21x44m24x41)) +
                     (M14 * m21x42m22x41)) * inverseDet,
                    -(((M11 * m22x34m24x32) - (M12 * m21x34m23x31)) +
                      (M14 * m21x32m22x31)) * inverseDet,
                    m21m22m23 * inverseDet,
                    (((M11 * m32x43m33x42) - (M12 * m31x43m33x41)) +
                     (M13 * m31x42m32x41)) * inverseDet,
                    -(((M11 * m22x43m23x42) - (M12 * m21x43m23x41)) +
                      (M13 * m21x42m22x41)) * inverseDet,
                    (((M11 * m22x33m23x32) - (M12 * m21x33m23x31)) +
                     (M13 * m21x32m22x31)) * inverseDet);
            }
        }

        #endregion
        #region Constructers
        /// <summary>
        /// Creates a new Matrix.
        /// </summary>
        /// <param name="m11">First Row, First Column.</param>
        /// <param name="m12">First Row, Second Column.</param>
        /// <param name="m13">First Row, Third Column.</param>
        /// <param name="m14">First Row, Forth Column.</param>
        /// <param name="m21">Second Row, First Column.</param>
        /// <param name="m22">Second Row, Second Column.</param>
        /// <param name="m23">Second Row, Third Column.</param>
        /// <param name="m24">Second Row, Forth Column.</param>
        /// <param name="m31">Third Row, First Column.</param>
        /// <param name="m32">Third Row, Second Column.</param>
        /// <param name="m33">Third Row, Third Column.</param>
        /// <param name="m34">Third Row, Forth Column.</param>
        /// <param name="m41">Forth Row, First Column.</param>
        /// <param name="m42">Forth Row, Second Column.</param>
        /// <param name="m43">Forth Row, Third Column.</param>
        /// <param name="m44">Forth Row, Forth Column.</param>
        public Matrix(float m11, float m12, float m13, float m14,
            float m21, float m22, float m23, float m24,
            float m31, float m32, float m33, float m34,
            float m41, float m42, float m43, float m44)
        {
            M11 = m11;
            M12 = m12;
            M13 = m13;
            M14 = m14;
            M21 = m21;
            M22 = m22;
            M23 = m23;
            M24 = m24;
            M31 = m31;
            M32 = m32;
            M33 = m33;
            M34 = m34;
            M41 = m41;
            M42 = m42;
            M43 = m43;
            M44 = m44;
        }

        /// <summary>
        /// Creates a new Matrix. 
        /// </summary>
        /// <param name="firstRow">First row values as a Vector3.</param>
        /// <param name="secondRow">Second row values as a Vector3.</param>
        /// <param name="thirdRow">Third row values as a Vector3.</param>
        public Matrix(Vector3 firstRow, Vector3 secondRow, Vector3 thirdRow) :
            this(firstRow.X, firstRow.Y, firstRow.Z, 0, secondRow.X, secondRow.Y,
            secondRow.Z, 0, thirdRow.X, thirdRow.Y, thirdRow.Z, 0, 0, 0, 0, 1) { }

        /// <summary>
        /// Creates a new Matrix.
        /// </summary>
        /// <param name="valuesArray">The array to use to get values from. Should be of lenght 16.</param>
        public Matrix(float[] valuesArray)
        {
            if (valuesArray.Length < 16)
                throw new ArgumentOutOfRangeException("Array given in is to small.");
            else if (valuesArray.Length > 16)
                throw new ArgumentOutOfRangeException("Array given in is to big.");
            else
            {
                M11 = valuesArray[0];
                M12 = valuesArray[1];
                M13 = valuesArray[2];
                M14 = valuesArray[3];
                M21 = valuesArray[4];
                M22 = valuesArray[5];
                M23 = valuesArray[6];
                M24 = valuesArray[7];
                M31 = valuesArray[8];
                M32 = valuesArray[9];
                M33 = valuesArray[10];
                M34 = valuesArray[11];
                M41 = valuesArray[12];
                M42 = valuesArray[13];
                M43 = valuesArray[14];
                M44 = valuesArray[15];
            }
        }

        /// <summary>
        /// Creates a new Matrix.
        /// </summary>
        /// <param name="reader">The BinaryReader to use.</param>
        public Matrix(BinaryReader reader)
            : this()
        {
            Load(reader);
        }
        #endregion
        #region Defaults

        /// <summary>
        /// Returns the identity matrix.
        /// </summary>
        public static readonly Matrix Identity = new Matrix
            (1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);

        /// <summary>
        /// Returns a matrix with a value of Zero.
        /// </summary>
        public static readonly Matrix Zero = new Matrix
            (0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

        #endregion
        #region Creation
        #region Main

        /// <summary>
        /// Creates a Matrix from yaw, pitch, roll values.
        /// </summary>
        /// <param name="yaw">Rotation in the Y-axis.</param>
        /// <param name="pitch">Rotation in the X-axis.</param>
        /// <param name="roll">Rotation in the Z-axis.</param>
        /// <returns>The generated Matrix from the given values.</returns>
        public static Matrix CreateFromYawPitchRoll(float yaw, float pitch, float roll)
        {
            float cosYaw = MathHelper.Cos(yaw);
            float sinYaw = MathHelper.Sin(yaw);
            float cosPitch = MathHelper.Cos(pitch);
            float sinPitch = MathHelper.Sin(pitch);
            float cosRoll = MathHelper.Cos(roll);
            float sinRoll = MathHelper.Sin(roll);

            return new Matrix
            (
                cosYaw * cosRoll - sinRoll * sinYaw * sinPitch, cosYaw * sinRoll + sinYaw * sinPitch * cosRoll, -sinYaw * cosPitch, 0,
                -sinRoll * cosPitch, cosPitch * cosRoll, sinPitch, 0,
                sinYaw * cosRoll + sinPitch * cosYaw * sinRoll, sinYaw * sinRoll - sinPitch * cosYaw * cosRoll, cosYaw * cosPitch, 0,
                0, 0, 0, 1
            );
        }

        /// <summary>
        /// Creates a Matrix from yaw, pitch, roll values.
        /// </summary>
        /// <param name="value">The Vector3 to get the values from.</param>
        /// Y = Yaw.
        /// X = Pitch.
        /// Z = Roll.
        /// <returns>The generated Matrix from the given values.</returns>
        public static Matrix CreateFromYawPitchRoll(Vector3 value)
        {
            return CreateFromYawPitchRoll(value.Y, value.X, value.Z);
        }

        /// <summary>
        /// Creates a new Matrix from a Quaternion.
        /// </summary>
        /// <param name="value">The Quaternion to use.</param>
        /// <returns>The generated Matrix.</returns>
        public static Matrix CreateFromQuaternion(Quaternion value)
        {
            float qxx = value.X * value.X;
            float qyy = value.Y * value.Y;
            float qzz = value.Z * value.Z;
            float qxy = value.X * value.Y;
            float qzw = value.Z * value.W;
            float qwx = value.Z * value.X;
            float qyw = value.Y * value.W;
            float qyz = value.Y * value.Z;
            float qxw = value.X * value.W;

            return new Matrix(1f - (2f * (qyy + qzz)), 2f * (qxy + qzw), 2f * (qwx - qyw), 0f,
                2f * (qxy - qzw), 1f - (2f * (qzz + qxx)), 2f * (qyz + qxw), 0f,
                2f * (qwx + qyw), 2f * (qyz - qxw), 1f - (2f * (qyy + qxx)), 0f,
                0f, 0f, 0f, 1f);
        }

        /// <summary>
        /// Creates a look at Matrix.
        /// </summary>
        /// <param name="cameraPosition">The position of the camera.</param>
        /// <param name="cameraTarget">The camera's target.</param>
        /// <param name="up">The camera's up Vector3.</param>
        /// <returns>Look at Matrix.</returns>
        public static Matrix CreateLookAt(Vector3 cameraPosition,
            Vector3 cameraTarget, Vector3 up)
        {
            Vector3 direct = Vector3.Normalize(cameraPosition - cameraTarget);
            Vector3 upDirect = Vector3.Normalize(Vector3.Cross(up, direct));

            if (upDirect.LenghtSquared == 0)
                upDirect = Vector3.UnitY;

            Vector3 right = Vector3.Cross(direct, upDirect);

            return new Matrix(upDirect.X, right.X, direct.X, 0,
                up.Y, right.Y, direct.Y, 0, up.Z, right.Z, direct.Z, 0,
                -Vector3.Dot(upDirect, cameraPosition), -Vector3.Dot(right, cameraPosition),
                -Vector3.Dot(direct, cameraPosition), 1);
        }

        /// <summary>
        /// Creates a perspective field of view Matrix.
        /// </summary>
        /// <param name="fieldOfView">The field of view in radians.</param>
        /// <param name="aspectRatio">The aspect ratio to use.</param>
        /// <param name="nearPlane">The near plane distance.</param>
        /// <param name="farPlane">The far plane distance.</param>
        /// <returns>Perspective field of view matrix.</returns>
        public static Matrix CreatePerspectiveFeildOfView(float fieldOfView,
            float aspectRatio, float nearPlane, float farPlane)
        {
            float m22 = 1 / (float)System.Math.Tan(fieldOfView / 2);

            return new Matrix(m22 / aspectRatio, 0, 0, 0, 0, m22, 0, 0,
                0, 0, farPlane / (nearPlane - farPlane), -1, 0, 0,
                (nearPlane * farPlane) / (nearPlane - farPlane), 0);
        }

        /// <summary>
        /// Creates a new Orthographic projection Matrix.
        /// </summary>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        /// <param name="up">Up.</param>
        /// <param name="down">Down.</param>
        /// <param name="nearPlane">Near plane distance.</param>
        /// <param name="farPlane">Far plane distance.</param>
        /// <returns>A Matrix with value to represent orthographic projection.</returns>
        public static Matrix CreateOrthographicProjection(float left, float right,
            float up, float down, float nearPlane, float farPlane)
        {
            Matrix ortho = Zero;

            ortho.M11 = 2 / (right - left);
            ortho.M22 = 2 / (up - down);
            ortho.M33 = 1 / (nearPlane - farPlane);
            ortho.M41 = (left + right) / (left - right);
            ortho.M42 = (up + down) / (down - up);
            ortho.M43 = nearPlane / (nearPlane - farPlane);
            ortho.M44 = 1;

            return ortho;
        }

        /// <summary>
        /// Creates a Matrix with both Translation and scale values.
        /// </summary>
        /// <param name="position">The position to translate.</param>
        /// <param name="scale">The amount to scale by.</param>
        /// <returns>The generated matrix from the given values.</returns>
        public static Matrix CreateTranslationScale(Vector3 position, float scale)
        {
            return new Matrix(scale, 0, 0, 0,
                0, scale, 0, 0,
                0, 0, scale, 0,
                position.X, position.Y, position.Z, 1);
        }

        /// <summary>
        /// Creates a Matrix with both Translation and scale values.
        /// </summary>
        /// <param name="position">The position to translate.</param>
        /// <param name="scale">The amount to scale by.</param>
        /// <returns>The generated matrix from the given values.</returns>
        public static Matrix CreateTranslationScale(Vector3 position, Vector3 scale)
        {
            return new Matrix(scale.X, 0, 0, 0,
                0, scale.Y, 0, 0,
                0, 0, scale.Z, 0,
                position.X, position.Y, position.Z, 1);
        }

        #endregion
        #region Translation

        /// <summary>
        /// Creates a translation Matrix.
        /// </summary>
        /// <param name="x">X co-ordinates.</param>
        /// <param name="y">Y co-ordinates.</param>
        /// <param name="z">Z co-ordinates.</param>
        /// <returns>Translation Matrix made from the given position.</returns>
        public static Matrix CreateTranslation(float x, float y, float z)
        {
            return new Matrix(1, 0, 0, 0,
                            0, 1, 0, 0,
                            0, 0, 1, 0,
                            x, y, z, 1);
        }

        /// <summary>
        /// Creates a translation Matrix.
        /// </summary>
        /// <param name="position">Position for translation.</param>
        /// <returns>Translation Matrix made from the given position.</returns>
        public static Matrix CreateTranslation(Vector3 position)
        {
            return CreateTranslation(position.X, position.Y, position.Z);
        }

        #endregion
        #region Rotation

        /// <summary>
        /// Creates a Matrix with rotation values along the X axis.
        /// </summary>
        /// <param name="degrees">The amount to rotate by in degrees.</param>
        /// <returns>A new matrix with rotation spacific values.</returns>
        public static Matrix CreateXAxisRotation(float degrees)
        {
            float cos = MathHelper.Cos(degrees);
            float sin = MathHelper.Sin(degrees);

            return new Matrix
                (1, 0, 0, 0,
                0, cos, sin, 0,
                0, -sin, cos, 0,
                0, 0, 0, 1);
        }

        /// <summary>
        /// Creates a Matrix with rotation values along the Y axis.
        /// </summary>
        /// <param name="degrees">The amount to rotate by in degrees.</param>
        /// <returns>A new matrix with rotation spacific values.</returns>
        public static Matrix CreateYAxisRotation(float degrees)
        {
            float cos = MathHelper.Cos(degrees);
            float sin = MathHelper.Sin(degrees);

            return new Matrix
                (cos, 0, -sin, 0,
                0, 1, 0, 0,
                sin, 0, cos, 0,
                0, 0, 0, 1);
        }

        /// <summary>
        /// Creates a Matrix with rotation values along the Z axis.
        /// </summary>
        /// <param name="degrees">The amount to rotate by in degrees.</param>
        /// <returns>A new matrix with rotation spacific values.</returns>
        public static Matrix CreateZAxisRotation(float degrees)
        {
            float cos = MathHelper.Cos(degrees);
            float sin = MathHelper.Sin(degrees);

            return new Matrix
                (cos, sin, 0, 0,
                -sin, cos, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1);
        }

        /// <summary>
        /// Creates a Matrix that has rotation values for all axis. 
        /// </summary>
        /// <param name="xAmount">The amount ot roatate by in the X-axis.</param>
        /// <param name="yAmount">The amount ot roatate by in the Y-axis.</param>
        /// <param name="zAmount">The amount ot roatate by in the Z-axis.</param>
        /// <returns>The generated Matrix.</returns>
        public static Matrix AllAxisRotation(float xAmount, float yAmount, float zAmount)
        {
            float cosX = 1;
            float sinX = 0;
            float cosY = 1;
            float sinY = 0;
            float cosZ = 1;
            float sinZ = 0;

            if (xAmount != 0)
            {
                cosX = MathHelper.Cos(xAmount);
                sinX = MathHelper.Sin(xAmount);
            }

            if (yAmount != 0)
            {
                cosY = MathHelper.Cos(yAmount);
                sinY = MathHelper.Sin(yAmount);
            }

            if (zAmount != 0)
            {
                cosZ = MathHelper.Cos(zAmount);
                sinZ = MathHelper.Sin(zAmount);
            }

            return new Matrix
            (
                cosY * cosZ, cosY * sinZ, -sinY, 0,
                (sinX * sinY * cosZ) + (cosX * -sinZ), (sinX * sinY * sinZ) + (cosX * cosZ), sinX * cosY, 0,
                (cosX * sinY * cosZ) + (sinX * -sinZ), (cosX * sinY * sinZ) + (-sinX * cosZ), cosX * cosY, 0,
                0, 0, 0, 1
            );
        }

        /// <summary>
        /// Creates a Matrix that has rotation values for all axis. 
        /// </summary>
        /// <param name="amount">The amount to scale by.</param>
        /// <returns>The generated Matrix.</returns>
        public static Matrix AllAxisRotation(Vector3 amount)
        {
            float cosX = 1;
            float sinX = 0;
            float cosY = 1;
            float sinY = 0;
            float cosZ = 1;
            float sinZ = 0;

            if (amount.X != 0)
            {
                cosX = MathHelper.Cos(amount.X);
                sinX = MathHelper.Sin(amount.X);
            }

            if (amount.Y != 0)
            {
                cosY = MathHelper.Cos(amount.Y);
                sinY = MathHelper.Sin(amount.Y);
            }

            if (amount.Z != 0)
            {
                cosZ = MathHelper.Cos(amount.Z);
                sinZ = MathHelper.Sin(amount.Z);
            }

            return new Matrix
            (
                cosY * cosZ, cosY * sinZ, -sinY, 0,
                (sinX * sinY * cosZ) + (cosX * -sinZ), (sinX * sinY * sinZ) + (cosX * cosZ), sinX * cosY, 0,
                (cosX * sinY * cosZ) + (sinX * -sinZ), (cosX * sinY * sinZ) + (-sinX * cosZ), cosX * cosY, 0,
                0, 0, 0, 1
            );
        }

        #endregion
        #region Scale

        /// <summary>
        /// Creates a scale Matrix.
        /// </summary>
        /// <param name="xAmount">The amount of scaling in the X-axis.</param>
        /// <param name="yAmount">The amount of scaling in the Y-axis.</param>
        /// <param name="zAmount">The amount of scaling in the Z-axis.</param>
        /// <returns>A new Matrix with values spacific for scaling.</returns>
        public static Matrix CreateScale(float xAmount, float yAmount, float zAmount)
        {
            return new Matrix(xAmount, 0, 0, 0,
                0, yAmount, 0, 0,
                0, 0, zAmount, 0,
                0, 0, 0, 1);
        }

        /// <summary>
        /// Creates a scale Matrix.
        /// </summary>
        /// <param name="amount">The amount to uniformally scale by.</param>
        /// <returns>A new Matrix with values spacific for scaling.</returns>
        public static Matrix CreateScale(float amount)
        {
            return CreateScale(amount, amount, amount);
        }

        /// <summary>
        /// Creates a scale Matrix.
        /// </summary>
        /// <param name="scale">The amount to scale by.</param>
        /// <returns>A new Matrix with values spacific for scaling.</returns>
        public static Matrix CreateScale(Vector3 scale)
        {
            return CreateScale(scale.X, scale.Y, scale.Z);
        }

        #endregion
        #endregion
        #region String Related

        /// <summary>
        /// Creates a Matrix from a string.
        /// </summary>
        /// <param name="value">The string to use.</param>
        /// <returns>The generated Matrix.</returns>
        public static Matrix CreateFromString(string value)
        {
            value = value.Replace("(", "").Replace(")", "");

            string[] values = StringHelper.SplitRemove(value, ",");

            if (values.Length == 16)
            {
                return new Matrix
                    (
                        StringHelper.StringToType<float>(values[0], 1f),
                        StringHelper.StringToType<float>(values[1], 1f),
                        StringHelper.StringToType<float>(values[2], 1f),
                        StringHelper.StringToType<float>(values[3], 1f),

                        StringHelper.StringToType<float>(values[4], 1f),
                        StringHelper.StringToType<float>(values[5], 1f),
                        StringHelper.StringToType<float>(values[6], 1f),
                        StringHelper.StringToType<float>(values[7], 1f),

                        StringHelper.StringToType<float>(values[8], 1f),
                        StringHelper.StringToType<float>(values[9], 1f),
                        StringHelper.StringToType<float>(values[10], 1f),
                        StringHelper.StringToType<float>(values[11], 1f),

                        StringHelper.StringToType<float>(values[12], 1f),
                        StringHelper.StringToType<float>(values[13], 1f),
                        StringHelper.StringToType<float>(values[14], 1f),
                        StringHelper.StringToType<float>(values[15], 1f)
                    );
            }

            return Identity;
        }

        #endregion
        #region Other

        /// <summary>
        /// Translates this Matrix by a specified amount in 2D space.
        /// </summary>
        /// <param name="amount">The amount to translate the Matrix by.</param>
        public void Translate(Vector2 amount)
        {
            M41 += amount.X * M11 + amount.Y * M21;
            M42 += amount.X * M12 + amount.Y * M22;
        }

        /// <summary>
        /// Switches rows with columns.
        /// </summary>
        public void Transpose()
        {
            Matrix matrix = this;

            matrix.M12 = this.M12;
            matrix.M13 = this.M31;
            matrix.M14 = this.M41;
            matrix.M21 = this.M12;
            matrix.M23 = this.M32;
            matrix.M24 = this.M42;
            matrix.M31 = this.M13;
            matrix.M32 = this.M23;
            matrix.M34 = this.M43;
            matrix.M41 = this.M14;
            matrix.M42 = this.M24;
            matrix.M43 = this.M34;
        }

        /// <summary>
        /// Switches rows with columns.
        /// </summary>
        /// <param name="matrix">The Matrix to be Transposed.</param>
        /// <returns>The result of the Transpose.</returns>
        public static Matrix Transpose(Matrix matrix)
        {
            return new Matrix(matrix.M11, matrix.M12, matrix.M31,
            matrix.M41, matrix.M12, matrix.M22,
            matrix.M32, matrix.M42, matrix.M13,
            matrix.M23, matrix.M33, matrix.M43,
            matrix.M14, matrix.M24, matrix.M34, matrix.M44);
        }

        /// <summary>
        /// Switches rows with columns.
        /// </summary>
        /// <param name="matrix">The Matrix to be Transposed.</param>
        /// <param name="result">The result of the Transpose.</param>
        public static void Transpose(ref Matrix matrix, out Matrix result)
        {
            result.M11 = matrix.M11;
            result.M12 = matrix.M12;
            result.M13 = matrix.M31;
            result.M14 = matrix.M41;
            result.M21 = matrix.M12;
            result.M22 = matrix.M22;
            result.M23 = matrix.M32;
            result.M24 = matrix.M42;
            result.M31 = matrix.M13;
            result.M32 = matrix.M23;
            result.M33 = matrix.M33;
            result.M34 = matrix.M43;
            result.M41 = matrix.M14;
            result.M42 = matrix.M24;
            result.M43 = matrix.M34;
            result.M44 = matrix.M44;
        }

        /// <summary>
        /// Gets a row as a Vector3 at an index.
        /// </summary>
        /// <param name="index">The index to get the row at.</param>
        /// <returns>The row at that index.</returns>
        public Vector3 GetRow(int index)
        {
            if (index == 0)
                return Right;
            else if (index == 1)
                return Up;
            else if (index == 2)
                return Forward;
            else if (index == 3)
                return Translation;
            else
                throw new IndexOutOfRangeException(string.Format("Max index = 3, This index = {0}", index));
        }

        /// <summary>
        /// A check to see it the translations of the two Matrices are equal.
        /// </summary>
        /// <param name="value1">Matrix 1.</param>
        /// <param name="value2">Matrix 2.</param>
        /// <returns>The result of the check.</returns>
        public static bool TranslationEqualEnough(ref Matrix value1, ref Matrix value2)
        {
            return ((value1.Translation - value2.Translation).LenghtSquared <
                MathHelper.Epsilon * MathHelper.Epsilon);
        }

        /// <summary>
        /// Return true if the two Matrices are nearly equal.
        /// </summary>
        /// <param name="value1">The matrix to check aginst.</param>
        /// <param name="value2">The Matrix to check.</param>
        /// <returns>The result of the check.</returns>
        public bool EqualEnough(ref Matrix value1, ref Matrix value2)
        {
            return (MathHelper.EqualEnough(value1.M11, value2.M11) &&
                    MathHelper.EqualEnough(value1.M12, value2.M12) &&
                    MathHelper.EqualEnough(value1.M13, value2.M13) &&
                    MathHelper.EqualEnough(value1.M14, value2.M14) &&

                    MathHelper.EqualEnough(value1.M21, value2.M21) &&
                    MathHelper.EqualEnough(value1.M22, value2.M22) &&
                    MathHelper.EqualEnough(value1.M23, value2.M23) &&
                    MathHelper.EqualEnough(value1.M24, value2.M24) &&

                    MathHelper.EqualEnough(value1.M31, value2.M31) &&
                    MathHelper.EqualEnough(value1.M32, value2.M32) &&
                    MathHelper.EqualEnough(value1.M33, value2.M33) &&
                    MathHelper.EqualEnough(value1.M34, value2.M34) &&

                    MathHelper.EqualEnough(value1.M41, value2.M41) &&
                    MathHelper.EqualEnough(value1.M42, value2.M42) &&
                    MathHelper.EqualEnough(value1.M43, value2.M43) &&
                    MathHelper.EqualEnough(value1.M44, value2.M44));
        }

        /// <summary>
        /// Return true if the two Matrices aren't equal.
        /// </summary>
        /// <param name="value1">The matrix to check aginst.</param>
        /// <param name="value2">The Matrix to check.</param>
        /// <returns>The result of the check.</returns>
        public static bool NotEqual(ref Matrix value1, ref Matrix value2)
        {
            if (value1.M11 == value2.M11 && value1.M12 == value2.M12 &&
                value1.M13 == value2.M13 && value1.M14 == value2.M14 &&
                value1.M21 == value2.M21 && value1.M22 == value2.M22 &&
                value1.M23 == value2.M23 && value1.M24 == value2.M24 &&
                value1.M31 == value2.M31 && value1.M32 == value2.M32 &&
                value1.M33 == value2.M33 && value1.M34 == value2.M34 &&
                value1.M41 == value2.M41 && value1.M42 == value2.M42 &&
                value1.M43 == value2.M43 && value1.M44 == value2.M44)
                return false;
            else
                return true;
        }

        #endregion
        #region Base Methods

        /// <summary>
        /// A check to see if two Matrixes are equal. 
        /// </summary>
        /// <param name="obj">The object to check.</param>
        /// <returns>The Result.</returns>
        public override bool Equals(object obj)
        {
            return (obj is Matrix) && Equals((Matrix)obj);
        }

        /// <summary>
        /// A check to see if two Matrixes are equal. 
        /// </summary>
        /// <param name="other">The other Matrix.</param>
        /// <returns>The Result.</returns>
        public bool Equals(Matrix other)
        {
            return this == other;
        }

        /// <summary>
        /// Saves this Matrix.
        /// </summary>
        /// <param name="writer">The writer to use.</param>
        public void Save(BinaryWriter writer)
        {
            writer.Write(M11);
            writer.Write(M12);
            writer.Write(M13);
            writer.Write(M14);

            writer.Write(M21);
            writer.Write(M22);
            writer.Write(M23);
            writer.Write(M24);

            writer.Write(M31);
            writer.Write(M32);
            writer.Write(M33);
            writer.Write(M34);

            writer.Write(M41);
            writer.Write(M42);
            writer.Write(M43);
            writer.Write(M44);
        }

        /// <summary>
        /// Loads in a Matrix.
        /// </summary>
        /// <param name="reader">The BinaryReader to use.</param>
        public void Load(BinaryReader reader)
        {
            M11 = reader.ReadSingle();
            M12 = reader.ReadSingle();
            M13 = reader.ReadSingle();
            M14 = reader.ReadSingle();

            M21 = reader.ReadSingle();
            M22 = reader.ReadSingle();
            M23 = reader.ReadSingle();
            M24 = reader.ReadSingle();

            M31 = reader.ReadSingle();
            M32 = reader.ReadSingle();
            M33 = reader.ReadSingle();
            M34 = reader.ReadSingle();

            M41 = reader.ReadSingle();
            M42 = reader.ReadSingle();
            M43 = reader.ReadSingle();
            M44 = reader.ReadSingle();
        }

        /// <summary>
        /// Gets the hash code for this Matrix.
        /// </summary>
        /// <returns>The Matrixes hash code.</returns>
        public override int GetHashCode()
        {
            return (M11.GetHashCode() ^ M12.GetHashCode() ^ M13.GetHashCode() ^ M14.GetHashCode() ^
                M21.GetHashCode() ^ M22.GetHashCode() ^ M23.GetHashCode() ^ M24.GetHashCode() ^
                M31.GetHashCode() ^ M32.GetHashCode() ^ M33.GetHashCode() ^ M34.GetHashCode() ^
                M41.GetHashCode() ^ M42.GetHashCode() ^ M43.GetHashCode() ^ M44.GetHashCode());
        }

        /// <summary>
        /// This Matrix as a string that can be use to load it in.
        /// </summary>
        /// <returns>This Matrix as a string.</returns>
        public override string ToString()
        {
            string format = "0.00000";

            return ('(' +
                StringHelper.NumberToString(M11, format) + ", " +
                StringHelper.NumberToString(M12, format) + ", " +
                StringHelper.NumberToString(M13, format) + ", " +
                StringHelper.NumberToString(M14, format) + ", " +
                StringHelper.NumberToString(M21, format) + ", " +
                StringHelper.NumberToString(M22, format) + ", " +
                StringHelper.NumberToString(M23, format) + ", " +
                StringHelper.NumberToString(M24, format) + ", " +
                StringHelper.NumberToString(M31, format) + ", " +
                StringHelper.NumberToString(M32, format) + ", " +
                StringHelper.NumberToString(M33, format) + ", " +
                StringHelper.NumberToString(M34, format) + ", " +
                StringHelper.NumberToString(M41, format) + ", " +
                StringHelper.NumberToString(M42, format) + ", " +
                StringHelper.NumberToString(M43, format) + ", " +
                StringHelper.NumberToString(M44, format) + ')');
        }

        #endregion
        #region Operators

        /// <summary>
        /// A check to see if two Matrixes are equal.
        /// </summary>
        /// <param name="value1">Matrix 1.</param>
        /// <param name="value2">Matrix 2.</param>
        /// <returns>The result of the check.</returns>
        public static bool operator ==(Matrix value1, Matrix value2)
        {
            return (value1.M11 == value2.M11 && value1.M12 == value2.M12 &&
                value1.M13 == value2.M13 && value1.M14 == value2.M14 &&
                value1.M21 == value2.M21 && value1.M22 == value2.M22 &&
                value1.M23 == value2.M23 && value1.M24 == value2.M24 &&
                value1.M31 == value2.M31 && value1.M32 == value2.M32 &&
                value1.M33 == value2.M33 && value1.M34 == value2.M34 &&
                value1.M41 == value2.M41 && value1.M42 == value2.M42 &&
                value1.M43 == value2.M43 && value1.M44 == value2.M44);
        }

        /// <summary>
        /// A check to see if two Matrixes aren't equal.
        /// </summary>
        /// <param name="value1">Matrix 1.</param>
        /// <param name="value2">Matrix 2.</param>
        /// <returns>The result of the check.</returns>
        public static bool operator !=(Matrix value1, Matrix value2)
        {
            return (value1.M11 != value2.M11 || value1.M12 != value2.M12 ||
                value1.M13 != value2.M13 || value1.M14 != value2.M14 ||
                value1.M21 != value2.M21 || value1.M22 != value2.M22 ||
                value1.M23 != value2.M23 || value1.M24 != value2.M24 ||
                value1.M31 != value2.M31 || value1.M32 != value2.M32 ||
                value1.M33 != value2.M33 || value1.M34 != value2.M34 ||
                value1.M41 != value2.M41 || value1.M42 != value2.M42 ||
                value1.M43 != value2.M43 || value1.M44 != value2.M44);
        }

        /// <summary>
        /// Returns the negative of the Matrix.
        /// </summary>
        /// <param name="value">The value to negate.</param>
        /// <returns>The negitive of the given Matrix.</returns>
        public static Matrix operator -(Matrix value)
        {
            return new Matrix
            (
                -value.M11, -value.M12, -value.M13, -value.M14,
                -value.M21, -value.M22, -value.M23, -value.M24,
                -value.M31, -value.M32, -value.M33, -value.M34,
                -value.M41, -value.M42, -value.M43, -value.M44
            );
        }

        /// <summary>
        /// Adds two Matrices.
        /// </summary>
        /// <param name="value1">Matrix 1.</param>
        /// <param name="value2">Matrix 2.</param>
        /// <returns>The result of the addition.</returns>
        public static Matrix operator +(Matrix value1, Matrix value2)
        {
            return new Matrix
            (
                value1.M11 + value2.M11, value1.M12 + value2.M12,
                value1.M13 + value2.M13, value1.M14 + value2.M14,

                value1.M21 + value2.M21, value1.M22 + value2.M22,
                value1.M23 + value2.M23, value1.M24 + value2.M24,

                value1.M31 + value2.M31, value1.M32 + value2.M32,
                value1.M33 + value2.M33, value1.M34 + value2.M34,

                value1.M41 + value2.M41, value1.M42 + value2.M42,
                value1.M43 + value2.M43, value1.M44 + value2.M44
            );
        }

        /// <summary>
        /// Subtracts two Matrices.
        /// </summary>
        /// <param name="value1">Matrix 1.</param>
        /// <param name="value2">Matrix 2.</param>
        /// <returns>The result of the subtraction.</returns>
        public static Matrix operator -(Matrix value1, Matrix value2)
        {
            return new Matrix
            (
                value1.M11 - value2.M11, value1.M12 - value2.M12,
                value1.M13 - value2.M13, value1.M14 - value2.M14,

                value1.M21 - value2.M21, value1.M22 - value2.M22,
                value1.M23 - value2.M23, value1.M24 - value2.M24,

                value1.M31 - value2.M31, value1.M32 - value2.M32,
                value1.M33 - value2.M33, value1.M34 - value2.M34,

                value1.M41 - value2.M41, value1.M42 - value2.M42,
                value1.M43 - value2.M43, value1.M44 - value2.M44
            );
        }

        /// <summary>
        /// Multiplies two Matrices.
        /// </summary>
        /// <param name="value1">Matrix 1.</param>
        /// <param name="value2">Matrix 2.</param>
        /// <returns>The result of the multiplication.</returns>
        public static Matrix operator *(Matrix value1, Matrix value2)
        {
            if (value2 == Identity)
                return value1;
            else
                return new Matrix
                ((value1.M11 * value2.M11) + (value1.M12 * value2.M21) +
                (value1.M13 * value2.M31) + (value1.M14 * value2.M41),
                (value1.M11 * value2.M12) + (value1.M12 * value2.M22) +
                (value1.M13 * value2.M32) + (value1.M14 * value2.M42),
                (value1.M11 * value2.M13) + (value1.M12 * value2.M23) +
                (value1.M13 * value2.M33) + (value1.M14 * value2.M43),
                (value1.M11 * value2.M14) + (value1.M12 * value2.M24) +
                (value1.M13 * value2.M34) + (value1.M14 * value2.M44),

                (value1.M21 * value2.M11) + (value1.M22 * value2.M21) +
                (value1.M23 * value2.M31) + (value1.M24 * value2.M41),
                (value1.M21 * value2.M12) + (value1.M22 * value2.M22) +
                (value1.M23 * value2.M32) + (value1.M24 * value2.M42),
                (value1.M21 * value2.M13) + (value1.M22 * value2.M23) +
                (value1.M23 * value2.M33) + (value1.M24 * value2.M43),
                (value1.M21 * value2.M14) + (value1.M22 * value2.M24) +
                (value1.M23 * value2.M34) + (value1.M24 * value2.M44),

                (value1.M31 * value2.M11) + (value1.M32 * value2.M21) +
                (value1.M33 * value2.M31) + (value1.M34 * value2.M41),
                (value1.M31 * value2.M12) + (value1.M32 * value2.M22) +
                (value1.M33 * value2.M32) + (value1.M34 * value2.M42),
                (value1.M31 * value2.M13) + (value1.M32 * value2.M23) +
                (value1.M33 * value2.M33) + (value1.M34 * value2.M43),
                (value1.M31 * value2.M14) + (value1.M32 * value2.M24) +
                (value1.M33 * value2.M34) + (value1.M34 * value2.M44),

                (value1.M41 * value2.M11) + (value1.M42 * value2.M21) +
                (value1.M43 * value2.M31) + (value1.M44 * value2.M41),
                (value1.M41 * value2.M12) + (value1.M42 * value2.M22) +
                (value1.M43 * value2.M32) + (value1.M44 * value2.M42),
                (value1.M41 * value2.M13) + (value1.M42 * value2.M23) +
                (value1.M43 * value2.M33) + (value1.M44 * value2.M43),
                (value1.M41 * value2.M14) + (value1.M42 * value2.M24) +
                (value1.M43 * value2.M34) + (value1.M44 * value2.M44));
        }

        /// <summary>
        /// Multiplies a Matrix by a Vector3.
        /// </summary>
        /// <param name="value1">The Matrix to use.</param>
        /// <param name="value2">The Vector3 to use.</param>
        /// <returns>The result of the Multiplication.</returns>
        public static Vector3 operator *(Matrix value1, Vector3 value2)
        {
            return new Vector3
            (
                (value2.X * value1.M11) + (value2.Y * value1.M21) +
                (value2.Z * value1.M31) + value1.M41,

                (value2.X * value1.M12) + (value2.Y * value1.M22) +
                (value2.Z * value1.M32) + value1.M42,

                (value2.X * value1.M13) + (value2.Y * value1.M23) +
                (value2.Z * value1.M33) + value1.M43
            );
        }

        /// <summary>
        /// Multiplies a Matrix by a float.
        /// </summary>
        /// <param name="value1">The Matrix to use.</param>
        /// <param name="amount">The float to use.</param>
        /// <returns>The result of the Multiplication.</returns>
        public static Matrix operator *(Matrix value, float amount)
        {
            return new Matrix
            (
                value.M11 * amount, value.M12 * amount,
                value.M13 * amount, value.M14 * amount,
                value.M21 * amount, value.M22 * amount,
                value.M23 * amount, value.M24 * amount,
                value.M31 * amount, value.M32 * amount,
                value.M33 * amount, value.M34 * amount,
                value.M41 * amount, value.M42 * amount,
                value.M43 * amount, value.M44 * amount
            );
        }

        /// <summary>
        /// Multiplies a Matrix by a float.
        /// </summary>
        /// <param name="value1">The Matrix to use.</param>
        /// <param name="amount">The float to use.</param>
        /// <returns>The result of the Multiplication.</returns>
        public static Matrix operator *(float amount, Matrix value)
        {
            return new Matrix
            (
                value.M11 * amount, value.M12 * amount,
                value.M13 * amount, value.M14 * amount,
                value.M21 * amount, value.M22 * amount,
                value.M23 * amount, value.M24 * amount,
                value.M31 * amount, value.M32 * amount,
                value.M33 * amount, value.M34 * amount,
                value.M41 * amount, value.M42 * amount,
                value.M43 * amount, value.M44 * amount
            );
        }

        /// <summary>
        /// Divides a Matrix by another.
        /// </summary>
        /// <param name="value1">The Matrix to use.</param>
        /// <param name="value2">The other Matrix to use.</param>
        /// <returns>The result of the Division.</returns>
        public static Matrix operator /(Matrix value1, Matrix value2)
        {
            return new Matrix
            (
                value1.M11 / value2.M11, value1.M12 / value2.M12,
                value1.M13 / value2.M13, value1.M14 / value2.M14,
                value1.M21 / value2.M21, value1.M22 / value2.M22,
                value1.M23 / value2.M23, value1.M24 / value2.M24,
                value1.M31 / value2.M31, value1.M32 / value2.M32,
                value1.M33 / value2.M33, value1.M34 / value2.M34,
                value1.M41 / value2.M41, value1.M42 / value2.M42,
                value1.M43 / value2.M43, value1.M44 / value2.M44
            );
        }

        /// <summary>
        /// Divides a Matrix by a float.
        /// </summary>
        /// <param name="value">The Matrix to use.</param>
        /// <param name="amount">The float to use.</param>
        /// <returns>The result of the Division.</returns>
        public static Matrix operator /(Matrix value, float amount)
        {
            return new Matrix
            (
                value.M11 / amount, value.M12 / amount,
                value.M13 / amount, value.M14 / amount,
                value.M21 / amount, value.M22 / amount,
                value.M23 / amount, value.M24 / amount,
                value.M31 / amount, value.M32 / amount,
                value.M33 / amount, value.M34 / amount,
                value.M41 / amount, value.M42 / amount,
                value.M43 / amount, value.M44 / amount
            );
        }

        /// <summary>
        /// Divides a Matrix by a float.
        /// </summary>
        /// <param name="value">The Matrix to use.</param>
        /// <param name="amount">The float to use.</param>
        /// <returns>The result of the Division.</returns>
        public static Matrix operator /(float amount, Matrix value)
        {
            return new Matrix
            (
                value.M11 / amount, value.M12 / amount,
                value.M13 / amount, value.M14 / amount,
                value.M21 / amount, value.M22 / amount,
                value.M23 / amount, value.M24 / amount,
                value.M31 / amount, value.M32 / amount,
                value.M33 / amount, value.M34 / amount,
                value.M41 / amount, value.M42 / amount,
                value.M43 / amount, value.M44 / amount
            );
        }

        #endregion
    }
}

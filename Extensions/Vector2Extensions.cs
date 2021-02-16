using UnityEngine;

namespace SnUnityCommonUtils.Extensions
{
    public static class Vector2Extensions
    {
        #region Set

        public static Vector3 SetX(this Vector2 vector, float x)
        {
            return new Vector3(x, vector.y);
        }

        public static Vector3 SetY(this Vector2 vector, float y)
        {
            return new Vector3(vector.x, y);
        }

        public static Vector3 SetZ(this Vector2 vector, float z)
        {
            return new Vector3(vector.x, vector.y, z);
        }

        #endregion
    }
}
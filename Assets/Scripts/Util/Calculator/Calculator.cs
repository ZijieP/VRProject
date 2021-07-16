using UnityEngine;
using System.Collections.Generic;
using System;
// using Sirenix.OdinInspector;
namespace Calculator3D
{
    public static class Calculator
    {
        /// <summary>
        /// Point to straight line distance
        /// </summary>
        /// <param name="point">Point coordinates</param>
        /// <param name="linePoint1">The coordinates of a point on the line</param>
        /// <param name="linePoint2">The coordinates of another point on the line</param>
        /// <returns></returns>
        public static float DisPoint2Line(Vector3 point, Vector3 linePoint1, Vector3 linePoint2)
        {
            Vector3 vec1 = point - linePoint1;
            Vector3 vec2 = linePoint2 - linePoint1;
            Vector3 vecProj = Vector3.Project(vec1, vec2);
            float dis = Mathf.Sqrt(Mathf.Pow(Vector3.Magnitude(vec1), 2) - Mathf.Pow(Vector3.Magnitude(vecProj), 2));
            return dis;
        }

        public static Vector3 ProjectLineOnPlane(Vector3 linePoint1, Vector3 linePoint2, Vector3 normalVectorOnPlane)
        {
            Vector3 line = linePoint1 - linePoint2;
            return Vector3.ProjectOnPlane(line, normalVectorOnPlane);
        }



    }
}
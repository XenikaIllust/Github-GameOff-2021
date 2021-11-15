using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils
{
    public static Vector2[] GenerateIsometricCirclePoints(float radius)
    {
        List<Vector2> points = new List<Vector2>();
        float rotation = 0;
        int smoothness = 20;
        
        float ang = 0;
        float o = rotation * Mathf.Deg2Rad;

        float radiusX = radius * 0.816f;
        float radiusY = radius * 0.816f / 2;

        for (int i = 0; i <= smoothness; i++)
        {
            float a = ang * Mathf.Deg2Rad;
            float x = radiusX * Mathf.Cos(a) * Mathf.Cos(o) - radiusY * Mathf.Sin(a) * Mathf.Sin(o);
            float y = -radiusX * Mathf.Cos(a) * Mathf.Sin(o) - radiusY * Mathf.Sin(a) * Mathf.Cos(o);

            points.Add(new Vector2(x, y));
            ang += 360f/smoothness;
        }

        points.RemoveAt(0);
        return points.ToArray();
    }
}

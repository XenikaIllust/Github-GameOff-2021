using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils
{
    public static float Mod(float a,float b)
    {
        return a - b * Mathf.Floor(a / b);
    }

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

    public static float ConvertStandardToAzimuth(float originalRotation) 
    {
        float azimuthRotation;

        if(originalRotation >= 0 && originalRotation < 90) {
            azimuthRotation = 90 - originalRotation;
        }
        else if(originalRotation >= 90 && originalRotation < 180) {
            azimuthRotation = 270 + ( 90 - ( originalRotation - 90 ) );
        }
        else if(originalRotation >= 180 && originalRotation < 270) {
            azimuthRotation = 180 + ( 90 - ( originalRotation - 180 ) );
        }
        else {
            azimuthRotation = 90 + ( 270 - (originalRotation - 90) );
        }

        return azimuthRotation;
    }

    public static void ConvertCircleToIsometricCircle(float radius, GameObject circularIndicatorObject) {
        circularIndicatorObject.transform.localScale = new Vector3(radius * 0.825f, radius * 0.5f * 0.825f, 1.0f); // this assumes the graphic is 512x512, and pixels per unit is 256!
    }
}

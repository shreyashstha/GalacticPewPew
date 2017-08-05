using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Boundary: Static class that includes the Screen boundaries. minimum & maximum (X/Y)
/// </summary>
public static class Boundary{

    // Vectors representing Screen boundaries
    private static float distance = Camera.main.transform.position.z;
    private static Vector3 botLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
    private static Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, distance));

    public static float MinimumX()
    {
        return botLeft.x;
    }

    public static float MaximumX()
    {
        return topRight.x;
    }

    public static float MinimumY()
    {
        return botLeft.y;
    }

    public static float MaximumY()
    {
        return topRight.y;
    }
}

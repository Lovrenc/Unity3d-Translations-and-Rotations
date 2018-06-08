using UnityEngine;

public static class Extensions
{
    public static Vector3 UpdateY(this Vector3 vector, float moveY)
    {
        return new Vector3(vector.x, moveY, vector.z);
    }
}
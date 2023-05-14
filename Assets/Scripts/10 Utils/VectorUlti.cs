using UnityEngine;

public static class VectorUlti
{
    public static Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad));
    }

    public static int GetAngleFromVector(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        int angle = Mathf.RoundToInt(n);

        return angle;
    }

    public static Vector3 Set(this Vector3 vector3, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x: x == null ? vector3.x : (float)x,
                           y: y == null ? vector3.y : (float)y,
                           z: z == null ? vector3.z : (float)z);
    }
}

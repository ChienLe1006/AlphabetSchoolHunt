using UnityEngine;

public static class GameObjectExtensions
{
    public static GameObject Spawn(this GameObject prefab, Vector3 worldPosition)
    {
        return ObjectPoolManager.Spawn(prefab,
            worldPosition, Quaternion.identity, prefab.transform.localScale,
            null);
    }

    public static GameObject Spawn(this GameObject prefab)
    {
        return ObjectPoolManager.Spawn(prefab,
            Vector3.zero, Quaternion.identity, prefab.transform.localScale,
            null);
    }

    public static GameObject Spawn(this GameObject prefab, Vector3 worldPosition, Transform lookAtTarget)
    {
        return ObjectPoolManager.Spawn(prefab,
            worldPosition, Quaternion.identity, prefab.transform.localScale,
            null, lookAtTarget);
    }

    public static GameObject Spawn(this GameObject prefab, Transform parent)
    {
        return ObjectPoolManager.Spawn(prefab,
            Vector3.zero, Quaternion.identity, prefab.transform.localScale,
            parent, null, true, true);
    }

    public static GameObject Spawn(this GameObject prefab,
        Vector3 position, Quaternion rotation, Vector3 scale,
        Transform parent = null,
        bool useLocalPosition = false, bool useLocalRotation = false)
    {
        return ObjectPoolManager.Spawn(prefab, position, rotation, scale,
            parent, null, useLocalPosition, useLocalRotation);
    }

    public static void Despawn(this GameObject obj, bool surpassWarning = false)
    {
        ObjectPoolManager.Kill(obj, surpassWarning);
    }
}
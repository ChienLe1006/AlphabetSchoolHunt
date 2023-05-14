using UnityEngine;

public class AutoDestructor : MonoBehaviour
{
    [SerializeField] private float timeDestroy = 1.5f;
    [SerializeField] private bool isPutToPool = true;

    private void OnEnable()
    {
        Invoke(nameof(AutoDestroy), timeDestroy);
    }

    private void AutoDestroy()
    {
        if (isPutToPool)
        {
            SimplePool.Despawn(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
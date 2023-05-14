using UnityEngine;

public class Spinner: MonoBehaviour
{
    [SerializeField] private Vector3 rotationSpeed = new Vector3(0, 120, 0);

    private Vector3 lastPosition;

    private void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
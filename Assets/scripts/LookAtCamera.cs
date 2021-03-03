using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform _cameraTransform;

    private void Start()
    {
        _cameraTransform = Camera.main.gameObject.transform;
    }
    private void FixedUpdate()
    {
        transform.LookAt(_cameraTransform);
    }
}

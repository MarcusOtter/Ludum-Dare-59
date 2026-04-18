using UnityEngine;
using UnityEngine.InputSystem;

public class FollowMouse : MonoBehaviour
{
    [SerializeField] private bool withOffset;
    private Camera _mainCamera;
    private Vector3 _offset;


    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        var mouseScreenPosition = Mouse.current.position.ReadValue();
        var mouseWorldPosition = _mainCamera.ScreenToWorldPoint(mouseScreenPosition).With(z: 0);

        transform.position = withOffset ? mouseWorldPosition - _offset : mouseWorldPosition;
    }

    private void OnEnable()
    {
        var mouseScreenPosition = Mouse.current.position.ReadValue();
        var mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition).With(z: 0);
        _offset = mouseWorldPosition - transform.position;
    }
}
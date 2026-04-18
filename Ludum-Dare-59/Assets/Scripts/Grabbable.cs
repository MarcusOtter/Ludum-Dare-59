using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class Grabbable : MonoBehaviour
{
    [SerializeField] private UnityEvent onFocusEnter;
    [SerializeField] private UnityEvent onFocusExit;
    [SerializeField] private UnityEvent onGrabEnter;
    [SerializeField] private UnityEvent onGrabExit;

    [SerializeField] private float rotationSpeed = 1f;

    private bool _isFocused;
    private bool _isGrabbed;

    private InputAction _rotateAction;

    private void Start()
    {
        _rotateAction = InputSystem.actions.FindAction("Move");
    }

    private void Update()
    {
        if (!_isFocused)
        {
            return;
        }

        transform.Rotate(Vector3.forward * (_rotateAction.ReadValue<Vector2>().x * rotationSpeed));
    }

    public void GrabEnter()
    {
        _isGrabbed = true;
        onGrabEnter?.Invoke();
    }

    public void GrabExit()
    {
        _isGrabbed = false;
        onGrabExit?.Invoke();
    }

    public void FocusEnter()
    {
        _isFocused = true;
        onFocusEnter?.Invoke();
    }

    public void FocusExit()
    {
        _isFocused = false;
        onFocusExit?.Invoke();
    }
}
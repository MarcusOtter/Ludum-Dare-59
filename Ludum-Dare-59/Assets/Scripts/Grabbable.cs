using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class Grabbable : MonoBehaviour
{
    private const float RotationSpeed = 125f;
    private const float PreciseRotationSpeed = 25f;

    [SerializeField] private UnityEvent onFocusEnter;
    [SerializeField] private UnityEvent onFocusExit;
    [SerializeField] private UnityEvent onGrabEnter;
    [SerializeField] private UnityEvent onGrabExit;

    private bool _isFocused;
    private bool _isGrabbed;

    private InputAction _preciseRotationButton;
    private InputAction _rotateAction;

    private void Start()
    {
        _rotateAction = InputSystem.actions.FindAction("Move");
        _preciseRotationButton = InputSystem.actions.FindAction("Sprint");
    }

    private void Update()
    {
        if (!_isFocused)
        {
            return;
        }

        var speed = _preciseRotationButton.IsPressed() ? PreciseRotationSpeed : RotationSpeed;
        var rotationDiff = _rotateAction.ReadValue<Vector2>().x * -1f * speed * Time.deltaTime;

        transform.Rotate(Vector3.forward * rotationDiff);
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
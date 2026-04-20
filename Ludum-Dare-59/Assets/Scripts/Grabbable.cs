using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class Grabbable : MonoBehaviour
{
    private const float RotationSpeed = 150f;
    private const float PreciseRotationSpeed = 16f;

    [SerializeField] private CustomAudioClip pickupSound;
    [SerializeField] private SpriteRenderer backgroundRenderer;
    [SerializeField] private UnityEvent onGrabEnter;
    [SerializeField] private UnityEvent onGrabExit;

    [SerializeField] [Range(0, 1)] private float addedHoverAlpha = 0.2f;
    [SerializeField] [Range(0, 1)] private float addedFocusAlpha = 0.3f;

    private float _backgroundBaseAlpha;

    private bool _isFocused;
#pragma warning disable CS0414 // Field is assigned but its value is never used
    private bool _isGrabbed;
#pragma warning restore CS0414 // Field is assigned but its value is never used
    private bool _isHovered;

    private InputAction _preciseRotationButton;
    private InputAction _rotateAction;

    private void Start()
    {
        _rotateAction = InputSystem.actions.FindAction("Move");
        _preciseRotationButton = InputSystem.actions.FindAction("Sprint");

        _backgroundBaseAlpha = backgroundRenderer.color.a;
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver)
        {
            return;
        }

        backgroundRenderer.color = backgroundRenderer.color.With(a: GetBackgroundAlpha());

        if (!_isFocused)
        {
            return;
        }

        var speed = _preciseRotationButton.IsPressed() ? PreciseRotationSpeed : RotationSpeed;
        var rotationDiff = _rotateAction.ReadValue<Vector2>().x * -1f * speed * Time.deltaTime;

        transform.Rotate(Vector3.forward * rotationDiff);
    }

    private float GetBackgroundAlpha()
    {
        var value = _backgroundBaseAlpha;
        if (_isHovered)
        {
            value += addedHoverAlpha;
        }

        if (_isFocused)
        {
            value += addedFocusAlpha;
        }

        return Mathf.Clamp01(value);
    }

    public void HoverEnter()
    {
        _isHovered = true;
    }

    public void HoverExit()
    {
        _isHovered = false;
    }

    public void GrabEnter()
    {
        if (_isGrabbed)
        {
            return;
        }

        _isGrabbed = true;
        if (pickupSound)
        {
            AudioPlayer.Instance.PlaySoundEffect(pickupSound);
        }

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
    }

    public void FocusExit()
    {
        _isFocused = false;
    }
}
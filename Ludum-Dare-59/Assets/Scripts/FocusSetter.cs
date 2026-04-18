using UnityEngine;
using UnityEngine.InputSystem;

public class FocusSetter : MonoBehaviour
{
    private static Grabbable FocusedGrabbable;

    private Grabbable _grabbed;
    private Camera _mainCamera;

    private InputAction _selectAction;

    private void Start()
    {
        _selectAction = InputSystem.actions.FindAction("Attack");
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        var mousePos = _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var hit = Physics2D.Raycast(mousePos, Vector2.zero);
        var isMousePressed = _selectAction.IsPressed();
        var isMousePressedThisFrame = _selectAction.WasPressedThisFrame();
        var isHoveringGrabbable = (bool)hit.collider;

        if (isMousePressedThisFrame)
        {
            if (!isHoveringGrabbable)
            {
                StopFocus();
            }
            else
            {
                hit.transform.TryGetComponent<Grabbable>(out var grabbable);
                StartFocus(grabbable);
            }
        }

        if (isMousePressed && isHoveringGrabbable)
        {
            hit.transform.TryGetComponent<Grabbable>(out var grabbable);
            StartGrab(grabbable);
        }

        if (!isMousePressed) StopGrab();
    }

    private void StartFocus(Grabbable grabbable)
    {
        if (grabbable == null) return;

        if (FocusedGrabbable != null) FocusedGrabbable.FocusExit();

        FocusedGrabbable = grabbable;
        grabbable.FocusEnter();
    }

    private void StopFocus()
    {
        if (FocusedGrabbable == null) return;

        FocusedGrabbable.FocusExit();
        FocusedGrabbable = null;
    }

    private void StartGrab(Grabbable grabbable)
    {
        if (grabbable == null) return;
        if (grabbable != FocusedGrabbable) return;
        // if (_grabbed != null)
        // {
        //     _grabbed.GrabExit();
        // }

        _grabbed = grabbable;
        grabbable.GrabEnter();
    }

    private void StopGrab()
    {
        if (_grabbed == null) return;
        _grabbed.GrabExit();
        _grabbed = null;
    }
}
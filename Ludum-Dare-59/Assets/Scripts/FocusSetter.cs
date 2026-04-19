using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FocusSetter : MonoBehaviour
{
    private Grabbable _focused;
    private Grabbable _grabbed;

    private bool _hasPickedUpPiece;
    private Grabbable _hovered;

    private Camera _mainCamera;
    private InputAction _selectAction;

    private void Start()
    {
        _selectAction = InputSystem.actions.FindAction("Attack");
        _mainCamera = Camera.main;
        _hasPickedUpPiece = false;
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

        if (isHoveringGrabbable)
        {
            hit.transform.TryGetComponent<Grabbable>(out var grabbable);
            StartHover(grabbable);

            if (isMousePressed)
            {
                StartGrab(grabbable);
            }
        }
        else
        {
            StopHover();
        }

        if (!isMousePressed)
        {
            StopGrab();
        }
    }

    public static event Action OnFirstPiecePickedUp;

    private void StartHover(Grabbable grabbable)
    {
        if (!grabbable)
        {
            return;
        }

        if (_hovered == grabbable)
        {
            return;
        }

        if (_hovered)
        {
            _hovered.HoverExit();
        }

        _hovered = grabbable;
        grabbable.HoverEnter();
    }

    private void StopHover()
    {
        if (!_hovered)
        {
            return;
        }

        _hovered.HoverExit();
        _hovered = null;
    }

    private void StartFocus(Grabbable grabbable)
    {
        if (!grabbable)
        {
            return;
        }

        if (_focused)
        {
            _focused.FocusExit();
        }

        _focused = grabbable;
        grabbable.FocusEnter();
    }

    private void StopFocus()
    {
        if (!_focused)
        {
            return;
        }

        _focused.FocusExit();
        _focused = null;
    }

    private void StartGrab(Grabbable grabbable)
    {
        if (!grabbable)
        {
            return;
        }

        if (grabbable != _focused)
        {
            return;
        }

        if (!_hasPickedUpPiece)
        {
            OnFirstPiecePickedUp?.Invoke();
            _hasPickedUpPiece = true;
        }

        _grabbed = grabbable;
        grabbable.GrabEnter();
    }

    private void StopGrab()
    {
        if (!_grabbed)
        {
            return;
        }

        _grabbed.GrabExit();
        _grabbed = null;
    }
}
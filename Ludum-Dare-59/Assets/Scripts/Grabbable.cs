using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Grabbable : MonoBehaviour
{
    [SerializeField] private UnityEvent onFocusEnter;
    [SerializeField] private UnityEvent onFocusExit;
    [SerializeField] private UnityEvent onGrabEnter;
    [SerializeField] private UnityEvent onGrabExit;

    private void Start()
    {
    }

    public void GrabEnter()
    {
        onGrabEnter?.Invoke();
    }

    public void GrabExit()
    {
        onGrabExit?.Invoke();
    }

    public void FocusEnter()
    {
        onFocusEnter?.Invoke();
    }

    public void FocusExit()
    {
        onFocusExit?.Invoke();
    }
}
using UnityEngine;

public class FollowTargetSlot : MonoBehaviour
{
    private Transform _targetSlot;

    private void Start()
    {
        _targetSlot = GameObject.FindGameObjectWithTag("TargetSlot").transform;
    }

    private void Update()
    {
        transform.position = _targetSlot.position;
    }
}
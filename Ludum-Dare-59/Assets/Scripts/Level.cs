using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private float secondsUntilGameOver = 120f;
    [SerializeField] private SpriteRenderer target;
    [SerializeField] private Transform[] pieces;
}
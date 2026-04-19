using UnityEngine;

public class Level : MonoBehaviour
{
    public float secondsUntilGameOver = 60f;
    public Transform target;
    public Vector3 shapeGrades = new(0.7f, 0.8f, 0.9f);
    public Vector3 timeGrades = new(10f, 20f, 45f);
    public Transform[] pieces;
}
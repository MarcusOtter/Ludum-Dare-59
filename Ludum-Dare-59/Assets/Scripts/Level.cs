using UnityEngine;

[CreateAssetMenu(menuName = "Level")]
public class Level : ScriptableObject
{
    [Header("Difficulty")] public float secondsUntilGameOver = 60f;
    public Vector3 shapeGrades = new(0.7f, 0.8f, 0.9f);
    public Vector3 timeGrades = new(10f, 20f, 45f);

    [Header("Prefabs")] public PuzzlePiece target;
    public PuzzlePiece[] pieces;

    // TODO: Dialogue on start
}
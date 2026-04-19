using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Level[] levels;
    [SerializeField] private int currentLevelIndex;
    [SerializeField] private Transform pieceSlotParent;
    [SerializeField] private Transform targetParent;

    private readonly List<Transform> _pieceSlots = new();
    private Level _currentLevel;

    private void Start()
    {
        foreach (Transform t in pieceSlotParent)
        {
            _pieceSlots.Add(t);
        }

        StartLevel(levels[currentLevelIndex]);
    }

    private void Update()
    {
        if (InputSystem.actions.FindAction("Jump").WasPressedThisFrame())
        {
            StartLevel(levels[currentLevelIndex]);
            // SimpleSceneManager.LoadNextScene();
        }
    }

    private void StartLevel(Level level)
    {
        if (_currentLevel)
        {
            Destroy(_currentLevel.gameObject);
        }

        _currentLevel = Instantiate(level, transform);

        for (var i = 0; i < _currentLevel.pieces.Length; i++)
        {
            var piece = _currentLevel.pieces[i];
            var angle = Random.Range(0, 360f);
            piece.position = _pieceSlots[i].position;
            piece.Rotate(Vector3.forward, angle);
        }

        _currentLevel.target.position = targetParent.position;
        _currentLevel.target.gameObject.SetActive(true);

        // TODO Do something with this
        // _currentLevel.secondsUntilGameOver
    }
}
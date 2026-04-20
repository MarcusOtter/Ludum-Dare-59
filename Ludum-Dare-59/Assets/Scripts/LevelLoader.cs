using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeRemainingText;
    [SerializeField] private Level[] levels;
    [SerializeField] private int currentLevelIndex;
    [SerializeField] private Transform pieceSlotParent;
    [SerializeField] private Transform targetParent;

    private readonly List<Transform> _pieceSlots = new();
    private Level _currentLevel;
    private bool _isTimerRunning;

    private float _timeRemaining;

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
        if (_isTimerRunning)
        {
            SetTimeRemaining(_timeRemaining - Time.deltaTime);
        }

        if (_timeRemaining <= 0)
        {
            _isTimerRunning = false;
            SetTimeRemaining(0f);
            GameManager.Instance.TriggerGameOver();
        }
    }

    private void OnEnable()
    {
        FocusSetter.OnFirstPiecePickedUp += StartLevelTimer;
    }

    private void OnDisable()
    {
        FocusSetter.OnFirstPiecePickedUp -= StartLevelTimer;
    }

    private void SetTimeRemaining(float newTimeRemaining)
    {
        _timeRemaining = newTimeRemaining;
        timeRemainingText.text = _timeRemaining.ToString("F2");
    }

    private void StartLevelTimer()
    {
        _isTimerRunning = true;
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

        SetTimeRemaining(_currentLevel.secondsUntilGameOver);
    }
}
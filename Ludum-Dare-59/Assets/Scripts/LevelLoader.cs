using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeRemainingText;
    [SerializeField] private Level level;
    [SerializeField] private Transform pieceSlotParent;

    private readonly List<Transform> _pieceSlots = new();
    private bool _isTimerRunning;
    private float _timeRemaining;

    private void Start()
    {
        foreach (Transform t in pieceSlotParent)
        {
            _pieceSlots.Add(t);
        }

        StartLevel();
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

    private void StartLevel()
    {
        GameManager.Instance.TriggerGameStart();

        Instantiate(level.target, transform);

        for (var i = 0; i < level.pieces.Length; i++)
        {
            var piece = Instantiate(level.pieces[i], transform);
            var angle = Random.Range(0, 360f);
            piece.transform.position = _pieceSlots[i].position;
            piece.transform.Rotate(Vector3.forward, angle);
        }

        SetTimeRemaining(level.secondsUntilGameOver);
    }
}
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
    private Level _spawnedLevel;
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
        if (_spawnedLevel)
        {
            Destroy(_spawnedLevel.gameObject);
        }

        GameManager.Instance.TriggerGameStart();
        _spawnedLevel = Instantiate(level, transform);

        for (var i = 0; i < _spawnedLevel.pieces.Length; i++)
        {
            var piece = _spawnedLevel.pieces[i];
            var angle = Random.Range(0, 360f);
            piece.position = _pieceSlots[i].position;
            piece.Rotate(Vector3.forward, angle);
        }

        _spawnedLevel.target.gameObject.SetActive(true);

        SetTimeRemaining(_spawnedLevel.secondsUntilGameOver);
    }
}
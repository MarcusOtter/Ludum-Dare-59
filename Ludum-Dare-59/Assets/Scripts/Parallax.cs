using UnityEngine;

public class Parallax : MonoBehaviour
{
    private const float Duration = 2f;

    [SerializeField] private Vector2 startOffset;
    [SerializeField] private bool playOnStart;
    [SerializeField] private AnimationCurve animationCurve;
    private Vector2 _spawnPosition;

    private Vector2 _startPosition;
    private Vector2 _targetPosition;

    private float _timeLeft;

    private void Start()
    {
        _spawnPosition = transform.position;

        if (playOnStart)
        {
            StartParallax();
        }
    }

    private void Update()
    {
        if (_timeLeft > 0f)
        {
            var t = _timeLeft / Duration;
            t = animationCurve.Evaluate(t);
            var newPosition = Vector2.Lerp(_targetPosition, _startPosition, t);
            transform.position = newPosition;
            _timeLeft -= Time.deltaTime;
        }
        else
        {
            transform.position = _targetPosition;
        }
    }

    public void StartParallax(bool inverseDirection = false)
    {
        if (inverseDirection)
        {
            _targetPosition = _spawnPosition + startOffset;
            _startPosition = _spawnPosition;
        }
        else
        {
            _targetPosition = _spawnPosition;
            _startPosition = _spawnPosition + startOffset;
        }

        transform.position = _startPosition;
        _timeLeft = Duration;
    }
}
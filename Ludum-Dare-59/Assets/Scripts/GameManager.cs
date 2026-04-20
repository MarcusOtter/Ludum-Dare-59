using System;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    private RenderTextureComprarer _textureComparer;
    public Sprite LatestImage { get; private set; }
    public float LatestScore { get; private set; }
    public bool IsGameOver { get; private set; }

    // TODO: Store all level scores with persistence, it's easy

    private void Start()
    {
        _textureComparer = GetComponentInChildren<RenderTextureComprarer>();
    }

    public event Action OnGameOver;

    public void TriggerGameStart()
    {
        IsGameOver = false;
    }

    public void TriggerGameOver()
    {
        if (IsGameOver)
        {
            return;
        }

        LatestScore = _textureComparer.GetScore();
        LatestImage = _textureComparer.GetPlayerSprite();

        // TODO: Get grades from level


        IsGameOver = true;
        OnGameOver?.Invoke();
        SimpleSceneManager.LoadNextScene();
    }
}
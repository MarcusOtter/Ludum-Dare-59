using System;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public Sprite LatestImage { get; private set; }
    public float LatestScore { get; private set; }
    public bool IsGameOver { get; private set; }
    public event Action OnGameOver;

    // TODO: Store all level scores with persistence, it's easy

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

        var textureComparer = FindAnyObjectByType<RenderTextureComprarer>();
        LatestScore = textureComparer.GetScore();
        LatestImage = textureComparer.GetPlayerSprite();

        // TODO: Get grades from level


        IsGameOver = true;
        OnGameOver?.Invoke();
        SimpleSceneManager.LoadNextScene();
    }
}
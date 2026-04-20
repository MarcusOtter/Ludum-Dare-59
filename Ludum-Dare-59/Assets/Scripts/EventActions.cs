using UnityEngine;


// Script for triggering different things from UnityEvents and Animations

public class EventActions : MonoBehaviour
{
    public void TriggerGameOver()
    {
        GameManager.Instance.TriggerGameOver();
    }

    public void LoadNextScene()
    {
        SimpleSceneManager.LoadNextScene();
    }

    public void PlaySound(CustomAudioClip audioClip)
    {
        AudioPlayer.Instance.PlaySoundEffect(audioClip);
    }
}
using UnityEngine;


// Script for triggering different things from UnityEvents and Animations

public class EventActions : MonoBehaviour
{
    public void TriggerGameOver()
    {
        GameManager.Instance.TriggerGameOver();
    }

    public void PlaySound(CustomAudioClip audioClip)
    {
        AudioPlayer.Instance.PlaySoundEffect(audioClip);
    }
}
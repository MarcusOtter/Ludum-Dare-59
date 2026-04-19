using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : SingletonMonoBehaviour<AudioPlayer>
{
    private AudioSource _audioSource;
    public bool IsMuted { get; private set; }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundEffect(CustomAudioClip clip)
    {
        PlayCustomAudioClip(clip);
    }

    public void Mute(bool mute)
    {
        if (mute == IsMuted)
        {
            return;
        }

        IsMuted = mute;
        _audioSource.mute = mute;

        foreach (var audioSources in FindObjectsByType<AudioSource>())
        {
            audioSources.mute = mute;
        }
    }

    private void PlayCustomAudioClip(CustomAudioClip clip)
    {
        if (IsMuted)
        {
            return;
        }

        if (Mathf.Approximately(clip.GetPitch(), 1))
        {
            _audioSource.PlayOneShot(clip.GetClip(), clip.GetVolume());
            return;
        }

        var source = SpawnNewAudioSource();
        PlayClipOnAudioSource(source, clip);
    }

    private static void PlayClipOnAudioSource(AudioSource source, CustomAudioClip clip)
    {
        source.volume = clip.GetVolume();
        source.pitch = clip.GetPitch();
        source.clip = clip.GetClip();
        source.Play();

        Destroy(source.gameObject, clip.GetClip().length + 2f);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private static AudioSource SpawnNewAudioSource()
    {
        var obj = new GameObject("Audio source", typeof(AudioSource));
        obj.transform.parent = Instance.transform;
        return obj.GetComponent<AudioSource>();
    }
}
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : SingletonMonoBehaviour<AudioPlayer>
{
    // TODO remove
    [SerializeField] private Dialogue[] dialogueSequenceTest;
    [SerializeField] private Dialogue dialogueOneshotTest;
    [SerializeField] private float delayBetweenDialogue = 1f;

    public UnityEvent<Dialogue> onDialogueStart;
    public UnityEvent onDialogueEnd;

    private int _activeDialogueIndex = int.MaxValue - 1000;
    private Dialogue[] _activeDialogueSequence;
    private AudioSource _audioSource;
    private AudioSource _dialogueSource;
    private bool _isMuted;
    private float _nextSyllablePlayTime;
    private int _remainingSyllables;

    private InputAction _skipKeyA;
    // private InputAction _skipKeyB;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _dialogueSource = SpawnNewAudioSource("Audio Source (Dialogue)");

        _skipKeyA = InputSystem.actions.FindAction("Jump");
        // _skipKeyB = InputSystem.actions.FindAction("Attack");

        // TODO: Remove
        // if (dialogueSequenceTest.Length != 0)
        // {
        //     PlayDialogueSequence(dialogueSequenceTest);
        // }
        // else if (dialogueOneshotTest)
        // {
        //     PlayDialogue(dialogueOneshotTest);
        // }
    }

    private void Update()
    {
        if (_activeDialogueIndex > 10000)
        {
            return;
        }

        if (_remainingSyllables <= 0 || _skipKeyA.WasPressedThisFrame())
        {
            GoToNextDialogue();
        }

        if (_activeDialogueSequence.Length == 0 || _activeDialogueIndex >= _activeDialogueSequence.Length)
        {
            return;
        }

        var dialogue = _activeDialogueSequence[_activeDialogueIndex];
        if (Time.time >= _nextSyllablePlayTime)
        {
            PlaySyllable(dialogue);
        }
    }

    public void PlaySoundEffect(CustomAudioClip clip)
    {
        PlayCustomAudioClip(clip);
    }


    public void PlayDialogue(Dialogue dialogue)
    {
        StartDialogueSequence(new[] { dialogue });
    }

    public void PlayDialogueSequence(Dialogue[] dialogueSequence)
    {
        StartDialogueSequence(dialogueSequence);
    }

    private void PlaySyllable(Dialogue dialogue)
    {
        PlayCustomAudioClip(dialogue.voice);
        var randomDelay = Random.Range(dialogue.minDelayBetweenSyllables, dialogue.maxDelayBetweenSyllables);
        _nextSyllablePlayTime = Time.time + randomDelay;
        _remainingSyllables--;
    }

    private void StartDialogueSequence(Dialogue[] sequence)
    {
        if (_isMuted)
        {
            return;
        }

        if (sequence.Length == 0)
        {
            return;
        }

        var dialogue = sequence[0];
        _activeDialogueSequence = sequence;
        _activeDialogueIndex = 0;
        _remainingSyllables = dialogue.syllables;

        onDialogueStart?.Invoke(dialogue);
    }

    private void GoToNextDialogue()
    {
        _dialogueSource.Stop();
        _nextSyllablePlayTime = Time.time + delayBetweenDialogue;

        if (_activeDialogueIndex + 1 < _activeDialogueSequence.Length)
        {
            var newIndex = _activeDialogueIndex + 1;
            var dialogue = _activeDialogueSequence[newIndex];

            onDialogueStart?.Invoke(dialogue);
            _remainingSyllables = dialogue.syllables;
            _activeDialogueIndex = newIndex;
        }
        else
        {
            _activeDialogueSequence = new Dialogue[] { };
            _activeDialogueIndex = int.MaxValue - 100;
            onDialogueEnd?.Invoke();
        }
    }

    public void Mute(bool mute)
    {
        if (mute == _isMuted)
        {
            return;
        }

        _isMuted = mute;
        _audioSource.mute = mute;

        foreach (var audioSources in FindObjectsByType<AudioSource>())
        {
            audioSources.mute = mute;
        }
    }

    // Returns clip length
    private float PlayCustomAudioClip(CustomAudioClip clip)
    {
        if (_isMuted)
        {
            return 0f;
        }

        if (Mathf.Approximately(clip.GetPitch(), 1))
        {
            _audioSource.PlayOneShot(clip.GetClip(), clip.GetVolume());
            return 0f;
        }

        var source = SpawnNewAudioSource();
        return PlayClipOnAudioSource(source, clip);
    }


    // Returns clips length
    private static float PlayClipOnAudioSource(AudioSource source, CustomAudioClip clip)
    {
        var audioClip = clip.GetClip();
        source.volume = clip.GetVolume();
        source.pitch = clip.GetPitch();
        source.clip = audioClip;
        source.Play();

        Destroy(source.gameObject, clip.GetClip().length + 2f);
        return audioClip.length;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private static AudioSource SpawnNewAudioSource(string name = "Audio source")
    {
        var obj = new GameObject(name, typeof(AudioSource));
        obj.transform.parent = Instance.transform;
        return obj.GetComponent<AudioSource>();
    }
}
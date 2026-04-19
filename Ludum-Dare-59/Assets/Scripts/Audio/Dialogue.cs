using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public int syllables;
    public string text;
    public CustomAudioClip voice;
    public float minDelayBetweenSyllables = 0.2f;
    public float maxDelayBetweenSyllables = 0.3f;
}
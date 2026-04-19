using TMPro;
using UnityEngine;

public class DialogueUI : SingletonMonoBehaviour<DialogueUI>
{
    [SerializeField] private TextMeshProUGUI dialogueTextField;
    [SerializeField] private float dialogueSwitchDelay = 1f;

    private string _textToSet;
    private float _timeToSetText;

    private void Update()
    {
        if (Time.time >= _timeToSetText)
        {
            dialogueTextField.text = _textToSet;
            _timeToSetText = float.MaxValue;
            _textToSet = null;
        }
    }

    private void OnEnable()
    {
        AudioPlayer.Instance.onDialogueStart.AddListener(HandleDialogueStart);
        AudioPlayer.Instance.onDialogueEnd.AddListener(HandleDialogueEnd);
    }

    private void OnDisable()
    {
        AudioPlayer.Instance.onDialogueStart.RemoveListener(HandleDialogueStart);
        AudioPlayer.Instance.onDialogueEnd.RemoveListener(HandleDialogueEnd);
    }

    private void HandleDialogueStart(Dialogue dialogue)
    {
        if (_textToSet != null)
        {
            dialogueTextField.text = _textToSet;
        }

        _timeToSetText = Time.time + dialogueSwitchDelay;
        _textToSet = dialogue.text;
    }

    private void HandleDialogueEnd()
    {
        if (_textToSet != null)
        {
            dialogueTextField.text = _textToSet;
        }

        _timeToSetText = Time.time + dialogueSwitchDelay;
        _textToSet = "";
    }
}
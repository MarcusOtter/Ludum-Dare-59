using TMPro;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] private TextMeshProUGUI _timeRemaining;


    private void Start()
    {
        print("Run start");
    }
}
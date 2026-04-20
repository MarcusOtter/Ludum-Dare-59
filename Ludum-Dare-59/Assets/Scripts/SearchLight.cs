using UnityEngine;

public class SearchLight : MonoBehaviour
{
    [SerializeField] private SpriteMask mask;

    private void Start()
    {
        mask.sprite = GameManager.Instance.LatestImage;
    }
}
using UnityEngine;

public class PhotoBox : MonoBehaviour
{
    [SerializeField] private float scale = 2f;
    [SerializeField] private Transform[] transformsToScale;
    [SerializeField] private Camera cameraToScale;

    private float _prevScale;

    private void Start()
    {
        UpdateScale();
    }

    private void Update()
    {
        if (Mathf.Approximately(_prevScale, scale))
        {
            return;
        }

        UpdateScale();
    }

    private void UpdateScale()
    {
        foreach (var t in transformsToScale)
        {
            t.localScale = new Vector3(scale, scale, scale);
        }

        cameraToScale.orthographicSize = scale / 2f;
        _prevScale = scale;
    }
}
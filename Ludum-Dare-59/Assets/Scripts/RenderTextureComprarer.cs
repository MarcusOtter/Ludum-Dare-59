using TMPro;
using UnityEngine;

public class RenderTextureComprarer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;

    [SerializeField] private RenderTexture renderTextureA;
    [SerializeField] private RenderTexture renderTextureB;


    private void Update()
    {
        // TODO: Do not run this in Update, only on submit (performance hit)
        var score = MaskIoU.Score(renderTextureA, renderTextureB);
        var matchPercent = 100 * score;
        textMesh.text = $"Match: {matchPercent}%";
    }
}
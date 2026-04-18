using TMPro;
using UnityEngine;

public class RenderTextureComprarer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;

    [SerializeField] private RenderTexture renderTextureA;
    [SerializeField] private RenderTexture renderTextureB;


    private void Update()
    {
        var maskA = MaskIoU.ReadBinaryMask(renderTextureA);
        var maskB = MaskIoU.ReadBinaryMask(renderTextureB);
        var iou = MaskIoU.ComputeIoU(maskA, maskB);
        var matchPercent = 100 * iou;
        textMesh.text = $"Match: {matchPercent}%";
    }
}
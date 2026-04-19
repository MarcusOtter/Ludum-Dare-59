using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class RenderTextureComprarer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;

    [FormerlySerializedAs("renderTextureA")] [SerializeField]
    private RenderTexture targetRenderTexture;

    [FormerlySerializedAs("renderTextureB")] [SerializeField]
    private RenderTexture playerRenderTexture;


    private void Update()
    {
        if (!InputSystem.actions.FindAction("Jump").WasPressedThisFrame())
        {
            return;
        }

        var score = MaskIoU.ScoreIgnorePositionAndRotation(
            targetRenderTexture,
            playerRenderTexture,
            0.3f,
            2f);
        var matchPercent = 100 * score;
        textMesh.text = $"Match: {matchPercent}%";
    }
}
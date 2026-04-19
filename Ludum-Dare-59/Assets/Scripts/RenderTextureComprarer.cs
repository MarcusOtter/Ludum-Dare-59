using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RenderTextureComprarer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private RenderTexture targetRenderTexture;
    [SerializeField] private RenderTexture playerRenderTexture;
    [SerializeField] private SpriteMask testMask;

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

        var rect = new Rect(0, 0, playerRenderTexture.width, playerRenderTexture.height);
        var texture2D = ToTexture2D(playerRenderTexture, rect);
        var sprite = Sprite.Create(texture2D, rect, new Vector2(0.5f, 0.5f));

        testMask.sprite = sprite;
    }

    // Source - https://stackoverflow.com/a/44265122
    // Posted by Programmer, modified by community. See post 'Timeline' for change history
    // Retrieved 2026-04-19, License - CC BY-SA 4.0
    private Texture2D ToTexture2D(RenderTexture rTex, Rect rect)
    {
        var tex = new Texture2D(512, 512, TextureFormat.Alpha8, false);
        // ReadPixels looks at the active RenderTexture.
        RenderTexture.active = rTex;
        tex.ReadPixels(rect, 0, 0);
        tex.Apply();
        return tex;
    }
}
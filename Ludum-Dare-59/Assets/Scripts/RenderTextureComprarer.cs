using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RenderTextureComprarer : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private RenderTexture targetRenderTexture;

    [SerializeField] private RenderTexture playerRenderTexture;

    [Header("Debug")] [SerializeField] private bool isDebug;
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private SpriteMask testMask;

    private void Start()
    {
        if (!isDebug)
        {
            return;
        }

        testMask.sprite = null;
    }

    private void Update()
    {
        if (!isDebug)
        {
            return;
        }

        if (!InputSystem.actions.FindAction("Jump").WasPressedThisFrame())
        {
            return;
        }

        var score = GetScore();
        var sprite = GetPlayerSprite();
        var matchPercent = 100 * score;

        if (textMesh)
        {
            textMesh.text = $"Match: {matchPercent}%";
        }

        if (testMask)
        {
            testMask.sprite = sprite;
        }
    }

    public float GetScore()
    {
        var score = MaskIoU.ScoreIgnorePositionAndRotation(
            targetRenderTexture,
            playerRenderTexture,
            0.3f,
            2f);

        return score;
    }

    public Sprite GetPlayerSprite()
    {
        var rect = new Rect(0, 0, playerRenderTexture.width, playerRenderTexture.height);
        var texture2D = ToTexture2D(playerRenderTexture, rect);
        var sprite = Sprite.Create(texture2D, rect, new Vector2(0.5f, 0.5f));

        return sprite;
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
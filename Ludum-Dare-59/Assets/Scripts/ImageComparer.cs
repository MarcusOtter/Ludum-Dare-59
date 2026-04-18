using UnityEngine;

public static class MaskIoU
{
    public static bool[] ReadBinaryMask(RenderTexture rt, float alphaThreshold = 0.5f)
    {
        var prev = RenderTexture.active;
        RenderTexture.active = rt;

        var tex = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        var pixels = tex.GetPixels32();
        var mask = new bool[pixels.Length];

        for (var i = 0; i < pixels.Length; i++)
        {
            mask[i] = pixels[i].a >= alphaThreshold * 255f;
        }

        Object.Destroy(tex);
        RenderTexture.active = prev;
        return mask;
    }

    public static float ComputeIoU(bool[] a, bool[] b)
    {
        if (a.Length != b.Length)
        {
            return 0f;
        }

        var intersection = 0;
        var union = 0;

        for (var i = 0; i < a.Length; i++)
        {
            var av = a[i];
            var bv = b[i];

            if (av && bv)
            {
                intersection++;
            }

            if (av || bv)
            {
                union++;
            }
        }

        return union == 0 ? 1f : (float)intersection / union;
    }
}
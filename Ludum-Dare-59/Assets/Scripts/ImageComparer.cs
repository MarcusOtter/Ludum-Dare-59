using UnityEngine;

public static class MaskIoU
{
    public static float ScoreShapeIgnorePlacement(
        RenderTexture a,
        RenderTexture b,
        float alphaThreshold = 0.3f,
        int dilationRadius = 5)
    {
        if (a.width != b.width || a.height != b.height)
        {
            return 0f;
        }

        var width = a.width;
        var height = a.height;

        var maskA = ReadBinaryMask(a, alphaThreshold);
        var maskB = ReadBinaryMask(b, alphaThreshold);

        if (!TryGetBounds(maskA, width, height, out var boundsA))
        {
            return 0f;
        }

        if (!TryGetBounds(maskB, width, height, out var boundsB))
        {
            return 0f;
        }

        if (dilationRadius > 0)
        {
            maskA = Dilate(maskA, width, height, dilationRadius);
            maskB = Dilate(maskB, width, height, dilationRadius);
        }

        var centerA = GetBoundsCenter(boundsA);
        var centerB = GetBoundsCenter(boundsB);

        var offsetX = Mathf.RoundToInt(centerA.x - centerB.x);
        var offsetY = Mathf.RoundToInt(centerA.y - centerB.y);

        return ComputeIoUAtOffset(maskA, maskB, width, height, offsetX, offsetY);
    }

    public static bool[] ReadBinaryMask(RenderTexture rt, float alphaThreshold)
    {
        var prev = RenderTexture.active;
        RenderTexture.active = rt;

        var tex = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        var pixels = tex.GetPixels32();
        var mask = new bool[pixels.Length];
        var threshold = alphaThreshold * 255f;

        for (var i = 0; i < pixels.Length; i++)
        {
            mask[i] = pixels[i].a >= threshold;
        }

        Object.Destroy(tex);
        RenderTexture.active = prev;
        return mask;
    }

    public static bool TryGetBounds(bool[] mask, int width, int height, out RectInt bounds)
    {
        int minX = width, minY = height, maxX = -1, maxY = -1;

        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
        {
            if (!mask[y * width + x])
            {
                continue;
            }

            if (x < minX)
            {
                minX = x;
            }

            if (y < minY)
            {
                minY = y;
            }

            if (x > maxX)
            {
                maxX = x;
            }

            if (y > maxY)
            {
                maxY = y;
            }
        }

        if (maxX < minX || maxY < minY)
        {
            bounds = default;
            return false;
        }

        bounds = new RectInt(minX, minY, maxX - minX + 1, maxY - minY + 1);
        return true;
    }

    public static Vector2 GetBoundsCenter(RectInt bounds)
    {
        return new Vector2(
            bounds.x + (bounds.width - 1) * 0.5f,
            bounds.y + (bounds.height - 1) * 0.5f);
    }

    public static bool[] Dilate(bool[] src, int width, int height, int radius)
    {
        var dst = new bool[src.Length];

        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
        {
            var on = false;

            for (var dy = -radius; dy <= radius && !on; dy++)
            {
                var ny = y + dy;
                if (ny < 0 || ny >= height)
                {
                    continue;
                }

                for (var dx = -radius; dx <= radius; dx++)
                {
                    var nx = x + dx;
                    if (nx < 0 || nx >= width)
                    {
                        continue;
                    }

                    if (src[ny * width + nx])
                    {
                        on = true;
                        break;
                    }
                }
            }

            dst[y * width + x] = on;
        }

        return dst;
    }

    public static float ComputeIoUAtOffset(bool[] a, bool[] b, int width, int height, int offsetX, int offsetY)
    {
        var intersection = 0;
        var union = 0;

        for (var y = 0; y < height; y++)
        {
            var by = y - offsetY;
            var validY = by >= 0 && by < height;

            for (var x = 0; x < width; x++)
            {
                var av = a[y * width + x];
                var bv = false;

                if (validY)
                {
                    var bx = x - offsetX;
                    if (bx >= 0 && bx < width)
                    {
                        bv = b[by * width + bx];
                    }
                }

                if (av && bv)
                {
                    intersection++;
                }

                if (av || bv)
                {
                    union++;
                }
            }
        }

        return union == 0 ? 1f : (float)intersection / union;
    }
}
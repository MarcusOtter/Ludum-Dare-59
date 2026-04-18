using UnityEngine;

public static class MaskIoU
{
    public static float Score(
        RenderTexture a,
        RenderTexture b,
        float alphaThreshold = 0.3f,
        int dilationRadius = 0,
        int maxShift = 32)
    {
        if (a.width != b.width || a.height != b.height)
        {
            return 0f;
        }

        var width = a.width;
        var height = a.height;

        var maskA = ReadBinaryMask(a, alphaThreshold);
        var maskB = ReadBinaryMask(b, alphaThreshold);

        if (dilationRadius > 0)
        {
            maskA = Dilate(maskA, width, height, dilationRadius);
            maskB = Dilate(maskB, width, height, dilationRadius);
        }

        return ComputeBestShiftedIoU(maskA, maskB, width, height, maxShift);
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

    public static bool[] Dilate(bool[] src, int width, int height, int radius)
    {
        var dst = new bool[src.Length];

        for (var y = 0; y < height; y++)
        {
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
        }

        return dst;
    }

    public static float ComputeBestShiftedIoU(bool[] a, bool[] b, int width, int height, int maxShift)
    {
        var best = 0f;

        for (var offsetY = -maxShift; offsetY <= maxShift; offsetY++)
        {
            for (var offsetX = -maxShift; offsetX <= maxShift; offsetX++)
            {
                var iou = ComputeIoUAtOffset(a, b, width, height, offsetX, offsetY);
                if (iou > best)
                {
                    best = iou;
                }
            }
        }

        return best;
    }

    private static float ComputeIoUAtOffset(bool[] a, bool[] b, int width, int height, int offsetX, int offsetY)
    {
        var intersection = 0;
        var union = 0;

        for (var y = 0; y < height; y++)
        {
            var sampleY = y - offsetY;
            var validY = sampleY >= 0 && sampleY < height;

            for (var x = 0; x < width; x++)
            {
                var av = a[y * width + x];
                var bv = false;

                if (validY)
                {
                    var sampleX = x - offsetX;
                    if (sampleX >= 0 && sampleX < width)
                    {
                        bv = b[sampleY * width + sampleX];
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
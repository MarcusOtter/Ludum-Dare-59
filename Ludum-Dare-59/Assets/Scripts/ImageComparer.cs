using UnityEngine;

public static class MaskIoU
{
    public static float ScoreIgnorePositionAndRotation(
        RenderTexture targetRt,
        RenderTexture playerRt,
        float alphaThreshold = 0.3f,
        float angleStep = 5f)
    {
        if (targetRt.width != playerRt.width || targetRt.height != playerRt.height)
        {
            return 0f;
        }

        var width = targetRt.width;
        var height = targetRt.height;

        var target = ReadBinaryMask(targetRt, alphaThreshold);
        var player = ReadBinaryMask(playerRt, alphaThreshold);

        if (!TryGetCentroid(target, width, height, out var targetCentroid, out var targetArea))
        {
            return 0f;
        }

        if (!TryGetCentroid(player, width, height, out var playerCentroid, out var playerArea))
        {
            return 0f;
        }

        var bestIoU = 0f;

        for (var angle = 0f; angle < 360f; angle += angleStep)
        {
            var rotated = RotateMask(player, width, height, playerCentroid, angle);

            if (!TryGetCentroid(rotated, width, height, out var rotatedCentroid, out _))
            {
                continue;
            }

            var iou = ComputeIoUAtAlignedCentroid(
                target,
                rotated,
                width,
                height,
                targetCentroid,
                rotatedCentroid);

            if (iou > bestIoU)
            {
                bestIoU = iou;
            }
        }

        var areaSimilarity = Mathf.Min(targetArea, playerArea) / (float)Mathf.Max(targetArea, playerArea);
        return bestIoU * areaSimilarity;
    }

    public static bool[] ReadBinaryMask(RenderTexture rt, float alphaThreshold = 0.3f)
    {
        var previous = RenderTexture.active;
        RenderTexture.active = rt;

        var tex = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        var pixels = tex.GetPixels32();
        var mask = new bool[pixels.Length];
        var threshold = (byte)Mathf.RoundToInt(alphaThreshold * 255f);

        for (var i = 0; i < pixels.Length; i++)
        {
            mask[i] = pixels[i].a >= threshold;
        }

        Object.Destroy(tex);
        RenderTexture.active = previous;

        return mask;
    }

    public static bool TryGetCentroid(bool[] mask, int width, int height, out Vector2 centroid, out int area)
    {
        long sumX = 0;
        long sumY = 0;
        area = 0;

        for (var y = 0; y < height; y++)
        {
            var row = y * width;

            for (var x = 0; x < width; x++)
            {
                if (!mask[row + x])
                {
                    continue;
                }

                sumX += x;
                sumY += y;
                area++;
            }
        }

        if (area == 0)
        {
            centroid = default;
            return false;
        }

        centroid = new Vector2((float)sumX / area, (float)sumY / area);
        return true;
    }

    public static bool[] RotateMask(bool[] src, int width, int height, Vector2 center, float angleDeg)
    {
        var dst = new bool[src.Length];

        var rad = angleDeg * Mathf.Deg2Rad;
        var cos = Mathf.Cos(rad);
        var sin = Mathf.Sin(rad);

        for (var y = 0; y < height; y++)
        {
            var row = y * width;

            for (var x = 0; x < width; x++)
            {
                var dx = x - center.x;
                var dy = y - center.y;

                var sx = cos * dx + sin * dy + center.x;
                var sy = -sin * dx + cos * dy + center.y;

                var ix = Mathf.RoundToInt(sx);
                var iy = Mathf.RoundToInt(sy);

                if ((uint)ix < (uint)width && (uint)iy < (uint)height)
                {
                    dst[row + x] = src[iy * width + ix];
                }
            }
        }

        return dst;
    }

    public static float ComputeIoUAtAlignedCentroid(
        bool[] a,
        bool[] b,
        int width,
        int height,
        Vector2 centroidA,
        Vector2 centroidB)
    {
        var offsetX = Mathf.RoundToInt(centroidA.x - centroidB.x);
        var offsetY = Mathf.RoundToInt(centroidA.y - centroidB.y);

        var intersection = 0;
        var union = 0;

        for (var y = 0; y < height; y++)
        {
            var by = y - offsetY;
            var validY = (uint)by < (uint)height;
            var rowA = y * width;

            for (var x = 0; x < width; x++)
            {
                var av = a[rowA + x];
                var bv = false;

                if (validY)
                {
                    var bx = x - offsetX;
                    if ((uint)bx < (uint)width)
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
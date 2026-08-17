using System;
using System.IO;
using UnityEditor;
using UnityEngine;

internal sealed class FlappyPixelImporter : AssetPostprocessor
{
    private const string AssetFolder = "Assets/Art/FlappyPixel/";
    private const string BirdSheet = AssetFolder + "bird_flight_6frames.png";

    private void OnPreprocessTexture()
    {
        if (!assetPath.StartsWith(AssetFolder, StringComparison.Ordinal))
        {
            return;
        }

        var importer = (TextureImporter)assetImporter;
        importer.textureType = TextureImporterType.Sprite;
        importer.spritePixelsPerUnit = 100f;
        importer.mipmapEnabled = false;
        importer.filterMode = FilterMode.Point;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.crunchedCompression = false;
        importer.maxTextureSize = 4096;
        importer.npotScale = TextureImporterNPOTScale.None;
        importer.alphaIsTransparency = true;
        importer.wrapMode = TextureWrapMode.Clamp;
        if (assetPath == BirdSheet)
        {
            importer.spriteImportMode = SpriteImportMode.Multiple;
#pragma warning disable CS0618 // Unity 6.5 still imports this metadata correctly.
            importer.spritesheet = BuildBirdFrames();
#pragma warning restore CS0618
        }
        else
        {
            importer.spriteImportMode = SpriteImportMode.Single;
        }
    }

    private SpriteMetaData[] BuildBirdFrames()
    {
        const int frameCount = 6;
        var (width, height) = ReadPngDimensions(assetPath);

        if (width <= 0 || height <= 0 || width % frameCount != 0)
        {
            throw new InvalidDataException(
                $"Bird sheet must contain {frameCount} equally sized horizontal frames: {assetPath}");
        }

        var frameWidth = width / frameCount;
        var frames = new SpriteMetaData[frameCount];

        for (var index = 0; index < frameCount; index++)
        {
            frames[index] = new SpriteMetaData
            {
                name = $"bird_flight_{index}",
                rect = new Rect(index * frameWidth, 0, frameWidth, height),
                alignment = (int)SpriteAlignment.Center,
                pivot = new Vector2(0.5f, 0.5f),
                border = Vector4.zero
            };
        }

        return frames;
    }

    private static (int width, int height) ReadPngDimensions(string relativePath)
    {
        using var stream = File.OpenRead(Path.GetFullPath(relativePath));
        var header = new byte[24];

        if (stream.Read(header, 0, header.Length) != header.Length)
        {
            return (0, 0);
        }

        return (ReadBigEndianInt32(header, 16), ReadBigEndianInt32(header, 20));
    }

    private static int ReadBigEndianInt32(byte[] bytes, int offset)
    {
        return (bytes[offset] << 24)
            | (bytes[offset + 1] << 16)
            | (bytes[offset + 2] << 8)
            | bytes[offset + 3];
    }
}

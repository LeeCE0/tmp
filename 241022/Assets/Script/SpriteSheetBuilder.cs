#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SpriteSheetBuilder
{
    public static void BuildSpriteSheet(string folderPath, string outputFileName, int frameWidth, int frameHeight, int framesPerRow = 0)
    {
        string[] files = Directory.GetFiles(folderPath, "*.png");

        List<Texture2D> frames = new List<Texture2D>();
        foreach (var file in files)
        {
            byte[] bytes = File.ReadAllBytes(file);
            Texture2D tex = new Texture2D(frameWidth, frameHeight, TextureFormat.RGBA32, false);
            tex.LoadImage(bytes);
            frames.Add(tex);
        }

        int totalFrames = frames.Count;
        int columns = framesPerRow > 0 ? framesPerRow : totalFrames;
        int rows = Mathf.CeilToInt((float)totalFrames / columns);

        int sheetWidth = columns * frameWidth;
        int sheetHeight = rows * frameHeight;

        Texture2D spriteSheet = new Texture2D(sheetWidth, sheetHeight, TextureFormat.RGBA32, false);

        for (int i = 0; i < totalFrames; i++)
        {
            int x = (i % columns) * frameWidth;
            int y = (rows - 1 - (i / columns)) * frameHeight;
            spriteSheet.SetPixels(x, y, frameWidth, frameHeight, frames[i].GetPixels());
        }

        spriteSheet.Apply();

        string fullPath = Path.Combine(folderPath, outputFileName);
        File.WriteAllBytes(fullPath, spriteSheet.EncodeToPNG());
        Debug.Log($"?? 시트 저장 완료: {fullPath}");

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }
}

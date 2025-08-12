using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public class PortraitCaptureTool : MonoBehaviour
{
    [MenuItem("Tools/Capture SPUM Portrait")]
    public static void CapturePortrait()
    {
        Camera cam = GameObject.Find("PortraitCamera").GetComponent<Camera>();
        RenderTexture rt = cam.targetTexture;

        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();
        RenderTexture.active = null;

        string path = "Assets/Portraits/";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        string filename = $"portrait_{System.DateTime.Now.Ticks}.png";
        string fullPath = path + filename;

        File.WriteAllBytes(fullPath, tex.EncodeToPNG());
        Debug.Log("Saved: " + fullPath);

        AssetDatabase.Refresh();
    }
}

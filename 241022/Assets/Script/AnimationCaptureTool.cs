#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class AnimationCaptureTool : MonoBehaviour
{
    [Header("📷 Capture Target")]
    public Camera captureCamera;
    public Animator targetAnimator;
    public string animationStateName = "Idle";

    [Header("🎞 Animation Capture Settings")]
    public float captureDuration = 1.0f;
    public float frameRate = 24f;
    public int textureWidth = 256;
    public int textureHeight = 256;

    [Header("💾 Save Settings")]
    public string UnitName = "";
    public string outputFolder = "Assets/AnimationCaptures";
    public string sheetSavePath = "Assets/Resources/Animation";

    private RenderTexture rt;

    public void StartCapture()
    {
        StartCoroutine(CaptureAnimationCoroutine());
    }

    private IEnumerator CaptureAnimationCoroutine()
    {
        if (captureCamera == null || targetAnimator == null)
        {
            Debug.LogError("❌ Camera 또는 Animator가 설정되지 않았습니다.");
            yield break;
        }

        string dir = Path.Combine(outputFolder, UnitName, animationStateName);
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        rt = new RenderTexture(textureWidth, textureHeight, 24, RenderTextureFormat.ARGB32);
        captureCamera.targetTexture = rt;
        RenderTexture.active = rt;

        targetAnimator.Play(animationStateName, 0, 0f);
        targetAnimator.Update(0f);

        float timeElapsed = 0f;
        int frameCount = 0;
        float interval = 1f / frameRate;

        while (timeElapsed < captureDuration)
        {
            yield return new WaitForEndOfFrame();

            // 1️⃣ 카메라 강제 렌더링
            captureCamera.Render();

            // 2️⃣ RenderTexture를 임시 Texture2D로 안전하게 복사
            RenderTexture prev = RenderTexture.active;
            RenderTexture.active = captureCamera.targetTexture;

            Texture2D temp = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
            temp.ReadPixels(new Rect(0, 0, textureWidth, textureHeight), 0, 0);
            temp.Apply();  // CPU 메모리에 픽셀 확정

            RenderTexture.active = prev;

            // 3️⃣ 새로운 Texture2D를 생성해서 CPU 메모리에서만 처리
            Texture2D cpuCopy = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
            cpuCopy.SetPixels(temp.GetPixels());
            cpuCopy.Apply();

            // 4️⃣ PNG 저장
            string filename = Path.Combine(dir, $"{UnitName}_{animationStateName}_{frameCount:D3}.png");
            File.WriteAllBytes(filename, cpuCopy.EncodeToPNG());

            // 5️⃣ 메모리 정리
            Destroy(temp);
            Destroy(cpuCopy);

            frameCount++;
            timeElapsed += interval;
            yield return new WaitForSeconds(interval);
        }
         

        captureCamera.targetTexture = null;
        RenderTexture.active = null;

        Debug.Log($"🎬 애니메이션 캡처 완료: {frameCount} 프레임 저장됨 ({dir})");

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

    public void BuildSpriteSheet(int framesPerRow = 0)
    {
        string dir = Path.Combine(outputFolder, UnitName, animationStateName);
        string[] files = Directory.GetFiles(dir, "*.png");
        if (files.Length == 0)
        {
            Debug.LogError("❌ PNG 프레임이 없습니다. 먼저 캡처를 실행하세요.");
            return;
        }

        List<Texture2D> frames = new List<Texture2D>();
        foreach (var file in files)
        {
            byte[] bytes = File.ReadAllBytes(file);
            Texture2D tex = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
            tex.LoadImage(bytes);
            frames.Add(tex);
        }

        int totalFrames = frames.Count;
        int columns = framesPerRow > 0 ? framesPerRow : totalFrames;
        int rows = Mathf.CeilToInt((float)totalFrames / columns);

        int sheetWidth = columns * textureWidth;
        int sheetHeight = rows * textureHeight;

        Texture2D spriteSheet = new Texture2D(sheetWidth, sheetHeight, TextureFormat.RGBA32, false);

        for (int i = 0; i < totalFrames; i++)
        {
            int x = (i % columns) * textureWidth;
            int y = (rows - 1 - (i / columns)) * textureHeight;
            spriteSheet.SetPixels(x, y, textureWidth, textureHeight, frames[i].GetPixels());
        }

        spriteSheet.Apply();

        string outputPath = Path.Combine(sheetSavePath, UnitName, $"{UnitName}_{animationStateName}.png");
        File.WriteAllBytes(outputPath, spriteSheet.EncodeToPNG());

        Debug.Log($"🧩 스프라이트 시트 저장 완료: {outputPath}");

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(AnimationCaptureTool))]
    public class AnimationCaptureToolEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var tool = (AnimationCaptureTool)target;

            GUILayout.Label("🎯 Capture Target", EditorStyles.boldLabel);
            tool.captureCamera = (Camera)EditorGUILayout.ObjectField("Capture Camera", tool.captureCamera, typeof(Camera), true);
            tool.targetAnimator = (Animator)EditorGUILayout.ObjectField("Target Animator", tool.targetAnimator, typeof(Animator), true);
            tool.animationStateName = EditorGUILayout.TextField("Animation State Name", tool.animationStateName);

            EditorGUILayout.Space();
            GUILayout.Label("🎞 Animation Settings", EditorStyles.boldLabel);
            tool.captureDuration = EditorGUILayout.FloatField("Capture Duration (sec)", tool.captureDuration);
            tool.frameRate = EditorGUILayout.FloatField("Frame Rate (fps)", tool.frameRate);
            tool.textureWidth = EditorGUILayout.IntField("Texture Width", tool.textureWidth);
            tool.textureHeight = EditorGUILayout.IntField("Texture Height", tool.textureHeight);

            EditorGUILayout.Space();
            GUILayout.Label("💾 Save Settings", EditorStyles.boldLabel);
            tool.UnitName = EditorGUILayout.TextField("Unit Name", tool.UnitName);
            tool.outputFolder = EditorGUILayout.TextField("Output Folder", tool.outputFolder);

            EditorGUILayout.HelpBox($"Output Path: {tool.outputFolder}/{"UnitName"}/{tool.animationStateName}", MessageType.Info);

            EditorGUILayout.Space();
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("🎬 애니메이션 캡처 시작", GUILayout.Height(30)))
            {
                tool.StartCapture();
            }
            GUI.backgroundColor = Color.white;

            EditorGUILayout.Space();
            GUI.backgroundColor = Color.cyan;
            if (GUILayout.Button("🧩 스프라이트 시트 생성", GUILayout.Height(30)))
            {
                tool.BuildSpriteSheet(8);
            }
            GUI.backgroundColor = Color.white;
        }
    }
#endif
}

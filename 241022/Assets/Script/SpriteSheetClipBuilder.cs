#if UNITY_EDITOR
using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;
using UnityEditor;
using UnityEditor.Animations;
using Unity.VisualScripting;

public class SpriteSheetClipBuilder : MonoBehaviour
{
    [Header("Source")]
    [Tooltip("Sprite Mode=Multiple로 슬라이스된 시트를 넣으세요.")]
    public Texture2D spritesheet;

    [Header("AnimatorController")]
    [Tooltip("복사할 AnimatorController")]
    public AnimatorController controller;

    [Header("Clip Settings")]
    [Min(1f)] public float fps = 12f;
    public bool loop = true;
    public string clipName = "NewClip";
    public string unitName = "";

    [Header("Output")]
    [Tooltip("Assets 폴더 하위 경로로 지정하세요.")]
    public string outputDir = "Assets/Resources/Animation";

    [Header("Automation")]
    [Tooltip("시트 변경 시 자동 생성")]
    public bool autoBuildOnAssign = true;

#if UNITY_EDITOR
    [HideInInspector] public string lastClipPath;
    private Texture2D _lastSheet;

    private void OnValidate()
    {
        if (!autoBuildOnAssign) return;
        if (spritesheet == null) return;
        if (spritesheet == _lastSheet) return;

        _lastSheet = spritesheet;

        UnityEditor.EditorApplication.delayCall += () =>
        {
            if (this == null) return;
            SpriteSheetClipBuilderEditor.BuildFromComponent(this);
        };
    }
#endif
}


[CustomEditor(typeof(SpriteSheetClipBuilder))]
public class SpriteSheetClipBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (target == null) return;
        base.OnInspectorGUI();
        var comp = (SpriteSheetClipBuilder)target;

        EditorGUILayout.Space(8);

        using (new EditorGUI.DisabledScope(!CanBuild(comp)))
        {
            if (GUILayout.Button("Build AnimationClip (.anim)", GUILayout.Height(28)))
                BuildFromComponent(comp);
        }

        //AnimatorController 생성 버튼
        if (GUILayout.Button("AnimatorController 생성", GUILayout.Height(22)))
        {
            if (string.IsNullOrWhiteSpace(comp.unitName))
            {
                EditorUtility.DisplayDialog("Error", "unitName이 비어 있습니다.", "OK");
                return;
            }
            if (comp.controller == null)
            {
                EditorUtility.DisplayDialog("Error", "controller(원본)가 비어 있습니다.", "OK");
                return;
            }

            // Assets 폴더 체인 보장 (AssetDatabase.CreateFolder 사용)
            string outDir = comp.outputDir.Replace("\\", "/");
            if (!outDir.StartsWith("Assets"))
            {
                EditorUtility.DisplayDialog("Error", "outputDir는 반드시 Assets 하위여야 합니다.", "OK");
                return;
            }
            string[] parts = outDir.Split('/');
            string cur = "Assets";
            for (int i = 1; i < parts.Length; i++)
            {
                if (string.IsNullOrEmpty(parts[i])) continue;
                string next = $"{cur}/{parts[i]}";
                if (!AssetDatabase.IsValidFolder(next))
                    AssetDatabase.CreateFolder(cur, parts[i]);
                cur = next;
            }

            // 경로 조합
            string fileName = comp.unitName;
            foreach (var ch in Path.GetInvalidFileNameChars()) fileName = fileName.Replace(ch, '_');
            if (string.IsNullOrEmpty(fileName)) fileName = "NewController";

            string savePath = $"{outDir}/{fileName}.controller";

            // 존재하면 에셋 기준으로 확인/삭제
            var existing = AssetDatabase.LoadAssetAtPath<AnimatorController>(savePath);
            if (existing != null)
            {
                if (!EditorUtility.DisplayDialog("Overwrite?",
                    $"{savePath} 가 이미 존재합니다.\n덮어쓸까요?", "Yes", "No")) return;
                AssetDatabase.DeleteAsset(savePath);
            }

            // 원본 경로 확인 후 복사
            string originalPath = AssetDatabase.GetAssetPath(comp.controller);
            if (string.IsNullOrEmpty(originalPath))
            {
                EditorUtility.DisplayDialog("Error", "원본 컨트롤러 경로를 찾을 수 없습니다.", "OK");
                return;
            }

            bool ok = AssetDatabase.CopyAsset(originalPath, savePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            if (!ok)
            {
                EditorUtility.DisplayDialog("Copy Failed", $"복사 실패\nFrom: {originalPath}\nTo: {savePath}", "OK");
                return;
            }

            // 새 에셋 선택 & GUI 종료(에디터 오류 방지)
            var newCtrl = AssetDatabase.LoadAssetAtPath<AnimatorController>(savePath);
            if (newCtrl != null)
            {
                Selection.activeObject = newCtrl;
                EditorGUIUtility.PingObject(newCtrl);
                GUIUtility.ExitGUI(); // ★ 여기 한 줄이 콘솔에 떴던 에러들 차단
            }
        }

        if (!string.IsNullOrEmpty(comp.lastClipPath))
        {
            EditorGUILayout.Space(4);
            EditorGUILayout.LabelField("Last Clip", comp.lastClipPath);
            if (GUILayout.Button("Ping Last Clip"))
            {
                var obj = AssetDatabase.LoadAssetAtPath<AnimationClip>(comp.lastClipPath);
                if (obj) EditorGUIUtility.PingObject(obj);
            }
        }
    }

    private static bool CanBuild(SpriteSheetClipBuilder c)
    {
        return c != null &&
               c.spritesheet != null &&
               !string.IsNullOrEmpty(c.clipName) &&
               c.fps > 0f &&
               !string.IsNullOrEmpty(c.outputDir) &&
               c.outputDir.Replace("\\", "/").StartsWith("Assets");
    }

    public static void BuildFromComponent(SpriteSheetClipBuilder c)
    {
        if (c == null || !c) return;
        if (!CanBuild(c))
        {
            EditorUtility.DisplayDialog("Build 실패", "설정을 확인하세요.\n• Spritesheet\n• FPS/ClipName\n• Output=Assets 하위", "OK");
            return;
        }

        var sprites = LoadSpritesFromTexture(c.spritesheet);
        if (sprites.Count == 0)
        {
            EditorUtility.DisplayDialog("No Sprites", "Sprite Mode=Multiple로 슬라이스했는지 확인하세요.", "OK");
            return;
        }
        string outputDir = Path.Combine(c.outputDir, c.unitName);
        Directory.CreateDirectory(outputDir);

        var clip = CreateClipFromSprites(sprites, c.fps, c.loop);

        string fileName = MakeSafeFileName(c.clipName) + ".anim";
        string clipPath = Path.Combine(outputDir, fileName).Replace("\\", "/");

        var existing = AssetDatabase.LoadAssetAtPath<AnimationClip>(clipPath);
        if (existing != null)
            AssetDatabase.DeleteAsset(clipPath);

        AssetDatabase.CreateAsset(clip, clipPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        c.lastClipPath = clipPath;
        EditorUtility.SetDirty(c);
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<AnimationClip>(clipPath);

        EditorUtility.DisplayDialog("완료", $"AnimationClip 생성 완료\n• Frames: {sprites.Count}\n• Path: {clipPath}", "OK");
    }

    private static List<Sprite> LoadSpritesFromTexture(Texture2D tex)
    {
        string path = AssetDatabase.GetAssetPath(tex);
        var all = AssetDatabase.LoadAllAssetsAtPath(path);
        return all.OfType<Sprite>()
                  .OrderBy(s => s.name, new NaturalComparer())
                  .ToList();
    }

    private static AnimationClip CreateClipFromSprites(List<Sprite> sprites, float fps, bool loop)
    {
        var clip = new AnimationClip { frameRate = fps };

        var binding = new EditorCurveBinding
        {
            type = typeof(SpriteRenderer),
            path = "",
            propertyName = "m_Sprite"
        };

        float frameTime = 1f / fps;
        var keys = new ObjectReferenceKeyframe[sprites.Count];
        for (int i = 0; i < sprites.Count; i++)
        {
            keys[i] = new ObjectReferenceKeyframe
            {
                time = i * frameTime,
                value = sprites[i]
            };
        }

        AnimationUtility.SetObjectReferenceCurve(clip, binding, keys);

        var settings = AnimationUtility.GetAnimationClipSettings(clip);
        settings.loopTime = loop;
        AnimationUtility.SetAnimationClipSettings(clip, settings);

        return clip;
    }

    private static string MakeSafeFileName(string name)
    {
        foreach (var ch in Path.GetInvalidFileNameChars())
            name = name.Replace(ch, '_');
        return name.Trim();
    }

    private class NaturalComparer : IComparer<string>
    {
        public int Compare(string a, string b) => EditorUtility.NaturalCompare(a, b);
    }
}
#endif

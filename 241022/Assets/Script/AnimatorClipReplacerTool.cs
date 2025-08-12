#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;

public class AnimatorClipReplacerTool : MonoBehaviour
{
    [Header("대상 Animator Controller")]
    public AnimatorController controller;

    [System.Serializable]
    public class ClipReplacement
    {
        public string stateName;        // 애니메이터 상태 이름
        public AnimationClip newClip;   // 교체할 애니메이션
    }

    [Header("교체할 클립 목록")]
    public List<ClipReplacement> replacements = new List<ClipReplacement>();
}

[CustomEditor(typeof(AnimatorClipReplacerTool))]
public class AnimatorClipReplacerToolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (target == null) return;
        base.OnInspectorGUI();

        var tool = (AnimatorClipReplacerTool)target;

        EditorGUILayout.Space();
        if (GUILayout.Button("Replace Clips in Controller", GUILayout.Height(30)))
        {
            ReplaceClips(tool);
        }
    }

    private void ReplaceClips(AnimatorClipReplacerTool tool)
    {
        if (tool.controller == null)
        {
            Debug.LogError("AnimatorController가 설정되지 않았습니다.");
            return;
        }

        Undo.RecordObject(tool.controller, "Replace Animation Clips");

        foreach (var rep in tool.replacements)
        {
            if (rep.newClip == null || string.IsNullOrEmpty(rep.stateName))
                continue;

            bool replaced = false;

            foreach (var layer in tool.controller.layers)
            {
                var state = FindStateRecursive(layer.stateMachine, rep.stateName);
                if (state != null)
                {
                    state.motion = rep.newClip;
                    replaced = true;
                    Debug.Log($"State '{rep.stateName}' → '{rep.newClip.name}' 교체 완료");
                }
            }

            if (!replaced)
                Debug.LogWarning($"State '{rep.stateName}'를 찾지 못했습니다.");
        }

        EditorUtility.SetDirty(tool.controller);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private AnimatorState FindStateRecursive(AnimatorStateMachine sm, string stateName)
    {
        foreach (var child in sm.states)
        {
            if (child.state.name == stateName)
                return child.state;
        }

        foreach (var subSm in sm.stateMachines)
        {
            var found = FindStateRecursive(subSm.stateMachine, stateName);
            if (found != null)
                return found;
        }

        return null;
    }
}
#endif

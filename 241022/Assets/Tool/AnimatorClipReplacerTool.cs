#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;

public class AnimatorClipReplacerTool : MonoBehaviour
{
    [Header("��� Animator Controller")]
    public AnimatorController controller;

    [System.Serializable]
    public class ClipReplacement
    {
        public string stateName;        // �ִϸ����� ���� �̸�
        public AnimationClip newClip;   // ��ü�� �ִϸ��̼�
    }

    [Header("��ü�� Ŭ�� ���")]
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
            Debug.LogError("AnimatorController�� �������� �ʾҽ��ϴ�.");
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
                    Debug.Log($"State '{rep.stateName}' �� '{rep.newClip.name}' ��ü �Ϸ�");
                }
            }

            if (!replaced)
                Debug.LogWarning($"State '{rep.stateName}'�� ã�� ���߽��ϴ�.");
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

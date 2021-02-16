using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SnUnityCommonUtils.Animation
{
    /// <summary>
    /// This utility automatically replaces position components in the all recorded keys with the local position of their
    /// transforms. For example, if the key has Z = 1, but the corresponding transform has Z = 0, then the key will have Z = 0
    /// (and the other keys in the other clips will also be modified).
    /// It can be useful in situations where you had to change the Z of a transform, but the many clips still have old value.
    /// 
    /// The utility can also update the local position only for transforms with the specific path.
    /// </summary>
    public class EditorWindowResetKeysToObjectPosition : EditorWindow
    {
        private bool _toggleX;
        private bool _toggleY;
        private bool _toggleZ;
        private string _pathFilter;
        private string _errorStr;

        [MenuItem("SnUtils/Animation/Reset Keys to Default Position...")]
        private static void ShowWindow()
        {
            var window = GetWindow<EditorWindowResetKeysToObjectPosition>();
            window.titleContent = new GUIContent("Reset Keys to Object Position");
            window.Show();
        }

        private void OnEnable()
        {
            _errorStr = "";
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Components to reset:");
            
            EditorGUI.indentLevel++;
            _toggleX = EditorGUILayout.Toggle("X", _toggleX);
            _toggleY = EditorGUILayout.Toggle("Y", _toggleY);
            _toggleZ = EditorGUILayout.Toggle("Z", _toggleZ);
            EditorGUI.indentLevel--;
            
            EditorGUILayout.LabelField("String that path should contains (empty for all):");
            _pathFilter = EditorGUILayout.TextField(_pathFilter);

            Animator animator = null;
            if (Selection.activeGameObject == null || !Selection.activeGameObject.TryGetComponent(out animator))
                _errorStr = "Please select a GameObject with Animator component, and then make this window focused.";
            else
                _errorStr = "";

            if (!string.IsNullOrEmpty(_errorStr))
            {
                EditorGUILayout.LabelField(_errorStr);
                EditorGUI.BeginDisabledGroup(true);
            }

            if (GUILayout.Button("Apply"))
                SetDefaultZToAllKeys(animator);

            if (!string.IsNullOrEmpty(_errorStr))
                EditorGUI.EndDisabledGroup();
        }

        private void SetDefaultZToAllKeys(Animator animator)
        {
            try
            {
                AssetDatabase.StartAssetEditing();

                var clips = animator.runtimeAnimatorController.animationClips;

                /*
                 * Keys - Paths to each child in the animator transform. The path has the same format as the path in each
                 * animation track.
                 * Values - localPosition of each child.
                 */
                var dict = animator.GetComponentsInChildren<Transform>().ToDictionary(
                    item => GetPathForObject(animator, item),
                    item => item.localPosition);

                foreach (var clip in clips)
                {
                    var curveBindings = AnimationUtility.GetCurveBindings(clip);

                    var clipChanged = false;
                    foreach (var curveBinding in curveBindings)
                    {
                        if (!string.IsNullOrEmpty(_pathFilter) && !curveBinding.path.Contains(_pathFilter))
                            continue;

                        if (TryGetValueFromDict(dict, curveBinding.path, curveBinding.propertyName, out var value))
                        {
                            var editorCurve = AnimationUtility.GetEditorCurve(clip, curveBinding);
                            for (var i = editorCurve.keys.Length - 1; i >= 0; i--)
                            {
                                var key = editorCurve.keys[i];
                                if (key.value != value)
                                {
                                    Debug.Log($"{curveBinding.path}: {key.value} -> {value}");
                                    editorCurve.RemoveKey(i);
                                    editorCurve.AddKey(key.time, value);

                                    clipChanged = true;
                                }
                            }

                            AnimationUtility.SetEditorCurve(clip, curveBinding, editorCurve);
                        }
                    }

                    if (clipChanged)
                        EditorUtility.SetDirty(clip);
                }
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                AssetDatabase.SaveAssets();
            }

        }

        private string GetPathForObject(Animator animator, Transform obj)
        {
            var list = new List<string>();

            var curObj = obj;
            while (curObj != null && curObj != animator.transform)
            {
                list.Add(curObj.name);
                curObj = curObj.parent;
            }

            list.Reverse();
            return string.Join("/", list).Trim('/');
        }

        private bool TryGetValueFromDict(Dictionary<string, Vector3> dict, string curvePath, string propName, out float value)
        {
            value = default;
            if (propName == "m_LocalPosition.x")
            {
                if (_toggleX && dict.TryGetValue(curvePath, out var localPos))
                {
                    value = localPos.x;
                    return true;
                }

                return false;
            }

            if (propName == "m_LocalPosition.y")
            {
                if (_toggleY && dict.TryGetValue(curvePath, out var localPos))
                {
                    value = localPos.y;
                    return true;
                }

                return false;
            }

            if (propName == "m_LocalPosition.z")
            {
                if (_toggleZ && dict.TryGetValue(curvePath, out var localPos))
                {
                    value = localPos.z;
                    return true;
                }

                return false;
            }

            return false;
        }
    }
}
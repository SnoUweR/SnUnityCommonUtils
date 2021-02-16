using UnityEditor;
using UnityEngine;

namespace SnUnityCommonUtils.Building
{
    public class EditorWindowAutoKeystore : EditorWindow
    {
        [MenuItem("SnUtils/Building/Save Keystore Credentials...")]
        private static void ShowWindow()
        {
            var window = GetWindow<EditorWindowAutoKeystore>();
            window.titleContent = new GUIContent("Set Keystore");
            window.Show();
        }

        private void OnGUI()
        {
            EditorAutoKeystorePrefs.RestoreKeystore = EditorGUILayout.Toggle("Enabled", EditorAutoKeystorePrefs.RestoreKeystore);
            EditorAutoKeystorePrefs.AliasName = EditorGUILayout.TextField("Alias Name", EditorAutoKeystorePrefs.AliasName);
            EditorAutoKeystorePrefs.AliasPass = EditorGUILayout.PasswordField("Alias Pass", EditorAutoKeystorePrefs.AliasPass);
            EditorAutoKeystorePrefs.KeystorePass = EditorGUILayout.PasswordField("Keystore Pass", EditorAutoKeystorePrefs.KeystorePass);

            if (GUILayout.Button("Set Now"))
                EditorAutoKeystorePrefs.SetNow();
        }
    }
}
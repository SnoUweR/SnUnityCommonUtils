using UnityEditor;

namespace SnUnityCommonUtils.Building
{
    /// <summary>
    /// Automatically restore keystore name and passwords from the local editor prefs on each Unity Editor start.
    /// You can save these parameters in <see cref="EditorWindowAutoKeystore"/>.
    /// Warning! It can be dangerous to save your keystore credentials on the public computers, because EditorPrefs
    /// can be viewed by anyone!
    /// </summary>
    [InitializeOnLoad]
    public class EditorAutoKeystorePrefs
    {
        private static string KeystorePassKey => $"{PlayerSettings.productName}_KEYSTORE_PASS";
        private static string AliasPassKey => $"{PlayerSettings.productName}_ALIAS_PASS";
        private static string AliasNameKey => $"{PlayerSettings.productName}_ALIAS_NAME";
        private static string RestoreKeystoreKey => $"{PlayerSettings.productName}_RESTORE_KEYSTORE";
        
        public static string KeystorePass
        {
            get => EditorPrefs.GetString(KeystorePassKey);
            set => EditorPrefs.SetString(KeystorePassKey, value);
        }
        
        public static string AliasPass
        {
            get => EditorPrefs.GetString(AliasPassKey);
            set => EditorPrefs.SetString(AliasPassKey, value);
        }
        
        public static string AliasName
        {
            get => EditorPrefs.GetString(AliasNameKey);
            set => EditorPrefs.SetString(AliasNameKey, value);
        }

        public static bool RestoreKeystore
        {
            get => EditorPrefs.GetBool(RestoreKeystoreKey);
            set => EditorPrefs.SetBool(RestoreKeystoreKey, value);
        }
        
        static EditorAutoKeystorePrefs()
        {
            if (RestoreKeystore)
                SetNow();
        }

        public static void SetNow()
        {
            PlayerSettings.Android.keystorePass = KeystorePass;
            PlayerSettings.Android.keyaliasName = AliasName;
            PlayerSettings.Android.keyaliasPass = AliasPass;
        }
    }
}
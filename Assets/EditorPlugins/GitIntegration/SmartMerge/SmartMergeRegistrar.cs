#if UNITY_EDITOR
using System;
using GitIntegration;
using UnityEditor;
using UnityEngine;

// Плагин для установки корректных настроек гита
namespace EditorPlugins.GitIntegration.SmartMerge
{
    [InitializeOnLoad]
    public class SmartMergeRegistrar
    {
        private const string SmartMergeRegistrarEditorPrefsKey = "smart_merge_installed";
        private const int Version = 1;
        private static readonly string UnityVersionKey = $"{Version}_{Application.unityVersion}";

        // Регистрируем UnityYAMLMerge
        [MenuItem("Tools/Git/SmartMerge registration")]
        private static void SmartMergeRegister()
        {
            try
            {
                var unityYamlMergePath = $"{EditorApplication.applicationPath}/Tools/UnityYAMLMerge";
                Utils.ExecuteGitWithParams("config merge.unityyamlmerge.name \"Unity SmartMerge (UnityYamlMerge)\"");
                Utils.ExecuteGitWithParams(
                    $"config merge.unityyamlmerge.driver \"\\\"{unityYamlMergePath}\\\" merge -h -p --force --fallback none %O %B %A %A\"");
                Utils.ExecuteGitWithParams("config merge.unityyamlmerge.recursive binary");
                EditorPrefs.SetString(SmartMergeRegistrarEditorPrefsKey, UnityVersionKey);
                Debug.Log($"Successfully registered UnityYAMLMerge with path {unityYamlMergePath}");
            }
            catch (Exception e)
            {
                Debug.Log($"Fail to register UnityYAMLMerge with error: {e}");
            }
        }

        // Отменяем регистрацию UnityYAMLMerge
        [MenuItem("Tools/Git/SmartMerge unregistration")]
        private static void SmartMergeUnregister()
        {
            Utils.ExecuteGitWithParams("config --remove-section merge.unityyamlmerge");
            Debug.Log("Successfully unregistered UnityYAMLMerge");
        }

        // Unity вызывает статические конструкторы при запуске редактора
        static SmartMergeRegistrar()
        {
            var installedVersionKey = EditorPrefs.GetString(SmartMergeRegistrarEditorPrefsKey);
            if (installedVersionKey != UnityVersionKey)
            {
                SmartMergeRegister();
            }
        }
    }
}
#endif
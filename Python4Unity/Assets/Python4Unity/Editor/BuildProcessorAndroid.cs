using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Python4Unity
{
    public class BuildProcessorAndroid : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        bool isStartedPostProcess_ = false;

        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
#if UNITY_ANDROID
            if (!AssetDatabase.IsValidFolder("Assets/Plugins"))
            {
                AssetDatabase.CreateFolder("Assets", "Plugins");
            }
            if (!AssetDatabase.IsValidFolder("Assets/Plugins/Android"))
            {
                AssetDatabase.CreateFolder("Assets/Plugins", "Android");
            }

            if (File.Exists("Assets/Plugins/Android/baseProjectTemplate.gradle"))
            {
                File.Copy("Assets/Plugins/Android/baseProjectTemplate.gradle", "Assets/Plugins/Android/baseProjectTemplate.gradle.pybackup");
            }
            else
            {
                File.Copy(Path.Combine(Path.GetDirectoryName(EditorApplication.applicationPath), "Data/PlaybackEngines/AndroidPlayer/Tools/GradleTemplates/baseProjectTemplate.gradle"),
                    "Assets/Plugins/Android/baseProjectTemplate.gradle");
            }
            string contents = File.ReadAllText("Assets/Plugins/Android/baseProjectTemplate.gradle");
            if (!contents.Contains("id 'com.chaquo.python' version '15.0.1' apply false // ADD (Python4Unity)"))
            {
                contents = contents.Replace("**BUILD_SCRIPT_DEPS**", "**BUILD_SCRIPT_DEPS**\n    id 'com.chaquo.python' version '15.0.1' apply false // ADD (Python4Unity)");
                File.WriteAllText("Assets/Plugins/Android/baseProjectTemplate.gradle", contents);
            }

            if (File.Exists("Assets/Plugins/Android/mainTemplate.gradle"))
            {
                File.Copy("Assets/Plugins/Android/mainTemplate.gradle", "Assets/Plugins/Android/mainTemplate.gradle.pybackup");
            }
            else
            {
                File.Copy(Path.Combine(Path.GetDirectoryName(EditorApplication.applicationPath), "Data/PlaybackEngines/AndroidPlayer/Tools/GradleTemplates/mainTemplate.gradle"),
                    "Assets/Plugins/Android/mainTemplate.gradle");
            }
            contents = File.ReadAllText("Assets/Plugins/Android/mainTemplate.gradle");
            if (!contents.Contains("// ADD (Python4Unity)"))
            {
                contents = contents.Replace("**APPLY_PLUGINS**", "**APPLY_PLUGINS**\napply plugin: 'com.chaquo.python' // ADD (Python4Unity)");
                string pythonPath = "C:/Users/satoh/anaconda3/envs/unity/python.exe";
                string version = "3.8";
                string requirementsPath = Path.GetFullPath("Assets/requirements.txt").Replace("\\", "/");
                string addContents = "// ADD (Python4Unity)\nchaquopy {\n    defaultConfig {\n        version \"" + version + "\"\n        buildPython(\"" + pythonPath + "\")\n        pip {\n            install \"-r\", \"" + requirementsPath + "\"\n        }\n    }\n    productFlavors { }\n}";
                contents = contents + "\n" + addContents;
                File.WriteAllText("Assets/Plugins/Android/mainTemplate.gradle", contents);
            }

            AssetDatabase.Refresh();

            EditorApplication.update += postProcess;
#endif
        }

        void postProcess()
        {
            if (isStartedPostProcess_)
            {
                return;
            }
            isStartedPostProcess_ = true;

#if UNITY_ANDROID
            AssetDatabase.DeleteAsset("Assets/Plugins/Android/baseProjectTemplate.gradle");
            if (File.Exists("Assets/Plugins/Android/baseProjectTemplate.gradle.pybackup"))
            {
                AssetDatabase.MoveAsset("Assets/Plugins/Android/baseProjectTemplate.gradle.pybackup", "Assets/Plugins/Android/baseProjectTemplate.gradle");
            }

            AssetDatabase.DeleteAsset("Assets/Plugins/Android/mainTemplate.gradle");
            if (File.Exists("Assets/Plugins/Android/mainTemplate.gradle.pybackup"))
            {
                AssetDatabase.MoveAsset("Assets/Plugins/Android/mainTemplate.gradle.pybackup", "Assets/Plugins/Android/mainTemplate.gradle");
            }
#endif

            EditorApplication.update -= postProcess;
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            postProcess();
        }
    }
}
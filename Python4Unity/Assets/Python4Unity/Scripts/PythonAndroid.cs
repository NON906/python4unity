using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Python4Unity
{
    public class PythonAndroid : IPython
    {
        static bool isStarted_ = false;
        AndroidJavaObject pythonObject_ = null;

        public static void Start()
        {
            if (isStarted_)
            {
                return;
            }

            using AndroidJavaClass python = new AndroidJavaClass("com.chaquo.python.Python");
            using AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            using AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            using AndroidJavaObject androidPlatform = new AndroidJavaObject("com.chaquo.python.android.AndroidPlatform", activity);
            python.CallStatic("start", androidPlatform);
            isStarted_ = true;
        }

        public PythonAndroid()
        {
            using AndroidJavaClass python = new AndroidJavaClass("com.chaquo.python.Python");
            pythonObject_ = python.CallStatic<AndroidJavaObject>("getInstance");
        }

        public void Dispose()
        {
            if (pythonObject_ != null)
            {
                pythonObject_.Dispose();
                pythonObject_ = null;
            }
        }

        ~PythonAndroid()
        {
            Dispose();
        }

        public IPyObject GetModule(string name)
        {
            var rawModule = pythonObject_.Call<AndroidJavaObject>("getModule", name);
            return new PyObjectAndroid(rawModule);
        }

        public IPyObject GetBuiltins()
        {
            var rawModule = pythonObject_.Call<AndroidJavaObject>("getBuiltins");
            return new PyObjectAndroid(rawModule);
        }
    }
}

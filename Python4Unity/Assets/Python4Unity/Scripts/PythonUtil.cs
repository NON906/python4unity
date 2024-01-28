using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Python4Unity
{
    public static class PythonUtil
    {
        public static IPython GetInstance()
        {
#if UNITY_ANDROID
            PythonAndroid.Start();
            return new PythonAndroid();
#else
            return null;
#endif
        }
    }
}

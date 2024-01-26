using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Python4Unity
{
    public interface IPython : IDisposable
    {
        static IPython GetInstance()
        {
#if UNITY_ANDROID
            PythonAndroid.Start();
            return new PythonAndroid();
#else
            return null;
#endif
        }

        IPyObject GetModule(string name);
        IPyObject GetBuiltins();
    }
}

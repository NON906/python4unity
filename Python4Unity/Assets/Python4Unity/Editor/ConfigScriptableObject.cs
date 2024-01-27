using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Python4Unity
{
    //[CreateAssetMenu(fileName = "Python4Unity_Config", menuName = "Python4Unity/ConfigScriptableObject", order = 1)]
    public class ConfigScriptableObject : ScriptableObject
    {
        public string PythonPath = "python";
        public string Version = "3.8";
    }
}

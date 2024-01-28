using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Python4Unity
{
    //[CreateAssetMenu(fileName = "Python4Unity_Config", menuName = "Python4Unity/Config")]
    public class ConfigScriptableObject : ScriptableObject
    {
        public string PythonPath = "";
        public string Version = "";
    }
}

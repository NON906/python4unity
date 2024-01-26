using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Python4Unity.Sample
{
    public class APIExample2 : MonoBehaviour
    {
        void Start()
        {
            using var py = IPython.GetInstance();
            var sys = py.GetModule("sys");
            UnityEngine.Debug.Log(sys.Get<long>("maxsize"));
            UnityEngine.Debug.Log(sys.Get<string>("version"));
            UnityEngine.Debug.Log(sys.CallAttr<bool>("is_finalizing"));
        }
    }
}

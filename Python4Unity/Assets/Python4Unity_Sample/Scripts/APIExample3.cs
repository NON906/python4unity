using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Python4Unity.Sample
{
    public class APIExample3 : MonoBehaviour
    {
        void Start()
        {
            using var py = IPython.GetInstance();

            var sys = py.GetModule("sys");
            UnityEngine.Debug.Log(sys.Get<IPyObject>("version_info").ToList<string>()[0]);

            var os = py.GetModule("os");
            var hello = os.Get<IPyObject>("environ").ToDictionary<string, string>();
            hello["HELLO"] = "world";
            os.Get<IPyObject>("environ").SetFrom(hello);
            hello = os.Get<IPyObject>("environ").ToDictionary<string, string>();
            UnityEngine.Debug.Log(hello["HELLO"]);
        }
    }
}

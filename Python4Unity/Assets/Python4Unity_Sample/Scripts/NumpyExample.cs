using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Python4Unity.Sample
{
    public class NumpyExample : MonoBehaviour
    {
        void Start()
        {
            using var py = PythonUtil.GetInstance();

            var useNumpy = py.GetModule("useNumpy");
            UnityEngine.Debug.Log(useNumpy.CallAttr<int>("getRand", 100));
            var result = useNumpy.CallAttr<int[]>("getMaxAndMin", new int[] { 1, 2, 3, 4, 5 });
            UnityEngine.Debug.Log("Max: " + result[0] + ", Min: " + result[1]);
        }
    }
}

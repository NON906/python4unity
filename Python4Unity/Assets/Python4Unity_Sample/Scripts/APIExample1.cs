using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Python4Unity.Sample
{
    public class APIExample1 : MonoBehaviour
    {
        void Start()
        {
            File.WriteAllText(Application.persistentDataPath + "/example.txt", "This is a sample text file.");

            using var py = PythonUtil.GetInstance();
            var zipfile = py.GetModule("zipfile");
            var zf = zipfile.CallAttr<IPyObject>("ZipFile", Application.persistentDataPath + "/example.zip", "w");
            zf.CallAttr("write", Application.persistentDataPath + "/example.txt",
                new KeyValuePair<string, object>("compress_type", zipfile.Get<IPyObject>("ZIP_STORED")));
        }
    }
}

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
            PythonEditor.Start();
            return new PythonEditor();
        }
    }
}

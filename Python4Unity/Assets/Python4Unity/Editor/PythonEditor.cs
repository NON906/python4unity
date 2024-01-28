using Python.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Scripting.Python;
using UnityEngine;

namespace Python4Unity
{
    public class PythonEditor : IPython
    {
        static bool isStarted_ = false;
        Py.GILState gilState_ = null;

        public static void Start()
        {
            if (isStarted_)
            {
                return;
            }

            PythonRunner.EnsureInitialized();

            isStarted_ = true;
        }

        public PythonEditor()
        {
            gilState_ = Py.GIL();
        }

        public void Dispose()
        {
            if (gilState_ != null)
            {
                gilState_.Dispose();
                gilState_ = null;
            }
        }

        ~PythonEditor()
        {
            Dispose();
        }

        public IPyObject GetModule(string name)
        {
            return new PyObjectEditor(Py.Import(name));
        }

        public IPyObject GetBuiltins()
        {
            return new PyObjectEditor(Py.Import("builtins"));
        }
    }
}

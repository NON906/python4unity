using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Python4Unity
{
    public interface IPython : IDisposable
    {
        IPyObject GetModule(string name);
        IPyObject GetBuiltins();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Python4Unity
{
    public interface IPyObject
    {
        void CallAttr(string key, params object[] args);
        T CallAttr<T>(string key, params object[] args);
        void Call(params object[] args);
        T Call<T>(params object[] args);
        void Put(string key, object value);
        T Get<T>(object key);
        T ToValue<T>();
        T[] ToArray<T>();
        List<T> ToList<T>();
        Dictionary<T0, T1> ToDictionary<T0, T1>();
        void SetFrom<T>(T[] array);
        void SetFrom<T>(IEnumerable<T> list);
        void SetFrom<T0, T1>(IDictionary<T0, T1> dict);
    }
}

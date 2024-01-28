using Python.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Python4Unity
{
    public class PyObjectEditor : IPyObject
    {
        dynamic rawPyObject_;
        dynamic rawPyBuiltins_;

        public PyObjectEditor(dynamic rawPyObject)
        {
            rawPyObject_ = rawPyObject;
            rawPyBuiltins_ = Py.Import("builtins");
        }

        T convertTo<T>(dynamic input)
        {
            if (typeof(T) == typeof(IPyObject) || typeof(T) == typeof(PyObjectEditor))
            {
                return (T)(object)new PyObjectEditor(input);
            }
            else if (typeof(T).IsArray)
            {
                var newPyObj = new PyObjectEditor(input);
                var innerTypes = typeof(T).GenericTypeArguments;
                var method = typeof(PyObjectEditor).GetMethod("ToArray", 1, new Type[] { });
                if (innerTypes.Length > 0)
                {
                    method = method.MakeGenericMethod(innerTypes);
                }
                else
                {
                    method = method.MakeGenericMethod(new[] { typeof(T).GetElementType() });
                }
                return (T)method.Invoke(newPyObj, null);
            }
            else if (typeof(T).Name == typeof(Dictionary<,>).Name || typeof(T).Name == typeof(IDictionary<,>).Name)
            {
                var newPyObj = new PyObjectEditor(input);
                var innerTypes = typeof(T).GenericTypeArguments;
                var method = typeof(PyObjectEditor).GetMethod("ToDictionary", 2, new Type[] { });
                method = method.MakeGenericMethod(innerTypes);
                return (T)method.Invoke(newPyObj, null);
            }
            else if (typeof(T).Name == typeof(List<>).Name || typeof(T).Name == typeof(ICollection<>).Name)
            {
                var newPyObj = new PyObjectEditor(input);
                var innerTypes = typeof(T).GenericTypeArguments;
                var method = typeof(PyObjectEditor).GetMethod("ToList", 1, new Type[] { });
                method = method.MakeGenericMethod(innerTypes);
                return (T)method.Invoke(newPyObj, null);
            }
            else
            {
                return (T)input;
            }
        }

        object parseArg(object arg)
        {
            PyObject parseArray(Array array)
            {
                var list = new PyList();
                for (int loop = 0; loop < array.Length; loop++)
                {
                    list.Append(array.GetValue(loop).ToPython());
                }
                return list;
            }

            if (arg.GetType().Name == typeof(KeyValuePair<,>).Name)
            {
                var key = arg.GetType().GetProperty("Key").GetValue(arg);
                var value = arg.GetType().GetProperty("Value").GetValue(arg);
                if (value is PyObjectEditor)
                {
                    value = ((PyObjectEditor)value).rawPyObject_;
                }
                var newArg = new KeyValuePair<PyObject, PyObject>(key.ToPython(), value.ToPython());
                return newArg;
            }
            else if (arg.GetType().GetInterfaces().Any(item => item.Name == typeof(IDictionary<,>).Name))
            {
               var dict = new PyDict();

                foreach (var item in (IEnumerable)arg)
                {
                    var key = item.GetType().GetProperty("Key").GetValue(item).ToString();
                    var value = item.GetType().GetProperty("Value").GetValue(item);

                    dict.Append(key.ToPython());
                    dict.Append(value.ToPython());
                }

                return dict;
            }
            else if (arg.GetType().IsArray)
            {
                return parseArray((Array)arg);
            }
            else if (arg.GetType().GetInterfaces().Any(item => item.Name == typeof(ICollection<>).Name))
            {
                return parseArray(arg.GetType().GetEnumValues());
            }

            return arg.ToPython();
        }

        class Args
        {
            public List<PyObject> List = new List<PyObject>();
            public PyDict Dict = new PyDict();
        }

        Args parseArgs(object[] args)
        {
            var newArgs = new Args();
            foreach (object arg in args)
            {
                var arg2 = parseArg(arg);
                if (arg2 is KeyValuePair<PyObject, PyObject>)
                {
                    var pair = (KeyValuePair<PyObject, PyObject>)arg2;
                    newArgs.Dict[pair.Key] = pair.Value;
                }
                else
                {
                    newArgs.List.Add((PyObject)arg2);
                }
            }
            return newArgs;
        }

        public void Call(params object[] args)
        {
            var newArgs = parseArgs(args);
            ((PyObject)rawPyObject_).Invoke(newArgs.List.ToArray(), newArgs.Dict);
        }

        public T Call<T>(params object[] args)
        {
            var newArgs = parseArgs(args);
            var ret = ((PyObject)rawPyObject_).Invoke(newArgs.List.ToArray(), newArgs.Dict);
            return convertTo<T>(ret);
        }

        public void CallAttr(string key, params object[] args)
        {
            var newArgs = parseArgs(args);
            ((PyObject)rawPyObject_).InvokeMethod(key, newArgs.List.ToArray(), newArgs.Dict);
        }

        public T CallAttr<T>(string key, params object[] args)
        {
            var newArgs = parseArgs(args);
            var ret = ((PyObject)rawPyObject_).InvokeMethod(key, newArgs.List.ToArray(), newArgs.Dict);
            return convertTo<T>(ret);
        }

        public T Get<T>(object key)
        {
            dynamic ret = rawPyObject_.Variables()[key.ToPython()];
            return convertTo<T>(ret);
        }

        public void Put(string key, object value)
        {
            rawPyObject_[key.ToPython()] = value.ToPython();
        }

        public void SetFrom<T>(T[] array)
        {
            for (int loop = 0; loop < array.Length; loop++)
            {
                rawPyObject_[loop] = array[loop];
            }
        }

        public void SetFrom<T>(IEnumerable<T> list)
        {
            int loop = 0;
            foreach (var item in list)
            {
                rawPyObject_[loop] = item;
                loop++;
            }
        }

        public void SetFrom<T0, T1>(IDictionary<T0, T1> dict)
        {
            foreach (var pair in dict)
            {
                rawPyObject_[pair.Key.ToPython()] = pair.Value.ToPython();
            }
        }

        public T[] ToArray<T>()
        {
            var ret = new T[rawPyBuiltins_.len(rawPyObject_)];
            if (typeof(T) == typeof(string))
            {
                for (int loop = 0; loop < ret.Length; loop++)
                {
                    ret[loop] = rawPyObject_[loop].ToString();
                }
            }
            else
            {
                for (int loop = 0; loop < ret.Length; loop++)
                {
                    ret[loop] = (T)rawPyObject_[loop];
                }
            }
            return ret;
        }

        public Dictionary<T0, T1> ToDictionary<T0, T1>()
        {
            var keysList = rawPyObject_.keys();
            var length = (int)rawPyBuiltins_.len(keysList);
            var ret = new Dictionary<T0, T1>();
            for (int loop = 0; loop < length; loop++)
            {
                var key = rawPyBuiltins_.list(keysList).GetItem(loop);
                if (typeof(T0) == typeof(string))
                {
                    ret[key.ToString()] = (T1)rawPyObject_.GetItem(key);
                }
                else
                {
                    ret[(T0)key] = (T1)rawPyObject_.GetItem(key);
                }
            }
            return ret;
        }

        public List<T> ToList<T>()
        {
            return ToArray<T>().ToList();
        }

        public T ToValue<T>()
        {
            return (T)rawPyObject_;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

namespace Python4Unity
{
    public class PyObjectAndroid : IPyObject
    {
        AndroidJavaObject rawPyObject_ = null;

        public PyObjectAndroid(AndroidJavaObject rawPyObject)
        {
            rawPyObject_ = rawPyObject;
        }

        T convertTo<T>(AndroidJavaObject input)
        {
            if (typeof(T) == typeof(bool))
            {
                return input.Call<T>("toBoolean");
            }
            else if (typeof(T) == typeof(byte))
            {
                return input.Call<T>("toByte");
            }
            else if (typeof(T) == typeof(char))
            {
                return input.Call<T>("toChar");
            }
            else if (typeof(T) == typeof(double))
            {
                return input.Call<T>("toDouble");
            }
            else if (typeof(T) == typeof(float))
            {
                return input.Call<T>("toFloat");
            }
            else if (typeof(T) == typeof(int))
            {
                return input.Call<T>("toInt");
            }
            else if (typeof(T) == typeof(long))
            {
                return input.Call<T>("toLong");
            }
            else if (typeof(T) == typeof(short))
            {
                return input.Call<T>("toShort");
            }
            else if (typeof(T) == typeof(string))
            {
                return input.Call<T>("toString");
            }
            else if (typeof(T) == typeof(IPyObject) || typeof(T) == typeof(PyObjectAndroid))
            {
                return (T)(object)new PyObjectAndroid(input);
            }
            else
            {
                return (T)(object)input;
            }
        }

        AndroidJavaObject parseArg(object arg)
        {
            if (arg is KeyValuePair<string, object>)
            {
                KeyValuePair<string, object> castedArg = (KeyValuePair<string, object>)arg;
                var value = castedArg.Value;
                if (value is PyObjectAndroid)
                {
                    value = ((PyObjectAndroid)value).rawPyObject_;
                }
                var newArg = new AndroidJavaObject("com.chaquo.python.Kwarg", castedArg.Key, value);
                return newArg;
            }
            /*
            else if (arg is IDictionary<string, object>)
            {
                foreach (var pair in (IDictionary<string, object>)arg)
                {
                    var value = pair.Value;
                    if (value is PyObjectAndroid)
                    {
                        value = ((PyObjectAndroid)value).rawPyObject_;
                    }
                    var newArg = new AndroidJavaObject("com.chaquo.python.Kwarg", pair.Key, value);
                    newArgs.Add(newArg);
                }
            }
            */
            else if (arg is object[])
            {
                using var javaObjClass = new AndroidJavaClass("java.lang.Object");
                using var javaArrayClass = new AndroidJavaClass("java.lang.Array");
                int length = ((object[])arg).Length;
                var javaArray = javaArrayClass.CallStatic<AndroidJavaObject>("newInstance", javaObjClass, length);
                for (int loop = 0; loop < length; loop++)
                {
                    javaArrayClass.CallStatic("set", javaArray, loop, ((object[])arg)[loop]);
                }
                return javaArray;
            }
            else if (arg is List<object>)
            {
                using var javaObjClass = new AndroidJavaClass("java.lang.Object");
                using var javaArrayClass = new AndroidJavaClass("java.lang.Array");
                int length = ((List<object>)arg).Count;
                var javaArray = javaArrayClass.CallStatic<AndroidJavaObject>("newInstance", javaObjClass, length);
                for (int loop = 0; loop < length; loop++)
                {
                    javaArrayClass.CallStatic("set", javaArray, loop, ((object[])arg)[loop]);
                }
                return javaArray;
            }
            else if (arg is PyObjectAndroid)
            {
                return ((PyObjectAndroid)arg).rawPyObject_;
            }
            else if (arg is bool)
            {
                return new AndroidJavaObject("java.lang.Boolean", arg);
            }
            else if (arg is byte)
            {
                return new AndroidJavaObject("java.lang.Byte", arg);
            }
            else if (arg is char)
            {
                return new AndroidJavaObject("java.lang.Character", arg);
            }
            else if (arg is double)
            {
                return new AndroidJavaObject("java.lang.Double", arg);
            }
            else if (arg is float)
            {
                return new AndroidJavaObject("java.lang.Float", arg);
            }
            else if (arg is int)
            {
                return new AndroidJavaObject("java.lang.Integer", arg);
            }
            else if (arg is long)
            {
                return new AndroidJavaObject("java.lang.Long", arg);
            }
            else if (arg is short)
            {
                return new AndroidJavaObject("java.lang.Short", arg);
            }
            else if (arg is string)
            {
                return new AndroidJavaObject("java.lang.String", arg);
            }
            else if (arg is AndroidJavaObject)
            {
                return (AndroidJavaObject)arg;
            }
            else
            {
                throw new Exception("Unsupported type.");
            }
        }

        AndroidJavaObject[] parseArgs(object[] args)
        {
            List<AndroidJavaObject> newArgs = new List<AndroidJavaObject>();
            foreach (object arg in args)
            {
                newArgs.Add(parseArg(arg));
            }
            return newArgs.ToArray();
        }

        AndroidJavaObject callAttr(string key, params object[] args)
        {
            return rawPyObject_.Call<AndroidJavaObject>("callAttr", key, parseArgs(args));
        }

        public void CallAttr(string key, params object[] args)
        {
            callAttr(key, args);
        }

        public T CallAttr<T>(string key, params object[] args)
        {
            AndroidJavaObject result = callAttr(key, args);
            return convertTo<T>(result);
        }

        AndroidJavaObject call(params object[] args)
        {
            return rawPyObject_.Call<AndroidJavaObject>("call", (object)parseArgs(args));
        }

        public void Call(params object[] args)
        {
            call(args);
        }

        public T Call<T>(params object[] args)
        {
            AndroidJavaObject result = call(args);
            return convertTo<T>(result);
        }

        public void Put(string key, object value)
        {
            rawPyObject_.Call<AndroidJavaObject>("put", key, parseArg(value));
        }

        public T Get<T>(object key)
        {
            AndroidJavaObject result = rawPyObject_.Call<AndroidJavaObject>("get", key);
            return convertTo<T>(result);
        }

        public T[] ToArray<T>()
        {
            using var javaList = rawPyObject_.Call<AndroidJavaObject>("asList");
            int length = javaList.Call<int>("size");

            var array = new T[length];
            for (int loop = 0; loop < length; loop++)
            {
                var result = javaList.Call<AndroidJavaObject>("get", loop);
                array[loop] = convertTo<T>(result);
            }

            return array;
        }

        public List<T> ToList<T>()
        {
            return ToArray<T>().ToList();
        }

        public Dictionary<T0, T1> ToDictionary<T0, T1>()
        {
            using var javaMap = rawPyObject_.Call<AndroidJavaObject>("asMap");
            using var keySet = javaMap.Call<AndroidJavaObject>("keySet");
            using var keysArray = keySet.Call<AndroidJavaObject>("toArray");
            using var arrayClass = new AndroidJavaClass("java.lang.reflect.Array");
            int length = arrayClass.CallStatic<int>("getLength", keysArray);

            var dict = new Dictionary<T0, T1>();
            for (int loop = 0; loop < length; loop++)
            {
                var keyTemp = arrayClass.CallStatic<AndroidJavaObject>("get", keysArray, loop);
                var key = convertTo<T0>(keyTemp);
                var valueTemp = javaMap.Call<AndroidJavaObject>("get", key);
                var value = convertTo<T1>(valueTemp);
                dict.Add(key, value);
            }

            return dict;
        }

        public void SetFrom<T>(T[] array)
        {
            using var javaList = rawPyObject_.Call<AndroidJavaObject>("asList");
            using var clazz = new AndroidJavaClass("com.chaquo.python.PyObject");
            javaList.Call("clear");
            for (int loop = 0; loop < array.Length; loop++)
            {
                using var setObj = clazz.CallStatic<AndroidJavaObject>("fromJava", array[loop]);
                javaList.Call("set", loop, setObj);
            }
        }

        public void SetFrom<T>(IEnumerable<T> list)
        {
            SetFrom(list.ToArray());
        }

        public void SetFrom<T0, T1>(IDictionary<T0, T1> dict)
        {
            using var javaMap = rawPyObject_.Call<AndroidJavaObject>("asMap");
            using var clazz = new AndroidJavaClass("com.chaquo.python.PyObject");
            javaMap.Call("clear");
            foreach (var pair in dict)
            {
                using var putKey = clazz.CallStatic<AndroidJavaObject>("fromJava", pair.Key);
                using var putValue = clazz.CallStatic<AndroidJavaObject>("fromJava", pair.Value);
                javaMap.Call<AndroidJavaObject>("put", putKey, putValue);
            }
        }
    }
}

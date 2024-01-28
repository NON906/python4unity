# Chaquopy4Unity

(日本語版は[こちら](README_ja.md))

This is a wrapper for using [Chaquopy](https://chaquo.com/chaquopy/), which can run Python on Android, with Unity.

It confirmed with Unity 2022.3.17f1.

NOTE:  
Works only on Android and Editor.  
It does not work on apps built other than Android.

## How to install

Add the following git repository using the "+" button in Package Manager.

```
https://github.com/NON906/python4unity.git?path=Python4Unity/Assets/Python4Unity
```

When running the sample scene NumpyExample, when using libraries such as numpy or external scripts, please also do the following.

1. Add the following path (or path with Python script) to Player Settings -> Python Scripting -> Package Directories.
```
Assets/Samples/Chaquopy4Unity/0.0.1/Samples/Python
```
2. Create ``ProjectSettings/requirements.txt`` and enter the following content (or the library you want to use).
```
numpy
```
3. Restart Unity Editor.

Please refer to the sample scene for usage instructions.
# Chaquopy4Unity

Android上でPythonを動作させることが出来る[Chaquopy](https://chaquo.com/chaquopy/)をUnityで使うためのラッパーです。

Unity 2022.3.17f1で動作確認しています。

NOTE:  
AndroidおよびEditor上でのみ動作します。  
Android以外にビルドしたアプリ上では動作しません。

## インストール方法

Package Managerの「+」ボタンで以下のgitリポジトリを追加してください。

```
https://github.com/NON906/python4unity.git?path=Python4Unity/Assets/Python4Unity
```

サンプルシーンのNumpyExampleを動作させる場合、numpyなどのライブラリや外部のスクリプトを使用する場合は以下も行ってください。

1. Player SettingsのPython Scriptingにある、Package Directoriesに以下のパス（もしくはPythonスクリプトのあるパス）を追加する
```
Assets/Samples/Chaquopy4Unity/0.0.1/Samples/Python
```
2. ``ProjectSettings/requirements.txt``を作成し、以下の内容（もしくは使用したいライブラリ）を入力する
```
numpy
```
3. Unity Editorを再起動する

使用方法はサンプルシーンを参考にしてください。
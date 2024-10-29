using CompilerTestC.CompileCode;
using CompilerTestC.WorkingWithFiles;
using System.Diagnostics;




string cFilePath = "test.c"; 
string cSharpFilePath = "test.cs"; 
string pyFilePath = "test.py"; 
string javaFilePath = "test.java"; 
string cOutputFileName = "finish"; 

CompileCCode.CompileC(cFilePath, cOutputFileName);
CompileCCode.CompileCSharp(cSharpFilePath);

RunCode.RunCScript(cOutputFileName);
RunCode.RunCScript("Test.exe");

RunCode.RunPythonScript(pyFilePath);
RunCode.RunJavaScript(javaFilePath);


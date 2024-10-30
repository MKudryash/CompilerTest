using CompilerTestC.CompileCode;
using CompilerTestC.WorkingWithFiles;
using System.Diagnostics;




string cFilePath = "test.c"; 
string cSharpFilePath = "test.cs"; 
string pyFilePath = "test.py"; 
string javaFilePath = "test.java"; 
string cOutputFileName = "finish"; 

await WriteCode.WriteCodeToFile("#include <stdio.h>\r\n\r\nint main(void)\r\n{\r\n\tprintf(\"Hello from C\");\r\n\treturn 0;\r\n}", cFilePath);
await WriteCode.WriteCodeToFile("print(\"Hello from Python!\")", pyFilePath);

CompileCCode.CompileC(cFilePath, cOutputFileName);
CompileCCode.CompileCSharp(cSharpFilePath);

RunCode.RunCScript(cOutputFileName);
RunCode.RunCSScript("test.exe");

RunCode.RunPythonScript(pyFilePath);
RunCode.RunJavaScript(javaFilePath);


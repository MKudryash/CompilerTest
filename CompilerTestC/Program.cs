using CompilerTestC.CompileCode;
using CompilerTestC.WorkingWithFiles;

//Наименование файлов
string cFilePath = "test.c"; 
string cSharpFilePath = "test.cs"; 
string pyFilePath = "test.py"; 
string javaFilePath = "test.java"; 
string cOutputFileName = "finish"; 


/*Запись кода в файлы*/
await WriteCode.WriteCodeToFile("#include <stdio.h>\r\n\r\nint main(void)\r\n{\r\n\tprintf(\"Hello from C\");\r\n\treturn 0;\r\n}", cFilePath);
await WriteCode.WriteCodeToFile("print(\"Hello from Python!\")", pyFilePath);

CompileCCode.CompileC(cFilePath, cOutputFileName); //Компиояция Си

RunCode.RunScript($"./{cOutputFileName}",null); //Зауск exe Си

RunCode.RunScript("dotnet-exec",cSharpFilePath); //Компиляция и запуск C#

RunCode.RunScript("python",pyFilePath); //Компиляция и запуск Pyhton

RunCode.RunScript("java",javaFilePath); //Компиляция и запуск Java


using CompilerTestC;
using CompilerTestC.CompileCode;
using CompilerTestC.GenerateCode;
using CompilerTestC.WorkingWithFiles;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;


//Наименование файлов
string cFilePath = "test.c";
string cSharpFilePath = "test.cs";
string pyFilePath = "test.py";
string javaFilePath = "test.java";
string cOutputFileName = "CodeC";


//Тестовые данные для Си
List<ArgumentsFunction> arguments = new List<ArgumentsFunction>() { new ArgumentsFunction("int", "a"), new ArgumentsFunction("int", "b") };
List<string> libraries = new List<string>() { "stdio.h" };
List<Tuple<string, string>> argumentsForFunction = new List<Tuple<string, string>>() { new Tuple<string, string>("1 2", "3"), new Tuple<string, string>("2 2", "4"), new Tuple<string, string>("5 2", "7") };

//Тестовые данные для C#
List<string> librariesCSharp = new List<string>() { "System" };

/*Запись кода в файлы*/
await WorkWithFile.WriteCodeToFile(GenerateCodeC.GenerateMain(arguments, libraries, "int Sum(int a, int b)\r\n{\r\n\treturn a + b;\r\n}\n", "Sum", "int"), cFilePath);
await WorkWithFile.WriteCodeToFile("print(\"Hello from Python!\")", pyFilePath);
await WorkWithFile.WriteCodeToFile(GenerateCodeCSharp.GenerateMain(arguments, librariesCSharp, "static int Sum(int a, int b)\r\n{ \r\nreturn a + b;\r\n}", "Sum", "int"), cSharpFilePath);
await WorkWithFile.WriteCodeToFile("public class test{ \r\n      \r\n    public static void main (String args[]){\r\n          \r\n        System.out.println(\"Hello from Java\");\r\n    }\r\n}", javaFilePath);



//Проверка компиляции кода 
RunCode.RunScript(RunCode.CreateRunScript("python", pyFilePath)); //Компиляция и запуск Python

RunCode.RunScript(RunCode.CreateRunScript("java", javaFilePath)); //Компиляция и запуск Java

//Проверка работоспособности тестов для Си
RunCode.RunScript(RunCode.CreateRunScript($"gcc", $"{cFilePath} -o {cOutputFileName}"));
Console.WriteLine(GenerateTest.CheckCodeUser(argumentsForFunction, $"./{cOutputFileName}"));

Console.WriteLine(GenerateTest.CheckCodeUserCSharp(argumentsForFunction, $"dotnet-exec", cSharpFilePath));



//Удаление файлов после компиляции из директории
WorkWithFile.DeleteFileFromDirectoryApp(cOutputFileName);
WorkWithFile.DeleteFileFromDirectoryApp(cFilePath);
WorkWithFile.DeleteFileFromDirectoryApp(cSharpFilePath);
WorkWithFile.DeleteFileFromDirectoryApp(pyFilePath);
WorkWithFile.DeleteFileFromDirectoryApp(javaFilePath);

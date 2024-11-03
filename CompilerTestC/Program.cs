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

internal class Program
{
    private static async Task Main(string[] args)
    {
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



        /*Запись кода в файлы*/
        await WorkWithCode.WriteCodeToFile(GenerateCodeC.GenerateMain(arguments, libraries, "int Sum(int a, int b)\r\n{\r\n\treturn a + b;\r\n}\n", "Sum", "int"), cFilePath);
        await WorkWithCode.WriteCodeToFile("print(\"Hello from Python!\")", pyFilePath);
        await WorkWithCode.WriteCodeToFile("using System;\r\n\r\nclass Program\r\n{\r\n    static void Main(string[] args)\r\n    {\r\n        Console.WriteLine(\"Hello CSharp\");  // Теперь компилятор найдет Console\r\n    }\r\n}\r\n", cSharpFilePath);
        await WorkWithCode.WriteCodeToFile("public class test{ \r\n      \r\n    public static void main (String args[]){\r\n          \r\n        System.out.println(\"Hello from Java\");\r\n    }\r\n}", javaFilePath);



        //Проверка компиляции кода 
        RunCode.RunScript(RunCode.CreateRunScript("python", pyFilePath)); //Компиляция и запуск Python

        RunCode.RunScript(RunCode.CreateRunScript("dotnet-exec", cSharpFilePath)); //Компиляция и запуск C#

        RunCode.RunScript(RunCode.CreateRunScript("java", javaFilePath)); //Компиляция и запуск Java*/

        //Проверка работоспособности тестов для Си
        RunCode.RunScript(RunCode.CreateRunScript($"gcc", $"{cFilePath} -o {cOutputFileName}"));
        Console.WriteLine(GenerateTest.CheckCodeUser(argumentsForFunction, $"./{cOutputFileName}"));
       


        //Удаление файлов после компиляции из директории
        WorkWithCode.DeleteFileFromDirectoryApp(cOutputFileName);
        WorkWithCode.DeleteFileFromDirectoryApp(cFilePath);
        WorkWithCode.DeleteFileFromDirectoryApp(cSharpFilePath);
        WorkWithCode.DeleteFileFromDirectoryApp(pyFilePath);
        WorkWithCode.DeleteFileFromDirectoryApp(javaFilePath);
    }
}
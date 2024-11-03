using CompilerTestC.CompileCode;
using CompilerTestC.WorkingWithFiles;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Text;

//Наименование файлов
string cFilePath = "test.c";
string cSharpFilePath = "test.cs";
string pyFilePath = "test.py";
string javaFilePath = "test.java";
string cOutputFileName = "CodeC";

List<ArgumentsFunction> arguments = new List<ArgumentsFunction>() {
new ArgumentsFunction("int","a"),
new ArgumentsFunction("int","b")
};

List<string> libraries = new List<string>() {
"stdio.h"
};

var theSpecifierType = new Dictionary<string, string>()
{
    {"char*","s" },
    {"int","d" },
    {"double","lf" },
    {"float","f" },
};

/*Запись кода в файлы*/
//await WriteCode.WriteCodeToFile("#include <stdio.h>\r\n\r\nint sum(int a, int b)\r\n{\r\n\r\n}\r\n\r\nint main(int argc,char* argv []) {\r\n    for (int i = 0; i < argc; i++)\r\n    {\r\n        printf(\"%s \\n\", argv[i]);\r\n    }\r\n    return 0;\r\n}", cFilePath);
await WriteCode.WriteCodeToFile(GenerateMain(arguments, libraries, "int Sum(int a, int b)\r\n{\r\n\treturn a + b;\r\n}\n", "testuser", "Sum", "int"), cFilePath);
await WriteCode.WriteCodeToFile("print(\"Hello from Python!\")", pyFilePath);
await WriteCode.WriteCodeToFile("using System;\r\n\r\nclass Program\r\n{\r\n    static void Main(string[] args)\r\n    {\r\n        Console.WriteLine(\"Hello CSharp\");  // Теперь компилятор найдет Console\r\n    }\r\n}\r\n", cSharpFilePath);
await WriteCode.WriteCodeToFile("public class test{ \r\n      \r\n    public static void main (String args[]){\r\n          \r\n        System.out.println(\"Hello from Java\");\r\n    }\r\n}", javaFilePath);

Console.WriteLine(GenerateMain(arguments, libraries, "int Sum(int a, int b)\r\n{\r\n\treturn a + b;\r\n}\n", "testuser", "Sum", "int"));

RunCode.RunScript($"gcc", $"{cFilePath} -o {cOutputFileName}"); //Компиляция Си



RunCode.RunScript($"gcc", $"{cFilePath} -o {cOutputFileName}"); //Компиляция Си

RunCode.RunScript($"./{cOutputFileName}", " -52 46 \"Hello from C generate\""); //Запуск exe Си
RunCode.RunScript($"./{cOutputFileName}", " -52 52 \"Hello from C generate\""); //Запуск exe Си

RunCode.RunScript("python", pyFilePath); //Компиляция и запуск Python

RunCode.RunScript("dotnet-exec", cSharpFilePath); //Компиляция и запуск C#

RunCode.RunScript("java", javaFilePath); //Компиляция и запуск Java

//Проверка работоспособности тестов
List<string> argumentsForFunction = new List<string>()
{
"1 2",
"1 4",
"-5 6",
"-5 6",
};
List<string> answers = new List<string>()
{
"3",
"5",
"5",
"1"
};
for (int i = 0; i < argumentsForFunction.Count; i++)
{
    if (!CheckCorrectAnswer(argumentsForFunction[i], answers[i], "testuser").Result)
    {
        Console.WriteLine("Тест не пройден");
        break;
    }
}


//Удаление файлов после компиляции из директории
DeleteFileFromDirectoryApp(cOutputFileName);
DeleteFileFromDirectoryApp(cFilePath);
DeleteFileFromDirectoryApp(cSharpFilePath);
DeleteFileFromDirectoryApp(pyFilePath);
DeleteFileFromDirectoryApp(javaFilePath);


void DeleteFileFromDirectoryApp(string outputFileName)
{
    FileInfo fileInfo = new FileInfo(outputFileName);
    if (fileInfo.Exists)
    {
        fileInfo.Delete();
    }

}



string GenerateMain(List<ArgumentsFunction> arguments, List<string> libraries, string codeFunction, string userName, string functionName, string typeFunction)
{
    string libs = "", declareArguments = "", specifier, specifierFunction, initArguments = "", argumentsForFunction = "";

    int i = 1;

    theSpecifierType.TryGetValue(typeFunction, out specifierFunction);

    foreach (string lib in libraries)
    {
        libs += $"#include <{lib}>\n";
    }
    foreach (var arg in arguments)
    {
        theSpecifierType.TryGetValue(arg.TypeVariable, out specifier);
        declareArguments += (arg.TypeVariable == "char*" ? $"\t{arg.TypeVariable} {arg.NameVariable};\n" : $"\t{arg.TypeVariable} {arg.NameVariable};\n");
        initArguments += arg.TypeVariable == "char*" ? $"\t{arg.NameVariable} = argv[{i}];" : $"\tsscanf(argv[{i}], \"%{specifier}\", &{arg.NameVariable});\n";
        argumentsForFunction += arg.NameVariable + (arguments.Count > i ? "," : "");
        i++;
    }
    string printVariable = $"\n\tFILE* myfile = fopen(\"{userName}.txt\", \"w\");" +
        $"\n\tfprintf(myfile, \"%{specifierFunction}\", {functionName}({argumentsForFunction}));" +
        $"\n\tfclose(myfile);\n";
    return $"{libs}\n {codeFunction} int main(int argc, char* argv[]) {{\n{declareArguments}{initArguments}{printVariable}\treturn 0;\r\n}}";
}


async Task<bool> CheckCorrectAnswer(string argumentsTest, string correctAnswer, string userName)
{
    RunCode.RunScript($"./{cOutputFileName}", $"{argumentsTest}"); //Запуск exe Си
    return await ReadFileAnswerTaskUser(userName) == correctAnswer;
}

async Task<string> ReadFileAnswerTaskUser(string userName)
{
    using (FileStream fstream = File.OpenRead($"{userName}.txt"))
    {
        // выделяем массив для считывания данных из файла
        byte[] buffer = new byte[fstream.Length];
        // считываем данные
        await fstream.ReadAsync(buffer, 0, buffer.Length);
        // декодируем байты в строку
        return Encoding.Default.GetString(buffer);
    }
}

/*string GenerateMain(List<string> argumentsTest, List<string> libraries, string codeFunction, string userName, string functionName, string typeFunction)
{
    string libs = "", specifierFunction, argumentsForFunction = "", recordFile = $"\tFILE* myfile = fopen(\"{userName}.txt\", \"w\");";
    int i = 1;

    theSpecifierType.TryGetValue(typeFunction, out specifierFunction);

    foreach (string lib in libraries)
    {
        libs += $"#include <{lib}>\n";
    }
    foreach (var arg in argumentsTest)
    {
        recordFile += $"\n\tfprintf(myfile, \"%{specifierFunction}\\n\", {functionName}({arg}));";
    }
    recordFile += $"\n\tfclose(myfile);";
    return $"{libs}\n {codeFunction} int main() {{\n{recordFile}\n\treturn 0;}}";
}

string ReadFileAnswerTaskUser(string userName)
{

}

bool ResultTestsTask(List<string> userAnswer, List<string> correctAnswer)
{


}*/

class ArgumentsFunction
{
    public ArgumentsFunction(string typeVariable, string nameVariavle)
    {
        TypeVariable = typeVariable;
        NameVariable = nameVariavle;
    }

    public string TypeVariable { get; set; }
    public string NameVariable { get; set; }
}
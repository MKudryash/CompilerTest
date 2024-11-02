using CompilerTestC.CompileCode;
using CompilerTestC.WorkingWithFiles;
using System.IO;
using System.Reflection;

//Наименование файлов
string cFilePath = "test.c";
string cSharpFilePath = "test.cs";
string pyFilePath = "test.py";
string javaFilePath = "test.java";
string cOutputFileName = "CodeC";

List<ArgumentsFunction> arguments = new List<ArgumentsFunction>() {
new ArgumentsFunction("int","a"),
new ArgumentsFunction("double","b"),
new ArgumentsFunction("char*","c"),
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
await WriteCode.WriteCodeToFile(GenerateMain(arguments, "#include <stdio.h>"), cFilePath);
await WriteCode.WriteCodeToFile("print(\"Hello from Python!\")", pyFilePath);
await WriteCode.WriteCodeToFile("using System;\r\n\r\nclass Program\r\n{\r\n    static void Main(string[] args)\r\n    {\r\n        Console.WriteLine(\"Hello CSharp\");  // Теперь компилятор найдет Console\r\n    }\r\n}\r\n", cSharpFilePath);
await WriteCode.WriteCodeToFile("public class test{ \r\n      \r\n    public static void main (String args[]){\r\n          \r\n        System.out.println(\"Hello from Java\");\r\n    }\r\n}", javaFilePath);
Console.WriteLine(GenerateMain(arguments, "#include <stdio.h>"));

RunCode.RunScript($"gcc", $"{cFilePath} -o {cOutputFileName}"); //Компиляция Си



RunCode.RunScript($"gcc", $"{cFilePath} -o {cOutputFileName}"); //Компиляция Си

RunCode.RunScript($"./{cOutputFileName}", " 1 2.2 \"Hello from C generate\""); //Запуск exe Си

RunCode.RunScript("python", pyFilePath); //Компиляция и запуск Pyhton

RunCode.RunScript("dotnet-exec", cSharpFilePath); //Компиляция и запуск C#

RunCode.RunScript("java", javaFilePath); //Компиляция и запуск Java

FileInfo fileInfo = new FileInfo(cFilePath);
if (fileInfo.Exists)
{
    fileInfo.Delete();
}
fileInfo = new FileInfo(pyFilePath);
if (fileInfo.Exists)
{
    fileInfo.Delete();
}
fileInfo = new FileInfo(cSharpFilePath);
if (fileInfo.Exists)
{
    fileInfo.Delete();
}
fileInfo = new FileInfo(javaFilePath);
if (fileInfo.Exists)
{
    fileInfo.Delete();
}
fileInfo = new FileInfo(cOutputFileName);
if (fileInfo.Exists)
{
    fileInfo.Delete();
}


string GenerateMain(List<ArgumentsFunction> arguments, string libraries)
{
    string declareArguments = "", specifier, initArguments = "";
    int i = 1;
    foreach (var arg in arguments)
    {
        theSpecifierType.TryGetValue(arg.TypeVariable, out specifier);
        declareArguments += arg.TypeVariable == "char*" ? $"\t{arg.TypeVariable} {arg.NameVariavle};\n" : $"\t{arg.TypeVariable} {arg.NameVariavle};\n";
        string argumentAmpersand = arg.TypeVariable == "char" ? $"{arg.NameVariavle});\n" : $"&{arg.NameVariavle});\n";
        initArguments += arg.TypeVariable == "char*" ? $"\t{arg.NameVariavle} = argv[{i}];" :  $"\tsscanf(argv[{i}], \"%{specifier}\", {argumentAmpersand}";
        i++;
    }
    string printVariable = "\n\tprintf(\"%d\\n\", a);\r\n\tprintf(\"%lf\\n\", b);\n\tprintf(\"%s\\n\", c);\n";
    return $"{libraries}\nint main(int argc, char* argv[]) {{\n{declareArguments}{initArguments}{printVariable}\treturn 0;\r\n}}";
}

class ArgumentsFunction
{
    public ArgumentsFunction(string typeVariable, string nameVariavle)
    {
        TypeVariable = typeVariable;
        NameVariavle = nameVariavle;
    }

    public string TypeVariable { get; set; }
    public string NameVariavle { get; set; }
}

namespace CompilerTestC.GenerateCode
{
    public static class GenerateCodeC
    {

        // Словарь спецификаторов в Си (пока лежит так, может придумаю еще что-нибудь)
        private static Dictionary<string, string> theSpecifierType = new Dictionary<string, string>() { { "char*", "s" }, { "int", "d" }, { "double", "lf" }, { "float", "f" }, };

        /// <summary>
        /// Генерация кода языка Си с функцией написанной пользователем и запуском его в main. В main считываются аргументы командной строки и выводится ответ на консоль запущенного метода
        /// </summary>
        /// <param name="arguments">Аргументы командной строки</param>
        /// <param name="libraries">Библиотеки для данного кода</param>
        /// <param name="codeFunction">Функция написанная пользователем</param>
        /// <param name="functionName">Имя запускаемой функции</param>
        /// <param name="typeFunction">Тип возвращаемой функции</param>
        /// <returns>Возвращаем полностью сгенерированный код</returns>
        public static string GenerateMain(List<ArgumentsFunction> arguments, List<string> libraries, string codeFunction,  string functionName, string typeFunction)
        {
            string libs = "", declareArguments = "", specifier, specifierFunction, initArguments = "", argumentsForFunction = "";

            int i = 1; // Аргументы считываются почему-то со второго

            theSpecifierType.TryGetValue(typeFunction, out specifierFunction); // Получение из словаря спецификатор по типу функции

            // Генерация строк подключаемых библиотек 
            foreach (string lib in libraries)
            {
                libs += $"#include <{lib}>\n";
            }
            // Объявление и инициализация переменных из аргументов командной строки
            foreach (var arg in arguments)
            {
                theSpecifierType.TryGetValue(arg.TypeVariable, out specifier); // Получение спецификатора из словаря для переменной
                declareArguments += (arg.TypeVariable == "char*" ? $"\t{arg.TypeVariable} {arg.NameVariable};\n" : $"\t{arg.TypeVariable} {arg.NameVariable};\n"); // Объявление переменной
                initArguments += arg.TypeVariable == "char*" ? $"\t{arg.NameVariable} = argv[{i}];" : $"\tsscanf(argv[{i}], \"%{specifier}\", &{arg.NameVariable});\n"; // Инициализация переменной из получаемого аргумента
                argumentsForFunction += arg.NameVariable + (arguments.Count > i ? "," : "");// Генерация переменных для запуска функции (к примеру: arg1, arg2)
                i++;
            }
            string printVariable = $"\n\tprintf(\"%{specifierFunction}\", {functionName}({argumentsForFunction}));";// Запускаем функции и выводим результат на экран

            // Собираем весь код в едино с добавлением все в main
            return $"{libs}\n {codeFunction} int main(int argc, char* argv[]) {{\n{declareArguments}{initArguments}{printVariable}\n\treturn 0;\r\n}}";
        }
    }
}

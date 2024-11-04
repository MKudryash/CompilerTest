using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerTestC.GenerateCode
{
    public class GenerateCodeCSharp
    {
        // Словарь спецификаторов в Си (пока лежит так, может придумаю еще что-нибудь)
        private static Dictionary<string, string> theConvertType = new Dictionary<string, string>() { { "string", "ToString" }, { "int", "ToInt32" }, { "double", "ToDouble" }, { "bool", "ToBoolean" }, };

        /// <summary>
        /// Генерация кода языка Си с функцией написанной пользователем и запуском его в main. В main считываются аргументы командной строки и выводится ответ на консоль запущенного метода
        /// </summary>
        /// <param name="arguments">Аргументы командной строки</param>
        /// <param name="libraries">Библиотеки для данного кода</param>
        /// <param name="codeFunction">Функция написанная пользователем</param>
        /// <param name="functionName">Имя запускаемой функции</param>
        /// <param name="typeFunction">Тип возвращаемой функции</param>
        /// <returns>Возвращаем полностью сгенерированный код</returns>
        public static string GenerateMain(List<ArgumentsFunction> arguments, List<string> libraries, string codeFunction, string functionName, string typeFunction)
        {
            string libs = "", declareArguments = "", convertType, initArguments = "", argumentsForFunction = "";

            int i = 0; // Счетчик аргументов


            // Генерация строк подключаемых библиотек 
            foreach (string lib in libraries)
            {
                libs += $"using {lib};\n";
            }
            // Объявление и инициализация переменных из аргументов командной строки
            foreach (var arg in arguments)
            {
                theConvertType.TryGetValue(arg.TypeVariable, out convertType); // Получение спецификатора из словаря для переменной
                declareArguments += $"{arg.TypeVariable} {arg.NameVariable} = Convert.{ convertType} (args[{ i}]);\n" ; ; // Объявление переменной и инициализация переменной из получаемого аргумента
                i++;
                argumentsForFunction += arg.NameVariable + (arguments.Count - 1 >= i ? "," : "");// Генерация переменных для запуска функции (к примеру: arg1, arg2)
            }
            string printAnswer = $"Console.Write(\"{{0}}\", {functionName}({argumentsForFunction}));";// Запускаем функции и выводим результат на экран
            // Собираем весь код в едино с добавлением все в main
            return $"{libs}\n public class Programs\r\n{{{codeFunction}\nprivate static void Main(string[] args)\r\n    {{ {declareArguments} {initArguments} {printAnswer}  }}\r\n}}";
        }
    }
}

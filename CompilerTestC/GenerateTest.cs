using CompilerTestC.CompileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerTestC
{
    // Возможно пока тестовый класс для проверки прохождения тестов для разных языков
    public static class GenerateTest
    {
        /// <summary>
        /// Проверка ответа пользователя и сгенерированных для тестов
        /// </summary>
        /// <param name="argumentsForFunction">Аргументы для проверки и ответы к ним</param>
        /// <param name="fileName">Наименование команды или пакета (dotnet-exec, gcc, ./ name.exe, python, java)</param>
        /// <returns></returns>
        public static string CheckCodeUser(List<Tuple<string, string>> argumentsForFunction, string fileName)
        {
            // Перебор всех тестов
            for (int i = 0; i < argumentsForFunction.Count; i++)
            {
                // Запуск компиляции с передачей аргументов командную строку
                var run = RunCode.RunScript(RunCode.CreateRunScript(fileName, argumentsForFunction[i].Item1));

                // Если хоть один ответ не сходится возвращаем, что тесты не пройдены
                if (argumentsForFunction[i].Item2 != run.answerCode) return ($"Expected {argumentsForFunction[i].Item2} result {run.answerCode}"); ;
            }
            return "All tests have been successfully passed";// Если все тесты пройдут возвращаем, что все хорошо
        }
    }
}

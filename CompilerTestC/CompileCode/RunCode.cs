using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerTestC.CompileCode
{
    public static class RunCode
    {
        /// <summary>
        /// Создает один из возможных скриптов или exe файлов (C#, C, C++, Python, Java)
        /// </summary>
        /// <param name="fileName">Наименование команды или пакета (dotnet-exec, gcc, ./ name.exe, python, java)</param>
        /// <param name="arguments">Наименование запускаемого файла/команда (name.cs,name.c -o finishName, name.py, name.java)</param>
        public static ProcessStartInfo CreateRunScript(string fileName, string? arguments)
        {
            // Создание процесса с настройкой определенных параметров
            return new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
        }

        /// <summary>
        /// Запускает один из ранее созданных скриптов или exe файлов (C#, C, C++, Python, Java)
        /// </summary>
        /// <param name="startInfo">Созданный процесс скрипта</param>
        /// <returns>Возвращает напечатанный результат кода (ошибка или результат функции) и его статус</returns>
        public static (string answerCode, bool status) RunScript(ProcessStartInfo startInfo)
        {
            using (Process process = new Process { StartInfo = startInfo })
            {
                // Запуск созданного процесса
                process.Start();

                // Чтение вывода
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                // Ожидание конца завершение процесса (возможно стоит указать время?)
                process.WaitForExit();

                //Возврат в зависимости от результата компиляции
                if (!string.IsNullOrEmpty(error)) return (error, false);
                else return (output, true);
            }
        }
    }
}

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
        /// Запускает один из возможных скриптов или exe файлов (C#, C, C++, Python, Java)
        /// </summary>
        /// <param name="fileName">Наименование команды или пакета (dotnet-exec, gcc, ./ name.exe, python, java)</param>
        /// <param name="arguments">Наименование запускаемого файла (name.cs,name.c -o finishName, name.py, name.java)</param>
        public static void RunScript(string fileName, string? arguments)
        {
            // Создание процесса с настройкой определенных параметров
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = fileName, 
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = startInfo })
            {
                // Запуск созданного процесса
                process.Start();

                // Чтение вывода
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                // Выводим результат выполнения
                Console.WriteLine(output);
                if (!string.IsNullOrEmpty(error))
                {
                    Console.WriteLine("Error:");
                    Console.WriteLine(error);
                }
            }
        }
    }
}

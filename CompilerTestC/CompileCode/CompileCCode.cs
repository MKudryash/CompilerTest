using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerTestC.CompileCode
{
    public static class CompileCCode
    {
        public static void CompileC(string cFilePath, string outputFileName)
        {

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "gcc",
                Arguments = $"{cFilePath} -o {outputFileName}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = startInfo })
            {
                process.Start();

                // Чтение вывода и ошибок
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                // Выводим результат компиляции
                if (process.ExitCode == 0)
                {
                    Console.WriteLine("Compilation successful.");
                }
                else
                {
                    Console.WriteLine("Compilation failed:");
                    Console.WriteLine(error);
                }
            }
        }

        public static void CompileCSharp(string cFilePath)
        {

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "csc.exe",
                Arguments = $"-target:winexe {cFilePath}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = startInfo })
            {
                process.Start();

                // Чтение вывода и ошибок
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                // Выводим результат компиляции
                if (process.ExitCode == 0)
                {
                    Console.WriteLine("Compilation successful.");
                }
                else
                {
                    Console.WriteLine("Compilation failed:");
                    Console.WriteLine(error);
                }
            }
        }
    }
}

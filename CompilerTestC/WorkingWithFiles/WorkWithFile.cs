using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CompilerTestC.WorkingWithFiles
{
    //Класс для работы с внешними файлами
    public static class WorkWithFile
    {

        /// <summary>
        /// Запись генерируемого кода в файл для дальнейшей его компиляции
        /// </summary>
        /// <param name="code">Код на одном из выбранных ранее языке (C, C#, Python, Java)</param>
        /// <param name="path">Путь для создаваемого файла</param>
        /// <returns></returns>
        public static async Task WriteCodeToFile(string code, string path)
        {
            try
            {
                // Создаем и записываем в файл код
                using (StreamWriter writer = new StreamWriter(path, false))
                {
                    await writer.WriteLineAsync(code);
                    Console.WriteLine("Текст записан в файл");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// Удаление файлов после компиляции
        /// </summary>
        /// <param name="outputFileName">Путь для удаляемого файла</param>
        public static void DeleteFileFromDirectoryApp(string outputFileName)
        {
            try
            {
                // Создаем файл используя путь к нему
                FileInfo fileInfo = new FileInfo(outputFileName);
                // Если файл существует удаляем
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                    Console.WriteLine("Файла успешно удален");

                }
                else Console.WriteLine("Файла не существует");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

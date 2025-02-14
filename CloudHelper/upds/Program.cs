using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace upds
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string folderPath = "C:\\CloudHelper";
            string exeFilePath = Path.Combine(folderPath, "CloudHelper_protected.exe");
            string dllFilePath = Path.Combine(folderPath, "Guna.UI.dll");

            // Проверяем, существует ли папка, и создаем её, если нет
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Если файлы уже существуют, запускаем exe и завершаем программу
            if (File.Exists(exeFilePath) && File.Exists(dllFilePath))
            {
                System.Diagnostics.Process.Start(exeFilePath);
                return;
            }

            // Скачиваем файлы, если они отсутствуют
            await DownloadFileAsync("https://github.com/DaniilWellnes/CloudHelper/raw/refs/heads/main/CloudHelper_protected.exe", exeFilePath);
            await DownloadFileAsync("https://github.com/DaniilWellnes/CloudHelper/raw/refs/heads/main/Guna.UI.dll", dllFilePath);

            // Запускаем скачанный exe файл
            System.Diagnostics.Process.Start(exeFilePath);
        }

        static async Task DownloadFileAsync(string url, string destinationFilePath)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode(); // Проверяем успешность запроса

                // Сохраняем файл
                using (FileStream fs = new FileStream(destinationFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await response.Content.CopyToAsync(fs);
                }
            }
        }
    }
}

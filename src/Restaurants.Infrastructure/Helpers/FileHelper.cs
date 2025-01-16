using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Infrastructure.Helpers
{
    public static class FileHelper
    {
        public static void UnZipToDir(string zipPath, string extractedPath)
        {
            try
            {
                System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, extractedPath);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string GetFileName(string fileName)
        {
            int index = fileName.LastIndexOf('.');
            return fileName.Substring(0, index);
        }

        public static string GetMimeType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out var mimeType))
            {
                mimeType = "application/octet-stream";
            }
            return mimeType;
        }

        public static long GetFolderSize(string folderPath)
        {
            long size = 0;

            try
            {
                string[] files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);

                foreach (string filePath in files)
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    size += fileInfo.Length;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return size;
        }

        public static double BytesToKilobytes(long bytes)
        {
            return Math.Round(bytes / 1024.0, 2);
        }

        public static double BytesToMegabytes(long bytes)
        {
            var tmp = bytes / 1024.0 / 1024.0;
            return Math.Round(tmp, 10);
        }

        public static double BytesToGigabytes(long bytes)
        {
            return Math.Round(bytes / 1024.0 / 1024.0 / 1024.0, 2);
        }

        public static string FormatSize(double sizeInBytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double size = sizeInBytes;

            int index = 0;
            while (size >= 1024 && index < sizes.Length - 1)
            {
                size /= 1024;
                index++;
            }
            return $"{size:0.##} {sizes[index]}";
        }

        public static bool CombineMultipleFilesIntoSingleFile(string inputDirectoryPath, string outputFilePath)
        {
            try
            {
                if (Directory.Exists(inputDirectoryPath))
                {
                    string[] inputFilePaths = Directory.GetFiles(inputDirectoryPath).OrderBy(f => int.Parse(Path.GetFileNameWithoutExtension(f).Split('_')[0])).ToArray();

                    Console.WriteLine("Number of files: {0}.", inputFilePaths.Length);

                    if (!File.Exists(outputFilePath))
                    {
                        using (var outputStream = File.Create(outputFilePath))
                        {
                            foreach (var inputFilePath in inputFilePaths)
                            {
                                Console.WriteLine($"file ----{Path.GetFileName(inputFilePath)}");
                                using (var inputStream = File.OpenRead(inputFilePath))
                                {
                                    // Buffer size can be passed as the second argument.
                                    inputStream.CopyTo(outputStream);
                                }

                                File.Delete(inputFilePath);
                                Console.WriteLine("The file {0} has been processed.", inputFilePath);
                            }
                            Directory.Delete(inputDirectoryPath);

                            return true;
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Combine files error:{e.Message}");
                return false;
            }
        }

        public static async Task AreAllFilesReleased(string folderPath)
        {
            // Check if all files are ready
            foreach (var filePath in Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories))
            {
                while (!IsFileReady(filePath))
                {
                    await Task.Delay(500);
                }
            }
        }

        public static bool IsFileReady(string filename)
        {
            try
            {
                using (FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                    return inputStream.Length > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsFile(string path)
        {
            try
            {
                string extension = Path.GetExtension(path);
                return !string.IsNullOrEmpty(extension);
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}

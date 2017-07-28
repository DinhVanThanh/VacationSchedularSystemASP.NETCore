using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace App.common.core.Helpers
{
    public class Uploader
    {

        public static string GetFileName(IFormFile file)
        {
            var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            return filename;
        }

        public static void Upload(IFormFile file, string directory, string fileName)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string destination = Path.Combine(directory, fileName);
            using (FileStream fs = System.IO.File.Create(destination))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
        }

        public static void Upload(byte[] fileBytes, string directory, string fileName)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string destination = Path.Combine(directory, fileName);
            File.WriteAllBytes(destination, fileBytes);
        }

        public async static Task<bool> DownloadAsyn(string fileUrl, string directory, string fileName)
        {
            if (string.IsNullOrEmpty(fileUrl))
                return false;
            string destination = Path.Combine(directory, fileName);
            using (var httpClient = new HttpClient())
            {
                using (var contentStream = await httpClient.GetStreamAsync(fileUrl))
                {
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    using (var fileStream = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.None, 1048576, true))
                    {
                        await contentStream.CopyToAsync(fileStream);
                        return true;
                    }
                }
            }
        }

        public static string MoveToPrimaryFolder(string fileName, string primaryFolder, string sourceUrl, string destinationUrl)
        {
            var tempFileUrl = Path.Combine(sourceUrl, fileName);
            var desFileUrl = Path.Combine(destinationUrl, fileName);
            if (!Directory.Exists(destinationUrl))
            {
                Directory.CreateDirectory(destinationUrl);
            }
            if (!File.Exists(tempFileUrl))
                throw new Exception("CANNOT_UPLOAD_IMAGE");
            if (File.Exists(desFileUrl))
                File.Delete(desFileUrl);
            File.Move(tempFileUrl, desFileUrl);
            return $"/{primaryFolder}/{fileName}";
        }

        public static void RemoveFile(string fileName, string destinationUrl)
        {
            var desFileUrl = Path.Combine(destinationUrl, fileName);
            if (File.Exists(desFileUrl))
                File.Delete(desFileUrl);
        }
    }
}

using Microsoft.AspNetCore.Http;

namespace Pustok.Business.Extensions
{
    public static class Helper
    {
        public static string SaveFile(string rootPath, string folderName, IFormFile file)
        {
            string fileName = file.FileName.Length > 64 ? file.FileName.Substring(file.FileName.Length -64,64) : file.FileName;

            fileName = Guid.NewGuid().ToString() + fileName;

            string path = Path.Combine(rootPath, folderName, fileName);

            using(FileStream stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return fileName;
        }
    }
}

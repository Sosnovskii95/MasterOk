namespace MasterOk.Models.FilesModify
{
    public static class ChangeFiles
    {
        //метод производит сохранение файлов отправленных при добавлении товара/категории/подкатегории и т.д.
        //возвращает словарь название файла и его тип - для последующей загрузки.
        //в качестве ключа используется название файла поскольку реализацией любой ос запрещено два одинаковых название файла. Нет колезий.
        public static async Task<Dictionary<string, string>> SaveCreateUploadFiles(int idFolderFiles, string nameFolderFiles, IFormFileCollection listUploadsFiles)
        {
            string pathCreateFiles = nameFolderFiles + idFolderFiles;
            Dictionary<string, string> nameFiles = new Dictionary<string, string>();
            if (!Directory.Exists(pathCreateFiles))
            {
                Directory.CreateDirectory(pathCreateFiles);
            }

            foreach (var file in listUploadsFiles)
            {
                string fileName = pathCreateFiles + "/" + file.FileName;
                using (var fileStream = new FileStream(fileName, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                nameFiles.Add(file.FileName, file.ContentType);
            }

            return nameFiles;
        }

        public static void DeleteFiles(int idFolderFiles, string nameFolderFiles, string deleteNameFile)
        {
            string pathFiles = nameFolderFiles + "/" + idFolderFiles;
            File.Delete(pathFiles + "/" + deleteNameFile);
        }
    }
}

namespace MasterOk.Models.CreateFiles
{
    public static class SaveRenameUploadFiles
    {
        public static async Task<List<string>> SaveCreateFiles(int idFolderFiles, string nameFolderFiles, IFormFileCollection listUploadsFiles)
        {
            string pathCreateFiles = nameFolderFiles + "/" + idFolderFiles;
            if (!Directory.Exists(pathCreateFiles))
            {
                Directory.CreateDirectory(pathCreateFiles);
            }

            foreach (var file in listUploadsFiles)
            {
                string fileName = pathCreateFiles + "/" + file.FileName;
                using (var fileStream = new FileStream(fileName, FileMode.Create))
                {
                    await fileStream.CopyToAsync(fileStream);
                }
            }

            IEnumerable<FileInfo> nameFilesDirectory = Directory.GetFiles(pathCreateFiles + "/").Select(f => new FileInfo(f));

            int count = 1;
            List<string> nameFiles = new List<string>();

            foreach (var file in nameFilesDirectory)
            {
                string newFileName = $@"{idFolderFiles + "_" + count++}{file.Extension}";
                string newPathFile = Path.Combine(file.DirectoryName, newFileName);
                File.Move(file.FullName, newPathFile);
                nameFiles.Add(newFileName);
            }

            return nameFiles;
        }
    }
}

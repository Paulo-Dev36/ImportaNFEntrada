using System.IO.Compression;

namespace ImportaNFEntrada.Utils
{
    public class ZipUtils
    {
        public ArquivosUtils arquivosUtils = new();
        public  bool DescompactarArquivosEmpresa(string path)
        {
            try
            {
                List<FileInfo> arquivosCompactados = arquivosUtils.GetListaZipFiles(path);

                foreach (FileInfo file in arquivosCompactados)
                {
                    Unzip(file, path);
                    MoverArquivos(file, new DirectoryInfo(Path.Combine(path, "ZIP")));
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return false;
        }

        public bool MoverArquivos(FileInfo origem, DirectoryInfo destino)
        {
            try
            {
                FileInfo destinoArquivo = new FileInfo(Path.Combine(destino.FullName, origem.Name));
                File.Copy(origem.FullName, destinoArquivo.FullName);
                origem.Delete();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        public static void Unzip(FileInfo zipFile, string destinationFolderPath)
        {
            try
            {
                using (ZipArchive archive = ZipFile.OpenRead(zipFile.FullName))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        string entryDestinationPath = Path.Combine(destinationFolderPath, entry.FullName);
                        entry.ExtractToFile(entryDestinationPath, overwrite: true);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}

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
                if (!Directory.Exists(path + "\\XML_IMPORTAR"))
                {
                    Directory.CreateDirectory(path + "\\XML_IMPORTAR");
                }
                    List<FileInfo> arquivosCompactados = arquivosUtils.GetListaZipFiles(path + "\\XML_IMPORTAR");

                foreach (FileInfo file in arquivosCompactados)
                {
                    Unzip(file, path + "\\XML_IMPORTAR");
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
                if (!destino.Exists)
                {
                    Directory.CreateDirectory(destino.ToString());
                }
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

        public bool MoverXml(FileInfo arquivo, string caminhoDestino)
        {
            try
            {

                if (!Directory.Exists(caminhoDestino))
                {
                    Directory.CreateDirectory(caminhoDestino);
                }
                // Move o arquivo
                File.Move(arquivo.FullName, caminhoDestino+'\\'+arquivo.Name);

                //Console.WriteLine("Arquivo movido com sucesso.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro ao mover o arquivo: " + ex.Message);
            }
            return false;
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

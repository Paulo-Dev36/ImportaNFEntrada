namespace ImportaNFEntrada.Utils
{
    public class ArquivosUtils
    {
        protected string pathNotas = "\\\\srv-arquivos\\ARQUIVOS";

        public List<FileInfo> GetListaZipFiles(string path)
        {
            string[] extensions = { "zip", "ZIP" };
            DirectoryInfo directory = new DirectoryInfo(path);
            List<FileInfo> listaZipFiles = directory.GetFiles().Where(x => x.Extension.Contains("zip")).ToList();
            listaZipFiles.AddRange(directory.GetFiles().Where(x => x.Extension.Contains("ZIP")).ToList());
            return listaZipFiles;
        }

        public byte[] FileToByteArray(FileInfo file)
        {
            byte[] bytes = null;
            using (FileStream stream = file.OpenRead())
            {
                int length = (int)stream.Length;
                bytes = new byte[length];
                int bytesRead = 0;
                int offset = 0;

                while (offset < length && (bytesRead = stream.Read(bytes, offset, length - offset)) > 0)
                {
                    offset += bytesRead;
                }
            }
            return bytes;
        }
    }
}

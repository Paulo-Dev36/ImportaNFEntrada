using HtmlAgilityPack;
using ImportaNFEntrada.Classes;
using ImportaNFEntrada.Utils;
using System.Text;
using System.Text.RegularExpressions;

namespace ImportaNFEntrada
{
    public class ImportaNFEntradaNeg
    {
        protected string[] extensions = new string[] { "xml", "XML" };

        string padraoChaveNfe = @"<chNFe>(.*?)<\/chNFe>";
        string padraoNumeroNfe = @"<nNF>(.*?)<\/nNF>";
        string padraoSerieNfe = @"<serie>(.*?)<\/serie>";
        string padraoModeloNfe = @"<mod>(.*?)<\/mod>";
        string padraoPeriodoNfe = @"<dhEmi>(.*?)<\/dhEmi>";
        string padraoValorTotalNfe = @"<vNF>(.*?)<\/vNF>";
        string padraoCnpjNfe = @"<emit><CNPJ>(.*?)<\/CNPJ>";
        string padraoCnpjDestNfe = @"<dest><CNPJ>(.*?)<\/CNPJ>";
        string padraoCpfNfe = @"<emit><CPF>(.*?)<\/CPF>";

        public Dictionary<string, NFEntrada> CarregarNotas(string path, int codigoempresa, int codigoestab)
        {
            Dictionary<string, NFEntrada> lista = new Dictionary<string, NFEntrada>();
            List<FileInfo> listaXML = new();
            ArquivosUtils arquivosUtils = new();
            try
            {
                DirectoryInfo directory = new DirectoryInfo(path);
                listaXML = directory.GetFiles().Where(x => x.Extension.Contains("xml")).ToList();
                listaXML.AddRange(directory.GetFiles().Where(x => x.Extension.Contains("XML")).ToList());
                Console.WriteLine($"QUANTIDADE DE NOTAS A CARREGAR: {listaXML.Count()}");

                foreach (FileInfo xml in listaXML)
                {
                    try
                    {
                        byte[] encoded = File.ReadAllBytes(xml.FullName);
                        string x = Encoding.UTF8.GetString(encoded);
                        x = Regex.Replace(x, "<Signature(.*?)</Signature>", "");
                        x = Regex.Replace(x, "<indIntermed(.*?)</indIntermed>", "");
                        x = Regex.Replace(x, "<infIntermed(.*?)</infIntermed>", "");

                        //NFNotaProcessada notaProcessada = new DFPersister().read(
                        //  NFNotaProcessada.class, x);

                        NFEntrada nFEntrada = new NFEntrada();

                        nFEntrada.ChaveNf = Regex.Match(x, padraoChaveNfe).Groups[1].Value;
                        nFEntrada.CodigoEmpresa = codigoempresa;
                        nFEntrada.CodigoEstab = codigoestab;
                        nFEntrada.Numeronf = Regex.Match(x, padraoNumeroNfe).Groups[1].Value;
                        nFEntrada.SerieNf = Regex.Match(x, padraoSerieNfe).Groups[1].Value;
                        nFEntrada.EspecielNf = Regex.Match(x, padraoModeloNfe).Groups[1].Value.Equals("55") ? "NFE" : "NFCE";
                        string periodo = Regex.Match(x, padraoPeriodoNfe).Groups[1].Value.Substring(0, 10);
                        nFEntrada.Periodo = Convert.ToDateTime(periodo);
                        nFEntrada.Xml = arquivosUtils.FileToByteArray(xml);
                        nFEntrada.ValorNf = Convert.ToDecimal((Regex.Match(x, padraoValorTotalNfe).Groups[1]).Value.Replace('.', ','));
                        nFEntrada.InscricaoFederal = !string.IsNullOrEmpty(Regex.Match(x, padraoCnpjNfe).Groups[1].Value) ?
                                                                Regex.Match(x, padraoCnpjNfe).Groups[1].Value :
                                                                Regex.Match(x, padraoCpfNfe).Groups[1].Value;
                        nFEntrada.InscricaoFederalDestinatario = Regex.Match(x, padraoCnpjDestNfe).Groups[1].Value;
                        lista.Add(nFEntrada.ChaveNf, nFEntrada);

                        Console.WriteLine($"NOTA PROCESSADA: {nFEntrada.ChaveNf}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro xml: {xml.FullName} - {ex.Message}");
                        Console.WriteLine("VERIFICANDO SE A NOTA ESTÁ CANCELADA...");

                        NFEntrada nFEntrada = GetNotaCancelada(xml, codigoempresa, codigoestab);
                        if(nFEntrada is not null)
                        {
                            Console.WriteLine($"NOTA CANCELADA: {nFEntrada.ChaveNf}");
                            lista.Add(nFEntrada.ChaveNf, nFEntrada);
                        }

                        Console.WriteLine("------------------------");
                    }
                }

                return lista;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"ERRO AO PROCESSAR NOTAS: {ex.Message}");
                return null;
            }
            finally
            {

            }
        }

        public NFEntrada GetNotaCancelada(FileInfo arquivoXml, int codigoEmpresa, int codigoEstab)
        {
            byte[] encoded = File.ReadAllBytes(arquivoXml.FullName);
            string xmlString = Encoding.UTF8.GetString(encoded);
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(xmlString);

            string evento = htmlDoc.DocumentNode.SelectSingleNode("//descEvento")?.InnerText;
            if (evento != null)
            {
                if (evento.Equals("Cancelamento") || evento.Equals("Cancelamento por substituicao"))
                {
                    NFEntrada nfentrada = new NFEntrada();
                    string? chavenfe = htmlDoc.DocumentNode.SelectSingleNode("//chNFe")?.InnerText;
                    string mesanonf = chavenfe.Substring(2, 6);
                    string numeronf = chavenfe.Substring(26, 34);
                    string serienf = chavenfe.Substring(22, 25);
                    string modelonf = chavenfe.Substring(20, 22);

                    // Numero NF
                    nfentrada.Numeronf = numeronf;

                    // Serie NF
                    nfentrada.SerieNf = serienf;

                    // Chave da nota
                    nfentrada.ChaveNf = chavenfe;

                    // Código modelo
                    nfentrada.EspecielNf = modelonf.Equals("55") ? "NFE" : "NFCE";

                    nfentrada.Periodo = DateTime.ParseExact("01/" + mesanonf.Substring(2, 4) + "/" + mesanonf.Substring(0, 2), "dd/MM/yy", null);

                    nfentrada.NotaCancelada = true;
                    nfentrada.CodigoEmpresa = codigoEmpresa;
                    nfentrada.CodigoEstab = codigoEstab;
                    nfentrada.Xml = File.ReadAllBytes(arquivoXml.FullName);

                    return nfentrada;
                }
            }
            return null;
        }
    }
}

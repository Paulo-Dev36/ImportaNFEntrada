using ImportaNFEntrada.Classes;
using ImportaNFEntrada.Repositorio;
using ImportaNFEntrada.Utils;

namespace ImportaNFEntrada
{
    public class PrincipalNeg
    {
        protected string pathEmpresa = "";
        protected string local = "S:\\";
        protected Agendamento agendamento;
        protected string data;
        protected ZipUtils zipUtilz = new();
        protected ImportaNFEntradaNeg importaNFEntradaNeg = new();
        protected NfEntradaNeg nfEntradaNeg = new();
        protected EstabNeg estabNeg = new();

        public bool ProcessarNotasEmpresas(Agendamento agendamento, string data)
        {
            try
            {
                data = data.Substring(5, 2) + "-" + data.Substring(0, 4);

                Console.WriteLine($"\nINICIANDO PROCESSAMENTO NOTAS EMPRESA {agendamento.CodigoEmpresa} - {agendamento.CodigoEstab}");

                Console.WriteLine("DESCOMPACTANDO AS NOTAS...");

                zipUtilz.DescompactarArquivosEmpresa(GetPathEmpresa(agendamento, data));

                Dictionary<string, NFEntrada> listaNotasXML = importaNFEntradaNeg
                                    .CarregarNotas(GetPathEmpresa(agendamento, data),
                                        agendamento.CodigoEmpresa, agendamento.CodigoEstab);

                List<string> listaChaveNFe = nfEntradaNeg.
                    GetListaNotasEntradaEmpresaNovo(agendamento.CodigoEmpresa, agendamento.CodigoEstab);

                Console.WriteLine(listaNotasXML.Count);
                foreach (string nota in listaChaveNFe)
                {
                    if (listaNotasXML.ContainsKey(nota))
                    {
                        listaNotasXML.Remove(nota);
                    }
                }

                Dictionary<string, NFEntrada> listaNotasXMLNova = new Dictionary<string, NFEntrada>(listaNotasXML);

                foreach (var entry in listaNotasXML)
                {
                    listaNotasXMLNova[entry.Key] = entry.Value;
                }

                string cnpj = estabNeg.GetCNPJEmpresa(agendamento.CodigoEmpresa, agendamento.CodigoEstab);

                foreach (KeyValuePair<string, NFEntrada> nfentrada in listaNotasXML)
                {
                    Console.WriteLine(nfentrada.Value.ChaveNf);
                    if (nfentrada.Value.InscricaoFederal != null)
                    {
                        if (!nfentrada.Value.InscricaoFederal.Equals(cnpj))
                        {
                            if (!nfentrada.Value.InscricaoFederalDestinatario.Equals(cnpj))
                            {
                                listaNotasXMLNova.Remove(nfentrada.Key);
                            }
                        }
                    }
                }

                // Insere as notas no banco

                if (listaNotasXMLNova.Count > 0)
                {
                    nfEntradaNeg.InserirNotas(listaNotasXMLNova);
                }
                else
                {
                    Console.WriteLine("SEM NOTAS A INSERIR!");
                }

                return true;
                Console.WriteLine(listaNotasXML.Count);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }

        public string GetPathEmpresa(Agendamento agendamento, string data)
        {
            string pathEmpresa = Path.Combine(local,
                LeftPad(agendamento.CodigoEmpresa.ToString(), 3, '0'),
                agendamento.CodigoEstab.ToString(),
                data,
                "NFE",
                "ENTRADA");
            return pathEmpresa;
        }
        public string GetPathEmpresaCTE(Agendamento agendamento, string data)
        {
            string pathEmpresa = Path.Combine(local,
                LeftPad(agendamento.CodigoEmpresa.ToString(), 3, '0'),
                agendamento.CodigoEstab.ToString(),
                data,
                "CTE",
                "ENTRADA");
            return pathEmpresa;
        }

        public string LeftPad(string input, int totalWidth, char paddingChar)
        {
            if (input.Length >= totalWidth)
                return input;

            return new string(paddingChar, totalWidth - input.Length) + input;
        }

        public bool ProcessarCTEsEmpresas(Agendamento agendamento, string data)
        {
            try
            {
                data = data.Substring(5, 2) + "-" + data.Substring(0, 4);

                Console.WriteLine($"\nINICIANDO PROCESSAMENTO CTEs EMPRESA {agendamento.CodigoEmpresa} - {agendamento.CodigoEstab}");

                Console.WriteLine("DESCOMPACTANDO OS CTEs...");

                zipUtilz.DescompactarArquivosEmpresa(GetPathEmpresaCTE(agendamento, data));

                Dictionary<string, NFEntrada> listaNotasXML = importaNFEntradaNeg
                                    .CarregarCTEs(GetPathEmpresa(agendamento, data),
                                        agendamento.CodigoEmpresa, agendamento.CodigoEstab);

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;
    }
}

using ImportaNFEntrada.Classes;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using WLContab;
using WLContab.Classes;

namespace ImportaNFEntrada
{
    public class NfEntradaNeg
    {
        public ConexaoPGAutenticacao conexaoPGAutenticacao = new();
        public List<string> GetListaNotasEntradaEmpresaNovo(int codigoempresa, int codigoestab)
        {
            string query = $"SELECT n.chavenf FROM Nfentrada n WHERE n.codigoempresa = {codigoempresa} AND n.codigoestab = {codigoestab}";

            NpgsqlConnection connect = conexaoPGAutenticacao.ConexaoBanco();

            try
            {
                connect.Open();
                var cmd = connect.CreateCommand();
                cmd.CommandText = query;

                var cmdDt = new NpgsqlDataAdapter(cmd);
                var dataTable = new DataTable();
                cmdDt.Fill(dataTable);

                List<string> chavesNFe = new List<string>();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    chavesNFe.Add(dataTable.Rows[i][0].ToString());
                }

                return chavesNFe;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            finally
            {
                connect.Close();
            }
        }

        public List<string> GetListaNotasEntradaEmpresaNovoCTE(int codigoempresa, int codigoestab)
        {
            string query = $"SELECT n.chavenf FROM Nfentrada n WHERE n.codigoempresa = {codigoempresa} AND n.codigoestab = {codigoestab} AND n.especienf = 'CTE' ";

            NpgsqlConnection connect = conexaoPGAutenticacao.ConexaoBanco();

            try
            {
                connect.Open();
                var cmd = connect.CreateCommand();
                cmd.CommandText = query;

                var cmdDt = new NpgsqlDataAdapter(cmd);
                var dataTable = new DataTable();
                cmdDt.Fill(dataTable);

                List<string> chavesNFe = new List<string>();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    chavesNFe.Add(dataTable.Rows[i][0].ToString());
                }

                return chavesNFe;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            finally
            {
                connect.Close();
            }
        }

        public bool InserirNotas(Dictionary<string, NFEntrada> listaNotasXML)
        {
            try
            {
                Console.WriteLine("QUANTIDADE DE NOTAS A SALVAR NO BANCO: " + listaNotasXML.Count);

                foreach (KeyValuePair<string, NFEntrada> nfentrada in listaNotasXML)
                {
                    InserirNotas(nfentrada.Value);
                }
                Console.WriteLine("NOTAS INSERIDAS COM SUCESSO!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }
        public bool InserirNotas(NFEntrada nFEntrada)
        {
            try
            {
                NpgsqlConnection connect = conexaoPGAutenticacao.ConexaoBanco();
                connect.Open();

                string query = @"INSERT INTO NFENTRADA (numeronf, serienf, especienf, chavenf, xml, codigoempresa, codigoestab, periodo, notacancelada, inscricaofederal, valornfe) 
                               VALUES(@Numeronf, @Serienf, @Especienf, @Chavenf, @Xml, @Codigoempresa, @Codigoestab, @Periodo, @Notacancelada, @Inscricaofederal, @Valornfe)";

                var cmd = connect.CreateCommand();
                cmd.CommandText = query;

                cmd.Parameters.AddWithValue(@"Numeronf", nFEntrada.Numeronf);
                cmd.Parameters.AddWithValue(@"Serienf", nFEntrada.SerieNf);
                cmd.Parameters.AddWithValue(@"Especienf", nFEntrada.EspecielNf);
                cmd.Parameters.AddWithValue(@"Chavenf", nFEntrada.ChaveNf);
                cmd.Parameters.AddWithValue(@"Xml", nFEntrada.Xml);
                cmd.Parameters.AddWithValue(@"Codigoempresa", nFEntrada.CodigoEmpresa);
                cmd.Parameters.AddWithValue(@"Codigoestab", nFEntrada.CodigoEstab);
                cmd.Parameters.AddWithValue(@"Periodo", nFEntrada.Periodo);
                cmd.Parameters.AddWithValue(@"Notacancelada", nFEntrada.NotaCancelada);
                cmd.Parameters.AddWithValue(@"Inscricaofederal", nFEntrada.InscricaoFederal);
                cmd.Parameters.AddWithValue(@"Valornfe", nFEntrada.ValorNf);

                cmd.ExecuteNonQuery();
                connect.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }
    }
}

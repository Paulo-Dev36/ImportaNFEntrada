using FirebirdSql.Data.FirebirdClient;
using Npgsql;
using System.Data;
using WLContab;

namespace ImportaNFEntrada.Repositorio
{
    public class EstabNeg
    {
        public ConexaoFDB conexaoFDB = new();
        public string GetCNPJEmpresa(int codigoEmpresa, int codigoEstab)
        {
            string query = $"SELECT E.INSCRFEDERAL FROM ESTAB E WHERE CODIGOEMPRESA = " + codigoEmpresa + " AND CODIGOESTAB = " + codigoEstab;

            FbConnection connect = conexaoFDB.ConexaoBanco();

            try
            {
                connect.Open();
                var cmd = connect.CreateCommand();
                cmd.CommandText = query;

                var cmdDt = new FbDataAdapter(cmd);
                var dataTable = new DataTable();
                cmdDt.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    string cnpj = dataTable.Rows[0][0].ToString();
                    return cnpj.Replace(".", "").Replace("/", "").Replace("-", "");
                }
                else
                {
                    return null;
                }
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
    }
}

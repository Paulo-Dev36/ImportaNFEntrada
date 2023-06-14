using ConexoesBanco;
using ImportaNFEntrada.Classes;
using Npgsql;
using System.Data;
using WLContab.Classes;

namespace ImportaNFEntrada.Repositorio
{
    public class AgendamentoNeg
    {
        private ConexaoPGAutenticacao conexaoPG = new ConexaoPGAutenticacao();
        public List<Agendamento> GetListaAgendamentosPeriodo(string datainicial, string datafinal)
        {
            string query = "SELECT DISTINCT a.codigoEmpresa, a.codigoEstab FROM Agendamento a WHERE a.datainicial >= '"
                    + datainicial + "' and a.datafinal <= '" + datafinal + "'";


            NpgsqlConnection connect = conexaoPG.ConexaoBanco();

            try
            {
                connect.Open();
                var cmd = connect.CreateCommand();
                cmd.CommandText = query;

                var cmdDt = new NpgsqlDataAdapter(cmd);
                var dataTable = new DataTable();
                cmdDt.Fill(dataTable);

                List<Agendamento> agendamentos = new List<Agendamento>();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Agendamento agendamento = new Agendamento();

                    agendamento.CodigoEmpresa = (int)dataTable.Rows[i][0];
                    agendamento.CodigoEstab = (int)dataTable.Rows[i][1];

                    agendamentos.Add(agendamento);
                }
                return agendamentos;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Não foi possível consultar os agendamentos! {ex}");
                connect.Close();
                return null;
            }
            finally
            {
                connect.Close();
            }
        }
    }
}

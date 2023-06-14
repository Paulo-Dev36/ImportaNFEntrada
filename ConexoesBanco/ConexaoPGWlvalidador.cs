using Npgsql;

namespace ConexoesBanco
{
    public class ConexaoPGWlValidador
    {
        public const string ServerName = "192.168.1.5";
        public const string Porta = "5432";
        public const string UserName = "postgres";
        public const string Password = "wl@post2013\r\n";
        public const string DataBase = "wlvalidador";
        public string ConnString = $@"Server={ServerName};Port={Porta};User Id={UserName};Password={Password};Database={DataBase};";

        public NpgsqlConnection Conexao;

        public NpgsqlConnection ConexaoBanco()
        {
            return Conexao = new NpgsqlConnection(ConnString);
        }
    }
}

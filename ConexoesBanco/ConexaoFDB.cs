using FirebirdSql.Data.FirebirdClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WLContab
{
    public class ConexaoFDB
    {
            public const string ServerName = "192.168.1.6";
            public const string Porta = "3050";
            public const string UserName = "sysdba";
            public const string Password = "wldb2017";
            public const string DataBase = @"QUESTOR";
            public string ConnString = $@"Server={ServerName}; User={UserName}; Password={Password}; Database={DataBase}; Port:{Porta}";

        public FbConnection Conexao;

            public FbConnection ConexaoBanco()
            {
                return Conexao = new FbConnection(ConnString);
            }
    }
}

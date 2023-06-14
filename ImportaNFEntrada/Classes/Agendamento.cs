namespace ImportaNFEntrada.Classes
{
    public class Agendamento
    {
        public int IdAgendamento { get; set; }
        public int CodigoEmpresa { get; set; }
        public int CodigoEstab { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public DateTime DataAgendamentoInicial { get; set; }
        public DateTime DataAgendamentoFinal { get; set; }
        public string Status { get; set; }
        public string Sincronizado { get; set; }
        public DateTime DataAgendamento { get; set; }
        public string Usuario { get; set; }

    }
}

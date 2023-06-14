namespace ImportaNFEntrada.Classes
{
    public class NFEntrada
    {
        public int IdNfentrada { get; set; }
        public string Numeronf { get; set; }
        public string SerieNf { get; set; }
        public string EspecielNf { get; set; }
        public string ChaveNf { get; set; }
        public byte[] Xml { get; set; }
        public int CodigoEmpresa { get; set; }
        public int CodigoEstab { get; set; }
        public DateTime Periodo { get; set; }
        public bool NotaCancelada { get; set; }
        public decimal ValorNf { get; set; }
        public string InscricaoFederal { get; set; }
        public string InscricaoFederalDestinatario { get; set; }
    }
}

using ImportaNFEntrada.Classes;
using ImportaNFEntrada.Repositorio;
using ImportaNFEntrada.Utils;

namespace ImportaNFEntrada
{
    public class PrincipalControlador
    {
        protected AgendamentoNeg agendamentoNeg = new AgendamentoNeg();
        protected List<Agendamento> listaAgendamentos;
        protected DataUtils dataUtils = new DataUtils();
        protected PrincipalNeg principalNeg = new PrincipalNeg();
        public void Processar() 
        {
            StreamWriter streamWriter = new StreamWriter($"S:\\LOGs ENTRADAS\\{DateTime.Today.ToString().Substring(0, 10).Replace('/', '-')}" +
                $"-{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.txt");
            string dataInicial = dataUtils.ConverteData(dataUtils.GetPrimeiroDiaMesAnterior());
            string dataFinal = dataUtils.DateToStringNew(dataUtils.GetUltimoDiaMes(DateTime.Now), "yyyy-MM-dd");
            streamWriter.WriteLine("INICIANDO PROCESSAMENTO DE AGENDAMENTOS");
            
            Console.WriteLine("INICIANDO PROCESSAMENTO DE AGENDAMENTOS");

            listaAgendamentos = agendamentoNeg.GetListaAgendamentosPeriodo(dataInicial, dataFinal);

            foreach (Agendamento agendamento in listaAgendamentos)
            {
                principalNeg.ProcessarNotasEmpresas(agendamento, dataInicial);
                principalNeg.ProcessarCTEsEmpresas(agendamento, dataInicial);
            }

            // MÊS ATUAL
            streamWriter.WriteLine("\nINICIANDO PROCESSAMENTO DE AGENDAMENTOS MÊS ATUAL");
            Console.WriteLine("\nINICIANDO PROCESSAMENTO DE AGENDAMENTOS MÊS ATUAL");
            string dataInicialNova = dataUtils.GetYear() + "-" + principalNeg.LeftPad(dataUtils.GetMesDataCorretoNumero(DateTime.Now), 2, '0') + "-01";

            foreach (Agendamento agendamento in listaAgendamentos)
            {
                principalNeg.ProcessarNotasEmpresas(agendamento, dataInicialNova);
                principalNeg.ProcessarCTEsEmpresas(agendamento, dataInicialNova);
            }
            streamWriter.Close();
        }
    }
}

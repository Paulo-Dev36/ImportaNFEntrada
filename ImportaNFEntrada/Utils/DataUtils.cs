namespace ImportaNFEntrada.Utils
{
    public class DataUtils
    {
        public string ConverteData(DateTime dtData)
        {
            try
            {
                return dtData.ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return "Erro na conversão da data";
            }
        }

        public DateTime GetPrimeiroDiaMesAnterior()
        {
            DateTime mesanterior = GetNextMonth(DateTime.Now, -1);
            DateTime end = SetDia(mesanterior, 1);
            return end;
        }

        public DateTime GetNextMonth(DateTime data, int qtde)
        {
            DateTime nextMonth = data.AddMonths(qtde);
            return nextMonth;
        }

        public DateTime SetDia(DateTime data, int dia)
        {
            DateTime newData = new DateTime(data.Year, data.Month, dia);
            return newData;
        }

        public DateTime GetUltimoDiaMes(DateTime date)
        {
            int ultimoDiaMes = DateTime.DaysInMonth(date.Year, date.Month);
            DateTime ultimoDia = SetDia(date, ultimoDiaMes);
            return ultimoDia;
        }

        public string DateToStringNew(DateTime date, string mask)
        {
            string data = date.ToString(mask);
            return data;
        }

        public string GetYear()
        {
            int year = DateTime.Now.Year;
            return year.ToString();
        }

        public string GetMesDataCorretoNumero(DateTime date)
        {
            int mes = date.Month;
            return mes.ToString();
        }
    }
}

using ImportaNFEntrada;

public class Principal
{
    public static void Main(string[] Args)
    {
        PrincipalControlador principalControlador = new PrincipalControlador();
        principalControlador.Processar();
        Environment.Exit(0);
    }
}
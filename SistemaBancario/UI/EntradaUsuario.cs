using System.Globalization;

namespace SistemaBancario.UI;

public static class EntradaUsuario
{
    private static readonly CultureInfo CulturaBrasil = new("pt-BR");

    public static string LerTexto(string mensagem)
    {
        while (true)
        {
            Console.Write(mensagem);
            string texto = Console.ReadLine()?.Trim() ?? string.Empty;

            if (texto.Length > 0)
                return texto;

            MostrarErro("O campo não pode ficar vazio.");
        }
    }

    public static int LerInteiro(string mensagem)
    {
        while (true)
        {
            try
            {
                return int.Parse(LerTexto(mensagem));
            }
            catch (FormatException)
            {
                MostrarErro("Digite apenas números inteiros.");
            }
            catch (OverflowException)
            {
                MostrarErro("Número muito grande.");
            }
        }
    }

    public static decimal LerDecimal(string mensagem)
    {
        while (true)
        {
            try
            {
                string texto = LerTexto(mensagem).Replace('.', ',');
                return decimal.Parse(texto, NumberStyles.Number, CulturaBrasil);
            }
            catch (FormatException)
            {
                MostrarErro("Valor inválido. Exemplo: 150,75");
            }
            catch (OverflowException)
            {
                MostrarErro("Valor muito grande.");
            }
        }
    }

    public static void MostrarErro(string mensagem)
    {
        EscreverColorido($"  Erro: {mensagem}", ConsoleColor.Red);
    }

    public static void MostrarSucesso(string mensagem)
    {
        EscreverColorido($"  {mensagem}", ConsoleColor.Green);
    }

    private static void EscreverColorido(string mensagem, ConsoleColor cor)
    {
        Console.ForegroundColor = cor;
        Console.WriteLine(mensagem);
        Console.ResetColor();
    }
}

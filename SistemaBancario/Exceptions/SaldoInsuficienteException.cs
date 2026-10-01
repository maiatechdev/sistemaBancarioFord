namespace SistemaBancario.Exceptions;

public class SaldoInsuficienteException : Exception
{
    public SaldoInsuficienteException(decimal saldoAtual, decimal valorNecessario)
        : base($"Saldo insuficiente. Saldo atual: {saldoAtual:C} | Valor necessário: {valorNecessario:C}")
    {
    }
}

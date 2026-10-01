namespace SistemaBancario.Exceptions;

public class ContaNaoEncontradaException : Exception
{
    public ContaNaoEncontradaException(int numeroConta)
        : base($"Conta {numeroConta} não encontrada.")
    {
    }
}

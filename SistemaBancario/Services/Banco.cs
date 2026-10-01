using SistemaBancario.Exceptions;
using SistemaBancario.Interfaces;
using SistemaBancario.Models;

namespace SistemaBancario.Services;

public class Banco
{
    private readonly Dictionary<int, ContaBancaria> _contas = new();
    private int _proximoNumero = 1001;

    public string Nome { get; }

    public IReadOnlyCollection<ContaBancaria> Contas => _contas.Values;

    public Banco(string nome)
    {
        Nome = nome;
    }

    public ContaBancaria AbrirConta(TipoConta tipo, string titular)
    {
        ContaBancaria conta = tipo switch
        {
            TipoConta.Corrente => new ContaCorrente(_proximoNumero, titular),
            TipoConta.Poupanca => new ContaPoupanca(_proximoNumero, titular),
            TipoConta.Empresarial => new ContaEmpresarial(_proximoNumero, titular),
            _ => throw new ArgumentException("Tipo de conta inválido.")
        };

        _contas.Add(conta.NumeroConta, conta);
        _proximoNumero++;

        return conta;
    }

    public ContaBancaria BuscarConta(int numeroConta)
    {
        if (!_contas.TryGetValue(numeroConta, out ContaBancaria? conta))
            throw new ContaNaoEncontradaException(numeroConta);

        return conta;
    }

    public void Transferir(int numeroOrigem, int numeroDestino, decimal valor)
    {
        if (numeroOrigem == numeroDestino)
            throw new InvalidOperationException("A conta de origem e destino devem ser diferentes.");

        ContaBancaria origem = BuscarConta(numeroOrigem);
        ContaBancaria destino = BuscarConta(numeroDestino);

        origem.Sacar(valor);
        destino.Depositar(valor);
    }

    public int AplicarRendimentos()
    {
        int contasAtualizadas = 0;

        foreach (ContaBancaria conta in _contas.Values)
        {
            if (conta is IRentavel rentavel && rentavel.AplicarRendimento() > 0)
                contasAtualizadas++;
        }

        return contasAtualizadas;
    }
}

using SistemaBancario.Exceptions;

namespace SistemaBancario.Models;

public abstract class ContaBancaria
{
    private readonly List<Transacao> _historico = new();

    public int NumeroConta { get; }
    public string Titular { get; }

    public decimal Saldo { get; private set; }

    public IReadOnlyList<Transacao> Historico => _historico;

    public abstract string Tipo { get; }

    protected ContaBancaria(int numeroConta, string titular)
    {
        if (string.IsNullOrWhiteSpace(titular))
            throw new ArgumentException("O nome do titular é obrigatório.");

        NumeroConta = numeroConta;
        Titular = titular.Trim();
    }

    public void Depositar(decimal valor)
    {
        ValidarValor(valor);
        Creditar(valor, "Depósito");
    }

    public abstract void Sacar(decimal valor);

    public virtual string ObterResumo()
    {
        return $"[{NumeroConta}] {Tipo,-11} | {Titular,-20} | Saldo: {Saldo,12:C}";
    }

    protected static void ValidarValor(decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentException("O valor deve ser maior que zero.");
    }

    protected void GarantirSaldo(decimal valorNecessario)
    {
        if (valorNecessario > Saldo)
            throw new SaldoInsuficienteException(Saldo, valorNecessario);
    }

    protected void Creditar(decimal valor, string descricao)
    {
        Saldo += valor;
        _historico.Add(new Transacao(descricao, valor, DateTime.Now));
    }

    protected void Debitar(decimal valor, string descricao)
    {
        Saldo -= valor;
        _historico.Add(new Transacao(descricao, -valor, DateTime.Now));
    }
}

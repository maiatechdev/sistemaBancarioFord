namespace SistemaBancario.Models;

public class ContaEmpresarial : ContaBancaria
{
    public const decimal LimitePadrao = 10_000m;

    public decimal LimiteEmprestimo { get; }
    public decimal EmprestimoUtilizado { get; private set; }
    public decimal LimiteDisponivel => LimiteEmprestimo - EmprestimoUtilizado;

    public override string Tipo => "Empresarial";

    public ContaEmpresarial(int numeroConta, string titular, decimal limiteEmprestimo = LimitePadrao)
        : base(numeroConta, titular)
    {
        if (limiteEmprestimo < 0)
            throw new ArgumentException("O limite de empréstimo não pode ser negativo.");

        LimiteEmprestimo = limiteEmprestimo;
    }

    public override void Sacar(decimal valor)
    {
        ValidarValor(valor);
        GarantirSaldo(valor);

        Debitar(valor, "Saque");
    }

    public void SolicitarEmprestimo(decimal valor)
    {
        ValidarValor(valor);

        if (valor > LimiteDisponivel)
            throw new InvalidOperationException(
                $"Valor acima do limite disponível para empréstimo ({LimiteDisponivel:C}).");

        EmprestimoUtilizado += valor;
        Creditar(valor, "Empréstimo");
    }

    public override string ObterResumo()
    {
        return base.ObterResumo() + $" | Limite disponível: {LimiteDisponivel:C}";
    }
}

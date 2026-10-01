using SistemaBancario.Interfaces;

namespace SistemaBancario.Models;

public class ContaPoupanca : ContaBancaria, IRentavel
{
    public decimal TaxaRendimentoMensal => 0.005m; // 0,5% ao mês

    public override string Tipo => "Poupança";

    public ContaPoupanca(int numeroConta, string titular)
        : base(numeroConta, titular)
    {
    }

    public override void Sacar(decimal valor)
    {
        ValidarValor(valor);
        GarantirSaldo(valor);

        Debitar(valor, "Saque");
    }

    public decimal AplicarRendimento()
    {
        decimal rendimento = Math.Round(Saldo * TaxaRendimentoMensal, 2);

        if (rendimento > 0)
            Creditar(rendimento, "Rendimento mensal");

        return rendimento;
    }
}

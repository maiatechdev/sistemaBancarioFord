namespace SistemaBancario.Models;

public class ContaCorrente : ContaBancaria
{
    public const decimal TaxaSaque = 2.50m;

    public override string Tipo => "Corrente";

    public ContaCorrente(int numeroConta, string titular)
        : base(numeroConta, titular)
    {
    }

    public override void Sacar(decimal valor)
    {
        ValidarValor(valor);
        GarantirSaldo(valor + TaxaSaque);

        Debitar(valor, "Saque");
        Debitar(TaxaSaque, "Taxa de saque");
    }
}

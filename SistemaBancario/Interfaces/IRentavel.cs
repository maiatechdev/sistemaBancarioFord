namespace SistemaBancario.Interfaces;

public interface IRentavel
{
    decimal TaxaRendimentoMensal { get; }

    decimal AplicarRendimento();
}

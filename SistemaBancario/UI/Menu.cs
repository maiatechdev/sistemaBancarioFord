using SistemaBancario.Exceptions;
using SistemaBancario.Models;
using SistemaBancario.Services;

namespace SistemaBancario.UI;

public class Menu
{
    private static readonly string[] Opcoes =
    {
        "1 - Abrir conta",
        "2 - Depositar",
        "3 - Sacar",
        "4 - Transferir",
        "5 - Consultar extrato",
        "6 - Listar contas",
        "7 - Solicitar empréstimo (conta empresarial)",
        "8 - Aplicar rendimento mensal (poupanças)",
        "0 - Sair"
    };

    private readonly Banco _banco;

    public Menu(Banco banco)
    {
        _banco = banco;
    }

    public void Executar()
    {
        int opcao;

        do
        {
            ExibirMenu();
            opcao = EntradaUsuario.LerInteiro("Escolha uma opção: ");
            Console.WriteLine();

            try
            {
                ExecutarOpcao(opcao);
            }
            catch (SaldoInsuficienteException ex)
            {
                EntradaUsuario.MostrarErro(ex.Message);
            }
            catch (ContaNaoEncontradaException ex)
            {
                EntradaUsuario.MostrarErro(ex.Message);
            }
            catch (ArgumentException ex)
            {
                EntradaUsuario.MostrarErro(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                EntradaUsuario.MostrarErro(ex.Message);
            }
        } while (opcao != 0);
    }

    private void ExibirMenu()
    {
        Console.WriteLine();
        Console.WriteLine($"========== {_banco.Nome} ==========");
        foreach (string item in Opcoes)
            Console.WriteLine(item);
        Console.WriteLine(new string('=', 22 + _banco.Nome.Length));
    }

    private void ExecutarOpcao(int opcao)
    {
        switch (opcao)
        {
            case 1: AbrirConta(); break;
            case 2: Depositar(); break;
            case 3: Sacar(); break;
            case 4: Transferir(); break;
            case 5: ConsultarExtrato(); break;
            case 6: ListarContas(); break;
            case 7: SolicitarEmprestimo(); break;
            case 8: AplicarRendimentos(); break;
            case 0: Console.WriteLine("Obrigado por usar o sistema. Até logo!"); break;
            default: EntradaUsuario.MostrarErro("Opção inválida."); break;
        }
    }

    private void AbrirConta()
    {
        Console.WriteLine("Tipos de conta: 1 - Corrente | 2 - Poupança | 3 - Empresarial");
        int tipo = EntradaUsuario.LerInteiro("Tipo: ");

        if (!Enum.IsDefined(typeof(TipoConta), tipo))
            throw new ArgumentException("Tipo de conta inválido.");

        string titular = EntradaUsuario.LerTexto("Nome do titular: ");
        ContaBancaria conta = _banco.AbrirConta((TipoConta)tipo, titular);

        EntradaUsuario.MostrarSucesso($"Conta {conta.Tipo} nº {conta.NumeroConta} aberta para {conta.Titular}.");
    }

    private void Depositar()
    {
        ContaBancaria conta = _banco.BuscarConta(EntradaUsuario.LerInteiro("Número da conta: "));
        decimal valor = EntradaUsuario.LerDecimal("Valor do depósito: R$ ");

        conta.Depositar(valor);
        EntradaUsuario.MostrarSucesso($"Depósito realizado. Novo saldo: {conta.Saldo:C}");
    }

    private void Sacar()
    {
        ContaBancaria conta = _banco.BuscarConta(EntradaUsuario.LerInteiro("Número da conta: "));

        if (conta is ContaCorrente)
            Console.WriteLine($"  Atenção: conta corrente cobra taxa de {ContaCorrente.TaxaSaque:C} por saque.");

        decimal valor = EntradaUsuario.LerDecimal("Valor do saque: R$ ");

        conta.Sacar(valor);
        EntradaUsuario.MostrarSucesso($"Saque realizado. Novo saldo: {conta.Saldo:C}");
    }

    private void Transferir()
    {
        int origem = EntradaUsuario.LerInteiro("Conta de origem: ");
        int destino = EntradaUsuario.LerInteiro("Conta de destino: ");
        decimal valor = EntradaUsuario.LerDecimal("Valor da transferência: R$ ");

        _banco.Transferir(origem, destino, valor);
        EntradaUsuario.MostrarSucesso("Transferência realizada com sucesso.");
    }

    private void ConsultarExtrato()
    {
        ContaBancaria conta = _banco.BuscarConta(EntradaUsuario.LerInteiro("Número da conta: "));

        Console.WriteLine($"--- Extrato da conta {conta.NumeroConta} ({conta.Tipo}) - {conta.Titular} ---");

        if (conta.Historico.Count == 0)
            Console.WriteLine("Nenhuma movimentação.");

        foreach (Transacao transacao in conta.Historico)
            Console.WriteLine($"{transacao.Data:dd/MM/yyyy HH:mm}  {transacao.Descricao,-18} {transacao.Valor,12:C}");

        Console.WriteLine($"Saldo atual: {conta.Saldo:C}");
    }

    private void ListarContas()
    {
        if (_banco.Contas.Count == 0)
        {
            Console.WriteLine("Nenhuma conta cadastrada.");
            return;
        }

        // Polimorfismo: cada conta monta o próprio resumo.
        foreach (ContaBancaria conta in _banco.Contas)
            Console.WriteLine(conta.ObterResumo());
    }

    private void SolicitarEmprestimo()
    {
        ContaBancaria conta = _banco.BuscarConta(EntradaUsuario.LerInteiro("Número da conta: "));

        if (conta is not ContaEmpresarial empresarial)
            throw new InvalidOperationException("Empréstimo disponível apenas para contas empresariais.");

        Console.WriteLine($"  Limite disponível: {empresarial.LimiteDisponivel:C}");
        decimal valor = EntradaUsuario.LerDecimal("Valor do empréstimo: R$ ");

        empresarial.SolicitarEmprestimo(valor);
        EntradaUsuario.MostrarSucesso($"Empréstimo liberado. Novo saldo: {empresarial.Saldo:C}");
    }

    private void AplicarRendimentos()
    {
        int quantidade = _banco.AplicarRendimentos();
        EntradaUsuario.MostrarSucesso($"Rendimento aplicado em {quantidade} conta(s) poupança.");
    }
}

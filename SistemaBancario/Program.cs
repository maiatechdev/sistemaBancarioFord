using System.Globalization;
using System.Text;
using SistemaBancario.Services;
using SistemaBancario.UI;

Console.OutputEncoding = Encoding.UTF8;
CultureInfo.CurrentCulture = new CultureInfo("pt-BR");

var banco = new Banco("Banco Ford Enter");
var menu = new Menu(banco);

menu.Executar();

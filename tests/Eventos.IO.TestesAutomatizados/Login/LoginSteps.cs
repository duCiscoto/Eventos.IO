using Eventos.IO.TestesAutomatizados.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventos.IO.TestesAutomatizados.Login
{
    public class LoginSteps
    {
        public static void Login(SeleniumHelper browser)
        {
            browser.ClicarLinkPorTexto("Entrar");
            browser.PrencherTextBoxPorId("email", ConfigurationHelper.TestUserName);
            browser.PrencherTextBoxPorId("senha", ConfigurationHelper.TestPassword);
            browser.ClicarBotaoPorId("login");
        }
    }
}

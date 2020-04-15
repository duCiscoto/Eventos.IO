using Eventos.IO.TestesAutomatizados.Config;
using Eventos.IO.TestesAutomatizados.Login;
using System;
using System.Threading;
using TechTalk.SpecFlow;
using Xunit;

namespace Eventos.IO.TestesAutomatizados.CadastrarEvento
{
    [Binding]
    public class AdicionarNovoEventoSteps
    {
        public SeleniumHelper Browser;

        public AdicionarNovoEventoSteps()
        {
            Browser = SeleniumHelper.Instance();
        }

        [Given(@"que o Organizador está no site")]
        public void DadoQueOOrganizadorEstaNoSite()
        {
            var url = Browser.NavegarParaUrl(ConfigurationHelper.SiteUrl);
            Assert.Equal(ConfigurationHelper.SiteUrl, url);
        }
        
        [Given(@"Realiza o Login")]
        public void DadoRealizaOLogin()
        {
            LoginSteps.Login(Browser);
            Thread.Sleep(5000);
        }
        
        [Given(@"Navega até a área administrativa")]
        public void DadoNavegaAteAAreaAdministrativa()
        {
            Browser.ClicarLinkPorTexto("Meus Eventos");
        }
        
        [Given(@"Clica em novo evento")]
        public void DadoClicaEmNovoEvento()
        {
            Browser.ClicarBotaoPorId("novoEvento");
        }
        
        [Given(@"Preenche o formulário com os valores")]
        public void DadoPreencheOFormularioComOsValores(Table table)
        {
            Browser.PrencherTextBoxPorId(table.Rows[0][0], table.Rows[0][1]);
            Browser.PrencherTextBoxPorId(table.Rows[1][0], table.Rows[1][1]);
            Browser.PrencherTextBoxPorId(table.Rows[2][0], table.Rows[2][1]);
            Browser.PrencherDropDownPorId(table.Rows[3][0], table.Rows[3][1]);
            Browser.PrencherTextBoxPorXPath(table.Rows[4][0], table.Rows[4][1]);
            Browser.PrencherTextBoxPorXPath(table.Rows[5][0], table.Rows[5][1]);
            Browser.PrencherTextBoxPorId(table.Rows[6][0], table.Rows[6][1]);
            Browser.PrencherTextBoxPorId(table.Rows[7][0], table.Rows[7][1]);
            Browser.PrencherTextBoxPorId(table.Rows[8][0], table.Rows[8][1]);
            Browser.PrencherTextBoxPorId(table.Rows[9][0], table.Rows[9][1]);
            Browser.PrencherTextBoxPorId(table.Rows[10][0], table.Rows[10][1]);
            Browser.PrencherTextBoxPorId(table.Rows[11][0], table.Rows[11][1]);
            Browser.PrencherTextBoxPorId(table.Rows[12][0], table.Rows[12][1]);
            Browser.PrencherTextBoxPorId(table.Rows[13][0], table.Rows[13][1]);
            Browser.PrencherTextBoxPorId(table.Rows[14][0], table.Rows[14][1]);
            Browser.PrencherTextBoxPorId(table.Rows[15][0], table.Rows[15][1]);
            Browser.PrencherDropDownPorId(table.Rows[16][0], table.Rows[16][1]);
        }
        
        [When(@"clicar no botão adicionar")]
        public void QuandoClicarNoBotaoAdicionar()
        {
            Browser.ClicarBotaoPorId("adicionarEvento");
            Thread.Sleep(5000);
        }
        
        [Then(@"O evento é registrado e o usuário é redirecionado para a lista de eventos")]
        public void EntaoOEventoERegistradoEOUsuarioERedirecionadoParaAListaDeEventos()
        {
            Assert.Equal("http://localhost:44361/eventos/meus-eventos", Browser.ObterUrl());
            Browser.ObterScreenShot("NovoEventoCadastrado");
        }
    }
}

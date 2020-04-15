using Eventos.IO.TestesAutomatizados.Config;
using System;
using TechTalk.SpecFlow;
using Xunit;

namespace Eventos.IO.TestesAutomatizados.CadastrarOrganizador
{
    [Binding]
    public class CadastroDeOrganizadorSteps
    {
        public SeleniumHelper Browser;

        public CadastroDeOrganizadorSteps()
        {
            Browser = SeleniumHelper.Instance();
        }

        // AAA -> Arrange, Act, Assert

        // Arrange (preparação para testar)

        [Given(@"que o Organizador está no site, na página inicial")]
        public void DadoQueOOrganizadorEstaNoSiteNaPaginaInicial()
        {
            var url = Browser.NavegarParaUrl(ConfigurationHelper.SiteUrl);
            Assert.Equal(ConfigurationHelper.SiteUrl, url);
        }

        [Given(@"clica no link de registro")]
        public void DadoClicaNoLinkDeRegistro()
        {
            Browser.ClicarLinkPorTexto("Registre-se");
        }

        [Given(@"preenche os campos com os valores")]
        public void DadoPreencheOsCamposComOsValores(Table table)
        {
            Browser.PrencherTextBoxPorId(table.Rows[0][0], table.Rows[0][1]);
            Browser.PrencherTextBoxPorId(table.Rows[1][0], table.Rows[1][1]);
            Browser.PrencherTextBoxPorId(table.Rows[2][0], table.Rows[2][1]);
            Browser.PrencherTextBoxPorId(table.Rows[3][0], table.Rows[3][1]);
            Browser.PrencherTextBoxPorId(table.Rows[4][0], table.Rows[4][1]);
        }

        // Act (Ação)

        [When(@"clicar no botão registrar")]
        public void QuandoClicarNoBotaoRegistrar()
        {
            Browser.ClicarBotaoPorId("Registrar");
        }

        // Assert (Validação)

        [Then(@"será registrado e redirecionado com sucesso")]
        public void EntaoSeraRegistradoERedirecionadoComSucesso()
        {
            var returnText = Browser.ObterTextoElementoPorId("saudacaoUsuario");

            Assert.Contains("olá eduardo", returnText.ToLower());

            Browser.ObterScreenShot("EvidenciaCadastro");
        }

        // Cenários de Falha

        [Then(@"recebe uma mensagem de erro de CPF já cadastrado")]
        public void EntaoRecebeUmaMensagemDeErroDeCPFJaCadastrado()
        {
            var result = Browser.ObterTextoElementoPorClasse("alert-danger");
            Assert.Contains("cpf ou e-mail já utilizados", result.ToLower());

            Browser.ObterScreenShot("CPF_Erro");
        }

        [Then(@"recebe uma mensagem de erro de Email já cadastrado")]
        public void EntaoRecebeUmaMensagemDeErroDeEmailJaCadastrado()
        {
            var result = Browser.ObterTextoElementoPorClasse("alert-danger");
            Assert.Contains("is already taken", result.ToLower());

            Browser.ObterScreenShot("Email_Erro");
        }

        [Then(@"recebe uma mensagem de erro que a senha requer número")]
        public void EntaoRecebeUmaMensagemDeErroQueASenhaRequerNumero()
        {
            var result = Browser.ObterTextoElementoPorClasse("alert-danger");
            Assert.Contains("passowords must have at least one digit ('0'-'9')", result.ToLower());
        }

        [Then(@"recebe uma mensagem de erro que a senha requer letra maiúscula")]
        public void EntaoRecebeUmaMensagemDeErroQueASenhaRequerLetraMaiuscula()
        {
            var result = Browser.ObterTextoElementoPorClasse("alert-danger");
            Assert.Contains("passowords must have at least one uppercase ('a'-'z')", result.ToLower());
        }

        [Then(@"recebe uma mensagem de erro que a senha requer letra minúscula")]
        public void EntaoRecebeUmaMensagemDeErroQueASenhaRequerLetraMinuscula()
        {
            var result = Browser.ObterTextoElementoPorClasse("alert-danger");
            Assert.Contains("passowords must have at least one lowercase ('a'-'z')", result.ToLower());
        }

        [Then(@"recebe uma mensagem de erro que a senha requer caracter especial")]
        public void EntaoRecebeUmaMensagemDeErroQueASenhaRequerCaracterEspecial()
        {
            var result = Browser.ObterTextoElementoPorClasse("alert-danger");
            Assert.Contains("passowords must have at least one non alphanumeric character", result.ToLower());
        }

        [Then(@"recebe uma mensagem de erro que a senha é menor que o tamanho permitido")]
        public void EntaoRecebeUmaMensagemDeErroQueASenhaEMenorQueOTamanhoPermitido()
        {
            var result = Browser.ObterTextoElementoPorClasse("alert-danger");
            Assert.Contains("a senha deve possuir no mínimo 6 caracteres", result.ToLower());
        }

        [Then(@"recebe uma mensagem de erro que a senhas diferentes")]
        public void EntaoRecebeUmaMensagemDeErroQueASenhasDiferentes()
        {
            var result = Browser.ObterTextoElementoPorClasse("alert-danger");
            Assert.Contains("as senhas não conferem", result.ToLower());
        }

    }
}

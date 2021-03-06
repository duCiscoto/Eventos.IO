﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventos.IO.TestesAutomatizados.Config
{
    public class SeleniumHelper
    {
        public static IWebDriver CD;

        // Executa todos os testes na mesma janela
        private static SeleniumHelper _instance;

        public WebDriverWait Wait;

        protected SeleniumHelper()
        {
            CD = new ChromeDriver(ConfigurationHelper.ChromeDrive);
            Wait = new WebDriverWait(CD, TimeSpan.FromSeconds(30));
        }

        public static SeleniumHelper Instance()
        {
            return _instance ?? (_instance = new SeleniumHelper());
        }

        public string ObterUrl()
        {
            return CD.Url;
        }

        public bool ValidarConteudoUrl(string conteudo)
        {
            return Wait.Until(ExpectedConditions.UrlContains(conteudo));
        }

        public string NavegarParaUrl(string url)
        {
            CD.Navigate().GoToUrl(url);

            return CD.Url;
        }

        public void ClicarLinkPorTexto(string linkText)
        {
            var link = Wait.Until(ExpectedConditions.ElementIsVisible(By.LinkText(linkText)));
            
            link.Click();
        }

        public void ClicarBotaoPorId(string botaoId)
        {
            var botao = Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(botaoId)));

            botao.Click();
        }

        public void PrencherTextBoxPorId(string idCampo, string valorCampo)
        {
            var campo = Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(idCampo)));
            
            campo.SendKeys(valorCampo);
        }

        public void PrencherTextBoxPorXPath(string xpath, string valorCampo)
        {
            var campo = Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xpath)));

            campo.SendKeys(valorCampo);
        }

        public void PrencherDropDownPorId(string idCampo, string valorCampo)
        {
            var campo = Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(idCampo)));

            var selectElement = new SelectElement(campo);

            selectElement.SelectByValue(valorCampo);
        }

        public string ObterTextoElementoPorClasse(string className)
        {
            return Wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName(className))).Text;
        }
        
        public string ObterTextoElementoPorId(string id)
        {
            return Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(id))).Text;
        }

        public IEnumerable<IWebElement> ObterListaPorClasse(string className)
        {
            return Wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.ClassName(className)));
        }

        public void ObterScreenShot(string nome)
        {
            var screenShot = ((ITakesScreenshot)CD).GetScreenshot();
            SalvarScreenShot(screenShot, string.Format("{0}_" + nome + ".png", DateTime.Now.ToFileTime()));
        }

        private static void SalvarScreenShot(Screenshot screenShot, string fileName)
        {
            screenShot.SaveAsFile(string.Format("{0}{1}", ConfigurationHelper.FolderPicture, fileName),
                ScreenshotImageFormat.Png);
        }
    }
}

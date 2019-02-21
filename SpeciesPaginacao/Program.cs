using System;

namespace ConsoleApp3
{
    using HtmlAgilityPack;
    using SpeciesPaginacao.Models;
    using System.Collections.Generic;

    class Program
    {
        public static List<Specie> listaEspecie = new List<Specie>();

        static void Main(string[] args)
        {
            var link = "https://www.worldwildlife.org/species/directory?sort=extinction_status";

            CapturaTodasEspecies(link);
        }

        private static void CapturaTodasEspecies(string link)
        {
            var doc = GetHtmlNode(link);

            AddEspecieNaLista(doc);

            var nodeNext = doc.SelectSingleNode("//a[@rel='next']");

            if (nodeNext != null)
            {
                var att = nodeNext.GetAttributeValue("href", string.Empty);
                string attLink = "https://www.worldwildlife.org" + att;

                CapturaTodasEspecies(attLink);
            }
        }

        private static HtmlNode GetHtmlNode(string href)
        {
            var web = new HtmlWeb();
            HtmlDocument htmldoc = web.Load(href);
            HtmlNode htmlNode = htmldoc.DocumentNode;

            return htmlNode;
        }

        private static void AddEspecieNaLista(HtmlNode htmlNode)
        {
            var linhas = htmlNode.SelectNodes("//tbody/tr");

            foreach (var linha in linhas)
            {
                Specie especie = CriaEspecie(linha);

                listaEspecie.Add(especie);
            }
        }

        private static Specie CriaEspecie(HtmlNode linha)
        {
            HtmlNode nomeComum = linha.SelectSingleNode("./td[1]");
            HtmlNode nomeCientifico = linha.SelectSingleNode("./td[2]");
            HtmlNode statusConcervacao = linha.SelectSingleNode("./td[3]");

            if (nomeComum == null || nomeCientifico == null || statusConcervacao == null)
            {
                throw new Exception("Não foi possivel capturar os atributos da especie!");
            }

            Specie specie = new Specie
            {
                NomeCientifico = nomeCientifico.InnerText,
                NomeComum = nomeComum.InnerText,
                StatusConcervacao = statusConcervacao.InnerText
            };

            return specie;
        }
    }
}

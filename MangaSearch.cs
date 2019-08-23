using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MangaPDF
{
    class MangaSearch
    {

        HttpClient httpClient;
        string html, url;
        HtmlDocument htmlDocument;

        public MangaSearch()
        {
            httpClient = new HttpClient();
            htmlDocument = new HtmlDocument();
        }

        public async Task<HtmlDocument> GetHtmlDocument()
        {
            html = await httpClient.GetStringAsync(url);
            htmlDocument.LoadHtml(html);

            return htmlDocument;
        }

        public async Task<HtmlDocument> GetHtmlDocument(String customUrl)
        {
            html = await httpClient.GetStringAsync(customUrl);
            htmlDocument.LoadHtml(html);

            return htmlDocument;
        }

        public void StopConnection()
        {
            httpClient.CancelPendingRequests();
            httpClient.Dispose();
        }

        public MangaSearch SetURL(string[] terms)
        {
            if (terms.Length == 0) throw new Exception("Search terms are empty.");

            url = @"https://manganelo.com/search/";
            
            foreach(string term in terms) url += "_" + term;

            return this;
        }

        public List<Manga> GetMangasFromHtml()
        {
            List<Manga> mangas = new List<Manga>();

            var divs = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "").Equals("story_item")).ToList();

            foreach(HtmlNode div in divs)
            {
                string imageSrc, mangaName, mangaUrl;

                imageSrc = div.Descendants("img").FirstOrDefault().GetAttributeValue("src", "");
                mangaName = div.Descendants("h3").FirstOrDefault().Descendants("a").FirstOrDefault().InnerText;
                mangaUrl = div.Descendants("h3").FirstOrDefault().Descendants("a").FirstOrDefault().GetAttributeValue("href", "");

                mangas.Add(new Manga(mangaName, mangaUrl, imageSrc));
            }

            return mangas;
        }

        public async Task<List<string>> getImageSources(String url)
        {
            List<string> sources = new List<string>();

            html = await httpClient.GetStringAsync(url);
            htmlDocument.LoadHtml(html);

            var imgs = htmlDocument.DocumentNode.Descendants("div").Where(node => node.GetAttributeValue("class", "").Equals("vung-doc")).FirstOrDefault().Descendants("img").ToList();

            foreach (var img in imgs)
            {
                sources.Add(img.GetAttributeValue("src", ""));
            }

            return sources;
        }

        public async Task<List<HtmlNode>> getChapterLinks(string url)
        {
            html = await httpClient.GetStringAsync(url);
            htmlDocument.LoadHtml(html);

            var divs = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "").Equals("row")).ToList();

            divs.Reverse();

            Console.WriteLine("NUMBER: " + divs.Count);

            return divs;
        }

    }
}

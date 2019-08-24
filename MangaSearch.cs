using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

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

        /**
         * Connects and loads HTML document
         * @return html document
         */
        public async Task<HtmlDocument> GetHtmlDocument()
        {
            html = await httpClient.GetStringAsync(url);
            htmlDocument.LoadHtml(html);

            return htmlDocument;
        }

        /**
         * Connects and loads HTML document from custom URL
         * @return html document
         */
        public async Task<HtmlDocument> GetHtmlDocument(String customUrl)
        {
            html = await httpClient.GetStringAsync(customUrl);
            htmlDocument.LoadHtml(html);

            return htmlDocument;
        }

        /**
         * Sets URL according to given terms
         * @param terms search terms that are to be integrated in the search url
         */
        public void SetURL(string[] terms)
        {
            url = @"https://manganelo.com/search/";
            
            foreach(string term in terms) url += "_" + term;
        }

        /**
         * Searches the html document for div's with the class story_item.
         * Creates and stores an instance of a Manga class with the name, url and imageSrc of said manga
         * @return list of type Manga with all mangas found
         */
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

        /**
         * Connects and loads html from given URL. Searches for all divs with class "row". These are all chapters that contain it's url.
         * @param url manga page which contains all chapters
         * @return List of type HtmlNode
         */
        public async Task<List<HtmlNode>> getChapterLinks(string url)
        {
            await GetHtmlDocument(url);

            var divs = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "").Equals("row")).ToList();

            divs.Reverse();

            return divs;
        }

        /**
         * Connects and loads html from given URL. Searches for all images inside a div with class "vung-doc". Adds its sources to a list of type string.
         * @param url chapter url
         * @return sources of all images of that chapter
         */
        public async Task<List<string>> getImageSources(String url)
        {
            List<string> sources = new List<string>();

            await GetHtmlDocument(url);

            var imgs = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "").Equals("vung-doc")).FirstOrDefault().Descendants("img").ToList();

            foreach (var img in imgs)
            {
                sources.Add(img.GetAttributeValue("src", ""));
            }

            return sources;
        }
    }
}

using HtmlAgilityPack;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaPDF
{
    class Manga
    {

        public string mangaName, mangaUrl, mangaImageSrc;

        public Manga(string mangaName, string mangaUrl, string mangaImageSrc)
        {
            this.mangaName = mangaName;
            this.mangaUrl = mangaUrl;
            this.mangaImageSrc = mangaImageSrc;
        }
    }
}

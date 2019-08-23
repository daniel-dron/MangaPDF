using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MangaPDF
{
    public partial class MangaPDF : Form
    {
        public MangaPDF()
        {
            InitializeComponent();
        }

        MangaSearch mangaSearch;
        List<Manga> mangas;
        List<string> chapterLinks;

        private void MangaPDF_Load(object sender, EventArgs e)
        {
            chapterLinks = new List<string>();

            chapterList.CheckOnClick = true;
            

            mangaListView.View = View.Details;
            mangaListView.Columns.Add("Manga", 150);
            mangaListView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);
            mangaListView.MultiSelect = false;
            mangaListView.Font = new Font("Arial", 14, FontStyle.Bold);
        }

        private async void SearchButton_ClickAsync(object sender, EventArgs e)
        {
            string[] terms = mangaSearchTerm.Text.Split(' ');

            chapterLinks.Clear();
            chapterList.Items.Clear();

            mangaSearch = new MangaSearch();
            mangaSearch.SetURL(terms);
            await mangaSearch.GetHtmlDocument();

            mangas = mangaSearch.GetMangasFromHtml();

            loadMangas();
        }

        private void loadMangas()
        {
            mangaListView.Items.Clear();

            ImageList imgs = new ImageList
            {
                ImageSize = new Size(90, 130)
            };

            //TODO implement image loader
            foreach (Manga manga in mangas)
            {
                var request = WebRequest.Create(manga.mangaImageSrc);

                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    imgs.Images.Add(Image.FromStream(stream));
                }
            }


            mangaListView.SmallImageList = imgs;

            for(int i = 0; i < mangas.Count; i++)
            {
                mangaListView.Items.Add(mangas[i].mangaName, i);
            }
        }

        private async void MangaListView_SelectedIndexChangedAsync(object sender, EventArgs e)
        {
            if(mangaListView.SelectedItems.Count > 0)
            {
                int idx = mangaListView.Items.IndexOf(mangaListView.SelectedItems[0]);

                string mangaUrl = mangas[idx].mangaUrl;

                var divs = await Task.Run(() => mangaSearch.getChapterLinks(mangaUrl));

                chapterList.Items.Clear();
                chapterLinks.Clear();

                foreach (var div in divs)
                {
                    chapterLinks.Add(div.Descendants("a").FirstOrDefault().GetAttributeValue("href", ""));
                    chapterList.Items.Add(div.Descendants("a").FirstOrDefault().InnerText, CheckState.Unchecked);
                }
            }
        }

        private void selectAll(object sender, EventArgs e)
        {
            for (int i = 0; i < chapterList.Items.Count; i++)
            {
                chapterList.SetItemCheckState(i, CheckState.Checked);
            }
        }

        private void deselectAll(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < chapterList.Items.Count; i++)
            {
                chapterList.SetItemCheckState(i, CheckState.Unchecked);
            }
        }

        private void DownloadBtn_Click(object sender, EventArgs e)
        {

        }

        //TODO implement download chapters and PDF generation
    }
}

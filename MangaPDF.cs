using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

        private MangaSearch mangaSearch;
        private List<Manga> mangas;
        private List<string> chapterLinks;
        private List<string> imageSources;
        private string directory;
        private string imageDirectory;
        private int numberOfImages;
        private bool directoryChanged = false;

        private void MangaPDF_Load(object sender, EventArgs e)
        {
            //Sets not resizable
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            chapterLinks = new List<string>();

            chapterList.CheckOnClick = true;

            //Set up for Manga List View
            mangaListView.View = View.Details;
            mangaListView.Columns.Add("Manga", 150);
            mangaListView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);
            mangaListView.MultiSelect = false;
            mangaListView.Font = new System.Drawing.Font("Arial", 14, FontStyle.Bold);
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

        /**
         * Loads all mangas to manga list view
         */
        private void loadMangas()
        {
            mangaListView.Items.Clear();

            ImageList imgs = new ImageList
            {
                ImageSize = new Size(90, 130)
            };

            //Get's all images from its source and stores them into an ImageList.
            foreach (Manga manga in mangas)
            {
                var request = WebRequest.Create(manga.mangaImageSrc);

                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    imgs.Images.Add(System.Drawing.Image.FromStream(stream));
                }
            }

            //Sets mangaListView's images to imgs ImageList
            mangaListView.SmallImageList = imgs;

            for(int i = 0; i < mangas.Count; i++)
            {
                mangaListView.Items.Add(mangas[i].mangaName, i);
            }
        }

        /**
         * Connects to the manga selected and displays and stores all chapters available
         */
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

        /**
         * Downloads all chapters selected to a folder designated by the user
         */
        private async void DownloadBtn_ClickAsync(object sender, EventArgs e)
        {
            if(!directoryChanged)
            {
                MessageBox.Show("Select a destination", "Directory Empty", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) directoryChanged = true;

                directory = folderBrowserDialog1.SelectedPath;
                directoryLabel.Text = directory + "\\" + (pdfNameInput.Text == "" ? "CHOOSE A NAME" : pdfNameInput.Text + ".pdf");

                return;
            }

            imageSources = new List<string>();

            //Create directory for the to be downloaded images
            imageDirectory = directory + "\\" + pdfNameInput.Text;

            if (Directory.Exists(imageDirectory)) await Task.Run(() => Directory.Delete(imageDirectory, true));

            Directory.CreateDirectory(imageDirectory);

            for (int i = 0; i < chapterLinks.Count; i++)
            {
                if (chapterList.GetItemCheckState(i) != CheckState.Checked) continue;

                var temp = await mangaSearch.getImageSources(chapterLinks[i]);

                foreach (string s in temp) imageSources.Add(s);

                numberOfImages = i;
            }

            //Download all images from sources
            await Task.Run(() => downloadSourcesToTempAsync());
        }

        private async void downloadSourcesToTempAsync()
        {
            if (Directory.Exists(imageDirectory)) await Task.Run(() => Directory.Delete(imageDirectory, true));

            Directory.CreateDirectory(imageDirectory);

            //index for naming files in order
            int i = 0;
            foreach (String source in imageSources)
            {
                if (source.StartsWith("Chapter")) continue;

                String path = imageDirectory + "\\" + (i++) + ".jpg";
                long length;
                //download image until length != 0
                //this solves some bugs when it downloads an empty/corrupted image
                do length = await Task.Run(() => downloadImageAsync(source, path));
                while (length == 0);
            }

            numberOfImages = i;

            //Generate PDF file from downloaded images
            generatePDF();

            if (Directory.Exists(imageDirectory)) await Task.Run(() => Directory.Delete(imageDirectory, true));
        }

        //Download image from source to path and return the size
        private async Task<long> downloadImageAsync(String source, String path)
        {
            using (WebClient client = new WebClient())
            {
                await client.DownloadFileTaskAsync(new Uri(source), path);
            }
            return new FileInfo(path).Length;
        }

        private void generatePDF()
        {
            Document document = new Document();
            try
            {
                PdfWriter.GetInstance(document, new FileStream(imageDirectory + ".pdf", FileMode.Create));
                document.Open();
            }
            catch (IOException)
            {
                MessageBox.Show("There is an external software using the current PDF file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            for (int i = 0; i < numberOfImages; i++)
            {

                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imageDirectory + "\\" + i + ".jpg");
                img.SetAbsolutePosition(0, 0);
                document.SetPageSize(new iTextSharp.text.Rectangle(img.Width, img.Height));
                document.NewPage();
                document.Add(img);
            }
            document.Close();
        }

        private void changeFolderBtnClick(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) directoryChanged = true;

            directory = folderBrowserDialog1.SelectedPath;
            directoryLabel.Text = directory + "\\" + (pdfNameInput.Text == "" ? "CHOOSE A NAME" : pdfNameInput.Text + ".pdf");
        }

        private void PdfNameInput_TextChanged(object sender, EventArgs e)
        {
            if(directoryChanged) directoryLabel.Text = directory + "\\" + (pdfNameInput.Text == "" ? "CHOOSE A NAME" : pdfNameInput.Text + ".pdf");
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
    }
}

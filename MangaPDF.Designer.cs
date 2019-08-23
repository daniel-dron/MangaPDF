namespace MangaPDF
{
    partial class MangaPDF
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.searchMangaLabel = new System.Windows.Forms.Label();
            this.mangaSearchTerm = new System.Windows.Forms.TextBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.mangaListView = new System.Windows.Forms.ListView();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.chapterList = new System.Windows.Forms.CheckedListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pdfNameInput = new System.Windows.Forms.TextBox();
            this.downloadBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // searchMangaLabel
            // 
            this.searchMangaLabel.AutoSize = true;
            this.searchMangaLabel.Location = new System.Drawing.Point(13, 13);
            this.searchMangaLabel.Name = "searchMangaLabel";
            this.searchMangaLabel.Size = new System.Drawing.Size(77, 13);
            this.searchMangaLabel.TabIndex = 0;
            this.searchMangaLabel.Text = "Search Manga";
            // 
            // mangaSearchTerm
            // 
            this.mangaSearchTerm.Location = new System.Drawing.Point(16, 30);
            this.mangaSearchTerm.Name = "mangaSearchTerm";
            this.mangaSearchTerm.Size = new System.Drawing.Size(382, 20);
            this.mangaSearchTerm.TabIndex = 1;
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(428, 28);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(75, 23);
            this.searchButton.TabIndex = 2;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.SearchButton_ClickAsync);
            // 
            // mangaListView
            // 
            this.mangaListView.Location = new System.Drawing.Point(16, 57);
            this.mangaListView.Name = "mangaListView";
            this.mangaListView.Size = new System.Drawing.Size(487, 560);
            this.mangaListView.TabIndex = 3;
            this.mangaListView.UseCompatibleStateImageBehavior = false;
            this.mangaListView.SelectedIndexChanged += new System.EventHandler(this.MangaListView_SelectedIndexChangedAsync);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(874, 28);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 18;
            this.button3.Text = "Deselect All";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.MouseClick += new System.Windows.Forms.MouseEventHandler(this.deselectAll);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(793, 28);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 17;
            this.button2.Text = "Select All";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.selectAll);
            // 
            // chapterList
            // 
            this.chapterList.FormattingEnabled = true;
            this.chapterList.Location = new System.Drawing.Point(564, 57);
            this.chapterList.Name = "chapterList";
            this.chapterList.Size = new System.Drawing.Size(385, 484);
            this.chapterList.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(561, 551);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(166, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "PDF Name (ex.: One-Punch-Man)";
            // 
            // pdfNameInput
            // 
            this.pdfNameInput.Location = new System.Drawing.Point(562, 567);
            this.pdfNameInput.Name = "pdfNameInput";
            this.pdfNameInput.Size = new System.Drawing.Size(279, 20);
            this.pdfNameInput.TabIndex = 14;
            // 
            // downloadBtn
            // 
            this.downloadBtn.Location = new System.Drawing.Point(562, 593);
            this.downloadBtn.Name = "downloadBtn";
            this.downloadBtn.Size = new System.Drawing.Size(150, 23);
            this.downloadBtn.TabIndex = 13;
            this.downloadBtn.Text = "Download Chapters";
            this.downloadBtn.UseVisualStyleBackColor = true;
            this.downloadBtn.Click += new System.EventHandler(this.DownloadBtn_Click);
            // 
            // MangaPDF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(977, 637);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.chapterList);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pdfNameInput);
            this.Controls.Add(this.downloadBtn);
            this.Controls.Add(this.mangaListView);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.mangaSearchTerm);
            this.Controls.Add(this.searchMangaLabel);
            this.Name = "MangaPDF";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MangaPDF_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label searchMangaLabel;
        private System.Windows.Forms.TextBox mangaSearchTerm;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.ListView mangaListView;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckedListBox chapterList;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox pdfNameInput;
        private System.Windows.Forms.Button downloadBtn;
    }
}
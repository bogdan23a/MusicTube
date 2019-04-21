using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;


namespace MusicTube
{
    public class SearchResultObject
    {
        private readonly int id;
        private int currentLeftPosition;
        private int currentTopPosition;

        private int thumbnailWidth = 175;
        private int thumbnailHeight = 175;
        private int downloadButtonWidth = 75;
        private int downloadButtonHeight = 20;
        private int paddingThumbnail = 15;
        private int paddingLeft = 75;
        private int paddintTop = 50;
        public static int videosPerRow = 2;
        string subtractID;
        public Button downloadButton;
        public Image thumbnail;
        public TextBlock titleLabel;
        public SearchResultObject()
        {

        }

        private void computeLeft(int ID)
        {
            switch (videosPerRow) {

                case 2: paddingLeft = 50;
                    break;
                case 3: paddingLeft = 50;
                    break;
                case 5: paddingLeft = 75;
                    break;
            }
            currentLeftPosition = (thumbnailWidth * (ID % videosPerRow)) + (ID % videosPerRow) * paddingLeft + paddingLeft;
        }
        private void computeTop(int ID)
        {
            currentTopPosition = (thumbnailHeight * (ID / videosPerRow)) + (ID / videosPerRow) * paddintTop + paddintTop;
        }

       
        public SearchResultObject(int ID, string title)   
        {
            computeLeft(ID);
            computeTop(ID);
            this.id = ID;
            subtractID = title.Split('(')[title.Split('(').Length - 1].Substring(0, title.Split('(')[title.Split('(').Length - 1].Length - 1);

            thumbnail = new CustomThumbnail(
                currentLeftPosition,
                currentTopPosition,
                thumbnailWidth,
                thumbnailHeight,
                true,
                false,
                subtractID
                );

            downloadButton = new CustomButton(
                "Save",
                currentLeftPosition + (thumbnailWidth - downloadButtonWidth), 
                currentTopPosition + thumbnailHeight + paddingThumbnail,
                downloadButtonWidth,
                downloadButtonHeight,
                new SolidColorBrush(Color.FromArgb(255, 0, 198, 169)),
                new SolidColorBrush(Color.FromArgb(255, 0, 137, 111)),
                title
                );

            //downloadButton.Click += async (o, e) =>
            //{
            //    await YoutubeApi.saveVideoToDiskAsync("https://www.youtube.com/watch?v=" + subtractID);
            //};
            downloadButton.Click += new RoutedEventHandler(DownloadButton_Click);
            
            titleLabel = new CustomLabel(
                title,
                currentLeftPosition,
                currentTopPosition + thumbnailHeight + paddingThumbnail,
                thumbnailWidth - downloadButtonWidth - 10,
                title
                );


        }

        private void DownloadButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    MainWindow.videoAndPath.Add(subtractID, dialog.SelectedPath);
                    MainWindow.downloadLinksLabels[MainWindow.currentLinksSaved].Text = ((CustomButton)sender).ToolTip.ToString().Substring(32);
                    ToolTip tt = new ToolTip();
                    tt.Content = dialog.SelectedPath;
                    MainWindow.downloadLinksLabels[MainWindow.currentLinksSaved].ToolTip = tt;
                }
            }
            MainWindow.currentLinksSaved++;

        }
    }
}

using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace MusicTube
{
    public class CustomThumbnail : Image
    {
        public CustomThumbnail()
        {

        }
        private BitmapImage setSource(string link)
        {
            BitmapImage img = new BitmapImage();
            int BytesToRead = 100;
            //thumbnail setup with youtube link
            WebRequest request = WebRequest.Create(new Uri(@"https://img.youtube.com/vi/" + link + "/mqdefault.jpg", UriKind.Absolute));
            request.Timeout = -1;
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            BinaryReader reader = new BinaryReader(responseStream);
            MemoryStream memoryStream = new MemoryStream();

            byte[] bytebuffer = new byte[BytesToRead];
            int bytesRead = reader.Read(bytebuffer, 0, BytesToRead);

            while (bytesRead > 0)
            {
                memoryStream.Write(bytebuffer, 0, bytesRead);
                bytesRead = reader.Read(bytebuffer, 0, BytesToRead);
            }

            img.BeginInit();
            memoryStream.Seek(0, SeekOrigin.Begin);

            img.StreamSource = memoryStream;
            img.EndInit();

           return img;
        }
        public CustomThumbnail(int posX, int posY, string link)
        {
            this.Margin = new Thickness(posX, posY, 0, 0);
            this.Source = setSource(link);
        }
        public CustomThumbnail(int posX, int posY, int width, int height, string link)
        {
            this.Width = width;
            this.Height = height;
            this.Margin = new Thickness(posX, posY, 0, 0);
            this.Source = setSource(link);
        }
        public CustomThumbnail(int posX, int posY, int width, int height, string toolTip, string link)
        {
            this.Width = width;
            this.Height = height;
            ToolTip tt = new ToolTip();
            tt.Content = toolTip;
            this.ToolTip = tt;
            this.Margin = new Thickness(posX, posY, 0, 0);
            this.Source = setSource(link);
        }
        public CustomThumbnail(double posX, double posY, int width, int height, bool blur, bool dropShadow, string link)
        {
            this.Width = width;
            this.Height = height;
            this.Margin = new Thickness(posX, posY, 0, 0);
            if (blur)
            {
                this.MouseEnter += mouseEnterBlur;
                this.MouseLeave += mouseLeaveBlur;
            }
            if(dropShadow)
            {
                this.MouseEnter += mouseEnterShadow;
                this.MouseLeave += mouseLeaveShadow;
            }
            this.Source = setSource(link);
        }

        private void mouseLeaveBlur(object sender, MouseEventArgs e)
        {
            BlurEffect effect = new BlurEffect();
            effect.Radius = 0;
            this.Effect = effect;
        }

        private void mouseEnterBlur(object sender, MouseEventArgs e)
        {
            BlurEffect effect = new BlurEffect();
            effect.Radius = 5;
            this.Effect = effect;
        }
        private void mouseLeaveShadow(object sender, MouseEventArgs e)
        {
            DropShadowEffect effect = new DropShadowEffect();
            effect.ShadowDepth = 0;
            this.Effect = effect;
        }

        private void mouseEnterShadow(object sender, MouseEventArgs e)
        {
            DropShadowEffect effect = new DropShadowEffect();
            effect.ShadowDepth = 5;
            effect.BlurRadius = 5;
            this.Effect = effect;
        }


        /*public CustomThumbnail(int posX, int posY, int width, string toolTip, Brush color)
        {
            this.Width = width;
            this.Height = height;
            this.Margin = new Thickness(posX, posY, 0, 0);
            if (blur)
            {
                this.MouseEnter += mouseEnter;
                this.MouseLeave += mouseLeave;
            }
            this.Source = setSource(link);
        }*/
    }
}


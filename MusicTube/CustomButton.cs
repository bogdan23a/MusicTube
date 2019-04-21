using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace MusicTube
{
    public class CustomButton : Button
    {
        public static string titleOfVideo;
       
        public CustomButton()
        {
            
        }
        public CustomButton(string buttonName)
        {
            this.Content = buttonName;
        }
        public CustomButton(string buttonName, int posX, int posY)
        {
            this.Content = buttonName;
            this.Margin = new Thickness(posX, posY, 0, 0);
        }
        public CustomButton(string buttonName, int posX, int posY, int width, int height)
        {
            this.Content = buttonName;
            this.Width = width;
            this.Height = height;
            this.Margin = new Thickness(posX, posY, 0, 0);
        }
        public CustomButton(string buttonName, int posX, int posY, int width, int height, Brush background)
        {
            this.Content = buttonName;
            this.Width = width;
            this.Height = height;
            this.Background = background;
            this.Margin = new Thickness(posX, posY, 0, 0);
        }
        public CustomButton(string buttonName, int posX, int posY, int width, int height, GradientBrush background)
        {
            this.Content = buttonName;
            this.Width = width;
            this.Height = height;
            this.Background = background;
            this.Margin = new Thickness(posX, posY, 0, 0);
        }
        public CustomButton(string buttonName, double posX, double posY, double width, double height, Brush background, Brush border, string toolTip)
        {
            this.Content = buttonName;
            this.Width = width;
            this.Height = height;
            this.Background = background;
            this.BorderBrush = border;
            this.Margin = new Thickness(posX, posY, 0, 0);
            ToolTip tt = new ToolTip();
            tt.Content = toolTip;
            this.ToolTip = tt;
        }
    }
}

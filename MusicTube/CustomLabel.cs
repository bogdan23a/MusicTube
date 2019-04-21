using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MusicTube
{
    public class CustomLabel : TextBlock
    {
        public CustomLabel()
        {

        }
        public CustomLabel(string labelName)
        {
            this.Text = labelName;
        }
        public CustomLabel(string labelName, int posX, int posY)
        {
            this.Text = labelName;
            this.Margin = new Thickness(posX, posY, 0, 0);
        }
        public CustomLabel(string labelName, int posX, int posY, int width)
        {
            this.Text = labelName;
            this.Width = width;
            this.Margin = new Thickness(posX, posY, 0, 0);
        }
        public CustomLabel(string labelName, double posX, double posY, double width, string toolTip)
        {
            this.Text = labelName;
            this.Width = width;
            ToolTip tt = new ToolTip();
            tt.Content = toolTip;
            this.ToolTip = tt;
            this.Margin = new Thickness(posX, posY, 0, 0);
        }
        public CustomLabel(string labelName, int posX, int posY, int width, string toolTip, Brush color)
        {
            this.Text = labelName;
            this.Width = width;
            ToolTip tt = new ToolTip();
            tt.Content = toolTip;
            this.ToolTip = tt;
            this.Margin = new Thickness(posX, posY, 0, 0);
            this.Foreground = color;
        }
        public CustomLabel(string labelName, int posX, int posY, int width, string toolTip, Brush color, Brush background)
        {
            this.Text = labelName;
            this.Width = width;
            ToolTip tt = new ToolTip();
            tt.Content = toolTip;
            this.ToolTip = tt;
            this.Margin = new Thickness(posX, posY, 0, 0);
            this.Foreground = color;
            this.Background = background;
        }
    }
}

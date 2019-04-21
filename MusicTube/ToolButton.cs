using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MusicTube
{
    public class ToolButton
    {

        public static Ellipse Create(double posX, double posY, int width, int height, Color color)
        {
            Ellipse ellipse = new Ellipse();
            SolidColorBrush closeColor = new SolidColorBrush();
            closeColor.Color = color;
            ellipse.Fill = closeColor;
            ellipse.StrokeThickness = 1;
            ellipse.Stroke = Brushes.Black;

            ellipse.Width = width;
            ellipse.Height = height;

            ellipse.Margin = new Thickness(posX, posY, 0, 0);

            ellipse.MouseEnter += Ellipse_MouseEnter;
            ellipse.MouseLeave += Ellipse_MouseLeave;

            return ellipse;
        }

        private static void Ellipse_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Ellipse ellipse = (Ellipse)sender;
            SolidColorBrush getColor = (SolidColorBrush)ellipse.Fill;
            SolidColorBrush newColor = new SolidColorBrush();
            newColor.Color = Color.FromArgb(getColor.Color.A,  (byte)(getColor.Color.R + 100), (byte)(getColor.Color.G + 30), (byte)(getColor.Color.B + 20));
            ellipse.Fill = newColor;
            ellipse.StrokeThickness = 1;
            ellipse.Stroke = Brushes.Black;
        }

        private static void Ellipse_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
           Ellipse ellipse = (Ellipse)sender;
            SolidColorBrush getColor = (SolidColorBrush)ellipse.Fill;
            SolidColorBrush newColor = new SolidColorBrush();
            newColor.Color = Color.FromArgb(getColor.Color.A,  (byte)(getColor.Color.R - 100), (byte)(getColor.Color.G - 30), (byte)(getColor.Color.B - 20));
            ellipse.Fill = newColor;
            ellipse.StrokeThickness = 1;
            ellipse.Stroke = Brushes.Black;
        }
    }
}

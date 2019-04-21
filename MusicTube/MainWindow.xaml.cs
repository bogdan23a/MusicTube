using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MusicTube
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //VARIABLE DECLARATION

        const int DOWNLOAD_AT_ONCE_CAPACITY = 100;

        /* 
         * Static variables used in other clases in order to
         * get or set information
         */

        /* 
         * Gets link and path for each video to download 
         * Information is gathered when user clicks the 
         * Save button in each SearchResultObject
         */
        public static Dictionary<String, String> videoAndPath = new Dictionary<string, string>();
        /* 
         * Preinstantiated in the load of the window
         * This is updated when a user clicks the Save
         * button in each SearchResultObject 
         * Used to display to the user the videos that
         * he wants to download 
         */
        public static List<TextBlock> downloadLinksLabels;
        /*
         * Used to reference the list of downloaded
         * videos
         * Main Window keeps the value but it is actually
         * updated when you click the Save button in each
         * SearchResultObject
         */
        public static int currentLinksSaved;


        /* 
         * This list holds the currently displayed videos
         */
        List<SearchResultObject> videoOnScreenList = new List<SearchResultObject>();


        /*
         *Variables used for utilities accross the 
         * display of the window
         */
        TextBox searchBar;
        Storyboard story;
        CustomButton downloadAll;
        Ellipse closeButton;
        Ellipse windowButton;
        Ellipse settingsButton;
        TextBlock downloadListLabel;
        public MainWindow()
        {
            InitializeComponent();
        }

        /* 
         * Displays given video on canvas and adds it to the list
         */
        private void displayOneVideo(SearchResultObject videoToDisplay)
        {
            canvas.Children.Add(videoToDisplay.downloadButton);
            canvas.Children.Add(videoToDisplay.thumbnail);
            canvas.Children.Add(videoToDisplay.titleLabel);
            videoOnScreenList.Add(videoToDisplay);
        }

        /* 
         * Clears the videos from the screen in order to be able to 
         * display another batch of videos on canvas later
         */
        private void cleanCanvas()
        {
            foreach(SearchResultObject item in videoOnScreenList)
            {
                canvas.Children.Remove(item.downloadButton);
                canvas.Children.Remove(item.thumbnail);
                canvas.Children.Remove(item.titleLabel);
            }
        }

        // Prerequisites 
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            /* 
             * Gradient color used on the background of canvas
             */
            GradientStopCollection gradient = new GradientStopCollection();
            gradient.Add(new GradientStop(Color.FromArgb(255, 15, 32, 39), 0));
            gradient.Add(new GradientStop(Color.FromArgb(255, 32, 58, 67), 0));
            gradient.Add(new GradientStop(Color.FromArgb(255, 44, 83, 100), 0));
            canvas.Background = new LinearGradientBrush(gradient);
            
            /*
             * Both animations widen the window in order to 
             * make it visible
             */
            story = new Animation(
                1,
                500,
                TimeSpan.FromSeconds(0.8),
                1,
                new CubicEase(),
                new PropertyPath(MainWindow.WidthProperty),
                this                
                );

            story = new Animation(
                Left,
                Left - 250,
                TimeSpan.FromSeconds(0.8),
                1,
                new CubicEase(),
                new PropertyPath(MainWindow.LeftProperty),
                this,
                true
                );

            story.Completed += Story_Completed;
            story.Begin(this);


            /* 
             * Initialize the header of the download list
             * It is only visible after the user has pressed
             * the Enter key on the keyboard and on the 
             * medium size of the window
             */
            downloadListLabel = new CustomLabel(
                "Downloads: ",
                150,
                500,
                100,
                "The list of saved links displayed below"
                );

            /*
             * Initializing all the labels in the beginning 
             * and setting all of the to null string 
             * They will be changed ine by one when the user
             * presses the Save button in each SearchResltObject
             */
            downloadLinksLabels = new List<TextBlock>();
            for (int i = 0; i < DOWNLOAD_AT_ONCE_CAPACITY; i++)
            {
                downloadLinksLabels.Add(new CustomLabel(
                   "",
                   180,
                   520 + i * 20,
                   500,
                   ""
                   ));
                canvas.Children.Add(downloadLinksLabels[i]);

            }
        }

        //Happens when the download button is finnaly pressed
        private void DownloadAll_Click(object sender, RoutedEventArgs e)
        {
            //Loop trough all the videos that the user saved
            foreach (KeyValuePair<String, String> pair in videoAndPath)
            {
                /*
                 * For each video to download we create a new proccess
                 * and use the saveVideoToDiskAsync method provided by 
                 * the YoutubeApi class to start downloading specified 
                 * video to the specified path the file from Youtube
                 */
                new Task(
                            async () =>
                {
                    String key = pair.Key;
                    String value = pair.Value;
                    YoutubeApi.saveVideoToDiskAsync("https://www.youtube.com/watch?v=" + key, value);
                }
                        ).RunSynchronously();

            }

            /* Clear the list and reset the index after downloading 
             * everything so the proccess can be correctly done again
             */
            videoAndPath.Clear();
            currentLinksSaved = 0;
        }

        // Called after the widening animation is completed
        private void Story_Completed(object sender, EventArgs e)
        {
            // Instantiates the searchbox
            searchBar = new TextBox();
            searchBar.Width = 300;
            searchBar.Margin = new Thickness(Width / 2 - searchBar.Width / 2, Height / 2 , 0, 0);
            searchBar.KeyUp += searchKeyUpEvent;
            searchBar.Name = "searchBar";
            searchBar.Text = "Search for songs ...";
            searchBar.PreviewMouseLeftButtonDown += SearchBar_MouseDown;
            canvas.RegisterName(searchBar.Name, searchBar);
            canvas.Children.Add(searchBar);

            //Instantiates the close button for the window
            closeButton = ToolButton.Create(
                canvas.Width - 50,
                20,
                15,
                15,
                Color.FromArgb(255, 192, 57, 43)
                );

            closeButton.MouseUp += CloseButton_MouseUp;
            canvas.Children.Add(closeButton);


            // Future settings button for the window
            settingsButton = ToolButton.Create(
                canvas.Width - 70,
                20,
                15,
                15,
                Color.FromArgb(255, 241, 196, 15)
                );
            settingsButton.MouseUp += SettingsButton_MouseUp;
            canvas.Children.Add(settingsButton);


            // Resizing button for the window
            windowButton = ToolButton.Create(
               canvas.Width - 90,
               20,
               15,
               15,
               Color.FromArgb(255, 52, 152, 219)
               );
            windowButton.MouseUp += WindowButton_MouseUp; ;
            canvas.Children.Add(windowButton);


            // Download button for the selected videos
            downloadAll = new CustomButton(
                "Download All",
                Width - 200,
                Height - 50,
                100,
                30,
                new SolidColorBrush(Color.FromArgb(255, 0, 198, 169)),
                new SolidColorBrush(Color.FromArgb(255, 0, 137, 111)),
                "Click to begin downloading"
                );
            downloadAll.Click += DownloadAll_Click;
        }

        // Clears the searchbox when user clicks
        private void SearchBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            searchBar.Text = "";
        }

        /*
         * Method used for positioning all the elements 
         * correctly on the canvas when the user has resized
         * the window 
         * The index is used to keep track of which size the 
         * window currently is in
         */
        int indexSizing = 0;
        private void WindowButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            
            if (indexSizing == 0) /// Medium Window Size
            {
                // 4 animations: resize window size from small to medium
                story = new Animation(
                    Width,
                    Width + 200,
                    TimeSpan.FromSeconds(0.5),
                    1,
                    new CubicEase(),
                    new PropertyPath(MainWindow.WidthProperty),
                    this
                    );
                story = new Animation(
                    Left,
                    Left - 100,
                    TimeSpan.FromSeconds(0.5),
                    1,
                    new CubicEase(),
                    new PropertyPath(MainWindow.LeftProperty),
                    this
                    );
                
                story = new Animation(
                    Height,
                    System.Windows.SystemParameters.PrimaryScreenHeight,
                    TimeSpan.FromSeconds(0.9),
                    1,
                    new SineEase(),
                    new PropertyPath(MainWindow.HeightProperty),
                    this
                    );
                story = new Animation(
                    Top,
                    0,
                    TimeSpan.FromSeconds(0.5),
                    1,
                    new CubicEase(),
                    new PropertyPath(MainWindow.TopProperty),
                    this
                    );



                SearchResultObject.videosPerRow = 3;
                Search.numberSearchResult = 6;

                // Position of buttons on screen
                downloadAll.Margin = new Thickness(500, 680, 0, 0);
                windowButton.Margin = new Thickness(600, 20, 0, 0);
                settingsButton.Margin = new Thickness(620, 20, 0, 0);
                closeButton.Margin = new Thickness(640, 20, 0, 0);
                searchBar.Margin = new Thickness(searchBar.Margin.Left + 100, searchBar.Margin.Top, 0, 0);

                indexSizing++;
            }
            else if( indexSizing == 1) /// Big Window Size
            {
                // 2 animations: window size from medium to big
                story = new Animation(
                    Width,
                    System.Windows.SystemParameters.PrimaryScreenWidth,
                    TimeSpan.FromSeconds(0.5),
                    1,
                    new CubicEase(),
                    new PropertyPath(MainWindow.WidthProperty),
                    this
                    );
                story = new Animation(
                    Left,
                    0,
                    TimeSpan.FromSeconds(0.5),
                    1,
                    new CubicEase(),
                    new PropertyPath(MainWindow.LeftProperty),
                    this
                    );


                SearchResultObject.videosPerRow = 5;
                Search.numberSearchResult = 10;

                // Position of buttons on screen
                downloadAll.Margin = new Thickness(1000, 680, 0, 0);
                windowButton.Margin = new Thickness(1200, 20, 0, 0);
                settingsButton.Margin = new Thickness(1220, 20, 0, 0);
                closeButton.Margin = new Thickness(1240, 20, 0, 0);

                searchBar.Margin = new Thickness(searchBar.Margin.Left + 300, searchBar.Margin.Top, 0, 0);

                indexSizing++;
            }
            else /// Small Window Size
            {
                // 4 animations: window size from big to small
                story = new Animation(
                    Width,
                    500,
                    TimeSpan.FromSeconds(0.5),
                    1,
                    new CubicEase(),
                    new PropertyPath(MainWindow.WidthProperty),
                    this
                    );
                story = new Animation(
                    0,
                    Width/2 -250,
                    TimeSpan.FromSeconds(0.5),
                    1,
                    new CubicEase(),
                    new PropertyPath(MainWindow.LeftProperty),
                    this
                    );

                story = new Animation(
                    Height,
                    450,
                    TimeSpan.FromSeconds(0.5),
                    1,
                    new CubicEase(),
                    new PropertyPath(MainWindow.HeightProperty),
                    this
                    );
                story = new Animation(
                    0,
                    System.Windows.SystemParameters.PrimaryScreenHeight/2 - 250,
                    TimeSpan.FromSeconds(0.5),
                    1,
                    new CubicEase(),
                    new PropertyPath(MainWindow.TopProperty),
                    this
                    );

                SearchResultObject.videosPerRow = 2;
                Search.numberSearchResult = 2;

                // Position of buttons on screen
                downloadAll.Margin = new Thickness(320, 400, 0, 0);
                windowButton.Margin = new Thickness(420, 20, 0, 0);
                settingsButton.Margin = new Thickness(440, 20, 0, 0);
                closeButton.Margin = new Thickness(460, 20, 0, 0);
                searchBar.Margin = new Thickness(searchBar.Margin.Left - 400, searchBar.Margin.Top, 0, 0);

                indexSizing = 0;
            }

        }

        // Future settings button
        private void SettingsButton_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        // Functionality of close button for window
        private void CloseButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        /*
         * Method called when the user presses a key
         * Used only when the Enter Key is pressed
         * Index is used to give each SearchResultObject
         * that is created an ID 
         */
        int indexing = 0;
        private void searchKeyUpEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                /*
                 * Checks if there is need to display
                 * the downloads
                 */
                if (indexing == 0)
                {
                    canvas.Children.Add(downloadAll);
                    canvas.Children.Add(downloadListLabel);
                }

                /*
                 * Checks if there have already been videos
                 * displayed to clear the canvas and reset 
                 * the index
                 * If not, the animation for lifting the searchbar 
                 * is played
                 */
                if (videoOnScreenList.Count == 0)
                    story = new Animation(
                        searchBar.Margin.Top / 2 - 150,
                        -searchBar.Margin.Top + 20,
                        TimeSpan.FromSeconds(0.5),
                        1,
                        new CubicEase(),
                        new PropertyPath(TopProperty),
                        searchBar
                        );
                else
                {
                    cleanCanvas();
                    indexing = 0;
                }

                /* Creates a list to save the videos that the 
                 * Youtube Api finds with the user input and then 
                 * for each video found it displays it
                 */
                List<string> videos = new List<string>();
                videos = YoutubeApi.searchForVideo(searchBar.Text);

                foreach (string searchResult in videos)
                    displayOneVideo(new SearchResultObject(indexing++, searchResult));
            }

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            canvas.Height = e.NewSize.Height;
            canvas.Width = e.NewSize.Width;

        }
    }
}

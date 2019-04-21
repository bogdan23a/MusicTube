using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoLibrary;
using System.IO;
using Google.Apis.YouTube.v3;
using Google.Apis.Services;
using MediaToolkit.Model;
using MediaToolkit;
using System.Windows;

namespace MusicTube
{
    internal class YoutubeApi
    {
        
        public static async Task saveVideoToDiskAsync(string link, string folder)
        {
            try
            {
                folder = folder + "/";
                var youtube = YouTube.Default;
                var video = youtube.GetVideo(link);
                File.WriteAllBytes(folder + video.FullName, 
                    await video.GetBytesAsync());

                var inputFile = new MediaFile { Filename = folder + video.FullName };
                //var outputFile = new MediaFile { Filename = $"{folder + video.Title}.wav" };
                //MessageBox.Show("maai");
                //using (var engine = new Engine())
                //{
                //    engine.GetMetadata(inputFile);
                //    engine.Convert(inputFile, outputFile);
                //}

                //File.Delete(inputFile.Filename);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        
        public static List<string> searchForVideo(string searchString)
        {
            List<string> list = new List<string>();
            list = Search.run(searchString);

            return list;
        }
    }
   public class Search
    {

        private static List<string> video;
        private const string API_KEY = "AIzaSyApeYOtQSZAnLpDcjO0xPHorlG8Zj3CXnk";
        public static int numberSearchResult = 2;
        [STAThread]
        public static List<string> run(string searchString)
        {
            video = new List<string>();
            try
            {
                new Search().Run(searchString).Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }

            return video;
        }

        private async Task Run(string searchString)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = API_KEY,
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");

            searchListRequest.Q = searchString;
            searchListRequest.MaxResults = numberSearchResult;

            var searchListResponse = searchListRequest.Execute();


            foreach (var searchResult in searchListResponse.Items)
                if (searchResult.Id.Kind == "youtube#video")
                    video.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.VideoId));
        }
    }
}

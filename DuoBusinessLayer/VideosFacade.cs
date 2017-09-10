using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.QuickTime;

namespace DuoBusinessLayer
{
    public class VideosFacade
    {
        private IList<string> _paths;
        private DateTime _birthday;
        public VideosFacade(IList<string> paths)
        {
            _paths = paths;
            string date = "2016-09-19";
            _birthday = DateTime.Parse(date);
        }

        public IList<VideosViewModel> GetVideosDetails()
        {
            IList<VideosViewModel> videosModels = new List<VideosViewModel>();
            var allFiles = GetVediosFiles();
            foreach (var file in allFiles)
            {
                var metadata = ExtractMetadata(file);
                var lala = Convert.ToInt64(metadata["File Size"].Split(' ')[0]);
                string format = "ddd MMM dd HH:mm:ss yyyy";
                DateTime date;
                bool result = DateTime.TryParseExact(metadata["Created"], format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
                var name = Path.GetFileNameWithoutExtension(file);
                var extension = Path.GetExtension(file);
                var videoViewModel = new VideosViewModel
                {
                    Name = name,
                    Extension = extension,
                    FileSize = lala,
                    CreateTime = date,
                    Duration = Convert.ToInt64(metadata["Duration"]),
                    Age = (date - _birthday).TotalDays,
                    Src = "http://localhost:50593/api/Videos/"+ name + "/" + extension.Substring(1)
                };
                videosModels.Add(videoViewModel);
            }
            return videosModels;
        }

        public IDictionary<string,string> ExtractMetadata(string file)
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            try
            {
                var directories = ImageMetadataReader.ReadMetadata(file);
                var fileName = Path.GetFileNameWithoutExtension(file);
                foreach (var directory in directories)
                {
                    if ("QuickTime Track Header".ToLower().Equals(directory.Name.ToString().ToLower()) || "File".ToLower().Equals(directory.Name.ToString().ToLower()))
                    {
                        foreach (var tag in directory.Tags)
                        {
                            if (!dictionary.ContainsKey(tag.Name) && ("Duration".ToLower().Equals(tag.Name.ToLower()) || "Created".ToLower().Equals(tag.Name.ToLower()) || "File Size".ToLower().Equals(tag.Name.ToLower())))
                            {
                                dictionary.Add(tag.Name, tag.Description);
                            }
                        }
                        if (directory.HasError)
                        {
                            foreach (var error in directory.Errors) Console.WriteLine($"ERROR: {error}");
                        }
                    }
                }
            }
            catch (IOException)
            {
                //
            }
           // File.WriteAllLines(@"C:\Project\" + fileName, list);       
            return dictionary;
        }

        private IList<string> GetVediosFiles()
        {
            IList<string> files = new List<string>();
            foreach (var path in _paths)
            {
                
                var listedFiles = System.IO.Directory.GetFiles(path);
                foreach (var item in listedFiles)
                {
                    files.Add(item);
                }
            }
            return files;
        }
    }
}

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;

namespace DuoBusinessLayer
{
    public class VideoStream
    {
        private readonly string BASE_DIRECTORY = @"C:\Project\videodata\";
        private readonly string _filename;

        public VideoStream(string filename, string ext)
        {
            _filename = BASE_DIRECTORY + filename + "." + ext;
        }
        public async void WriteToStream(Stream outputStream, HttpContent content, TransportContext context)
        {
            try
            {
                var buffer = new byte[65536];

                using (var video = File.Open(_filename, FileMode.Open, FileAccess.Read))
                {
                    var length = (int)video.Length;
                    var bytesRead = 1;

                    while (length > 0 && bytesRead > 0)
                    {
                        bytesRead = video.Read(buffer, 0, Math.Min(length, buffer.Length));
                        await outputStream.WriteAsync(buffer, 0, bytesRead);
                        length -= bytesRead;
                    }
                }
            }
            catch (HttpException ex)
            {
                return;
            }
            finally
            {
                outputStream.Close();
            }
        }
    }
}

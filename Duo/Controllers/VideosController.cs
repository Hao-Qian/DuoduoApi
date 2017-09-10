using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using DuoBusinessLayer;

namespace Duo.Controllers
{
    public class VideosController : ApiController
    {
        public HttpResponseMessage Get(string filename, string ext)
        {
            var video = new VideoStream(filename, ext);
            var response = Request.CreateResponse();
            Action<Stream, HttpContent, TransportContext> action = video.WriteToStream;
            response.Content = new PushStreamContent(action, new MediaTypeHeaderValue("video/" + ext));
            return response;
        }

        public HttpResponseMessage Get()
        {
            var path = new List<string>()
            {
                @"C:\Project\videodata"
            };
            var facade = new VideosFacade(path);    
            return Request.CreateResponse(HttpStatusCode.OK, facade.GetVideosDetails());
        }
    }
}

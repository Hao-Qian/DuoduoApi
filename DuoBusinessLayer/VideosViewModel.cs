using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoBusinessLayer
{
    public class VideosViewModel
    {
        public string Name { set; get; }
        public string Extension { set; get; }
        public long Duration { set; get; }
        public long FileSize { get; set; }
        public string Creator { set; get; }
        public DateTime CreateTime { set; get; }
        public DateTime ModifiedTime { set; get; }
        public string Src { set; get; }
        public double Age { set; get; }
    }
}

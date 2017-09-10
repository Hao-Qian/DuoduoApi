using System;
using System.Collections.Generic;
using MetadataExtractor;

namespace DuoBusinessLayer
{
    public class Metadata
    {
        public List<string> ExtractMetadata(string file)
        {
            List<string> list = new List<string>();
            var directories = ImageMetadataReader.ReadMetadata(file);

            foreach (var directory in directories)
            {
                foreach (var tag in directory.Tags)
                {
                    list.Add($"[{directory.Name}] {tag.Name} = {tag.Description}");
                }

                if (directory.HasError)
                {
                    foreach (var error in directory.Errors) Console.WriteLine($"ERROR: {error}");
                }
            }
            return list;
        }
    }
}

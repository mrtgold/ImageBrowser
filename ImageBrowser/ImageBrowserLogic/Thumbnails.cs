using System.Collections.Generic;
using System.Linq;

namespace ImageBrowserLogic
{
    public class Thumbnails : List<Thumbnail>
    {
        public Thumbnails()
        {
            
        }

        private Thumbnails(IEnumerable<Thumbnail> thumbnails)
            :base(thumbnails)
        {
        }

        public Thumbnails GetFromDir(string directory)
        {
           return new Thumbnails( this.Where(t => t.ParentDir == directory));
        }
    }
}
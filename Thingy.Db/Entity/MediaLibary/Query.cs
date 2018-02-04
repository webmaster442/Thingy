using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.Db.Entity.MediaLibary
{
    public class SongQuery
    {
		public StringQuery Artist { get; set; }
        public StringQuery Title { get; set; }
        public StringQuery Album { get; set; }
		public StringQuery Genre { get; set; }
		public IntQuery Year { get; set; }
		public bool? Liked { get; set; }

		public SongQuery()
        {
            Artist = new StringQuery();
            Title = new StringQuery();
            Album = new StringQuery();
            Genre = new StringQuery();
            Year = new IntQuery();
        }
    }
}

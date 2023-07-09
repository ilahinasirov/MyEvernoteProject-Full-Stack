using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities
{
    [Table("Notes")]
    public class Note : MyEntityBase
    {
        [DisplayName("Note Title"), Required,StringLength(70)]
        public string Title { get; set; }


        [DisplayName("Note Text"), Required, StringLength(2000)]
        public string Text { get; set; }

        [DisplayName("Draft")]
        public bool IsDraft { get; set; }


        [DisplayName("Likes")]
        public int LikeCount { get; set; }


        [DisplayName("Category")]
        public int CategoryId { get; set; }

        public virtual EvernoteUser Owner { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<Liked> Likes { get; set; }


        public Note()
        {
            Comments = new List<Comment>();
            Likes = new List<Liked>();
        }

    }
}

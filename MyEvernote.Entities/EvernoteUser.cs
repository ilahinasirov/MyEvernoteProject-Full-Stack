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
    [Table("EvernoteUsers")]

    public class EvernoteUser : MyEntityBase
    {
        [DisplayName("First Name"), StringLength(25)]
        public string Name { get; set; }


        [DisplayName("Last Name"), StringLength(25)]
        public string SurName { get; set; }


        [DisplayName("Userame"), StringLength(25)]
        public string UserName { get; set; }



        [Required, StringLength(70)]
        public string Email { get; set; }


        [Required, StringLength(25)]
        public string Password { get; set; }


        [StringLength(75), ScaffoldColumn(false)]
        public string ProfileImageFileName { get; set; }

        public bool IsActive { get; set; }


        [Required, ScaffoldColumn(false)]
        public Guid ActivateGuid { get; set; }
        public bool IsAdmin { get; set; }

        public virtual List<Note> Notes { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }




    }
}

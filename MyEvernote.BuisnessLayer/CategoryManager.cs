using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEvernote.BuisnessLayer.Abstract;
using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;

namespace MyEvernote.BuisnessLayer
{
    public class CategoryManager : ManagerBase<Category>
    {
        //public override int Delete(Category category)
        //{ //Categorynin silinmesi
        //    NoteManager noteManager = new NoteManager();
        //    LikedManager likedManager = new LikedManager();
        //    CommentManager commentManager = new CommentManager();
        //    // ilk olaraq Category ilə bağlı notlar silinməlidir. yoxsa category silinmez
        //    foreach (Note note in category.Notes.ToList())
        //    {

        //        //Notla bağlı like-lar silinməlidi

        //        foreach (Liked like in note.Likes.ToList())
        //        {
        //            likedManager.Delete(like);
        //        }

        //        //Notla bağlı commennt-ler silinməlidi
        //        foreach (Comment comment in note.Comments.ToList())
        //        {
        //            commentManager.Delete(comment);
        //        }

        //        noteManager.Delete(note);
        //    }

        //    return base.Delete(category);
        //}
    }
}

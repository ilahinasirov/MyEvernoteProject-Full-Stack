

// Bütün klassların,tabloların (List<Category>,List<Note>,List<Comment>) Repositorilərini
// ayrı-ayrılıqda new-ləyəndə hər biri üçün 
// private Databasecontext db= new DatabaseContext(); ayrica new-lənməsin deyə. Bunu bir dəfə yaradırıq
// və hər birində bundan istifadə edirik. Buna **SİNGLETON PATTERN** deyilir.
// Bu klası bunun üçün yaradırıq və aşağıdakı şəkildə yazırıq
namespace MyEvernote.DataAccessLayer.EntityFramework
{
    public class RepositoryBase
    {
        protected private static DatabaseContext context;
        private static object _lockSync = new object(); //aşağıdakı lock metodu üçün

        protected RepositoryBase() // hər dəfə new-lənməsin deyə protected bir constructor yaradırıq
        {
            CreateContext();
        }

        private static void CreateContext() // new-leye billmədiyimiz üçün static veririk. Çünki
                                            // statik metodlar new-lənmədən işləyir. məsələn biz
                                            // işlədəcəyimiz klasda yuxarida (private DatabaseContext db;) yazıb
                                            //  hardasa birbaşa context= RepositoryBase.CreateContext(); yazıb
                                            // işlədə bilərik
        {
            if (context == null)
            {
                //lock metodu eyni anda birdən çox element yaradan proyektlərdə if-in
                //içərisində eyni zamanda 2 dəfə və daha çox new-lənmə eləməsin deyə istifadə olunur
                //ona görədə if-də elementi yaradan kodu lock-un içərisinə alırıq

                lock (_lockSync)
                {
                    if (context == null)
                    {
                        context = new DatabaseContext();

                    }

                }
            }

        }
    }
}

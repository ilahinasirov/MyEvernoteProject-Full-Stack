using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using MyEvernote.BuisnessLayer;
using MyEvernote.BuisnessLayer.Results;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ValueObjects;
using MyEvernote.Filters;
using MyEvernote.Models;
using MyEvernote.ViewModels;

namespace MyEvernote.Controllers
{
    [Exc]
    public class HomeController : Controller
    {
        private NoteManager noteManager = new NoteManager();
        private CategoryManager categoryManager = new CategoryManager();
        private UserManager userManager = new UserManager();


        [AllowAnonymous]
        public ActionResult Index()
        {
            //object o = 0;
            //int a = 1;
            //int c = a / (int)o;

            //throw new Exception("An Error Occurred!");

            //if (TempData["mm"]!=null)
            //{
            //    return View(TempData["mm"] as List<Note>); //Category Controllerden gelen View telebi ve model, Tempdata ile category listelesek 
            //}

            return View(noteManager.QueryableList().OrderByDescending(x => x.ModifiedOn).ToList());
            //return View(noteManager.GetAllNoteQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
        }


        public ActionResult ByCategory(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category cat = categoryManager.Find(x => x.Id == id.Value); //id nullable oldugu uchun id.value yaziriq

            if (cat == null)
            {
                return HttpNotFound();
                //return RedirectToAction("Index", "Home");
            }


            return View("Index", cat.Notes.OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult MostLiked()
        {
            return View("Index", noteManager.QueryableList().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult About()
        {
            return View();
        }


        [Auth]
        public ActionResult ShowProfile()
        {
            BuisnessLayerResult<EvernoteUser> res = userManager.GetUserById(CurrentSession.User.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Error..",
                    RedirectingUrl = "/Home/Login",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);
            }

            return View(res.Result);
        }

        [Auth]
        public ActionResult EditProfile()
        {
            BuisnessLayerResult<EvernoteUser> res = userManager.GetUserById(CurrentSession.User.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Error..",
                    RedirectingUrl = "/Home/Login"

                };

                return View("Error", errorNotifyObj);
            }

            return View(res.Result);
        }

        [Auth]
        [HttpPost]
        /*public ActionResult EditProfile(EvernoteUser model, HttpPostedFileBase ProfileImage)*/ // profili edit edəndə şəkil əlavə edə bilmək üçün HttpPostedFileBase Profileİmage yazırıq. Profileİmage bizim edit səhifəmizdəki name ilə mütləq eyni olmalıdır.
        public ActionResult EditProfile(EvernoteUser model, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                    (ProfileImage.ContentType == "image/jpeg" ||
                     ProfileImage.ContentType == "image/jpg" ||
                     ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    model.ProfileImageFileName = filename;
                }

                BuisnessLayerResult<EvernoteUser> res = userManager.UpdateProfile(model);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profile Could Not Updated.",
                        RedirectingUrl = "/Home/EditProfile"
                    };

                    return View("Error", errorNotifyObj);
                }

                // Profil güncellendiği için session güncellendi.
                CurrentSession.Set<EvernoteUser>("login", res.Result);

                return RedirectToAction("ShowProfile");
            }

            return View(model);
        }


        [Auth]
        public ActionResult DeleteProfile()

        {
            // Sessiondaki useri getiririk ve user manager uzerinden

            BuisnessLayerResult<EvernoteUser> res = userManager.RemoveUserById(CurrentSession.User.Id); //useri silirik

            if (res.Errors.Count > 0)
            {
                ErrorViewModel messages = new ErrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Profile is Not Deleted",
                    RedirectingUrl = "/Home/EditProfile"
                };

                return View("Error", messages);
            }

            Session.Clear();
            return RedirectToAction("Index");

        }

        
        public ActionResult TestNotify()
        {
            ErrorViewModel model = new ErrorViewModel()
            {
                Header = "Redirecting",
                Title = "Ok Test",
                RedirectingTimeout = 10000,
                Items = new List<ErrorMessageObj>()
                {
                    new ErrorMessageObj() { Message = "Test succesfully 1"},
                    new ErrorMessageObj() { Message = "Test succesfully 2" }

                }

            };

            return View("Error", model);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {


            if (ModelState.IsValid)
            {
                UserManager um = new UserManager();
                BuisnessLayerResult<EvernoteUser> res = um.LoginUser(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    if (res.Errors.Find(x => x.Code == ErrorMessageCodes.UserIsNotActivated) != null)
                    {

                    }
                    return View(model);
                }



                CurrentSession.Set<EvernoteUser>("login", res.Result);       //Sessionda user məlumatlarını saxlamaq
                return RedirectToAction("Index");   //Yönləndirmə

            }



            return View(model);
        }
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(SignUpViewModel model)
        {
            // user,email və şifrə yoxlanışı 
            //Kayıt işlemi
            //Activation Emaili göndərmək

            if (ModelState.IsValid)
            {

                BuisnessLayerResult<EvernoteUser> res = userManager.RegisterUser(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "Success!",
                    RedirectingUrl = "/Home/Login",

                };
                notifyObj.Items.Add("Now You Can Add Notes, Comment and Like Posts");
                return View("Ok", notifyObj);
            }
            return View(model);
        }



        public ActionResult UserActivate(Guid id)
        {
            // Userin Activationu üçün
            BuisnessLayerResult<EvernoteUser> res = userManager.ActivateUser(id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Account is Not Available..",
                    RedirectingUrl = "/Home/Login",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);

                //TempData["errors"]=res.Errors;
                //return View("Error");

            }

            OkViewModel okNotifyObj = new OkViewModel()
            {
                Title = "Your Account is Activated..",
                RedirectingUrl = "/Home/Login",
            };

            okNotifyObj.Items.Add("Now You can add notes and likes.");



            return View("Ok", okNotifyObj);
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }



        [OutputCache(Duration = 1200)] // Cache the output for 20 minute
        public ActionResult Sponsors()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult AccessDenied()
        {
            return View();
        }
        public ActionResult HasError()
        {
            return View();
        }

    }
}
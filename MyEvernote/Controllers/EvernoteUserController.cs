using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyEvernote.BuisnessLayer;
using MyEvernote.BuisnessLayer.Results;
using MyEvernote.Entities;
using MyEvernote.Filters;

namespace MyEvernote.Controllers
{
     [Auth]
     [AuthAdmin]
     [Exc]
    public class EvernoteUserController : Controller
    {
        private UserManager userManager = new UserManager();

        public ActionResult Index()
        {
            return View(userManager.List());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EvernoteUser evernoteUser = userManager.Find(x=>x.Id ==id.Value);
            if (evernoteUser == null)
            {
                return HttpNotFound();
            }
            return View(evernoteUser);
        }

        public ActionResult Create()
        {
            return View();
        }

  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EvernoteUser evernoteUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
              BuisnessLayerResult<EvernoteUser> res = userManager.Insert(evernoteUser);

              if (res.Errors.Count>0)
              {
                  res.Errors.ForEach(x=>ModelState.AddModelError("",x.Message));
                  return View(evernoteUser);
              }
                return RedirectToAction("Index");
            }

            return View(evernoteUser);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EvernoteUser evernoteUser =  userManager.Find(x => x.Id == id.Value);
            if (evernoteUser == null)
            {
                return HttpNotFound();
            }
            return View(evernoteUser);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( EvernoteUser evernoteUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {

                BuisnessLayerResult<EvernoteUser> res = userManager.Update(evernoteUser);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(evernoteUser);
                }
                return RedirectToAction("Index");
            }
            return View(evernoteUser);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EvernoteUser evernoteUser = userManager.Find(x => x.Id == id.Value);
            if (evernoteUser == null)
            {
                return HttpNotFound();
            }
            return View(evernoteUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EvernoteUser evernoteUser = userManager.Find(x => x.Id == id);
            userManager.Delete(evernoteUser);
            return RedirectToAction("Index");
        }

    }
}

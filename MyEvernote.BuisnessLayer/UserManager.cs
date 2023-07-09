using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEvernote.BuisnessLayer.Abstract;
using MyEvernote.BuisnessLayer.Results;
using MyEvernote.Common.Helpers;
using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ValueObjects;

namespace MyEvernote.BuisnessLayer
{
    public class UserManager: ManagerBase<EvernoteUser>

    {
    public BuisnessLayerResult<EvernoteUser> RegisterUser(SignUpViewModel data)
    {
        // user,email və şifrə yoxlanışı 
        //Kayıt işlemi
        //Activation Emaili göndərmək
        EvernoteUser user = Find(x => x.UserName == data.Username || x.Email == data.Email);
        BuisnessLayerResult<EvernoteUser> res = new BuisnessLayerResult<EvernoteUser>();

        if (user != null)
        {
            if (user.UserName == data.Username)
            {
                res.AddError(ErrorMessageCodes.UsernameAlreadyExists, "This UserName is Already Exists!");
            }

            if (user.Email == data.Email)
            {
                res.AddError(ErrorMessageCodes.EmailAlreadyExists, "This Email is Already Exists!");
            }
        }
        else
        {
            int dbResult = base.Insert(new EvernoteUser()
            {
                UserName = data.Username,
                Email = data.Email,
                ProfileImageFileName = "user-boy.png",
                Password = data.Password,
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false
            });
            if (dbResult > 0)
            {
                res.Result = Find(x => x.UserName == data.Username &&
                                                  x.Email == data.Email);

                //TODO aktivasyon maili atilacaq
                //layerResult.Result.ActivateGuid

                string siteUrl = ConfigHelper.Get<string>("SiteRouteUrl");
                string activateUrl = $"{siteUrl}/Home/ UserActivate/{res.Result.ActivateGuid}";
                string body =
                    $"Welcome {res.Result.UserName} <br> <br> <a href='{activateUrl}' target ='blank'>Click Here</a>" +
                    $" For Activate Your Account.";
                MailHelper.SendMail(body, res.Result.Email, "MyEvernote Activating Account");
            }
        }

        return res;
    }

    public BuisnessLayerResult<EvernoteUser> LoginUser(LoginViewModel data)
    {
        //Giriş yoxlanışı
        //Hesab aktiv edildimi?
        BuisnessLayerResult<EvernoteUser> res = new BuisnessLayerResult<EvernoteUser>();
        res.Result = Find(x => x.UserName == data.Username && x.Password ==
            data.Password);


        if (res.Result != null)
        {
            if (!res.Result.IsActive)
            {
                res.AddError(ErrorMessageCodes.UserIsNotActivated, "Username is not Activated!");
                res.AddError(ErrorMessageCodes.CheckYourEmail, "Please Check Your Email Address.");
            }
        }
        else
        {
            res.AddError(ErrorMessageCodes.UsernameOrPasswordDontMatch, "Username or Password Does not Match!");
        }

        return res;
    }

    public BuisnessLayerResult<EvernoteUser> ActivateUser(Guid activateId)
    {
        BuisnessLayerResult<EvernoteUser> res = new BuisnessLayerResult<EvernoteUser>();
        EvernoteUser user = Find(x => x.ActivateGuid == activateId);
        if (res.Result != null)
        {
            if (res.Result.IsActive)
            {
                res.AddError(ErrorMessageCodes.UserAlreadyActive, "User is Already Activated..");
                return res;
            }

            res.Result.IsActive = true;
            Update(res.Result);
        }
        else
        {
            res.AddError(ErrorMessageCodes.ActivateIdDoesNotExists, "User Is Not Find For Activating!");
        }

        return res;
    }

    public BuisnessLayerResult<EvernoteUser> GetUserById(int id)
    {
        BuisnessLayerResult<EvernoteUser> res = new BuisnessLayerResult<EvernoteUser>();
        res.Result = Find(x => x.Id == id);
        if (res.Result == null)
        {
            res.AddError(ErrorMessageCodes.UserNotFound, "User Not Found!");

        }

        return res;
    }

    public BuisnessLayerResult<EvernoteUser> UpdateProfile(EvernoteUser data)
    {
        EvernoteUser db_user = Find(x => x.UserName == data.UserName ||
                                                    x.Email == data.Email);
        BuisnessLayerResult<EvernoteUser> res = new BuisnessLayerResult<EvernoteUser>();

        res.Result = data;
        if (db_user != null && db_user.Id != data.Id)
        {
            if (db_user.UserName == data.UserName)
            {
                res.AddError(ErrorMessageCodes.UsernameAlreadyExists, "This Username is Already Exists!");
            }

            if (db_user.Email == data.Email)
            {
                res.AddError(ErrorMessageCodes.EmailAlreadyExists, "This Email is Already Exists");
            }

            return res;
        }

        res.Result = Find(x => x.Id == data.Id);
        res.Result.Email = data.Email;
        res.Result.Name = data.Name;
        res.Result.SurName = data.SurName;
        res.Result.Password = data.Password;
        res.Result.UserName = data.UserName;
        //res.Result.IsActive = data.IsActive;
        //res.Result.IsAdmin = data.IsAdmin;

        if (string.IsNullOrEmpty(data.ProfileImageFileName)==false)
        {
            res.Result.ProfileImageFileName = data.ProfileImageFileName;
        }

        if (base.Update(res.Result) == 0)
        {
            res.AddError(ErrorMessageCodes.UserCouldNotUpdated, "User Not Updated!");
        }

        return res;
    }

    public BuisnessLayerResult<EvernoteUser> RemoveUserById(int id)
    {
        BuisnessLayerResult<EvernoteUser> res = new BuisnessLayerResult<EvernoteUser>();
        EvernoteUser user = Find(x => x.Id == id);

        if (user != null)
        {
            if (Delete(user) == 0)
            {
                res.AddError(ErrorMessageCodes.UserCouldNotRemove, "User is Not Removed!");
                return res;
            }

        }
        else
        {
            res.AddError(ErrorMessageCodes.UserCouldNotFound, "User Could'not Find");
        }

        return res;
    }


    // method hiding...new ile biz eyni adli ve parametrli metodu evvelki metodla evez ede bilirik
    public new BuisnessLayerResult<EvernoteUser> Insert(EvernoteUser data)
    {

            EvernoteUser user = Find(x => x.UserName == data.UserName || x.Email == data.Email);
            BuisnessLayerResult<EvernoteUser> res = new BuisnessLayerResult<EvernoteUser>();
            res.Result = data;
            if (user != null)
            {
                if (user.UserName == data.UserName)
                {
                    res.AddError(ErrorMessageCodes.UsernameAlreadyExists, "This UserName is Already Exists!");
                }

                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCodes.EmailAlreadyExists, "This Email is Already Exists!");
                }
            }
            else
            {
                res.Result.ProfileImageFileName = "user-boy.png";
                res.Result.ActivateGuid = Guid.NewGuid();
                if (base.Insert(res.Result)==0)
                {
                    res.AddError(ErrorMessageCodes.UserCouldNotInserted, "This User  Could Not Inserted!");

                }


            }

            return res;
        }


    public new BuisnessLayerResult<EvernoteUser> Update(EvernoteUser data)
    {
        EvernoteUser db_user = Find(x => x.UserName == data.UserName ||
                                         x.Email == data.Email);
        BuisnessLayerResult<EvernoteUser> res = new BuisnessLayerResult<EvernoteUser>();

        if (db_user != null && db_user.Id != data.Id)
        {
            if (db_user.UserName == data.UserName)
            {
                res.AddError(ErrorMessageCodes.UsernameAlreadyExists, "This Username is Already Exists!");
            }

            if (db_user.Email == data.Email)
            {
                res.AddError(ErrorMessageCodes.EmailAlreadyExists, "This Email is Already Exists");
            }

            return res;
        }

        res.Result = Find(x => x.Id == data.Id);
        res.Result.Email = data.Email;
        res.Result.Name = data.Name;
        res.Result.SurName = data.SurName;
        res.Result.Password = data.Password;
        res.Result.UserName = data.UserName;
        res.Result.IsActive = data.IsActive;
        res.Result.IsAdmin = data.IsAdmin;

            if (string.IsNullOrEmpty(data.ProfileImageFileName) == false)
        {
            res.Result.ProfileImageFileName = data.ProfileImageFileName;
        }

        if (base.Update(res.Result) == 0)
        {
            res.AddError(ErrorMessageCodes.ProfileCouldNotUpdated, "Profile Not Updated!");
        }

        return res;
        }
    }

}


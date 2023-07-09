using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyEvernote.Common;
using MyEvernote.Entities;
using MyEvernote.Models;

namespace MyEvernote.Init
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUsername()
        {
            EvernoteUser user = CurrentSession.User;
            if (user != null)
            {
                return user.UserName;

            }

            else
            {
                return "system";

            }

        }
    }
}
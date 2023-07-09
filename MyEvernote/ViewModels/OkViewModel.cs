using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEvernote.ViewModels
{
    public class OkViewModel:NotifyViewModelBase<string>
    {
        public OkViewModel()
        {
            Title = "Success!";
        }
    }
}
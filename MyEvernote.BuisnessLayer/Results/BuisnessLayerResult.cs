using System.Collections.Generic;
using MyEvernote.Entities.Messages;

namespace MyEvernote.BuisnessLayer.Results
{
    public class BuisnessLayerResult<T> where T:class
    {
       public List<ErrorMessageObj> Errors { get; set; } // list 2 property almir deye
                                                                                  // keyvaluepair istifade edirik.

       public T Result { get; set; }

       public BuisnessLayerResult()
       {
           Errors = new List<ErrorMessageObj>();
       }

       public void AddError(ErrorMessageCodes code, string message)
       {
           Errors.Add(new ErrorMessageObj()
           {
              Code = code,
              Message = message
           });
       }
    }
}

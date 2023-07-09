using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyEvernote.Entities.ValueObjects
{
    public class LoginViewModel
    {
        [DisplayName("Username"),
         Required(ErrorMessage = "You Must Enter The {0}"),
         StringLength(25, ErrorMessage = "{0} Max. Must be {1} Character ")]
        public string Username { get; set; }



        [DisplayName("Password"),
         Required(ErrorMessage = "You Must Enter The Password"),
         DataType(DataType.Password),
         StringLength(25, ErrorMessage = "{0} Max. Must be {1} Character ")]
        public string Password { get; set; }    
    }
}
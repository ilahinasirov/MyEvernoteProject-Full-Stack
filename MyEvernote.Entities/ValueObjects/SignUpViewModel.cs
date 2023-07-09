using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyEvernote.Entities.ValueObjects
{
    public class SignUpViewModel
    {
        //[DisplayName("Name"),
        // Required(ErrorMessage = "You Must Enter The {0} !"),
        // StringLength(25, ErrorMessage = "{0} Max. Must be {1} Character !")]
        //public string Name { get; set; }

        //[DisplayName("Surname"),
        // Required(ErrorMessage = "You Must Enter The {0} !"),
        // StringLength(25, ErrorMessage = "{0} Max. Must be {1} Character !")]
        //public string Surname { get; set; }

        [DisplayName("Username"),
         Required(ErrorMessage = "You Must Enter The {0} !"),
         StringLength(25, ErrorMessage = "{0} Max. Must be {1} Character !")]
        public string Username { get; set; }


        [DisplayName("Email"), Required(ErrorMessage = "You Must Enter The {0} !"),
         StringLength(70, ErrorMessage = "{0} Max. Must be {1} Character !"),
         EmailAddress(ErrorMessage = "Please Write an Available Email Address for {0} !")]
        public string Email { get; set; }




        [DisplayName("Password"),
         Required(ErrorMessage = "You Must Enter The {0} !"),
         DataType(DataType.Password),
         StringLength(25, ErrorMessage = "{0} Max. Must be {1} Character !")]
        public string Password { get; set; }



        [DisplayName("Re-Password"),
         Required(ErrorMessage = "You Must Enter The {0} !"),
         DataType(DataType.Password),
         StringLength(25, ErrorMessage = "{0} Max. Must be {1} Character !"),
        Compare("Password",ErrorMessage = "{0} and {1} Does Not Match !")]
        public string RePassword { get; set; }

        
    }
}

namespace MyEvernote.Entities.Messages
{

    //Ayrı-ayrı Xəta mesajlarını kod şəklində verə bilmək üçün
public enum ErrorMessageCodes 
    {
        UsernameAlreadyExists = 101,
        EmailAlreadyExists = 102,
        UserIsNotActivated =151,
        UsernameOrPasswordDontMatch= 152,
        CheckYourEmail = 153,
        UserAlreadyActive =154,
        ActivateIdDoesNotExists =155,
        UserNotFound=156,
        ProfileCouldNotUpdated=157,
        UserCouldNotRemove = 158,
        UserCouldNotFound =159,
        UserCouldNotInserted=160,
        UserCouldNotUpdated=161
    }
}

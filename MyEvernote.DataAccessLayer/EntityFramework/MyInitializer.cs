using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MyEvernote.Entities;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    public class MyInitializer : CreateDatabaseIfNotExists<DatabaseContext>

    {
        protected override void Seed(DatabaseContext context)
        {
            //Adding admin user...
            EvernoteUser admin = new EvernoteUser()
            {
                Name = "Ilahi",
                SurName = "Nasirov",
                Email = "ilahinasirov@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                UserName = "ilahinasirov",
                Password = "123456",
                ProfileImageFileName = "user-boy.png",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUserName = "ilahinasirov"
            };
            //Adding standart user...
            EvernoteUser standartUser = new EvernoteUser()
            {
                Name = "Amrah",
                SurName = "Nasirov",
                Email = "amrahnasirov@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                UserName = "amrahnasirov",
                Password = "654321",
                ProfileImageFileName = "user-boy.png",
                CreatedOn = DateTime.Now.AddHours(1),
                ModifiedOn = DateTime.Now.AddMinutes(65),
                ModifiedUserName = "ilahinasirov"
            };
            context.EvernoteUsers.Add(admin);
            context.EvernoteUsers.Add(standartUser);

            //Adding fake users...
            for (int f = 0; f < 8; f++)
            {
                EvernoteUser user = new EvernoteUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    SurName = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    UserName = $"user{f}",
                    Password = "123456",
                    ProfileImageFileName = "user-boy.png",
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddDays(-1), DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddDays(-1), DateTime.Now),
                    ModifiedUserName = $"user{f}"
                };
                context.EvernoteUsers.Add(user);

            }
            context.SaveChanges();

            //User List For Using in project...
            List<EvernoteUser> userList = context.EvernoteUsers.ToList();

            //Adding fake categories
            for (int i = 0; i < 10; i++)
            {
                Category categ = new Category()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUserName = "ilahinasirov"
                };

                context.Categories.Add(categ);

                //Adding fake  Notes...
                for (int k = 0; k < FakeData.NumberData.GetNumber(5, 9); k++)
                {
                    EvernoteUser owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                    Note note = new Note()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 40)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        //Category = categ,
                        IsDraft = false,
                        LikeCount = FakeData.NumberData.GetNumber(1, 9),
                        Owner = owner,
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddDays(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddDays(-1), DateTime.Now),
                        ModifiedUserName = owner.UserName,
                    };
                    categ.Notes.Add(note);

                    //Adding fake Comments...
                    for (int j = 0; j < FakeData.NumberData.GetNumber(3, 8); j++)
                    {

                        EvernoteUser comment_owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            //Note = note,
                            Owner = comment_owner,
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddDays(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddDays(-1), DateTime.Now),
                            ModifiedUserName = comment_owner.UserName,
                        };
                        note.Comments.Add(comment);
                    }

                    //Adding fake likes...


                    for (int m = 0; m < note.LikeCount; m++)
                    {
                        Liked liked = new Liked()
                        {
                            LikedUser = userList[m],
                        };
                        note.Likes.Add(liked);
                    }

                }

            }

            context.SaveChanges();
        }
    }
}

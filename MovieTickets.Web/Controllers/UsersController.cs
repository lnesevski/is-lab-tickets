using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieTickets.Domain.DomainModels;
using MovieTickets.Domain.DTO;
using MovieTickets.Domain.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTickets.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {

        private readonly UserManager<MoviesUser> userManager;

        public UsersController(UserManager<MoviesUser> userManager)
        {
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }



        public async Task<IActionResult> ImportUsers(IFormFile file)
        {
            if(file == null)
            {
                return RedirectToAction("Index");
            }
            //make a copy
            string pathToUpload = $"{Directory.GetCurrentDirectory()}\\files\\{file.FileName}";

            using (FileStream fileStream = System.IO.File.Create(pathToUpload))
            {
                file.CopyTo(fileStream);

                fileStream.Flush();
            }


            List<MoviesUserDto> users = getAllUsersFromFile(file.FileName);

            bool status = true;

            foreach (var item in users)
            {
                var userCheck = userManager.FindByEmailAsync(item.Email).Result;

                if (userCheck == null)
                {
                    var user = new MoviesUser
                    {
                        UserName = item.Email,
                        NormalizedUserName = item.Email,
                        Email = item.Email,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        UserTickets = new UserTickets()
                    };
                    var result = await userManager.CreateAsync(user, item.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, item.UserRole);
                    }
                    status = status && result.Succeeded;
                }
                else
                {
                    continue;
                }
            }
            if (status) 
            { 
                return RedirectToAction("Index", "Orders");
            }
            else
            {
                return RedirectToAction("Index", "Orders");
            }
        }

        private List<MoviesUserDto> getAllUsersFromFile(string fileName)
        {

            List<MoviesUserDto> users = new List<MoviesUserDto>();

            string filePath = $"{Directory.GetCurrentDirectory()}\\files\\{fileName}";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);


            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        users.Add(new MoviesUserDto
                        {
                            Email = reader.GetValue(0).ToString(),
                            Password = reader.GetValue(1).ToString(),
                            ConfirmPassword = reader.GetValue(1).ToString(),
                            UserRole = reader.GetValue(2).ToString() == "Administrator" ? "Administrator" : "StandardUser"
                        });
                    }
                }
            }


            return users;
        }


    }


}

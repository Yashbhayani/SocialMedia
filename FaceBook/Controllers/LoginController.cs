using FaceBook.MainModels;
using FaceBookApp.Controllers;
using FaceBookApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FaceBookApp.MainController
{

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        public IConfiguration _configuration;

        public LoginController(IConfiguration config)
        {
            _configuration = config;
        }

        [Route("/Signup")]
        [HttpPost]
        public async Task<IActionResult> Signup([FromForm] SignUPClass signUPClass)
        {
            try
            {
                using (var db = new FaceBookdbContext())
                {
                    if (db.UserTables.Where(x => x.Email == signUPClass.Emial).Count() > 0)
                    {
                        StatusCodeMessageClass falseStatusCodeMessageClass = new StatusCodeMessageClass
                        {
                            Success = false,
                            Message = "Email id Alredy Added!"
                        };
                        return Ok(value: falseStatusCodeMessageClass);
                    }

                    var folderName = Path.Combine("Images", "Userprofile");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    if (signUPClass.image.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(signUPClass.image.ContentDisposition).FileName.Trim('"');
                        string renameFile = Convert.ToString(Guid.NewGuid()) + "." + fileName.Split('.').Last();
                        var fullPath = Path.Combine(pathToSave, renameFile);
                        var _contexxtPath = Path.Combine(folderName, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            signUPClass.image.CopyTo(stream);
                        }

                        UserTable userTable = new UserTable();
                        userTable.FirstName = signUPClass.FirstName;
                        userTable.LastName = signUPClass.LastName;
                        userTable.Email = signUPClass.Emial;
                        userTable.Password = signUPClass.Password;
                        userTable.Image = renameFile;
                        db.UserTables.Add(userTable);
                        await db.SaveChangesAsync();

                        StatusCodeMessageClass trueStatusCodeMessageClass = new StatusCodeMessageClass
                        {
                            Success = true,
                            Message = "Account is created!"
                        };
                        return Ok(value: trueStatusCodeMessageClass);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }    
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("/Signin")]
        [HttpPost]
        public IActionResult SignIn([FromBody] SigninClass signinClass)
        {
            try
            {
                using (var db = new FaceBookdbContext())
                {
                    if (db.UserTables.Where(x => x.Email == signinClass.EmialId && x.Password == signinClass.password).FirstOrDefault() != null)
                    {
                        var UserData = db.UserTables.Where(x => x.Email == signinClass.EmialId).FirstOrDefault();
                        var token = TokenController.GetToken(UserData.Email, UserData.FirstName, UserData.LastName, _configuration);
                        TokenClass tokenClass = new TokenClass()
                        {
                            Success = true,
                            Message = "Login Success Full!",
                            Token = token,
                            Email = UserData.Email
                        };
                        return Ok(value: tokenClass);
                    }
                    else
                    {
                        StatusCodeMessageClass falseStatusCodeMessageClass = new StatusCodeMessageClass
                        {
                            Success = false,
                            Message = "User is Not valid!"
                        };
                        return Ok(value: falseStatusCodeMessageClass);
                    }
                }
            }
            
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }
    }
}

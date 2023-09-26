using FaceBook.MainModels;
using FaceBookApp.Controllers;
using FaceBookApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using SocialMediaApplication.Model;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SocialMediaApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        [Route("/GetPostData")]
        [HttpGet]
        public IActionResult GetPostData([FromHeader] string token)
        {
            try
            {
                var data = TokenController.GetEmailMethode(token);
                if(data.Email != null || data.Email != "")
                {
                    using (var db = new FaceBookdbContext())
                    {
                        if (db.UserTables.Where(x => x.Email == data.Email).Count() > 0)
                        {
                            var UserData = db.UserTables.Where(x => x.Email == data.Email).FirstOrDefault();

                            var friendRequestSenderIds = db.FriendRequestTables.
                                Where(fr => fr.ReceiverUserId == UserData.Id && fr.Status == "Accepted")
                                .Select(fr => fr.SenderUserId)
                                .Distinct();

                            var friendRequestReceiverIds = db.FriendRequestTables
                                .Where(fr => fr.SenderUserId == UserData.Id && fr.Status == "Accepted")
                                .Select(fr => fr.ReceiverUserId)
                                .Distinct();

                            var query = from post in db.PostTables
                                        join user in db.UserTables on post.UserId equals user.Id into userGroup
                                        from user in userGroup.DefaultIfEmpty()
                                        where friendRequestSenderIds.Contains(post.UserId)
                                            || friendRequestReceiverIds.Contains(post.UserId)
                                            || user.Id == UserData.Id
                                        let IsLikedByLoggedInUser = db.LikeTables.Any(l => l.PostId == post.Id && l.UserId == UserData.Id)
                                        select new
                                        {
                                            LogInUserID = UserData.Id,
                                            Post_id = post.Id,
                                            Post_UserId = user.Id,
                                            UserID = user.Id,
                                            UserImage = user.Image,
                                            First_Name = user.FirstName,
                                            Last_Name = user.LastName,
                                            Image = post.Image,
                                            Post_Content = post.Content,
                                            Total_Like = db.LikeTables.Count(like => like.PostId == post.Id),
                                            Total_Comment = db.CommentTables.Count(comment => comment.PostId == post.Id),
                                            status = IsLikedByLoggedInUser ? "TRUE" : "FALSE"
                                        };

                            return Ok(new
                            {
                                Success = true,
                                Data = query.Distinct().ToList()
                            }) ;
                        }
                        else
                        {
                            StatusCodeMessageClass falseStatusCodeMessageClass = new StatusCodeMessageClass
                            {
                                Success = false,
                                Message = "User not Found!"
                            };
                            return Ok(value: falseStatusCodeMessageClass);
                        }
                    }    
                }
                else
                {
                    StatusCodeMessageClass falseStatusCodeMessageClass = new StatusCodeMessageClass
                    {
                        Success = false,
                        Message = "User can not Authenticate!"
                    };
                    return Ok(value: falseStatusCodeMessageClass);
                }
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [Route("/AddPostData")]
        [HttpPost]
        public async Task<IActionResult> AddPostData([FromForm] AddPostClass addPostClass, [FromHeader] string token)
        {
            try
            {
                var data = TokenController.GetEmailMethode(token);
                if (data.Email != null || data.Email != "")
                {
                    using (var db = new FaceBookdbContext())
                    {
                        if (db.UserTables.Where(x => x.Email == data.Email).Count() != 1)
                        {
                            StatusCodeMessageClass falseStatusCodeMessageClass = new StatusCodeMessageClass
                            {
                                Success = false,
                                Message = "User is Invalid!"
                            };
                            return Ok(value: falseStatusCodeMessageClass);
                        }
                        var folderName = Path.Combine("Images", "PostedImage");
                        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                        if(addPostClass.image.Length > 0)
                        {
                            var fileName = ContentDispositionHeaderValue.Parse(addPostClass.image.ContentDisposition).FileName.Trim('"');
                            string renameFile = Convert.ToString(Guid.NewGuid()) + "." + fileName.Split('.').Last();
                            var fullPath = Path.Combine(pathToSave, renameFile);
                            var dbPath = Path.Combine(folderName, fileName);
                            using (var stream = new FileStream(fullPath, FileMode.Create))
                            {
                                addPostClass.image.CopyTo(stream);
                            }

                            var UserData = db.UserTables.Where(x => x.Email == data.Email).FirstOrDefault();

                            PostTable postTable = new PostTable();
                            postTable.UserId = UserData.Id;
                            postTable.Image = renameFile;
                            postTable.Content = addPostClass.Content;

                            db.PostTables.Add(postTable);
                            await db.SaveChangesAsync();

                            StatusCodeMessageClass trueStatusCodeMessageClass = new StatusCodeMessageClass
                            {
                                Success = true,
                                Message = "Post Added!"
                            };
                            return Ok(value: trueStatusCodeMessageClass);
                        }
                        else
                        {
                            return BadRequest();
                        }

                    }
                }
                else
                {
                    StatusCodeMessageClass falseStatusCodeMessageClass = new StatusCodeMessageClass
                    {
                        Success = false,
                        Message = "User can not Authenticate!"
                    };
                    return Ok(value: falseStatusCodeMessageClass);
                }
            }
            catch (Exception)
            {
                return Ok();
            }
        }

        [Route("/ImageChangeData")]
        [HttpPut]
        public async Task<IActionResult> ImageChangeData([FromForm] UpdatePostClass updatePostClass, [FromHeader] string token)
        {
            try {
                var data = TokenController.GetEmailMethode(token);
                if (data.Email != null || data.Email != "")
                {
                    using (var db = new FaceBookdbContext())
                    {
                        if (db.UserTables.Where(x => x.Email == data.Email).Count() != 1)
                        {
                            StatusCodeMessageClass falseStatusCodeMessageClass = new StatusCodeMessageClass
                            {
                                Success = false,
                                Message = "User is Invalid!"
                            };
                            return Ok(value: falseStatusCodeMessageClass);
                        }

                        var folderName = Path.Combine("Images", "PostedImage");
                        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                        if (updatePostClass.image.Length > 0)
                        {
                            var fileName = ContentDispositionHeaderValue.Parse(updatePostClass.image.ContentDisposition).FileName.Trim('"');
                            string renameFile = Convert.ToString(Guid.NewGuid()) + "." + fileName.Split('.').Last();
                            var fullPath = Path.Combine(pathToSave, renameFile);
                            var dbPath = Path.Combine(folderName, fileName);
                            using (var stream = new FileStream(fullPath, FileMode.Create))
                            {
                                updatePostClass.image.CopyTo(stream);
                            }
                            var PostData = db.PostTables.Where(x => x.Id == updatePostClass.Id);
                            if (PostData.Count() > 0)
                            {
                                if (db.UserTables.Where(x => x.Email == data.Email && x.Id == PostData.FirstOrDefault().UserId).Count() > 0)
                                {
                                    var P = PostData.FirstOrDefault();

                                    P.Image = renameFile;
                                    P.Content = updatePostClass.Content;
                                    await db.SaveChangesAsync();
                                    StatusCodeMessageClass trueStatusCodeMessageClass = new StatusCodeMessageClass
                                    {
                                        Success = true,
                                        Message = "Post Updated!"
                                    };
                                    return Ok(value: trueStatusCodeMessageClass);
                                }
                                else
                                {
                                    StatusCodeMessageClass falseStatusCodeMessageClass = new StatusCodeMessageClass
                                    {
                                        Success = false,
                                        Message = "User is Invalid!"
                                    };
                                    return Ok(value: falseStatusCodeMessageClass);
                                }
                            }
                            else
                            {
                                StatusCodeMessageClass falseStatusCodeMessageClass = new StatusCodeMessageClass
                                {
                                    Success = false,
                                    Message = "Post is Invalid!"
                                };
                                return Ok(value: falseStatusCodeMessageClass);
                            }

                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                }
                else
                {
                    StatusCodeMessageClass falseStatusCodeMessageClass = new StatusCodeMessageClass
                    {
                        Success = false,
                        Message = "User can not Authenticate!"
                    };
                    return Ok(value: falseStatusCodeMessageClass);
                }
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [Route("/NoImageChangeData")]
        [HttpPut]
        public async Task<IActionResult> NoImageChangeData([FromBody] NoImageClass updatePostClass, [FromHeader] string token)
        {
            try
            {
                var data = TokenController.GetEmailMethode(token);
                if (data.Email != null || data.Email != "")
                {
                    using (var db = new FaceBookdbContext())
                    {
                        if (db.UserTables.Where(x => x.Email == data.Email).Count() != 1)
                        {
                            StatusCodeMessageClass falseStatusCodeMessageClass = new StatusCodeMessageClass
                            {
                                Success = false,
                                Message = "User is Invalid!"
                            };
                            return Ok(value: falseStatusCodeMessageClass);
                        }
                            var PostData = db.PostTables.Where(x => x.Id == updatePostClass.Id);
                            if (PostData.Count() > 0)
                            {
                                if (db.UserTables.Where(x => x.Email == data.Email && x.Id == PostData.FirstOrDefault().UserId).Count() > 0)
                                {
                                    var P = PostData.FirstOrDefault();

                                    P.Content = updatePostClass.Content;
                                    await db.SaveChangesAsync();
                                    StatusCodeMessageClass trueStatusCodeMessageClass = new StatusCodeMessageClass
                                    {
                                        Success = true,
                                        Message = "Post Updated!"
                                    };
                                    return Ok(value: trueStatusCodeMessageClass);
                                }
                                else
                                {
                                    StatusCodeMessageClass falseStatusCodeMessageClass = new StatusCodeMessageClass
                                    {
                                        Success = false,
                                        Message = "User is Invalid!"
                                    };
                                    return Ok(value: falseStatusCodeMessageClass);
                                }
                            }
                            else
                            {
                                StatusCodeMessageClass falseStatusCodeMessageClass = new StatusCodeMessageClass
                                {
                                    Success = false,
                                    Message = "Post is Invalid!"
                                };
                                return Ok(value: falseStatusCodeMessageClass);
                            }

                        
                    }
                }
                else
                {
                    StatusCodeMessageClass falseStatusCodeMessageClass = new StatusCodeMessageClass
                    {
                        Success = false,
                        Message = "User can not Authenticate!"
                    };
                    return Ok(value: falseStatusCodeMessageClass);
                }
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [Route("/DeletePostData")]
        [HttpDelete]
        public IActionResult DelatePost(int id, [FromHeader] string token)
        {
            try
            {
                var data = TokenController.GetEmailMethode(token);
                if (data.Email != null || data.Email != "")
                {
                    using (var db = new FaceBookdbContext())
                    {
                        if (db.UserTables.Where(x => x.Email == data.Email).Count() != 1)
                        {
                            StatusCodeMessageClass falseStatusCodeMessageClass = new StatusCodeMessageClass
                            {
                                Success = false,
                                Message = "User is Invalid!"
                            };
                            return Ok(value: falseStatusCodeMessageClass);
                        }

                        var Postdata = db.PostTables.Where(x => x.Id == id);

                        if (Postdata.Count() > 0)
                        {
                            var U = db.UserTables.Where(x => x.Email == data.Email).FirstOrDefault();
                            if (db.PostTables.Where(x => x.Id == id && x.UserId == U.Id).Count() > 0)
                            {
                                var P = Postdata.FirstOrDefault();
                                db.PostTables.Remove(P);
                                db.SaveChanges();
                                StatusCodeMessageClass trueStatusCodeMessageClass = new StatusCodeMessageClass
                                {
                                    Success = true,
                                    Message = "Post Deleted!"
                                };
                                return Ok(value: trueStatusCodeMessageClass);
                            }
                            else
                            {
                                StatusCodeMessageClass falseStatusCodeMessageClass = new StatusCodeMessageClass
                                {
                                    Success = false,
                                    Message = "User is Not Valid!"
                                };
                                return Ok(value: falseStatusCodeMessageClass);
                            }
                        }
                        else
                        {
                            StatusCodeMessageClass falseStatusCodeMessageClass = new StatusCodeMessageClass
                            {
                                Success = false,
                                Message = "Id is Not Valid!"
                            };
                            return Ok(value: falseStatusCodeMessageClass);
                        }
                    }
                }
                else
                {
                    StatusCodeMessageClass falseStatusCodeMessageClass = new StatusCodeMessageClass
                    {
                        Success = false,
                        Message = "User can not Authenticate!"
                    };
                    return Ok(value: falseStatusCodeMessageClass);
                }
            }catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

    }
}

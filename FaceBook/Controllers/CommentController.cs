using FaceBook.MainModels;
using FaceBookApp.Controllers;
using FaceBookApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaApplication.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : Controller
    {
        [Route("/AddComment")]
        [HttpPatch]
        public async Task<IActionResult> AddComment([FromBody] CommentClass commentClass, [FromHeader] string token)
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

                        var UserData = db.UserTables.Where(x => x.Email == data.Email).FirstOrDefault();
                        var DP = db.PostTables.Where(x => x.Id == commentClass.PostId);
                        if (DP.Count() > 0)
                        {
                            CommentTable commentTable = new CommentTable();
                            commentTable.UserId = UserData.Id;
                            commentTable.PostId = commentClass.PostId;
                            commentTable.Comment = commentClass.Comment_Content;

                            db.CommentTables.Add(commentTable);
                            await db.SaveChangesAsync();
                            StatusCodeMessageClass trueStatusCodeMessageClass = new StatusCodeMessageClass
                            {
                                Success = true,
                                Message = "Comment Added!"
                            };
                            return Ok(value: trueStatusCodeMessageClass);
                        }
                        else
                        {
                            StatusCodeMessageClass falseStatusCodeMessageClass = new StatusCodeMessageClass
                            {
                                Success = false,
                                Message = "Post Invalid!"
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
                return View(e.Message);
            }
        }


        [Route("/GetComment")]
        [HttpPatch]
        public IActionResult GetComment([FromBody] int Postid, [FromHeader] string token)
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

                        var UserData = db.UserTables.Where(x => x.Email == data.Email).FirstOrDefault();
                        var DP = db.PostTables.Where(x => x.Id == Postid);
                        if (DP.Count() > 0)
                        {
                            var Postdata = from post in db.PostTables
                                           where post.Id == Postid
                                           join user in db.UserTables on post.UserId equals user.Id
                                           let totalLikes = db.LikeTables.Count(like => like.PostId == post.Id)
                                           let totalComments = db.CommentTables.Count(comment => comment.PostId == post.Id)
                                           let userLiked = db.LikeTables.Any(liked => liked.PostId == post.Id && liked.UserId == UserData.Id)
                                           select new
                                           {
                                               post_id = post.Id,
                                               First_Name = user.FirstName,
                                               Last_Name = user.LastName,
                                               UserImage = user.Image,
                                               Image = post.Image,
                                               Post_Content = post.Content,
                                               Total_Like = totalLikes,
                                               Total_Comment = totalComments,
                                               status = userLiked ? "TRUE" : "FALSE",
                                               _time = post.CreatedAt
                                           };
                            var query = from comment in db.CommentTables
                                        where comment.PostId == Postid
                                        join user in db.UserTables on comment.UserId equals user.Id into userGroup
                                        from user in userGroup.DefaultIfEmpty()
                                        select new
                                        {
                                            LOGUserData = UserData.Id,
                                            Id = comment.Id,
                                            Userid = user.Id,
                                            First_Name = user.FirstName,
                                            Last_Name = user.LastName,
                                            Email = user.Email,
                                            Image = user.Image,
                                            Comment = comment.Comment,
                                            Date_time = comment.CreatedAt
                                        };


                            return Ok(new
                            {
                                Success = true,
                                Postdata = Postdata.ToList(),
                                Data = query.ToList()
                            });
                        }
                        else
                        {
                            StatusCodeMessageClass falseStatusCodeMessageClass = new StatusCodeMessageClass
                            {
                                Success = false,
                                Message = "Post Invalid!"
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
                return View(e.Message);
            }
        }





        [Route("/DeleteComment")]
        [HttpDelete]
        public async Task<IActionResult> DeleteComment(int commentid, [FromHeader] string token)
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

                        var UserData = db.UserTables.Where(x => x.Email == data.Email).FirstOrDefault();

                        var POST_Comment = db.CommentTables.Where(x => x.Id == commentid).ToList().FirstOrDefault();

                        if (POST_Comment.UserId == UserData.Id)
                        {
                            db.CommentTables.Remove(POST_Comment);
                            await db.SaveChangesAsync();
                            StatusCodeMessageClass trueStatusCodeMessageClass = new StatusCodeMessageClass
                            {
                                Success = true,
                                Message = "Comment Deleted!"
                            };
                            return Ok(trueStatusCodeMessageClass);
                        }
                        else 
                        {
                            StatusCodeMessageClass falseStatusCodeMessageClass = new StatusCodeMessageClass
                            {
                                Success = true,
                                Message = "User is Not valid!"
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
                return View(e.Message);
            }
        }
    }
}

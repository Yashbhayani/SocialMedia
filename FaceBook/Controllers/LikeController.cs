using FaceBook.MainModels;
using FaceBookApp.Controllers;
using FaceBookApp.Model;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApplication.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : Controller
    {
        [Route("/AddAndDeleteLike")]
        [HttpPatch]
        public async Task<IActionResult> AddAndDeleteLike([FromBody] int POSTID, [FromHeader] string token)
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

                        if (db.LikeTables.Where(x => x.PostId == POSTID && x.UserId == UserData.Id).Count() > 0)
                        {
                            var DP = db.LikeTables.Where(x => x.PostId == POSTID && x.UserId == UserData.Id).FirstOrDefault();
                            db.LikeTables.Remove(DP);
                            db.SaveChanges();
                            StatusCodeMessageClass trueStatusCodeMessageClass = new StatusCodeMessageClass
                            {
                                Success = true,
                                Message = "Like Deleted!"
                            };
                            return Ok(value: trueStatusCodeMessageClass);
                        }
                        else
                        {

                            var DP = db.PostTables.Where(x => x.Id == POSTID);
                            if (DP.Count() > 0)
                            {
                                LikeTable likeTable = new LikeTable();
                                likeTable.PostId = POSTID;
                                likeTable.UserId = UserData.Id;

                                db.LikeTables.Add(likeTable);
                                await db.SaveChangesAsync();
                                StatusCodeMessageClass trueStatusCodeMessageClass = new StatusCodeMessageClass
                                {
                                    Success = true,
                                    Message = "Like Added!"
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


        [Route("/LikeList")]
        [HttpPatch]
        public IActionResult LikeList([FromBody] int POSTID, [FromHeader] string token)
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

                        if (db.PostTables.Where(x=> x.Id == POSTID).Count() > 0) {
                            var result = from l in db.LikeTables
                                         join u in db.UserTables on l.UserId equals u.Id into userGroup
                                         from u in userGroup.DefaultIfEmpty()
                                         where l.PostId == POSTID
                                         select new
                                         {
                                             FirstName = u.FirstName,
                                             LastName = u.LastName,
                                             Image = u.Image
                                         };

                            return Ok(new
                            {
                                Success = true,
                                Data = result.ToList()
                            });
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
                return View(e.Message);
            }
        }
    }
}


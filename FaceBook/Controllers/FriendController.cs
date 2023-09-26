using FaceBook.MainModels;
using FaceBookApp.Controllers;
using FaceBookApp.Model;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendController : Controller
    {
        [Route("/SendandcancleFriendRequest")]
        [HttpPatch]
        public async Task<IActionResult> SendAndCancleFriendRequest([FromBody] int ReciverUserId, [FromHeader] string token)
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

                        if (db.FriendRequestTables.Where(x => x.SenderUserId == UserData.Id && x.ReceiverUserId == ReciverUserId).Count() <= 0)
                        {
                            if(db.FriendRequestTables.Where(x => x.SenderUserId == ReciverUserId && x.ReceiverUserId == UserData.Id && x.Status == "Accepted").Count() > 0)
                            {
                                StatusCodeMessageClass falseStatusCodeMessageClass = new StatusCodeMessageClass
                                {
                                    Success = false,
                                    Message = "Alredy Friend"
                                };
                                return Ok(value: falseStatusCodeMessageClass);
                            }

                            if(db.FriendRequestTables.Where(x => x.SenderUserId == ReciverUserId && x.ReceiverUserId == UserData.Id && x.Status == "Pending").Count() > 0)
                            {
                                
                                StatusCodeMessageClass falseStatusCodeMessageClass = new StatusCodeMessageClass
                                {
                                    Success = false,
                                    Message = "Request Pending"
                                };
                                return Ok(value: falseStatusCodeMessageClass);
                            }

                            if (db.FriendRequestTables.Where(x => x.SenderUserId == ReciverUserId && x.ReceiverUserId == UserData.Id && x.Status == "Rejected").Count() > 0)
                            {
                                var Req = db.FriendRequestTables.Where(x => x.SenderUserId == ReciverUserId && x.ReceiverUserId == UserData.Id && x.Status == "Rejected").FirstOrDefault();

                                db.FriendRequestTables.Remove(Req);
                                db.SaveChanges();
                            }

                            FriendRequestTable friendRequestTable = new FriendRequestTable();
                            friendRequestTable.ReceiverUserId = ReciverUserId;
                            friendRequestTable.SenderUserId = UserData.Id;

                            db.FriendRequestTables.Add(friendRequestTable);
                            await db.SaveChangesAsync();
                            StatusCodeMessageClass trueStatusCodeMessageClass = new StatusCodeMessageClass
                            {
                                Success = true,
                                Message = "Send Friend Request!"
                            };
                            return Ok(value: trueStatusCodeMessageClass);
                        }
                        else
                        {
                            StatusCodeMessageClass falseStatusCodeMessageClass = new StatusCodeMessageClass
                            {
                                Success = false,
                                Message = "Alredy send Request!"
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

        [Route("/CancleFriendRequest")]
        [HttpPatch]
        public  IActionResult CancleFriendRequest([FromBody] int ReciverUserId, [FromHeader] string token)
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

                        if (db.FriendRequestTables.Where(x => x.SenderUserId == UserData.Id && x.ReceiverUserId == ReciverUserId &&  x.Status == "Pending").Count() > 0)
                        {
                            var Req = db.FriendRequestTables.Where(x => x.SenderUserId == UserData.Id && x.ReceiverUserId == ReciverUserId && x.Status == "Pending").FirstOrDefault();
                            db.FriendRequestTables.Remove(Req);
                            db.SaveChanges();
                            StatusCodeMessageClass trueStatusCodeMessageClass = new StatusCodeMessageClass
                            {
                                Success = true,
                                Message = "Remove Friend!"
                            };
                            return Ok(value: trueStatusCodeMessageClass);
                        }

                        if (db.FriendRequestTables.Where(x => x.SenderUserId == ReciverUserId  && x.ReceiverUserId == UserData.Id && x.Status == "Pending").Count() > 0)
                        {
                            var Req = db.FriendRequestTables.Where(x => x.SenderUserId == ReciverUserId && x.ReceiverUserId == UserData.Id && x.Status == "Pending").FirstOrDefault();
                            db.FriendRequestTables.Remove(Req);
                            db.SaveChanges();
                            StatusCodeMessageClass trueStatusCodeMessageClass = new StatusCodeMessageClass
                            {
                                Success = true,
                                Message = "Remove Friend!"
                            };
                            return Ok(value: trueStatusCodeMessageClass);

                        }
                        else
                        {
                            StatusCodeMessageClass falseStatusCodeMessageClass = new StatusCodeMessageClass
                            {
                                Success = false,
                                Message = "Alredy send Request!"
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

        [Route("/AcceptRequest")]
        [HttpPatch]
        public async Task<IActionResult> AcceptRequest([FromBody] int SenerUserId, [FromHeader] string token)
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
                        if (db.FriendRequestTables.Where(x => x.SenderUserId == SenerUserId && x.ReceiverUserId == UserData.Id && x.Status == "Pending").Count() > 0)
                        {
                            var Req = db.FriendRequestTables.Where(x => x.SenderUserId == SenerUserId && x.ReceiverUserId == UserData.Id && x.Status == "Pending").FirstOrDefault();
                            Req.Status = "Accepted";
                            await db.SaveChangesAsync();

                            StatusCodeMessageClass trueStatusCodeMessageClass = new StatusCodeMessageClass
                            {
                                Success = true,
                                Message = "Add Friend!"
                            };
                            return Ok(value: trueStatusCodeMessageClass);
                        }
                        else
                        {
                            StatusCodeMessageClass falseStatusCodeMessageClass = new StatusCodeMessageClass
                            {
                                Success = false,
                                Message = "Not Valid!"
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

        [Route("/RemoveFriend")]
        [HttpDelete]
        public IActionResult RemoveFriend(int UserID, [FromHeader] string token)
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
                        if (db.FriendRequestTables.Where(x => x.SenderUserId == UserID && x.ReceiverUserId == UserData.Id && x.Status == "Accepted").Count() > 0)
                        {
                            var Req = db.FriendRequestTables.Where(x => x.SenderUserId == UserID && x.ReceiverUserId == UserData.Id && x.Status == "Accepted").FirstOrDefault();
                            db.FriendRequestTables.Remove(Req);
                            db.SaveChanges();

                            StatusCodeMessageClass trueStatusCodeMessageClass = new StatusCodeMessageClass
                            {
                                Success = true,
                                Message = "Remove Friend!"
                            };
                            return Ok(value: trueStatusCodeMessageClass);
                        }

                        if (db.FriendRequestTables.Where(x => x.SenderUserId == UserData.Id && x.ReceiverUserId == UserID && x.Status == "Accepted").Count() > 0)
                        {
                            var Req = db.FriendRequestTables.Where(x => x.SenderUserId == UserData.Id && x.ReceiverUserId == UserID && x.Status == "Accepted").FirstOrDefault();
                            db.FriendRequestTables.Remove(Req);
                            db.SaveChanges();

                            StatusCodeMessageClass trueStatusCodeMessageClass = new StatusCodeMessageClass
                            {
                                Success = true,
                                Message = "Remove Friend!"
                            };
                            return Ok(value: trueStatusCodeMessageClass);
                        }
                        else
                        {
                            StatusCodeMessageClass falseStatusCodeMessageClass = new StatusCodeMessageClass
                            {
                                Success = false,
                                Message = "Not Valid!"
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

        [Route("/GetFriendList")]
        [HttpGet]
        public IActionResult GetFriendList([FromHeader] string token)
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
                    var senderAcceptedFriends = from friendRequest in db.FriendRequestTables
                                                join user in db.UserTables on friendRequest.ReceiverUserId equals user.Id
                                                where friendRequest.SenderUserId == UserData.Id && friendRequest.Status == "Accepted"
                                                select new
                                                {
                                                    RequestId = friendRequest.Id,
                                                    UserId = user.Id,
                                                    FirstName = user.FirstName,
                                                    LastName = user.LastName,
                                                    Email = user.Email,
                                                    Image = user.Image
                                                };

                    var receiverAcceptedFriends = from friendRequest in db.FriendRequestTables
                                                  join user in db.UserTables on friendRequest.SenderUserId equals user.Id
                                                  where friendRequest.ReceiverUserId == UserData.Id && friendRequest.Status == "Accepted"
                                                  select new
                                                  {
                                                      RequestId = friendRequest.Id,
                                                      UserId = user.Id,
                                                      FirstName = user.FirstName,
                                                      LastName = user.LastName,
                                                      Email = user.Email,
                                                      Image = user.Image
                                                  };

                    var result = senderAcceptedFriends.Union(receiverAcceptedFriends).Distinct().ToList();
                    return Ok(new
                    {
                        Success = true,
                        Data = result.ToList()
                    });

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

        [Route("/GetSendFriendList")]
        [HttpGet]
        public IActionResult GetSendFriendList([FromHeader] string token)
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
                    var result = from friendRequest in db.FriendRequestTables
                                 join user in db.UserTables on friendRequest.ReceiverUserId equals user.Id
                                 where friendRequest.SenderUserId == UserData.Id && friendRequest.Status == "Pending"
                                 select new
                                 {
                                     RequestId = friendRequest.Id,
                                     UserId = user.Id,
                                     FirstName = user.FirstName,
                                     LastName = user.LastName,
                                     Email = user.Email,
                                     Image = user.Image
                                 };
                    return Ok(new
                    {
                        Success = true,
                        Data = result.ToList()
                    });

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


        [Route("/GetReciveFriendList")]
        [HttpGet]
        public IActionResult GetReciveFriendList([FromHeader] string token)
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

                    var result = from friendRequest in db.FriendRequestTables
                                                  join user in db.UserTables on friendRequest.SenderUserId equals user.Id
                                                  where friendRequest.ReceiverUserId == UserData.Id && friendRequest.Status == "Pending"
                                                  select new
                                                  {
                                                      RequestId = friendRequest.Id,
                                                      UserId = user.Id,
                                                      FirstName = user.FirstName,
                                                      LastName = user.LastName,
                                                      Email = user.Email,
                                                      Image = user.Image
                                                  };

                    return Ok(new
                    {
                        Success = true,
                        Data = result.ToList()
                    });

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


        [Route("/SearchList")]
        [HttpPatch]
        public IActionResult SearchList([FromBody] SearchUserModel uname,[FromHeader] string token)
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
                    var query = from u in db.UserTables
                                where (u.FirstName.Contains(uname.uname) || u.LastName.Contains(uname.uname) || u.Email.Contains(uname.uname)) && u.Id != UserData.Id
                                select new
                                {
                                    UserId = u.Id,
                                    FirstName = u.FirstName,
                                    LastName = u.LastName,
                                    Email = u.Email,
                                    Image = u.Image,
                                    Status = (
                                        from f1 in db.FriendRequestTables
                                        where f1.SenderUserId == u.Id && f1.ReceiverUserId == UserData.Id && f1.Status == "Accepted"
                                        select "R"
                                    ).FirstOrDefault() ??
                                    (
                                        from f2 in db.FriendRequestTables
                                        where f2.SenderUserId == UserData.Id && f2.ReceiverUserId == u.Id && f2.Status == "Accepted"
                                        select "R"
                                    ).FirstOrDefault() ??
                                    (
                                        from f1 in db.FriendRequestTables
                                        where f1.SenderUserId == u.Id && f1.ReceiverUserId == UserData.Id && f1.Status == "Pending"
                                        select "AC"
                                    ).FirstOrDefault() ??
                                    (
                                        from f2 in db.FriendRequestTables
                                        where f2.SenderUserId == UserData.Id && f2.ReceiverUserId == u.Id && f2.Status == "Pending"
                                        select "C"
                                    ).FirstOrDefault() ??
                                    "Add Friend"
                                };

                    var distinctQuery = query.Distinct().ToList();


                    return Ok(new
                    {
                        Success = true,
                        Data = distinctQuery
                    });

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
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using demo.Models;
using demo.ModelViews;
using demo.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace demo.Controllers
{
    public class UsersController : ControllerBase
    {
        public IActionResult GetUsers()
        {
            BusinessContext context = new BusinessContext();
            User user = new User();
            user.Username = "a";
            user.Password = "b";
            context.Add(user);
            context.SaveChanges();
            UserView userView = Mapper.Map<UserView>(user);
            return Ok(userView);
        }

        [HttpPost]
        public IActionResult Login([FromForm] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            BusinessContext context = new BusinessContext();
            User user = context.User.Where(x => x.Username.Equals(loginRequest.Username) && x.Password.Equals(loginRequest.Password)).FirstOrDefault();
            if (user == null)
                return BadRequest();
            

            return Ok(user);
        }
    }
}
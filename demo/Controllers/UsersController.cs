using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using demo.Models;
using demo.ModelViews;
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
    }
}
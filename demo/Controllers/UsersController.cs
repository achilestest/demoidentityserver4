using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using demo.IdentityServer;
using demo.Models;
using demo.ModelViews;
using demo.Request;
using IdentityModel.Client;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace demo.Controllers
{
    public class UsersController : ControllerBase
    {
        [HttpGet]
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
        public async Task<IActionResult> Login([FromForm] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            HttpClient client = new HttpClient();
            TokenRequest tokenRequest = new TokenRequest
            {
                Address = Discovery.LoginEndpoint,
                ClientId = loginRequest.Client_id,
                GrantType = "password",
                ClientSecret = "bsd",
            };
            tokenRequest.Parameters.Add("username", loginRequest.Username);
            tokenRequest.Parameters.Add("password", loginRequest.Password);
            var response = await client.RequestTokenAsync(tokenRequest);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK || response.IsError)
            {
                throw new Exception(response.Error);
            }
            return Ok(response.Json);
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromForm] Request.RefreshTokenRequest refreshRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            HttpClient client = new HttpClient();
            IdentityModel.Client.RefreshTokenRequest tokenRequest = new IdentityModel.Client.RefreshTokenRequest
            {
                Address = Discovery.LoginEndpoint,
                ClientId = refreshRequest.Client_id,
                ClientSecret = "bsd",
                RefreshToken = refreshRequest.RefreshToken,
                GrantType = "refresh_token"
            };
            var response = await client.RequestRefreshTokenAsync(tokenRequest);
            //var tokenClient = new TokenClient(Discovery.LoginEndpoint, refreshRequest.Client_id,"bsd");
            //var response = tokenClient.RequestRefreshTokenAsync(refreshRequest.RefreshToken).Result;
            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK || response.IsError)
            {
                return BadRequest(response.Error);
            }
            return Ok(response.Json);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    
    public class Account : Base
    {
        private readonly DataContext _dataContext;
        private readonly IToken _tokenService;

        public Account(DataContext dataContext,IToken tokenService) 
        {
            _dataContext = dataContext;
           _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>>Register(RegisterDto registerDto)
        {
            if(await Userexists(registerDto.UserName)) return BadRequest("Username taken");

            using var hmac=new HMACSHA512();
            var user=new AppUser(){
                UserName=registerDto.UserName,
                PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                Passwordsalt=hmac.Key
            };
            _dataContext.AppUsers.Add(user);
           await _dataContext.SaveChangesAsync();
           return new UserDto{
            UserName=user.UserName,
            Token=_tokenService.CreateToken(user)
           };
        }

        private async Task<bool>Userexists(string UserName){
        return await  _dataContext.AppUsers.AnyAsync(x=>x.UserName.ToLower()==UserName.ToLower());
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
            {
               var user= await _dataContext.AppUsers.SingleOrDefaultAsync(x=>x.UserName.ToLower()==loginDto.UserName.ToLower());
               if(user==null)return Unauthorized("invalid username");
               using var hmac= new HMACSHA512(user.Passwordsalt);
               var ComputeHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
               for(int i=0;i<ComputeHash.Length;i++){
                   if(ComputeHash[i]!=user.PasswordHash[i]) return Unauthorized("invalid password");
               }
               return  new UserDto{
            UserName=user.UserName,
            Token=_tokenService.CreateToken(user)
           };
            }
    }
}
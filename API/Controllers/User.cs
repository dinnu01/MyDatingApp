using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{  
    public class User : Base
    {
        private readonly DataContext _dataContext;
       public User(DataContext dataContext)
       {
        _dataContext=dataContext;
       }
    [AllowAnonymous]
    [HttpGet]
      public async Task<ActionResult<List<AppUser>>>GetUsers(){

        return await _dataContext.AppUsers.ToListAsync();
      }

      [Authorize]
      [HttpGet("{id}")]
      public async Task<ActionResult<AppUser>>GetUser(int id)
      {
        return await _dataContext.AppUsers.FindAsync(id);
      }




    }
}
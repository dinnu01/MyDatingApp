using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{   [ApiController]
    [Route("api/[controller]")]
    public class User : ControllerBase
    {
        private readonly DataContext _dataContext;
       public User(DataContext dataContext)
       {
        _dataContext=dataContext;
       }
    
    [HttpGet]
      public async Task<ActionResult<List<AppUser>>>GetUsers(){

        return await _dataContext.AppUsers.ToListAsync();
      }
      [HttpGet("{id}")]
      public async Task<ActionResult<AppUser>>GetUser(int id)
      {
        return await _dataContext.AppUsers.FindAsync(id);
      }




    }
}
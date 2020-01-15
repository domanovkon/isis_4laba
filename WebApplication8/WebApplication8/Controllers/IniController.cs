using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication8.Models;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication8.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IniController : Controller
    {
        Ini_FilesContext db;
        public IniController(Ini_FilesContext context)
        {
            db = context;
            if (!db.Ini_Files1.Any())
            {
                db.Ini_Files1.Add(new Ini_Files { Name = "ini_2", Value_Button1 = "Старт", Value_Button2 = "Загрузить текст", Value_Button1_Color = "Color[Control]", Value_Button2_Color = "Color[Control]" });
                db.Ini_Files1.Add(new Ini_Files { Name = "ini_4", Value_Button1 = "Старт", Value_Button2 = "Загрузить текст", Value_Button1_Color = "Color[Control]", Value_Button2_Color = "Color[Control]" });
                db.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ini_Files>>> Get()
        {
            return await db.Ini_Files1.ToListAsync();
        }

        // GET api/ini_files1/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ini_Files>> Get(int id)
        {
            Ini_Files ini = await db.Ini_Files1.FirstOrDefaultAsync(x => x.Id == id);
            if (ini == null)
            {
                return NotFound();
            }
            return new ObjectResult(ini);
        }

        // POST api/ini_files1
        [HttpPost]
        public async Task<ActionResult<Ini_Files>> Post(Ini_Files ini)
        {
            if (ini == null)
            {
                return BadRequest();
            }
            db.Ini_Files1.Add(ini);
            await db.SaveChangesAsync();
            return Ok(ini);
        }

        // PUT api/ini_files1/
        [HttpPut]
        public async Task<ActionResult<Ini_Files>> Put(Ini_Files ini)
        {
            if (ini == null)
            {
                return BadRequest();
            }
            if (!db.Ini_Files1.Any(x => x.Id == ini.Id))
            {
                return NotFound();
            }
            db.Update(ini);
            await db.SaveChangesAsync();
            return Ok(ini);
        }

        // DELETE api/ini_files1/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Ini_Files>> Delete(int id)
        {
            Ini_Files ini = db.Ini_Files1.FirstOrDefault(x => x.Id == id);
            if (ini == null)
            {
                return NotFound();
            }
            db.Ini_Files1.Remove(ini);
            await db.SaveChangesAsync();
            return Ok(ini);
        }
    }
}

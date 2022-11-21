using Fast_Excel_Save.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Fast_Excel_Save.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            
            return View();
        }

      
        public async Task<IActionResult> upload(IFormFile files)
        {
            if (files==null)
            {
                return Ok("plase select excell file!");
            }
            try
            {
                if (files.Length > 0)
                {
                    var filePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Upload", files.FileName));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await files.CopyToAsync(stream);
                    }
                }
            }
            catch (Exception)
            {

                return Ok("false");
            }
           
            var main = new Fast_Excel_Save_Lib.Main();
           var test= main.Excel_Saving("./upload/"+files.FileName);
            return Ok(test);
        }
    }
}

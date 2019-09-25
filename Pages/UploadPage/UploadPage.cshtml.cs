using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using System.IO;
using Microsoft.AspNetCore.Http;


namespace Sharer.Pages.UploadPage
{
    public class UploadPageModel : PageModel
    {

        public void OnGet()
        {
            
        }

        

        public async Task<IActionResult> OnPost()
        {
            string addition = HttpContext.Request.Form["addition_text"];

            // save files
            Directory.CreateDirectory("./uploads.user");
            IFormFileCollection files = HttpContext.Request.Form.Files;
            foreach (var file in files)
            {
                if (file.Length > 0 && file.Length < 1024 * 1024 * 50)
                {
                    // var filePath = Path.Combine("./uploads", string.Format("{0}_{1}", file.FileName, DateTime.Now.ToString("MMM ddd d HH:mm yyyy")));
                    var filePath = Path.Combine("./uploads.user", string.Format("{1}_{2}_{0}", file.FileName, DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss"), addition));
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                }
            }

            Console.WriteLine("___Message From sender___");
            Console.WriteLine(addition);
            Console.WriteLine("----- Message Ends -----");
            Console.WriteLine();

            return Page();
        }
    }
}

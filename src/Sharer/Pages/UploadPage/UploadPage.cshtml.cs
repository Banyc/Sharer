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
    [RequestFormLimits(MultipartBodyLengthLimit = 1024 * 1024 * 1024)]
    [RequestSizeLimit(1024 * 1024 * 1024)]
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
                    // for security, any user input should not be taken as file name.
                    var filePath = Path.Combine("./uploads.user", string.Format("{0}.{1}{2}", DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss"), Guid.NewGuid(), Path.GetExtension(file.FileName)));
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);

                        // fileStream.Seek(0, SeekOrigin.Begin);

                        //     System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(fileStream);
                        //     SetClipboard(bitmap);
                        // try
                        // {
                        // }
                        // catch (Exception e)
                        // {

                        // }

                    }

                }
            }

            Console.WriteLine("___Message From sender___");
            Console.WriteLine(addition);
            Console.WriteLine("----- Message Ends -----");
            Console.WriteLine();

            return Page();
        }

        // private void SetClipboard(System.Drawing.Bitmap bitmap)
        // {
        //     // Assembly assembly = Assembly.LoadFrom("./ClipboardHelper.dll");
        //     // Type type = assembly.GetType("ClipboardHelper.SetClipboard");
        //     // object instanceOfSetClipboard = Activator.CreateInstance(type);

        //     // MethodInfo[] methods = type.GetMethods();
        //     // object res = methods[1].Invoke(instanceOfSetClipboard, new object[] {bitmap});

        //     ClipboardHelper.SetClipboard helper = new ClipboardHelper.SetClipboard();
        //     helper.AddImage(bitmap);
        // }
    }
}

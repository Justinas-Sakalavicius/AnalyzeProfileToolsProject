using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ProfileSample.DAL;
using ProfileSample.Models;

namespace ProfileSample.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            using (var context = new ProfileSampleEntities())
            {
                var sources = await context.ImgSources.Take(20).ToListAsync();

                var model = sources.Select(item => new ImageModel
                {
                    Name = item.Name,
                    Data = item.Data
                }).ToList();

                return View(model);
            }
        }

        public async Task<ActionResult> Convert()
        {
            var files = Directory.GetFiles(Server.MapPath("~/Content/Img"), "*.jpg");

            using (var context = new ProfileSampleEntities())
            {
                foreach (var file in files)
                {
                    context.ImgSources.Add(
                        new ImgSource
                        {
                            Name = Path.GetFileName(file),
                            Data = System.IO.File.ReadAllBytes(file),
                        });
                }

                await context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
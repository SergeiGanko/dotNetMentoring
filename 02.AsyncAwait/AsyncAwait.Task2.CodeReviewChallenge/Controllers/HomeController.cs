using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AsyncAwait.CodeReviewChallenge.Services;
using AsyncAwait.CodeReviewChallenge.Models.Help;
using AsyncAwait.CodeReviewChallenge.Models;

namespace AsyncAwait.Task2.CodeReviewChallenge.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Privacy()
        {
            ViewBag.Message = await DataService.GetPrivacyDataAsync();
            return View();
        }

        public async Task<IActionResult> Help()
        {
            IAssistent assistent = new ManualAssistent();
            ViewBag.RequestInfo = await assistent.RequestAssistanceAsync(HttpContext.Request.Path);
            return View();
        }

        public async Task<IActionResult> Faq()
        {
            IAssistent assistent = new FaqAssistent();
            ViewBag.RequestInfo = await assistent.RequestAssistanceAsync(HttpContext.Request.Path);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

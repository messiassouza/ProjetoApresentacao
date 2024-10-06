using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoApresentacao.Models;

namespace ProjetoApresentacao.Controllers
{
    [Authorize]
    public class ProdutoController : Controller
    {


        public IActionResult Index()
        {
            return View();
        } 
    }
}

using Modelo.Tabelas;
using Persistencia.Contexts;
using Servico.Tabelas;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class CategoriasController : Controller
    {
        private CategoriaServico categoriaServico = new CategoriaServico();
        private readonly EFContext context = new EFContext();
        public ActionResult Index()
        {
            return View(categoriaServico.ObterCategoriasClassificadasPorNome());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                context.Categorias.Add(categoria);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();

        }
        public ActionResult Edit(long? id)
        {
            PopularViewBag(categoriaServico.ObterCategoriaPorId((long)id));
            return ObterVisaoCategoriaPorId(id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                context.Entry(categoria).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categoria);
        }
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categoria categoria = context.Categorias.Where(c => c.CategoriaId == id).
            Include("Produtos.Fabricante").First();
            if (categoria == null)
            {
                return HttpNotFound();
            }
            return View(categoria);
        }
        //public ActionResult Delete(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Categoria categoria = context.Categorias.Find(id);
        //    if (categoria == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(categoria);
        //}
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Categoria categoria = categoriaServico.EliminarCategoriaPorId(id);
                TempData["Message"] = "Categoria " + categoria.Nome.ToUpper() + " foi removido";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(long id)
        {
            Categoria categoria = context.Categorias.Find(id);
            context.Categorias.Remove(categoria);
            context.SaveChanges();
            TempData["Message"] = "Categoria " + categoria.Nome.ToUpper() + " foi removido";
            return RedirectToAction("Index");
        }

    }
}
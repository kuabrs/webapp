using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using WebApp.Models;
using System.Net;
using System.IO;
using Modelo.Cadastros;
using Servico.Cadastros;
using Servico.Tabelas;

namespace WebApp.Controllers
{
    public class ProdutosController : Controller
    {

        //private EFContext context = new EFContext();
        private ProdutoServico produtoServico = new ProdutoServico();
        private CategoriaServico categoriaServico = new CategoriaServico();
        private FabricanteServico fabricanteServico = new FabricanteServico();
        // GET: Produtos
        public ActionResult Index()
        {
            //var produtos =
            //context.Produtos.Include(c => c.Categoria).Include(f => f.Fabricante).
            //OrderBy(n => n.Nome);
            //return View(produtos);
            return View(produtoServico.ObterProdutosClassificadosPorNome());
        }

        // GET: Produtos/Details/5
        public ActionResult Details(long? id)
        {
            return ObterVisaoProdutoPorId(id);
        }
        private ActionResult ObterVisaoProdutoPorId(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = produtoServico.ObterProdutoPorId((long)id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // GET: Produtos/Create
        public ActionResult Create()
        {
                PopularViewBag();
                return View();
        }

        // POST: Produtos/Create
        [HttpPost]
        public ActionResult Create(Produto produto)
        {
            return GravarProduto(produto);
        }

        // GET: Produtos/Edit/5
        public ActionResult Edit(long? id)
        {
            PopularViewBag(produtoServico.ObterProdutoPorId((long)id));
            return ObterVisaoProdutoPorId(id);
        }
        
        //public void GravarProduto(Produto produto)
        //{
        //    if (produto.ProdutoId == null)
        //    {
        //        context.Produtos.Add(produto);
        //    }
        //    else
        //    {
        //        context.Entry(produto).State = EntityState.Modified;
        //    }
        //    context.SaveChanges();
        //}
        // Metodo Privado
        private ActionResult GravarProduto(Produto produto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    produtoServico.GravarProduto(produto);
                    return RedirectToAction("Index");
                }
                PopularViewBag(produto);
                return View(produto);
            }
            catch
            {
                return View(produto);
            }
        }

        // Metodo Privado
        private void PopularViewBag(Produto produto = null)
        {
            if (produto == null)
            {
                ViewBag.CategoriaId = new SelectList(categoriaServico.ObterCategoriasClassificadasPorNome(),
                "CategoriaId", "Nome");
                ViewBag.FabricanteId = new SelectList(fabricanteServico.ObterFabricantesClassificadosPorNome(),
                "FabricanteId", "Nome");
            }
            else
            {
                ViewBag.CategoriaId = new SelectList(categoriaServico.ObterCategoriasClassificadasPorNome(),
                "CategoriaId", "Nome", produto.CategoriaId);
                ViewBag.FabricanteId = new SelectList(fabricanteServico.ObterFabricantesClassificadosPorNome(),
                "FabricanteId", "Nome", produto.FabricanteId);
            }
        }
//        private ActionResult GravarProduto(Produto produto,
//HttpPostedFileBase logotipo, string chkRemoverImagem)
//        {
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    if (chkRemoverImagem != null)
//                    {
//                        produto.Logotipo = null;
//                    }
//                    if (logotipo != null)
//                    {
//                        produto.LogotipoMimeType = logotipo.ContentType;
//                        produto.Logotipo = SetLogotipo(logotipo);
//                        produto.NomeArquivo = logotipo.FileName;
//                        produto.TamanhoArquivo = logotipo.ContentLength;
//                    }
//                    GravarProduto(produto);
//                    return RedirectToAction("Index");
//                }
//                PopularViewBag(produto);
//                return View(produto);
//            }
//            catch
//            {
//                PopularViewBag(produto);
//                return View(produto);
//            }
//        }

        // POST: Produtos/Edit/5
        //[HttpPost]
        //public ActionResult Edit(Produto produto, HttpPostedFileBase logotipo = null, string chkRemoverImagem = null)
        //{
        //    return GravarProduto(produto, logotipo, chkRemoverImagem);
        //    //try
        //    //{
        //    //    if (ModelState.IsValid)
        //    //    {
        //    //        context.Entry(produto).State = EntityState.Modified;
        //    //        context.SaveChanges();
        //    //        return RedirectToAction("Index");
        //    //    }
        //    //    return View(produto);
        //    //}
        //    //catch
        //    //{
        //    //    return View(produto);
        //    //}
        //}


        // POST: Produtos/Edit/5
        [HttpPost]
        public ActionResult Edit(Produto produto)
        {
            return GravarProduto(produto);
        }
        // GET: Produtos/Delete/5
        public ActionResult Delete(long? id)
        {
            return ObterVisaoProdutoPorId(id);
        }

        // POST: Produtos/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        Produto produto = context.Produtos.Find(id);
        //        context.Produtos.Remove(produto);
        //        context.SaveChanges();
        //        TempData["Message"] = "Produto " + produto.Nome.ToUpper() + " foi removido";
        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // POST: Produtos/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Produto produto = produtoServico.EliminarProdutoPorId(id);
                TempData["Message"] = "Produto " + produto.Nome.ToUpper() + " foi removido";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult DownloadArquivo(long id)
        {
            Produto produto = produtoServico.ObterProdutoPorId(id);
            FileStream fileStream = new FileStream(Server.MapPath(
            "~/App_Data/" + produto.NomeArquivo), FileMode.Create,
            FileAccess.Write);
            fileStream.Write(produto.Logotipo, 0,
            Convert.ToInt32(produto.TamanhoArquivo));
            fileStream.Close();
            return File(fileStream.Name, produto.LogotipoMimeType, produto.NomeArquivo);
        }
        public FileContentResult GetLogotipo(long id)
        {
            Produto produto = produtoServico.ObterProdutoPorId(id);
            if (produto != null)
            {
                return File(produto.Logotipo, produto.LogotipoMimeType);
            }
            return null;
        }
        public FileContentResult GetLogotipo2(long id)
        {
            Produto produto = produtoServico.ObterProdutoPorId(id);
            if (produto != null)
            {
                if (produto.NomeArquivo != null)
                {
                    var bytesLogotipo = new byte[produto.TamanhoArquivo];
                    FileStream fileStream = new
                    FileStream(Server.MapPath("~/App_Data/" + produto.NomeArquivo), FileMode.Open,
                    FileAccess.Read);
                    fileStream.Read(bytesLogotipo, 0, (int)produto.TamanhoArquivo);
                    return File(bytesLogotipo, produto.LogotipoMimeType);
                }
            }
            return null;
        }
    }
}

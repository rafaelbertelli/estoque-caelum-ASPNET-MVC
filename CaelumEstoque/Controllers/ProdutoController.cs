using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaelumEstoque.DAO;
using CaelumEstoque.Models;
using CaelumEstoque.Filters;

namespace CaelumEstoque.Controllers
{
    [AutorizacaoFilterAttribute]
    public class ProdutoController : Controller
    {
        [Route("Produtos", Name = "listaProdutos")]
        public ActionResult Index()
        {
            ProdutosDAO dao = new ProdutosDAO();
            IList<Produto> produtos = dao.Lista();
            //ViewBag.Produtos = produtos;

            return View(produtos);
        }

        public ActionResult Form()
        {
            ViewBag.Produto = new Produto();
            CategoriasDAO dao = new CategoriasDAO();
            IList<CategoriaDoProduto> categorias = dao.Lista();
            ViewBag.Categorias = categorias;
            
            return View(categorias);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Adiciona(Produto produto)
        {
            int idInformatica = 1;
            double valorCorte = 100;

            if (produto.CategoriaId.Equals(idInformatica) && produto.Preco <= 100)
            {
                ModelState.AddModelError("produto.Invalido", string.Format("Produto de informática com preço abaixo de R$ {0}", valorCorte));
            }

            if(ModelState.IsValid)
            {
                ProdutosDAO dao = new ProdutosDAO();
                dao.Adiciona(produto);

                return RedirectToAction("Index", "Produto");
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);

                ViewBag.Produto = produto;
                CategoriasDAO categoriasDao = new CategoriasDAO();
                ViewBag.Categorias = categoriasDao.Lista();

                return View("Form");
            }

        }

        [Route("produtos/{id}", Name = "visualizaProduto")]
        public ActionResult Visualiza(int id)
        {
            ProdutosDAO dao = new ProdutosDAO();
            Produto produto = dao.BuscaPorId(id);
            ViewBag.Produto = produto;
            return View(produto);

        }

        public ActionResult DecrementaQtd(int id)
        {
            ProdutosDAO dao = new ProdutosDAO();
            Produto produto = dao.BuscaPorId(id);
            produto.Quantidade--;
            dao.Atualiza(produto);
            //return RedirectToAction("Index");
            return Json(produto);
        }


    }
}
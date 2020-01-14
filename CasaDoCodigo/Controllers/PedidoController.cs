using CasaDoCodigo.Models;
using CasaDoCodigo.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Controllers
{


    public class PedidoController : Controller
    {
        private readonly IDataService _dataService;
        public PedidoController(IDataService dataService)
        {
            this._dataService = dataService;
        }
        public IActionResult Carrossel()
        {
            IList<Produto> produtos = _dataService.GetProdutos();
            return View(produtos);
        }

        public IActionResult Carrinho(int? produtoId) //Adicionaremos ? após o int, o que fará com que o produtoId possa ser um valor nulo. Com isso, não podemos simplesmente adicionar um produtoId ao AddItemPedido(). Temos que acessar o valor de um tipo anulável.
        { 
            if (produtoId.HasValue) //Para verificar se o produtoId tem valor ou não, utilizaremos um If(). Se o produtoId possuir valor, adicionaremos o item do pedido:...

            {

            _dataService.AddItemPedido(produtoId.Value);

            }

            CarrinhoViewModel viewModel = GetCarrinhoViewModel();

            return View(viewModel);
        }

        private CarrinhoViewModel GetCarrinhoViewModel()
        {
            List<Produto> produtos = this._dataService.GetProdutos();

            var itensCarrinho = this._dataService.GetItensPedido();

            CarrinhoViewModel viewModel = 
                new CarrinhoViewModel(itensCarrinho);
            return viewModel;
        }

        public IActionResult Cadastro()
        {
            var pedido = _dataService.GetPedido();

            if (pedido == null)
            {
                return RedirectToAction("Carrossel");
            }
            else
            {
                return View(pedido);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken] //Utilizando a chave ValidateAntiForgeryToken, evitamos o ataque CSRF! Ela funciona como um filtro, verificando se o cookie está presente, e se o valor do campo hidden bate com o último valor do cookie.
        public IActionResult Resumo(Pedido cadastro)
        {
            if (ModelState.IsValid)
            {
                var pedido = _dataService.UpdateCastro(cadastro);

                return View(pedido);
            }
            else
            {
                return RedirectToAction("Cadastro");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //O atributo FromBody, como o próprio nome já diz, "algo vem do corpo", então entende-se que o que virá do corpo serão os dados linkados no parâmetro input....
        public UpdateItemPedidoResponse PostQuantidade([FromBody]ItemPedido input) //pedidoController.cs, temos o PostQuantidade(), e nesse método precisamos pegar a entrada (input) e atualizar a quantidade no database, 
            //Para isso, temos que acessar a classe de serviços de dados, e chamar o método do dataservice para atualizar o item de pedido....
        {
            return _dataService.UpdateItemPedido(input);
        }
    }
}

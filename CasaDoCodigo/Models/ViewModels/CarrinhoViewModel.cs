using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//Conseguimos que CarrinhoViewModel seja o responsável por calcular o total e centralizamos o cálculo do valor total do carrinho.Se houver
//alguma alteração, faremos no CarrinhoViewModel.
namespace CasaDoCodigo.Models.ViewModels
{
    public class CarrinhoViewModel
    {
        public List<ItemPedido> Itens { get; private set; }
        //decimal, e receberá o nome de "Total". Ela retornará a conta que fizemos da soma dos itens do pedido
        public decimal Total 
        {
            get
            {
                return Itens.Sum(i => i.Subtotal);
            }
        }

        public CarrinhoViewModel(List<ItemPedido> itens)
        {
            this.Itens = itens;
        }
    }
}

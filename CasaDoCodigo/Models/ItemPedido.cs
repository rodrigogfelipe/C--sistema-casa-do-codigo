using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CasaDoCodigo.Models
{

    public class ItemPedido : BaseModel
    {
        [DataMember]
        [Required] //adicionaremos outro atributo à propriedade Pedido, o qual exigirá que o pedido seja obrigatório.
        public Pedido Pedido { get; private set; } //Será necessário modificar o ItemPedido para que ele aponte para esse pedido, e um relacionamento do Item com o Pedido à qual ele faz referência.
        [DataMember]
        [Required]
        public Produto Produto { get; private set; }
        [DataMember]
        public int Quantidade { get; private set; }
        [DataMember]
        public decimal PrecoUnitario { get; private set; }
        [DataMember]
        public decimal Subtotal {

            get
            {
                return Quantidade * PrecoUnitario;
            }
        }

        public ItemPedido()
        {

        }

        //Faremos um encadeamento de construtores, ou seja, o primeiro construtor irá chamar o segundo, e será passado os parâmetros
        //produto e quantidade, e os dois construtores funcionarão corretamente.
        public ItemPedido(int id, Pedido pedido, Produto produto,
            int quantidade) : this(pedido, produto, quantidade)
        {
            this.Id = id;
        }

        public ItemPedido(Pedido pedido, Produto produto,
            int quantidade)
        {
            this.Pedido = pedido;
            this.Produto = produto;
            this.Quantidade = quantidade;
            this.PrecoUnitario = produto.Preco;
        }

        public void AtualizaQuantidade(int quantidade) //criaremos o método AtualizaQuantidade(), passando um inteiro que é a quantidade nova. Em seguida, vamos settar a quantidade do objeto.
        {
            this.Quantidade = quantidade;
        }
    }
}




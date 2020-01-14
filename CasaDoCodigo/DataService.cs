using CasaDoCodigo.Models;
using CasaDoCodigo.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

//Criaremos um novo método que fará os inserts dos dados nas tabelas de Produto e ItemPedido. Essa nova classe, será um serviço de dados, e vamos chamá-la de DataService.cs.

namespace CasaDoCodigo
{
    //Para acessar o banco de dados através do Entity Framework, primeiro precisamos ter acesso ao contexto. Uma maneira muito boa de ter
    //acesso ao contexto é receber o contexto através do construtor da classe DataService.cs.

    public class DataService : IDataService
    {
        private readonly Contexto _contexto; // readolnly leitura
        private readonly IHttpContextAccessor _contextAccessor;

        public DataService(Contexto contexto,
            IHttpContextAccessor contextAccessor) //Vamos armazenar esse contexto dentro de um campo somente leitura
        {
            this._contexto = contexto;
            this._contextAccessor = contextAccessor;
        }

        public void AddItemPedido(int produtoId)
        {
            var produto =
                _contexto.Produtos
                .Where(p => p.Id == produtoId) //Em Where(), foi passada uma expressão lambda que usará somente um produto da coleção..... 
                .SingleOrDefault(); // O SingleOrDefault() irá trazer apenas um produto ou um valor nulo.....

            if (produto != null) //Após a condição, se o produto não for nulo, teremos que chegar ao pedido do banco de dados a partir do contexto.
            {
                int? pedidoId = GetSessionPedidoId();

                Pedido pedido = null;
                if (pedidoId.HasValue)
                {
                    pedido = _contexto.Pedidos
                        .Where(p => p.Id == pedidoId.Value)
                        .SingleOrDefault();
                }

                if (pedido == null)
                    pedido = new Pedido(); //Agora precisaremos tratar a possibilidade do pedido ser nulo e, se este for o caso, criaremos uma nova instância de pedido.

                if (!_contexto.ItensPedido
                    .Where(i =>
                        i.Pedido.Id == pedido.Id
                        && i.Produto.Id == produtoId)
                    .Any()) //Com Any() encontraremos "qualquer", de modo que se existir qualquer item de pedido com tal produto, sabemos que poderemos inserir um item se não tivermos nenhum item de pedido com esse mesmo produto. O contrário de any (qualquer) é nenhum!
                {
                    _contexto.ItensPedido.Add(
                        new ItemPedido(pedido, produto, 1));

                    _contexto.SaveChanges();

                    SetSessionPedidoId(pedido);
                }
            }
        }

        private void SetSessionPedidoId(Pedido pedido)
        {
            _contextAccessor.HttpContext
                .Session.SetInt32("pedidoId", pedido.Id); //A partir do _contextAcessor, acessamos o HttpContext, que armazenará a sessão. GetInt32() trará um valor inteiro, o qual passamos como "chave", para que teremos o valor do pedidoId.
        }

        private int? GetSessionPedidoId()
        {
            return _contextAccessor.HttpContext
                .Session.GetInt32("pedidoId");
        }

        public List<ItemPedido> GetItensPedido()
        {
            var pedidoId = GetSessionPedidoId();
            var pedido = _contexto.Pedidos
                .Where(p => p.Id == pedidoId)
                .Single();

            return this._contexto.ItensPedido
                .Where(i => i.Pedido.Id == pedido.Id)
                .ToList();
        }

        public List<Produto> GetProdutos()
        {
            return this._contexto.Produtos.ToList();
        }
        //InicializaDB()! Antes de começar a inserir os dados, precisamos verificar se o banco de dados existe. Faremos isso primeiro acessando o contexto
        public void InicializaDB()
        {
            this._contexto.Database.EnsureCreated(); //criado com o método EnsureCreated(), porém, caso ele não exista, vamos criar um novo. Faremos isso através do objeto database:

            if (this._contexto.Produtos.Count() == 0) //no _contexto, e verificamos a quantidade de registros que estão na tabela com o método Count().Se a contagem estiver vazia, ou seja, se for igual a 0, será inserido.
            {
                List<Produto> produtos = new List<Produto>
                {
                    new Produto("Sleep not found", 59.90m),
                    new Produto("May the code be with you", 59.90m),
                    new Produto("Rollback", 59.90m),
                    new Produto("REST", 69.90m),
                    new Produto("Design Patterns com Java", 69.90m),
                    new Produto("Vire o jogo com Spring Framework", 69.90m),
                    new Produto("Test-Driven Development", 69.90m),
                    new Produto("iOS: Programe para iPhone e iPad", 69.90m),
                    new Produto("Desenvolvimento de Jogos para Android", 69.90m)
                };

                foreach (var produto in produtos)
                {
                    this._contexto.Produtos
                        .Add(produto);//Usamos o .Add(produto) para inserir no banco de dados, uma instância de produto. Após a inserção, precisamos adicionar cada um dos itens do pedido....

                    //this._contexto.ItensPedido
                    //    .Add(new ItemPedido(produto, 1));
                }

                this._contexto.SaveChanges();
            }
        }

        public UpdateItemPedidoResponse UpdateItemPedido(ItemPedido itemPedido)
        {
            var itemPedidoDB =
            _contexto.ItensPedido //Precisamos acessar a coleção de itens de pedido que está no _contexto do Entity Framework. ItensPedido tem essa coleção que virá do banco de dados.

                .Where(i => i.Id == itemPedido.Id) // o método .Where(). Esse método pertence à linguagem LINQ, e ele nos permite buscar qual é o item 
                //do pedido que será atualizado. O .Where() receberá uma expressão lambda, que tem 2 lados. O primeiro lado é o parâmetro da função, e o segundo lado é o acesso ao objeto que está sendo investigado.

                .SingleOrDefault(); //.SingleOrDefault(), retornamos um único elemento. Armazenaremos tudo isso em uma variável local itemPedidoDB...

            if (itemPedidoDB != null) //O próximo passo é verificar se o itemPedidoDB é nulo. Pois, só podemos atualizar no banco de dados se ele não for nulo....
            {
                itemPedidoDB.AtualizaQuantidade(itemPedido.Quantidade); // iremos chamar esse método para atualizar a quantiadade, passando o itemPedido.Quantidade.

                if (itemPedidoDB.Quantidade == 0) // Quantidade == 0 removera o itemPedido....
                    _contexto.ItensPedido.Remove(itemPedidoDB);

                _contexto.SaveChanges(); // Para salvar as atualizações no banco de dados, acessaremos:SavaChanges
            }

            var itensPedido = _contexto.ItensPedido.ToList();

            var carrinhoViewModel = new CarrinhoViewModel(itensPedido);

            return new UpdateItemPedidoResponse(itemPedidoDB, carrinhoViewModel);
        }

        public Pedido GetPedido() //GetPedido(), que obtém dados a partir da coleção de pedidos, cujo Id da coleção bate com o da sessão....
        {
            int? pedidoId = GetSessionPedidoId();

            return _contexto.Pedidos
                        .Include(p => p.Itens) //método chamado Include() a partir da coleção de pedidos. Este método faz parte de uma extensão que não está declarada nessa classe.
                        .ThenInclude(p => p.Produto) // produto está relacionado ao item do pedido, e para que ele seja incluído, é necessário utilizarmos o método ThenInclude(). Assim, depois que os itens forem colocados no pedido, os produtos também serão.
                        .Where(p => p.Id == pedidoId)
                        .SingleOrDefault();
        }

        public Pedido UpdateCastro(Pedido cadastro) //temos a atualização do cadastro, e o pedido atualizado deverá salvar estas informações no banco de dados....
        {
            var pedido = GetPedido();
            pedido.UpdateCadastro(cadastro);
            _contexto.SaveChanges();
            return pedido;
        }
    }
}

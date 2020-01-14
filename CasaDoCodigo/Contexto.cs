using CasaDoCodigo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo
{
    public class Contexto : DbContext
    {
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; } //adicionar os pedidos como uma coleção do contexto do Entity Framework, pois esta é uma maneira de se acessar a tabela de Pedidos. Se não o adicionarmos no contexto, não teremos nem como obter o valor de pedidos ou salvar os pedidos.
        public DbSet<ItemPedido> ItensPedido { get; set; }
        //parâmetro chamado DbContextOptions<TContext> para que ele consiga receber as opções de configuração do banco de dados
        public Contexto(DbContextOptions<Contexto> options) : base(options) 

        {

        }




    }
}

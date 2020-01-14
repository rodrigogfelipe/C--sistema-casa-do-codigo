class Carrinho {

    //clickIncremento() utiliza os dados, terá que incrementar a quantidade do objeto data. Em seguida, irá mandar a quantidade para o AJAX.
    clickIncremento(btn) {
        var data = this.getData(btn); //Iremos chamá-los na getData() - que significa pegar dados....
        data.Quantidade++;
        this.postQuantidade(data); //Chamaremos essa função de postQuantidade(), que receberá um objeto data que contém os dados que receberá o elemento clicado.....
    }
    //Da mesma forma, só com um detalhe diferente, será a função clickDecremento().
    clickDecremento(btn) {
        var data = this.getData(btn);
        data.Quantidade--;
        this.postQuantidade(data);
    }
    //recisaremos de um método que atualize a quantidade de acordo com o valor informado pelo usuário. Chamaremos essa nova função de updateQuantidade(), que receberá o elemento HTML alterado: o input....
    updateQuantidade(input) {
        //nstanciar o data, passando o input como parâmetro. Em seguida, passaremos o objeto data para ser atualizado
        var data = this.getData(input);
        this.postQuantidade(data);
    }

    getData(elemento) {
        var linhaDoItem = $(elemento).parents('[item-id]'); //A função parents() recebe também um parâmetro, através do qual podemos filtrar em qual elemento queremos obter. No nosso caso, não queremos chegar em qualquer elemento ou usar todos os pais. Queremos somente o elemento que contém certo atributo colocado para o ARROBa item.Id.
        var itemId = linhaDoItem.attr('item-id');
        var qtde = linhaDoItem.find('input').val();

        //return para que ele retorne esse objeto:
        return { 
            Id: itemId,
            Quantidade: qtde
        };
    }

    postQuantidade(data) {

        var token = $('input[name=__RequestVerificationToken]').val(); //Para usarmos este campo oculto na página, teremos que obter o input através de name="__RequestVerificationToken".....
        var header = {}; //Após obtermos o token de verificação, o armazenaremos em um header (cabeçalho) da requisição:
        header['RequestVerificationToken'] = token; //

        //O método .ajax(), recebe alguns parâmetros para a chamada ao servidor....
        $.ajax({
            url: '/pedido/PostQuantidade', //URL, serviço a ser chamado no servidor,
            type: 'POST', //type, que é o tipo de requisição,
            contentType: 'application/json',
            data: JSON.stringify(data), //data em JSON. Para usar esses dados, foi criado um objeto var data que recebe o ID e a quantidade. Só que para passar esse novo objeto data para o atributo data, temos que serializá-lo por meio do JSON
            headers: header

        }).done(function (response) {
            this.setQuantidade(response.itemPedido);
            this.setSubtotal(response.itemPedido);
            this.setTotal(response.carrinhoViewModel);
            this.setNumeroItens(response.carrinhoViewModel);
            //Colocaremos a função de exclusão de item, caso a atualização de quantidade chegar a 0.
            if (response.itemPedido.quantidade == 0)
                this.removeItem(response.itemPedido);
        }.bind(this));
    }

    setQuantidade(itemPedido) {
        this.getLinhaDoItem(itemPedido)
            .find('input').val(itemPedido.quantidade);
    }

    //atributo "Subtotal", e para encontrar um elemento com certo atributo, chamamos a função jQuery .find() e, logo após, a função .html(), usada para 
    //alterar o conteúdo HTML passando o itemPedido.subtotal
    setSubtotal(itemPedido) {
        this.getLinhaDoItem(itemPedido) //getLinhaDoItem() irá receber o itemPedido, e retornará o elemento HTML do carrinho.
            .find('[subtotal]').html(itemPedido.subtotal.duasCasas());
    }

    //setTotal() utilizará um "carrinhoViewModel" com todos os dados do carrinho, e em seguida acessaremos o elemento que contém o total geral do pedido...
    setTotal(carrinhoViewModel) {
        $('[total]').html(carrinhoViewModel.total.duasCasas());
    }

    getLinhaDoItem(itemPedido) {
        return $('[item-id=' + itemPedido.id + ']');
    }

    //Nessa função passamos o item pedido como parâmetro, e utilizamos o jQuery para fazer a remoção. Precisaremos chegar à linha do item selecionado utilizando-se getLinhaDoItem() que recebe itemPedido. O .remove() é o método jQuery.
    removeItem(itemPedido) {
        this.getLinhaDoItem(itemPedido).remove();
    }

    //texto foi quebrado em vários pedaços, para que essa atualização da quantidade total de itens seja alterada automaticamente.....
    setNumeroItens(carrinhoViewModel) {
        var texto =
            'Total: '
            + carrinhoViewModel.itens.length
            + ' '
            + (carrinhoViewModel.itens.length > 1 //olocamos a quantidade de itens do carrinho, que está em carrinhoViewModel.itens.length. A palavra "item" pode ser singular ou plural dependendo da quantidade de itens. Por isso, utilizamos um operador ternário.
                ? 'itens' : 'item');
        $('[numero-itens]').html(texto);
    }
}


var carrinho = new Carrinho();
//A função .toFixed() força um formato, no nosso caso, com duas casas decimais. Em seguida, o . é trocado por , utilizando-se a função de troca .replace()...
Number.prototype.duasCasas = function () {
    return this.toFixed(2).replace('.', ',');
}














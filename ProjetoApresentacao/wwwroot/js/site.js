// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



var tableProdutos = new DataTable('#tableProdutos', {
    dom: 'Bfrtip',
    buttons: [
        {
            text: ' Novo Produto ',
            key: {
                key: 'N',
                altKey: true
            },
            action: function (e, dt, node, config) {
                e.preventDefault();
                // window.location.href = '/Produto/ProdutoNovo';




                $('#modal-Produto').iziModal('open');

            }
        }
    ],
    "order": [[0, 'asc']],
    "responsive": true,
    "lengthChange": false,
    "autoWidth": false,
    "searching": true,
    "ajax": {
        "url": '/api/Produtos',
        "type": "GET",
        "dataType": "json",
        "dataSrc": function (json) {
            return json;
        }
    },
    "columns": [
        { "data": "id", "title": "ID", width: 50 },
        { "data": "nome", "title": "Nome", width: 200 },
        { "data": "descricao", "title": "Descrição" },
        { "data": "preco", "title": "Preço", render: function (data) { return data.toFixed(2); } },
        { "data": "dataCriacao", "title": "Data de Criação", width: 100, render: function (data) { return new Date(data).toLocaleDateString(); } },
        {
            render: function (data, type, row) {
                return `
                    <div class="row">                       

                        <button style="width:28px;height:28px;padding:0px;margin:1px;" class="btn btn-primary btn-edit" type="button">
                            <i class="fa-solid fa-pencil fa-2xs"></i>
                        </button>                     

                        <button style="width:28px;height:28px;padding:0px;margin:1px;" class="btn btn-warning btn-delete" type="button">
                            <i class="fa-solid fa-trash fa-2xs"></i>
                        </button>

                    </div>
                `;
            },
            title: 'Ações',width: 30 
        }
    ]
});

$('#createProductForm').on('submit', function (e) {
    e.preventDefault(); 
    var produto = {
        Nome: $('#Nome').val(),
        Descricao: $('#Descricao').val(),
        Preco: parseFloat($('#Preco').val()), 
        DataCriacao: $('#DataCriacao').val()
    };

    console.log(produto);

    $.ajax({
        url: '/api/Produtos',  
        method: 'POST',  
        contentType: 'application/json',  
        data: JSON.stringify(produto),  
        success: function (response) { 
            alert('Produto criado com sucesso!');
            window.location.href = '/Produto';
        },
        error: function () {
            alert('Erro ao criar o produto. Verifique os dados.');
        }
    });



});


$('#tableProdutos').on('click', '.btn-edit', function () {

    var produtoData = tableProdutos.row($(this).parents('tr')).data(); 
    var formElement = document.getElementById("FormDadosProduto");     

     
    formElement['Id'].value = produtoData.id;  
    formElement['nome'].value = produtoData.nome;  
    formElement['descricao'].value = produtoData.descricao;  
    formElement['preco'].value = produtoData.preco;  

    if (produtoData.dataCriacao) { 
        formElement['dataCriacao'].value = new Date(produtoData.dataCriacao).toISOString().split('T')[0];
    } else { 
        formElement['dataCriacao'].value = '';  
    }



    $('#modal-Produto').iziModal('open');
});


$('#tableProdutos').on('click', '.btn-delete', function () {

    console.log(tableProdutos.row($(this).parents('tr')).data());

    $.ajax({
        url: '/api/Produtos/' + tableProdutos.row($(this).parents('tr')).data().id,  
        type: 'DELETE',
        contentType: "application/json",
        success: function (data, textStatus, xhr) {

            Swal.fire({
                title: 'Deletado!',
                text: "Registro deletado com sucesso!",
                icon: 'success',
            }).then((result) => {
                window.location.reload();
                // Lógica após o sucesso
            });
        },
        error: function (xhr, textStatus, errorThrown) {


            console.log(textStatus);

            Swal.fire({
                title: 'Erro!',
                text: "Registro não foi deletado!",
                icon: 'error',
            }).then((result) => {
                window.location.reload();
                // Lógica após o erro
            });
        }
    });

 

}); 
 
function getFormData(formElement) {
    var postData = {};
    for (var i = 0; i < formElement.length; i++) {
        if (['INPUT', 'TEXTAREA', 'SELECT'].includes(formElement[i].tagName)) {
            var field = formElement[i];
            if (field.type === 'checkbox') {
                postData[field.id] = field.checked;
            } else if (field.type === 'number') {
                postData[field.id] = parseFloat(field.value) || 0;
            } else {
                postData[field.id] = field.value;
            }
        }
    }
    return postData;
}



 
function postProdutoData(postData) {

    console.log(postData);

    $.ajax({
        url: 'api/Produtos',  
        type: 'POST',
        contentType: 'application/json', 
        data: JSON.stringify(postData),  
        success: function (response) {
            Swal.fire({
                title: 'Produto criado!',
                text: "O produto foi criado/atualizado com sucesso!",
                icon: 'success',
            }).then(() => {
                $('#modal-Produto').iziModal('open');
                window.location.reload();
            });
        },
        error: function (xhr) {
            Swal.fire({
                title: 'Erro!',
                text: `Falha ao salvar o produto. Status: ${xhr.status}, Mensagem: ${xhr.responseText}`,
                icon: 'error',
            });
        }
    });
}

 
function putProdutoData(postData) {
    console.log(postData);

    $.ajax({
        url: 'api/Produtos/' + postData.Id, 
        type: 'PUT',  
        contentType: 'application/json',  
        data: JSON.stringify(postData),  
        success: function (response) {
            Swal.fire({
                title: 'Produto atualizado!',
                text: "O produto foi atualizado com sucesso!",
                icon: 'success',
            }).then(() => {
                $('#modal-Produto').iziModal('open');
                window.location.reload();
            });
        },
        error: function (xhr) {
            Swal.fire({
                title: 'Erro!',
                text: `Falha ao atualizar o produto. Status: ${xhr.status}, Mensagem: ${xhr.responseText}`,
                icon: 'error',
            });
        }
    });
}

 
$('#bttSalvarProduto').click(function () {
    var formElement = document.getElementById("FormDadosProduto");
    var isEdit = formElement['Id'].value !== "0";  

    var confirmTitle = isEdit ? 'Confirma Alteração do Produto?' : 'Confirma Inclusão do Produto?';

 
    Swal.fire({
        title: confirmTitle,
        showDenyButton: true,
        confirmButtonText: 'Sim',
        denyButtonText: 'Não',
        customClass: {
            actions: 'my-actions',
            confirmButton: 'order-2',
            denyButton: 'order-3',
        }
    }).then((result) => {
        if (result.isConfirmed) {
            var postData = getFormData(formElement);  

            if (isEdit) {
                putProdutoData(postData); 
            } else {
                postProdutoData(postData);  
            }
        } else if (result.isDenied) {
            Swal.fire('Solicitação Cancelada', '', 'info');
        }
    });
});

$('#bttLogOut').click(function () {
    
    $.ajax({
        url: '/api/Auth/Logout', // Ajuste a URL conforme necessário
        type: 'GET',
        success: function (response) {
            // Redirecionar ou realizar alguma ação após o logout
            window.location.href = '/Home'; // Redireciona para a página inicial
        },
        error: function (xhr, status, error) {
            console.error("Erro ao fazer logout:", error);
        }
    });
});
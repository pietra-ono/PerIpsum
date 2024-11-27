// MODAL VISUALIZAÇÃO RASCUNHOS 

$('.rascunho-container').on('click', '.card', function () {

    var id = $(this).data('id');

    var imagem = $(this).find('#imagemCard').attr('src');
    var titulo = $(this).find('#titulo').text();
    var descricao = $(this).find('#descricao').text();
    var categoria = $(this).find('#categorias').text();
    var data = $(this).find('#data').text();
    var pais = $(this).find('#pais').text();
    var tipo = $(this).find('#tipo').text();
    var link = $(this).find('#link').text();
    var bandeira = $(this).find('.paisDesign img').attr('src');



    var modal = $('#rascunhoModal-' + id);
    modal.find('#imgModal').attr('src', imagem);
    modal.find('#nomeModal').text(titulo);
    modal.find('#descricaoModal').text(descricao);
    modal.find('#categoriasModal').text(categoria);
    modal.find('#dataModal').text(data);
    modal.find('#linkModal').attr('href', link);
    modal.find('#tipoModal').text(tipo).css('background-color', $(this).find('.tipoDesign').css('background-color'));
    modal.find('#paisModal').text(pais);
    modal.find('#bandeiraModal').attr('src', bandeira);

    modal.modal('show');
});



// Fechar modal ao clicar no botão de fechar
$('.btn-close').click(function () {
    $('.modal').modal('hide');
});

$(document).ready(function () {
    // Função de drag-and-drop para upload de imagens
    $('#imagem').on('dragover', function (e) {
        e.preventDefault();
        $(this).addClass('dragover');
    });

    $('#imagem').on('dragleave', function (e) {
        $(this).removeClass('dragover');
    });

    $('#imagem').on('drop', function (e) {
        e.preventDefault();
        $(this).removeClass('dragover');

        var files = e.originalEvent.dataTransfer.files;
        if (files.length > 0) {
            var file = files[0];
            var formData = new FormData();
            formData.append('imagem', file);

            $.ajax({
                type: 'POST',
                url: '/Admin/AdicionarRascunho',
                data: formData,
                contentType: false,
                processData: false,
                success: function (data) {
                    console.log('Upload realizado com sucesso!');
                    location.reload();
                },
                error: function (xhr, status, error) {
                    console.log('Erro ao realizar upload: ' + error);
                }
            });
        }
    });
});
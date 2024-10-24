$('.fundo').on('click', '.card', function () {

    var id = $(this).data('id');

    var imagem = $(this).find('#imagemCardRascunho').attr('src');
    var titulo = $(this).find('#textoTituloRascunho').text();
    var descricao = $(this).find('#dataTextoDescricaoRascunho').text();
    var data = $(this).find('#dataTextoRascunho').text();
    var pais = $(this).find('#paisTextoRascunho').text();
    var tipo = $(this).find('#tipoTextoRascunho').text();
    var link = $(this).find('#linkTextoRascunho').text();
    var bandeira = $(this).find('#paisDesignRascunho img').attr('src');



    var modal = $('#rascunhoModal-' + id);
    modal.find('#modalImageRascunho').attr('src', imagem);
    modal.find('#modalTitleRascunho').text(titulo);
    modal.find('#modalDescriptionRascunho').text(descricao);
    modal.find('#modalDataRascunho').text(data);
    modal.find('#modalLinkRascunho').attr('href', link);
    modal.find('#modalTipoRascunho').text(tipo).css('background-color', $(this).find('#tipoDesignRascunho').css('background-color'));
    modal.find('#modalPaisDisplayRascunho').text(pais);
    modal.find('#modalPaisFlagRascunho').attr('src', bandeira);


    modal.modal('show');
});
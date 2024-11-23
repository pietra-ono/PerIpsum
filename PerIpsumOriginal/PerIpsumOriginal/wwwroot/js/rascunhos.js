// MODAL VISUALIZAÇÃO RASCUNHOS 

$('.fundo').on('click', '.card', function () {

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



    var modal = $('#feedModal-' + id);
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
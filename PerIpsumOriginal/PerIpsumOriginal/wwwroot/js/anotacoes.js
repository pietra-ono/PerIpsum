// MODAL VISUALIZAÇÃO ANOTAÇÕES
$('.anotacoes-container').on('click', '.blocoAnotacao', function () {

    var id = $(this).data('id');

    var titulo = $(this).find('#tituloAnotacao').attr('src');
    var descricao = $(this).find('#descricaoAnotacao').attr('src');



    var modal = $('#anotVisuModal-' + id);

    modal.find('#modalTitulo').text(titulo);
    modal.find('#modalDescricao').text(descricao);

    modal.modal('show');
});

document.addEventListener('DOMContentLoaded', function () {
    const searchInput = document.getElementById('searchInputFeed');
    const blocos = document.querySelectorAll('.blocoAnotacao');

    searchInput.addEventListener('input', function () {
        const searchTerm = searchInput.value.toLowerCase();

        blocos.forEach(bloco => {
            const titulo = bloco.querySelector('#tituloAnotacao').textContent.toLowerCase();
            const descricao = bloco.querySelector('#descricaoAnotacao').textContent.toLowerCase();

            const matchesSearch = searchTerm === '' ||
                titulo.includes(searchTerm) ||
                descricao.includes(searchTerm);

            bloco.style.display = matchesSearch ? '' : 'none';
        });
    });
});

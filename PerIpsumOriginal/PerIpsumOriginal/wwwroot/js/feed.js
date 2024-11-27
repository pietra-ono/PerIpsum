// MODAL VISUALIZAÇÃO FEED

$('.feed-container').on('click', '.card', function (e) {
    if ($(e.target).closest('.favoritar-btn').length) {
        return;
    }

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
    modal.find('#categoriasModal').text('Palavras-chave: ' + categoria);
    modal.find('#dataModal').text(data);
    modal.find('#linkModal').attr('href', link);
    modal.find('#tipoModal').text(tipo).css('background-color', $(this).find('.tipoDesign').css('background-color'));
    modal.find('#paisModal').text(pais);
    modal.find('#bandeiraModal').attr('src', bandeira);

    modal.modal('show');
});

// Variável para armazenar o ID do conteúdo a ser excluído
let contentIdToDelete;

$('.feed-container').on('click', '#openDeleteModalBtn', function () {
    // Fecha o modal de visualização se ele estiver aberto
    $('.modal').modal('hide'); // Fecha todos os modais abertos, incluindo o de visualização

    // Armazena o ID do conteúdo
    contentIdToDelete = $(this).data('id'); // Armazena o ID do conteúdo
    $('#deleteModal').modal('show'); // Mostra o modal de exclusão
});

// Confirmar exclusão
$('#confirmDeleteBtn').click(function () {
    fetch(`/Usuario/ApagarConteudo/${contentIdToDelete}`, {
        method: 'POST' // Ou DELETE, dependendo da sua implementação
    })
        .then(response => {
            if (response.ok) {
                // Atualiza a página ou remove o card do feed
                location.reload(); // Recarrega a página
            } else {
                alert('Erro ao excluir o conteúdo.'); // Mensagem de erro
            }
        })
        .catch(error => {
            console.error('Erro na requisição:', error);
            alert('Erro ao processar a solicitação.');
        });
});


// Fechar modal ao clicar no botão de fechar
$('.btn-close').click(function () {
    $('.modal').modal('hide');
});

function toggleFavorite(conteudoId) {
    const iconFeed = document.getElementById(`favorito-${conteudoId}`);
    const iconModal = document.querySelector(`#feedModal-${conteudoId} #favorito-${conteudoId}`);

    const isFavorited = iconFeed.src.includes("Favorito.svg");

    fetch(`/Favoritos/ToggleFavorito?conteudoId=${conteudoId}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => {
            if (response.ok) {
                // Alterna a imagem de favorito no feed
                iconFeed.src = isFavorited
                    ? '/img/Desfavorito.svg'
                    : '/img/Favorito.svg';

                // Alterna a imagem de favorito no modal, se existir
                if (iconModal) {
                    iconModal.src = isFavorited
                        ? '/img/Desfavorito.svg'
                        : '/img/Favorito.svg';
                }
            } else if (response.status === 401) {
                // Redireciona para a página de cadastro se não autenticado
                response.text().then(registerUrl => {
                    window.location.href = registerUrl;
                });
            } else {
                console.error(`Erro: Status ${response.status}, Texto: ${response.statusText}`);
                response.text().then(texto => console.error(`Mensagem de erro: ${texto}`));
                alert("Erro ao favoritar/desfavoritar o conteúdo.");
            }
        })
        .catch(error => {
            console.error('Erro na requisição:', error);
            alert("Erro ao processar a solicitação.");
        });
}


document.addEventListener('DOMContentLoaded', function () {
    const searchInput = document.getElementById('searchInputFeed');
    const cards = document.querySelectorAll('.card');
    const checkboxesTipos = document.querySelectorAll('.tipos input[type="checkbox"]');
    const checkboxesPaises = document.querySelectorAll('.paises input[type="checkbox"]');

    // Adiciona evento de input para o campo de busca
    searchInput.addEventListener('input', filterCards);

    // Adiciona eventos de mudança para os checkboxes de tipos e países
    checkboxesTipos.forEach(checkbox => {
        checkbox.addEventListener('change', filterCards);
    });
    checkboxesPaises.forEach(checkbox => {
        checkbox.addEventListener('change', filterCards);
    });

    function filterCards() {
        const searchTerm = searchInput.value.toLowerCase();
        const selectedTipos = [...checkboxesTipos].filter(cb => cb.checked).map(cb => cb.parentElement.textContent.trim());
        const selectedPaises = [...checkboxesPaises].filter(cb => cb.checked).map(cb => cb.parentElement.textContent.trim());

        cards.forEach(card => {
            const titulo = card.querySelector('#titulo').textContent.toLowerCase();
            const descricao = card.querySelector('#descricao').textContent.toLowerCase();
            const categorias = card.querySelector('#categorias').textContent.toLowerCase();
            const tipo = card.querySelector('#tipo').textContent.trim();
            const pais = card.querySelector('#pais').textContent.trim();

            const matchesSearch = searchTerm === '' ||
                titulo.includes(searchTerm) ||
                descricao.includes(searchTerm) ||
                categorias.includes(searchTerm);

            const matchesTipo = selectedTipos.length === 0 || selectedTipos.includes(tipo);
            const matchesPais = selectedPaises.length === 0 || selectedPaises.includes(pais);

            // Atualiza a exibição do card com base nos critérios
            card.style.display = (matchesSearch && matchesTipo && matchesPais) ? '' : 'none';
        });
    }
});




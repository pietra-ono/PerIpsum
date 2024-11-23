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
            } else {
                // Log detalhado do erro para depuração
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
    const filtroPersonalizado = document.getElementById('filtroPersonalizado');

    // Inicialmente, os checkboxes de preferências não devem estar marcados
    checkboxesTipos.forEach(cb => cb.checked = false);
    checkboxesPaises.forEach(cb => cb.checked = false);

    // Adiciona evento de input para o campo de busca
    searchInput.addEventListener('input', filterCards);

    // Adiciona eventos de mudança para os checkboxes de tipos e países
    checkboxesTipos.forEach(checkbox => {
        checkbox.addEventListener('change', filterCards);
    });
    checkboxesPaises.forEach(checkbox => {
        checkbox.addEventListener('change', filterCards);
    });

    // Adiciona evento de mudança para o filtro personalizado
    filtroPersonalizado.addEventListener('change', function () {
        if (this.checked) {
            // Desmarcar e desabilitar outros checkboxes
            checkboxesTipos.forEach(cb => {
                cb.checked = false; // Desmarcar
                cb.disabled = true; // Desabilitar
            });
            checkboxesPaises.forEach(cb => {
                cb.checked = false; // Desmarcar
                cb.disabled = true; // Desabilitar
            });

            // Marcar as preferências salvas
            Model.TiposSelecionados.forEach(tipo => {
                const checkbox = [...checkboxesTipos].find(cb => cb.value === tipo.toString());
                if (checkbox) checkbox.checked = true;
            });
            Model.PaisesSelecionados.forEach(pais => {
                const checkbox = [...checkboxesPaises].find(cb => cb.value === pais.toString());
                if (checkbox) checkbox.checked = true;
            });
        } else {
            // Reabilitar checkboxes se "Personalizado" não estiver selecionado
            checkboxesTipos.forEach(cb => {
                cb.checked = false; // Desmarcar
                cb.disabled = false; // Habilitar
            });
            checkboxesPaises.forEach(cb => {
                cb.checked = false; // Desmarcar
                cb.disabled = false; // Habilitar
            });
        }
        filterCards(); // Atualizar a filtragem
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

            // Se "Personalizado" estiver marcado, a filtragem deve ser aplicada
            card.style.display = (matchesSearch && matchesTipo && matchesPais) ? '' : 'none';
        });
    }
});






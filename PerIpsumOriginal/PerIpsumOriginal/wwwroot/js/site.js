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

    // MODAL VISUALIZAÇÃO RASCUNHO
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

    // MODAL VISUALIZAÇÃO FEED

    $('.fundo').on('click', '.card', function () {

        var id = $(this).data('id');

        var imagem = $(this).find('#imagemCard1').attr('src');
        var titulo = $(this).find('#textoTitulo1').text();
        var descricao = $(this).find('#dataTextoDescricao1').text();
        var data = $(this).find('#dataTexto1').text();
        var pais = $(this).find('#paisTexto1').text();
        var tipo = $(this).find('#tipoTexto1').text();
        var link = $(this).find('#linkTexto1').text();
        var bandeira = $(this).find('#paisDesign1 img').attr('src');



        var modal = $('#conteudoModal1-' + id);
        modal.find('#modalImage1').attr('src', imagem);
        modal.find('#modalTitle1').text(titulo);
        modal.find('#modalDescription1').text(descricao);
        modal.find('#modalData1').text(data);
        modal.find('#modalLink1').attr('href', link);
        modal.find('#modalTipo1').text(tipo).css('background-color', $(this).find('#tipoDesign').css('background-color'));
        modal.find('#modalPaisDisplay1').text(pais);
        modal.find('#modalPaisFlag1').attr('src', bandeira);

        modal.modal('show');
    });



    // Fechar modal ao clicar no botão de fechar
    $('.btn-close').click(function () {
        $('.modal').modal('hide');
    });



    $('.chosen-select').chosen({ width: "95%" });


    // MODAL VISUALIZAÇÃO ANOTAÇÕES
    $('.fundoAnotacoes').on('click', '.blocoAnotacao', function () {

        var id = $(this).data('id');

        var titulo = $(this).find('#tituloAnotacao').attr('src');
        var descricao = $(this).find('#descricaoAnotacao').attr('src');



        var modal = $('#anotVisuModal-' + id);

        modal.find('#modalTitulo').text(titulo);
        modal.find('#modalDescricao').text(descricao);

        modal.modal('show');
    });
});


function toggleFavorite(id) {
    var usuarioId = '@_userManager.GetUserId(User)';
    var favorito = {
        UsuarioId: usuarioId,
        ConteudoId: id
    };

    $.ajax({
        type: 'POST',
        url: '/Home/Favoritar',
        data: favorito,
        success: function (data) {
            if (data.success) {
                // Atualizar a imagem do botão de favoritar/desfavoritar
                var imagem = $('#favorito-img-' + id);
                if (data.isFavorited) {
                    imagem.attr('src', '/images/Favorito.svg');
                } else {
                    imagem.attr('src', '/images/Desfavorito.svg');
                }
            }
        }
    });
}



document.getElementById("searchInput").addEventListener("keyup", function () {
    var input, filter, cards, card, i, txtValue;
    input = document.getElementById("searchInput");
    filter = input.value.toLowerCase();
    cards = document.getElementsByClassName("card");

    for (i = 0; i < cards.length; i++) {
        card = cards[i];
        txtValue = card.textContent || card.innerText;
        if (txtValue.toLowerCase().indexOf(filter) > -1) {
            card.style.display = "";
        } else {
            card.style.display = "none";
        }
    }
});

function applyFilters() {
    var checkboxes = document.querySelectorAll('.form-check-input');
    var selectedCategories = Array.from(checkboxes).filter(i => i.checked).map(i => i.value);
    var cards = document.getElementsByClassName("card");

    for (var i = 0; i < cards.length; i++) {
        var card = cards[i];
        var categorias = card.querySelector("#categoriasTexto").textContent.split(",");
        var showCard = selectedCategories.every(cat => categorias.includes(cat.trim()));

        if (showCard) {
            card.style.display = "";
        } else {
            card.style.display = "none";
        }
    }
}

document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');
    var calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth',
        events: '/Usuario/ObterEventos',  // URL para carregar os eventos
        editable: true
    });
    calendar.render();
});


function abrirModalCriacao(data) {
    $('#modalCriacao').modal('show');
    $('#DataCriacao').val(data); // Passa a data clicada para o campo do formulário
}

$('#formCriacao').submit(function (e) {
    e.preventDefault();
    var dadosEvento = {
        Titulo: $('#TituloCriacao').val(),
        Descricao: $('#DescricaoCriacao').val(),
        Data: $('#DataCriacao').val()
    };

    $.ajax({
        type: "POST",
        url: "/Usuario/AdicionarEvento",
        data: dadosEvento,
        success: function () {
            $('#modalCriacao').modal('hide');
            calendar.refetchEvents(); // Recarrega os eventos no calendário
        },
        error: function () {
            alert("Erro ao criar evento.");
        }
    });
});

function abrirModalEdicao(evento) {
    $('#modalEdicao').modal('show');
    $('#IdEdicao').val(evento.id);
    $('#TituloEdicao').val(evento.title);
    $('#DescricaoEdicao').val(evento.extendedProps.description);
    $('#DataEdicao').val(evento.startStr); // Data do evento
}

// Exemplo de AJAX para editar evento
$('#formEdicao').submit(function (e) {
    e.preventDefault();
    var dadosEvento = {
        Id: $('#IdEdicao').val(),
        Titulo: $('#TituloEdicao').val(),
        Descricao: $('#DescricaoEdicao').val(),
        Data: $('#DataEdicao').val()
    };

    $.ajax({
        type: "POST",
        url: "/Usuario/AlterarEvento",
        data: dadosEvento,
        success: function () {
            $('#modalEdicao').modal('hide');
            calendar.refetchEvents(); // Recarrega os eventos no calendário
        },
        error: function () {
            alert("Erro ao editar evento.");
        }
    });
});


$('#btnDeletarEvento').click(function () {
    var id = $('#IdEdicao').val();

    $.ajax({
        type: "POST",
        url: "/Usuario/ApagarEvento",
        data: { id: id },
        success: function () {
            $('#modalEdicao').modal('hide');
            calendar.refetchEvents(); // Recarrega os eventos no calendário
        },
        error: function () {
            alert("Erro ao deletar evento.");
        }
    });


    // FUNÇÕES INÍCIO

    let slideIndex = 0;
    showSlides();

    function showSlides() {
        let slides = document.getElementsByClassName("mySlides");
        // Esconder todas as imagens
        for (let i = 0; i < slides.length; i++) {
            slides[i].style.display = "none";
        }
        // Incrementa o index do slide
        slideIndex++;
        // Se o index ultrapassar o número de slides, volta ao primeiro
        if (slideIndex > slides.length) {
            slideIndex = 1;
        }
        // Exibe o slide atual
        slides[slideIndex - 1].style.display = "block";
        // Altera a imagem a cada 5 segundos
        setTimeout(showSlides, 5000);
    }

    document.addEventListener("DOMContentLoaded", function () {
        const path = window.location.pathname;

        // Detecta a página atual e adiciona a classe 'active' no link correspondente
        if (path.includes("Inicio")) {
            document.querySelector('.icone-home').classList.add('active');
        } else if (path.includes("Feed")) {
            document.querySelector('.icone-feed').classList.add('active');
        }
    });

    document.querySelectorAll('.faq-item').forEach(item => {
        item.addEventListener('click', function () {
            // Alternar a classe 'show' para mostrar/esconder a resposta
            const answer = this.querySelector('.faq-answer');
            answer.classList.toggle('show');
        });
    });

    document.querySelectorAll('.faq-item').forEach(item => {
        item.addEventListener('click', () => {
            item.classList.toggle('expanded');
        });
    });

    document.querySelector('a[href="#sobre-nos"]').addEventListener('click', function (e) {
        e.preventDefault(); // Evita o comportamento padrão
        document.querySelector('#sobre-nos').scrollIntoView({ behavior: 'smooth' });
    });

});

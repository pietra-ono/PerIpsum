$(document).ready(function () {
    $('.feedback-item').on('click', function (e) {
        e.preventDefault();

        // Remove a classe de seleção de outros itens
        $('.feedback-item').removeClass('active');

        // Adiciona a classe de seleção ao item clicado
        $(this).addClass('active');

        // Obtém o ID do feedback
        var feedbackId = $(this).data('id');

        // URL completa para a action
        var url = '/Admin/ObterDetalhesdoFeedback';

        // Carrega os detalhes do feedback via AJAX
        $.ajax({
            url: url,
            type: 'GET',
            data: { id: feedbackId },
            success: function (result) {
                // Atualiza o painel de detalhes com os dados recebidos
                $('#feedbackDetailsContent').html(result);
            },
            error: function () {
                $('#feedbackDetailsContent').html('<p>Erro ao carregar detalhes do feedback.</p>');
            }
        });
    });
});
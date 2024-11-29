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

    document.addEventListener("DOMContentLoaded", function () {
        const path = window.location.pathname;

        // Detecta a página atual e adiciona a classe 'active' no link correspondente
        if (path.includes("Inicio")) {
            document.querySelector('.icone-home').classList.add('active');
        } else if (path.includes("Feed")) {
            document.querySelector('.icone-feed').classList.add('active');
        }
    });

    document.querySelector('a[href="#sobre-nos"]').addEventListener('click', function (e) {
        e.preventDefault(); // Evita o comportamento padrão
        document.querySelector('#sobre-nos').scrollIntoView({ behavior: 'smooth' });
    });






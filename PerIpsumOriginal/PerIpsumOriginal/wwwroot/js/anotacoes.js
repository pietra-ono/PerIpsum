document.addEventListener('DOMContentLoaded', function () {
    const searchInput = document.getElementById('searchInputFeed');
    const blocos = document.querySelectorAll('.blocoAnotacao');

    // Função de busca
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

    // Adicionar evento de clique para visualizar anotações
    $('.anotacoes-container').on('click', '.blocoAnotacao', function () {
        const id = $(this).data('id');
        const titulo = $(this).find('#tituloAnotacao p').text();
        const descricao = $(this).find('#descricaoAnotacao p').text();
        const cor = $(this).css('background-color');
        const baseUrl = window.location.origin; // Obtém o domínio atual
        const delButtonImg = `${baseUrl}/img/DelButton.svg`;
        const editButtonImg = `${baseUrl}/img/EditButton.svg`;
        const checkIconImg = `${baseUrl}/img/checkIcon.svg`;
        const paintIconimg = `${baseUrl}/img/paintIcon.svg`;

        // Criar modal dinamicamente
        const modalHtml = `
            <div class="modal fade" id="viewAnotacaoModal" tabindex="-1">
                <div class="modal-dialog modal-dialog-centered" style=" margin: auto !important; max-width: 100%; width: 950px; height: 70%;">
                    <div class="modal-content" style="position: relative; display: flex; flex-direction: column; width: 100%; max-height: 90vh; height: 70%;
                    pointer-events: auto; background-color: #fffff3; background-clip: border-box; overflow-y: auto; border: none; border-radius: 15px; box-shadow: 0px 3px 4px rgba(0, 0, 0, 0.3); padding: 1.5em;" >
                        <div class="btn-modal-container" style=" display: inline-flex; position: absolute; top: 2%; right: 2%; justify-content: end;" >
                            <button class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close" style="filter: invert(19%) sepia(72%) saturate(5297%) hue-rotate(353deg) brightness(88%) contrast(108%);" ></button>
                        </div>
                        <div class="header-modal-anotacoes" style="display: inline-flex; justify-content: start; width: 100%;">
                            <div class="titulo-modal-anotacoes" style="background-color: ${cor}; border-radius: 15px; padding-inline: 1em;">
                                <h5 class="modal-title" id="modalAnotacaoTitulo" style="font-family: Krub; font-size: 20px; color: #fffff3;" >${titulo}</h5>
                            </div>
                        </div>
                        <div class="modal-body">
                            <input type="hidden" id="editAnotacaoId" value="${id}">
                            <div id="viewAnotacaoContent">
                                <div class="descricao-modal-anotacoes" style="display: flex; flex-direction: column; height: 400px; width: 100%; flex-grow: 1; overflow-y: auto; margin-top: 1em; max-width: 100%;margin-top: 1em; box-sizing: border-box;" >
                                    <p id="modalAnotacaoDescricao" style="font-family: Krub; font-size: 16px; word-wrap: break-word; overflow-wrap: break-word; white-space: pre-wrap; overflow-y: auto; height: 100%;" >${descricao}</p>
                                </div>
                            </div>
                            <div id="editAnotacaoContent" style="display:none;">
                                <div class="header-modal-anotacoes" style="display: inline-flex; justify-content: space-between; margin: 1em 0;" >
                                    <div class="titulo-modal-anotacoes" >
                                        <input type="text" class="form-control" id="editAnotacaoTitulo" value="${titulo}" style="background-color: transparent; padding: 0; border: none; width: 150px; font-family: Krub; outline: none; background-color: #FFFFF3; border-color: #009846; box-shadow: none;" >
                                    </div>
                                    <div class="cor-modal-anotacoes" style="display: inline-flex; width: 40%; width: 80px; background-color: #fffff3; border-radius: 15px; padding-inline: 0.1em; box-shadow: 0px 3px 4px rgba(0, 0, 0, 0.3); justify-content: space-between; margin-left: 1em;" >
                                        <input type="color" class="form-control" id="editAnotacaoCor" value="${rgbToHex(cor)}" style="border-radius: 15px 0 0 15px; padding: 0; border: none; cursor: pointer;" >
                                        <img src="${paintIconimg}" alt="Cor" style="width: 25px; height: 25px;" />
                                    </div>
                                </div>
                                <div class="descricao-modal-anotacoes" style=" display: flex; flex-direction: column; height: 400px; width: auto; max-width: 100%; margin-top: 1em; box-sizing: border-box;" >
                                    <textarea class="form-control" id="editAnotacaoDescricao" style="flex-grow: 1; resize: none; padding: 10px; font-family: Krub; font-size: 16px; background-color: transparent; border: none; outline: none; background-color: #FFFFF3; border-color: #009846; box-shadow: none;" >${descricao}</textarea>
                                </div>
                            </div>
                        </div>
                        <div class="footerModal" style="display: inline-flex; justify-content: space-between; position: relative;" >
                            <button type="button" class="btn" id="deleteAnotacaoBtn"><img src="${delButtonImg}" alt="Deletar" /></button>
                            <button type="button" class="btn" id="editAnotacaoBtn"><img src="${editButtonImg}" alt="Editar" /></button>
                            <button type="button" class="btn" id="saveAnotacaoBtn" style="display:none;"><img src="${checkIconImg}" alt="Salvar" /></button>
                        </div>
                    </div>
                </div>
            </div>
        `;

        // Remover modal existente e adicionar novo
        $('#viewAnotacaoModal').remove();
        $('body').append(modalHtml);
        $('#viewAnotacaoModal').modal('show');
    });

    // Evento de edição
    $('body').on('click', '#editAnotacaoBtn', function () {
        $('#viewAnotacaoContent').hide();
        $('#editAnotacaoContent').show();
        $('#editAnotacaoBtn').hide();
        $('#saveAnotacaoBtn').show();
    });

    // Evento de salvar
    $('body').on('click', '#saveAnotacaoBtn', function () {
        const anotacao = {
            id: $('#editAnotacaoId').val(),
            titulo: $('#editAnotacaoTitulo').val(),
            descricao: $('#editAnotacaoDescricao').val(),
            cor: $('#editAnotacaoCor').val()
        };

        fetch('/Usuario/AlterarAnotacao', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(anotacao)
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    location.reload(); // Recarrega a página para mostrar as alterações
                }
            });
    });

    // Evento de exclusão
    let anotacaoIdToDelete = null; // Para armazenar o ID da anotação a ser deletada

    // Evento de exclusão
    $('body').on('click', '#deleteAnotacaoBtn', function () {

        $('.modal').modal('hide');
        // Obtém o ID da anotação para exclusão
        anotacaoIdToDelete = $('#editAnotacaoId').val();

        // Mostra a modal de confirmação
        $('#deleteModal').modal('show');
    });

    // Evento de confirmação da exclusão
    $('body').on('click', '#confirmDeleteBtn', function () {
        if (anotacaoIdToDelete) {
            fetch(`/Usuario/ApagarAnotacao/${anotacaoIdToDelete}`, {
                method: 'POST'
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        location.reload(); // Recarrega a página após exclusão
                    } else {
                        alert('Erro ao deletar a anotação.'); // Apenas para fallback
                    }
                })
                .catch(err => console.error('Erro ao deletar:', err));
        }
    });

    // Função para converter RGB para Hexadecimal
    function rgbToHex(rgb) {
        // Remove 'rgb(' e ')' e divide os valores
        const [r, g, b] = rgb.match(/\d+/g).map(Number);
        // Converte para hexadecimal
        return "#" + ((1 << 24) + (r << 16) + (g << 8) + b).toString(16).slice(1);
    }
});
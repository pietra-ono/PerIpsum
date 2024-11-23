document.addEventListener('DOMContentLoaded', function () {
    var calendar = new FullCalendar.Calendar(document.getElementById('calendar'), {
        initialView: 'dayGridMonth',
        locale: 'pt-br',
        events: '/Usuario/GetEventos',
        selectable: true,
        customButtons: {
            search: {
                text: '',
                click: function () {
                    // Lógica da pesquisa
                },
                html: '<input type="text" placeholder="Pesquisar..." class="fc-search-input">'
            },
            filter: {
                text: 'Filtrar',
                click: function () {
                    // Lógica do filtro
                }
            },
            addEvent: {
                text: 'Adicionar Evento',
                click: function () {
                    $('#createEventModal').modal('show');
                }
            }
        },
        headerToolbar: {
            left: 'search filter prev',
            center: 'title',
            right: 'next addEvent'
        },
        buttonText: {
            today: 'Hoje',
            month: 'Mês',
            week: 'Semana',
            day: 'Dia',
            list: 'Lista'
        },
        weekText: 'Sem',
        allDayText: 'Dia inteiro',
        moreLinkText: 'mais',
        noEventsText: 'Não há eventos para mostrar',
        select: function (info) {
            $('#newEventDate').val(info.startStr);
            $('#createEventModal').modal('show');
        },
        eventClick: function (info) {
            const event = info.event;
            document.getElementById('modalTitle').innerText = event.title;
            document.getElementById('modalDescription').innerText = event.extendedProps.description || '';

            if (event.extendedProps.isUserEvent) {
                $('#userEventButtons').show();
                $('#editEventId').val(event.id);
                $('#editEventTitle').val(event.title);
                $('#editEventDescription').val(event.extendedProps.description);
                $('#editEventDate').val(event.startStr);
            } else {
                $('#userEventButtons').hide();
                document.getElementById('modalLink').href = event.extendedProps.link;
                document.getElementById('modalTipo').innerText = event.extendedProps.tipo;
            }

            $('#eventModal').modal('show');
        }
    });

    calendar.render();



    calendar.render();
    // Adicione no início do DOMContentLoaded
    $('#novoEventoBtn').click(function () {
        // Limpa os campos do formulário
        $('#newEventTitle').val('');
        $('#newEventDescription').val('');
        $('#newEventDate').val('');
        $('#createEventModal').modal('show');
    });

    // Adicione estas funções para limpar os modais quando fechados
    $('#eventModal').on('hidden.bs.modal', function () {
        $('#viewEventContent').show();
        $('#editEventContent').hide();
        $('#editEventBtn').show();
        $('#saveEventBtn').hide();
    });

    $('#createEventModal').on('hidden.bs.modal', function () {
        $('#newEventTitle').val('');
        $('#newEventDescription').val('');
        $('#newEventDate').val('');
    });

    
    // Criar Evento
    $('#createEventBtn').click(function () {
        const evento = {
            titulo: $('#newEventTitle').val(),
            descricao: $('#newEventDescription').val(),
            data: $('#newEventDate').val()
        };

        fetch('/Usuario/CriarEvento', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(evento)
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    calendar.refetchEvents();
                    $('#createEventModal').modal('hide');
                }
            });
    });

    // Editar Evento
    $('#editEventBtn').click(function () {
        $('#viewEventContent').hide();
        $('#editEventContent').show();
        $('#editEventBtn').hide();
        $('#saveEventBtn').show();
    });

    // Salvar Edição
    $('#saveEventBtn').click(function () {
        const evento = {
            id: $('#editEventId').val(),
            titulo: $('#editEventTitle').val(),
            descricao: $('#editEventDescription').val(),
            data: $('#editEventDate').val()
        };

        fetch('/Usuario/AtualizarEvento', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(evento)
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    calendar.refetchEvents();
                    $('#eventModal').modal('hide');
                }
            });
    });

    // Deletar Evento
    $('#deleteEventBtn').click(function () {
        if (confirm('Tem certeza que deseja excluir este evento?')) {
            const eventId = $('#editEventId').val();

            fetch(`/Usuario/DeletarEvento/${eventId}`, {
                method: 'DELETE'
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        calendar.refetchEvents();
                        $('#eventModal').modal('hide');
                    }
                });
        }
    });
});

// Função para resetar o formulário de edição
    function resetEditForm() {
        $('#viewEventContent').show();
        $('#editEventContent').hide();
        $('#editEventBtn').show();
        $('#saveEventBtn').hide();
    }

document.addEventListener('DOMContentLoaded', function () {
    var calendar = new FullCalendar.Calendar(document.getElementById('calendar'), {
        initialView: 'dayGridMonth',
        locale: 'pt-br',
        events: '/Usuario/GetEventos',
        selectable: true,
        customButtons: {
            addEvent: {
                text: 'Adicionar Evento',
                click: function () {
                    $('#createEventModal').modal('show');
                }
            }
        },
        headerToolbar: {
            left: 'prev',
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

            // Configurações gerais
            $('#modalTitle').text(event.title || 'Sem título');
            $('#modalDescription').text(event.extendedProps.description || '');
            $('#modalDate').text(event.startStr || '');

            // Resetar a visibilidade dos botões de edição/exclusão
            $('#userEventButtons').hide();

            // Verificar se é um evento de usuário
            const isUserEvent = event.extendedProps.isUserEvent === true;

            if (!isUserEvent) {
                // Configurações para eventos do site (como antes)
                const pais = event.extendedProps.pais || 'Não informado';
                const bandeira = event.extendedProps.bandeira || '';

                $('#modalPais').text(pais);
                $('#bandeiraModal').attr('src', bandeira).attr('alt', `Bandeira de ${pais}`);
                $('#modalTipo').text(event.extendedProps.tipo || 'Não informado');
                $('#modalLink').text('Acessar').attr('href', event.extendedProps.link || '#').attr('target', '_blank');

                // Definir cor de fundo do tipo (código anterior)
                const tipo = event.extendedProps.tipo;
                let backgroundColor = '#717171'; // cor padrão
                switch (tipo) {
                    case 'Bolsas':
                        backgroundColor = '#C50003';
                        break;
                    case 'Intercambios':
                        backgroundColor = '#E2CB26';
                        break;
                    case 'Programas':
                        backgroundColor = '#642C8F';
                        break;
                    case 'Estagios':
                        backgroundColor = '#009846';
                        break;
                    case 'Cursos':
                        backgroundColor = '#002279';
                        break;
                    case 'Eventos':
                        backgroundColor = '#931486';
                        break;
                    default:
                        backgroundColor = '#717171';
                }
                $('#modalTipo').css('background-color', backgroundColor);
            } else {
                // Para eventos criados pelo usuário
                $('#userEventButtons').show(); // Mostrar botões de edição/exclusão

                // Preencher campos de edição
                $('#editEventId').val(event.id);
                $('#editEventTitle').val(event.title);
                $('#editEventDescription').val(event.extendedProps.description || '');
                $('#editEventDate').val(event.startStr);

                // Limpar informações extras de eventos do site
                $('#modalPais').text('');
                $('#bandeiraModal').attr('src', '').attr('alt', '');
                $('#modalTipo').text('');
                $('#modalLink').text('').attr('href', '#');
                $('#modalTipo').css('background-color', '');
            }

            $('#eventModal').modal('show');
        }
    });

    calendar.render();

    // Botão para criar novo evento
    $('#createEventBtn').click(function () {
        const evento = {
            data: $('#newEventDate').val(),
            titulo: $('#newEventTitle').val(),
            descricao: $('#newEventDescription').val()
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

    // Botão para ativar edição de evento
    $('#editEventBtn').click(function () {
        const event = calendar.getEventById($('#editEventId').val());

        // Verificar se é um evento de usuário antes de permitir edição
        if (event && event.extendedProps.isUserEvent === true) {
            $('#viewEventContent').hide();
            $('#editEventContent').show();
            $('#editEventBtn').hide();
            $('#saveEventBtn').show();
        } else {
            alert('Apenas eventos criados pelo usuário podem ser editados.');
        }
    });

    // Botão para salvar edição de evento
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

    $('#deleteEventBtn').click(function () {
        const event = calendar.getEventById($('#editEventId').val());

        // Verificar se é um evento de usuário antes de permitir exclusão
        if (event && event.extendedProps.isUserEvent === true) {
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
        } else {
            alert('Apenas eventos criados pelo usuário podem ser excluídos.');
        }
    });

    // Resetar modal ao fechar
    $('#eventModal').on('hidden.bs.modal', function () {
        $('#viewEventContent').show();
        $('#editEventContent').hide();
        $('#editEventBtn').show();
        $('#saveEventBtn').hide();
    });
});

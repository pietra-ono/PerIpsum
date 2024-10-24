function toggleFavorito(conteudoId) {
    fetch(`/UsuarioController/ToggleFavorito?conteudoId=${conteudoId}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'X-CSRF-TOKEN': $('input[name="__RequestVerificationToken"]').val()
        },
    })
        .then(response => {
            if (response.ok) {
                const icon = document.getElementById(`favoritoIcon-${conteudoId}`);
                if (icon.src.includes('Desfavorito.svg')) {
                    icon.src = '/img/Favorito.svg';
                } else {
                    icon.src = '/img/Desfavorito.svg';
                }
            } else {
                console.error('Failed to toggle favorito');
            }
        })
        .catch(error => console.error('Error:', error));
}



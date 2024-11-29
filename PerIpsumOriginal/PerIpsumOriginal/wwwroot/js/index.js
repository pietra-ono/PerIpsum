document.querySelectorAll('.faq-item').forEach(item => {
    item.addEventListener('click', () => {
        const answer = item.querySelector('.faq-answer');

        if (item.classList.contains('expanded')) {
            // Recolhe a resposta do item clicado
            answer.style.maxHeight = '0';
            item.classList.remove('expanded');
        } else {
            // Fecha todas as respostas antes de abrir o novo item
            document.querySelectorAll('.faq-item').forEach(otherItem => {
                const otherAnswer = otherItem.querySelector('.faq-answer');
                otherAnswer.style.maxHeight = '0';
                otherItem.classList.remove('expanded');
            });

            // Expande a resposta do item clicado
            answer.style.maxHeight = answer.scrollHeight + "px";
            item.classList.add('expanded');
        }
    });
});

// Função para o slideshow
let slideIndex = 0;
function showSlides() {
    const slides = document.querySelectorAll(".mySlides");

    // Esconde todas as imagens
    slides.forEach(slide => (slide.style.display = "none"));

    // Avança para a próxima imagem
    slideIndex++;
    if (slideIndex > slides.length) {
        slideIndex = 1; // Reinicia o índice
    }

    // Exibe a imagem atual
    slides[slideIndex - 1].style.display = "block";

    // Troca a imagem a cada 5 segundos
    setTimeout(showSlides, 5000);
}

// Inicia o slideshow ao carregar a página
showSlides();
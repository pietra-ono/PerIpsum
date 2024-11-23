const toggleButton = document.getElementById('toggle-btn');
const sidebar = document.getElementById('sidebar');


function toggleSidebar() {
    sidebar.classList.toggle('close');
};

document.addEventListener("DOMContentLoaded", () => {
    const currentPath = window.location.pathname.split('/').pop();
    const sidebarLinks = document.querySelectorAll('#sidebar a');

    sidebarLinks.forEach(link => {
        const page = link.getAttribute('data-page');

        if (page === currentPath) {
            link.classList.add('active');

            // Adiciona o caminho absoluto do ícone
            const icon = link.querySelector('.icon');
            const activeIconPath = link.getAttribute('data-icon-active');
            icon.setAttribute('src', `/img/${activeIconPath}`);
        }
    });
});

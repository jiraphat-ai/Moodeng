
// navbar toggle
const navMenu = document.getElementById('nav-menu');
const navToggle = document.getElementById('nav-toggle');

if (navToggle) {
    navToggle.addEventListener('click', () => {
        navMenu.classList.toggle('max-[768px]:h-[280px]');
        navMenu.classList.toggle('max-[768px]:h-0');
    });
}
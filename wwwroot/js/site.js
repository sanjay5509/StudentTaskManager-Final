// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function toggleSidebar() {
    const sidebar = document.querySelector('.sidebar');
    // active-sidebar class ko add/remove karega
    sidebar.classList.toggle('active-sidebar');
}
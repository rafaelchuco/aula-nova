// Mejora progresiva: las operaciones y la validación del servidor funcionan sin JavaScript.
document.querySelectorAll('form').forEach(function (form) {
    form.addEventListener('submit', function () {
        if (!form.checkValidity()) return;
        var button = form.querySelector('button[type="submit"]');
        if (button && !form.classList.contains('search')) {
            button.disabled = true;
            button.textContent = 'Procesando…';
        }
    });
});
window.addEventListener('pageshow', function () {
    document.querySelectorAll('button[disabled]').forEach(function (button) {
        // Al volver desde el historial, recarga el formulario para recuperar su estado original.
        window.location.reload();
    });
});

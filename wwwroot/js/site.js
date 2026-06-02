// =============================================
// SportBookingSystem – site.js
// Vanilla JavaScript – interaktywność strony
// =============================================

document.addEventListener('DOMContentLoaded', function () {

    // --- Auto-zamykanie alertów po 5 sekundach ---
    const alerts = document.querySelectorAll('.alert.alert-success:not(.alert-dismissible-manual)');
    alerts.forEach(function (alert) {
        setTimeout(function () {
            const bsAlert = bootstrap.Alert.getOrCreateInstance(alert);
            if (bsAlert) bsAlert.close();
        }, 5000);
    });

    // --- Podświetlenie aktywnego linku w nawigacji ---
    const currentPath = window.location.pathname.toLowerCase();
    const navLinks = document.querySelectorAll('.navbar-nav .nav-link');
    navLinks.forEach(function (link) {
        const href = link.getAttribute('href');
        if (href && currentPath.startsWith(href.toLowerCase()) && href !== '/') {
            link.classList.add('active');
        }
        // Specjalny przypadek dla strony głównej
        if (href === '/' && currentPath === '/') {
            link.classList.add('active');
        }
    });

    // --- Animacja kart przy scrollowaniu (Intersection Observer) ---
    const cards = document.querySelectorAll('.court-card, .card.shadow-sm');
    if ('IntersectionObserver' in window) {
        const observer = new IntersectionObserver(function (entries) {
            entries.forEach(function (entry) {
                if (entry.isIntersecting) {
                    entry.target.style.opacity = '1';
                    entry.target.style.transform = 'translateY(0)';
                    observer.unobserve(entry.target);
                }
            });
        }, { threshold: 0.1 });

        cards.forEach(function (card) {
            card.style.opacity = '0';
            card.style.transform = 'translateY(20px)';
            card.style.transition = 'opacity 0.4s ease, transform 0.4s ease';
            observer.observe(card);
        });
    }

    // --- Walidacja daty – nie można wybrać przeszłości ---
    const dateInputs = document.querySelectorAll('input[type="date"]');
    dateInputs.forEach(function (input) {
        const today = new Date().toISOString().split('T')[0];
        if (!input.min) {
            input.min = today;
        }
        input.addEventListener('change', function () {
            if (input.value < today) {
                input.setCustomValidity('Nie możesz wybrać daty z przeszłości.');
                input.reportValidity();
                input.value = today;
            } else {
                input.setCustomValidity('');
            }
        });
    });

    console.log('SportBookingSystem – JS zainicjalizowany ✓');
});

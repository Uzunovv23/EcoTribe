
document.addEventListener('DOMContentLoaded', function () {
    initNavbar();
    initScrollAnimation();
    initStatCounters();
    initScrollToTop();
    initAnimations();
});

function initNavbar() {
    const navbar = document.querySelector('.navbar');
    const navLinks = document.querySelectorAll('.nav-link');
    const brandIcon = document.querySelector('.navbar-brand i');

    if (!navbar) return;

    window.addEventListener('scroll', function () {
        if (window.scrollY > 50) {
            navbar.classList.add('scrolled');
            if (brandIcon) {
                brandIcon.style.color = '#2ecc71';
            }
        } else {
            navbar.classList.remove('scrolled');
            if (brandIcon) {
                brandIcon.style.color = '#ffffff';
            }
        }
    });

    const currentLocation = window.location.pathname;
    navLinks.forEach(link => {
        const linkPath = link.getAttribute('href');
        if (linkPath === currentLocation || (currentLocation === '/' && linkPath === '/Home')) {
            link.classList.add('active');
        }
    });
}

function initScrollAnimation() {
    const heroSection = document.querySelector('.hero-section');
    const scrollIndicator = document.querySelector('.scroll-indicator');

    if (!heroSection || !scrollIndicator) return;

    window.addEventListener('scroll', function () {
        const scrollPosition = window.scrollY;
        heroSection.style.backgroundPositionY = scrollPosition * 0.5 + 'px';

        const heroContent = document.querySelector('.hero-content');
        if (heroContent) {
            heroContent.style.opacity = 1 - scrollPosition / 700;
            heroContent.style.transform = `translateY(${scrollPosition * 0.1}px)`;
        }
    });

    scrollIndicator.addEventListener('click', function () {
        const statsSection = document.querySelector('.impact-stats');
        if (statsSection) {
            statsSection.scrollIntoView({ behavior: 'smooth' });
        }
    });
}

function initStatCounters() {
    const statCards = document.querySelectorAll('.stat-card');

    if (statCards.length === 0) return;

    const options = {
        threshold: 0.2,
        rootMargin: '0px 0px -100px 0px'
    };

    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const statValue = entry.target.querySelector('h3');
                if (statValue) {
                    const finalValue = parseInt(statValue.textContent.replace(/,/g, ''), 10);
                    animateCounter(statValue, finalValue);
                }
                observer.unobserve(entry.target);
            }
        });
    }, options);

    statCards.forEach(card => {
        observer.observe(card);
    });
}

function animateCounter(element, finalValue, duration = 2000) {
    let startValue = 0;
    let startTime = null;

    function step(timestamp) {
        if (!startTime) startTime = timestamp;
        const progress = Math.min((timestamp - startTime) / duration, 1);
        const currentValue = Math.floor(progress * finalValue);
        element.textContent = currentValue.toLocaleString();

        if (progress < 1) {
            window.requestAnimationFrame(step);
        }
    }

    window.requestAnimationFrame(step);
}

function initScrollToTop() {
    const scrollTopBtn = document.createElement('div');
    scrollTopBtn.className = 'scroll-top-btn';
    scrollTopBtn.innerHTML = '<i class="bi bi-arrow-up"></i>';
    document.body.appendChild(scrollTopBtn);

    window.addEventListener('scroll', function () {
        if (window.scrollY > 300) {
            scrollTopBtn.classList.add('visible');
        } else {
            scrollTopBtn.classList.remove('visible');
        }
    });

    scrollTopBtn.addEventListener('click', function () {
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    });
}

function initAnimations() {
    const animatedElements = document.querySelectorAll('.fade-in, .slide-up, .scale-in');

    if (animatedElements.length === 0) return;

    const options = {
        threshold: 0.2,
        rootMargin: '0px 0px -100px 0px'
    };

    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('active');
                if (entry.target.dataset.delay) {
                    setTimeout(() => {
                        entry.target.classList.add('active');
                    }, parseInt(entry.target.dataset.delay, 10));
                }
                observer.unobserve(entry.target);
            }
        });
    }, options);

    animatedElements.forEach(el => {
        observer.observe(el);
    });

    const initiativeCards = document.querySelectorAll('.initiative-card');

    if (initiativeCards.length > 0) {
        initiativeCards.forEach((card, index) => {
            card.classList.add('scale-in');
            card.style.transitionDelay = `${index * 0.2}s`;
            observer.observe(card);
        });
    }
}
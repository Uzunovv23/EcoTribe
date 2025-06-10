document.addEventListener('DOMContentLoaded', function () {
    initSideNavigation();
    initDashboardAnimations();
    initManagementCards();
});

function initDashboardAnimations() {
    const cards = document.querySelectorAll('.management-card, .stat-card');

    if (cards.length === 0) return;

    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver((entries) => {
        entries.forEach((entry, index) => {
            if (entry.isIntersecting) {
                setTimeout(() => {
                    entry.target.style.opacity = '1';
                    entry.target.style.transform = 'translateY(0)';
                }, index * 100);
                observer.unobserve(entry.target);
            }
        });
    }, observerOptions);

    cards.forEach(card => {
        card.style.opacity = '0';
        card.style.transform = 'translateY(30px)';
        card.style.transition = 'opacity 0.6s ease, transform 0.6s ease';
        observer.observe(card);
    });
}

function initManagementCards() {
    const managementCards = document.querySelectorAll('.management-card');

    managementCards.forEach(card => {
        card.addEventListener('click', function (e) {
            if (!e.target.closest('.management-btn')) {
                const button = this.querySelector('.management-btn');
                if (button) {
                    button.click();
                }
            }
        });

        card.style.cursor = 'pointer';
    });

    const buttons = document.querySelectorAll('.management-btn');

    buttons.forEach(button => {
        button.addEventListener('click', function (e) {
            const ripple = document.createElement('span');
            const rect = this.getBoundingClientRect();
            const size = Math.max(rect.width, rect.height);
            const x = e.clientX - rect.left - size / 2;
            const y = e.clientY - rect.top - size / 2;

            ripple.style.width = ripple.style.height = size + 'px';
            ripple.style.left = x + 'px';
            ripple.style.top = y + 'px';
            ripple.classList.add('ripple');

            this.appendChild(ripple);

            setTimeout(() => {
                ripple.remove();
            }, 600);
        });
    });
}

function initSmoothScrolling() {
    const links = document.querySelectorAll('a[href^="#"]');

    links.forEach(link => {
        link.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));

            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    });
}

function addEnvironmentalEffects() {
    const particlesContainer = document.createElement('div');
    particlesContainer.className = 'environmental-particles';
    particlesContainer.style.cssText = `
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        pointer-events: none;
        z-index: -1;
        overflow: hidden;
    `;

    document.body.appendChild(particlesContainer);

    for (let i = 0; i < 10; i++) {
        setTimeout(() => {
            createLeafParticle(particlesContainer);
        }, i * 2000);
    }
}

function createLeafParticle(container) {
    const leaf = document.createElement('div');
    leaf.innerHTML = '🍃';
    leaf.style.cssText = `
        position: absolute;
        font-size: ${Math.random() * 20 + 15}px;
        opacity: ${Math.random() * 0.5 + 0.2};
        left: ${Math.random() * 100}%;
        animation: fall ${Math.random() * 10 + 10}s linear infinite;
        animation-delay: ${Math.random() * 5}s;
    `;

    container.appendChild(leaf);

    setTimeout(() => {
        if (leaf.parentNode) {
            leaf.parentNode.removeChild(leaf);
        }
    }, 15000);
}

const style = document.createElement('style');
style.textContent = `
    @keyframes fall {
        0% {
            transform: translateY(-100vh) rotate(0deg);
        }
        100% {
            transform: translateY(100vh) rotate(360deg);
        }
    }
`;
document.head.appendChild(style);

addEnvironmentalEffects();
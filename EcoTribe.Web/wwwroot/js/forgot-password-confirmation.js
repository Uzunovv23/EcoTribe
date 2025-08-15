document.addEventListener('DOMContentLoaded', function () {
    // Enhanced page interactions and animations

    // Add subtle parallax effect to floating shapes
    const shapes = document.querySelectorAll('.floating-shape');
    let mouseX = 0;
    let mouseY = 0;

    document.addEventListener('mousemove', function (e) {
        mouseX = (e.clientX / window.innerWidth) * 100;
        mouseY = (e.clientY / window.innerHeight) * 100;

        shapes.forEach((shape, index) => {
            const speed = (index + 1) * 0.5;
            const x = (mouseX - 50) * speed * 0.01;
            const y = (mouseY - 50) * speed * 0.01;

            shape.style.transform = `translate(${x}px, ${y}px)`;
        });
    });

    // Enhanced button interactions
    const buttons = document.querySelectorAll('.btn');

    buttons.forEach(button => {
        // Add ripple effect on click
        button.addEventListener('click', function (e) {
            const ripple = document.createElement('div');
            const rect = this.getBoundingClientRect();
            const size = Math.max(rect.width, rect.height);
            const x = e.clientX - rect.left - size / 2;
            const y = e.clientY - rect.top - size / 2;

            ripple.style.cssText = `
                position: absolute;
                width: ${size}px;
                height: ${size}px;
                left: ${x}px;
                top: ${y}px;
                background: rgba(255, 255, 255, 0.3);
                border-radius: 50%;
                transform: scale(0);
                animation: ripple 0.6s linear;
                pointer-events: none;
                z-index: 1;
            `;

            this.style.position = 'relative';
            this.style.overflow = 'hidden';
            this.appendChild(ripple);

            setTimeout(() => {
                if (this.contains(ripple)) {
                    this.removeChild(ripple);
                }
            }, 600);
        });

        // Add hover sound effect (visual feedback)
        button.addEventListener('mouseenter', function () {
            this.style.transform = 'translateY(-2px) scale(1.02)';
        });

        button.addEventListener('mouseleave', function () {
            this.style.transform = 'translateY(0) scale(1)';
        });
    });

    // Info cards hover effects
    const infoCards = document.querySelectorAll('.info-card');

    infoCards.forEach((card, index) => {
        card.addEventListener('mouseenter', function () {
            // Add subtle glow effect
            this.style.boxShadow = '0 15px 35px rgba(102, 126, 234, 0.2), 0 5px 15px rgba(0, 0, 0, 0.1)';

            // Animate icon
            const icon = this.querySelector('.card-icon');
            if (icon) {
                icon.style.transform = 'scale(1.1) rotate(5deg)';
            }
        });

        card.addEventListener('mouseleave', function () {
            this.style.boxShadow = '';

            const icon = this.querySelector('.card-icon');
            if (icon) {
                icon.style.transform = 'scale(1) rotate(0deg)';
            }
        });
    });

    // Add CSS for ripple animation
    const style = document.createElement('style');
    style.textContent = `
        @keyframes ripple {
            to {
                transform: scale(4);
                opacity: 0;
            }
        }
        
        .card-icon {
            transition: transform 0.3s cubic-bezier(0.4, 0, 0.2, 1);
        }
        
        .btn {
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
        }
    `;
    document.head.appendChild(style);

    // Intersection Observer for scroll animations
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.style.opacity = '1';
                entry.target.style.transform = 'translateY(0)';
            }
        });
    }, observerOptions);

    // Observe elements for scroll animations
    const animatedElements = document.querySelectorAll('.info-card, .actions');
    animatedElements.forEach(el => {
        observer.observe(el);
    });

    // Add keyboard navigation enhancements
    document.addEventListener('keydown', function (e) {
        // Allow Enter key to activate focused buttons
        if (e.key === 'Enter' && e.target.classList.contains('btn')) {
            e.target.click();
        }

        // Add escape key to go back to login (if back button exists)
        if (e.key === 'Escape') {
            const backButton = document.querySelector('.btn-primary');
            if (backButton) {
                backButton.focus();
            }
        }
    });

    // Auto-focus the primary action button after animations complete
    setTimeout(() => {
        const primaryButton = document.querySelector('.btn-primary');
        if (primaryButton) {
            primaryButton.focus();
        }
    }, 1500);

    // Add subtle page entrance animation
    document.body.style.opacity = '0';
    setTimeout(() => {
        document.body.style.transition = 'opacity 0.5s ease-in-out';
        document.body.style.opacity = '1';
    }, 100);

    // Performance optimization: Reduce animations on low-end devices
    if (navigator.hardwareConcurrency && navigator.hardwareConcurrency < 4) {
        const reducedMotionStyle = document.createElement('style');
        reducedMotionStyle.textContent = `
            .floating-shape {
                animation-duration: 40s !important;
            }
            
            .checkmark-circle,
            .checkmark-check {
                animation-duration: 0.3s !important;
            }
        `;
        document.head.appendChild(reducedMotionStyle);
    }

    // Add copy email functionality (if needed in future)
    function addCopyEmailFeature() {
        const message = document.querySelector('.message');
        if (message && message.textContent.includes('@')) {
            // This could be enhanced to extract and copy email addresses
            // For now, it's just a placeholder for future functionality
        }
    }

    // Initialize additional features
    addCopyEmailFeature();

    // Add smooth scroll behavior for any internal links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
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

    // Console log for debugging (remove in production)
    console.log('Forgot Password Confirmation page loaded successfully');
});
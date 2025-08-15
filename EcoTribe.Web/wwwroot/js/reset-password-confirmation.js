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
            const speed = (index + 1) * 0.3;
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

        // Enhanced hover effects
        button.addEventListener('mouseenter', function () {
            this.style.transform = 'translateY(-3px) scale(1.02)';
        });

        button.addEventListener('mouseleave', function () {
            this.style.transform = 'translateY(0) scale(1)';
        });
    });

    // Info cards hover effects
    const infoCards = document.querySelectorAll('.info-card');

    infoCards.forEach((card, index) => {
        card.addEventListener('mouseenter', function () {
            // Add enhanced glow effect
            this.style.boxShadow = '0 20px 40px rgba(102, 126, 234, 0.25), 0 10px 20px rgba(0, 0, 0, 0.1)';

            // Animate icon with rotation
            const icon = this.querySelector('.card-icon');
            if (icon) {
                icon.style.transform = 'scale(1.1) rotate(10deg)';
            }

            // Add subtle background shift
            this.style.background = 'rgba(255, 255, 255, 0.95)';
        });

        card.addEventListener('mouseleave', function () {
            this.style.boxShadow = '';
            this.style.background = 'rgba(255, 255, 255, 0.7)';

            const icon = this.querySelector('.card-icon');
            if (icon) {
                icon.style.transform = 'scale(1) rotate(0deg)';
            }
        });
    });

    // Add celebration confetti effect
    function createConfetti() {
        const colors = ['#667eea', '#764ba2', '#48bb78', '#ed8936', '#e53e3e', '#38b2ac'];
        const container = document.querySelector('.confirmation-container');

        for (let i = 0; i < 50; i++) {
            const confetti = document.createElement('div');
            confetti.style.cssText = `
                position: absolute;
                width: 10px;
                height: 10px;
                background: ${colors[Math.floor(Math.random() * colors.length)]};
                border-radius: 50%;
                pointer-events: none;
                z-index: 1000;
            `;

            const startX = Math.random() * container.offsetWidth;
            const startY = -20;
            const endX = startX + (Math.random() - 0.5) * 300;
            const endY = container.offsetHeight + 100;
            const rotation = Math.random() * 360;

            confetti.style.left = startX + 'px';
            confetti.style.top = startY + 'px';

            container.appendChild(confetti);

            // Animate confetti with physics
            const duration = 3000 + Math.random() * 2000;
            const startTime = Date.now();

            function animateConfetti() {
                const elapsed = Date.now() - startTime;
                const progress = elapsed / duration;

                if (progress < 1) {
                    const easeOut = 1 - Math.pow(1 - progress, 3);
                    const currentX = startX + (endX - startX) * progress;
                    const currentY = startY + (endY - startY) * easeOut;

                    confetti.style.left = currentX + 'px';
                    confetti.style.top = currentY + 'px';
                    confetti.style.opacity = Math.max(0, 1 - progress);
                    confetti.style.transform = `rotate(${rotation * progress}deg) scale(${1 - progress * 0.5})`;

                    requestAnimationFrame(animateConfetti);
                } else {
                    if (container.contains(confetti)) {
                        container.removeChild(confetti);
                    }
                }
            }

            setTimeout(() => animateConfetti(), Math.random() * 1000);
        }
    }

    // Trigger confetti after checkmark animation
    setTimeout(() => {
        createConfetti();
    }, 1500);

    // Add CSS for enhanced animations
    const style = document.createElement('style');
    style.textContent = `
        @keyframes ripple {
            to {
                transform: scale(4);
                opacity: 0;
            }
        }
        
        .card-icon {
            transition: transform 0.4s cubic-bezier(0.4, 0, 0.2, 1);
        }
        
        .btn {
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
        }
        
        .info-card {
            transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1);
        }
        
        /* Enhanced particle effects */
        .particle {
            box-shadow: 0 0 6px rgba(72, 187, 120, 0.6);
        }
        
        /* Glow effect for success elements */
        .checkmark-container::after {
            content: '';
            position: absolute;
            top: -10px;
            left: -10px;
            right: -10px;
            bottom: -10px;
            background: radial-gradient(circle, rgba(72, 187, 120, 0.3) 0%, transparent 70%);
            border-radius: 50%;
            z-index: -1;
            animation: glow 2s ease-in-out infinite alternate;
        }
        
        @keyframes glow {
            from {
                opacity: 0.5;
                transform: scale(0.95);
            }
            to {
                opacity: 0.8;
                transform: scale(1.05);
            }
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
    const animatedElements = document.querySelectorAll('.info-card, .actions, .security-tip');
    animatedElements.forEach(el => {
        observer.observe(el);
    });

    // Add keyboard navigation enhancements
    document.addEventListener('keydown', function (e) {
        // Allow Enter key to activate focused buttons
        if (e.key === 'Enter' && e.target.classList.contains('btn')) {
            e.target.click();
        }

        // Add escape key to focus primary button
        if (e.key === 'Escape') {
            const primaryButton = document.querySelector('.btn-primary');
            if (primaryButton) {
                primaryButton.focus();
            }
        }

        // Add space key support for buttons
        if (e.key === ' ' && e.target.classList.contains('btn')) {
            e.preventDefault();
            e.target.click();
        }
    });

    // Auto-focus the primary action button after animations complete
    setTimeout(() => {
        const primaryButton = document.querySelector('.btn-primary');
        if (primaryButton) {
            primaryButton.focus();
        }
    }, 2000);

    // Add smooth page entrance animation
    document.body.style.opacity = '0';
    setTimeout(() => {
        document.body.style.transition = 'opacity 0.6s ease-in-out';
        document.body.style.opacity = '1';
    }, 100);

    // Performance optimization for low-end devices
    if (navigator.hardwareConcurrency && navigator.hardwareConcurrency < 4) {
        const reducedMotionStyle = document.createElement('style');
        reducedMotionStyle.textContent = `
            .floating-shape {
                animation-duration: 50s !important;
            }
            
            .particle {
                display: none !important;
            }
            
            .checkmark-container::after {
                animation: none !important;
            }
        `;
        document.head.appendChild(reducedMotionStyle);
    }

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

    // Add copy functionality for security tips (future enhancement)
    const securityTip = document.querySelector('.security-tip');
    if (securityTip) {
        securityTip.addEventListener('dblclick', function () {
            // Could add copy to clipboard functionality here
            console.log('Security tip interaction detected');
        });
    }

    // Console log for debugging (remove in production)
    console.log('Reset Password Confirmation page loaded successfully');

    // Add subtle breathing animation to the main container
    const container = document.querySelector('.confirmation-container');
    if (container) {
        container.style.animation = 'breathe 4s ease-in-out infinite';

        const breatheStyle = document.createElement('style');
        breatheStyle.textContent = `
            @keyframes breathe {
                0%, 100% {
                    transform: scale(1);
                }
                50% {
                    transform: scale(1.005);
                }
            }
        `;
        document.head.appendChild(breatheStyle);
    }
});
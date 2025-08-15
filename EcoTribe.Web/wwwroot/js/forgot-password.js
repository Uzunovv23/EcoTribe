document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('forgotPasswordForm');
    const submitBtn = document.getElementById('submitBtn');
    const btnText = submitBtn.querySelector('.btn-text');
    const btnLoader = submitBtn.querySelector('.btn-loader');
    const emailInput = document.getElementById('email');
    const successMessage = document.getElementById('successMessage');

    // Enhanced form validation
    function validateEmail(email) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    }

    function showFieldError(field, message) {
        const errorSpan = field.nextElementSibling;
        if (errorSpan && errorSpan.classList.contains('error-message')) {
            errorSpan.textContent = message;
            field.classList.add('input-validation-error');

            // Add shake animation for visual feedback
            field.style.animation = 'shake 0.5s ease-in-out';
            setTimeout(() => {
                field.style.animation = '';
            }, 500);
        }
    }

    function clearFieldError(field) {
        const errorSpan = field.nextElementSibling;
        if (errorSpan && errorSpan.classList.contains('error-message')) {
            errorSpan.textContent = '';
            field.classList.remove('input-validation-error');
        }
    }

    // Real-time validation
    emailInput.addEventListener('input', function () {
        const email = this.value.trim();
        clearFieldError(this);

        if (email && !validateEmail(email)) {
            showFieldError(this, 'Please enter a valid email address');
        }
    });

    // Enhanced form submission
    form.addEventListener('submit', function (e) {
        e.preventDefault();

        const email = emailInput.value.trim();
        let isValid = true;

        // Clear previous errors
        clearFieldError(emailInput);

        // Validate email
        if (!email) {
            showFieldError(emailInput, 'Email address is required');
            isValid = false;
        } else if (!validateEmail(email)) {
            showFieldError(emailInput, 'Please enter a valid email address');
            isValid = false;
        }

        if (!isValid) {
            // Focus on first error field
            const firstErrorField = form.querySelector('.input-validation-error');
            if (firstErrorField) {
                firstErrorField.focus();
            }
            return;
        }

        // Show loading state
        setLoadingState(true);

        // Create FormData object for proper ASP.NET MVC submission
        const formData = new FormData(form);

        // Submit form using fetch API
        fetch(form.action, {
            method: 'POST',
            body: formData,
            headers: {
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
            }
        })
            .then(response => {
                if (response.ok) {
                    return response.text();
                }
                throw new Error('Network response was not ok');
            })
            .then(data => {
                // Check if response contains validation errors
                if (data.includes('validation-summary-errors') || data.includes('field-validation-error')) {
                    // Parse validation errors from response
                    const parser = new DOMParser();
                    const doc = parser.parseFromString(data, 'text/html');
                    const errorElements = doc.querySelectorAll('.field-validation-error');

                    errorElements.forEach(errorElement => {
                        const fieldName = errorElement.getAttribute('data-valmsg-for');
                        const errorMessage = errorElement.textContent;
                        const field = form.querySelector(`[name="${fieldName}"]`);

                        if (field) {
                            showFieldError(field, errorMessage);
                        }
                    });

                    setLoadingState(false);
                } else {
                    // Success - show success message
                    showSuccessMessage();
                }
            })
            .catch(error => {
                console.error('Error:', error);
                showFieldError(emailInput, 'An error occurred. Please try again.');
                setLoadingState(false);
            });
    });

    function setLoadingState(loading) {
        submitBtn.disabled = loading;

        if (loading) {
            btnText.style.opacity = '0';
            btnLoader.style.display = 'flex';
            submitBtn.style.cursor = 'not-allowed';
        } else {
            btnText.style.opacity = '1';
            btnLoader.style.display = 'none';
            submitBtn.style.cursor = 'pointer';
        }
    }

    function showSuccessMessage() {
        // Hide form and show success message
        form.style.opacity = '0';
        form.style.transform = 'translateY(-20px)';

        setTimeout(() => {
            form.style.display = 'none';
            successMessage.style.display = 'block';
            successMessage.style.opacity = '0';
            successMessage.style.transform = 'translateY(20px)';

            // Animate success message in
            setTimeout(() => {
                successMessage.style.opacity = '1';
                successMessage.style.transform = 'translateY(0)';
            }, 100);
        }, 300);

        // Add confetti effect
        createConfetti();
    }

    // Fun confetti effect for success
    function createConfetti() {
        const colors = ['#667eea', '#764ba2', '#48bb78', '#ed8936', '#e53e3e'];
        const container = document.querySelector('.form-container');

        for (let i = 0; i < 30; i++) {
            const confetti = document.createElement('div');
            confetti.style.position = 'absolute';
            confetti.style.width = '8px';
            confetti.style.height = '8px';
            confetti.style.backgroundColor = colors[Math.floor(Math.random() * colors.length)];
            confetti.style.borderRadius = '50%';
            confetti.style.pointerEvents = 'none';
            confetti.style.zIndex = '1000';

            const startX = Math.random() * container.offsetWidth;
            const startY = -10;
            const endX = startX + (Math.random() - 0.5) * 200;
            const endY = container.offsetHeight + 50;

            confetti.style.left = startX + 'px';
            confetti.style.top = startY + 'px';

            container.appendChild(confetti);

            // Animate confetti
            const duration = 2000 + Math.random() * 1000;
            const startTime = Date.now();

            function animateConfetti() {
                const elapsed = Date.now() - startTime;
                const progress = elapsed / duration;

                if (progress < 1) {
                    const currentX = startX + (endX - startX) * progress;
                    const currentY = startY + (endY - startY) * progress * progress;

                    confetti.style.left = currentX + 'px';
                    confetti.style.top = currentY + 'px';
                    confetti.style.opacity = 1 - progress;
                    confetti.style.transform = `rotate(${progress * 360}deg)`;

                    requestAnimationFrame(animateConfetti);
                } else {
                    container.removeChild(confetti);
                }
            }

            setTimeout(() => animateConfetti(), Math.random() * 500);
        }
    }

    // Enhanced input interactions
    const inputs = form.querySelectorAll('.form-input');
    inputs.forEach(input => {
        // Add ripple effect on focus
        input.addEventListener('focus', function () {
            const container = this.parentElement;
            const ripple = document.createElement('div');
            ripple.style.position = 'absolute';
            ripple.style.borderRadius = '50%';
            ripple.style.background = 'rgba(102, 126, 234, 0.3)';
            ripple.style.transform = 'scale(0)';
            ripple.style.animation = 'ripple 0.6s linear';
            ripple.style.pointerEvents = 'none';

            const rect = this.getBoundingClientRect();
            const containerRect = container.getBoundingClientRect();
            ripple.style.width = ripple.style.height = '20px';
            ripple.style.left = '20px';
            ripple.style.top = '16px';

            container.style.position = 'relative';
            container.appendChild(ripple);

            setTimeout(() => {
                if (container.contains(ripple)) {
                    container.removeChild(ripple);
                }
            }, 600);
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
        
        @keyframes shake {
            0%, 100% { transform: translateX(0); }
            10%, 30%, 50%, 70%, 90% { transform: translateX(-5px); }
            20%, 40%, 60%, 80% { transform: translateX(5px); }
        }
    `;
    document.head.appendChild(style);

    // Accessibility enhancements
    form.addEventListener('keydown', function (e) {
        // Enter key should submit form
        if (e.key === 'Enter' && e.target.type !== 'submit') {
            e.preventDefault();
            form.dispatchEvent(new Event('submit'));
        }

        // Escape key should clear current field
        if (e.key === 'Escape' && e.target.matches('.form-input')) {
            e.target.value = '';
            clearFieldError(e.target);
        }
    });

    // Auto-focus first input when page loads
    setTimeout(() => {
        emailInput.focus();
    }, 600);
});
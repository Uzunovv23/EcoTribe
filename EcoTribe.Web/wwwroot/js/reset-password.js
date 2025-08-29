document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('resetPasswordForm');
    const submitBtn = document.getElementById('submitBtn');
    const btnText = submitBtn.querySelector('.btn-text');
    const btnLoader = submitBtn.querySelector('.btn-loader');
    const passwordInput = document.getElementById('password');
    const confirmPasswordInput = document.getElementById('confirmPassword');
    const passwordToggle = document.getElementById('passwordToggle');
    const confirmPasswordToggle = document.getElementById('confirmPasswordToggle');
    const passwordStrength = document.getElementById('passwordStrength');
    const strengthFill = passwordStrength.querySelector('.strength-fill');
    const strengthText = passwordStrength.querySelector('.strength-text');
    const requirements = document.querySelectorAll('.requirement');

    // === Password visibility toggles ===
    function setupPasswordToggle(input, toggle) {
        toggle.addEventListener('click', function () {
            const type = input.getAttribute('type') === 'password' ? 'text' : 'password';
            input.setAttribute('type', type);
            const eyeIcon = toggle.querySelector('.eye-icon');
            if (type === 'text') {
                eyeIcon.innerHTML = `
                  <path d="M17.94 17.94A10.07 10.07 0 0 1 12 20c-7 0-11-8-11-8
                           a18.45 18.45 0 0 1 5.06-5.94M9.9 4.24A9.12 9.12 0 0 1 12 4
                           c7 0 11 8 11 8a18.5 18.5 0 0 1-2.16 3.19m-6.72-1.07
                           a3 3 0 1 1-4.24-4.24"></path>
                  <line x1="1" y1="1" x2="23" y2="23"></line>
                `;
            } else {
                eyeIcon.innerHTML = `
                  <path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"></path>
                  <circle cx="12" cy="12" r="3"></circle>
                `;
            }
        });
    }
    setupPasswordToggle(passwordInput, passwordToggle);
    setupPasswordToggle(confirmPasswordInput, confirmPasswordToggle);

    // === Password strength checker ===
    function checkPasswordStrength(password) {
        let score = 0;
        let feedback = '';

        if (password.length >= 8) score += 1;
        if (password.length >= 12) score += 1;
        if (/[a-z]/.test(password)) score += 1;
        if (/[A-Z]/.test(password)) score += 1;
        if (/[0-9]/.test(password)) score += 1;
        if (/[^A-Za-z0-9]/.test(password)) score += 1;

        if (score < 3) {
            strengthFill.className = 'strength-fill weak';
            feedback = 'Weak password';
        } else if (score < 4) {
            strengthFill.className = 'strength-fill fair';
            feedback = 'Fair password';
        } else if (score < 5) {
            strengthFill.className = 'strength-fill good';
            feedback = 'Good password';
        } else {
            strengthFill.className = 'strength-fill strong';
            feedback = 'Strong password';
        }
        strengthText.textContent = feedback;
        return score;
    }

    function updateRequirements(password) {
        const checks = {
            length: password.length >= 8,
            uppercase: /[A-Z]/.test(password),
            lowercase: /[a-z]/.test(password),
            number: /[0-9]/.test(password)
        };
        requirements.forEach(req => {
            const requirement = req.getAttribute('data-requirement');
            if (checks[requirement]) {
                req.classList.add('met');
            } else {
                req.classList.remove('met');
            }
        });
        return Object.values(checks).every(check => check);
    }

    passwordInput.addEventListener('input', function () {
        const password = this.value;
        if (password.length > 0) {
            passwordStrength.classList.add('show');
            checkPasswordStrength(password);
            updateRequirements(password);
        } else {
            passwordStrength.classList.remove('show');
            requirements.forEach(req => req.classList.remove('met'));
        }
        clearFieldError(this);
        if (confirmPasswordInput.value) {
            validatePasswordMatch();
        }
    });

    confirmPasswordInput.addEventListener('input', function () {
        clearFieldError(this);
        if (this.value) {
            validatePasswordMatch();
        }
    });

    function validatePasswordMatch() {
        const password = passwordInput.value;
        const confirmPassword = confirmPasswordInput.value;
        if (confirmPassword && password !== confirmPassword) {
            showFieldError(confirmPasswordInput, 'Passwords do not match');
            return false;
        }
        return true;
    }

    function validatePassword(password) {
        if (!password) return 'Password is required';
        if (password.length < 8) return 'Password must be at least 8 characters long';
        if (!/[A-Z]/.test(password)) return 'Password must contain at least one uppercase letter';
        if (!/[a-z]/.test(password)) return 'Password must contain at least one lowercase letter';
        if (!/[0-9]/.test(password)) return 'Password must contain at least one number';
        return null;
    }

    function showFieldError(field, message) {
        const errorSpan = field.parentElement.nextElementSibling;
        if (errorSpan && errorSpan.classList.contains('error-message')) {
            errorSpan.textContent = message;
            field.classList.add('input-validation-error');
            field.style.animation = 'shake 0.5s ease-in-out';
            setTimeout(() => { field.style.animation = ''; }, 500);
        }
    }

    function clearFieldError(field) {
        const errorSpan = field.parentElement.nextElementSibling;
        if (errorSpan && errorSpan.classList.contains('error-message')) {
            errorSpan.textContent = '';
            field.classList.remove('input-validation-error');
        }
    }

    // === Form submission (with redirect support) ===
    form.addEventListener('submit', function (e) {
        e.preventDefault();

        const password = passwordInput.value.trim();
        const confirmPassword = confirmPasswordInput.value.trim();
        let isValid = true;

        clearFieldError(passwordInput);
        clearFieldError(confirmPasswordInput);

        const passwordError = validatePassword(password);
        if (passwordError) {
            showFieldError(passwordInput, passwordError);
            isValid = false;
        }

        if (!confirmPassword) {
            showFieldError(confirmPasswordInput, 'Please confirm your password');
            isValid = false;
        } else if (password !== confirmPassword) {
            showFieldError(confirmPasswordInput, 'Passwords do not match');
            isValid = false;
        }

        if (!isValid) {
            const firstErrorField = form.querySelector('.input-validation-error');
            if (firstErrorField) {
                firstErrorField.focus();
            }
            return;
        }

        setLoadingState(true);

        const formData = new FormData(form);

        fetch(form.action, {
            method: 'POST',
            body: formData,
            headers: {
                'RequestVerificationToken':
                    document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
            }
        })
            .then(response => {
                if (response.redirected) {
                    // Server issued a redirect
                    window.location.href = response.url;
                    return null;
                }
                return response.text();
            })
            .then(data => {
                if (!data) return; // already redirected

                if (data.includes('validation-summary-errors') || data.includes('field-validation-error')) {
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
                    showSuccessAnimation();
                }
            })
            .catch(error => {
                console.error('Error:', error);
                showFieldError(passwordInput, 'An error occurred. Please try again.');
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

    function showSuccessAnimation() {
        const overlay = document.createElement('div');
        overlay.style.cssText = `
            position: fixed; top: 0; left: 0; width: 100%; height: 100%;
            background: rgba(72, 187, 120, 0.9);
            display: flex; align-items: center; justify-content: center;
            z-index: 10000; opacity: 0; transition: opacity 0.3s ease;
        `;
        const successIcon = document.createElement('div');
        successIcon.innerHTML = `
          <svg width="80" height="80" viewBox="0 0 24 24" fill="none" stroke="white" stroke-width="2">
            <path d="M22 11.08V12a10 10 0 1 1-5.93-9.14"></path>
            <polyline points="22,4 12,14.01 9,11.01"></polyline>
          </svg>
        `;
        successIcon.style.animation = 'bounce 0.6s ease-in-out';
        overlay.appendChild(successIcon);
        document.body.appendChild(overlay);
        setTimeout(() => { overlay.style.opacity = '1'; }, 100);
        setTimeout(() => { document.body.removeChild(overlay); }, 2000);
    }

    // === Enhanced input interactions (ripple, keyboard, etc.) ===
    const inputs = form.querySelectorAll('.form-input');
    inputs.forEach(input => {
        input.addEventListener('focus', function () {
            const container = this.parentElement;
            const ripple = document.createElement('div');
            ripple.style.cssText = `
                position: absolute; border-radius: 50%;
                background: rgba(102, 126, 234, 0.3);
                transform: scale(0);
                animation: ripple 0.6s linear;
                pointer-events: none;
                width: 20px; height: 20px;
                left: 20px; top: 18px;
            `;
            container.appendChild(ripple);
            setTimeout(() => { if (container.contains(ripple)) container.removeChild(ripple); }, 600);
        });
    });

    const style = document.createElement('style');
    style.textContent = `
        @keyframes ripple { to { transform: scale(4); opacity: 0; } }
        @keyframes shake {
            0%,100% { transform: translateX(0); }
            10%,30%,50%,70%,90% { transform: translateX(-5px); }
            20%,40%,60%,80% { transform: translateX(5px); }
        }
        @keyframes bounce {
            0%,20%,60%,100% { transform: translateY(0); }
            40% { transform: translateY(-20px); }
            80% { transform: translateY(-10px); }
        }
    `;
    document.head.appendChild(style);

    form.addEventListener('keydown', function (e) {
        if (e.key === 'Enter' && e.target.type !== 'submit') {
            e.preventDefault();
            form.dispatchEvent(new Event('submit'));
        }
        if (e.key === 'Escape' && e.target.matches('.form-input')) {
            e.target.value = '';
            clearFieldError(e.target);
            if (e.target === passwordInput) {
                passwordStrength.classList.remove('show');
                requirements.forEach(req => req.classList.remove('met'));
            }
        }
    });

    setTimeout(() => { passwordInput.focus(); }, 600);

    if (window.history.replaceState) {
        window.history.replaceState(null, null, window.location.href);
    }
});

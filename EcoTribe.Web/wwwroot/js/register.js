document.addEventListener('DOMContentLoaded', function () {
    const expandButton = document.querySelector('.expand-button');
    const additionalInfo = document.querySelector('.additional-info');
    const submitButton = document.querySelector('.submit-button');
    const form = document.querySelector('#registerForm');
    const sideNavToggle = document.querySelector('.side-nav-toggle');
    const sideNav = document.querySelector('.side-nav');
    const body = document.body;

    const togglePassword = document.querySelector('#togglePassword');
    const toggleConfirmPassword = document.querySelector('#toggleConfirmPassword');
    const password = document.querySelector('#Password');
    const confirmPassword = document.querySelector('#ConfirmPassword');

    sideNavToggle?.addEventListener('click', function () {
        this.classList.toggle('active');
        sideNav.classList.toggle('active');
        body.classList.toggle('nav-open');
    });

    document.addEventListener('click', function (e) {
        if (!sideNav.contains(e.target) && !sideNavToggle.contains(e.target)) {
            sideNavToggle.classList.remove('active');
            sideNav.classList.remove('active');
            body.classList.remove('nav-open');
        }
    });

    function togglePasswordVisibility(button, input) {
        button?.addEventListener('click', function () {
            const type = input.getAttribute('type') === 'password' ? 'text' : 'password';
            input.setAttribute('type', type);
            this.querySelector('i').classList.toggle('bi-eye');
            this.querySelector('i').classList.toggle('bi-eye-slash');
        });
    }

    togglePasswordVisibility(togglePassword, password);
    togglePasswordVisibility(toggleConfirmPassword, confirmPassword);

    const strengthBar = document.querySelector('.password-strength');
    const strengthText = document.querySelector('.password-strength-text');

    function checkPasswordStrength(password) {
        let strength = 0;
        const patterns = {
            length: password.length >= 8,
            lowercase: /[a-z]/.test(password),
            uppercase: /[A-Z]/.test(password),
            numbers: /\d/.test(password),
            special: /[!@#$%^&*(),.?":{}|<>]/.test(password)
        };

        strength = Object.values(patterns).filter(Boolean).length;

        const strengthLabels = ['Weak', 'Fair', 'Good', 'Strong', 'Very Strong'];
        const strengthColors = ['#dc3545', '#ffc107', '#28a745', '#198754', '#0d6efd'];

        if (strengthBar) {
            strengthBar.style.width = `${(strength / 5) * 100}%`;
            strengthBar.style.backgroundColor = strengthColors[strength - 1] || '';
        }
        if (strengthText) {
            strengthText.textContent = strength > 0 ? strengthLabels[strength - 1] : '';
        }
    }

    password?.addEventListener('input', function () {
        checkPasswordStrength(this.value);
    });

    expandButton?.addEventListener('click', function () {
        this.classList.toggle('expanded');
        additionalInfo.classList.toggle('expanded');

        setTimeout(() => {
            submitButton.classList.toggle('visible');
        }, additionalInfo.classList.contains('expanded') ? 300 : 0);
    });

    form?.addEventListener('submit', function (e) {
        if (!form.checkValidity()) {
            e.preventDefault();
            return;
        }

        const spinner = submitButton.querySelector('.spinner-border');
        const buttonText = submitButton.querySelector('span');

        submitButton.disabled = true;
        spinner.classList.remove('d-none');
        buttonText.textContent = 'Creating Account...';
    });

    const inputs = document.querySelectorAll('.form-control');
    inputs.forEach(input => {
        input.addEventListener('focus', function () {
            this.closest('.input-group')?.classList.add('focused');
        });

        input.addEventListener('blur', function () {
            this.closest('.input-group')?.classList.remove('focused');
        });
    });

    const currentLocation = window.location.pathname;
    const menuLinks = document.querySelectorAll('.side-nav-menu a');

    menuLinks.forEach(link => {
        const linkPath = link.getAttribute('href');
        if (linkPath === currentLocation ||
            (currentLocation === '/' && linkPath === '/Home') ||
            (currentLocation.includes('/Account/Login') && linkPath.includes('/Account/Login')) ||
            (currentLocation.includes('/Account/Register') && linkPath.includes('/Account/Register'))) {
            link.classList.add('active');
        }
    });
});
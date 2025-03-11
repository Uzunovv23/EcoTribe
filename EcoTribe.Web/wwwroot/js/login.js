document.addEventListener('DOMContentLoaded', function () {
    const togglePassword = document.querySelector('#togglePassword');
    const password = document.querySelector('#Password');

    togglePassword.addEventListener('click', function () {
        const type = password.getAttribute('type') === 'password' ? 'text' : 'password';
        password.setAttribute('type', type);
        this.querySelector('i').classList.toggle('bi-eye');
        this.querySelector('i').classList.toggle('bi-eye-slash');
    });

    const form = document.querySelector('#loginForm');
    const submitButton = form.querySelector('button[type="submit"]');
    const spinner = submitButton.querySelector('.spinner-border');
    const buttonText = submitButton.querySelector('span');

    form.addEventListener('submit', function (e) {
        if (!form.checkValidity()) {
            e.preventDefault();
            return;
        }

        submitButton.disabled = true;
        spinner.classList.remove('d-none');
        buttonText.textContent = 'Signing in...';
    });

    const inputs = document.querySelectorAll('.form-control');
    inputs.forEach(input => {
        input.addEventListener('focus', function () {
            this.closest('.input-group').classList.add('focused');
        });

        input.addEventListener('blur', function () {
            this.closest('.input-group').classList.remove('focused');
        });
    });
});

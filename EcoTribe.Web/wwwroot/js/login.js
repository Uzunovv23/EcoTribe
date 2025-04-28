document.addEventListener('DOMContentLoaded', function () {
    const togglePassword = document.querySelector('#togglePassword');
    const password = document.querySelector('#Password');
    const sideNavToggle = document.querySelector('.side-nav-toggle');
    const sideNav = document.querySelector('.side-nav');
    const navOverlay = document.querySelector('.nav-overlay');
    const body = document.body;

    sideNavToggle?.addEventListener('click', function () {
        this.classList.toggle('active');
        sideNav.classList.toggle('active');
        navOverlay.classList.toggle('active');
        body.classList.toggle('nav-open');
    });

    document.addEventListener('click', function (e) {
        if (!sideNav.contains(e.target) && !sideNavToggle.contains(e.target)) {
            sideNavToggle.classList.remove('active');
            sideNav.classList.remove('active');
            navOverlay.classList.remove('active');
            body.classList.remove('nav-open');
        }
    });

    togglePassword?.addEventListener('click', function () {
        const type = password.getAttribute('type') === 'password' ? 'text' : 'password';
        password.setAttribute('type', type);
        this.querySelector('i').classList.toggle('bi-eye');
        this.querySelector('i').classList.toggle('bi-eye-slash');
    });

    const form = document.querySelector('#loginForm');
    const submitButton = form.querySelector('button[type="submit"]');
    const spinner = submitButton.querySelector('.spinner-border');
    const buttonText = submitButton.querySelector('span');

    form?.addEventListener('submit', function (e) {
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
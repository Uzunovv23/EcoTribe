
document.addEventListener('DOMContentLoaded', function () {
    const form = document.querySelector('.org-create-form');
    const inputs = document.querySelectorAll('.form-group input, .form-group textarea');
    const descriptionField = document.getElementById('Description');
    const charCount = document.querySelector('.char-count span');
    const progressBar = document.querySelector('.progress-fill');
    const progressText = document.querySelector('.progress-text');

    const requiredFields = [
        document.getElementById('Name'),
        document.getElementById('ContactEmail'),
        document.getElementById('Description'),
        document.getElementById('City')
    ];

    const totalFields = document.querySelectorAll('.form-group input, .form-group textarea').length;

    updateProgress();

    inputs.forEach(input => {
        input.addEventListener('focus', function () {
            this.closest('.form-group').classList.add('active');
        });

        input.addEventListener('blur', function () {
            this.closest('.form-group').classList.remove('active');
            validateField(this);
        });

        input.addEventListener('input', function () {
            updateProgress();

            if (this.id === 'Description') {
                updateCharCount();
            }
        });
    });

    if (descriptionField) {
        updateCharCount();

        descriptionField.addEventListener('input', updateCharCount);
    }

    if (form) {
        form.addEventListener('submit', function (e) {
            let isValid = true;

            inputs.forEach(input => {
                if (!validateField(input)) {
                    isValid = false;
                }
            });

            if (!isValid) {
                e.preventDefault();
                showFormError("Please correct the errors in the form before submitting.");
            }
        });
    }

    function updateCharCount() {
        if (charCount && descriptionField) {
            charCount.textContent = descriptionField.value.length;
        }
    }

    function updateProgress() {
        if (!progressBar || !progressText) return;

        const filledRequiredFields = requiredFields.filter(field => field && field.value.trim() !== '').length;
        const requiredFieldsCount = requiredFields.length;

        const progress = Math.floor((filledRequiredFields / requiredFieldsCount) * 100);

        progressBar.style.width = `${progress}%`;
        progressText.textContent = `${progress}% Complete`;
    }

    function validateField(field) {
        if (!field) return true;

        const formGroup = field.closest('.form-group');
        let isValid = true;

        formGroup.classList.remove('is-invalid');

        if (field.hasAttribute('required') && field.value.trim() === '') {
            formGroup.classList.add('is-invalid');
            isValid = false;
        }

        if (field.type === 'email' && field.value.trim() !== '') {
            const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!emailPattern.test(field.value)) {
                formGroup.classList.add('is-invalid');
                isValid = false;
            }
        }

        if (field.type === 'url' && field.value.trim() !== '') {
            try {
                new URL(field.value);
            } catch (_) {
                formGroup.classList.add('is-invalid');
                isValid = false;
            }
        }

        return isValid;
    }

    function showFormError(message) {
        let errorContainer = document.querySelector('.form-error-message');

        if (!errorContainer) {
            errorContainer = document.createElement('div');
            errorContainer.className = 'form-error-message alert alert-danger mt-3';

            const formActions = document.querySelector('.form-actions');
            if (formActions) {
                formActions.parentNode.insertBefore(errorContainer, formActions);
            }
        }

        errorContainer.textContent = message;

        errorContainer.scrollIntoView({ behavior: 'smooth', block: 'center' });
    }

    function addTooltips() {
        const tooltipFields = [
            {
                id: 'Name',
                message: 'The official registered name of your organization'
            },
            {
                id: 'ContactEmail',
                message: 'This email will be used for communications'
            },
            {
                id: 'Description',
                message: 'Describe what your organization does and its environmental impact'
            }
        ];

        tooltipFields.forEach(field => {
            const input = document.getElementById(field.id);
            if (!input) return;

            const formGroup = input.closest('.form-group');
            const label = formGroup.querySelector('label');

            let inputWrapper = input.parentElement;
            if (!inputWrapper.classList.contains('input-wrapper')) {
                inputWrapper = document.createElement('div');
                inputWrapper.className = 'input-wrapper';
                input.parentNode.insertBefore(inputWrapper, input);
                inputWrapper.appendChild(input);
            }

            const tooltipIcon = document.createElement('div');
            tooltipIcon.className = input.tagName === 'TEXTAREA' ? 'tooltip-icon textarea-tooltip' : 'tooltip-icon';
            tooltipIcon.innerHTML = `
        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-info-circle" viewBox="0 0 16 16">
          <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z"/>
          <path d="m8.93 6.588-2.29.287-.082.38.45.083c.294.07.352.176.288.469l-.738 3.468c-.194.897.105 1.319.808 1.319.545 0 1.178-.252 1.465-.598l.088-.416c-.2.176-.492.246-.686.246-.275 0-.375-.193-.304-.533L8.93 6.588zM9 4.5a1 1 0 1 1-2 0 1 1 0 0 1 2 0z"/>
        </svg>
        <span class="tooltip">${field.message}</span>
      `;

            inputWrapper.appendChild(tooltipIcon);
        });
    }

    addTooltips();
});
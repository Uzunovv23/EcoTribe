document.addEventListener('DOMContentLoaded', function () {
    const form = document.querySelector('.event-edit-form');
    const inputs = document.querySelectorAll('.form-group input, .form-group textarea');
    const descriptionField = document.querySelector('textarea[name="Description"]');
    const charCount = document.querySelector('.char-count span');
    const startDate = document.querySelector('input[name="Start"]');
    const endDate = document.querySelector('input[name="End"]');

    inputs.forEach(input => {
        input.addEventListener('focus', function () {
            this.closest('.form-group').classList.add('active');
        });

        input.addEventListener('blur', function () {
            this.closest('.form-group').classList.remove('active');
            validateField(this);
        });

        if (input === descriptionField) {
            input.addEventListener('input', updateCharCount);
        }
    });

    if (descriptionField && charCount) {
        updateCharCount();
    }

    if (startDate && endDate) {
        startDate.addEventListener('change', validateDates);
        endDate.addEventListener('change', validateDates);
    }

    if (form) {
        form.addEventListener('submit', function (e) {
            let isValid = true;

            inputs.forEach(input => {
                if (!validateField(input)) {
                    isValid = false;
                }
            });

            if (!validateDates()) {
                isValid = false;
            }

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

    function validateDates() {
        if (!startDate || !endDate) return true;

        const start = new Date(startDate.value);
        const end = new Date(endDate.value);

        if (end <= start) {
            showFieldError(endDate, 'End date must be after start date');
            return false;
        }

        clearFieldError(endDate);
        return true;
    }

    function validateField(field) {
        if (!field) return true;

        const formGroup = field.closest('.form-group');
        let isValid = true;

        clearFieldError(field);

        if (field.hasAttribute('required') && field.value.trim() === '') {
            showFieldError(field, 'This field is required');
            isValid = false;
        }

        if (field.type === 'number') {
            const value = parseFloat(field.value);
            if (field.hasAttribute('min') && value < parseFloat(field.getAttribute('min'))) {
                showFieldError(field, `Value must be at least ${field.getAttribute('min')}`);
                isValid = false;
            }
        }

        if (field.name === 'Latitude') {
            const lat = parseFloat(field.value);
            if (isNaN(lat) || lat < -90 || lat > 90) {
                showFieldError(field, 'Latitude must be between -90 and 90');
                isValid = false;
            }
        }

        if (field.name === 'Longitude') {
            const lng = parseFloat(field.value);
            if (isNaN(lng) || lng < -180 || lng > 180) {
                showFieldError(field, 'Longitude must be between -180 and 180');
                isValid = false;
            }
        }

        return isValid;
    }

    function showFieldError(field, message) {
        const formGroup = field.closest('.form-group');
        formGroup.classList.add('is-invalid');

        let errorSpan = formGroup.querySelector('.text-danger');
        if (!errorSpan) {
            errorSpan = document.createElement('span');
            errorSpan.className = 'text-danger';
            formGroup.appendChild(errorSpan);
        }
        errorSpan.textContent = message;
    }

    function clearFieldError(field) {
        const formGroup = field.closest('.form-group');
        formGroup.classList.remove('is-invalid');

        const errorSpan = formGroup.querySelector('.text-danger');
        if (errorSpan) {
            errorSpan.textContent = '';
        }
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
});
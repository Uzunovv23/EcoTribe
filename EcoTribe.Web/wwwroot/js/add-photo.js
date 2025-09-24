document.addEventListener('DOMContentLoaded', function () {
    const uploadZone = document.getElementById('uploadZone');
    const photoInput = document.getElementById('photoInput');
    const previewArea = document.getElementById('photoPreviewArea');
    const previewGrid = document.getElementById('previewGrid');
    const uploadButton = document.getElementById('uploadButton');
    const uploadForm = document.getElementById('photoUploadForm');
    const progressContainer = document.getElementById('uploadProgress');
    const progressFill = document.getElementById('progressFill');
    const progressText = document.getElementById('progressText');

    let selectedFiles = [];

    initDragAndDrop();

    photoInput.addEventListener('change', handleFileSelect);

    uploadForm.addEventListener('submit', handleFormSubmit);

    function initDragAndDrop() {
        ['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
            uploadZone.addEventListener(eventName, preventDefaults, false);
            document.body.addEventListener(eventName, preventDefaults, false);
        });

        ['dragenter', 'dragover'].forEach(eventName => {
            uploadZone.addEventListener(eventName, highlight, false);
        });

        ['dragleave', 'drop'].forEach(eventName => {
            uploadZone.addEventListener(eventName, unhighlight, false);
        });

        uploadZone.addEventListener('drop', handleDrop, false);
    }

    function preventDefaults(e) {
        e.preventDefault();
        e.stopPropagation();
    }

    function highlight(e) {
        uploadZone.classList.add('drag-over');
    }

    function unhighlight(e) {
        uploadZone.classList.remove('drag-over');
    }

    function handleDrop(e) {
        const dt = e.dataTransfer;
        const files = dt.files;
        handleFiles(files);
    }

    function handleFileSelect(e) {
        const files = e.target.files;
        handleFiles(files);
    }

    function handleFiles(files) {
        selectedFiles = [];
        previewGrid.innerHTML = '';

        [...files].forEach(file => {
            if (isValidFile(file)) {
                selectedFiles.push(file);
                createPreview(file);
            }
        });

        updateUI();
    }

    function isValidFile(file) {
        const validTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/gif'];
        const maxSize = 10 * 1024 * 1024; 

        if (!validTypes.includes(file.type)) {
            showError(`${file.name} is not a valid image format. Please use JPG, PNG, or GIF.`);
            return false;
        }

        if (file.size > maxSize) {
            showError(`${file.name} is too large. Maximum file size is 10MB.`);
            return false;
        }

        return true;
    }

    function createPreview(file) {
        const reader = new FileReader();

        reader.onload = function (e) {
            const previewItem = document.createElement('div');
            previewItem.className = 'preview-item';
            previewItem.innerHTML = `
                <img src="${e.target.result}" alt="Preview" class="preview-image">
                <button type="button" class="preview-remove" onclick="removePreview(this, '${file.name}')">
                    <i class="bi bi-x"></i>
                </button>
            `;

            previewGrid.appendChild(previewItem);

            setTimeout(() => {
                previewItem.style.opacity = '1';
                previewItem.style.transform = 'scale(1)';
            }, 100);
        };

        reader.readAsDataURL(file);
    }

    window.removePreview = function (button, fileName) {
        const previewItem = button.closest('.preview-item');

        selectedFiles = selectedFiles.filter(file => file.name !== fileName);

        previewItem.style.opacity = '0';
        previewItem.style.transform = 'scale(0.8)';

        setTimeout(() => {
            previewItem.remove();
            updateUI();
        }, 300);
    };

    function updateUI() {
        if (selectedFiles.length > 0) {
            previewArea.style.display = 'block';
            uploadButton.disabled = false;
            uploadButton.innerHTML = `
                <i class="bi bi-upload"></i>
                <span>Upload ${selectedFiles.length} Photo${selectedFiles.length > 1 ? 's' : ''}</span>
            `;
        } else {
            previewArea.style.display = 'none';
            uploadButton.disabled = true;
            uploadButton.innerHTML = `
                <i class="bi bi-upload"></i>
                <span>Upload Photos</span>
            `;
        }
    }

    function handleFormSubmit(e) {
        e.preventDefault();

        if (selectedFiles.length === 0) {
            showError('Please select at least one photo to upload.');
            return;
        }

        progressContainer.style.display = 'block';
        uploadButton.disabled = true;

        simulateUpload();
    }

    function simulateUpload() {
        let progress = 0;
        const interval = setInterval(() => {
            progress += Math.random() * 15;

            if (progress >= 100) {
                progress = 100;
                clearInterval(interval);

                setTimeout(() => {
                    uploadForm.removeEventListener('submit', handleFormSubmit);
                    uploadForm.submit();
                }, 500);
            }

            updateProgress(progress);
        }, 200);
    }

    function updateProgress(progress) {
        progressFill.style.width = `${progress}%`;
        progressText.textContent = `Uploading... ${Math.round(progress)}%`;

        if (progress >= 100) {
            progressText.textContent = 'Upload Complete! Redirecting...';
        }
    }

    function showError(message) {
        const errorContainer = document.querySelector('.validation-messages');
        errorContainer.innerHTML = `<span class="validation-error">${message}</span>`;

        setTimeout(() => {
            errorContainer.innerHTML = '';
        }, 5000);
    }
    function initPageAnimations() {
        const elements = document.querySelectorAll('.upload-card, .upload-tips');
        elements.forEach((element, index) => {
            element.style.opacity = '0';
            element.style.transform = 'translateY(30px)';

            setTimeout(() => {
                element.style.transition = 'all 0.6s ease-out';
                element.style.opacity = '1';
                element.style.transform = 'translateY(0)';
            }, index * 200);
        });
    }

    initPageAnimations();

    uploadZone.addEventListener('click', () => {
        photoInput.click();
    });

    uploadZone.addEventListener('keydown', (e) => {
        if (e.key === 'Enter' || e.key === ' ') {
            e.preventDefault();
            photoInput.click();
        }
    });
});
document.addEventListener('DOMContentLoaded', function () {
    let deleteId = null;
    const searchInput = document.getElementById('organizationSearch');
    const rows = document.querySelectorAll('.eco-table tbody tr');
    const emptyState = document.querySelector('.eco-empty-state');
    const deleteModal = new bootstrap.Modal(document.getElementById('deleteConfirmationModal'));
    const confirmDeleteBtn = document.getElementById('confirmDeleteBtn');

    if (searchInput) {
        searchInput.addEventListener('input', function () {
            const searchTerm = this.value.toLowerCase().trim();
            let visibleCount = 0;

            rows.forEach(row => {
                const name = row.getAttribute('data-name').toLowerCase();
                const city = row.getAttribute('data-city').toLowerCase();

                if (name.includes(searchTerm) || city.includes(searchTerm)) {
                    row.style.display = '';
                    visibleCount++;
                    highlightMatches(row, searchTerm);
                } else {
                    row.style.display = 'none';
                }
            });

            if (visibleCount === 0) {
                emptyState.style.display = 'block';
            } else {
                emptyState.style.display = 'none';
            }
        });
    }

    function highlightMatches(row, searchTerm) {
        if (!searchTerm) {
            row.querySelectorAll('td').forEach(cell => {
                cell.innerHTML = cell.innerHTML.replace(/<span class="highlight">([^<]+)<\/span>/g, '$1');
            });
            return;
        }

        row.querySelectorAll('td').forEach(cell => {
            const text = cell.textContent;
            if (text.toLowerCase().includes(searchTerm)) {
                const regex = new RegExp(`(${searchTerm})`, 'gi');
                cell.innerHTML = text.replace(regex, '<span class="highlight">$1</span>');
            }
        });
    }

    if (confirmDeleteBtn) {
        confirmDeleteBtn.addEventListener('click', function () {
            if (deleteId !== null) {
                const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

                this.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Deleting...';
                this.disabled = true;

                fetch(`/Organization/DeleteConfirmed`, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/x-www-form-urlencoded",
                        "RequestVerificationToken": token
                    },
                    body: new URLSearchParams({ id: deleteId })
                })
                    .then(response => {
                        if (!response.ok) {
                            throw new Error("Failed to delete organization.");
                        }
                        return response.text();
                    })
                    .then(() => {
                        const row = document.getElementById(`organization-row-${deleteId}`);
                        if (row) {
                            row.style.opacity = '0';
                            row.style.transform = 'scale(0.9)';

                            setTimeout(() => {
                                row.remove();

                                const remainingRows = document.querySelectorAll('.eco-table tbody tr[style*="display: table-row"], .eco-table tbody tr:not([style*="display"])');
                                if (remainingRows.length === 0) {
                                    emptyState.style.display = 'block';
                                }
                            }, 300);
                        }

                        deleteId = null;
                        deleteModal.hide();
                        showToast('Organization deleted successfully', 'success');

                        this.innerHTML = 'Delete';
                        this.disabled = false;
                    })
                    .catch(error => {
                        console.error(error);
                        showToast(error.message, 'error');

                        this.innerHTML = 'Delete';
                        this.disabled = false;
                    });
            }
        });
    }

    function showToast(message, type = 'info') {
        let toastContainer = document.querySelector('.toast-container');
        if (!toastContainer) {
            toastContainer = document.createElement('div');
            toastContainer.className = 'toast-container position-fixed bottom-0 end-0 p-3';
            document.body.appendChild(toastContainer);
        }

        const toastEl = document.createElement('div');
        toastEl.className = `toast align-items-center text-white bg-${type === 'success' ? 'success' : type === 'error' ? 'danger' : 'primary'} border-0`;
        toastEl.setAttribute('role', 'alert');
        toastEl.setAttribute('aria-live', 'assertive');
        toastEl.setAttribute('aria-atomic', 'true');

        toastEl.innerHTML = `
            <div class="d-flex">
                <div class="toast-body">
                    ${message}
                </div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
        `;

        toastContainer.appendChild(toastEl);

        const toast = new bootstrap.Toast(toastEl, {
            autohide: true,
            delay: 3000
        });
        toast.show();

        toastEl.addEventListener('hidden.bs.toast', function () {
            toastEl.remove();
        });
    }
});

function confirmDelete(id, name) {
    deleteId = id;

    const nameElement = document.getElementById('organizationName');
    if (nameElement) {
        nameElement.textContent = name || 'this organization';
    }

    const deleteModal = new bootstrap.Modal(document.getElementById('deleteConfirmationModal'));
    deleteModal.show();
}
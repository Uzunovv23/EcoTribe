document.addEventListener('DOMContentLoaded', function () {
    initSideNavigation();
    initOrganizationAnimations();
    initStatusFilter();
    initActionButtons();
    initAlerts();
});

function initSideNavigation() {
    const sideNavToggle = document.querySelector('.side-nav-toggle');
    const sideNav = document.querySelector('.side-nav');
    const body = document.body;

    if (!sideNavToggle || !sideNav) return;

    sideNavToggle.addEventListener('click', function () {
        sideNavToggle.classList.toggle('active');
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

    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape') {
            sideNavToggle.classList.remove('active');
            sideNav.classList.remove('active');
            body.classList.remove('nav-open');
        }
    });
}

function initOrganizationAnimations() {
    const organizationCards = document.querySelectorAll('.organization-card');

    if (organizationCards.length === 0) return;

    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver((entries) => {
        entries.forEach((entry, index) => {
            if (entry.isIntersecting) {
                setTimeout(() => {
                    entry.target.style.opacity = '1';
                    entry.target.style.transform = 'translateY(0)';
                }, index * 100);
                observer.unobserve(entry.target);
            }
        });
    }, observerOptions);

    organizationCards.forEach(card => {
        card.style.opacity = '0';
        card.style.transform = 'translateY(30px)';
        card.style.transition = 'opacity 0.6s ease, transform 0.6s ease';
        observer.observe(card);
    });
}

function initStatusFilter() {
    const filterButtons = document.querySelectorAll('input[name="statusFilter"]');
    const organizationCards = document.querySelectorAll('.organization-card');

    filterButtons.forEach(button => {
        button.addEventListener('change', function () {
            const selectedStatus = this.value;

            organizationCards.forEach((card, index) => {
                const cardStatus = card.dataset.status;

                if (selectedStatus === 'all' || cardStatus === selectedStatus) {
                    setTimeout(() => {
                        card.style.display = 'block';
                        card.classList.remove('hidden');
                        card.classList.add('visible');
                    }, index * 50);
                } else {
                    card.classList.remove('visible');
                    card.classList.add('hidden');
                    setTimeout(() => {
                        card.style.display = 'none';
                    }, 300);
                }
            });

            updateFilterButtonAnimation(this);
        });
    });
}

function updateFilterButtonAnimation(activeButton) {
    const allFilterButtons = document.querySelectorAll('.btn-group .btn');

    allFilterButtons.forEach(btn => {
        btn.style.transform = 'scale(1)';
    });

    const activeLabel = activeButton.nextElementSibling;
    activeLabel.style.transform = 'scale(1.05)';

    setTimeout(() => {
        activeLabel.style.transform = 'scale(1)';
    }, 200);
}

function initActionButtons() {
    const actionButtons = document.querySelectorAll('.btn-success, .btn-danger, .btn-warning');

    actionButtons.forEach(button => {
        button.addEventListener('click', function (e) {
            if (this.dataset.loading === 'true') {
                e.preventDefault();
                return;
            }

            const originalText = this.innerHTML;
            this.dataset.loading = 'true';
            this.disabled = true;
            this.innerHTML = '<i class="bi bi-arrow-clockwise spin me-1"></i>Processing...';

            setTimeout(() => {
                this.dataset.loading = 'false';
                this.disabled = false;
                this.innerHTML = originalText;
            }, 3000);
        });

        button.addEventListener('mouseenter', function () {
            this.style.transform = 'translateY(-2px) scale(1.02)';
        });

        button.addEventListener('mouseleave', function () {
            this.style.transform = 'translateY(0) scale(1)';
        });
    });

    initConfirmationDialogs();
}

function initConfirmationDialogs() {
    const approveButtons = document.querySelectorAll('.btn-success');
    approveButtons.forEach(button => {
        if (button.innerHTML.includes('Approve')) {
            button.addEventListener('click', function (e) {
                e.preventDefault();
                showConfirmationDialog(
                    'Approve Organization',
                    'Are you sure you want to approve this environmental organization?',
                    'This will allow them to create and manage environmental events.',
                    'success',
                    'bi-check-circle',
                    'Approve',
                    () => this.closest('form').submit()
                );
            });
        }
    });

    const disapproveButtons = document.querySelectorAll('.btn-danger');
    disapproveButtons.forEach(button => {
        if (button.innerHTML.includes('Disapprove')) {
            button.addEventListener('click', function (e) {
                e.preventDefault();
                showConfirmationDialog(
                    'Disapprove Organization',
                    'Are you sure you want to disapprove this organization?',
                    'They will not be able to participate in the environmental platform.',
                    'danger',
                    'bi-x-circle',
                    'Disapprove',
                    () => this.closest('form').submit()
                );
            });
        }
    });

    const changeStatusButtons = document.querySelectorAll('.btn-warning');
    changeStatusButtons.forEach(button => {
        if (button.innerHTML.includes('Change Status')) {
            button.addEventListener('click', function (e) {
                e.preventDefault();
                showConfirmationDialog(
                    'Change Organization Status',
                    'Are you sure you want to change this organization\'s status?',
                    'This will toggle between approved and disapproved status.',
                    'warning',
                    'bi-arrow-repeat',
                    'Change Status',
                    () => this.closest('form').submit()
                );
            });
        }
    });
}

function showConfirmationDialog(title, message, description, type, icon, actionText, onConfirm) {
    const confirmDialog = document.createElement('div');
    confirmDialog.className = 'custom-confirm-dialog';

    const typeColors = {
        success: 'text-success',
        danger: 'text-danger',
        warning: 'text-warning'
    };

    const buttonClasses = {
        success: 'btn-success',
        danger: 'btn-danger',
        warning: 'btn-warning'
    };

    confirmDialog.innerHTML = `
        <div class="confirm-overlay">
            <div class="confirm-box">
                <div class="confirm-header">
                    <i class="bi ${icon} ${typeColors[type]}"></i>
                    <h5>${title}</h5>
                </div>
                <div class="confirm-body">
                    <p>${message}</p>
                    <p class="text-muted small">${description}</p>
                </div>
                <div class="confirm-footer">
                    <button class="btn btn-outline-secondary cancel-btn">Cancel</button>
                    <button class="btn ${buttonClasses[type]} confirm-btn">${actionText}</button>
                </div>
            </div>
        </div>
    `;

    document.body.appendChild(confirmDialog);

    confirmDialog.querySelector('.confirm-btn').addEventListener('click', () => {
        document.body.removeChild(confirmDialog);
        onConfirm();
    });

    confirmDialog.querySelector('.cancel-btn').addEventListener('click', () => {
        document.body.removeChild(confirmDialog);
    });

    confirmDialog.querySelector('.confirm-overlay').addEventListener('click', (e) => {
        if (e.target === confirmDialog.querySelector('.confirm-overlay')) {
            document.body.removeChild(confirmDialog);
        }
    });

    const handleEscape = (e) => {
        if (e.key === 'Escape') {
            document.body.removeChild(confirmDialog);
            document.removeEventListener('keydown', handleEscape);
        }
    };
    document.addEventListener('keydown', handleEscape);
}

function initAlerts() {
    const alerts = document.querySelectorAll('.alert');

    alerts.forEach(alert => {
        setTimeout(() => {
            if (alert.parentNode) {
                alert.style.opacity = '0';
                alert.style.transform = 'translateY(-20px)';

                setTimeout(() => {
                    if (alert.parentNode) {
                        alert.parentNode.removeChild(alert);
                    }
                }, 300);
            }
        }, 5000);
    });
}

function initSearchFilter() {
    const searchInput = document.createElement('input');
    searchInput.type = 'text';
    searchInput.className = 'form-control mb-3';
    searchInput.placeholder = 'Search organizations...';
    searchInput.style.borderRadius = '25px';

    const statusFilter = document.querySelector('.status-filter');
    if (statusFilter) {
        statusFilter.appendChild(searchInput);
    }

    searchInput.addEventListener('input', function () {
        const searchTerm = this.value.toLowerCase();
        const organizationCards = document.querySelectorAll('.organization-card');

        organizationCards.forEach(card => {
            const orgName = card.querySelector('.card-title').textContent.toLowerCase();
            const orgDescription = card.querySelector('.card-text').textContent.toLowerCase();

            if (orgName.includes(searchTerm) || orgDescription.includes(searchTerm)) {
                card.style.display = 'block';
                card.classList.remove('hidden');
                card.classList.add('visible');
            } else {
                card.classList.remove('visible');
                card.classList.add('hidden');
                setTimeout(() => {
                    card.style.display = 'none';
                }, 300);
            }
        });
    });
}

function initOrganizationCardInteractions() {
    const organizationCards = document.querySelectorAll('.organization-card');

    organizationCards.forEach(card => {
        card.addEventListener('mouseenter', function () {
            this.style.boxShadow = '0 15px 35px rgba(46, 204, 113, 0.2)';

            const cardElement = this.querySelector('.card');
            cardElement.style.borderLeft = '4px solid var(--primary-color)';
        });

        card.addEventListener('mouseleave', function () {
            this.style.boxShadow = '';

            const cardElement = this.querySelector('.card');
            cardElement.style.borderLeft = '';
        });

        const cardHeader = card.querySelector('.card-header');
        cardHeader.style.cursor = 'pointer';

        cardHeader.addEventListener('click', function () {
            const cardBody = card.querySelector('.card-body');
            const isExpanded = cardBody.style.maxHeight !== '' && cardBody.style.maxHeight !== '0px';

            if (!isExpanded) {
                cardBody.style.maxHeight = cardBody.scrollHeight + 'px';
                cardBody.style.overflow = 'visible';
                this.style.borderBottom = '2px solid var(--primary-color)';
            } else {
                cardBody.style.maxHeight = '0px';
                cardBody.style.overflow = 'hidden';
                this.style.borderBottom = '';
            }
        });
    });
}

const style = document.createElement('style');
style.textContent = `
    .spin {
        animation: spin 1s linear infinite;
    }
    
    @keyframes spin {
        from { transform: rotate(0deg); }
        to { transform: rotate(360deg); }
    }
    
    .custom-confirm-dialog {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        z-index: 2000;
        animation: fadeIn 0.3s ease;
    }
    
    .confirm-overlay {
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, 0.5);
        display: flex;
        align-items: center;
        justify-content: center;
        padding: 1rem;
    }
    
    .confirm-box {
        background: white;
        border-radius: 15px;
        max-width: 450px;
        width: 100%;
        box-shadow: 0 20px 40px rgba(0, 0, 0, 0.3);
        animation: slideInUp 0.3s ease;
    }
    
    .confirm-header {
        padding: 1.5rem 1.5rem 0.5rem;
        text-align: center;
    }
    
    .confirm-header i {
        font-size: 2.5rem;
        margin-bottom: 0.5rem;
    }
    
    .confirm-header h5 {
        margin: 0;
        color: var(--dark-color);
        font-weight: 600;
    }
    
    .confirm-body {
        padding: 1rem 1.5rem;
        text-align: center;
    }
    
    .confirm-footer {
        padding: 1rem 1.5rem 1.5rem;
        display: flex;
        gap: 1rem;
        justify-content: center;
    }
    
    .confirm-footer .btn {
        flex: 1;
    }
    
    @keyframes fadeIn {
        from { opacity: 0; }
        to { opacity: 1; }
    }
    
    @keyframes slideInUp {
        from {
            opacity: 0;
            transform: translateY(30px);
        }
        to {
            opacity: 1;
            transform: translateY(0);
        }
    }
    
    .organization-card.hidden {
        opacity: 0;
        transform: scale(0.8);
        transition: all 0.3s ease;
    }
    
    .organization-card.visible {
        opacity: 1;
        transform: scale(1);
        transition: all 0.3s ease;
    }
`;
document.head.appendChild(style);

initSearchFilter();
initOrganizationCardInteractions();
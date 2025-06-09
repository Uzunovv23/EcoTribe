document.addEventListener('DOMContentLoaded', function () {
    initSideNavigation();
    initUserManagementAnimations();
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

function initUserManagementAnimations() {
    const userCards = document.querySelectorAll('.user-group-card');

    if (userCards.length === 0) return;

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
                }, index * 200);
                observer.unobserve(entry.target);
            }
        });
    }, observerOptions);

    userCards.forEach(card => {
        card.style.opacity = '0';
        card.style.transform = 'translateY(30px)';
        card.style.transition = 'opacity 0.6s ease, transform 0.6s ease';
        observer.observe(card);
    });

    const tableRows = document.querySelectorAll('tbody tr');
    tableRows.forEach((row, index) => {
        row.style.opacity = '0';
        row.style.transform = 'translateX(-20px)';
        row.style.transition = 'opacity 0.4s ease, transform 0.4s ease';

        setTimeout(() => {
            row.style.opacity = '1';
            row.style.transform = 'translateX(0)';
        }, 500 + (index * 100));
    });
}

function initActionButtons() {
    const actionButtons = document.querySelectorAll('.action-btn');

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

    const dangerButtons = document.querySelectorAll('.btn-outline-danger');
    dangerButtons.forEach(button => {
        button.addEventListener('click', function (e) {
            e.preventDefault();

            const confirmDialog = document.createElement('div');
            confirmDialog.className = 'custom-confirm-dialog';
            confirmDialog.innerHTML = `
                <div class="confirm-overlay">
                    <div class="confirm-box">
                        <div class="confirm-header">
                            <i class="bi bi-exclamation-triangle text-warning"></i>
                            <h5>Confirm Action</h5>
                        </div>
                        <div class="confirm-body">
                            <p>Are you sure you want to remove the organizer role from this user?</p>
                            <p class="text-muted small">This action will revoke their ability to manage events and organizations.</p>
                        </div>
                        <div class="confirm-footer">
                            <button class="btn btn-outline-secondary cancel-btn">Cancel</button>
                            <button class="btn btn-danger confirm-btn">Remove Role</button>
                        </div>
                    </div>
                </div>
            `;

            document.body.appendChild(confirmDialog);

            confirmDialog.querySelector('.confirm-btn').addEventListener('click', () => {
                document.body.removeChild(confirmDialog);
                this.closest('form').submit();
            });

            confirmDialog.querySelector('.cancel-btn').addEventListener('click', () => {
                document.body.removeChild(confirmDialog);
            });

            confirmDialog.querySelector('.confirm-overlay').addEventListener('click', (e) => {
                if (e.target === confirmDialog.querySelector('.confirm-overlay')) {
                    document.body.removeChild(confirmDialog);
                }
            });
        });
    });

    const successButtons = document.querySelectorAll('.btn-outline-success');
    successButtons.forEach(button => {
        button.addEventListener('click', function (e) {
            e.preventDefault();

            const confirmDialog = document.createElement('div');
            confirmDialog.className = 'custom-confirm-dialog';
            confirmDialog.innerHTML = `
                <div class="confirm-overlay">
                    <div class="confirm-box">
                        <div class="confirm-header">
                            <i class="bi bi-person-plus text-success"></i>
                            <h5>Promote User</h5>
                        </div>
                        <div class="confirm-body">
                            <p>Are you sure you want to promote this user to organizer?</p>
                            <p class="text-muted small">This will grant them permissions to manage environmental events and organizations.</p>
                        </div>
                        <div class="confirm-footer">
                            <button class="btn btn-outline-secondary cancel-btn">Cancel</button>
                            <button class="btn btn-success confirm-btn">Promote User</button>
                        </div>
                    </div>
                </div>
            `;

            document.body.appendChild(confirmDialog);

            confirmDialog.querySelector('.confirm-btn').addEventListener('click', () => {
                document.body.removeChild(confirmDialog);
                this.closest('form').submit();
            });

            confirmDialog.querySelector('.cancel-btn').addEventListener('click', () => {
                document.body.removeChild(confirmDialog);
            });

            confirmDialog.querySelector('.confirm-overlay').addEventListener('click', (e) => {
                if (e.target === confirmDialog.querySelector('.confirm-overlay')) {
                    document.body.removeChild(confirmDialog);
                }
            });
        });
    });
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
        max-width: 400px;
        width: 100%;
        box-shadow: 0 20px 40px rgba(0, 0, 0, 0.3);
        animation: slideInUp 0.3s ease;
    }
    
    .confirm-header {
        padding: 1.5rem 1.5rem 0.5rem;
        text-align: center;
    }
    
    .confirm-header i {
        font-size: 2rem;
        margin-bottom: 0.5rem;
    }
    
    .confirm-header h5 {
        margin: 0;
        color: var(--dark-color);
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
`;
document.head.appendChild(style);

function addUserManagementEffects() {
    const userRows = document.querySelectorAll('tbody tr');
    userRows.forEach(row => {
        row.addEventListener('mouseenter', function () {
            this.style.backgroundColor = 'rgba(46, 204, 113, 0.05)';
            this.style.borderLeft = '4px solid var(--primary-color)';
        });

        row.addEventListener('mouseleave', function () {
            this.style.backgroundColor = '';
            this.style.borderLeft = '';
        });
    });

    const badges = document.querySelectorAll('.badge');
    badges.forEach(badge => {
        badge.addEventListener('mouseenter', function () {
            this.style.transform = 'scale(1.1)';
        });

        badge.addEventListener('mouseleave', function () {
            this.style.transform = 'scale(1)';
        });
    });
}

addUserManagementEffects();
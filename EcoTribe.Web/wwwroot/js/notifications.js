document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.timeago').forEach(element => {
        const timestamp = new Date(element.dataset.timestamp);
        element.textContent = timeAgo(timestamp);
    });

    document.querySelectorAll('.mark-read').forEach(button => {
        button.addEventListener('click', async function () {
            const id = this.dataset.id;
            try {
                const response = await fetch(`/Notification/MarkAsRead/${id}`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    }
                });

                if (response.ok) {
                    const row = this.closest('tr');
                    row.classList.remove('unread');
                    this.parentElement.innerHTML = `
                        <span class="status-read">
                            <i class="bi bi-check2-all"></i> Read
                        </span>`;
                    updateNotificationCount();
                }
            } catch (error) {
                console.error('Error marking notification as read:', error);
            }
        });
    });

    const markAllButton = document.getElementById('markAllRead');
    if (markAllButton) {
        markAllButton.addEventListener('click', async function () {
            try {
                const response = await fetch('/Notification/MarkAllAsRead', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    }
                });

                if (response.ok) {
                    document.querySelectorAll('tr.unread').forEach(row => {
                        row.classList.remove('unread');
                        const actionCell = row.querySelector('td:last-child');
                        actionCell.innerHTML = `
                            <span class="status-read">
                                <i class="bi bi-check2-all"></i> Read
                            </span>`;
                    });
                    this.style.display = 'none';
                    updateNotificationCount();
                }
            } catch (error) {
                console.error('Error marking all notifications as read:', error);
            }
        });
    }

    function updateNotificationCount() {
        fetch('/Notification/UnreadCount')
            .then(response => response.json())
            .then(count => {
                const countElement = document.getElementById('notification-count');
                if (count > 0) {
                    countElement.textContent = count;
                    countElement.classList.remove('d-none');
                } else {
                    countElement.classList.add('d-none');
                }
            });
    }

    function timeAgo(date) {
        const seconds = Math.floor((new Date() - date) / 1000);

        let interval = Math.floor(seconds / 31536000);
        if (interval >= 1) {
            return interval + ' year' + (interval === 1 ? '' : 's') + ' ago';
        }

        interval = Math.floor(seconds / 2592000);
        if (interval >= 1) {
            return interval + ' month' + (interval === 1 ? '' : 's') + ' ago';
        }

        interval = Math.floor(seconds / 86400);
        if (interval >= 1) {
            return interval + ' day' + (interval === 1 ? '' : 's') + ' ago';
        }

        interval = Math.floor(seconds / 3600);
        if (interval >= 1) {
            return interval + ' hour' + (interval === 1 ? '' : 's') + ' ago';
        }

        interval = Math.floor(seconds / 60);
        if (interval >= 1) {
            return interval + ' minute' + (interval === 1 ? '' : 's') + ' ago';
        }

        return seconds < 10 ? 'just now' : Math.floor(seconds) + ' seconds ago';
    }
});
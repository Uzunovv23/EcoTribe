const eventSearch = document.getElementById('eventSearch');
const eventTypeFilter = document.getElementById('eventTypeFilter');
const eventCityFilter = document.getElementById('eventCityFilter');
let eventCards = document.querySelectorAll('.event-card-wrapper');
let eventMaps = new Map();

document.addEventListener('DOMContentLoaded', function () {
    initAnimations();

    populateFilters();

    setupEventListeners();

    initEventMaps();
});

function initAnimations() {
    const animatedElements = document.querySelectorAll('.animate-on-scroll');

    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('show');

                const mapContainer = entry.target.querySelector('.event-map');
                if (mapContainer) {
                    const map = eventMaps.get(mapContainer.id);
                    if (map) {
                        map.invalidateSize();
                    }
                }
            }
        });
    }, {
        threshold: 0.1
    });

    animatedElements.forEach(element => {
        observer.observe(element);
    });
}

function populateFilters() {
    const eventTypes = new Set();
    const cities = new Set();

    eventCards.forEach(card => {
        const type = card.dataset.eventType;
        const city = card.dataset.eventCity;

        if (type) eventTypes.add(type);
        if (city) cities.add(city);
    });

    eventTypes.forEach(type => {
        const option = document.createElement('option');
        option.value = type;
        option.textContent = type;
        eventTypeFilter.appendChild(option);
    });

    cities.forEach(city => {
        const option = document.createElement('option');
        option.value = city;
        option.textContent = city;
        eventCityFilter.appendChild(option);
    });
}

function setupEventListeners() {
    eventSearch.addEventListener('input', filterEvents);
    eventTypeFilter.addEventListener('change', filterEvents);
    eventCityFilter.addEventListener('change', filterEvents);

    window.addEventListener('resize', () => {
        eventMaps.forEach(map => map.invalidateSize());
    });
}

function initEventMaps() {
    eventCards.forEach(card => {
        const eventId = card.dataset.eventId;
        const mapContainer = card.querySelector(`#map-${eventId}`);
        const coordsElement = card.querySelector('.event-coords');

        if (mapContainer && coordsElement) {
            const lat = parseFloat(coordsElement.dataset.lat);
            const lng = parseFloat(coordsElement.dataset.lng);
            const eventName = card.dataset.eventName;

            if (!isNaN(lat) && !isNaN(lng)) {
                const map = L.map(mapContainer).setView([lat, lng], 13);

                L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                    attribution: '© OpenStreetMap contributors',
                    maxZoom: 19
                }).addTo(map);

                const markerIcon = L.divIcon({
                    className: 'custom-marker',
                    html: '<i class="bi bi-geo-alt-fill"></i>',
                    iconSize: [36, 36],
                    iconAnchor: [18, 36],
                    popupAnchor: [0, -36]
                });

                const marker = L.marker([lat, lng], { icon: markerIcon }).addTo(map);

                const popupContent = `
                    <div class="custom-popup">
                        <div class="popup-header">
                            <h3>${eventName}</h3>
                        </div>
                        <div class="popup-body">
                            <p>Event Location</p>
                        </div>
                    </div>
                `;

                marker.bindPopup(popupContent);

                eventMaps.set(`map-${eventId}`, map);
            }
        }
    });
}

function filterEvents() {
    const searchText = eventSearch.value.toLowerCase();
    const selectedType = eventTypeFilter.value;
    const selectedCity = eventCityFilter.value;

    eventCards.forEach(card => {
        const eventName = card.dataset.eventName.toLowerCase();
        const eventType = card.dataset.eventType;
        const eventCity = card.dataset.eventCity;

        const matchesSearch = eventName.includes(searchText);
        const matchesType = selectedType === '' || eventType === selectedType;
        const matchesCity = selectedCity === '' || eventCity === selectedCity;

        if (matchesSearch && matchesType && matchesCity) {
            card.style.display = 'block';
        } else {
            card.style.display = 'none';
        }
    });
}

function deleteEvent(eventId) {
    const modal = document.createElement('div');
    modal.className = 'modal fade';
    modal.id = 'confirmDeleteModal';
    modal.setAttribute('tabindex', '-1');
    modal.setAttribute('aria-hidden', 'true');

    modal.innerHTML = `
        <div class="modal-dialog modal-confirm">
            <div class="modal-content">
                <div class="modal-header">
                    <div class="delete-icon">
                        <i class="bi bi-exclamation-triangle"></i>
                    </div>
                    <h4 class="modal-title w-100">Are you sure?</h4>
                </div>
                <div class="modal-body">
                    <p>Do you really want to delete this event? This process cannot be undone.</p>
                </div>
                <div class="modal-footer justify-content-center">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    <button type="button" class="btn btn-danger" id="confirmDelete">Delete</button>
                </div>
            </div>
        </div>
    `;

    document.body.appendChild(modal);

    const confirmModal = new bootstrap.Modal(document.getElementById('confirmDeleteModal'));
    confirmModal.show();

    document.getElementById('confirmDelete').addEventListener('click', function () {
        const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

        fetch(`/Event/DeleteConfirmed`, {
            method: "POST",
            headers: {
                "Content-Type": "application/x-www-form-urlencoded",
                "RequestVerificationToken": token
            },
            body: new URLSearchParams({ id: eventId })
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error("Failed to delete event.");
                }
                return response.text();
            })
            .then(() => {
                confirmModal.hide();

                confirmModal._element.addEventListener('hidden.bs.modal', function (event) {
                    document.getElementById('confirmDeleteModal').remove();
                });

                const eventCard = document.querySelector(`.event-card-wrapper[data-event-id="${eventId}"]`);
                if (eventCard) {
                    eventCard.style.transition = 'all 0.3s ease';
                    eventCard.style.transform = 'scale(0.9)';
                    eventCard.style.opacity = '0';

                    setTimeout(() => {
                        eventCard.remove();

                        if (document.querySelectorAll('.event-card-wrapper').length === 0) {
                            const emptyState = `
                            <div class="col-12">
                                <div class="empty-state animate-on-scroll fade-in show">
                                    <i class="bi bi-calendar-x empty-icon"></i>
                                    <p>No events available.</p>
                                </div>
                            </div>
                        `;
                            document.querySelector('.events-grid').innerHTML = emptyState;
                        }
                    }, 300);
                }
            })
            .catch(error => {
                alert(error.message);
                confirmModal.hide();
            });
    });

    document.getElementById('confirmDeleteModal').addEventListener('hidden.bs.modal', function (event) {
        setTimeout(() => {
            this.remove();
        }, 300);
    });
}
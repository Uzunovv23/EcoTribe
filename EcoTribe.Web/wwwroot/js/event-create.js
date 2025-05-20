let map, marker;

document.addEventListener('DOMContentLoaded', function () {
    initializeMap();
    initializeDateTimeValidation();
});

function initializeMap() {
    map = L.map('map').setView([42.6977, 23.3219], 13);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© OpenStreetMap contributors'
    }).addTo(map);

    map.on('click', function (e) {
        const lat = e.latlng.lat.toFixed(6);
        const lng = e.latlng.lng.toFixed(6);
        updateMarkerAndFields(lat, lng);
    });
}

function updateMarkerAndFields(lat, lng) {
    if (!marker) {
        marker = L.marker([lat, lng]).addTo(map);
    } else {
        marker.setLatLng([lat, lng]);
    }

    document.querySelector('[name="Latitude"]').value = lat;
    document.querySelector('[name="Longitude"]').value = lng;
    reverseGeocode(lat, lng);

    // Animate map zoom
    map.flyTo([lat, lng], 15, {
        duration: 1
    });
}

function geocodeAddress() {
    const address = document.getElementById("addressSearch").value;

    if (!address) {
        showAlert("Please enter an address.", "warning");
        return;
    }

    fetch(`https://nominatim.openstreetmap.org/search?format=json&q=${encodeURIComponent(address)}`)
        .then(response => response.json())
        .then(data => {
            if (data && data.length > 0) {
                const lat = parseFloat(data[0].lat).toFixed(6);
                const lon = parseFloat(data[0].lon).toFixed(6);
                updateMarkerAndFields(lat, lon);
            } else {
                showAlert("Location not found. Please try a more specific address.", "warning");
            }
        })
        .catch(err => {
            console.error("Geocoding error:", err);
            showAlert("Error searching address. Please try again.", "danger");
        });
}

function reverseGeocode(lat, lon) {
    fetch(`/api/geolocation/reverse?lat=${lat}&lon=${lon}`)
        .then(response => {
            if (!response.ok) throw new Error("Backend reverse geocoding failed");
            return response.json();
        })
        .then(data => {
            if (data && data.length > 0 && data[0].name) {
                document.getElementById("CityInput").value = data[0].name;
            }
        })
        .catch(err => {
            console.error("Reverse geocoding failed:", err);
            showAlert("Error getting location details.", "warning");
        });
}

function initializeDateTimeValidation() {
    const startInput = document.querySelector('[name="Start"]');
    const endInput = document.querySelector('[name="End"]');

    const today = new Date();
    const todayStr = today.toISOString().split('.')[0].slice(0, -3);
    startInput.min = todayStr;
    endInput.min = todayStr;

    startInput.addEventListener('change', function () {
        endInput.min = this.value;
        if (endInput.value && endInput.value < this.value) {
            endInput.value = this.value;
        }
    });
}

function showAlert(message, type = 'info') {
    const alertDiv = document.createElement('div');
    alertDiv.className = `alert alert-${type} alert-dismissible fade show position-fixed top-0 start-50 translate-middle-x mt-3`;
    alertDiv.style.zIndex = '1050';
    alertDiv.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    `;
    document.body.appendChild(alertDiv);

    setTimeout(() => {
        alertDiv.remove();
    }, 5000);
}
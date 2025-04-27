
 @param {number} latitude 
 @param {number} longitude 
 @param {string} eventName 
 
function initMap(latitude, longitude, eventName) {
    const map = L.map('map').setView([latitude, longitude], 14);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© OpenStreetMap contributors',
        maxZoom: 19
    }).addTo(map);

    const customIcon = L.divIcon({
        className: 'custom-map-marker',
        html: '<i class="fas fa-map-marker-alt" style="font-size: 36px; color: #3E885B;"></i>',
        iconSize: [36, 36],
        iconAnchor: [18, 36],
        popupAnchor: [0, -36]
    });

    const marker = L.marker([latitude, longitude], { icon: customIcon }).addTo(map);

    const popupContent = `
        <div style="text-align: center; padding: 8px;">
            <h4 style="margin: 0 0 8px 0; color: #3E885B; font-weight: 600;">${eventName}</h4>
            <p style="margin: 0; font-size: 14px;">Event Location</p>
        </div>
    `;

    marker.bindPopup(popupContent).openPopup();

    setTimeout(() => {
        map.flyTo([latitude, longitude], 15, {
            duration: 1.5,
            easeLinearity: 0.25
        });
    }, 1000);

    window.addEventListener('resize', () => {
        map.invalidateSize();
    });
}

function initAnimations() {
    const animatedElements = document.querySelectorAll('.animate-on-scroll');

    const isInViewport = (element) => {
        const rect = element.getBoundingClientRect();
        const windowHeight = window.innerHeight || document.documentElement.clientHeight;
        return (
            rect.top <= windowHeight * 0.85 &&
            rect.bottom >= 0
        );
    };

    const handleScroll = () => {
        animatedElements.forEach(element => {
            if (isInViewport(element) && !element.classList.contains('is-visible')) {
                element.classList.add('is-visible');
            }
        });
    };

    setTimeout(() => {
        handleScroll();
    }, 300);

    window.addEventListener('scroll', handleScroll);

    handleScroll();
}

function initStarRating() {
    const starLabels = document.querySelectorAll('.star-rating label');

    starLabels.forEach(label => {
        label.addEventListener('mouseenter', () => {
            let current = label;
            while (current !== null) {
                current.classList.add('hover');
                current = current.nextElementSibling;
                if (current && !current.classList.contains('star')) break;
            }
        });

        label.addEventListener('mouseleave', () => {
            document.querySelectorAll('.star-rating label').forEach(l => {
                l.classList.remove('hover');
            });
        });
    });
}

function addStyleToHead() {
    const link = document.createElement('link');
    link.rel = 'stylesheet';
    link.href = '/css/event-details.css';
    document.head.appendChild(link);

    if (!document.querySelector('link[href*="font-awesome"]')) {
        const fontAwesome = document.createElement('link');
        fontAwesome.rel = 'stylesheet';
        fontAwesome.href = 'https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css';
        document.head.appendChild(fontAwesome);
    }
}

document.addEventListener('DOMContentLoaded', () => {
    addStyleToHead();
});
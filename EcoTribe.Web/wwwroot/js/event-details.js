/**
 * EcoTribe Event Details JavaScript
 * Handles map initialization, animations, and interactive elements
 */

/**
 * Initialize the Leaflet map
 * @param {number} latitude - Event location latitude
 * @param {number} longitude - Event location longitude
 * @param {string} eventName - Name of the event for the marker popup
 */
function initMap(latitude, longitude, eventName) {
    // Create a custom map style with green-themed colors
    const map = L.map('map').setView([latitude, longitude], 14);

    // Use a styled map tile layer
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© OpenStreetMap contributors',
        maxZoom: 19
    }).addTo(map);

    // Create a custom icon for the marker
    const customIcon = L.divIcon({
        className: 'custom-map-marker',
        html: '<i class="fas fa-map-marker-alt" style="font-size: 36px; color: #3E885B;"></i>',
        iconSize: [36, 36],
        iconAnchor: [18, 36],
        popupAnchor: [0, -36]
    });

    // Add marker with custom icon and popup
    const marker = L.marker([latitude, longitude], { icon: customIcon }).addTo(map);

    // Create a styled popup
    const popupContent = `
        <div style="text-align: center; padding: 8px;">
            <h4 style="margin: 0 0 8px 0; color: #3E885B; font-weight: 600;">${eventName}</h4>
            <p style="margin: 0; font-size: 14px;">Event Location</p>
        </div>
    `;

    marker.bindPopup(popupContent).openPopup();

    // Add a subtle zoom animation
    setTimeout(() => {
        map.flyTo([latitude, longitude], 15, {
            duration: 1.5,
            easeLinearity: 0.25
        });
    }, 1000);

    // Make map responsive
    window.addEventListener('resize', () => {
        map.invalidateSize();
    });
}

/**
 * Initialize scroll-based animations
 */
function initAnimations() {
    // Get all elements that should be animated on scroll
    const animatedElements = document.querySelectorAll('.animate-on-scroll');

    // Function to check if element is in viewport
    const isInViewport = (element) => {
        const rect = element.getBoundingClientRect();
        const windowHeight = window.innerHeight || document.documentElement.clientHeight;
        return (
            rect.top <= windowHeight * 0.85 &&
            rect.bottom >= 0
        );
    };

    // Function to handle scroll events
    const handleScroll = () => {
        animatedElements.forEach(element => {
            if (isInViewport(element) && !element.classList.contains('is-visible')) {
                element.classList.add('is-visible');
            }
        });
    };

    // Add initial delay to first animations
    setTimeout(() => {
        handleScroll();
    }, 300);

    // Add scroll event listener
    window.addEventListener('scroll', handleScroll);

    // Trigger once on load to show initial elements
    handleScroll();
}

/**
 * Initialize star rating functionality
 */
function initStarRating() {
    const starLabels = document.querySelectorAll('.star-rating label');

    starLabels.forEach(label => {
        label.addEventListener('mouseenter', () => {
            // Add hover class to current and previous stars
            let current = label;
            while (current !== null) {
                current.classList.add('hover');
                current = current.nextElementSibling;
                if (current && !current.classList.contains('star')) break;
            }
        });

        label.addEventListener('mouseleave', () => {
            // Remove hover class from all stars
            document.querySelectorAll('.star-rating label').forEach(l => {
                l.classList.remove('hover');
            });
        });
    });
}

/**
 * Add to head to ensure CSS is applied
 */
function addStyleToHead() {
    // Create link element for custom styles
    const link = document.createElement('link');
    link.rel = 'stylesheet';
    link.href = '/css/event-details.css';
    document.head.appendChild(link);

    // Add Font Awesome if not already present
    if (!document.querySelector('link[href*="font-awesome"]')) {
        const fontAwesome = document.createElement('link');
        fontAwesome.rel = 'stylesheet';
        fontAwesome.href = 'https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css';
        document.head.appendChild(fontAwesome);
    }
}

// Run style addition on document load
document.addEventListener('DOMContentLoaded', () => {
    addStyleToHead();
});
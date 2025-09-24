/**
 *Initialize the Leaflet map
 @param {number} latitude 
 @param {number} longitude 
 @param {string} eventName 
 */

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

let currentPhotoIndex = 0;
let photoUrls = [];

function initPhotoGallery() {
    const galleryImages = document.querySelectorAll('.gallery-image');
    photoUrls = Array.from(galleryImages).map(img => img.src);


    document.addEventListener('keydown', (e) => {
        if (document.getElementById('lightboxModal').style.display === 'block') {
            switch (e.key) {
                case 'Escape':
                    closeLightbox();
                    break;
                case 'ArrowLeft':
                    prevPhoto();
                    break;
                case 'ArrowRight':
                    nextPhoto();
                    break;
            }
        }
    });

    document.getElementById('lightboxModal').addEventListener('click', (e) => {
        if (e.target.id === 'lightboxModal') {
            closeLightbox();
        }
    });
}

function openLightbox(index) {
    if (photoUrls.length === 0) return;

    currentPhotoIndex = index;
    const modal = document.getElementById('lightboxModal');
    const img = document.getElementById('lightboxImage');
    const counter = document.getElementById('lightboxCounter');

    img.src = photoUrls[currentPhotoIndex];
    counter.textContent = `${currentPhotoIndex + 1} / ${photoUrls.length}`;

    modal.style.display = 'block';
    document.body.style.overflow = 'hidden';

    preloadImages();
}

function closeLightbox() {
    const modal = document.getElementById('lightboxModal');
    modal.style.display = 'none';
    document.body.style.overflow = 'auto';
}

function nextPhoto() {
    currentPhotoIndex = (currentPhotoIndex + 1) % photoUrls.length;
    updateLightboxImage();
}

function prevPhoto() {
    currentPhotoIndex = (currentPhotoIndex - 1 + photoUrls.length) % photoUrls.length;
    updateLightboxImage();
}

function updateLightboxImage() {
    const img = document.getElementById('lightboxImage');
    const counter = document.getElementById('lightboxCounter');

    img.src = photoUrls[currentPhotoIndex];
    counter.textContent = `${currentPhotoIndex + 1} / ${photoUrls.length}`;

    preloadImages();
}

function preloadImages() {
    if (currentPhotoIndex < photoUrls.length - 1) {
        const nextImg = new Image();
        nextImg.src = photoUrls[currentPhotoIndex + 1];
    }

    if (currentPhotoIndex > 0) {
        const prevImg = new Image();
        prevImg.src = photoUrls[currentPhotoIndex - 1];
    }
}


document.addEventListener('DOMContentLoaded', () => {
    const viewGalleryLinks = document.querySelectorAll('a[href="#photoGallery"]');
    viewGalleryLinks.forEach(link => {
        link.addEventListener('click', (e) => {
            e.preventDefault();
            const gallerySection = document.getElementById('photoGallery');
            if (gallerySection) {
                gallerySection.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    });
});

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
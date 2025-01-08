var VendorLocation = {
    map: null,
    markers: [],
    bounds: null,

    init: function () {
        // Initialize map settings
        this.initMapSettings();
        
        // Initialize event handlers
        this.initEventHandlers();
    },

    initMapSettings: function () {
        // Default map settings (center on US by default)
        this.defaultCenter = { lat: 37.0902, lng: -95.7129 };
        this.defaultZoom = 4;
    },

    initEventHandlers: function () {
        // Handle vendor item hover
        $('.vendor-item').hover(
            function () {
                var lat = $(this).data('lat');
                var lng = $(this).data('lng');
                VendorLocation.highlightMarker(lat, lng);
            },
            function () {
                VendorLocation.resetMarkers();
            }
        );
    },

    initVendorMap: function (elementId, vendors) {
        // Load Google Maps if not already loaded
        if (typeof google === 'undefined' || typeof google.maps === 'undefined') {
            var script = document.createElement('script');
            script.src = 'https://maps.googleapis.com/maps/api/js?key=' + window.googleMapsApiKey;
            script.onload = function () {
                VendorLocation.createMap(elementId, vendors);
            };
            document.head.appendChild(script);
        } else {
            this.createMap(elementId, vendors);
        }
    },

    createMap: function (elementId, vendors) {
        // Initialize map
        this.bounds = new google.maps.LatLngBounds();
        
        this.map = new google.maps.Map(document.getElementById(elementId), {
            zoom: this.defaultZoom,
            center: this.defaultCenter,
            mapTypeControl: false,
            streetViewControl: false,
            fullscreenControl: false,
            styles: [
                {
                    featureType: 'poi',
                    elementType: 'labels',
                    stylers: [{ visibility: 'off' }]
                }
            ]
        });

        // Add markers for each vendor
        vendors.forEach(function (vendor) {
            var position = new google.maps.LatLng(vendor.lat, vendor.lng);
            var marker = new google.maps.Marker({
                position: position,
                map: VendorLocation.map,
                title: vendor.name,
                animation: google.maps.Animation.DROP
            });

            // Create info window
            var infoWindow = new google.maps.InfoWindow({
                content: VendorLocation.createInfoWindowContent(vendor)
            });

            // Add click listener
            marker.addListener('click', function () {
                VendorLocation.closeAllInfoWindows();
                infoWindow.open(VendorLocation.map, marker);
            });

            // Store marker and info window
            VendorLocation.markers.push({
                marker: marker,
                infoWindow: infoWindow,
                lat: vendor.lat,
                lng: vendor.lng
            });

            // Extend bounds
            VendorLocation.bounds.extend(position);
        });

        // Fit map to bounds if multiple vendors
        if (vendors.length > 1) {
            this.map.fitBounds(this.bounds);
        } else if (vendors.length === 1) {
            this.map.setCenter(new google.maps.LatLng(vendors[0].lat, vendors[0].lng));
            this.map.setZoom(15);
        }
    },

    createInfoWindowContent: function (vendor) {
        return '<div class="vendor-info-window">' +
               '<h5>' + vendor.name + '</h5>' +
               '<p class="distance">' + vendor.distance + '</p>' +
               '<p class="address">' + vendor.address + '</p>' +
               '</div>';
    },

    highlightMarker: function (lat, lng) {
        this.markers.forEach(function (item) {
            if (item.lat === lat && item.lng === lng) {
                item.marker.setAnimation(google.maps.Animation.BOUNCE);
                item.infoWindow.open(VendorLocation.map, item.marker);
            }
        });
    },

    resetMarkers: function () {
        this.markers.forEach(function (item) {
            item.marker.setAnimation(null);
            item.infoWindow.close();
        });
    },

    closeAllInfoWindows: function () {
        this.markers.forEach(function (item) {
            item.infoWindow.close();
        });
    }
};

// Initialize on document ready
$(document).ready(function () {
    VendorLocation.init();
});

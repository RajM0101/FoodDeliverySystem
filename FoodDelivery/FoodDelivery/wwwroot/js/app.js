// Include images
let img_src = [
    'images/iconfly.png'
]; 
// Name images included
let image_type = img_src.map(function (cuurentEl, index) { return "image" + index });

var dvParticlesJs = document.getElementById('particles-js');
if (dvParticlesJs != null) {
    // Configure particles-js
    particlesJS('particles-js',
        {
            "particles": {
                "number": {
                    "value": 8, // No of images
                    "density": {
                        "enable": true,
                        "value_area": 500 // Specify area (Lesser is greater density)
                    }
                },
                "shape": {
                    "type": image_type, // Add images to particle-js
                    "stroke": {
                        "width": 0,
                    },
                    "polygon": {
                        "nb_sides": 4
                    }
                },
                "size": {
                    "value": 50, // Adjust the image size
                    "random": false
                },
                "move": {
                    "enable": true,
                    "speed": 5,   // Speed of particle motion
                    "direction": "none",
                    "random": false,
                    "straight": false,
                    "out_mode": "out",
                    "bounce": false,
                    "attract": {
                        "enable": false,
                        "rotateX": 600,
                        "rotateY": 1200
                    }
                }
            },
            "retina_detect": true
        }
    ); 

}
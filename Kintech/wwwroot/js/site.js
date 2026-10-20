let currentSlide = 0;
let slides = [];
let dots = [];
let slideInterval;

document.addEventListener("DOMContentLoaded", function () {
    slides = document.querySelectorAll(".hero-slide");
    dots = document.querySelectorAll(".hero-dot");

    if (slides.length > 0) {
        startSlider();

        const stage = document.getElementById("hero3DStage");
        const tiltCard = document.getElementById("hero3DTiltCard");

        if (stage) {
            stage.addEventListener("mouseenter", stopSlider);
            stage.addEventListener("mouseleave", function () {
                startSlider();
                if (tiltCard) {
                    tiltCard.style.transform = "rotateY(-8deg) rotateX(4deg)";
                }
            });

            // Interactive 3D Mouse Perspective Tilt
            stage.addEventListener("mousemove", function (e) {
                if (!tiltCard || window.innerWidth < 992) return;
                const rect = stage.getBoundingClientRect();
                const x = e.clientX - rect.left;
                const y = e.clientY - rect.top;
                const centerX = rect.width / 2;
                const centerY = rect.height / 2;

                const rotateY = ((x - centerX) / centerX) * 10;
                const rotateX = ((centerY - y) / centerY) * 8;

                tiltCard.style.transform = `rotateY(${rotateY.toFixed(2)}deg) rotateX(${rotateX.toFixed(2)}deg)`;
            });
        }
    }
});

function startSlider() {
    stopSlider();
    slideInterval = setInterval(nextSlide, 3800);
}
document.addEventListener("visibilitychange", function () {
    document.hidden ? stopSlider() : startSlider();
});

function stopSlider() {
    if (slideInterval) {
        clearInterval(slideInterval);
    }
}

function showSlide(index) {
    if (!slides.length) return;
    slides.forEach(s => s.classList.remove("active"));
    dots.forEach(d => d.classList.remove("active"));

    slides[index].classList.add("active");
    if (dots[index]) {
        dots[index].classList.add("active");
    }

    currentSlide = index;
}

function nextSlide() {
    if (!slides.length) return;
    let next = (currentSlide + 1) % slides.length;
    showSlide(next);
}

function prevSlide() {
    if (!slides.length) return;
    let prev = (currentSlide - 1 + slides.length) % slides.length;
    showSlide(prev);
    startSlider();
}

function goToSlide(index) {
    showSlide(index);
    startSlider();
}
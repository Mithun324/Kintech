
document.addEventListener('DOMContentLoaded', function () {
    const track = document.getElementById('heroSliderTrack');
    const dotsContainer = document.getElementById('sliderDots');
    let slides = track.querySelectorAll('.slide');
    let currentIndex = 0;
    let intervalId = null;
    const totalSlides = slides.length;

  
    slides.forEach((_, i) => {
        const dot = document.createElement('span');
        if (i === 0) dot.classList.add('active');
        dot.dataset.index = i;
        dot.addEventListener('click', () => goToSlide(i));
        dotsContainer.appendChild(dot);
    });

    const dots = dotsContainer.querySelectorAll('span');

    function goToSlide(index) {
        if (index < 0) index = totalSlides - 1;
        if (index >= totalSlides) index = 0;
        currentIndex = index;
        track.style.transform = `translateX(-${currentIndex * 100}%)`;
        dots.forEach((dot, i) => {
            dot.classList.toggle('active', i === currentIndex);
        });
    }

    function nextSlide() {
        goToSlide(currentIndex + 1);
    }

    function startAutoPlay() {
        if (intervalId) clearInterval(intervalId);
        intervalId = setInterval(nextSlide, 2000);
    }

    function stopAutoPlay() {
        if (intervalId) {
            clearInterval(intervalId);
            intervalId = null;
        }
    }

    const slider = document.querySelector('.hero-slider');
    slider.addEventListener('mouseenter', stopAutoPlay);
    slider.addEventListener('mouseleave', startAutoPlay);

    startAutoPlay();

    dots.forEach(dot => {
        dot.addEventListener('click', () => {
            stopAutoPlay();
            goToSlide(parseInt(dot.dataset.index));
            startAutoPlay();
        });
    });
});


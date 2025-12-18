document.addEventListener("DOMContentLoaded", () => {
    const slides = document.querySelector(".slides");
    const images = document.querySelectorAll(".slides img");
    const next = document.querySelector(".next");
    const prev = document.querySelector(".prev");

    let index = 0;

    if (!slides || images.length === 0) return;

    function showSlide() {
        slides.style.transform = `translateX(-${index * 100}%)`;
    }

    next.addEventListener("click", () => {
        index = (index + 1) % images.length;
        showSlide();
    });

    prev.addEventListener("click", () => {
        index = (index - 1 + images.length) % images.length;
        showSlide();
    });
});
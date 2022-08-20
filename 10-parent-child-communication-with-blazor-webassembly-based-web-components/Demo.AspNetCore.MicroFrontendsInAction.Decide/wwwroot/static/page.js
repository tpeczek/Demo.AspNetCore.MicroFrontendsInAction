(function() {
  const element = document.querySelector(".decide_recos");
  const url = element.getAttribute("data-fragment");
  const platinum = document.querySelector(".decide_editions input");
  const image = document.querySelector(".decide_image");
  const buyButton = document.querySelector("checkout-buy");

  window
    .fetch(url)
    .then(res => res.text())
    .then(html => {
      element.innerHTML = html;
    });

  platinum.addEventListener("change", e => {
    const edition = e.target.checked ? "platinum" : "standard";
    buyButton.setAttribute("edition", edition);
    image.src = image.src.replace(/(standard|platinum)/, edition);
  });
})();
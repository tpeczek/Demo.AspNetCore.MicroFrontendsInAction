(function() {
  const inspireElement = document.querySelector(".decide_recos");
  const inspireElementUrl = inspireElement.getAttribute("data-fragment");

  const editionsInput = document.querySelector(".decide_editions input");
  const editionsImage = document.querySelector(".decide_image");

  const buyButton = document.querySelector("checkout-buy");

  window
    .fetch(inspireElementUrl)
    .then(res => res.text())
    .then(html => {
      inspireElement.innerHTML = html;
    });

  editionsInput.addEventListener("change", e => {
    const edition = e.target.checked ? "platinum" : "standard";
    buyButton.setAttribute("edition", edition);
    editionsImage.src = editionsImage.src.replace(/(standard|platinum)/, edition);
  });
})();
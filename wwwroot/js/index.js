document.querySelectorAll("button[aria-busy]").forEach((el) => {
	console.log("element found")
	el.addEventListener("click", (event) => { event.target.setAttribute("aria-busy", "true") });
});
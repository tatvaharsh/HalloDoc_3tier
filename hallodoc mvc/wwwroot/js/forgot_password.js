const togglePassword = document.querySelector("#togglePassword");
const password = document.querySelector("#floatingPassword");

togglePassword.addEventListener("click", function () {

    const type = password.getAttribute("type") === "password" ? "text" : "password";
    password.setAttribute("type", type);

    this.classList.toggle('bi-eye');
});

const togglePassword2 = document.querySelector("#togglePassword2");
const password2 = document.querySelector("#floatingPassword2");

togglePassword2.addEventListener("click", function () {

    const type = password2.getAttribute("type") === "password" ? "text" : "password";
    password2.setAttribute("type", type);

    this.classList.toggle('bi-eye');
});


var element = document.body;
if (localStorage.getItem("theme") == null) {
    localStorage.setItem("theme", "light");
}
if (localStorage.getItem("theme") == "dark") {

    element.classList.add("dark-mode");

}
function toggle() {
    if (localStorage.getItem("theme") == "light") {

        element.classList.add("dark-mode");
        localStorage.setItem("theme", "dark");


    }
    else {


        element.classList.remove("dark-mode");
        localStorage.setItem("theme", "light");

    }

}

function myfun1() {
    window.location.href = "./patient_submit.html";
}
const getFileData = (myFile) => {
    var file = myFile.files[0];
    var filename = file.name;
    document.getElementById("form-label").innerHTML = `${filename}`;
}
const load = () => {
    const phoneInputField = document.querySelector("#phone");
    const phoneInput = window.intlTelInput(phoneInputField, {
        utilsScript:
            "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
    });
}
window.addEventListener('DOMContentLoaded', () => {
    const popup = new bootstrap.Modal('#staticBackdrop')

    popup.show();
    console.log("hello");
});
;







window.addEventListener('DOMContentLoaded', () => {
    popup.show();
    console.log("hello");
});










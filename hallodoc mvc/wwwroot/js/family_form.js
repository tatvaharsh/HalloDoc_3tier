function myFunction() {
    var element = document.body;
    element.classList.toggle("dark-mode");
}

const getFileData = (myFile) => {
    var file = myFile.files[0];
    var filename = file.name;
    document.getElementById("form-label").innerHTML = `${filename}`;
}

const load = () => {
    const phoneInputField = document.getElementsByClassName("phone");
    for (let i = 0; i < phoneInputField.length; ++i) {
        const phoneInput = window.intlTelInput(phoneInputField[i], {
            utilsScript:
                "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
        });
    }
}
window.addEventListener('DOMContentLoaded', () => {
    const popup = new bootstrap.Modal('#staticBackdrop')

    popup.show();
    console.log("hello");
});
;
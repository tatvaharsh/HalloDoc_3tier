document.addEventListener("DOMContentLoaded", () => {    const phoneInputField = document.querySelector("#phone");    const phoneInput = window.intlTelInput(phoneInputField, {        utilsScript:            "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",    });
});


window.addEventListener('DOMContentLoaded', () => {
    const popup = new bootstrap.Modal('#myModalAssign')

    popup.show();
    console.log("hello");
});
;


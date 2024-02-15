//function myMap() {
//    var mapProp = {
//        center: new google.maps.LatLng(51.508742, -0.120850),
//        zoom: 5,
//    };
//    var map = new google.maps.Map(document.getElementById("googleMap"), mapProp);
//}
$(document).ready(function () {
    $("#openBtn").click(function () {
        $("#myModal1").modal("show");
        var Street = $("#street").val();
        var City = $("#city").val();
        var State = $("#state").val();
        var ZipCode = $("#zipcode").val();
        var address = "https://maps.google.com/maps?q=" + Street + City + State + ZipCode + "&t=&z=13&ie=UTF8&iwloc=&output=embed";
        $("#gmap_canvas").attr("src", address);
    });
});


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



$(document).ready(function () {
    var logElements = document.querySelectorAll('.log');
    logElements.forEach(function (element) {
        element.addEventListener('click', function () {

            $.get('/Home/patient_login', function (response) {
                console.log(response)
            });
        });
    });
});

function sidebar_open() {
    if (document.getElementById("mySidebar").offsetWidth == 0) {
        document.getElementById("mySidebar").style.width = "200px";
        document.getElementsByClassName("overlay")[0].style.display = "block";
        document.getElementsByClassName("navbar")[0].style.boxShadow = "none";
    }
    else {
        document.getElementById("mySidebar").style.width = "0px";
        document.getElementsByClassName("overlay")[0].style.display = "none";
        document.getElementsByClassName("navbar")[0].style.boxShadow = "3px -8px 17px 1px black";
    }

}

function sidebar_close() {
    document.getElementById("mySidebar").style.width = "0px";
    document.getElementsByClassName("overlay")[0].style.display = "none";
    for (let i = 0; i < document.getElementsByClassName('accordion-button').length; ++i) {
        document.getElementsByClassName('accordion-button')[i].style.zIndex = "1";
    }
}
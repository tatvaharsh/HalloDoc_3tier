

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


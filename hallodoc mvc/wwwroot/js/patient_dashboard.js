
if (localStorage.getItem("theme") == null) {
    localStorage.setItem("theme", "light");
}
if (localStorage.getItem("theme") == "dark") {
    document.documentElement.setAttribute('data-bs-theme', 'dark')
}
function toggle() {
    if (localStorage.getItem("theme") == "light") {
        document.documentElement.setAttribute('data-bs-theme', 'dark')
        localStorage.setItem("theme", "dark");


    }
    else {
        document.documentElement.setAttribute('data-bs-theme', 'light');
        localStorage.setItem("theme", "light");

    }

}
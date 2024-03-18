
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
       /* document.getElementById('adminnav').classList.toggle('bg-secondary');*/
        //document.getElementById('adminnav').classList.toggle('bg-white');

    }
    else {
        document.documentElement.setAttribute('data-bs-theme', 'light');
        localStorage.setItem("theme", "light");
       /* document.getElementById('adminnav').classList.toggle('bg-secondary');*/
        //document.getElementById('adminnav').classList.toggle('bg-white');

    }

}
//document.getElementById('btnSwitch').addEventListener('click', () => {
//    if (document.documentElement.getAttribute('data-bs-theme') == 'light') {

//    }
//    else {
//    }
//})


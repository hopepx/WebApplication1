const loginDropdown = document.querySelector('#login-dropdown');
const dropdownMenu = document.querySelector('.dropdown-menu');
const memberLoginBtn = document.querySelector('#member-login');
const staffLoginBtn = document.querySelector('#staff-login');

loginDropdown.addEventListener('click', function () {
    dropdownMenu.classList.toggle('show');
});

memberLoginBtn.addEventListener('click', function () {
    // Handle member login
});

staffLoginBtn.addEventListener('click', function () {
    // Handle staff login
});

// Close dropdown when clicking outside
document.addEventListener('click', function (event) {
    const target = event.target;
    if (!loginDropdown.contains(target)) {
        dropdownMenu.classList.remove('show');
    }
});



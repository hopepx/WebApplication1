function validateForm(event) {
    event.preventDefault();

    const passwordInput = document.getElementById('passwordInput');

    const password = passwordInput.value;

    if (password.length < 6) {
        alert("Password should be at least 6 characters long.");
        return;
    }

    // If the form passes all validations, submit the form
    event.target.submit();
}

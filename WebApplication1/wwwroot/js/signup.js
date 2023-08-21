function validateForm(event) {
  event.preventDefault();

  const passwordInput = document.getElementById('passwordInput');
  const confirmPasswordInput = document.getElementById('confirmPasswordInput');

  const password = passwordInput.value;
  const confirmPassword = confirmPasswordInput.value;

  if (password !== confirmPassword) {
    alert("Passwords do not match. Please retype your password.");
    return;
  }

  if (password.length < 6) {
    alert("Password should be at least 6 characters long.");
    return;
  }

  // If the form passes all validations, submit the form
  event.target.submit();
}

// Update ToCountry based on selected ToCity
var toCitySelect = document.getElementById('toCitySelect');
var toCountryInput = document.getElementById('toCountryInput');

toCitySelect.addEventListener('change', function () {
    var selectedCity = this.value;
    var cityAndCountryList = @Html.Raw(Json.Serialize(ViewBag.CityAndCountryList));

    for (var i = 0; i < cityAndCountryList.length; i++) {
        var cityAndCountry = cityAndCountryList[i];
        var city = cityAndCountry.split(',')[0].trim();
        var country = cityAndCountry.split(',')[1].trim();

        if (city === selectedCity) {
            toCountryInput.value = country;
            break;
        }
    }
});
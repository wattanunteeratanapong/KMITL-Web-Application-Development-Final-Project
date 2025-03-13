// Function to change the padlet colors when validation fails
function changePadletColorOnError() {
    // Change the padlet colors to red
    document.documentElement.style.setProperty('--padlet-1', '#ff003d');
    document.documentElement.style.setProperty('--padlet-2', '#ff99b1');
    document.documentElement.style.setProperty('--padlet-3', '#7c011e');
    document.documentElement.style.setProperty('--padlet-4', '#7c011e');
    document.documentElement.style.setProperty('--padlet-5', '#7c011e');

    // Reset the padlet colors after 0.3 seconds
    setTimeout(function () {
        document.documentElement.style.setProperty('--padlet-1', '#F5ECD5');
        document.documentElement.style.setProperty('--padlet-2', '#FFFAEC');
        document.documentElement.style.setProperty('--padlet-3', '#578E7E');
        document.documentElement.style.setProperty('--padlet-4', '#3D3D3D');
        document.documentElement.style.setProperty('--padlet-5', '#9AB9AA');
    }, 300);
}

// Attach validation check on form submit
document.getElementById('formbox').addEventListener('submit', function (event) {
    // Initialize flag for validation
    var isValid = true;

    // Email validation using regex
    var email = document.getElementById('emailinput').value;
    var emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    if (!emailRegex.test(email)) {
        isValid = false;
        alert('Invalid Gmail address!');
    }

    // First name and Last name validation (alphabet only)
    var firstName = document.getElementById('firstnameinput').value;
    var lastName = document.getElementById('lastnameinput').value;
    var nameRegex = /^[a-zA-Z]+$/;
    if (!nameRegex.test(firstName) || !nameRegex.test(lastName)) {
        isValid = false;
        alert('First and Last name should contain alphabet characters only!');
    }

    // Password and Confirm Password matching
    var password = document.getElementById('passwordinput').value;
    var confirmPassword = document.getElementById('confirmpasswordinput').value;
    if (password !== confirmPassword) {
        isValid = false;
        alert('Passwords do not match!');
    }

    // If any validation failed, prevent form submission and show padlet color change
    if (!isValid) {
        event.preventDefault(); // Prevent form submission
        changePadletColorOnError(); // Trigger the padlet color change effect
    }
});


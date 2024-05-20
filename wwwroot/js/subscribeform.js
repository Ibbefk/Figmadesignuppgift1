document.getElementById('subscribe-form').addEventListener('submit', function (event) {
    event.preventDefault();

    const email = document.getElementById('email').value;
    const subscribeMessage = document.getElementById('subscribe-message');
    const termsAgree = document.getElementById('terms-agree').checked;

    if (!termsAgree) {
        subscribeMessage.textContent = 'Please agree to the terms and privacy policy.';
        subscribeMessage.style.color = 'red';
        return;
    }

    if (validateEmail(email)) {
        subscribeMessage.textContent = 'Thank you for subscribing!';
        subscribeMessage.style.color = 'green';
    } else {
        subscribeMessage.textContent = 'Please enter a valid email address.';
        subscribeMessage.style.color = 'red';
    }
});

function validateEmail(email) {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
}

document.addEventListener("DOMContentLoaded", function () {
    const forms = document.querySelectorAll("form");
    forms.forEach(form => {
        form.addEventListener("submit", function (event) {
            const inputs = form.querySelectorAll("input, textarea");
            let isValid = true;
            inputs.forEach(input => {
                if (!input.checkValidity()) {
                    isValid = false;
                    input.classList.add("is-invalid");
                    const errorSpan = form.querySelector(`span[data-valmsg-for='${input.name}']`);
                    if (errorSpan) {
                        errorSpan.textContent = input.validationMessage;
                    }
                } else {
                    input.classList.remove("is-invalid");
                }
            });
            if (!isValid) {
                event.preventDefault();
            }
        });
    });
});

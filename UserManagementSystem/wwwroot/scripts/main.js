function showValidationErrors(form) {
    var formValidator = form.validate();

    if (!form.valid()) {
        var validationErrorsAreaId = "form-validation-errors-area";
        var validationErrorsArea = $("#" + validationErrorsAreaId);
        var validationErrors = "";

        for (var i = 0; i < formValidator.errorList.length; i++) {
            validationErrors += formValidator.errorList[i].message;
            if (i < formValidator.errorList.length - 1)
                validationErrors += "</br>";
        }
        console.log(validationErrors);
        if (validationErrorsArea.length == 0) {
            form.prepend("<div class=\"alert alert-danger alert-dismissible fade show\" id=\"" + validationErrorsAreaId + "\"></div>");
            validationErrorsArea = $("#" + validationErrorsAreaId);
        }

        validationErrorsArea.empty();
        validationErrorsArea.append(validationErrors);
        validationErrorsArea.append("<button type=\"button\" class=\"btn-close\" data-bs-dismiss=\"alert\" aria-label=\"Close\"></button>");
    }
}

$(document).ready(function () {
    $("#select-or-diselect-all-users-checkbox").click(function() {
        var userTableCheckboxes = $("#users-table > tbody > tr > td > input:checkbox");
        var isCheckboxChecked = $("#select-or-diselect-all-users-checkbox").prop("checked");

        $(userTableCheckboxes).each(function() {
            $(this).prop("checked", isCheckboxChecked);
        });
    });

    $("#login-button").click(function() {
        var loginForm = $("#login-form");

        showValidationErrors(loginForm);
    });

    $("#register-button").click(function () {
        var registrationForm = $("#registration-form");

        showValidationErrors(registrationForm);
    });
});

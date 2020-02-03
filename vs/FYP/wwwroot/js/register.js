var registering = false;
function register() {
    if (!registering) {
        registering = true;
        $('#registerButton').html('<i class="fas fa-spinner fa-spin"></i>');
        $('#registerButton').attr('class', 'btn btn-primary disabled');
        $.ajax({
            url: '/Api/User/Create',
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            data: JSON.stringify({ 'Email': $('#Email').val(), 'Password': $('#Password').val(), 'ConfirmPassword': $('#CPassword').val(), 'FName': $('#FName').val(), 'LName': $('#LName').val() }),
            success: function (responds) {
                if (responds.status == "OK") {
                    $('#errorFrame').attr('style', 'display:none;')
                    $('#registerButton').html('Registered');
                    $('#registerButton').attr('class', 'btn btn-success disabled');
                    setTimeout(function () { window.location.replace("/Login"); }, 2000);
                } else if (responds.status == "EMAIL_IS_NULL") {
                    $('#registerButton').html('Login Now');
                    $('#errorFrame').removeAttr('style');
                    $('#registerButton').attr('class', 'btn btn-primary');
                    $('#errorMsg').html('Error: Please enter your email.')
                    registering = false;
                } else if (responds.status == "PASSWORD_IS_NULL") {
                    $('#registerButton').html('Login Now');
                    $('#errorFrame').removeAttr('style');
                    $('#registerButton').attr('class', 'btn btn-primary');
                    $('#errorMsg').html('Error: Please enter your password.')
                    registering = false;
                } else if (responds.status == "PASSWORD_LENGTH_TOO_SHORT") {
                    $('#registerButton').html('Login Now');
                    $('#errorFrame').removeAttr('style');
                    $('#registerButton').attr('class', 'btn btn-primary');
                    $('#errorMsg').html('Error: Password length is too short (6 characters minimum).')
                    registering = false;
                } else if (responds.status == "PASSWORD_NOT_MATCH") {
                    $('#registerButton').html('Login Now');
                    $('#errorFrame').removeAttr('style');
                    $('#registerButton').attr('class', 'btn btn-primary');
                    $('#errorMsg').html('Error: Passwords do not match.')
                    registering = false;
                } else if (responds.status == "EMAIL_IN_USE") {
                    $('#registerButton').html('Login Now');
                    $('#errorFrame').removeAttr('style');
                    $('#registerButton').attr('class', 'btn btn-primary');
                    $('#errorMsg').html('Error: Email is already in used.')
                    registering = false;
                } else if (responds.status == "INTERNAL_ERROR") {
                    $('#registerButton').html('Login Now');
                    $('#errorFrame').removeAttr('style');
                    $('#registerButton').attr('class', 'btn btn-primary');
                    $('#errorMsg').html('Error: Internal error, please try again later.')
                    registering = false;
                }
            },
            error: function () {
                $('#registerButton').html('Login Now');
                $('#errorFrame').removeAttr('style');
                $('#registerButton').attr('class', 'btn btn-primary');
                $('#errorMsg').html('Error: Internal error, please try again later.')
                registering = false;
            }
        });
        return false;
    }
}
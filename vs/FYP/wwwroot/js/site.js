function logout() {
    $.ajax({
        url: '/Api/User/Logout',
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (responds) {
            if (responds.result == "OK") {
                $('#errorFrame').attr('style', 'display:none;')
                status = true;
            } else if (responds.result == "FIELD_INCOMPLETE") {
                $('#loginButton').html('Login Now');
                $('#errorFrame').removeAttr('style');
                $('#errorMsg').html('Error: Please login with your email and password.')
            } else if (responds.result == "USER_NOT_FOUND") {
                $('#loginButton').html('Login Now');
                $('#errorFrame').removeAttr('style');
                $('#errorMsg').html('Error: Email/password do not match.')
            } else if (responds.result == "PASSWORD_MISMATCH") {
                $('#loginButton').html('Login Now');
                $('#errorFrame').removeAttr('style');
                $('#errorMsg').html('Error: Email/password do not match.')
            }
        },
        error: function () {
            $('#loginButton').html('Login Now');
        }
    });
}
var loggingIn = false;
function login() {
    if (!loggingIn) {
        loggingIn = true;
        $('#loginButton').html('<i class="fas fa-spinner fa-spin"></i>');
        $('#loginButton').attr('class', 'btn btn-primary disabled');
        $.ajax({
            url: '/Api/User/WebLogin',
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            data: JSON.stringify({ 'Email': $('#Email').val(), 'Password': $('#Password').val() }),
            success: function (responds) {
                if (responds.result == "OK") {
                    $('#errorFrame').attr('style', 'display:none;')
                    $('#loginButton').html('Logged In');
                    $('#loginButton').attr('class', 'btn btn-success disabled');
                    checkUser();
                } else if (responds.result == "FIELD_INCOMPLETE") {
                    $('#loginButton').html('Login Now');
                    $('#errorFrame').removeAttr('style');
                    $('#loginButton').attr('class', 'btn btn-primary');
                    $('#errorMsg').html('Error: Please login with your email and password.')
                    loggingIn = false;
                } else if (responds.result == "USER_NOT_FOUND") {
                    $('#loginButton').html('Login Now');
                    $('#errorFrame').removeAttr('style');
                    $('#loginButton').attr('class', 'btn btn-primary');
                    $('#errorMsg').html('Error: Email/password do not match.')
                    loggingIn = false;
                } else if (responds.result == "PASSWORD_MISMATCH") {
                    $('#loginButton').html('Login Now');
                    $('#errorFrame').removeAttr('style');
                    $('#loginButton').attr('class', 'btn btn-primary');
                    $('#errorMsg').html('Error: Email/password do not match.')
                    loggingIn = false;
                }
            },
            error: function () {
                $('#loginButton').html('Login Now');
            }
        });
        return false;
    }
}

function checkUser() {
    $.ajax({
        url: '/Api/User/CheckStaffRole',
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function () {
            retrieveStore();
        }
    });
}

function retrieveStore() {
    $.ajax({
        url: '/Api/User/RetrieveTempStore',
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function () {
            setTimeout(function () { window.location.replace("/"); }, 2000);
        }
    });
}
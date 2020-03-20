var registering = false;
var targetEle;

function register(ele) {
    if (!registering) {
        targetEle = ele;
        registering = true;
        $(targetEle).find('#registerButton').html('<i class="fas fa-spinner fa-spin"></i>');
        $(targetEle).find('#registerButton').attr('class', 'btn btn-primary disabled');
        $.ajax({
            url: '/Api/User/Create',
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            data: JSON.stringify({ 'Email': $(targetEle).find('#Email').val(), 'Password': $(targetEle).find('#Password').val(), 'ConfirmPassword': $(targetEle).find('#CPassword').val(), 'FName': $(targetEle).find('#FName').val(), 'LName': $(targetEle).find('#LName').val() }),
            success: function (responds) {
                if (responds.status == "OK") {
                    $(targetEle).find('#errorFrame').attr('style', 'display:none;')
                    $(targetEle).find('#registerButton').html('Registered');
                    $(targetEle).find('#registerButton').attr('class', 'btn btn-success disabled');
                    setTimeout(function () { window.location.replace("/Login"); }, 2000);
                } else if (responds.status == "EMAIL_IS_NULL") {
                    $(targetEle).find('#registerButton').html('Login Now');
                    $(targetEle).find('#errorFrame').removeAttr('style');
                    $(targetEle).find('#registerButton').attr('class', 'btn btn-primary');
                    $(targetEle).find('#errorMsg').html('Error: Please enter your email.')
                    registering = false;
                } else if (responds.status == "PASSWORD_IS_NULL") {
                    $(targetEle).find('#registerButton').html('Login Now');
                    $(targetEle).find('#errorFrame').removeAttr('style');
                    $(targetEle).find('#registerButton').attr('class', 'btn btn-primary');
                    $(targetEle).find('#errorMsg').html('Error: Please enter your password.')
                    registering = false;
                } else if (responds.status == "PASSWORD_LENGTH_TOO_SHORT") {
                    $(targetEle).find('#registerButton').html('Login Now');
                    $(targetEle).find('#errorFrame').removeAttr('style');
                    $(targetEle).find('#registerButton').attr('class', 'btn btn-primary');
                    $(targetEle).find('#errorMsg').html('Error: Password length is too short (6 characters minimum).')
                    registering = false;
                } else if (responds.status == "PASSWORD_NOT_MATCH") {
                    $(targetEle).find('#registerButton').html('Login Now');
                    $(targetEle).find('#errorFrame').removeAttr('style');
                    $(targetEle).find('#registerButton').attr('class', 'btn btn-primary');
                    $(targetEle).find('#errorMsg').html('Error: Passwords do not match.')
                    registering = false;
                } else if (responds.status == "EMAIL_IN_USE") {
                    $(targetEle).find('#registerButton').html('Login Now');
                    $(targetEle).find('#errorFrame').removeAttr('style');
                    $(targetEle).find('#registerButton').attr('class', 'btn btn-primary');
                    $(targetEle).find('#errorMsg').html('Error: Email is already in used.')
                    registering = false;
                } else if (responds.status == "INTERNAL_ERROR") {
                    $(targetEle).find('#registerButton').html('Login Now');
                    $(targetEle).find('#errorFrame').removeAttr('style');
                    $(targetEle).find('#registerButton').attr('class', 'btn btn-primary');
                    $(targetEle).find('#errorMsg').html('Error: Internal error, please try again later.')
                    registering = false;
                }
            },
            error: function () {
                $(targetEle).find('#registerButton').html('Login Now');
                $(targetEle).find('#errorFrame').removeAttr('style');
                $(targetEle).find('#registerButton').attr('class', 'btn btn-primary');
                $(targetEle).find('#errorMsg').html('Error: Internal error, please try again later.')
                registering = false;
            }
        });
        return false;
    }
}
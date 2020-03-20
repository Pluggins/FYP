var loggingIn = false;
var targetEle;

function login(ele) {
    if (!loggingIn) {
        targetEle = ele;
        loggingIn = true;
        $(targetEle).find('#loginButton').html('<i class="fas fa-spinner fa-spin"></i>');
        $(targetEle).find('#loginButton').attr('class', 'btn btn-primary disabled');
        $.ajax({
            url: '/Api/User/WebLogin',
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            data: JSON.stringify({ 'Email': $(targetEle).find('#Email').val(), 'Password': $(targetEle).find('#Password').val() }),
            success: function (responds) {
                if (responds.result == "OK") {
                    $(targetEle).find('#errorFrame').attr('style', 'display:none;')
                    $(targetEle).find('#loginButton').html('Logged In');
                    $(targetEle).find('#loginButton').attr('class', 'btn btn-success disabled');
                    checkUser();
                } else if (responds.result == "FIELD_INCOMPLETE") {
                    $(targetEle).find('#loginButton').html('Login Now');
                    $(targetEle).find('#errorFrame').removeAttr('style');
                    $(targetEle).find('#loginButton').attr('class', 'btn btn-primary');
                    $(targetEle).find('#errorMsg').html('Error: Please login with your email and password.')
                    loggingIn = false;
                } else if (responds.result == "USER_NOT_FOUND") {
                    $(targetEle).find('#loginButton').html('Login Now');
                    $(targetEle).find('#errorFrame').removeAttr('style');
                    $(targetEle).find('#loginButton').attr('class', 'btn btn-primary');
                    $(targetEle).find('#errorMsg').html('Error: Email/password do not match.')
                    loggingIn = false;
                } else if (responds.result == "PASSWORD_MISMATCH") {
                    $(targetEle).find('#loginButton').html('Login Now');
                    $(targetEle).find('#errorFrame').removeAttr('style');
                    $(targetEle).find('#loginButton').attr('class', 'btn btn-primary');
                    $(targetEle).find('#errorMsg').html('Error: Email/password do not match.')
                    loggingIn = false;
                }
            },
            error: function () {
                $(targetEle).find('#loginButton').html('Login Now');
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
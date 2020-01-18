var vendorChecking = false;
var vendorCreating = false;
var vendorDeletingId;
var initiatingDeleteVendor = false;

$(document).ready(function () {
    $('#vendorList').DataTable({
        'order': [[2, "asc"]],
        "columnDefs": [
            { "width": "200px", "targets": 0 },
            { "width": "300px", "targets": 1 },
            { "width": "20px", "targets": 2 }
        ],
    });
});

$('#Email').on("keypress", function (e) {
    if (e.which == 13) {
        checkUser();
    }
});

function createVendor() {
    if (!vendorCreating) {
        vendorCreating = true;
        $('#createVendorButton').html('<i class="fas fa-spinner fa-spin"></i>');
        $('#createVendorButton').removeAttr('onclick');
        $('#createVendorButton').attr('class', 'btn btn-primary disabled');
        $.ajax({
            url: '/Api/Vendor/Create',
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            data: JSON.stringify({ 'Email': $('#Email').val(), 'Name': $('#Name').val() }),
            success: function (responds) {
                if (responds.result == "OK") {
                    $('#errorFrame').attr('style', 'background-color: #f3ffe5; border-radius: 5px; border: 1px solid #9f9292; padding:10px 20px; margin-top:20px;')
                    $('#errorMsg').html('<div style="text-align: center;">Vendor has been successfully created.</div>');
                    $('#createVendorButton').html('Created');
                    setTimeout(function () { window.location.replace("/Vendor"); }, 2000);
                } else if (responds.result == "FIELD_INCOMPLETE") {
                    $('#errorFrame').attr('style', 'background-color: #ffe6e5; border-radius: 5px; border: 1px solid #9f9292; padding:10px 20px; margin-top:20px;')
                    $('#errorMsg').html('<div style="text-align: center;">Please enter new vendor name and email.</div>');
                    $('#createVendorButton').attr('onclick', 'createVendor()');
                    $('#createVendorButton').attr('class', 'btn btn-primary');
                    $('#createVendorButton').html('Create');
                } else if (responds.result == "USER_NOT_FOUND") {
                    $('#errorFrame').attr('style', 'background-color: #ffe6e5; border-radius: 5px; border: 1px solid #9f9292; padding:10px 20px; margin-top:20px;')
                    $('#errorMsg').html('<div style="text-align: center;">User does not exist.</div>');
                    $('#createVendorButton').attr('onclick', 'createVendor()');
                    $('#createVendorButton').attr('class', 'btn btn-primary');
                    $('#createVendorButton').html('Create');
                }
            },
            error: function () {
                $('#errorFrame').attr('style', 'background-color: #ffe6e5; border-radius: 5px; border: 1px solid #9f9292; padding:10px 20px; margin-top:20px;')
                $('#errorMsg').html('<div style="text-align: center;">Server not responding, please try again later.</div>')
            }
        });
        vendorCreating = false;
    }
    return false;
}

function checkUser() {
    if (!vendorChecking) {
        if ($('#Email').val().length < 1 || $('#Name').val().length < 1) {
            $('#errorFrame').attr('style', 'background-color: #ffe6e5; border-radius: 5px; border: 1px solid #9f9292; padding:10px 20px; margin-top:20px;')
            $('#errorMsg').html('<div style="text-align: center;">Please fill in new vendor email and password.</div>');
        } else {
            vendorChecking = true;
            $('#searchBtn').html('<i class="fas fa-spinner fa-spin"></i>');
            $.ajax({
                url: '/Api/Vendor/CheckUser',
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                data: JSON.stringify({ 'Email': $('#Email').val() }),
                success: function (responds) {
                    if (responds.result == "OK") {
                        $('#errorFrame').attr('style', 'background-color: #f3ffe5; border-radius: 5px; border: 1px solid #9f9292; padding:10px 20px; margin-top:20px;')
                        $('#errorMsg').html('<div style="font-size:20px; margin-bottom:5px;">User Details</div>User ID: ' + responds.userID + '<br />First Name: ' + responds.firstName + '<br />Last Name: ' + responds.lastName + '<br />');
                    } else if (responds.result == "USER_NOT_FOUND") {
                        $('#errorFrame').attr('style', 'background-color: #ffe6e5; border-radius: 5px; border: 1px solid #9f9292; padding:10px 20px; margin-top:20px;')
                        $('#errorMsg').html('<div style="text-align: center;">User does not exist.</div>');
                    } else if (responds.result == "NO_PRIVILEGE") {
                        window.location.replace("/login");
                    }
                },
                error: function () {
                }
            });
            vendorChecking = false;
            $('#searchBtn').html('<i class="fas fa-search"></i>');
        }
    }
}

function reinitializeVendor() {
    $('#Email').val('');
    $('#Name').val('');
    $('#errorFrame').attr('style', 'display:none;');
}

function deleteVendor(id, ele) {
    $('#' + ele.id).html('<i class="fas fa-spinner fa-spin"></i>');
    $.ajax({
        url: '/Api/Vendor/RetrieveById',
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        data: JSON.stringify({ 'Id': id }),
        success: function (responds) {
            if (responds.result == "OK") {
                $('#vendorNameConfirmation').html(responds.vendor.name);
                $('#deleteVendorModal').modal('show');
                $('#' + ele.id).html('Delete');
                vendorDeletingId = id;
            } else if (responds.result == "NOT_FOUND") {
            }
        },
        error: function () {
        }
    });
}

function initiateDeleteVendor(id) {
    if (!initiatingDeleteVendor) {
        initiatingDeleteVendor = true;
        $('#deleteConfirmButton').attr('class', 'btn btn-primary disabled');
        $('#deleteConfirmButton').html('<i class="fas fa-spinner fa-spin"></i>');
        $.ajax({
            url: '/Api/Vendor/DeleteById',
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            data: JSON.stringify({ 'Id': id }),
            success: function (responds) {
                if (responds.result == "OK") {
                    $('#deleteConfirmButton').html('Deleted');
                    $('#deleteConfirmButton').attr('class', 'btn btn-success disabled');
                    vendorDeletingId = id;
                    setTimeout(function () { window.location.replace("/Vendor"); }, 2000);
                } else if (responds.result == "NOT_FOUND") {
                }
            },
            error: function () {
                $('#deleteConfirmButton').html('Delete');
                $('#deleteConfirmButton').attr('class', 'btn btn-primary');
                initiatingDeleteVendor = false;
            }
        });
    }
}
var menuDeletingId;
var creatingMenu = false;
var deletingMenu = false;
var initiatingDeleteMenu = false;

$(document).ready(function () {
    $('#menuList').DataTable({
        'order': [[2, "asc"]],
        "columnDefs": [
            { "width": "200px", "targets": 0 },
            { "width": "300px", "targets": 1 },
            { "width": "20px", "targets": 2 }
        ],
    });
});

function createMenu() {
    if (!creatingMenu) {
        if ($('#Name').val().length > 0) {
            creatingMenu = true;
            $('#createVendorButton').attr('class', 'btn btn-primary disabled');
            $('#createVendorButton').html('<i class="fas fa-spinner fa-spin"></i>');
            $.ajax({
                url: '/Api/Menu/Create',
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                data: JSON.stringify({ 'VendorId': vendorId, 'MenuName': $('#Name').val() }),
                success: function (responds) {
                    if (responds.result == "OK") {
                        $('#createVendorButton').attr('class', 'btn btn-success disabled');
                        $('#createVendorButton').html('Created');
                        setTimeout(function () { window.location.replace("/Menu/Edit/" + vendorId); }, 2000);
                    } else if (responds.result == "DOES_NOT_EXIST") {
                        creatingMenu = false;
                        $('#errorFrame').attr('style', 'background-color: #ffe6e5; border-radius: 5px; border: 1px solid #9f9292; padding:10px 20px; margin-top:20px;')
                        $('#errorMsg').html('<div style="text-align: center;">Internal error: Please try again later.</div>');
                        $('#createVendorButton').attr('class', 'btn btn-primary');
                        $('#createVendorButton').html('Create');
                    }
                },
                error: function () {
                    $('#createVendorButton').attr('class', 'btn btn-warning disabled');
                    $('#createVendorButton').html('ERROR');
                }
            });
        } else {
            $('#errorFrame').attr('style', 'background-color: #ffe6e5; border-radius: 5px; border: 1px solid #9f9292; padding:10px 20px; margin-top:20px;')
            $('#errorMsg').html('<div style="text-align: center;">Please enter the new menu name.</div>');
        }
    }
}

function reinitializeMenuCreation() {
    $('#Name').val('');
}

function deleteMenu(id, ele) {
    if (!deletingMenu) {
        deletingMenu = true;
        $('#' + ele.id).html('<i class="fas fa-spinner fa-spin"></i>');
        $.ajax({
            url: '/Api/Menu/RetrieveById',
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            data: JSON.stringify({ 'MenuId': id }),
            success: function (responds) {
                if (responds.result == "OK") {
                    $('#menuNameConfirmation').html(responds.menu.name);
                    $('#deleteMenuModal').modal('show');
                    $('#' + ele.id).html('Delete');
                    menuDeletingId = id;
                } else if (responds.result == "NOT_FOUND") {
                }
                deletingMenu = false;
            },
            error: function () {
            }
        });
    }
}

function initiateDeleteMenu(id) {
    if (!initiatingDeleteMenu) {
        initiatingDeleteMenu = true;
        $('#deleteConfirmButton').attr('class', 'btn btn-primary disabled');
        $('#deleteConfirmButton').html('<i class="fas fa-spinner fa-spin"></i>');
        $.ajax({
            url: '/Api/Menu/Delete',
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            data: JSON.stringify({ 'MenuId': id }),
            success: function (responds) {
                if (responds.result == "OK") {
                    $('#deleteConfirmButton').html('Deleted');
                    $('#deleteConfirmButton').attr('class', 'btn btn-success disabled');
                    vendorDeletingId = id;
                    setTimeout(function () { window.location.replace("/Menu/Edit/" + vendorId); }, 2000);
                } else if (responds.result == "NOT_FOUND") {
                }
            },
            error: function () {
                $('#deleteConfirmButton').html('Delete');
                $('#deleteConfirmButton').attr('class', 'btn btn-primary');
                initiatingDeleteMenu = false;
            }
        });
    }
}

function redirectToVendor() {
    window.location.href = "/Menu/Edit/" + $('#vendorSelect').val();
}
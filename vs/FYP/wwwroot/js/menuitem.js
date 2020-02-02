var menuId = '@Model.Menu.Id';
var creatingMenuItem = false;
var removingItem = false;
var initiatingRemoveItem = false;
var selectedItemId;

function createNewItemModal() {
    reinitializeCreateNewItem();
    $('#addItemModal').modal('show');
}

function reinitializeCreateNewItem() {
    $('#Name').val('');
    $('#Price').val('');
    $('#Desc').val('');
    $('#errorFrame').attr('style', 'display:none;');
}

function initiateCreateNewItem() {
    if (!creatingMenuItem) {
        creatingMenuItem = true;
        if ($('#Name').val().length < 1 || $('#Price').val().length < 1 || $('#Desc').val().length < 1) {
            $('#errorFrame').attr('style', 'background-color: #ffe6e5; border-radius: 5px; border: 1px solid #9f9292; padding:10px 20px; margin-top:20px;')
            $('#errorMsg').html('<div style="text-align: center;">Please enter the item name, unit price and description.</div>');
            creatingMenuItem = false;
        } else {
            $('#createItemButton').attr('class', 'btn btn-primary disabled');
            $('#createItemButton').html('<i class="fas fa-spinner fa-spin"></i>');
            $.ajax({
                url: '/Api/MenuItem/Create',
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                data: JSON.stringify({ 'MenuId': menuId, 'ItemName': $('#Name').val(), 'Price': $('#Price').val(), 'Desc': $('#Desc').val() }),
                success: function (responds) {
                    if (responds.result == "OK") {
                        $('#createItemButton').attr('class', 'btn btn-success disabled');
                        $('#createItemButton').html('Created');
                        setTimeout(function () { window.location.replace("/MenuItem/" + menuId); }, 2000);
                    } else if (responds.result == "DOES_NOT_EXIST") {
                        $('#errorFrame').attr('style', 'background-color: #ffe6e5; border-radius: 5px; border: 1px solid #9f9292; padding:10px 20px; margin-top:20px;')
                        $('#errorMsg').html('<div style="text-align: center;">Internal error: Please try again later.</div>');
                        $('#createItemButton').attr('class', 'btn btn-primary');
                        $('#createItemButton').html('Add');
                        creatingMenuItem = false;
                    } else if (responds.result == "PARSING_ERROR") {
                        $('#errorFrame').attr('style', 'background-color: #ffe6e5; border-radius: 5px; border: 1px solid #9f9292; padding:10px 20px; margin-top:20px;')
                        $('#errorMsg').html('<div style="text-align: center;">Input error: Price must be in numbers.</div>');
                        $('#createItemButton').attr('class', 'btn btn-primary');
                        $('#createItemButton').html('Add');
                        creatingMenuItem = false;
                    } else if (responds.result == "NO_PRIVILEGE") {
                        $('#errorFrame').attr('style', 'background-color: #ffe6e5; border-radius: 5px; border: 1px solid #9f9292; padding:10px 20px; margin-top:20px;')
                        $('#errorMsg').html('<div style="text-align: center;">Internal error: Please try again later.</div>');
                        $('#createItemButton').attr('class', 'btn btn-primary');
                        $('#createItemButton').html('Add');
                        creatingMenuItem = false;
                    }
                    deletingMenu = false;
                },
                error: function () {
                }
            });
        }
    }
}

function removeItem(itemId, ele) {
    if (!removingItem) {
        removingItem = true;
        $('#' + ele.id).html('<i class="fas fa-spinner fa-spin"></i>');
        $.ajax({
            url: '/Api/MenuItem/RetrieveById',
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            data: JSON.stringify({ 'MenuItemId': itemId }),
            success: function (responds) {
                if (responds.result == "OK") {
                    $('#' + ele.id).html('<i class="fas fa-minus" style="font-size:15px;"></i>');
                    selectedItemId = itemId;
                    $('#itemNameConfirmation').html(responds.menuName);
                    $('#deleteMenuItemModal').modal('show');
                } else {
                    $('#' + ele.id).html('<i class="fas fa-minus" style="font-size:15px;"></i>');
                }
                removingItem = false;
            },
            error: function () {
                $('#' + ele.id).html('<i class="fas fa-minus" style="font-size:15px;"></i>');
                removingItem = false;
            }
        });
    }
}

function initiateRemoveItem() {
    if (!initiatingRemoveItem) {
        initiatingRemoveItem = true;
        $('#deleteConfirmButton').attr('class', 'btn btn-primary disabled');
        $('#deleteConfirmButton').html('<i class="fas fa-spinner fa-spin"></i>');
        $.ajax({
            url: '/Api/MenuItem/Remove',
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            data: JSON.stringify({ 'MenuItemId': selectedItemId }),
            success: function (responds) {
                if (responds.result == "OK") {
                    $('#deleteConfirmButton').html('Deleted');
                    $('#deleteConfirmButton').attr('class', 'btn btn-success disabled');
                    setTimeout(function () { window.location.replace("/MenuItem/" + menuId); }, 2000);
                } else {
                    $('#deleteConfirmButton').html('Delete');
                    $('#deleteConfirmButton').attr('class', 'btn btn-primary');
                    initiatingRemoveItem = false;
                }
            },
            error: function () {
                $('#deleteConfirmButton').html('Delete');
                $('#deleteConfirmButton').attr('class', 'btn btn-primary');
                initiatingRemoveItem = false;
            }
        });
    }
}
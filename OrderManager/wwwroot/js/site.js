{
    $(document).on('click', '#Btn_AddItemSet', function (e) {
        $.ajax({
            url: '/Orders/DisplayNewItem',
            success: function (partialView) {
                $('.AddNewItem').append(partialView);
            }
        });
    });

    $(document).on("click", "#Btn_DeleteItem", function () {
        $(this).parent().parent().remove();
    });

    function getOrder() {
        var order = {

            Id: $("#OrderId").val(),
            ProviderId: $("#OrderProvider").val(),
            Date: $("#OrderDate").val(),
            Number: $("#OrderNumber").val()
        };

        return order;
    }

    function getItemSets() {
        itemSets = [];

        const itemIds = document.querySelectorAll('#ItemId');
        const itemNames = document.querySelectorAll('#ItemName');
        const itemQuantities = document.querySelectorAll('#ItemQuantity');
        const itemUnits = document.querySelectorAll('#ItemUnit');

        for (var i = 0; i < itemNames.length; i++) {
            if (itemNames[i].value != '') {
                itemSets.push({
                    Id: itemIds[i].value,
                    Name: itemNames[i].value,
                    Quantity: itemQuantities[i].value,
                    Unit: itemUnits[i].value
                });
            }
        }

        return itemSets;
    }

    $(document).on('click', '#Btn_Create', function (e) {

        var order = getOrder();
        var items = getItemSets();

        $.ajax({
            type: 'POST',
            data: { order, items },
            url: '/Orders/CreateOrder',
            success: function (response) {
                window.location.href = response.redirectToUrl;
            }
        });
    });

    jQueryAjaxPost = form => {

        var order = getOrder();
        var items = getItemSets();

        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: { order, items },
                success: function (response) {
                    window.location.href = response.redirectToUrl;
                }
            });
            //to prevent default form submit event
            return false;
        } catch (ex) {
            console.log(ex)
        }
    }
}
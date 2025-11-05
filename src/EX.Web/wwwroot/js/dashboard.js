$(function () {
    bootbox.setDefaults({
        locale: "ru",
        show: true,
        backdrop: true,
        closeButton: true,
        animate: true,
        centerVertical: true,
        scrollable: true
    });
});

function deleteConfirm(e) {

    bootbox.confirm({
        title: "Удалить?",
        message: "Вы уверены?",
        callback: function (result) {
            if (result) {
                $(e).closest("form").submit();
            }
        }
    });

    return false;
}

function confirmAction(e) {

    bootbox.confirm({
        title: "Вы уверены?", 
        callback: function (result) {
            if (result) {
                $(e).closest("form").submit();
            }
        }
    });

    return false;
}

$(document).ready(function () {
    $(".btn-primary").click(function () {
        var Id = $("#UserId").find('option:selected').val();
        window.location = "PhanQuyen/" + Id;
        //document.location.href = "GrantRights/PhanQuyen/" + Id;
    });

});
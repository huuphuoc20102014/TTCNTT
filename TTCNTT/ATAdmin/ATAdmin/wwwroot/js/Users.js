$(document).ready(function () {

    

    $(".btn-primary").click(function () {
        var Id = $("#UserId").find('option:selected').val();
        window.location = "PhanQuyen/" + Id;
        //document.location.href = "GrantRights/PhanQuyen/" + Id;

        //$.ajax({
        //    url: _urlUsers,
        //    type: 'POST',
        //    data: {
        //        ID: Id,
        //    },
        //    dataType: 'json',
        //    error: function (jqXHR, exception) {
        //        var msg = '';
        //        if (jqXHR.status === 0) {
        //            msg = 'Not connect.\n Verify Network.';
        //        } else if (jqXHR.status == 404) {
        //            msg = 'Requested page not found. [404]';
        //        } else if (jqXHR.status == 500) {
        //            msg = 'Internal Server Error [500].';
        //        } else if (exception === 'parsererror') {
        //            msg = 'Requested JSON parse failed.';
        //        } else if (exception === 'timeout') {
        //            msg = 'Time out error.';
        //        } else if (exception === 'abort') {
        //            msg = 'Ajax request aborted.';
        //        } else {
        //            msg = 'Uncaught Error.\n' + jqXHR.responseText;
        //        }
        //        alert(msg);
        //    },
        //    success: function (data) {
        //        if (data.errorMessage) {
        //            console.log(data.errorMessage);
        //            alert('Lỗi');
        //        }
        //        else {
        //        }
        //    },
        //});
    });

});
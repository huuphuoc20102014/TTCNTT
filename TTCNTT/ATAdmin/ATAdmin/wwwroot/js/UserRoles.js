$(document).ready(function () {

    $(".btn-primary").click(function () {
        var arrayId = $("#selected option");
        var idRoles;
        var array = [];

        for (var i = 0; i < arrayId.length; i++) {
            idRoles = $(arrayId[i]).val();
            array.push({ IDroles: idRoles });
        }
        console.log(array);
        if (arrayId.length == 0) {
            alert("Không có quyền nào được chọn");
        }
        else {
            $.ajax({
                url: _urlUserRoles,
                type: 'POST',
                data: JSON.stringify(array),
                dataType: 'application/json',
                error: function (jqXHR, exception) {
                    var msg = '';
                    if (jqXHR.status === 0) {
                        msg = 'Not connect.\n Verify Network.';
                    } else if (jqXHR.status == 404) {
                        msg = 'Requested page not found. [404]';
                    } else if (jqXHR.status == 500) {
                        msg = 'Internal Server Error [500].';
                    } else if (exception === 'parsererror') {
                        msg = 'Requested JSON parse failed.';
                    } else if (exception === 'timeout') {
                        msg = 'Time out error.';
                    } else if (exception === 'abort') {
                        msg = 'Ajax request aborted.';
                    } else {
                        msg = 'Uncaught Error.\n' + jqXHR.responseText;
                    }
                    alert(msg);
                },
                success: function (data) {
                    if (data.errorMessage) {
                        console.log(data.errorMessage);
                        alert('Lỗi');
                    }
                },
            });
        }

    });

});
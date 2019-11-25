$(document).ready(function () {

    var yourName = $('#Name');
    var yourEmail = $('#Email');
    var yourPhone = $('#Phone');
    var yourContent = $('#Content');
    var fkProductId = $('#fk_ProductId');

    $(".submit-comment").click(function () {
        $.ajax({
            url: _urlNewsComment,
            type: 'POST',
            data: {
                name: yourName.val(),
                email: yourEmail.val(),
                phone: yourPhone.val(),
                content: yourContent.val(),
                fkProductId: fkProductId.val()
            },
            dataType: 'json',
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
                else {
                    //Load lại div chỗ hiển thị cmt để hiện cmt
                    $(".users-list").load(location.href + " .users-list>*", "");

                    //Xóa tất cả dữ liệu trong ô nhập
                    yourName.val('');
                    yourEmail.val('');
                    yourPhone.val('');
                    yourContent.val('');
                }
            },
        });
    });

});
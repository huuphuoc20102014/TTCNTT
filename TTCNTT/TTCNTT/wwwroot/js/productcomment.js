$(document).ready(function () {

    //var yourName = $('#Name');
    //var yourEmail = $('#Email');
    //var yourPhone = $('#Phone');
    //var yourContent = $('#Content');
    var fkProductId = $('#fk_ProductId');

    $(".submit-comment").click(function () {
        $.ajax({
            url: _urlNewsComment,
            type: 'POST',
            data: {
                //name: yourName.val(),
                //email: yourEmail.val(),
                //phone: yourPhone.val(),
                //content: yourContent.val(),
                fkProductId: fkProductId.val()
            },
            dataType: 'json',
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
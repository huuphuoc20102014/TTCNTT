$(document).ready(function () {

    var fkProductId = $('#fk_ProductId');
    var yourName = $('#name');
    var yourEmail = $('#email');
    var yourPhone = $('#phone');
    var yourContent = $('#content');
    var myCommentId = $('#comment-id');

    $(".submit-comment").click(function () {
        $.ajax({
            url: _urlComment,
            type: 'POST',
            data: {
                fkProductId: fkProductId.val(),
                name: yourName.val(),
                email: yourEmail.val(),
                phone: yourPhone.val(),
                content: yourContent.val(),
                fkProductCommentId: myCommentId.val(),
            },
            dataType: 'json',
            success: function (data) {
                if (data.errorMessage) {
                    console.log(data.errorMessage);
                    alert('Lỗi');
                }
                else {
                    //alert('OK');
                    //Load lại div chỗ hiển thị cmt để hiện cmt
                    //$(".users-list").load(location.href + " .users-list>*", "");
                    $(".users-list").load(" .users-list > *");

                    $("h6.leave-comment").replaceWith('<h6 class="leave-comment">Để lại bình luận</h6>');
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
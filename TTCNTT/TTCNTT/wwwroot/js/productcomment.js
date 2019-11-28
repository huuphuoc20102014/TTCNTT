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
                    //Load lại chỗ danh sách comment
                    $(".users-list").load(" .users-list > *");

                    //Đăng ký lại nút Trả lời-Reply 
                    $(document).on('click', '.btn-Reply', function () {

                        //Lấy id của comment
                        var $clickedButton = $(this);
                        var commentId = $clickedButton.data('comment-id');
                        $('#comment-id').val(commentId);

                        //Hiệu ứng cuộn
                        $('html, body').animate({
                            scrollTop: parseInt($(".Reply-sec").offset().top)
                        }, 2000);

                        //Thế chỗ
                        $("h6.leave-comment").replaceWith('<h6 class="leave-comment" style="color:#116184;">Trả lời bình luận</h6>');
                        $(".submit-comment").val("Gửi trả lời").css("background-color", "#116184");
                        $(".submit-comment").css("background-color", "#116184")
                    }); 

                    //Thế lại như cũ
                    $("h6.leave-comment").replaceWith('<h6 class="leave-comment" style="color:#000;">Để lại bình luận</h6>');
                    $(".submit-comment").val("Gửi bình luận");
                    $(".submit-comment").css("background-color", "");

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
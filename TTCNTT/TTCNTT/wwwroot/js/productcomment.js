$(document).ready(function () {

    $(".button-comment").click(function () {

        var fkProductId = $('#fk_ProductId');
        var yourName = $('#name');
        var yourEmail = $('#email');
        var yourPhone = $('#phone');
        var yourContent = $('#content');

        var dataArray = {
            FkProductId: fkProductId.val(),
            MyName: yourName.val(),
            MyEmail: yourEmail.val(),
            MyPhone: yourPhone.val(),
            MyContent: yourContent.val()
        };

        $.ajax({
            url: _urlComment,
            type: 'POST',
            data: JSON.stringify(dataArray),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data.errorMessage) {
                    console.log(data.errorMessage);
                    alert('Lỗi');
                }
                else {
                    //Load lại chỗ danh sách comment
                    $(".users-list").load(" .users-list > *");
                    


                    //Xóa tất cả dữ liệu trong ô nhập
                    yourName.val('');
                    yourEmail.val('');
                    yourPhone.val('');
                    yourContent.val('');

                    //Đăng ký lại nút Trả lời-Reply 
                    $(document).on('click', '.btn-Reply', function () {

                        //Lấy id của comment
                        var $clickedButton = $(this);
                        var commentId = $clickedButton.data('comment-id');
                        $('#comment-id').val(commentId);

                        //Hiệu ứng cuộn
                        var arrayId = $(".users-list .Reply-section");
                        for (var i = 0; i < arrayId.length; i++) {
                            if ($(arrayId[i]).attr('id') == commentId) {
                                $("#" + $(arrayId[i]).attr('id')).slideToggle();
                            }
                        }
                        //$('html, body').animate({
                        //    scrollTop: parseInt($(".Reply-sec").offset().top)
                        //}, 2000);
                    }); 

                }
            },
        });
    });

    $(".button-reply").click(function () {

        var fkProductId = $('#fk_ProductId');
        var myName = $('#' + $('#comment-id').val()).find('.MyName');
        var myEmail = $('#' + $('#comment-id').val()).find('.MyEmail');
        var myPhone = $('#' + $('#comment-id').val()).find('.MyPhone');
        var myContent = $('#' + $('#comment-id').val()).find('.MyContent');
        var myCommentId = $('#comment-id');

        var array = {
            FkProductId: fkProductId.val(),
            MyName: myName.val(),
            MyEmail: myEmail.val(),
            MyPhone: myPhone.val(),
            MyContent: myContent.val(),
            FkProductCommentId: myCommentId.val()
        };

        $.ajax({
            url: _urlComment,
            type: 'POST',
            data: JSON.stringify(array),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data.errorMessage) {
                    console.log(data.errorMessage);
                    alert('Lỗi');
                }
                else {
                    //Load lại chỗ danh sách comment
                    $(".users-list").load(" .users-list > *");

                    //Xóa tất cả dữ liệu trong ô nhập
                    myName.val('');
                    myEmail.val('');
                    myPhone.val('');
                    myContent.val('');

                    //Đăng ký lại nút Trả lời-Reply 
                    $(document).on('click', '.btn-Reply', function () {

                        //Lấy id của comment
                        var $clickedButton = $(this);
                        var commentId = $clickedButton.data('comment-id');
                        $('#comment-id').val(commentId);

                        //Hiệu ứng cuộn
                        var arrayId = $(".users-list .Reply-section");
                        for (var i = 0; i < arrayId.length; i++) {
                            if ($(arrayId[i]).attr('id') == commentId) {
                                $("#" + $(arrayId[i]).attr('id')).slideToggle();
                            }
                        }
                        //$('html, body').animate({
                        //    scrollTop: parseInt($(".Reply-sec").offset().top)
                        //}, 2000);
                    }); 

                }
            },
        });
    });

});
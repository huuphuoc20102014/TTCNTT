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
                    //Refresh Comment
                    $(".blog-comment-sec").load(" .blog-comment-sec > *");

                    //Re-register event
                    $(document).on('click', '.btn-Reply', function () {

                        //Get id of comment
                        var $clickedButton = $(this);
                        var commentId = $clickedButton.data('comment-id');
                        $('#comment-id').val(commentId);

                        //Reply
                        var arrayId = $(".users-list .Reply-section");
                        for (var i = 0; i < arrayId.length; i++) {
                            if ($(arrayId[i]).attr('id') == commentId) {
                                $("#" + $(arrayId[i]).attr('id')).slideToggle();
                            }
                        }

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
                    //Refresh div comment
                    $(".users-list").load(" .users-list > *");

                    //Empty input
                    myName.val('');
                    myEmail.val('');
                    myPhone.val('');
                    myContent.val('');

                    //Re-register event
                    $(document).on('click', '.btn-Reply', function () {

                        //Get id of comment
                        var $clickedButton = $(this);
                        var commentId = $clickedButton.data('comment-id');
                        $('#comment-id').val(commentId);

                        //Reply
                        var arrayId = $(".users-list .Reply-section");
                        for (var i = 0; i < arrayId.length; i++) {
                            if ($(arrayId[i]).attr('id') == commentId) {
                                $("#" + $(arrayId[i]).attr('id')).show(300);
                            }
                        }

                    }); 

                }
            },
        });
    });

});
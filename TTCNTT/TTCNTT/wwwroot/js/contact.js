$(document).ready(function () {

    $("#ContactForm").validate({
        rules: {
            Name: {
                required: true,
                maxlength: 50
            },
            Email: {
                required: true,
                email: true,
                maxlength: 50
            },
            Phone: {
                required: true,
                minlength: 8,
                maxlength: 20
            },
            Body: {
                required: true,
                maxlength: 1000
            }
        },
        messages: {
            Name: {
                required: "Vui lòng nhâp tên!",
                maxlength: "Tên quá dài!"
            },
            Email: {
                required: 'Vui lòng nhập email!',
                email: 'Xin nhập email hợp lệ!',
                maxlength: 'Email quá dài!'
            },
            Phone: {
                required: 'Vui lòng nhập số điện thoại!',
                minlength: 'Xin nhập số hợp lệ!',
                maxlength: 'Số điện thoại quá dài!'
            },
            Body: {
                required: 'Vui lòng nhập nội dung!',
                maxlength: 'Nội dung quá dài!'
            }
        },

    });

    $("#contact-submit").click(function () {

        var yourName = $('#Name');
        var yourEmail = $('#Email');
        var yourTelephone = $('#Phone');
        var yourMessage = $('#Body');

        var listContact = {
            Name: yourName.val(),
            Email: yourEmail.val(),
            Phone: yourTelephone.val(),
            Content: yourMessage.val()
        };


        if ($("#ContactForm").valid()) {
            // make the ajax call

            $.ajax({
                url: _urlContact,
                type: 'POST',
                data: JSON.stringify(listContact),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data.errorMessage) {
                        console.log(data.errorMessage);

                        $(".col-md-12").append($('<div id="contact-failed"> Lỗi...!, gửi thất bại, mời bạn thử lại sau.</div>'));
                    }
                    else {
                        $(".content-area").load(location.href + " .content-area>*", "");

                    }
                },
            });
        }
        else { }

    });
});
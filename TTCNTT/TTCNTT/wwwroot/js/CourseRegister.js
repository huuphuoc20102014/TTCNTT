$(document).ready(function () {

    $("#RegisterForm").validate({
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
            member: {
                required: true,
                maxlength: 5
            },
            Content: {
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
            member: {
                required: 'Vui lòng nhập số người học!',
                maxlength: 'Số lượng quá nhiều!'
            },
            Content: {
                required: 'Vui lòng nhập nội dung!',
                maxlength: 'Nội dung quá dài!'
            }
        },

    });
    
    $("#contact-submit").click(function () {

        var yourName = $('#Name');
        var yourEmail = $('#Email');
        var yourPhone = $('#Phone');
        var yourContent = $('#Content');
        var yourMember = $('#member');
        var YourCourse = $('#fk_CourseId');

        if ($("#RegisterForm").valid()) {
            // make the ajax call

            $.ajax({
                url: _urlCourseRegister,
                type: 'POST',
                data: JSON.stringify({
                    Name: yourName.val(),
                    Email: yourEmail.val(),
                    Phone: yourPhone.val(),
                    Member: parseInt(yourMember.val()) ,
                    Content: yourContent.val(),
                    CourseId: YourCourse.val()
                }),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data.errorMessage) {
                        console.log(data.errorMessage);
                        alert('Lỗi');
                    }
                    else {
                        $(".Reply-sec").load(location.href + " .Reply-sec>*", "");

                    }
                },
            });
        }
        else { }


    });

});
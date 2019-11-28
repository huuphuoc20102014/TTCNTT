$(document).ready(function () {
    $("#CommentForm").validate({
        rules: {
            name: {
                required: true,
                maxlength: 50
            },
            email: {
                required: true,
                maxlength: 50
            },
            phone: {
                required: true,
                maxlength: 20
            },
            content: {
                required: true,
            },
        },
        messages: {
            name: {
                required: "Vui lòng nhâp tên!",
                maxlength: "Nội dung quá dài!"
            },
            email: {
                required: 'Vui lòng nhập email!',
                maxlength: 'Nội dung quá dài!'
            },
            phone: {
                required: 'Vui lòng nhập số điện thoại!',
                maxlength: 'Nội dung quá dài!'
            },
            content: {
                required: 'Vui lòng nhập nội dung!',
            },
        }
    });
});
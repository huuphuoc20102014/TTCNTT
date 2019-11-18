$(document).ready(function () {
    $("#myform").validate({
        rules: {
            hoten: {
                required: true,
                maxlength: 50
            },
            mail: {
                required: true,
                maxlength: 50
            },
            sdt: {
                required: true,
                maxlength: 20
            },
            noidung: {
                required: true,
            },
        },
        messages: {
            hoten: {
                required: "Vui lòng nhâp tên!",
                maxlength: "Nội dung quá dài!"
            },
            mail: {
                required: 'Vui lòng nhập email!',
                maxlength: 'Nội dung quá dài!'
            },
            sdt: {
                required: 'Vui lòng nhập số điện thoại!',
                maxlength: 'Nội dung quá dài!'
            },
            noidung: {
                required: 'Vui lòng nhập nội dung!',
                maxlength: 'Nội dung quá dài!'
            },
        }
    });
});
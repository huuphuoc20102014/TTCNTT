﻿$(document).ready(function () {
    $("#CommentForm").validate({
        rules: {
            Name: {
                required: true,
                maxlength: 50
            },
            Email: {
                required: true,
                maxlength: 50
            },
            Phone: {
                required: true,
                maxlength: 20
            },
            Content: {
                required: true,
            },
        },
        messages: {
            Name: {
                required: "Vui lòng nhâp tên!",
                maxlength: "Nội dung quá dài!"
            },
            Email: {
                required: 'Vui lòng nhập email!',
                maxlength: 'Nội dung quá dài!'
            },
            Phone: {
                required: 'Vui lòng nhập số điện thoại!',
                maxlength: 'Nội dung quá dài!'
            },
            Content: {
                required: 'Vui lòng nhập nội dung!',
            },
        }
    });
});
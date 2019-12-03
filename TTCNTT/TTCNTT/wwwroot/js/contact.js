$(document).ready(function () {

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
    });

});
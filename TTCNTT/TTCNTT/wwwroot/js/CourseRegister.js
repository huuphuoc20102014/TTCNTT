$(document).ready(function () {

    var yourName = $('#Name');
    var yourEmail = $('#Email');
    var yourPhone = $('#Phone');
    var yourContent = $('#Content');
    var yourMember = $('#member');
    var YourCourse = $('#fk_CourseId');
    
    $("#contact-submit").click(function () {
        $.ajax({
            url: _urlCourseRegister,
            type: 'POST',
            data: {
                name: yourName.val(),
                email: yourEmail.val(),
                phone: yourPhone.val(),
                membercount: parseInt(yourMember.val()) ,
                content: yourContent.val(),
                fkcourseid: YourCourse.val()
            },
            dataType: 'json',
            error: function (jqXHR, exception) {
                var msg = '';
                if (jqXHR.status === 0) {
                    msg = 'Not connect.\n Verify Network.';
                } else if (jqXHR.status == 404) {
                    msg = 'Requested page not found. [404]';
                } else if (jqXHR.status == 500) {
                    msg = 'Internal Server Error [500].';
                } else if (exception === 'parsererror') {
                    msg = 'Requested JSON parse failed.';
                } else if (exception === 'timeout') {
                    msg = 'Time out error.';
                } else if (exception === 'abort') {
                    msg = 'Ajax request aborted.';
                } else {
                    msg = 'Uncaught Error.\n' + jqXHR.responseText;
                }
                alert(msg);
            },
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
    });

});
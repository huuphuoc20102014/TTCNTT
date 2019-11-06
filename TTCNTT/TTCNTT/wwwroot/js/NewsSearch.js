$(document).ready(function () {

    var keyWord = $('#btn-search-event');

    $("#btn-search-event").keypress(function (e) {
        var key = e.which;
        if (key == 13)  // the enter key code
        {
            $.ajax({
                url: _urlNewsSearch,
                type: 'POST',
                data: {
                    search: keyWord.val()
                },
                dataType: 'text',
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
                    

                    }
                },
            });
        }

    });

});
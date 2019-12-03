$(document).ready(function () {

    var keyWord = $('#btn-search-event');

    $("#btn-search-event").keypress(function (e) {
        var key = e.which;
        if (key == 13)  // the enter key code
        {
            $.ajax({
                url: _urlCourseSearch,
                type: 'POST',
                data: {
                    search: keyWord.val()
                },
                dataType: 'text',
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
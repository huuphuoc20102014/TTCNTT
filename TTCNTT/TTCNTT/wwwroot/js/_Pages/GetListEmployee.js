$(document).ready(function () {
    $.ajax({
        url: _urlGetListEmployee, // Duong dan duoc set o view chinh
        type: "GET",
        error: function ($xhr, textStatus, errorThrown) {

        },
        success: function (data) {
            if (data) {
                console.log(data);
                // Truong hop thanh cong
                if (data.IsOk === true) {

                    var $listEmployee = $("#tt-employee");
                    var $template = $("#tt-template-list-employee").children();

                    // load
                    for (var i = 0; i < data.Payload.length; i++) {
                        var itemData = data.Payload[i];
                        var $itemView = $template.clone();

                        $itemView.find('#tt-employee-name').text(itemData.Name);
                        $itemView.find('#tt-employee-name').attr('title', itemData.Name);
                        $itemView.find('#tt-employee-name').attr('href', "Employee" + "/" + itemData.SlugName);
                        $itemView.find('#add-src').attr("src", "/Images/Employee/" + itemData.ImageSlug);
                        $itemView.find('#add-src').attr('title', itemData.Name);
                        $itemView.find('#add-src').attr('href', "Employee" + "/" + itemData.SlugName);

                        $itemView.find('#tt-Specialize').text(itemData.Specialize);
                        $itemView.find('#tt-employee-detail').text(itemData.LongDescription_Html);

                        $listEmployee.append($itemView);
                    }

                }
                // Truong hop loi tra ve tu web
                else {
                    console.log('start data.IsOk else');
                    // Show thong bao loi
                }
            }
        }
    });
});
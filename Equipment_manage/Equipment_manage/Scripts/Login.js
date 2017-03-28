$(document).ready(
    function () {
        $('#load').hide();
        $("#submit").click(function () {
            $('#load').show();
            var $e_ID = $("#e_ID").val();
            var $u_Password = $("#u_Password").val();
            $.ajax({
                url: "/Login/CheckLogin",
                type: "post",
                data: { "e_ID": $e_ID, "u_Password": $u_Password },
                success: function (msg) {
                    if (msg.state == "success") {
                        location = "/index/index";
                    }
                    else {
                        location = "/Login/index";
                    }
                    $('#load').hide();
                },
                error: function () {
                    $('#load').hide();
                    Messageshow("加载失败");
                }
            });
        });
        $("#clean").click(function () {
            $("#e_ID").val("");
            $("#u_Password").val("");
        });
    })
$(function () {
   
});
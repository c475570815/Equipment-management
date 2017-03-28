var table_id = '#table';
var button_name = '#btn_query';
var add_user = "";
var dic= Array();
var alert_data;
$(document).ready(function () {
    LoadSlect();
    $(table_id).bootstrapTable({
        url: '/Equipment/GetEquipment',
        method: 'post',
        toolbar: "#toolbar",//指定工具栏
        showRefresh: true, //是否显示刷新按钮
        pagination: true,     //是否显示分页（*）
        sidePagination: "server",   //分页方式：client客户端分页，server服务端分页（*）
        pageSize: 10,      //每页的记录行数（*）
        pageList: [10, 25, 50, 100],  //可供选择的每页的行数（*）
        queryParamsType: '',// 设置为 ''  在这种情况下传给服务器的参数为：pageSize,pageNumber// queryParams: oTableInit.queryParams,//传递参数（*）
        responseHandler: function(res)
        {
            for (var i = 0; i < res.rows.length; i++)
            {
                if (res.rows[i].di_Warranty!="")
                    res.rows[i].di_Warranty = data_string(res.rows[i].di_Warranty);
                if (res.rows[i].di_ButTime != "")
                        res.rows[i].di_ButTime = data_string(res.rows[i].di_ButTime);
            }
            alert_data = null
            return res;
        },//在得到数据后对数据做处理再加载
        onClickRow: function (row, $element) {
            $element.find("input")[0].click();
        },
        onCheck: function (row, $element)
        {
            alert_data = row;
        },
        columns: [
                   {
                     radio: true,
                     title:'修改'
                   }, {
                    sortable:true,
                    field: 'di_EqId',
                    title: '编号'
                }, {
                    field: 'di_Name',
                    title: '名称'
                }, {
                    field: 'di_Model',
                    title: '型号'
                }, {
                    field: 'di_ManuFacturers',
                    title: '出品商'
                }, {
                    sortable: true,
                    field: 'di_ButTime',
                    title: '购买日期'
                }, {
                    field: 'di_Warranty',
                    title: '保修截至日期'
                }, {
                    sortable: true,
                    field: 'di_Status',
                    title: '状态'
                }, {
                    field: 'di_User',
                    title: '使用人员'
                }, {
                    field: 'di_Position',
                    title: '存放位置'
                },
        ]
    });
    $(function () {
        $('#addform').bootstrapValidator({
            message: 'This value is not valid',
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {
                ID:
                    {
                        validators: {
                            notEmpty: {
                                message: '设备编号不能为空'
                            },
                            regexp: {/* 只需加此键值对，包含正则表达式，和提示 */
                                regexp: /^[0-9]*$/,
                                message: '只能是数字'
                            },
                            stringLength: {/*长度提示*/
                                min: 1,
                                max: 5,
                                message: '编号长度必须在1到5之间'
                            }/*最后一个没有逗号*/
                        }
                    },
                Name: {
                    validators: {
                        notEmpty: {
                            message: '设备名不能为空'
                        },
                        stringLength: {/*长度提示*/
                            min: 1,
                            max: 20,
                            message: '用户名长度必须在1到5之间'
                        }/*最后一个没有逗号*/
                    }
                },
                User: {
                    validators: {
                        notEmpty: {
                            message: '使用人名不能为空'
                        },
                        datatype: "string",
                    }
                },
                Model: {
                    validators: {
                        notEmpty: {
                            message: '型号不能为空'
                        },
                        datatype: "string",
                    }
                },
                Position: {
                    validators: {
                        notEmpty: {
                            message: '位置不能为空'
                        },
                        datatype: "string",
                    }
                },
                ManuFacturers: {
                    validators: {
                        notEmpty: {
                            message: '厂商不能为空'
                        },
                        datatype: "string",
                    }
                },
                Price: {
                    validators: {
                        notEmpty: {
                            message: '价格不能为空'
                        },
                        regexp: {/* 只需加此键值对，包含正则表达式，和提示 */
                            regexp: /^[0-9]*$/,
                            message: '只能是数字'
                        }
                    }
                },
            }
        });
    });//表单验证 
    $('#addModal').on('hidden.bs.modal', function () {
        $('#addform')[0].reset();
    })
});
function Editform(str)
{
    if (str == "add") {
        $('#btnSubmit')[0].setAttribute("onclick", "Editrecord('/Equipment/Add')")
        $('#addform')[0].reset();
        $("#Users").selectpicker({
            'selectedText': 'cat'
        });
    }
    else
    {
        if (alert_data==null)
        {
            Messageshow("请选中记录后再修改");
        }
        Alertrecord(alert_data);
        $('#btnSubmit')[0].setAttribute("onclick", "Editrecord('/Equipment/Alert')")
    }
    $('#addModal').modal('show');
}
function Editrecord(url)
{
    $('#addform').bootstrapValidator('validate');//验证表单
    if (!$("#addform").data('bootstrapValidator').isValid())//如果有错则不提交
        return;
    var typename= document.getElementById("Type").value;
    var date = Getjosn();
    $.ajax({
        url: url,
        type: 'GET',
        data: {
            date: date,
            typename:typename
        },
        success: function (resultData)
        {
            $('#addModal').modal('hide');//隐藏模态框
            $(table_id).bootstrapTable('refresh', { url: '/Equipment/GetEquipment' });
            Messageshow(resultData);
        }
    });
}
function Getjosn()
{
    var date = new Object();
    var type;
    $("input").each(function () {
        switch (this.id)
        {
            case "Name": date.di_Name = this.value; break;
            case "ID": date.di_EqId = this.value; break;
            case "Model": date.di_Model = this.value; break;
            case "Price": date.di_Price = this.value; break;
            case "Position": date.di_Position = this.value; break;
            case "ManuFacturers": date.di_ManuFacturers = this.value; break;
            case "Warranty": date.di_Warranty = "/Date(" + new Date(this.value).getTime() + ")/"; break;
            case "Buytime": date.di_ButTime = "/Date(" + new Date(this.value).getTime() + ")/"; break;
        }

    })
    date.di_Status = $("#Status")[0].value;
    date.di_User = $("button[data-id='User']>span")[0].innerText.replace(/\s/g, "");
    return JSON.stringify(date);
}
function query() {
    var keyword=$.trim(document.getElementById("keyword").value);
    var id=$.trim(document.getElementById("txt_search_id").value);
    $(table_id).bootstrapTable('refresh', { query: {keyword :keyword, id:id}});
}
function entersearch() {
    var event = window.event || arguments.callee.caller.arguments[0];
    if (event.keyCode == 13) {
        query();
    }
}
function data_string(str) {
    var d = eval('new ' + str.substr(1, str.length - 2));
    var ar_date = [d.getFullYear(), d.getMonth() + 1, d.getDate()];
    for (var i = 0; i < ar_date.length; i++) ar_date[i] = dFormat(ar_date[i]);
    return ar_date.join('-');
    function dFormat(i) { return i < 10 ? "0" + i.toString() : i; }
}
function Messageshow(date) {
    var a = document.getElementById("Message");
    a.innerText = date;
    $('#myModal').modal('show');
}
function LoadSlect()
{
    AjaxGet("/Common/GetAllpeople", LoadNameSelect);
    AjaxGet("/Common/GetAlltype", LoadTypeSelect);

}
function AjaxGet(url,success)
{
    $.ajax({
        url: url,
        type: "get",
        success: function (resultData) {
            success(resultData);
        }
    });
}
function LoadNameSelect(resultData)
{
    for (var i = 0; i < resultData.total; i++)
        {
        document.getElementById("User").options.add(new Option(resultData.rows[i], resultData.rows[i]));
    }
}
function LoadTypeSelect(resultData)
{
    document.getElementById("Type").innerHTML = "<option value=''>--请选择--</option>";
    for (var i = 0; i < resultData.total; i++) {
        dic[resultData.rows[i].dt_ID] = resultData.rows[i].dt_Name
        document.getElementById("Type").innerHTML += "<option value=" + resultData.rows[i].dt_Name + ">" + resultData.rows[i].dt_Name + "</option>";
    }
}
function Alertrecord(alert_data)
{
    var select =$(".form-group").find("select");
    var input = $(".form-group").find("input");
    for (var i = 0; i < select.length; i++)
        switch (select[i].id) {
            case "Status": select[i].value = alert_data.di_Status; break;
            case "Type": select[i].value = dic[alert_data.dt_ID]; break;
            case "User": $("button[data-id='User']>span")[0].innerText = alert_data.di_User; break;
        }
    for (var i = 0; i < input.length; i++)
        switch(input[i].id)
        {
            
            case "Name": input[i].value = alert_data.di_Name; break;
            case "ID": input[i].value = alert_data.di_EqId; break;
            case "Model": input[i].value = alert_data.di_Model; break;
            case "Price": input[i].value= alert_data.di_Price; break;
            case "Position": input[i].value= alert_data.di_Position; break;
            case "ManuFacturers": input[i].value= alert_data.di_ManuFacturers; break;
            case "Warranty": input[i].value= alert_data.di_Warranty; break;
            case "Buytime": input[i].value = alert_data.di_ButTime; break;
        }
}
function Delcfm()
{
    if (alert_data == null)
    {
        Messageshow("请选择一行再做删除");
        return;
    }
    $("#delcfmModel").modal('show');
}
function Dte()
{
    $.ajax({
        url: "/Equipment/Delete",
        type: "get",
        data:{id:alert_data.di_EqId},
        success: function (resultData) {
            $("#delcfmModel").modal('hide');
            $(table_id).bootstrapTable('refresh', { url: '/Equipment/GetEquipment' });
            Messageshow(resultData);
        }
    });
}


var table_id = '#table';
var button_name = '#btn_query';
var alert = null;
var dic = Array();
var del_id = "";
$(document).ready(
function () {
    GetType();
    $(table_id).bootstrapTable({
        url: '/Employee/GetEmployee',
        method: 'get',
        striped: true,
        showRefresh: true, //是否显示刷新按钮
        pagination: true,     //是否显示分页（*）
        sidePagination: "server",   //分页方式：client客户端分页，server服务端分页（*）
        pageSize: 10,      //每页的记录行数（*）
        pageList: [30],  //可供选择的每页的行数（*）
        toolbar: "#toolbar",//指定工具栏
        queryParamsType: '',// 设置为 ''  在这种情况下传给服务器的参数为：pageSize,pageNumber// queryParams: oTableInit.queryParams,//传递参数（*）
        onClickRow: function (row, $element) {
            $element.find("input")[0].click();
        },
        onCheck: function (row, $element) {
            del_id = del_id.replace(row.e_ID + ",", "");
            del_id += row.e_ID + ",";//replace
        },
        onUncheck: function (row, $element) {
            del_id = del_id.replace(row.e_ID + ",", "");
        },//取消选中事件
        responseHandler: function (res) {
            for (var i = 0; i < res.rows.length; i++) {
                res.rows[i].d_ID = dic[res.rows[i].d_ID];
            }
            return res;
        },//在得到数据后对数据做处理再加载
        columns: [{
                         checkbox: true
                        },
                        {
                            field: 'e_ID',
                            title: '员工编号'
                        }, {
                            field: 'e_Name',
                            title: '姓名'
                        }, {
                            field: 'e_Gender',
                            title: '性别',
                            sortable:true,
                        }, {
                            field: 'e_Mobile',
                            title: '电话'
                        }, {
                            field: 'd_ID',
                            sortable:true,
                            title: '部门'
                        }, {
                            width:130,
                            title: '操作',
                            formatter:function(value,row,index)
                            {
                                josn = JSON.stringify(row);
                                josn = josn.replace(/\"/g, "'");
                                var inf = '<button type="button" class="btn btn-default btn-xs" onclick="Editform('+josn+',event)"><span class="glyphicon glyphicon-pencil"></span>修改</button>'
                                    + '<button type="button" class="btn btn-default btn-xs" onclick="Rowdel(' + row.e_ID + ')"><span class="glyphicon glyphicon-remove"></span>删除</button>';
                                return  inf;
                            }
                        },
                     ]
    });
    $(function () {
        $('#Editform').bootstrapValidator({
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
                                message: '人员编号不能为空'
                            },
                            regexp: {/* 只需加此键值对，包含正则表达式，和提示 */
                                regexp: /^[0-9]*$/,
                                message: '只能是数字'
                            },
                            stringLength: {/*长度提示*/
                                min: 6,
                                max: 6,
                                message: '必须为6位数'
                            }
                        }
                    },
                Name:
                    {
                        validators: {
                            notEmpty: {
                                message: '价格不能为空'
                            },
                            regexp: {/* 只需加此键值对，包含正则表达式，和提示 */
                                regexp: /[\u4e00-\u9fa5]/,
                                message: '只能是中文'
                            }
                        }
                    },
                Department:
                      {
                          validators: {
                              notEmpty: {
                                  message: '部门不能为空'
                              }
                          }

                      },
                Sex:
               {
                   validators: {
                       notEmpty: {
                           message: '性别不能为空'
                       }
                   }

               },
                PhoneNumber:
                {
                    validators: {
                        notEmpty: {
                            message: '电话号码不能为空'
                        },
                        regexp: {/* 只需加此键值对，包含正则表达式，和提示 */
                            regexp: /^(13[0-9]|14[5|7]|15[0|1|2|3|5|6|7|8|9]|18[0|1|2|3|5|6|7|8|9])\d{8}$/,
                            message: '请输入正确的手机号码'
                        }
                    }
                }
            }
        });
    });//表单验证 
    });
function query() {
    var keyword = $.trim(document.getElementById("keyword").value);
    var id= $.trim(document.getElementById("txt_search_id").value);
    $(table_id).bootstrapTable('refresh', { query: { keyword: keyword, id: $.trim(id) } });
}
function entersearch() {
    var event = window.event || arguments.callee.caller.arguments[0];
    if (event.keyCode == 13) {
        query();
    }
}
function Deleteinf()
{
    if (del_id == "") {
        Messageshow("请选中一条记录后再做删除");
        return;
    }
    $('#delcfmModel').modal('show');
}
function Delete(del_id)
{
    $.ajax({
     url: "/Employee/Delete",
     type: "get",
     data:{del_id:del_id},
     success: function (resultData) {
         Messageshow(resultData);
         $(table_id).bootstrapTable('refresh', { url: '/Employee/GetEmployee' });
         del_id = "";
     }
    })
}
function Messageshow(date) {
    var a = document.getElementById("Message");
    a.innerText = date;
    $('#myModal').modal('show');
}
function Editform(str,event)
{
    if (str == "add") {
        $('#form')[0].reset();
        $('#btnSubmit')[0].setAttribute("onclick", "Editrecord('/Employee/Add')");
    }
    else {

        var select = $(".form-group").find("select");
        var input = $(".form-group").find("input");
        for (var i = 0; i < input.length; i++)
            switch (input[i].id) {
                case "PhoneNumber": input[i].value = str.e_Mobile; break;
                case "Name": input[i].value = str.e_Name; break;
                case "ID": input[i].value = str.e_ID; break;
            }
        for (var i = 0; i < select.length; i++)
            switch (select[i].id) {
                case "Department": select[i].value = str.d_Name; break;
                case "Sex": select[i].value = str.e_Gender; break;
            }
        $('#btnSubmit')[0].setAttribute("onclick", "Editrecord('/Employee/Alert')")
    }
    $('#Editform').modal('show');
    event.stopPropagation();//防止事件冒泡
}
function Editrecord(url)
{
    $('#Editform').bootstrapValidator('validate');//验证表单
    if (!$("#Editform").data('bootstrapValidator').isValid())//如果有错则不提交
        return;
    var Department = document.getElementById("Department").value;
    var date = Getjosn();
    $.ajax({
        url: url,
        type: 'GET',
        data: {
            date: date,
            Department: Department
        },
        success: function (resultData) {
            $('#Editform ').modal('hide');//隐藏模态框
            $(table_id).bootstrapTable('refresh', { url: '/Employee/GetEmployee' });
            Messageshow(resultData);
        }
    });
}
function Getjosn() {
    var date = new Object();
    var type;
    $("input").each(function () {
        switch (this.id) {
            case "Name": date.e_Name = this.value; break;
            case "ID": date.e_ID = this.value; break;
            case "PhoneNumber": date.e_Mobile = this.value; break;
        }
        date.e_Gender = document.getElementById("Sex").value;

    })
    return JSON.stringify(date);
}
function Rowdel(id)
{
    var del_id = id + ",";
    Delete(del_id);
    event.stopPropagation();//防止事件冒泡
}
function GetType()
{
    $.ajax({
        url: "/Common/GetAllDepartment",
        type: "get",
        success: function (resultData) {
            for (var i = 0; i < resultData.total; i++) {
                document.getElementById("Department").options.add(new Option(resultData.rows[i].d_Name, resultData.rows[i].d_Name));
                dic[resultData.rows[i].d_ID] = resultData.rows[i].d_Name;
            }
        }
    });
}
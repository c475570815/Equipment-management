var table_id = '#table';
var over_ra_ID;
$(document).ready(
    function () {
        $(table_id).bootstrapTable({
            url: '/Maintenance/GetMaintenanceForm',
            method: 'get',
            checkboxHeader: false,//隐藏全选
            uniqueId: "ra_ID",      //每一行的唯一标识，一般为主键列
            showRefresh: true, //是否显示刷新按钮
            pagination: true,     //是否显示分页（*）
            sidePagination: "server",   //分页方式：client客户端分页，server服务端分页（*）
            pageSize: 5,      //每页的记录行数（*）
            pageList: [10, 25, 50, 100],  //可供选择的每页的行数（*）
            queryParamsType: '',// 设置为 ''  在这种情况下传给服务器的参数为：pageSize,pageNumber// queryParams: oTableInit.queryParams,//传递参数（*）
            queryParams: function (params) {
                return params;
            },//查询方法
            columns: [
                    {
                        field: 'ra_ID',
                        title: '申请编号'
                    },
                    {
                        sortable: true,
                        field: 'StartTime',
                        title: '开始维修时间'
                    },
                    {
                        field: 'ra_Resion',
                        title: '故障'
                    },
                    {
                        field: 'name',
                        title: '维修人员'
                    },
                    {
                        field: 'over',
                        title: '是否完成'
                    },
                    {
                        field: 'file',
                        title: '操作',
                        formatter: function (value, row, index) {
                            if (row.ra_ID == undefined)
                                return "--";
                            var inf = '<button type="button" class="btn btn-info" id="information"  onclick="Information(' + row.ra_ID+ ')">查看详情</button>';
                            if (row.over == "未完成")
                                inf = '<button type="button" class="btn btn-info" id="information"  onclick="Over(' + row.ra_ID+ ')">完成维修</button>'
                            return inf;
                        }
                    }
            ]
        });
        $('#infModal').on('hidden.bs.modal', function () {
            $("#date>h4").each(function(i){
                this.innerText = "--";
            });
        })
        $('#load').hide();
    }
);
function Information(ra_id)
{
    console.log($('#load'));
    $('#load').show();
    $.ajax({
        url: "/Maintenance/Maintenanceinf",
        type: 'GET',
        data: { ra_id: ra_id },
        success: function (resultData) {
            resultData=JSON.parse(resultData)
            document.getElementById('Mb_applyid').innerText = "申请编号:" + resultData.ra_ID;
            document.getElementById('Mb_apply_people').innerText = "申请人:"+ resultData.ra_people;
            document.getElementById('Mb_maintain_id').innerText = "设备编号:" + resultData.ra_eqid;
            document.getElementById('Mb_startapply_time').innerText = "开始申请时间:" + data_string(resultData.ra_time);
            if (resultData.ra_pass == -1)
            {
                resultData.ra_pass = "已拒绝";
                document.getElementById('Mb_pass').innerText = "是否通过审批:" + resultData.ra_pass;
                $('#load').hide();
                $('#infModal').modal('show');
                return;
            }
            resultData.ra_pass ==1? resultData.ra_pass = "已通过" : resultData.ra_pass = "未通过";
            document.getElementById('Mb_pass').innerText = "是否通过审批:" + resultData.ra_pass;
            document.getElementById('Mb_pass_time').innerText = "通过时间:" + data_string(resultData.ra_paas_time);
            resultData.ra_AllocationPerson ? resultData.ra_AllocationPerson = "已派人" :resultData.ra_AllocationPerson = "未派人";
            document.getElementById('Mb_allocationPerson').innerText = "是否派人:" + resultData.ra_AllocationPerson;
            document.getElementById('Mb_end_time').innerText = "维修结束时间:" + data_string(resultData.rd_endtime);
            resultData.e_people==null?resultData.e_people="--":1;
            document.getElementById('Mb_name').innerText = "维修人员:" + resultData.re_people;
            resultData.ra_advice == null ? resultData.ra_advice = "--" : 1;
            document.getElementById('Mb_advice').innerText = "反馈信息:" + resultData.ra_advice;
            $('#load').hide();
            $('#infModal').modal('show');
        },
        error: function()
        {
            $('#load').hide();
            Messageshow("加载失败");
            }

    });
}
function Over(ra_id)
{
    over_ra_ID = ra_id;
    $('#backModal').modal('show');
}
function Over_back() {
    $.ajax({
        url: "/Maintenance/AddMaintenanceFeedback",
        type: "get",
        data: { rd_Advice:$("#rd_Advice").val(),ra_id:over_ra_ID },
        success: function (responseData) {
            $('#backModal').modal('hide');//隐藏模态框
            $(table_id).bootstrapTable('refresh', { url: 'GetMaintenanceForm' });
            Messageshow(responseData);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            Messageshow("提交回馈信息失败! ");
        }
    })
}
function Messageshow(date) {
    var a = document.getElementById("Message");
    a.innerText = date;
    $('#myModal').modal('show');

}
function data_string(str) {
    if (str == null)
        return "--";
    var d = eval('new ' + str.substr(1, str.length - 2));
    var ar_date = [d.getFullYear(), d.getMonth() + 1, d.getDate()];
    for (var i = 0; i < ar_date.length; i++) ar_date[i] = dFormat(ar_date[i]);
    return ar_date.join('-');
    function dFormat(i) { return i < 10 ? "0" + i.toString() : i; }
}
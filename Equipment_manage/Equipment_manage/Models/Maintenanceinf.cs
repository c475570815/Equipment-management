using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Equipment_manage.Models
{
    public class Maintenanceinf
    {

        public int ra_ID;//申请编号
        public string ra_people;//申请人
        public int ra_eqid;//设备编号
        public Nullable<System.DateTime> ra_time;//开始申请时间
        public Nullable<int> ra_pass;//是否通过审批
        public Nullable<int> ra_AllocationPerson;//是否派人
        public DateTime? ra_paas_time;//通过时间
        public string  re_people ;//维修人员
        public string ra_advice ;//反馈信息
        public DateTime? rd_endtime;//维修结束时间
    }
}
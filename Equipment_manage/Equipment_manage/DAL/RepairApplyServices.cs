using Equipment_manage.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Equipment_manage.DAL
{
    public class RepairApplyServices
    {
        private static Equipment DB = new Equipment();
        private static DbSet<RepairApply> table = DB.RepairApply;
        private static IQueryable<RepairApply> InitialDate = null;
        public static void AddRecorde(RepairApply recorde)
        {
            table.Add(recorde);
            DB.SaveChanges();
        }
        public static IQueryable<RepairApply> GetWhere()
        {
            var userdata = (System.Web.HttpContext.Current.User as Equipment_manage.Models.Authen.MyUserDataPrincipal).userdata;
            return table.Where(q => q.ra_Approval==0&&q.d_ID==userdata.d_ID);//制定设备编号
        }
        public static IQueryable<RepairApply> Sort(IQueryable<RepairApply> date, string sortOrder, string sortName)
        {
            InitialDate = date.OrderBy(i => i.ra_Time);
            if (sortName != "" && InitialDate != null)
            {
                if (sortOrder != "desc")
                    switch (sortName)
                    {
                        case "ra_Time": InitialDate = InitialDate.OrderBy(P => P.ra_Time); break;
                        default: break;
                    }
                else
                    switch (sortName)
                    {
                        case "ra_Time": InitialDate = InitialDate.OrderByDescending(P => P.ra_Time); break;
                        default: break;
                    }
            }
            return InitialDate;
        }
        public static IQueryable<RepairApply> GetAppointWhere()
        {
            return table.Where(p => p.ra_AllocationPerson == 0 && p.ra_Approval == 1).OrderBy(i => i.ra_ID);
        }
        public static RepairApply IdGetRecord(int id)
        {
            return table.FirstOrDefault(p => p.ra_ID == id);
        }
        public static int PassApply(int id)
        {
            var update = IdGetRecord(id);
            update.ra_Approval = 1;//更新为通过申请
            update.ra_ApprovalTime = DateTime.Now;
            RepairDetailsServices.AddRecorde(new RepairDetails()
            {
                ra_ID = id,
                ra_Resion = update.ra_Resion,
                rd_StartTime = DateTime.Now,
                di_EqId = update.di_EqId,
            });
            return DB.SaveChanges();
        }
        public static int RefuseApply(int id)
        {
            var update = RepairApplyServices.IdGetRecord(id);
            update.ra_Approval = -1;
            update.ra_Status = "已完成";
            var Irecord = DecivesInfoServices.IdGetRecorde(update.di_EqId);
            Irecord.di_Status = "可用";
            RepairDetailsServices.AddRecorde(new RepairDetails()
            {
                ra_ID = id,
                ra_Resion = update.ra_Resion,
                rd_StartTime = DateTime.Now,
                di_EqId = update.di_EqId,
            });
            return DB.SaveChanges();
        }
        public static string IdGetStatus(int id)
        {
            return  table.FirstOrDefault(p => p.ra_ID == id).ra_Status;
        }
        public static bool OverStatus(int id)
        {
            table.FirstOrDefault(p => p.ra_ID == id).ra_Status = "完成";
            return DB.SaveChanges() == 1 ? true : false;
        }
        public static void AlertApplyStatus(int id)
        { 
          var record=table.FirstOrDefault(p => p.ra_ID == id);//修改申请表状态
          record.ra_AllocationPerson = 1;
          record.ra_ApprovalTime = DateTime.Now;
          DB.SaveChanges();
        }
    }
}
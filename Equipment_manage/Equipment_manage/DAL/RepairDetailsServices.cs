using Equipment_manage.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Equipment_manage.DAL
{
    public class RepairDetailsServices
    {
        private static Equipment DB = new Equipment();
        private static DbSet<RepairDetails> table = DB.RepairDetails;
        private static IQueryable<RepairDetails> InitialDate = null;


        public static IQueryable<RepairDetails> GetWhere()
        {
             InitialDate=table.OrderBy(i => i.rd_StartTime);
             return InitialDate;
        }
        public static IQueryable<RepairDetails> Sort(IQueryable<RepairDetails> date, string sortOrder, string sortName)
        {
            if (sortName != "" && InitialDate != null)
            {
                if (sortOrder != "desc")
                    switch (sortName)
                    {
                        case "StartTime": InitialDate = InitialDate.OrderBy(P => P.rd_StartTime); break;
                        default: break;
                    }
                else
                    switch (sortName)
                    {
                        case "StartTime": InitialDate = InitialDate.OrderByDescending(P => P.rd_StartTime); break;
                        default: break;
                    }
            }
            return InitialDate;
        }

        public static void AddRecorde(RepairDetails recorde)
        {
            table.Add(recorde);
            DB.SaveChanges();
        }
        public static RepairDetails IdGetRecord(int id)
        {
            return  table.FirstOrDefault(p => p.rd_ID == id);
        
        }
        public static bool OverBack(int id,string advice)
        {
            var record = table.FirstOrDefault(p => p.ra_ID == id);
            record.rd_Advice = advice;
            record.rd_EndTime = DateTime.Now;
            return   DB.SaveChanges() == 1 ? true : false;
        }
        public static RepairDetails Ra_idGetRecord(int ra_id)
        {
            return table.FirstOrDefault(p => p.ra_ID == ra_id);
        }
        public static int GetWorkNumber(int id)
        {
            return table.Where(p => p.e_ID == id).Count();
        }
        public static void AlertAppointPeople(int e_id,int id)
        {
            table.FirstOrDefault(p => p.ra_ID == id).e_ID = e_id;//修改工单表状态
            DB.SaveChanges();
        }
    }
}
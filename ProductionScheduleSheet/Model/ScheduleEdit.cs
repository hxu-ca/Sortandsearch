using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ProductionScheduleSheet.Models
{
    public class ScheduleEdit
    {
        public List<ScheduleItem> Items { get; set; }
        public ScheduleEdit()
        {
            Items = new List<ScheduleItem>();
        }
        public ScheduleItem GetItem(string jobNum)
        {
            return Items.Where(x => x.JobNum == jobNum).FirstOrDefault();
        }
    }

    public class ScheduleItem
    {
        public ScheduleItem() { }

        [XmlAttribute]
        public string JobNum { get; set; }
        public Nullable<System.DateTime> WarningDate { get; set; }
        public string ReviewState { get; set; }
        public string Documentation { get; set; }
        public string BOMState { get; set; }
        public string EprintState { get; set; }
        public string EprintOwner { get; set; }
        public string ProgramState { get; set; }
        public string ProgramOwner { get; set; }
    }
}
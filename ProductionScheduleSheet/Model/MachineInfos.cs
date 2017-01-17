using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using MachineInfo;

namespace ProductionScheduleSheet.Model
{
    public class MachineInfos
    {
        public List<MachineInfo.MachineInfo> Items { get; set; }
        public MachineInfos()
        {
            Items = new List<MachineInfo.MachineInfo>();
        }
    }
}
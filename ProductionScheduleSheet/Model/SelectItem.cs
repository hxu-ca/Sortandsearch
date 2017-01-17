using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ProductionScheduleSheet.Models
{
    public class BaseSelection
    {
        public List<SelectItem> SelectItems { get; set; }
        public BaseSelection()
        {
            SelectItems = new List<SelectItem>();
        }
    }
    public class SelectItem
    {
        [XmlAttribute]
        public int id { get; set; }
        public string Value { get; set; }
    }
}
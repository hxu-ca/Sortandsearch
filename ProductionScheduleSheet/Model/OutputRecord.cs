using System;
using System.Linq;

namespace ProductionScheduleSheet.Models
{
    public class OutputRecord
    {
        // from prodct DB
        public Nullable<System.DateTime> ShipDate { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string MachinePartNum { get; set; }
        public string MachineStatus { get; set; }
        public string SerialNumber { get; set; }
        public string LineID { get; set; }
        public string JobNum { get; set; }
        // from xml
        public Nullable<System.DateTime> WarningDate { get; set; }
        public string ReviewState { get; set; }
        public string Documentation { get; set; }
        public string BOMState { get; set; }
        public string EprintState { get; set; }
        public string EprintOwner { get; set; }
        public string ProgramState { get; set; }
        public string ProgramOwner { get; set; }
        public string RevId { get; set; }
        public string DocId { get; set; }
        public string EprId { get; set; }
        public string BomId { get; set; }
        public string PrgId { get; set; }
        public string overDt  { get; set; }

    public OutputRecord() { }
        public OutputRecord(MachineInfo.MachineInfo dbInfo)
        {
            ShipDate = dbInfo.ShipDate;
            CustomerID = dbInfo.CustomerID;
            CustomerName = dbInfo.CustomerName;
            MachinePartNum = dbInfo.MachinePartNum;
            MachineStatus = dbInfo.MachineStatus;
            SerialNumber = dbInfo.SerialNumber;
            LineID = dbInfo.LineID;
            JobNum = dbInfo.JobNum;
            if (ShipDate != null)
                WarningDate = ((DateTime)ShipDate).AddDays(-12 * 7);
        }

        public OutputRecord(MachineInfo.MachineInfo dbInfo, ScheduleItem xmlInfo)
        {
            ShipDate = dbInfo.ShipDate;
            CustomerID = dbInfo.CustomerID;
            CustomerName = dbInfo.CustomerName;
            MachinePartNum = dbInfo.MachinePartNum;
            MachineStatus = dbInfo.MachineStatus;
            SerialNumber = dbInfo.SerialNumber;
            LineID = dbInfo.LineID;
            JobNum = dbInfo.JobNum;

            if (xmlInfo.WarningDate != null)
                WarningDate = xmlInfo.WarningDate;
            else if (ShipDate != null)
                WarningDate = ((DateTime)ShipDate).AddDays(-12 * 7);

            ReviewState = xmlInfo.ReviewState;
            Documentation = xmlInfo.Documentation;
            BOMState = xmlInfo.BOMState;
            EprintState = xmlInfo.EprintState;
            EprintOwner = xmlInfo.EprintOwner;
            ProgramState = xmlInfo.ProgramState;
            ProgramOwner = xmlInfo.ProgramOwner;

            var today = DateTime.Today;
            if (ShipDate < today.AddDays(12 * 7))
            {
                overDt = "1";
                //colWarning = "{'background-color':'#fce8b2'}";
            }
        }

        public OutputRecord(MachineInfo.MachineInfo dbInfo, ScheduleItem xmlInfo, IQueryable<SelectItem> bom, IQueryable<SelectItem> doc, IQueryable<SelectItem> rev, IQueryable<SelectItem> prg, IQueryable<SelectItem> epr) : this(dbInfo, xmlInfo)
        {
            if (!string.IsNullOrEmpty(xmlInfo.ReviewState))
            {
                RevId = rev.Where(r => r.Value == xmlInfo.ReviewState).Select(r => r.id).FirstOrDefault().ToString();
                //switch (Convert.ToInt32(RevId))
                //{
                //    case 0:
                //        colRevState = "{'background-color':'#f4c7c3'}";
                //        break;
                //    case 1:
                //        colRevState = "{'background-color':'#b7e1cd'}";
                //        break;
                //    case 2:
                //        colRevState = "{'background-color':'#fce8b2'}";
                //        break;
                //}
            }
            if (!string.IsNullOrEmpty(xmlInfo.Documentation))
            {
                DocId = doc.Where(r => r.Value == xmlInfo.Documentation).Select(r => r.id).FirstOrDefault().ToString();
                //switch (Convert.ToInt32(DocId))
                //{
                //    case 0:
                //        colDocState = "{'background-color':'#f4c7c3'}";
                //        break;
                //    case 1:
                //    case 2:
                //    case 3:
                //        colDocState = "{'background-color':'#fce8b2'}";
                //        break;
                //    case 4:
                //        colDocState = "{'background-color':'#b7e1cd'}";
                //        break;
                //}

            }
            if (!string.IsNullOrEmpty(xmlInfo.EprintState))
            {
                EprId = epr.Where(r => r.Value == xmlInfo.EprintState).Select(r => r.id).FirstOrDefault().ToString();
                //switch (Convert.ToInt32(EprId))
                //{
                //    case 0:
                //        colEprState = "{'background-color':'#f4c7c3'}";
                //        break;
                //    case 1:
                //    case 2:
                //    case 3:
                //        colEprState = "{'background-color':'#fce8b2'}";
                //        break;
                //    case 4:
                //        colEprState = "{'background-color':'#b7e1cd'}";
                //        break;
                //}
            }
            if (!string.IsNullOrEmpty(xmlInfo.ProgramState))
            {
                PrgId = prg.Where(r => r.Value == xmlInfo.ProgramState).Select(r => r.id).FirstOrDefault().ToString();
                //switch (Convert.ToInt32(PrgId))
                //{
                //    case 0:
                //        colPrgState = "{'background-color':'#f4c7c3'}";
                //        break;
                //    case 1:
                //    case 2:
                //    case 3:
                //    case 4:
                //    case 5:
                //    case 6:
                //        colPrgState = "{'background-color':'#fce8b2'}";
                //        break;
                //    case 7:
                //        colPrgState = "{'background-color':'#b7e1cd'}";
                //        break;
                //}
            }
            if (!string.IsNullOrEmpty(xmlInfo.BOMState))
            {
                BomId = bom.Where(r => r.Value == xmlInfo.BOMState).Select(r => r.id).FirstOrDefault().ToString();
                //switch (Convert.ToInt32(BomId))
                //{
                //    case 0:
                //        colBOMState = "{'background-color':'#f4c7c3'}";
                //        break;
                //    case 1:
                //    case 2:
                //        colBOMState = "{'background-color':'#fce8b2'}";
                //        break;
                //    case 3:
                //        colBOMState = "{'background-color':'#b7e1cd'}";
                //        break;
                //}
            }
        }

        public void Update(OutputRecord r)
        {
            WarningDate = r.WarningDate;
            ReviewState = r.ReviewState;
            Documentation = r.Documentation;
            BOMState = r.BOMState;
            EprintState = r.EprintState;
            EprintOwner = r.EprintOwner;
            ProgramState = r.ProgramState;
            ProgramOwner = r.ProgramOwner;
        }
    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MachineInfo
{
    using System;
    using System.Collections.Generic;
    
    public partial class MachineInfo
    {
        public string MachinePartNum { get; set; }
        public string SerialNumber { get; set; }
        public string MachineStatus { get; set; }
        public string JobNum { get; set; }
        public string Description { get; set; }
        public string ProgramNum { get; set; }
        public string ElecPrint { get; set; }
        public int SinglePHVolt { get; set; }
        public decimal SinglePHAmp { get; set; }
        public int ThreePHVolt { get; set; }
        public decimal ThreePHAmp { get; set; }
        public string Notes { get; set; }
        public Nullable<System.DateTime> WarrantyExpDate { get; set; }
        public Nullable<int> OrderNum { get; set; }
        public Nullable<int> OrderLine { get; set; }
        public Nullable<decimal> ProdQty { get; set; }
        public Nullable<System.DateTime> ShipDate { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string ShipToCustomerID { get; set; }
        public string ShipToCustomerName { get; set; }
        public string AirPrint { get; set; }
        public string CFM { get; set; }
        public string LineID { get; set; }
        public string OrderComment { get; set; }
        public Nullable<int> QuoteNum { get; set; }
        public string OrderHeadComment { get; set; }
    }
}
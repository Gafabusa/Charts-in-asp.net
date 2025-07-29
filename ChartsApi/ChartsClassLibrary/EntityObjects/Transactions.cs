using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartsClassLibrary.EntityObjects
{
    public class ReceivedTransactionDTO
    {
        public long TranId { get; set; }
        public string TransNo { get; set; }
        public string CustomerRef { get; set; }
        public string CustomerName { get; set; }
        public string CustomerType { get; set; }
        public string CustomerTel { get; set; }
        public string Area { get; set; }
        public string Tin { get; set; }
        public decimal TranAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime RecordDate { get; set; }
        public string TranType { get; set; }
        public string PaymentType { get; set; }
        public string VendorTranId { get; set; }
        public string ReceiptNo { get; set; }
        public string TranNarration { get; set; }
        public bool SmsSent { get; set; }
        public string VendorCode { get; set; }
        public string Teller { get; set; }
        public bool? Reversal { get; set; }
        public bool? Cancelled { get; set; }
        public bool? Offline { get; set; }
        public string UtilityCode { get; set; }
        public string UtilityTranRef { get; set; }
        public bool SentToUtility { get; set; }
        public string RegionCode { get; set; }
        public string DistrictCode { get; set; }
        public string VendorToken { get; set; }
        public bool? ReconFileProcessed { get; set; }
        public string Status { get; set; }
        public int? SentToVendor { get; set; }
        public DateTime? UtilitySentDate { get; set; }
        public DateTime? QueueTime { get; set; }
        public string Reason { get; set; }
    }

}

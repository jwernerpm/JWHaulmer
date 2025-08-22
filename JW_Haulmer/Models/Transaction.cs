using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JW_Haulmer.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string MerchantId { get; set; }
        public string PanMasked { get; set; }
        public string Expiry { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public string IsoCode { get; set; }
        public string AuthorizationCode { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

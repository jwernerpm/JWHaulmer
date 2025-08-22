using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JW_Haulmer.DTOs
{
    public class TransactionRequest
    {
        public string Pan { get; set; }
        public string Expiry { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Cvv { get; set; }
        public string MerchantId { get; set; }
    }
}

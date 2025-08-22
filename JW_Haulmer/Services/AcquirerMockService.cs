using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JW_Haulmer.Services
{
    public class AcquirerMockService
    {
        private static readonly string[] IsoCodes = { "00", "05", "51", "91", "87" };
        private readonly Random _rand = new Random();

        public (string isoCode, string authCode, string status) Authorize(decimal amount, string pan, string cvv,string expiry)
        {
            //prueba controlada para obtener transacciones aprobadas
            string iso = (!string.IsNullOrEmpty(pan) && pan.EndsWith("1234"))
            ? "00"
            : IsoCodes[_rand.Next(IsoCodes.Length)];

            string authCode = iso == "00" ? _rand.Next(100000, 999999).ToString() : null;
            string status = iso == "00" ? "Approved" : "Declined";

            return (iso, authCode, status);
        }
    }
}

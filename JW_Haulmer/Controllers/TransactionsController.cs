using Microsoft.AspNetCore.Mvc;
using JW_Haulmer.DTOs;
using JW_Haulmer.Models;
using JW_Haulmer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System;

namespace JW_Haulmer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly AcquirerMockService _acquirer;

        public TransactionsController(IConfiguration configuration, AcquirerMockService acquirer)
        {
            _connectionString = configuration.GetConnectionString("CardTransactionDb");
            _acquirer = acquirer;
        }

        [HttpPost]
        public async Task<IActionResult> PostTransaction([FromBody] TransactionRequest request)
        {
           // valida el largo del numero de la tarjeda
            if (string.IsNullOrWhiteSpace(request.Pan) || request.Pan.Length < 12 || request.Pan.Length > 19)
                return BadRequest("Invalid PAN");

            //valida formato de expiracion, Falta validar si esta o no expirada.
            if (string.IsNullOrWhiteSpace(request.Expiry) || !Regex.IsMatch(request.Expiry, @"^(0[1-9]|1[0-2])\/\d{2}$"))
            {
                return BadRequest("Invalid Expiry format (MM/YY)");
            }

            if (request.Amount <= 0)
                return BadRequest("Invalid amount");

            if (string.IsNullOrWhiteSpace(request.Currency) || request.Currency.Length != 3)
                return BadRequest("Invalid currency");

            // autorización mock ISO
            var (iso, authCode, status) = _acquirer.Authorize(request.Amount);
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                
                conn.Open();
               
                
                string sql = @"
                    INSERT INTO Transactions
                        (MerchantId, PanMasked, Expiry, Amount, Currency, Status, IsoCode, AuthorizationCode)
                    OUTPUT INSERTED.TransactionId
                    VALUES
                        (@MerchantId, @PanMasked, @Expiry, @Amount, @Currency, @Status, @IsoCode, @AuthorizationCode);
                ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                   // cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MerchantId", request.MerchantId);
                    cmd.Parameters.AddWithValue("@PanMasked", MaskPan(request.Pan));
                    cmd.Parameters.AddWithValue("@Expiry", request.Expiry);
                    cmd.Parameters.AddWithValue("@Amount", request.Amount);
                    cmd.Parameters.AddWithValue("@Currency", request.Currency);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@IsoCode", iso);
                    cmd.Parameters.AddWithValue("@AuthorizationCode", (object)authCode ?? DBNull.Value);

                    //int transactionId = (int)await cmd.ExecuteScalarAsync();
                    int transactionId = (int)cmd.ExecuteScalar();

                    return Ok(new
                    {
                        TransactionId = transactionId,
                        Status = status,
                        IsoCode = iso,
                        AuthorizationCode = authCode
                    });
                }
            }
        }

        private string MaskPan(string pan)
        {
            if (pan.Length < 10) return pan;
            return $"{pan.Substring(0, 6)}******{pan.Substring(pan.Length - 4)}";
        }
    }
}

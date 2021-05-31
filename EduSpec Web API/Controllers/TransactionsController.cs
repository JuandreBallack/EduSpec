using EduSpecWebAPI.Code;
using EduSpecWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EduSpecWebAPI.Controllers
{
    public class TransactionsController : ApiController
    {
        //public static IList<Transactions> Transactions = new List<Transactions>();
        
        [HttpPost]
        //[BasicAuthentication(RequireSsl = false)]
        [Route("api/Transactions/{InstID}")]
        public void Post(Transactions[] Transactions, int InstID)
        {
            DataTable transactions = BulkCopy.SASAMSTransactions();
            foreach (var Transaction in Transactions)
            {
                transactions.Rows.Add(
                    InstID,
                    Transaction.SubAccNumber,
                    Transaction.Date,
                    Transaction.Description,
                    Transaction.Amount);
            }
            BulkCopy.BulkCopyToSQLServer("SASAMS_Sync_Transactions", transactions, BulkCopy.SASAMSTransactionsFields());
        }

        // PUT api/<controller>/5
        [HttpPut]
        [Route("api/Transactions/ClearTransactionTable/{InstID}")]
        public void Put(int InstID)
        {
            using (var Context = new EduSpecWebAPIDataContext())
            {
                Context.Set_WebAPI_TransactionsClear(InstID);                
            }
        }

        // DELETE: api/Learners/5
        public void Delete(int id)
        {
        }
    }
}

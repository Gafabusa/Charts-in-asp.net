using ChartsClassLibrary.ControlObjects;
using ChartsClassLibrary.EntityObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace ChartsApi
{
    
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    
    public class ChartsApi : WebService
    {
        private readonly BusinessLogic businessLogic;
        public ChartsApi()
        {
            businessLogic = new BusinessLogic();
        }
        //inserting the csv file data in the db
        [WebMethod]
        public void InsertReceivedTransactionFromAPI(ReceivedTransactionDTO data)
        {
            businessLogic.InsertReceivedTransaction(data);
        }
        //fetch transaction counts by RecordDate
        [WebMethod]
        public DataTable GetTransactionCountsByRecordDate()
        {
            return businessLogic.GetTransactionCountsByRecordDate();
        }
        // Filter transactions based on criteria
        [WebMethod]
        public DataTable FilterReceivedTransactions(
       string transNo,
       string customerRef,
       string customerName,
       string customerType,
       string customerTel,
       string vendorTranId,
       string receiptNo,
       string vendorCode,
       string teller)
        {
            return businessLogic.FilterReceivedTransactions(
                transNo,
                customerRef,
                customerName,
                customerType,
                customerTel,
                vendorTranId,
                receiptNo,
                vendorCode,
                teller);
        }
        // Fetch all transactions
        [WebMethod]
        public DataTable GetAllReceivedTransactions()
        {
            return businessLogic.GetAllReceivedTransactions();
        }


    }
}

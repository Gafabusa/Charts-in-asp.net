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
        //authenticate admin
        [WebMethod]
        public DataTable AdminLogin(string email, string password)
        {
            return businessLogic.AdminLogin(email, password);
        }
        //create users
        [WebMethod]
        public DataTable CreateUser(string fullName, string email, string hashedPassword, int roleId)
        {
            return businessLogic.CreateUser(fullName, email, hashedPassword, roleId);
        }
        //get all roles
        [WebMethod]
        public DataTable GetAllRoles()
        {
            return businessLogic.GetAllRoles();
        }
        //get all users
        [WebMethod]
        public DataTable GetAllNonAdminUsers()
        {
            return businessLogic.GetAllNonAdminUsers();
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
        public DataTable FilterReceivedTransactions(string transNo,string customerRef,string customerName,string customerType,
       string customerTel,string vendorTranId,string receiptNo,string vendorCode,string teller)
        {
            return businessLogic.FilterReceivedTransactions(transNo,customerRef,customerName,customerType,customerTel,
                vendorTranId,receiptNo,vendorCode,teller);
        }
        // Fetch all transactions
        [WebMethod]
        public DataTable GetAllReceivedTransactions()
        {
            return businessLogic.GetAllReceivedTransactions();
        }
        //getting transaction count
        [WebMethod]
        public DataTable GetTotalTransactionCount()
        {
            return businessLogic.GetTotalTransactionCount();
        }
        //getting all transactions by vendor code
        [WebMethod]
        public DataTable GetAllReceivedTransactionsByVendor(string vendorCode)
        {
            return businessLogic.GetAllReceivedTransactionsByVendor(vendorCode);
        }
        // Fetch transaction counts by RecordDate and Vendor
        [WebMethod]
        public DataTable GetTransactionCountsByRecordDateAndVendor(string vendorCode)
        {
            return businessLogic.GetTransactionCountsByRecordDateAndVendor(vendorCode);
        }
        //get ordered transactions 
        [WebMethod]
        public DataTable GetAllReceivedTransactionsOrdered()
        {
            return businessLogic.GetAllReceivedTransactionsOrdered();
        }
        //Fetch transaction counts by VendorCode
        [WebMethod]
        public DataTable GetTransactionCountsByVendorCode()
        {
            return businessLogic.GetTransactionCountsByVendorCode();
        }
        //get most recent transactions
        [WebMethod]
        public DataTable GetMostRecentTransactions()
        {
            return businessLogic.GetMostRecentTransactions();
        }
        // Fetch customer type distribution
        [WebMethod]
        public System.Data.DataTable GetCustomerTypeDistribution()
        {
            return businessLogic.GetCustomerTypeDistribution();
        }
        //get status distribution
        [WebMethod]
        public DataTable GetStatusDistribution()
        {
            return businessLogic.GetStatusDistribution();
        }
        //hourly transactions
        [WebMethod]
        public DataTable GetTransactionsPerHour()
        {
            return businessLogic.GetTransactionsPerHour();
        }
    }
}

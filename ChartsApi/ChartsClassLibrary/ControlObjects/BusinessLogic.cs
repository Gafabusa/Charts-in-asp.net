using ChartsClassLibrary.EntityObjects;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ChartsClassLibrary.ControlObjects
{
    public class BusinessLogic
    {
        private readonly DatabaseHelper dbHelper;

        public BusinessLogic()
        {
            dbHelper = new DatabaseHelper();
        }

        // Inserting the CSV file data into the database
        public void InsertReceivedTransaction(ReceivedTransactionDTO data)
        {
            object[] parameters = new object[]
            {
                data.TransNo,
                data.CustomerRef,
                data.CustomerName,
                data.CustomerType,
                data.CustomerTel,
                data.Area,
                data.Tin,
                data.TranAmount,
                data.PaymentDate,
                data.RecordDate,
                data.TranType,
                data.PaymentType,
                data.VendorTranId,
                data.ReceiptNo,
                data.TranNarration,
                data.SmsSent,
                data.VendorCode,
                data.Teller,
                data.Reversal ?? (object)DBNull.Value,
                data.Cancelled ?? (object)DBNull.Value,
                data.Offline ?? (object)DBNull.Value,
                data.UtilityCode,
                data.UtilityTranRef,
                data.SentToUtility,
                data.RegionCode,
                data.DistrictCode,
                data.VendorToken,
                data.ReconFileProcessed ?? (object)DBNull.Value,
                data.Status,
                data.SentToVendor ?? (object)DBNull.Value,
                data.UtilitySentDate ?? (object)DBNull.Value,
                data.QueueTime ?? (object)DBNull.Value,
                data.Reason
            };

            dbHelper.ExecuteNonQuery("sp_InsertReceivedTransaction", parameters);
        }
        // Fetch transaction counts by RecordDate
        public DataTable GetTransactionCountsByRecordDate()
        {
            return dbHelper.ExecuteDataTable("sp_GetTransactionCountsByRecordDate");
        }
        // Filter transactions based on criteria
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
            object[] parameters = new object[]
            {
            string.IsNullOrWhiteSpace(transNo) ? (object)DBNull.Value : transNo,
            string.IsNullOrWhiteSpace(customerRef) ? (object)DBNull.Value : customerRef,
            string.IsNullOrWhiteSpace(customerName) ? (object)DBNull.Value : customerName,
            string.IsNullOrWhiteSpace(customerType) ? (object)DBNull.Value : customerType,
            string.IsNullOrWhiteSpace(customerTel) ? (object)DBNull.Value : customerTel,
            string.IsNullOrWhiteSpace(vendorTranId) ? (object)DBNull.Value : vendorTranId,
            string.IsNullOrWhiteSpace(receiptNo) ? (object)DBNull.Value : receiptNo,
            string.IsNullOrWhiteSpace(vendorCode) ? (object)DBNull.Value : vendorCode,
            string.IsNullOrWhiteSpace(teller) ? (object)DBNull.Value : teller
            };

            return dbHelper.ExecuteDataTable("sp_FilterReceivedTransactions", parameters);
        }
        // Fetch all transactions
        public DataTable GetAllReceivedTransactions()
        {
            return dbHelper.ExecuteDataTable("sp_GetAllReceivedTransactions");
        }
        //getting transaction count
        public DataTable GetTotalTransactionCount()
        {
            return dbHelper.ExecuteDataTable("GetTransactionCount");
        }
        //getting all transactions by vendor code
        public DataTable GetAllReceivedTransactionsByVendor(string vendorCode)
        {
            object[] parameters = new object[] { vendorCode };
            return dbHelper.ExecuteDataTable("sp_GetAllReceivedTransactionsByVendor", parameters);
        }
        // Fetch transaction counts by RecordDate and Vendor
        public DataTable GetTransactionCountsByRecordDateAndVendor(string vendorCode)
        {
            object[] parameters = new object[] { vendorCode };
            return dbHelper.ExecuteDataTable("sp_GetTransactionCountsByRecordDateAndVendor", parameters);
        }
        //get ordered transactions 
        public DataTable GetAllReceivedTransactionsOrdered()
        {
            return dbHelper.ExecuteDataTable("sp_GetAllReceivedTransactionsOrdered");
        }
        // Fetch transaction counts by VendorCode
        public DataTable GetTransactionCountsByVendorCode()
        {
            return dbHelper.ExecuteDataTable("sp_GetTransactionCountsByVendorCode");
        }
        //get most recent transactions
        public DataTable GetMostRecentTransactions()
        {
            return dbHelper.ExecuteDataTable("sp_GetMostRecentTransactions");
        }
        // Fetch customer type distribution
        public System.Data.DataTable GetCustomerTypeDistribution()
        {
            return dbHelper.ExecuteDataTable("sp_GetCustomerTypeDistribution");
        }
        //get status distribution
        public DataTable GetStatusDistribution()
        {
            return dbHelper.ExecuteDataTable("sp_GetStatusDistribution");
        }
        //hourly transactions
        public DataTable GetTransactionsPerHour()
        {
            return dbHelper.ExecuteDataTable("sp_GetTransactionsPerHour");
        }
    }
}
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;

using System.Threading.Tasks;

namespace ChartsClassLibrary.ControlObjects
{
    public class DatabaseHelper
    {
        private readonly Database db;

        public DatabaseHelper()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            db = factory.Create("Charts");
        }

        public void ExecuteNonQuery(string storedProcedureName, params object[] parameters)
        {
            // Convert all nulls to DBNull
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] == null)
                {
                    parameters[i] = DBNull.Value;
                }
            }

            using (DbCommand command = db.GetStoredProcCommand(storedProcedureName, parameters))
            {
                db.ExecuteNonQuery(command);
            }
        }


        public T ExecuteScalar<T>(string storedProcedureName, params object[] parameters)
        {
            using (DbCommand command = db.GetStoredProcCommand(storedProcedureName, parameters))
            {
                object result = db.ExecuteScalar(command);
                return result == DBNull.Value ? default(T) : (T)Convert.ChangeType(result, typeof(T));
            }
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as a DataTable.
        /// </summary>
        public DataTable ExecuteDataTable(string storedProcedureName, params object[] parameters)
        {
            using (DbCommand command = db.GetStoredProcCommand(storedProcedureName, parameters))
            {
                return db.ExecuteDataSet(command).Tables[0];
            }
        }
    }
}
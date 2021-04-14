using Microsoft.Data.SqlClient;
using ServiceManager.Common.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManager.Common.Helpers
{

    public class EventLogger
    {
        public long AddLogEntry(string serviceType, string status, string error, string functionName)
        {
            long ret2 = 0;
            try
            {
                error = functionName == string.Empty ? error : functionName + " - " + error;
                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@log_service_type", serviceType));
                param.Add(new SqlParameter("@log_status", status));
                param.Add(new SqlParameter("@log_error", error));
                param.Add(new SqlParameter() { ParameterName = "@out", Value = 0, Direction = ParameterDirection.Output });

                ret2 = this.ExecuteNonQuery("system_log_insert", param.ToArray());

                System.Console.ForegroundColor = status == "COMPLETED" ? ConsoleColor.Green : ConsoleColor.White;
                System.Console.WriteLine(status + " " + error);
                System.Console.WriteLine();
                System.Console.ForegroundColor = ConsoleColor.White;

            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error (AddLogEntry): " + ex.Message);

                //error = error + ex.ToString();
                AddLogToFile(serviceType, status, error).Wait();
            }

            return ret2;
        }

        public long AddLogEntry(string serviceType, string status, Exception error, string functionName, string customMessage = "")
        {
            long ret2 = 0;
            string entityError = "";
            string errorMessage = "";
            try
            {
                //if (error is System.Data.Entity.Validation.DbEntityValidationException)
                //{
                //    var exx = (System.Data.Entity.Validation.DbEntityValidationException)error;

                //    //errorMessage += exx.Message;
                //    foreach (var eve in exx.EntityValidationErrors)
                //    {
                //        entityError += string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                //        foreach (var ve in eve.ValidationErrors)
                //        {
                //            entityError += Environment.NewLine;
                //            entityError += string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                //        }
                //    }
                //}

                errorMessage = customMessage + " \t " + entityError + Environment.NewLine + error.ToString();
                errorMessage = functionName == string.Empty ? errorMessage : functionName + " - " + errorMessage;

                /*
                errorMessage = functionName == string.Empty ? errorMessage : functionName + " - " + errorMessage;
                errorMessage += Environment.NewLine + error.StackTrace;
                */

                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@log_service_type", serviceType));
                param.Add(new SqlParameter("@log_status", status));
                param.Add(new SqlParameter("@log_error", errorMessage));
                param.Add(new SqlParameter() { ParameterName = "@out", Value = 0, Direction = ParameterDirection.Output });

                ret2 = this.ExecuteNonQuery("system_log_insert", param.ToArray());

                System.Console.WriteLine(errorMessage);
                System.Console.WriteLine();

            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error (AddLogEntry): " + ex.ToString());

                errorMessage = errorMessage + ex.ToString();
                AddLogToFile(serviceType, status, errorMessage).Wait();
            }

            return ret2;
        }

        private async Task AddLogToFile(string serviceType, string status, string errorMessage)
        {
            try
            {
                // generate file name service wise
                string fileName = string.Format("{0}.txt", DateTime.Now.ToString("yyyy-MM-dd"));

                // get directory where to put files
                string directoryPath = Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Logs");

                // create directory is does not exists
                if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);


                var uniencoding = new UnicodeEncoding();
                byte[] result = uniencoding.GetBytes(string.Format("Date: {0}{1}Service: {2}{3}Status: {4}{5}Error: {6}{7}", DateTime.Now.ToString(), Environment.NewLine, serviceType, Environment.NewLine, status, Environment.NewLine, errorMessage, Environment.NewLine));

                using (FileStream SourceStream = File.Open(Path.Combine(directoryPath, fileName), FileMode.OpenOrCreate))
                {
                    SourceStream.Seek(0, SeekOrigin.End);
                    await SourceStream.WriteAsync(result, 0, result.Length);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error (AddLogToFile): " + ex.ToString());
            }
        }

        #region ----- ExecuteNonQuery -----

        /// <summary>
        /// Executes a stored procedure that does not return a dataTable and returns the
        /// first output parameter.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure to execute</param>
        /// <param name="arrParam">Parameters required by the stored procedure</param>
        /// <returns>First output parameter</returns>
        public int ExecuteNonQuery(string storedProcedureName, params SqlParameter[] arrParam)
        {
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException("storedProcedureName");
            }

            int retVal = 0;
            SqlParameter firstOutputParameter = null;

            using (var conn = new SqlConnection(Config.GetConnectionString(nameof(ServiceManagerContext))))
            {
                conn.Open();

                // Define the command
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcedureName;
                    cmd.CommandTimeout = conn.ConnectionTimeout;

                    // Handle the parameters
                    if (arrParam != null)
                    {
                        foreach (SqlParameter param in arrParam)
                        {
                            cmd.Parameters.Add(param);

                            // Find the first integer out parameter
                            if (firstOutputParameter == null &&
                                    param.Direction == ParameterDirection.Output &&
                                    param.SqlDbType == SqlDbType.Int)
                                firstOutputParameter = param;
                        }
                    }

                    // Execute the stored procedure
                    cmd.ExecuteNonQuery();

                    // Return the first output parameter value
                    if (firstOutputParameter != null)
                        retVal = (int)firstOutputParameter.Value;
                }
            }
            return retVal;
        }

        #endregion
    }
}

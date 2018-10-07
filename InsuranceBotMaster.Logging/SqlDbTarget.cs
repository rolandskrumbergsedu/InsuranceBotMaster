using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using NLog;
using NLog.Common;
using NLog.Targets;

namespace InsuranceBotMaster.Logging
{
    public class SqlDbTarget : TargetWithLayout
    {
        [Required]
        public string ConnectionString { get; set; }

        [Required]
        public string TableName { get; set; }

        public void WriteConversationLog(LogEventInfo logEvent)
        {
            try
            {
                var query = $"INSERT INTO [{TableName}] (ConversationId, Sender, Recipient, Message, LogTimeStamp, FullMessage)";
                query += " VALUES(@ConversationId, @Sender, @Recipient, @Message, @LogTimeStamp, @FullMessage)";

                using (var connection = new SqlConnection(ConnectionString))
                {
                    var cmd = new SqlCommand
                    {
                        Connection = connection,
                        CommandType = CommandType.Text,
                        CommandText = query
                    };

                    cmd.Parameters.Add("@ConversationId", SqlDbType.NVarChar, 50).Value = logEvent.Properties["ConversationId"];
                    cmd.Parameters.Add("@Sender", SqlDbType.NVarChar, 50).Value = logEvent.Properties["Sender"];
                    cmd.Parameters.Add("@Recipient", SqlDbType.NVarChar, 50).Value = logEvent.Properties["Recipient"];
                    cmd.Parameters.Add("@Message", SqlDbType.NVarChar, 4000).Value = logEvent.Properties["Message"];
                    cmd.Parameters.Add("@LogTimeStamp", SqlDbType.DateTime).Value = logEvent.Properties["LogTimeStamp"];
                    cmd.Parameters.Add("@FullMessage", SqlDbType.NVarChar, 4000).Value = logEvent.Properties["FullMessage"];

                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                if (ex is NLogConfigurationException || ex.GetType().IsSubclassOf(typeof(NLogConfigurationException)))
                {
                    throw;
                }

                Trace.TraceError($"NLog.Extensions.AzureSqlDb.Write() error: {ex}. Stacktrace: {ex.StackTrace}");
                InternalLogger.Error($"Error writing to Sql table: {ex}. Stacktrace: {ex.StackTrace}");
            }
        }

        public void WriteGeneralLog(LogEventInfo logEvent)
        {
            try
            {
                var query = $"INSERT INTO [{TableName}] (ConversationId, Sender, Recipient, Message, LogTimeStamp, FullMessage)";
                query += " VALUES(@ConversationId, @Sender, @Recipient, @Message, @LogTimeStamp, @FullMessage)";

                using (var connection = new SqlConnection(ConnectionString))
                {
                    var cmd = new SqlCommand
                    {
                        Connection = connection,
                        CommandType = CommandType.Text,
                        CommandText = query
                    };

                    cmd.Parameters.Add("@ConversationId", SqlDbType.NVarChar, 50).Value = logEvent.Properties["ConversationId"];
                    cmd.Parameters.Add("@Sender", SqlDbType.NVarChar, 50).Value = logEvent.Properties["Sender"];
                    cmd.Parameters.Add("@Recipient", SqlDbType.NVarChar, 50).Value = logEvent.Properties["Recipient"];
                    cmd.Parameters.Add("@Message", SqlDbType.NVarChar, 4000).Value = logEvent.Properties["Message"];
                    cmd.Parameters.Add("@LogTimeStamp", SqlDbType.DateTime).Value = logEvent.Properties["LogTimeStamp"];
                    cmd.Parameters.Add("@FullMessage", SqlDbType.NVarChar, 4000).Value = logEvent.Properties["FullMessage"];

                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                if (ex is NLogConfigurationException || ex.GetType().IsSubclassOf(typeof(NLogConfigurationException)))
                {
                    throw;
                }

                Trace.TraceError($"NLog.Extensions.AzureSqlDb.Write() error: {ex}. Stacktrace: {ex.StackTrace}");
                InternalLogger.Error($"Error writing to Sql table: {ex}. Stacktrace: {ex.StackTrace}");
            }
        }

        public void WriteLuisLog(LogEventInfo logEvent)
        {
            try
            {
                var query = $"INSERT INTO [{TableName}] (ConversationId, Sender, Recipient, Message, LogTimeStamp, FullMessage)";
                query += " VALUES(@ConversationId, @Sender, @Recipient, @Message, @LogTimeStamp, @FullMessage)";

                using (var connection = new SqlConnection(ConnectionString))
                {
                    var cmd = new SqlCommand
                    {
                        Connection = connection,
                        CommandType = CommandType.Text,
                        CommandText = query
                    };

                    cmd.Parameters.Add("@ConversationId", SqlDbType.NVarChar, 50).Value = logEvent.Properties["ConversationId"];
                    cmd.Parameters.Add("@Sender", SqlDbType.NVarChar, 50).Value = logEvent.Properties["Sender"];
                    cmd.Parameters.Add("@Recipient", SqlDbType.NVarChar, 50).Value = logEvent.Properties["Recipient"];
                    cmd.Parameters.Add("@Message", SqlDbType.NVarChar, 4000).Value = logEvent.Properties["Message"];
                    cmd.Parameters.Add("@LogTimeStamp", SqlDbType.DateTime).Value = logEvent.Properties["LogTimeStamp"];
                    cmd.Parameters.Add("@FullMessage", SqlDbType.NVarChar, 4000).Value = logEvent.Properties["FullMessage"];

                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                if (ex is NLogConfigurationException || ex.GetType().IsSubclassOf(typeof(NLogConfigurationException)))
                {
                    throw;
                }

                Trace.TraceError($"NLog.Extensions.AzureSqlDb.Write() error: {ex}. Stacktrace: {ex.StackTrace}");
                InternalLogger.Error($"Error writing to Sql table: {ex}. Stacktrace: {ex.StackTrace}");
            }
        }

        public void WriteQnaLog(LogEventInfo logEvent)
        {
            try
            {
                var query = $"INSERT INTO [{TableName}] (ConversationId, Sender, Recipient, Message, LogTimeStamp, FullMessage)";
                query += " VALUES(@ConversationId, @Sender, @Recipient, @Message, @LogTimeStamp, @FullMessage)";

                using (var connection = new SqlConnection(ConnectionString))
                {
                    var cmd = new SqlCommand
                    {
                        Connection = connection,
                        CommandType = CommandType.Text,
                        CommandText = query
                    };

                    cmd.Parameters.Add("@ConversationId", SqlDbType.NVarChar, 50).Value = logEvent.Properties["ConversationId"];
                    cmd.Parameters.Add("@Sender", SqlDbType.NVarChar, 50).Value = logEvent.Properties["Sender"];
                    cmd.Parameters.Add("@Recipient", SqlDbType.NVarChar, 50).Value = logEvent.Properties["Recipient"];
                    cmd.Parameters.Add("@Message", SqlDbType.NVarChar, 4000).Value = logEvent.Properties["Message"];
                    cmd.Parameters.Add("@LogTimeStamp", SqlDbType.DateTime).Value = logEvent.Properties["LogTimeStamp"];
                    cmd.Parameters.Add("@FullMessage", SqlDbType.NVarChar, 4000).Value = logEvent.Properties["FullMessage"];

                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                if (ex is NLogConfigurationException || ex.GetType().IsSubclassOf(typeof(NLogConfigurationException)))
                {
                    throw;
                }

                Trace.TraceError($"NLog.Extensions.AzureSqlDb.Write() error: {ex}. Stacktrace: {ex.StackTrace}");
                InternalLogger.Error($"Error writing to Sql table: {ex}. Stacktrace: {ex.StackTrace}");
            }
        }

        public void WriteTranslationLog(LogEventInfo logEvent)
        {

        }

        public void WriteAimlLog(LogEventInfo logEvent)
        {

        }
    }
}
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
                var query = $"INSERT INTO [{TableName}] (Message, LogTimeStamp)";
                query += " VALUES(@Message, @LogTimeStamp)";

                using (var connection = new SqlConnection(ConnectionString))
                {
                    var cmd = new SqlCommand
                    {
                        Connection = connection,
                        CommandType = CommandType.Text,
                        CommandText = query
                    };
                    
                    cmd.Parameters.Add("@Message", SqlDbType.NVarChar, 4000).Value = logEvent.Properties["Message"];
                    cmd.Parameters.Add("@LogTimeStamp", SqlDbType.DateTime).Value = logEvent.Properties["LogTimeStamp"];

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
                var query = $"INSERT INTO [{TableName}] (LogTimeStamp, Dialog, ConversationId, To, From, Query, TopScoringIntent, TopScoringIntentScore, TopScoringIntent2, TopScoringIntent2Score, TopScoringIntent3, TopScoringIntent3Score)";
                query += " VALUES(@LogTimeStamp, @Dialog, @ConversationId, @To, @From, @Query, @TopScoringIntent, @TopScoringIntentScore, @TopScoringIntent2, @TopScoringIntent2Score, @TopScoringIntent3, @TopScoringIntent3Score)";

                using (var connection = new SqlConnection(ConnectionString))
                {
                    var cmd = new SqlCommand
                    {
                        Connection = connection,
                        CommandType = CommandType.Text,
                        CommandText = query
                    };

                    cmd.Parameters.Add("@LogTimeStamp", SqlDbType.DateTime).Value = logEvent.Properties["LogTimeStamp"];
                    cmd.Parameters.Add("@Dialog", SqlDbType.NVarChar, 100).Value = logEvent.Properties["Dialog"];
                    cmd.Parameters.Add("@ConversationId", SqlDbType.NVarChar, 50).Value = logEvent.Properties["ConversationId"];
                    cmd.Parameters.Add("@Recipient", SqlDbType.NVarChar, 50).Value = logEvent.Properties["Recipient"];
                    cmd.Parameters.Add("@Sender", SqlDbType.NVarChar, 50).Value = logEvent.Properties["Sender"];
                    cmd.Parameters.Add("@Query", SqlDbType.NVarChar, 4000).Value = logEvent.Properties["Query"];
                    cmd.Parameters.Add("@TopScoringIntent", SqlDbType.NVarChar, 100).Value = logEvent.Properties["TopScoringIntent"];
                    cmd.Parameters.Add("@TopScoringIntentScore", SqlDbType.Decimal).Value = logEvent.Properties["TopScoringIntentScore"];
                    cmd.Parameters.Add("@TopScoringIntent2", SqlDbType.NVarChar, 100).Value = logEvent.Properties["TopScoringIntent2"];
                    cmd.Parameters.Add("@TopScoringIntent2Score", SqlDbType.Decimal).Value = logEvent.Properties["TopScoringIntent2Score"];
                    cmd.Parameters.Add("@TopScoringIntent3", SqlDbType.NVarChar, 100).Value = logEvent.Properties["TopScoringIntent3"];
                    cmd.Parameters.Add("@TopScoringIntent3Score", SqlDbType.Decimal).Value = logEvent.Properties["TopScoringIntent3Score"];

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
                var query = $"INSERT INTO [{TableName}] (LogTimeStamp, ConversationId, To, From, Query, TopScoringAnswer, TopScoringAnswerScore, TopScoringAnswer2, TopScoringAnswer2Score, TopScoringAnswer3, TopScoringAnswer3Score, MissedTreshold, Treshold)";
                query += " VALUES(@LogTimeStamp, @ConversationId, @To, @From, @Query, @TopScoringAnswer, @TopScoringAnswerScore, @TopScoringAnswer2, @TopScoringAnswer2Score, @TopScoringAnswer3, @TopScoringAnswer3Score, @MissedTreshold, @Treshold)";

                using (var connection = new SqlConnection(ConnectionString))
                {
                    var cmd = new SqlCommand
                    {
                        Connection = connection,
                        CommandType = CommandType.Text,
                        CommandText = query
                    };

                    cmd.Parameters.Add("@LogTimeStamp", SqlDbType.DateTime).Value = logEvent.Properties["LogTimeStamp"];
                    cmd.Parameters.Add("@ConversationId", SqlDbType.NVarChar, 50).Value = logEvent.Properties["ConversationId"];
                    cmd.Parameters.Add("@Recipient", SqlDbType.NVarChar, 50).Value = logEvent.Properties["Recipient"];
                    cmd.Parameters.Add("@Sender", SqlDbType.NVarChar, 50).Value = logEvent.Properties["Sender"];
                    cmd.Parameters.Add("@Query", SqlDbType.NVarChar, 4000).Value = logEvent.Properties["Query"];

                    cmd.Parameters.Add("@TopScoringAnswer", SqlDbType.NVarChar, 100).Value = logEvent.Properties["TopScoringAnswer"];
                    cmd.Parameters.Add("@TopScoringAnswerScore", SqlDbType.Decimal).Value = logEvent.Properties["TopScoringAnswerScore"];
                    cmd.Parameters.Add("@TopScoringAnswer2", SqlDbType.NVarChar, 100).Value = logEvent.Properties["TopScoringAnswer2"];
                    cmd.Parameters.Add("@TopScoringAnswer2Score", SqlDbType.Decimal).Value = logEvent.Properties["TopScoringAnswer2Score"];
                    cmd.Parameters.Add("@TopScoringAnswer3", SqlDbType.NVarChar, 100).Value = logEvent.Properties["TopScoringAnswer3"];
                    cmd.Parameters.Add("@TopScoringAnswer3Score", SqlDbType.Decimal).Value = logEvent.Properties["TopScoringAnswer3Score"];

                    cmd.Parameters.Add("@MissedTreshold", SqlDbType.NVarChar, 20).Value = logEvent.Properties["MissedTreshold"];
                    cmd.Parameters.Add("@Treshold", SqlDbType.Decimal).Value = logEvent.Properties["Treshold"];

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
            try
            {
                var query = $"INSERT INTO [{TableName}] (LogTimeStamp, Query, Result, SourceLanguage, TargetLanguage)";
                query += " VALUES(@LogTimeStamp, @Query, @Result, @SourceLanguage, @TargetLanguage)";

                using (var connection = new SqlConnection(ConnectionString))
                {
                    var cmd = new SqlCommand
                    {
                        Connection = connection,
                        CommandType = CommandType.Text,
                        CommandText = query
                    };

                    cmd.Parameters.Add("@LogTimeStamp", SqlDbType.DateTime).Value = logEvent.Properties["LogTimeStamp"];
                    cmd.Parameters.Add("@Query", SqlDbType.NVarChar, 4000).Value = logEvent.Properties["Query"];
                    cmd.Parameters.Add("@Result", SqlDbType.NVarChar, 4000).Value = logEvent.Properties["Result"];
                    cmd.Parameters.Add("@SourceLanguage", SqlDbType.NVarChar, 100).Value = logEvent.Properties["SourceLanguage"];
                    cmd.Parameters.Add("@TargetLanguage", SqlDbType.NVarChar, 100).Value = logEvent.Properties["TargetLanguage"];

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

        public void WriteAimlLog(LogEventInfo logEvent)
        {
            try
            {
                var query = $"INSERT INTO [{TableName}] (LogTimeStamp, Query, Result)";
                query += " VALUES(@LogTimeStamp, @Query, @Result)";

                using (var connection = new SqlConnection(ConnectionString))
                {
                    var cmd = new SqlCommand
                    {
                        Connection = connection,
                        CommandType = CommandType.Text,
                        CommandText = query
                    };

                    cmd.Parameters.Add("@LogTimeStamp", SqlDbType.DateTime).Value = logEvent.Properties["LogTimeStamp"];
                    cmd.Parameters.Add("@Query", SqlDbType.NVarChar, 4000).Value = logEvent.Properties["Query"];
                    cmd.Parameters.Add("@Result", SqlDbType.NVarChar, 4000).Value = logEvent.Properties["Result"];

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

        public void WriteErrorLog(LogEventInfo logEvent)
        {
            try
            {
                var query = $"INSERT INTO [{TableName}] (Message, Exception, Stacktrace, LogTimeStamp)";
                query += " VALUES(@Message, @Exception, @Stacktrace, @LogTimeStamp)";

                using (var connection = new SqlConnection(ConnectionString))
                {
                    var cmd = new SqlCommand
                    {
                        Connection = connection,
                        CommandType = CommandType.Text,
                        CommandText = query
                    };

                    cmd.Parameters.Add("@Message", SqlDbType.NVarChar, 4000).Value = logEvent.Properties["Message"];
                    cmd.Parameters.Add("@Exception", SqlDbType.NVarChar, 1000).Value = logEvent.Properties["Exception"];
                    cmd.Parameters.Add("@Stacktrace", SqlDbType.NVarChar).Value = logEvent.Properties["Stacktrace"];
                    cmd.Parameters.Add("@LogTimeStamp", SqlDbType.DateTime).Value = logEvent.Properties["LogTimeStamp"];

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

    }
}
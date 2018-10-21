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

                    var conversationId = logEvent.Properties.ContainsKey("ConversationId")
                        ? logEvent.Properties["ConversationId"]
                        : DBNull.Value;

                    var sender = logEvent.Properties.ContainsKey("Sender")
                        ? logEvent.Properties["Sender"]
                        : DBNull.Value;

                    var recipient = logEvent.Properties.ContainsKey("Recipient")
                        ? logEvent.Properties["Recipient"]
                        : DBNull.Value;

                    var message = logEvent.Properties.ContainsKey("Message")
                        ? logEvent.Properties["Message"]
                        : DBNull.Value;

                    var logtimeStamp = logEvent.Properties.ContainsKey("LogTimeStamp")
                        ? logEvent.Properties["LogTimeStamp"]
                        : DBNull.Value;

                    var fullMessage = logEvent.Properties.ContainsKey("FullMessage")
                        ? logEvent.Properties["FullMessage"]
                        : DBNull.Value;

                    cmd.Parameters.Add("@ConversationId", SqlDbType.NVarChar, 50).Value = conversationId;
                    cmd.Parameters.Add("@Sender", SqlDbType.NVarChar, 50).Value = sender;
                    cmd.Parameters.Add("@Recipient", SqlDbType.NVarChar, 50).Value = recipient;
                    cmd.Parameters.Add("@Message", SqlDbType.NVarChar, 4000).Value = message;
                    cmd.Parameters.Add("@LogTimeStamp", SqlDbType.DateTime).Value = logtimeStamp;
                    cmd.Parameters.Add("@FullMessage", SqlDbType.NVarChar, 4000).Value = fullMessage;

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

                    var message = logEvent.Properties.ContainsKey("Message")
                        ? logEvent.Properties["Message"]
                        : DBNull.Value;

                    var logtimeStamp = logEvent.Properties.ContainsKey("LogTimeStamp")
                        ? logEvent.Properties["LogTimeStamp"]
                        : DBNull.Value;

                    cmd.Parameters.Add("@Message", SqlDbType.NVarChar, 4000).Value = message;
                    cmd.Parameters.Add("@LogTimeStamp", SqlDbType.DateTime).Value = logtimeStamp;

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
                var query = $"INSERT INTO [{TableName}] (LogTimeStamp, Dialog, ConversationId, Recipient, Sender, Query, TopScoringIntent, TopScoringIntentScore, TopScoringIntent2, TopScoringIntent2Score, TopScoringIntent3, TopScoringIntent3Score)";
                query += " VALUES(@LogTimeStamp, @Dialog, @ConversationId, @Recipient, @Sender, @Query, @TopScoringIntent, @TopScoringIntentScore, @TopScoringIntent2, @TopScoringIntent2Score, @TopScoringIntent3, @TopScoringIntent3Score)";

                using (var connection = new SqlConnection(ConnectionString))
                {
                    var cmd = new SqlCommand
                    {
                        Connection = connection,
                        CommandType = CommandType.Text,
                        CommandText = query
                    };

                    var logtimeStamp = logEvent.Properties.ContainsKey("LogTimeStamp")
                        ? logEvent.Properties["LogTimeStamp"]
                        : DBNull.Value;

                    var dialog = logEvent.Properties.ContainsKey("Dialog")
                        ? logEvent.Properties["Dialog"]
                        : DBNull.Value;

                    var conversationId = logEvent.Properties.ContainsKey("ConversationId")
                        ? logEvent.Properties["ConversationId"]
                        : DBNull.Value;

                    var sender = logEvent.Properties.ContainsKey("Sender")
                        ? logEvent.Properties["Sender"]
                        : DBNull.Value;

                    var recipient = logEvent.Properties.ContainsKey("Recipient")
                        ? logEvent.Properties["Recipient"]
                        : DBNull.Value;

                    var queryText = logEvent.Properties.ContainsKey("Query")
                        ? logEvent.Properties["Query"]
                        : DBNull.Value;

                    var topScoringIntent = logEvent.Properties.ContainsKey("TopScoringIntent")
                        ? logEvent.Properties["TopScoringIntent"]
                        : DBNull.Value;

                    var topScoringIntentScore = logEvent.Properties.ContainsKey("TopScoringIntentScore")
                        ? logEvent.Properties["TopScoringIntentScore"]
                        : DBNull.Value;

                    var topScoringIntent2 = logEvent.Properties.ContainsKey("TopScoringIntent2")
                        ? logEvent.Properties["TopScoringIntent2"]
                        : DBNull.Value;

                    var topScoringIntent2Score = logEvent.Properties.ContainsKey("TopScoringIntent2Score")
                        ? logEvent.Properties["TopScoringIntent2Score"]
                        : DBNull.Value;

                    var topScoringIntent3 = logEvent.Properties.ContainsKey("TopScoringIntent3")
                        ? logEvent.Properties["TopScoringIntent3"]
                        : DBNull.Value;

                    var topScoringIntent3Score = logEvent.Properties.ContainsKey("TopScoringIntent3Score")
                        ? logEvent.Properties["TopScoringIntent3Score"]
                        : DBNull.Value;

                    cmd.Parameters.Add("@LogTimeStamp", SqlDbType.DateTime).Value = logtimeStamp;
                    cmd.Parameters.Add("@Dialog", SqlDbType.NVarChar, 100).Value = dialog;
                    cmd.Parameters.Add("@ConversationId", SqlDbType.NVarChar, 50).Value = conversationId;
                    cmd.Parameters.Add("@Recipient", SqlDbType.NVarChar, 50).Value = recipient;
                    cmd.Parameters.Add("@Sender", SqlDbType.NVarChar, 50).Value = sender;
                    cmd.Parameters.Add("@Query", SqlDbType.NVarChar, 4000).Value = queryText;
                    cmd.Parameters.Add("@TopScoringIntent", SqlDbType.NVarChar, 100).Value = topScoringIntent;
                    cmd.Parameters.Add("@TopScoringIntentScore", SqlDbType.Decimal).Value = topScoringIntentScore;
                    cmd.Parameters.Add("@TopScoringIntent2", SqlDbType.NVarChar, 100).Value = topScoringIntent2;
                    cmd.Parameters.Add("@TopScoringIntent2Score", SqlDbType.Decimal).Value = topScoringIntent2Score;
                    cmd.Parameters.Add("@TopScoringIntent3", SqlDbType.NVarChar, 100).Value = topScoringIntent3;
                    cmd.Parameters.Add("@TopScoringIntent3Score", SqlDbType.Decimal).Value = topScoringIntent3Score;

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
                var query = $"INSERT INTO [{TableName}] (LogTimeStamp, ConversationId, Recipient, Sender, Query, TopScoringAnswer, TopScoringAnswerScore, TopScoringAnswer2, TopScoringAnswer2Score, TopScoringAnswer3, TopScoringAnswer3Score, MissedTreshold, Treshold)";
                query += " VALUES(@LogTimeStamp, @ConversationId, @Recipient, @Sender, @Query, @TopScoringAnswer, @TopScoringAnswerScore, @TopScoringAnswer2, @TopScoringAnswer2Score, @TopScoringAnswer3, @TopScoringAnswer3Score, @MissedTreshold, @Treshold)";

                using (var connection = new SqlConnection(ConnectionString))
                {
                    var cmd = new SqlCommand
                    {
                        Connection = connection,
                        CommandType = CommandType.Text,
                        CommandText = query
                    };

                    var logtimeStamp = logEvent.Properties.ContainsKey("LogTimeStamp")
                        ? logEvent.Properties["LogTimeStamp"]
                        : DBNull.Value;

                    var conversationId = logEvent.Properties.ContainsKey("ConversationId")
                        ? logEvent.Properties["ConversationId"]
                        : DBNull.Value;

                    var sender = logEvent.Properties.ContainsKey("Sender")
                        ? logEvent.Properties["Sender"]
                        : DBNull.Value;

                    var recipient = logEvent.Properties.ContainsKey("Recipient")
                        ? logEvent.Properties["Recipient"]
                        : DBNull.Value;

                    var queryText = logEvent.Properties.ContainsKey("Query")
                        ? logEvent.Properties["Query"]
                        : DBNull.Value;

                    var topScoringAnswer = logEvent.Properties.ContainsKey("TopScoringAnswer")
                        ? logEvent.Properties["TopScoringAnswer"]
                        : DBNull.Value;

                    var topScoringAnswerScore = logEvent.Properties.ContainsKey("TopScoringAnswerScore")
                        ? logEvent.Properties["TopScoringAnswerScore"]
                        : DBNull.Value;

                    var topScoringAnswer2 = logEvent.Properties.ContainsKey("TopScoringAnswer2")
                        ? logEvent.Properties["TopScoringAnswer2"]
                        : DBNull.Value;

                    var topScoringAnswer2Score = logEvent.Properties.ContainsKey("TopScoringAnswer2Score")
                        ? logEvent.Properties["TopScoringAnswer2Score"]
                        : DBNull.Value;

                    var topScoringAnswer3 = logEvent.Properties.ContainsKey("TopScoringAnswer3")
                        ? logEvent.Properties["TopScoringAnswer3"]
                        : DBNull.Value;

                    var topScoringAnswer3Score = logEvent.Properties.ContainsKey("TopScoringAnswer3Score")
                        ? logEvent.Properties["TopScoringAnswer3Score"]
                        : DBNull.Value;

                    var missedTreshold = logEvent.Properties.ContainsKey("MissedTreshold")
                        ? logEvent.Properties["MissedTreshold"]
                        : DBNull.Value;

                    var treshold = logEvent.Properties.ContainsKey("Treshold")
                        ? logEvent.Properties["Treshold"]
                        : DBNull.Value;

                    cmd.Parameters.Add("@LogTimeStamp", SqlDbType.DateTime).Value = logtimeStamp;
                    cmd.Parameters.Add("@ConversationId", SqlDbType.NVarChar, 50).Value = conversationId;
                    cmd.Parameters.Add("@Recipient", SqlDbType.NVarChar, 50).Value = recipient;
                    cmd.Parameters.Add("@Sender", SqlDbType.NVarChar, 50).Value = sender;
                    cmd.Parameters.Add("@Query", SqlDbType.NVarChar, 4000).Value = queryText;

                    cmd.Parameters.Add("@TopScoringAnswer", SqlDbType.NVarChar, 100).Value = topScoringAnswer;
                    cmd.Parameters.Add("@TopScoringAnswerScore", SqlDbType.Decimal).Value = topScoringAnswerScore;
                    cmd.Parameters.Add("@TopScoringAnswer2", SqlDbType.NVarChar, 100).Value = topScoringAnswer2;
                    cmd.Parameters.Add("@TopScoringAnswer2Score", SqlDbType.Decimal).Value = topScoringAnswer2Score;
                    cmd.Parameters.Add("@TopScoringAnswer3", SqlDbType.NVarChar, 100).Value = topScoringAnswer3;
                    cmd.Parameters.Add("@TopScoringAnswer3Score", SqlDbType.Decimal).Value = topScoringAnswer3Score;

                    cmd.Parameters.Add("@MissedTreshold", SqlDbType.NVarChar, 20).Value = missedTreshold;
                    cmd.Parameters.Add("@Treshold", SqlDbType.Decimal).Value = treshold;

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

                    var logtimeStamp = logEvent.Properties.ContainsKey("LogTimeStamp")
                        ? logEvent.Properties["LogTimeStamp"]
                        : DBNull.Value;

                    var sourceLanguage = logEvent.Properties.ContainsKey("SourceLanguage")
                        ? logEvent.Properties["SourceLanguage"]
                        : DBNull.Value;

                    var targetLanguage = logEvent.Properties.ContainsKey("TargetLanguage")
                        ? logEvent.Properties["TargetLanguage"]
                        : DBNull.Value;

                    var result = logEvent.Properties.ContainsKey("Result")
                        ? logEvent.Properties["Result"]
                        : DBNull.Value;

                    var queryText = logEvent.Properties.ContainsKey("Query")
                        ? logEvent.Properties["Query"]
                        : DBNull.Value;

                    cmd.Parameters.Add("@LogTimeStamp", SqlDbType.DateTime).Value = logtimeStamp;
                    cmd.Parameters.Add("@Query", SqlDbType.NVarChar, 4000).Value = queryText;
                    cmd.Parameters.Add("@Result", SqlDbType.NVarChar, 4000).Value = result;
                    cmd.Parameters.Add("@SourceLanguage", SqlDbType.NVarChar, 100).Value = sourceLanguage;
                    cmd.Parameters.Add("@TargetLanguage", SqlDbType.NVarChar, 100).Value = targetLanguage;

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

                    var logtimeStamp = logEvent.Properties.ContainsKey("LogTimeStamp")
                        ? logEvent.Properties["LogTimeStamp"]
                        : DBNull.Value;

                    var queryText = logEvent.Properties.ContainsKey("Query")
                        ? logEvent.Properties["Query"]
                        : DBNull.Value;

                    var result = logEvent.Properties.ContainsKey("Result")
                        ? logEvent.Properties["Result"]
                        : DBNull.Value;

                    cmd.Parameters.Add("@LogTimeStamp", SqlDbType.DateTime).Value = logtimeStamp;
                    cmd.Parameters.Add("@Query", SqlDbType.NVarChar, 4000).Value = queryText;
                    cmd.Parameters.Add("@Result", SqlDbType.NVarChar, 4000).Value = result;

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
                var query = $"INSERT INTO [{TableName}] (Message, Exception, Stacktrace, LogTimeStamp, ConversationId)";
                query += " VALUES(@Message, @Exception, @Stacktrace, @LogTimeStamp, @ConversationId)";

                using (var connection = new SqlConnection(ConnectionString))
                {
                    var cmd = new SqlCommand
                    {
                        Connection = connection,
                        CommandType = CommandType.Text,
                        CommandText = query
                    };

                    var logtimeStamp = logEvent.Properties.ContainsKey("LogTimeStamp")
                        ? logEvent.Properties["LogTimeStamp"]
                        : DBNull.Value;

                    var message = logEvent.Properties.ContainsKey("Message")
                        ? logEvent.Properties["Message"]
                        : DBNull.Value;

                    var exception = logEvent.Properties.ContainsKey("Exception")
                        ? logEvent.Properties["Exception"]
                        : DBNull.Value;

                    var stacktrace = logEvent.Properties.ContainsKey("Stacktrace")
                        ? logEvent.Properties["Stacktrace"]
                        : DBNull.Value;

                    var conversationId = logEvent.Properties.ContainsKey("ConversationId")
                        ? logEvent.Properties["ConversationId"]
                        : DBNull.Value;

                    cmd.Parameters.Add("@Message", SqlDbType.NVarChar, 4000).Value = message;
                    cmd.Parameters.Add("@Exception", SqlDbType.NVarChar, 1000).Value = exception;
                    cmd.Parameters.Add("@Stacktrace", SqlDbType.NVarChar).Value = stacktrace;
                    cmd.Parameters.Add("@LogTimeStamp", SqlDbType.DateTime).Value = logtimeStamp;
                    cmd.Parameters.Add("@ConversationId", SqlDbType.NVarChar, 50).Value = conversationId;

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
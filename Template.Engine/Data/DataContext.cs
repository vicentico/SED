using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using Template.Engine.Helper;

namespace Template.Engine.Data
{
    public class DataContext : IDisposable
    {
        #region Dispose.

        public void Dispose()
        {
            Parameters = null;
            Connection = null;
            ConnectionString = "";
            Command = "";
            CommandType = CommandType.Text;
        }

        #endregion Dispose.

        #region ExecuteQuery.

	    public DataTable ExecuteQuery(int TableIndex = 0)
        {
            try
            {
                var ConnectionString_ =
                    ConfigurationManager.ConnectionStrings[
                        !string.IsNullOrEmpty(ConnectionString) ? ConnectionString : ""].ConnectionString;
                ConnectionString_ = string.IsNullOrEmpty(ConnectionString_)
                    ? Connection != null ? Connection.ConnectionString : ""
                    : "";

                if (string.IsNullOrEmpty(ConnectionString_))
                    throw new Exception("No se especificó la Cadena de Conexión para la Base de Datos.");

                using (var SqlCon = new SqlConnection(ConnectionString_))
                {
                    SqlCon.Open();

                    using (var SqlCom = new SqlCommand(Command, SqlCon))
                    {
                        SqlCom.CommandTimeout = 120;

                        if (Parameters != null)
                        {
                            foreach (SqlParameter Parameter in Parameters)
                                SqlCom.Parameters.AddWithValue(Parameter.ParameterName, Parameter.Value);
                        }

                        SqlCom.CommandType = CommandType;

                        LogSqlCommand(SqlCom);

                        using (var Sda = new SqlDataAdapter(SqlCom))
                        {
                            var Ds = new DataSet();

                            Sda.Fill(Ds);

                            if (Ds.Tables.Count > TableIndex && Ds.Tables[TableIndex].Rows.Count > 0)
                                return Ds.Tables[TableIndex];
                            return null;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Debug.WriteLine(string.Format("Error: {0}", Ex.ObtenerExcepcionCompleta()));
                throw new Exception("Error al ejecutar [ExecuteQuery].", Ex);
            }
        }

        #endregion ExecuteQuery.

        #region ExecuteQuery<T>.

        public List<T> ExecuteQuery<T>(int TableIndex = 0) where T : new()
        {
            try
            {
                var ConnectionString_ =
                    ConfigurationManager.ConnectionStrings[
                        !string.IsNullOrEmpty(ConnectionString) ? ConnectionString : ""]
                        .ConnectionString;
                ConnectionString_ = string.IsNullOrEmpty(ConnectionString_)
                    ? Connection != null ? Connection.ConnectionString : ""
                    : ConnectionString_;

                if (string.IsNullOrEmpty(ConnectionString_))
                    throw new Exception("No se especificó la Cadena de Conexión para la Base de Datos.");

                using (var SqlCon = new SqlConnection(ConnectionString_))
                {
                    SqlCon.Open();

                    using (var SqlCom = new SqlCommand(Command, SqlCon))
                    {
                        SqlCom.CommandTimeout = 120;

                        if (Parameters != null)
                        {
                            foreach (SqlParameter Parameter in Parameters)
                            {
                                SqlCom.Parameters.AddWithValue(Parameter.ParameterName, Parameter.Value);
                            }
                        }

                        SqlCom.CommandType = CommandType;

                        LogSqlCommand(SqlCom);

                        using (var Sda = new SqlDataAdapter(SqlCom))
                        {
                            var Ds = new DataSet();

                            Sda.Fill(Ds);

                            if (Ds.Tables.Count > TableIndex && Ds.Tables[TableIndex].Rows.Count > 0)
                            {
                                return Ds.Tables[TableIndex].ToList<T>();
                            }
                            return null;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Debug.WriteLine(string.Format("Error: {0}", Ex.ObtenerExcepcionCompleta()));
                throw new Exception("Error al ejecutar ExecuteQuery<T>.", Ex);
            }
        }

        #endregion ExecuteQuery<T>.

        #region ExecuteQuery DataTableCollection.

        public DataTableCollection ExecuteQuery()
        {
            try
            {
                var ConnectionString_ =
                    ConfigurationManager.ConnectionStrings[
                        !string.IsNullOrEmpty(ConnectionString) ? ConnectionString : ""]
                        .ConnectionString;
                ConnectionString_ = string.IsNullOrEmpty(ConnectionString_)
                    ? Connection != null ? Connection.ConnectionString : ""
                    : ConnectionString_;

                if (string.IsNullOrEmpty(ConnectionString_))
                    throw new Exception("No se especificó la Cadena de Conexión para la Base de Datos.");

                using (var SqlCon = new SqlConnection(ConnectionString_))
                {
                    SqlCon.Open();

                    using (var SqlCom = new SqlCommand(Command, SqlCon))
                    {
                        SqlCom.CommandTimeout = 120;

                        if (Parameters != null)
                        {
                            foreach (SqlParameter Parameter in Parameters)
                                SqlCom.Parameters.AddWithValue(Parameter.ParameterName, Parameter.Value);
                        }

                        SqlCom.CommandType = CommandType;

                        LogSqlCommand(SqlCom);

                        using (var Sda = new SqlDataAdapter(SqlCom))
                        {
                            var Ds = new DataSet();

                            Sda.Fill(Ds);

                            if (Ds.Tables.Count > 0)
                                return Ds.Tables;
                            return null;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Debug.WriteLine(string.Format("Error: {0}", Ex.ObtenerExcepcionCompleta()));
                throw new Exception("Error al ejecutar ExecuteQuery.", Ex);
            }
        }

        #endregion ExecuteQuery DataTableCollection.

        #region ExecuteNonQuery.

        public bool ExecuteNonQuery()
        {
            try
            {
                var ConnectionString_ =
                    ConfigurationManager.ConnectionStrings[
                        !string.IsNullOrEmpty(ConnectionString) ? ConnectionString : ""]
                        .ConnectionString;
                ConnectionString_ = string.IsNullOrEmpty(ConnectionString_)
                    ? Connection != null ? Connection.ConnectionString : ""
                    : ConnectionString_;

                if (string.IsNullOrEmpty(ConnectionString_))
                    throw new Exception("No se especificó la Cadena de Conexión para la Base de Datos.");

                using (var SqlCon = new SqlConnection(ConnectionString_))
                {
                    SqlCon.Open();

                    using (var SqlCom = new SqlCommand(Command, SqlCon))
                    {
                        SqlCom.CommandTimeout = 120;

                        if (Parameters != null)
                        {
                            foreach (SqlParameter Parameter in Parameters)
                            {
                                SqlCom.Parameters.AddWithValue(Parameter.ParameterName, Parameter.Value);
                            }
                        }

                        SqlCom.CommandType = CommandType;

                        LogSqlCommand(SqlCom);

                        if (SqlCom.ExecuteNonQuery() >= 0)
                        {
                            SqlCon.Close();

                            return true;
                        }
                        SqlCon.Close();

                        return false;
                    }
                }
            }
            catch (Exception Ex)
            {
                Debug.WriteLine(string.Format("Error: {0}", Ex.ObtenerExcepcionCompleta()));
                throw new Exception("Error al ejecutar ExecuteNonQuery.", Ex);
            }
        }

        #endregion ExecuteNonQuery.

        #region LogSQLCommand.

        private void LogSqlCommand(SqlCommand Command_)
        {
            string StrCommand = null;

            if (Command_.CommandType != CommandType.StoredProcedure)
            {
                if (Command_.CommandType == CommandType.Text)
                {
                    StrCommand = Command_.CommandText;

                    foreach (SqlParameter Parameter in Command_.Parameters)
                    {
                        switch (Parameter.SqlDbType)
                        {
                            case SqlDbType.Int:
                            case SqlDbType.Float:
                                StrCommand = StrCommand.Replace(Parameter.ParameterName, Parameter.Value.ToString());
                                break;
                            case SqlDbType.Text:
                            case SqlDbType.VarChar:
                            case SqlDbType.Char:
                            case SqlDbType.DateTime:
                            case SqlDbType.NVarChar:
                            case SqlDbType.UniqueIdentifier:
                                StrCommand = StrCommand.Replace(Parameter.ParameterName, string.Format("'{0}'", Parameter.Value));
                                break;
                            default:
                                StrCommand = StrCommand.Replace(Parameter.ParameterName, string.Format("'{0}'", Parameter.Value));
                                break;
                        }
                    }
                }
            }
            else
            {
                StrCommand = string.Format("{0}: {1}", Command_.Connection.Database, Command_.CommandText);

                foreach (SqlParameter Parameter in Command_.Parameters)
                {
                    switch (Parameter.SqlDbType)
                    {
                        case SqlDbType.Int:
                        case SqlDbType.Float:
                            var Value = Parameter.Value != null ? Parameter.Value.ToString() : DBNull.Value.ToString(CultureInfo.InvariantCulture);
                            StrCommand += string.Format("{0} = {1}, ", Parameter.ParameterName, Value);
                            break;
                        case SqlDbType.Text:
                        case SqlDbType.VarChar:
                        case SqlDbType.Char:
                        case SqlDbType.DateTime:
                        case SqlDbType.NVarChar:
                            Value = Parameter.Value != null ? Parameter.Value.ToString() : "";
                            StrCommand += string.Format("{0} = '{1}', ", Parameter.ParameterName, Value);
                            break;
                        case SqlDbType.VarBinary:
                            StrCommand += string.Format("{0} = {1}, ", Parameter.ParameterName, DBNull.Value);
                            break;
                        default:
                            StrCommand += string.Format("@{0} =  '{0}',", Parameter.ParameterName);
                            break;
                    }
                }

                if (Command_.Parameters.Count > 0) StrCommand.Substring(0, StrCommand.Length - 2);
            }

            Debug.WriteLine(string.Format("\n{0}\n", StrCommand));
        }

        #endregion LogSQLCommand.

        #region Atributos.

        public SqlParameterCollection Parameters { get; set; }
        public DbConnection Connection { get; set; }
        public string ConnectionString { get; set; }
        public string Command { get; set; }
        public CommandType CommandType { get; set; }

        #endregion Atributos.

        #region Constructores.

        public DataContext()
        {
            ConnectionString = "";
            Connection = null;
            Command = "";
            CommandType = CommandType.Text;
            Parameters = (SqlParameterCollection) typeof (SqlParameterCollection)
                .GetConstructor(
                    BindingFlags.NonPublic | BindingFlags.Instance,
                    null,
                    Type.EmptyTypes,
                    null
                ).Invoke(null);
        }

        public DataContext(string Catalog, string Command_, CommandType CommandType_)
        {
            Connection = null;
            ConnectionString = Catalog;
            Command = Command_;
            CommandType = CommandType_;
            Parameters = (SqlParameterCollection) typeof (SqlParameterCollection)
                .GetConstructor(
                    BindingFlags.NonPublic | BindingFlags.Instance,
                    null,
                    Type.EmptyTypes,
                    null
                ).Invoke(null);
        }

        public DataContext(DbConnection Connection_, string Command_, CommandType CommandType_)
        {
            Connection = Connection_;
            Command = Command_;
            CommandType = CommandType_;
            Parameters = (SqlParameterCollection) typeof (SqlParameterCollection)
                .GetConstructor(
                    BindingFlags.NonPublic | BindingFlags.Instance,
                    null,
                    Type.EmptyTypes,
                    null
                ).Invoke(null);
        }

        #endregion Constructores.
    }
}
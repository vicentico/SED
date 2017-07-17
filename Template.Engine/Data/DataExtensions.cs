using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Template.Engine.Data
{
    public static class DataExtensions
    {
        #region DataTable.ToList<T>.

        public static List<T> ToList<T>(this DataTable Table) where T : new()
        {
            MemberInfo[] Members = typeof (T).GetFields();

            if (Members.IsNullOrEmpty()) Members = typeof (T).GetProperties();

            return (from object Row_ in Table.Rows select CreateItemFromRow<T>((DataRow) Row_, Members)).ToList();
        }

        #endregion DataTable.ToList<T>.

        #region CreateItemFromRow.

        private static T CreateItemFromRow<T>(DataRow Row, IList<MemberInfo> Members) where T : new()
        {
            var Item = new T();

            try
            {
                foreach (var Member in Members)
                {
                    if (Row.Table.Columns.Contains(Member.Name))
                    {
                        var Data = Row[Member.Name].ToString();

                        if (Member.MemberType != MemberTypes.Property)
                        {
                            if (Member.MemberType != MemberTypes.Field) continue;
                            var Field = (FieldInfo) Member;

                            #region Valores.

                            if (Field.FieldType == typeof (DayOfWeek))
                            {
                                var Valor = (DayOfWeek) Enum.Parse(typeof (DayOfWeek), Row[Member.Name].ToString());

                                Field.SetValue(Item, Valor);
                            }
                            else if (Field.FieldType == typeof (DateTime) || Field.FieldType == typeof (DateTime?))
                            {
                                var Valor = string.IsNullOrEmpty(Data) ? (DateTime?) null : DateTime.Parse(Data);

                                Field.SetValue(Item, Valor);
                            }
                            else if (Field.FieldType == typeof (int))
                            {
                                int? Valor = string.IsNullOrEmpty(Data) ? 0 : int.Parse(Data);

                                Field.SetValue(Item, Valor);
                            }
                            else if (Field.FieldType == typeof (long))
                            {
                                long? Valor = string.IsNullOrEmpty(Data) ? 0 : long.Parse(Data);

                                Field.SetValue(Item, Valor);
                            }
                            else if (Field.FieldType == typeof (decimal))
                            {
                                decimal? Valor = string.IsNullOrEmpty(Data) ? 0 : decimal.Parse(Data);

                                Field.SetValue(Item, Valor);
                            }
                            else if (Field.FieldType == typeof (bool))
                            {
                                Data = Data == "0" ? "false" : (Data == "1" ? "true" : Data);

                                bool? Valor = !string.IsNullOrEmpty(Data) && bool.Parse(Data);

                                Field.SetValue(Item, Valor);
                            }
                            else
                            {
                                if (Row[Member.Name] == null || Row[Member.Name] == DBNull.Value)
                                {
                                    Field.SetValue(Item, null);
                                }
                                else
                                {
                                    Field.SetValue(Item, Row[Member.Name].ToString());
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            var Property = (PropertyInfo) Member;

                            #region Valores.

                            if (Property.PropertyType == typeof (DayOfWeek))
                            {
                                var Valor = (DayOfWeek) Enum.Parse(typeof (DayOfWeek), Row[Member.Name].ToString());

                                Property.SetValue(Item, Valor, null);
                            }
                            else if (Property.PropertyType == typeof (DateTime) ||
                                     Property.PropertyType == typeof (DateTime?))
                            {
                                var Valor = string.IsNullOrEmpty(Data) ? (DateTime?) null : DateTime.Parse(Data);

                                Property.SetValue(Item, Valor, null);
                            }
                            else if (Property.PropertyType == typeof (int))
                            {
                                int? Valor = string.IsNullOrEmpty(Data) ? 0 : int.Parse(Data);

                                Property.SetValue(Item, Valor, null);
                            }
                            else if (Property.PropertyType == typeof (long))
                            {
                                long? Valor = string.IsNullOrEmpty(Data) ? 0 : long.Parse(Data);

                                Property.SetValue(Item, Valor, null);
                            }
                            else if (Property.PropertyType == typeof (decimal))
                            {
                                decimal? Valor = string.IsNullOrEmpty(Data) ? 0 : decimal.Parse(Data);

                                Property.SetValue(Item, Valor, null);
                            }
                            else if (Property.PropertyType == typeof (bool))
                            {
                                Data = Data == "0" ? "false" : (Data == "1" ? "true" : Data);

                                bool? Valor = !string.IsNullOrEmpty(Data) && bool.Parse(Data);

                                Property.SetValue(Item, Valor, null);
                            }
                            else
                            {
                                if (Row[Member.Name] == null || Row[Member.Name] == DBNull.Value)
                                {
                                    Property.SetValue(Item, null, null);
                                }
                                else
                                {
                                    Property.SetValue(Item, Row[Member.Name].ToString(), null);
                                }
                            }

                            #endregion
                        }
                    }
                    else
                    {
                        if (Member.MemberType == MemberTypes.Property)
                        {
                            var Property = (PropertyInfo) Member;

                            Property.SetValue(Item, null, null);
                        }
                        else if (Member.MemberType == MemberTypes.Field)
                        {
                            var Field = (FieldInfo) Member;

                            Field.SetValue(Item, null);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Mapear Campo.", Ex);
            }

            return Item;
        }

        #endregion CreateItemFromRow.

        #region DataRow.GetFieldValue.

        public static object GetFieldValue(this DataRow Fila, string NombreCampo = "")
        {
            try
            {
                return Fila.Field<object>(NombreCampo);
            }
            catch (Exception Ex)
            {
                throw new Exception(string.Format("Error al Obtener Valor de Campo [{0}].", NombreCampo), Ex);
            }
        }

        #endregion DataRow.GetFieldValue.

        #region IEnumerable IsNullOrEmpty.

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> Enumerable)
        {
            if (Enumerable == null) return true;

            return !Enumerable.Any();
        }

        #endregion IEnumerable IsNullOrEmpty.
    }
}
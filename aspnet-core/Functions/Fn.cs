using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Functions
{
   public class Fn
    {
        public static DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                System.ComponentModel.PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

     

        public static DateTime ParseDate(string date)
        {
            string[] formats = {
                "d.MM.yyyy",
                "dd.MM.yyyy",
                "dd.M.yyyy",
                "d.M.yyyy",
                "d/MM/yyyy",
                "dd/MM/yyyy",
                "dd/M/yyyy",
                "d/M/yyyy",
                "yyyy-MM-dd'T'HH:mm:ss.fff'Z'"
            };
            DateTime day ;
            try
            {
                day = DateTime.ParseExact(date, "yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                day = System.Convert.ToDateTime(date, CultureInfo.InvariantCulture);
            }

            return day;
        }

        public static string GetDateReport(string date)
        {
            var t = ParseDate(date);
            return t.Day + "" + t.Month + "" + t.Year;
        }
        public static DateTime StartHour(DateTime dt)
        {
            var t = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
            return t;
        }

        public static DateTime EnDhour(DateTime dt)
        {
            var t = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
            return t;
        }

    }
}

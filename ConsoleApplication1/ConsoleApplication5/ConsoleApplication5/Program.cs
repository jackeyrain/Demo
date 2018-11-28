using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication5
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection conn = new SqlConnection("Data Source=(local);Initial Catalog=MyDB;Persist Security Info=True;User ID=sa;Password=sa");
            SqlDataAdapter adapter = new SqlDataAdapter("select * from [dbo].[P]", conn);
            
            DataSet ds = new DataSet();
            adapter.Fill(ds, "one");

            ds.Tables["one"].Rows[0]["Age"] = 100;
            SqlCommandBuilder MyCb = new SqlCommandBuilder(adapter);
            adapter.Update(ds);
            
        }
    }
}

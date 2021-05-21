using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace RSLTest
{
    public partial class Student : Form
    {
        public Student()
        {
            InitializeComponent();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Do You Want to Upload Data?");
                if (result == DialogResult.OK)
                {
                    this.Close();
                    DataTable dt = new DataTable();
                    using (StreamReader sr = new StreamReader(@"C:\Users\pc\Documents\Visual Studio 2012\Students.csv"))
                    {
                        string[] headers = sr.ReadLine().Split(',');
                        foreach (string header in headers)
                        {
                            dt.Columns.Add(header);
                        }
                        while (!sr.EndOfStream)
                        {
                            string[] rows = sr.ReadLine().Split(',');
                            DataRow dr = dt.NewRow();
                            for (int i = 0; i < headers.Length; i++)
                            {
                                dr[i] = rows[i];
                            }
                            dt.Rows.Add(dr);
                        }
                    }

                    using (SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-GDJP6SGB\SQLEXPRESS;Initial Catalog=Students;Integrated Security=SSPI;"))
                    {
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                        {
                            sqlBulkCopy.DestinationTableName = "dbo.Students_Data";
                            con.Open();
                            sqlBulkCopy.WriteToServer(dt);
                            con.Close();
                        }
                        MessageBox.Show("Save successfull");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-GDJP6SGB\SQLEXPRESS;Initial Catalog=Students;Integrated Security=SSPI;"))
                {
                    string sql = "SELECT * FROM Students_Data";
                    SqlDataAdapter dataadapter = new SqlDataAdapter(sql, con);
                    DataSet ds = new DataSet();
                    con.Open();
                    dataadapter.Fill(ds, "Students_Data");
                    con.Close();
                    dtStudents.DataSource = ds;
                    dtStudents.DataMember = "Students_Data";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                dtStudents.DataSource = null;
                dtStudents.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

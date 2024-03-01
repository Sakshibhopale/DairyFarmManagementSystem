using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DairyFarm
{
    public partial class Employees : Form
    {
        public Employees()
        {
            InitializeComponent();
            populate();
        }

        private void Employees_Load(object sender, EventArgs e)
        {

        }
        SqlConnection Con = new SqlConnection(@"Data Source=LAPTOP-Q05C0DKC\SQLEXPRESS01;Initial Catalog=DairyFarmDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");
        private void populate()
        {
            Con.Open();
            string query = "select * from EmployeeTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            EmployeesDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void Clear()
        {
            EmpNameTb.Text = "";
            GenCb.SelectedIndex = -1;
            PhoneTb.Text = "";
            AddressTb.Text = "";
            EmpPassTb.Text = "";
            key = 0;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (EmpNameTb.Text == "" || GenCb.SelectedIndex == -1 || PhoneTb.Text == "" || AddressTb.Text == "" || EmpPassTb.Text == "" )
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "INSERT INTO EmployeeTbl (EmpName, EmpDob, Gender, Phone, Address, EmpPass) " +
                                   "VALUES (@EmpName, @EmpDob, @Gender, @Phone, @Address, @EmpPass)";

                    SqlCommand cmd = new SqlCommand(Query, Con);

                    // Use parameters to prevent SQL injection
                    cmd.Parameters.AddWithValue("@EmpName", EmpNameTb.Text);
                    cmd.Parameters.AddWithValue("@EmpDob", DOB.Text);
                    cmd.Parameters.AddWithValue("@Gender", GenCb.Text);
                    cmd.Parameters.AddWithValue("@Phone", PhoneTb.Text);
                    cmd.Parameters.AddWithValue("@Address", AddressTb.Text);
                    cmd.Parameters.AddWithValue("@EmpPass", EmpPassTb.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Employee Data Saved Successfully");

                    Con.Close();
                    populate();
                    Clear();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }
        int key = 0;
        private void EmployeesDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < EmployeesDGV.Rows.Count)
            {
                DataGridViewRow row = EmployeesDGV.Rows[e.RowIndex];

                EmpNameTb.Text = row.Cells["EmpName"].Value.ToString();
                DOB.Text = row.Cells["EmpDob"].Value.ToString();
                GenCb.Text = row.Cells["Gender"].Value.ToString();
                PhoneTb.Text = row.Cells["Phone"].Value.ToString();
                AddressTb.Text = row.Cells["Address"].Value.ToString();
                EmpPassTb.Text = row.Cells["EmpPass"].Value.ToString();

                if (string.IsNullOrEmpty(EmpNameTb.Text))
                {
                    key = 0;
                }
                else
                {
                    key = Convert.ToInt32(row.Cells["EmpId"].Value);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (EmpNameTb.Text == "" || DOB.Text == "" || GenCb.SelectedIndex == -1 || PhoneTb.Text == "" || AddressTb.Text == "" || EmpPassTb.Text=="")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "update EmployeeTbl set EmpName = '" + EmpNameTb.Text + "' , EmpDob = '" + DOB.Text + "' , Gender = '" + GenCb.Text + "' , Phone = '" + PhoneTb.Text + "', Address = '" + AddressTb.Text + "', EmpPass = '"+EmpPassTb.Text+"' where EmpId = " + key + "; ";

                    SqlCommand cmd = new SqlCommand(Query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Employee Data Updated Successfully");

                    Con.Close();
                    populate();
                    Clear();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Select the Employee to be deleted");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "delete from EmployeeTbl where EmpId=" + key + ";";

                    SqlCommand cmd = new SqlCommand(Query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Employee Data Deleted Successfully");

                    Con.Close();
                    populate();
                    Clear();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

    }
}

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
    public partial class Finance : Form
    {
        public Finance()
        {
            InitializeComponent();
            populateExp();
            populateInc();
            FillEmpId();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            Cows Ob = new Cows();
            Ob.Show();
            this.Hide();
        }

        private void label13_Click(object sender, EventArgs e)
        {
            MilkProduction Ob = new MilkProduction();
            Ob.Show();
            this.Hide();
        }

        private void label15_Click(object sender, EventArgs e)
        {
            CowHealth Ob = new CowHealth();
            Ob.Show();
            this.Hide();
        }

        private void label17_Click(object sender, EventArgs e)
        {
            Breed Ob = new Breed();
            Ob.Show();
            this.Hide();
        }

        private void label19_Click(object sender, EventArgs e)
        {
            MilkSales Ob = new MilkSales();
            Ob.Show();
            this.Hide();
        }

        private void label23_Click(object sender, EventArgs e)
        {
            DashBoard Ob = new DashBoard();
            Ob.Show();
            this.Hide();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=LAPTOP-Q05C0DKC\SQLEXPRESS01;Initial Catalog=DairyFarmDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");
        
        private void populateExp()
        {
            Con.Open();
            string query = "select * from ExpenditureTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ExpDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void FilterExp()
        {
            Con.Open();
            string query = "SELECT * FROM ExpenditureTbl WHERE ExpDate = @ExpDate";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);

            // Use parameters to prevent SQL injection
            sda.SelectCommand.Parameters.AddWithValue("@ExpDate", ExpDateFilter.Value.Date);

            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ExpDGV.DataSource = ds.Tables[0];  // Make sure to use ExpDGV for ExpenditureDataGridView
            Con.Close();
        }

        private void ClearExp()
        {
            AmountTb.Text = "";
        }
        private void button5_Click(object sender, EventArgs e)
        {

            if (PurposeCb.SelectedIndex == -1 || AmountTb.Text == "" || EmpIdCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "INSERT INTO ExpenditureTbl (EmpId, ExpDate, ExpPurpose, ExpAmount) " +
                                   "VALUES ( @EmpId, @ExpDate, @ExpPurpose, @ExpAmount)";

                    SqlCommand cmd = new SqlCommand(Query, Con);

                    // Use parameters to prevent SQL injection
                    cmd.Parameters.AddWithValue("@EmpId", EmpIdCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@ExpDate", ExpDate.Text);
                    cmd.Parameters.AddWithValue("@ExpPurpose", PurposeCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@ExpAmount", AmountTb.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Expenditure Saved Successfully");

                    Con.Close();
                    populateExp();
                    ClearExp();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void FillEmpId()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("select EmpId from EmployeeTbl", Con);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("EmpId", typeof(int));
            dt.Load(Rdr);
            EmpIdCb.ValueMember = "EmpId";
            EmpIdCb.DataSource = dt;

            Con.Close();
        }
        private void ClearInc()
        {
            IncAmount.Text = "";
            IncPurCb.SelectedIndex = -1;
        }
        private void populateInc()
        {
            Con.Open();
            string query = "select * from IncomeTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            IncDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        
        private void FilterIncome()
        {
            Con.Open();
            string query = "SELECT * FROM IncomeTbl WHERE IncDate = @IncomeDate";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);

            // Use parameters to prevent SQL injection
            sda.SelectCommand.Parameters.AddWithValue("@IncomeDate", IncDateFilter.Value.Date);

            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            IncDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (IncPurCb.SelectedIndex == -1 || IncAmount.Text == "" || EmpIdCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "INSERT INTO IncomeTbl (EmpId, IncDate, IncPurpose, IncAmt) " +
                                   "VALUES ( @EmpId, @IncDate, @IncPurpose, @IncAmt)";

                    SqlCommand cmd = new SqlCommand(Query, Con);

                    // Use parameters to prevent SQL injection
                    cmd.Parameters.AddWithValue("@EmpId", EmpIdCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@IncDate", IncDate.Text);
                    cmd.Parameters.AddWithValue("@IncPurpose", IncPurCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@IncAmt", IncAmount.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Income Saved Successfully");

                    Con.Close();
                    populateInc();
                    ClearInc();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            FilterIncome();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            FilterExp();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            populateInc();
        }


        private void pictureBox10_Click(object sender, EventArgs e)
        {
            populateExp();
        }

        private void Finance_Load(object sender, EventArgs e)
        {

        }
    }
}

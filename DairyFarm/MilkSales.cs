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
    public partial class MilkSales : Form
    {
        public MilkSales()
        {
            InitializeComponent();
            FillEmpId();
            populate();
        }

        
        private void MilkSales_Load(object sender, EventArgs e)
        {

        }

        

        private void label23_Click(object sender, EventArgs e)
        {
            DashBoard Ob = new DashBoard();
            Ob.Show();
            this.Hide();
        }

        private void label21_Click(object sender, EventArgs e)
        {
            Finance Ob = new Finance();
            Ob.Show();
            this.Hide();
        }

        private void label17_Click(object sender, EventArgs e)
        {
            Breed Ob = new Breed ();
            Ob.Show();
            this.Hide();
        }

        private void label15_Click(object sender, EventArgs e)
        {
            CowHealth Ob = new CowHealth();
            Ob.Show();
            this.Hide();
        }

        private void label13_Click(object sender, EventArgs e)
        {
            MilkProduction Ob = new MilkProduction();
            Ob.Show();
            this.Hide();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            Cows Ob = new Cows();
            Ob.Show();
            this.Hide();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=LAPTOP-Q05C0DKC\SQLEXPRESS01;Initial Catalog=DairyFarmDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");
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
        private void populate()
        {
            Con.Open();
            string query = "select * from MilkSalesTable";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            SalesDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void Clear()
        {
            PhoneTb.Text = "";
            PriceTb.Text = "";
            ClientNameTb.Text = "";
            QuantityTb.Text = "";
            TotalTb.Text = "";
            key = 0;
        }

        private void QuantityTb_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(PriceTb.Text, out int PriceValue) &&
               int.TryParse(QuantityTb.Text, out int QuantityValue) )
            {
                // Perform the calculation and update TotalTb.
                int total = PriceValue * QuantityValue;
                TotalTb.Text = total.ToString();
            }
            else
            {
                // Handle parsing errors, e.g., display a message or set a default value.
                TotalTb.Text = "Invalid input";
            }
        }
        //private void SaveTransaction()
        //{

        //    {
        //        try
        //        {
        //            string Sales = "Sales";
        //            Con.Open();
        //            string Query = "INSERT INTO IncomeTbl ('"+ Date.Value.Date +"', '"+ Sales +"', "+ TotalTb +", "+ EmpIdCb.SelectedValue.ToString() +")";
        //            //string Query = "INSERT INTO IncomeTbl (Date, Type, Amount, EmpId) VALUES ('" + Date.Value.Date + "', '" + Sales + "', '" + TotalTb.Text + "', '" + EmpIdCb.SelectedValue.ToString() + "')";

        //            SqlCommand cmd = new SqlCommand(Query, Con);
        //            cmd.ExecuteNonQuery();
        //            MessageBox.Show("Income Saved Successfully");

        //            Con.Close();
        //        }
        //        catch (Exception Ex)
        //        {
        //            MessageBox.Show(Ex.Message);
        //        }
        //    }
        //}
        private void SaveTransaction()
        {
            try
            {
                string Sales = "Sales";
                Con.Open();

                string Query = "INSERT INTO IncomeTbl (IncDate, IncPurpose, IncAmt, EmpId) " +
                               "VALUES (@IncDate, @IncPurpose, @IncAmt, @EmpId)";

                SqlCommand cmd = new SqlCommand(Query, Con);

                // Use parameters to prevent SQL injection
                cmd.Parameters.AddWithValue("@IncDate", Date.Value.Date);
                cmd.Parameters.AddWithValue("@IncPurpose", Sales);
                cmd.Parameters.AddWithValue("@IncAmt", TotalTb.Text);
                cmd.Parameters.AddWithValue("@EmpId", EmpIdCb.SelectedValue.ToString());

                cmd.ExecuteNonQuery();
                MessageBox.Show("Income Saved Successfully");

                Con.Close();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (EmpIdCb.SelectedIndex == -1 || PriceTb.Text == "" || ClientNameTb.Text == "" || PhoneTb.Text == "" || QuantityTb.Text == "" || TotalTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "INSERT INTO MilkSalesTable (EmpId, ClientName, ClientPhone, Date, Price, Quantity, TotalAmount) " +
                                   "VALUES (@EmpId, @ClientName, @ClientPhone, @Date, @Price, @Quantity, @TotalAmount)";

                    SqlCommand cmd = new SqlCommand(Query, Con);

                    // Use parameters to prevent SQL injection
                    cmd.Parameters.AddWithValue("@EmpId", EmpIdCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@ClientName", ClientNameTb.Text);
                    cmd.Parameters.AddWithValue("@ClientPhone", PhoneTb.Text);
                    cmd.Parameters.AddWithValue("@Date", Date.Text);
                    cmd.Parameters.AddWithValue("@Price", PriceTb.Text);
                    cmd.Parameters.AddWithValue("@Quantity", QuantityTb.Text);
                    cmd.Parameters.AddWithValue("@TotalAmount", TotalTb.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Milk Sales Data Saved Successfully");

                    Con.Close();
                    populate();
                    SaveTransaction();
                    Clear();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }
        int key = 0;
        private void SalesDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < SalesDGV.Rows.Count)
            {
                DataGridViewRow row = SalesDGV.Rows[e.RowIndex];

                EmpIdCb.SelectedValue = row.Cells["EmpId"].Value.ToString();
                Date.Text = row.Cells["Date"].Value.ToString();
                PriceTb.Text = row.Cells["Price"].Value.ToString();
                ClientNameTb.Text = row.Cells["ClientName"].Value.ToString();
                PhoneTb.Text = row.Cells["ClientPhone"].Value.ToString();
                QuantityTb.Text = row.Cells["Quantity"].Value.ToString();
                TotalTb.Text = row.Cells["TotalAmount"].Value.ToString();

                if (string.IsNullOrEmpty(ClientNameTb.Text))
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
    }
}

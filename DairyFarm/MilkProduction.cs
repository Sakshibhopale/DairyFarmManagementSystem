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
    public partial class MilkProduction : Form
    {
        public MilkProduction()
        {
            InitializeComponent();
            FillCowId();
            populate();
        }

        private void MilkProduction_Load(object sender, EventArgs e)
        {

        }


        private void label12_Click(object sender, EventArgs e)
        {
            Cows Ob = new Cows();
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

        private void label21_Click(object sender, EventArgs e)
        {
            Finance Ob = new Finance();
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
        private void FillCowId()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("select CowId from CowTbl", Con);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("CowId", typeof(int));
            dt.Load(Rdr);
            CowIdCb.ValueMember = "CowId";
            CowIdCb.DataSource = dt;

            Con.Close();
        }
        private void populate()
        {
            Con.Open();
            string query = "select * from MilkTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            MilkDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void Clear()
        {
            Cownametb.Text = "";
            Amtb.Text = "";
            noonTb.Text = "";
            PmTb.Text = "";
            TotalTb.Text = "";
            key = 0;
        }
        private void GetCowName()
        {
            Con.Open();
            string query = "select * from CowTbl where CowId = "+CowIdCb.SelectedValue.ToString()+"";
            SqlCommand cmd = new SqlCommand(query, Con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach(DataRow dr in dt.Rows)
            {
                Cownametb.Text = dr["CowName"].ToString();

            }
            Con.Close() ;
        }
        //private void button1_Click(object sender, EventArgs e)
        //{
        //    if (CowIdCb.SelectedIndex == -1 || Cownametb.Text == "" || Amtb.Text == "" || noonTb.Text == "" || PmTb.Text == "" || TotalTb.Text == "" )
        //    {
        //        MessageBox.Show("Missing Information");
        //    }
        //    else
        //    {
        //        try
        //        {
        //            Con.Open();
        //            string Query = "INSERT INTO MilkTbl (CowId, CowName, AmMilk, NoonMilk, PmMilk, TotalMilk, DateProd) VALUES ('" + CowIdCb.SelectedValue.ToString() + "', '" + Cownametb.Text + "', '" + Amtb.Text + "', '" + noonTb.Text + "', " + PmTb.Text + " , "+TotalTb.Text+", "+Date.Value.Date+")";

        //            SqlCommand cmd = new SqlCommand(Query, Con);
        //            cmd.ExecuteNonQuery();
        //            MessageBox.Show("Milk Data Saved Successfully");

        //            Con.Close();
        //            populate();
        //            Clear();
        //        }
        //        catch (Exception Ex)
        //        {
        //            MessageBox.Show(Ex.Message);
        //        }
        //    }
        //}
        private void button1_Click(object sender, EventArgs e)
        {
            if (CowIdCb.SelectedIndex == -1 || Cownametb.Text == "" || Amtb.Text == "" || noonTb.Text == "" || PmTb.Text == "" || TotalTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "INSERT INTO MilkTbl (CowId, CowName, AmMilk, NoonMilk, PmMilk, TotalMilk, DateProd) " +
                                   "VALUES (@CowId, @CowName, @AmMilk, @NoonMilk, @PmMilk, @TotalMilk, @DateProd)";

                    SqlCommand cmd = new SqlCommand(Query, Con);

                    // Use parameters to prevent SQL injection
                    cmd.Parameters.AddWithValue("@CowId", CowIdCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@CowName", Cownametb.Text);
                    cmd.Parameters.AddWithValue("@AmMilk", Amtb.Text);
                    cmd.Parameters.AddWithValue("@NoonMilk", noonTb.Text);
                    cmd.Parameters.AddWithValue("@PmMilk", PmTb.Text);
                    cmd.Parameters.AddWithValue("@TotalMilk", TotalTb.Text);
                    cmd.Parameters.AddWithValue("@DateProd", Date.Value.Date);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Milk Data Saved Successfully");

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

        private void button4_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void CowIdCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetCowName();
        }

      

        private void PmTb_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(Amtb.Text, out int amtValue) &&
               int.TryParse(noonTb.Text, out int noonValue) &&
               int.TryParse(PmTb.Text, out int pmValue))
            {
                // Perform the calculation and update TotalTb.
                int total = amtValue + noonValue + pmValue;
                TotalTb.Text = total.ToString();
            }
            else
            {
                // Handle parsing errors, e.g., display a message or set a default value.
                TotalTb.Text = "Invalid input";
            }
        }
        int key = 0;
        private void MilkDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < MilkDGV.Rows.Count)
            {
                DataGridViewRow row = MilkDGV.Rows[e.RowIndex];

                CowIdCb.SelectedValue = row.Cells["CowId"].Value.ToString();
                Cownametb.Text = row.Cells["CowName"].Value.ToString();
                Amtb.Text = row.Cells["AmMilk"].Value.ToString();
                noonTb.Text = row.Cells["NoonMilk"].Value.ToString();
                PmTb.Text = row.Cells["PmMilk"].Value.ToString();
                TotalTb.Text = row.Cells["TotalMilk"].Value.ToString();
                Date.Text = row.Cells["DateProd"].Value.ToString();

                if (string.IsNullOrEmpty(Cownametb.Text))
                {
                    key = 0;
                }
                else
                {
                    key = Convert.ToInt32(row.Cells["CowId"].Value);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Select the milk production to be deleted");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "delete from MilkTbl where CowId=" + key + ";";

                    SqlCommand cmd = new SqlCommand(Query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Milk Production Data Deleted Successfully");

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

        private void button3_Click(object sender, EventArgs e)
        {
            if (CowIdCb.Text == "" || Cownametb.Text == "" || Amtb.Text == "" || noonTb.Text == "" || PmTb.Text == "" || TotalTb.Text == "" )
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "update MilkTbl set CowId = '" + CowIdCb.Text + "' , CowName = '" + Cownametb.Text + "' , AmMilk = '" + Amtb.Text + "' , NoonMilk = '" + noonTb.Text + "', PmMilk = '" + PmTb.Text + "', TotalMilk = '" + TotalTb.Text + "', DateProd = '" + Date.Text + "' where CowId = " + key + "; ";

                    SqlCommand cmd = new SqlCommand(Query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Milk Production Data Updated Successfully");

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
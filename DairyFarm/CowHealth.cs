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
    public partial class CowHealth : Form
    {
        public CowHealth()
        {
            InitializeComponent();
            FillCowId();
            populate();
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
            string query = "select * from HealthTable";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            HealthDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void GetCowName()
        {
            Con.Open();
            string query = "select * from CowTbl where CowId = " + CowIdCb.SelectedValue.ToString() + "";
            SqlCommand cmd = new SqlCommand(query, Con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                CowNameTb.Text = dr["CowName"].ToString();

            }
            Con.Close();
        }
        private void CowHealth_Load(object sender, EventArgs e)
        {

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

        private void CowIdCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetCowName();
        }

        private void Clear()
        {
            CowNameTb.Text = "";
            EventTb.Text = "";
            CostTb.Text = "";
            DiagnosisTb.Text = "";
            TreatmentTb.Text = "";
            VetNameTb.Text = "";
            key = 0;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (CowIdCb.SelectedIndex == -1 || CowNameTb.Text == "" || EventTb.Text == "" || CostTb.Text == "" || DiagnosisTb.Text == "" || TreatmentTb.Text == "" || VetNameTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "INSERT INTO HealthTable (CowId, Cowname, RepDate, Event, Diagnosis, Treatment, Cost, VetName) " +
                                   "VALUES (@CowId, @CowName,@RepDate, @Event, @Diagnosis, @Treatment, @Cost, @VetName)";

                    SqlCommand cmd = new SqlCommand(Query, Con);

                    // Use parameters to prevent SQL injection
                    cmd.Parameters.AddWithValue("@CowId", CowIdCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@CowName", CowNameTb.Text);
                    cmd.Parameters.AddWithValue("@RepDate", RepDateTb.Value.Date);
                    cmd.Parameters.AddWithValue("@Event", EventTb.Text);
                    cmd.Parameters.AddWithValue("@Diagnosis", DiagnosisTb.Text);
                    cmd.Parameters.AddWithValue("@Treatment", TreatmentTb.Text);
                    cmd.Parameters.AddWithValue("@Cost", CostTb.Text);
                    cmd.Parameters.AddWithValue("@VetName", VetNameTb.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Health Issue Saved");

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
        int key = 0;

        private void HealthDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < HealthDGV.Rows.Count)
            {
                DataGridViewRow row = HealthDGV.Rows[e.RowIndex];

                CowIdCb.SelectedValue = row.Cells["CowId"].Value.ToString();
                CowNameTb.Text = row.Cells["CowName"].Value.ToString();
                RepDateTb.Text = row.Cells["RepDate"].Value.ToString();
                EventTb.Text = row.Cells["Event"].Value.ToString();
                DiagnosisTb.Text = row.Cells["Diagnosis"].Value.ToString();
                TreatmentTb.Text = row.Cells["Treatment"].Value.ToString();
                CostTb.Text = row.Cells["Cost"].Value.ToString();
                VetNameTb.Text = row.Cells["VetName"].Value.ToString();

                if (string.IsNullOrEmpty(CowNameTb.Text))
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
                MessageBox.Show("Select the Health Report to be deleted");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "delete from HealthTable where CowId=" + key + ";";

                    SqlCommand cmd = new SqlCommand(Query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Health Report Deleted Successfully.");

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
            if (CowIdCb.Text == "" || CowNameTb.Text == "" || EventTb.Text == "" || DiagnosisTb.Text == "" || TreatmentTb.Text == "" || CostTb.Text == "" || VetNameTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "update HealthTable set CowId = '" + CowIdCb.SelectedValue.ToString() + "' , CowName = '" + CowNameTb.Text + "' , RepDate = '" + RepDateTb.Text + "' , Event = '" + EventTb.Text + "', Diagnosis = '" + DiagnosisTb.Text + "', Treatment = '" + TreatmentTb.Text + "', Cost = '" + CostTb.Text + "', VetName = '" + VetNameTb.Text + "' where CowId = " + key + "; ";

                    SqlCommand cmd = new SqlCommand(Query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Report Updated Successfully");

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

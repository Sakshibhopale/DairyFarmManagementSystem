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
using System.Windows.Forms.Design;

namespace DairyFarm
{
    public partial class Breed : Form
    {
        public Breed()
        {
            InitializeComponent();
            populate();
            FillCowId();
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
            string query = "select * from BreedTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            BreedDGV.DataSource = ds.Tables[0];
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
                CowAgeTb.Text = dr["Age"].ToString();

            }
            Con.Close();
        }

        private void Breed_Load(object sender, EventArgs e)
        {

        }

        private void CowIdCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetCowName();
        }
        private void Clear()
        {
            CowNameTb.Text = "";
            RemarksTb.Text = "";
            CowAgeTb.Text = "";
            key = 0;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (CowIdCb.SelectedIndex == -1 || CowNameTb.Text == "" || CowAgeTb.Text == "" || RemarksTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "INSERT INTO BreedTbl (CowId, CowName,HeatDate,BreedDate, PregDate, ExpDateCalve, DateCalved, CowAge, Remarks) " +
                                   "VALUES (@CowId, @CowName,@HeatDate,@BreedDate, @PregDate, @ExpDateCalve, @DateCalved, @CowAge, @Remarks)";

                    SqlCommand cmd = new SqlCommand(Query, Con);

                    // Use parameters to prevent SQL injection
                    cmd.Parameters.AddWithValue("@CowId", CowIdCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@CowName", CowNameTb.Text);
                    cmd.Parameters.AddWithValue("@HeatDate", HeatDate.Value.Date);
                    cmd.Parameters.AddWithValue("@BreedDate", BreedDate.Value.Date);
                    cmd.Parameters.AddWithValue("@PregDate", PregDate.Value.Date);
                    cmd.Parameters.AddWithValue("@ExpDateCalve", ExpDate.Value.Date);
                    cmd.Parameters.AddWithValue("@DateCalved", DateCalved.Value.Date);
                    cmd.Parameters.AddWithValue("@CowAge", CowAgeTb.Text);
                    cmd.Parameters.AddWithValue("@Remarks", RemarksTb.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Breeding Report Saved");

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
        private void BreedDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < BreedDGV.Rows.Count)
            {
                DataGridViewRow row = BreedDGV.Rows[e.RowIndex];

                CowIdCb.SelectedValue = row.Cells["CowId"].Value.ToString();
                CowNameTb.Text = row.Cells["CowName"].Value.ToString();
                HeatDate.Text = row.Cells["HeatDate"].Value.ToString();
                BreedDate.Text = row.Cells["BreedDate"].Value.ToString();
                PregDate.Text = row.Cells["PregDate"].Value.ToString();
                ExpDate.Text = row.Cells["ExpDateCalve"].Value.ToString();
                DateCalved.Text = row.Cells["DateCalved"].Value.ToString();
                CowAgeTb.Text = row.Cells["CowAge"].Value.ToString();
                RemarksTb.Text = row.Cells["Remarks"].Value.ToString();

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
                MessageBox.Show("Select the Breeding Report to be deleted");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "delete from BreedTbl where CowId=" + key + ";";

                    SqlCommand cmd = new SqlCommand(Query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Breeding Report Deleted Successfully.");

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
            if (CowIdCb.Text == "" || CowNameTb.Text == "" || CowAgeTb.Text == "" || RemarksTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "update BreedTbl set CowId = '" + CowIdCb.SelectedValue.ToString() + "' , CowName = '" + CowNameTb.Text + "' , HeatDate = '" + HeatDate.Text + "' , BreedDate = '" + BreedDate.Text + "', PregDate = '" + PregDate.Text + "', ExpDateCalve = '" + ExpDate.Text + "', DateCalved = '" + DateCalved.Text + "', CowAge = '" + CowAgeTb.Text + "', Remarks = '" + RemarksTb.Text + "' where CowId = " + key + "; ";

                    SqlCommand cmd = new SqlCommand(Query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Breeding Report Updated Successfully");

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

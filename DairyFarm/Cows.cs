using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DairyFarm
{
    public partial class Cows : Form
    {
        public Cows()
        {
            InitializeComponent();
            populate();
        }
        
        SqlConnection Con = new SqlConnection(@"Data Source=LAPTOP-Q05C0DKC\SQLEXPRESS01;Initial Catalog=DairyFarmDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");

        
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

        private void label21_Click(object sender, EventArgs e)
        {
            Finance Ob = new Finance();
            Ob.Show();
            this.Hide();
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }
        private void populate()
        {
            Con.Open();
            string query = "select * from Cowtbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            CowsDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        public  void Clear()
        {
            CowNameTb.Text = "";
            EarTagTb.Text = "";
            ColorTb.Text = "";
            BreedTb.Text = "";
            WeigthTb.Text = "";
            AgeTb.Text = "";
            PastureTb.Text = "";
            key = 0;
        }

        int age = 0;
        private void button1_Click(object sender, EventArgs e)
        {

            if (CowNameTb.Text == "" || EarTagTb.Text == "" || ColorTb.Text == "" || BreedTb.Text == "" || WeigthTb.Text == "" || AgeTb.Text == "" || PastureTb.Text == "" )
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "INSERT INTO CowTbl (CowName, EarTag, Color, Breed, Age, WeightAtBirth, Pasture) VALUES ('" + CowNameTb.Text + "', '" + EarTagTb.Text + "', '" + ColorTb.Text + "', '" + BreedTb.Text + "', " + age + ", " + WeigthTb.Text + ", '" + PastureTb.Text + "')";

                    SqlCommand cmd = new SqlCommand(Query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Cow Data Saved Successfully");

                    Con.Close();
                    populate();
                    Clear();
                }catch(Exception Ex)
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
        private void CowsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < CowsDGV.Rows.Count)
            {
                DataGridViewRow row = CowsDGV.Rows[e.RowIndex];

                CowNameTb.Text = row.Cells["CowName"].Value.ToString();
                EarTagTb.Text = row.Cells["EarTag"].Value.ToString();
                ColorTb.Text = row.Cells["Color"].Value.ToString();
                BreedTb.Text = row.Cells["Breed"].Value.ToString();
                WeigthTb.Text = row.Cells["WeightAtBirth"].Value.ToString();
                PastureTb.Text = row.Cells["Pasture"].Value.ToString();

                if (string.IsNullOrEmpty(CowNameTb.Text))
                {
                    key = 0;
                    age = 0;
                }
                else
                {
                    key = Convert.ToInt32(row.Cells["CowId"].Value);
                    age = Convert.ToInt32(row.Cells["Age"].Value);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Select the cow to be deleted");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "delete from CowTbl where CowId="+key+";";

                    SqlCommand cmd = new SqlCommand(Query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Cow Data Deleted Successfully");

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
            if (CowNameTb.Text == "" || EarTagTb.Text == "" || ColorTb.Text == "" || BreedTb.Text == "" || WeigthTb.Text == "" || AgeTb.Text == "" || PastureTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "update CowTbl set CowName = '"+CowNameTb.Text+"' , EarTag = '"+ EarTagTb.Text+ "' , Color = '"+ColorTb.Text+"' , Breed = '"+ BreedTb.Text+"', Age = '"+AgeTb.Text+"', WeightAtBirth = '"+WeigthTb.Text+"', Pasture = '"+PastureTb.Text+"' where CowId = "+key+"; ";

                    SqlCommand cmd = new SqlCommand(Query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Cow Data Updated Successfully");

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

        private void DOBDate_ValueChanged_1(object sender, EventArgs e)
        {
            age = Convert.ToInt32((DateTime.Today.Date - DOBDate.Value.Date).Days) / 365;
            MessageBox.Show("" + age);
        }

        private void DOBDate_MouseEnter(object sender, EventArgs e)
        {
            age = Convert.ToInt32((DateTime.Today.Date - DOBDate.Value.Date).Days) / 365;
            AgeTb.Text = "" + age;
        }

        private void Cows_Load(object sender, EventArgs e)
        {

        }

        private void SearchCow()
        {
            Con.Open();
            string query = "select * from CowTbl where CowName like '%" +SearchBoxTb.Text+  "%' ";
            SqlDataAdapter sda = new SqlDataAdapter(query , Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            CowsDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void SearchBoxTb_TextChanged(object sender, EventArgs e)
        {
            SearchCow();
        }
    }
}

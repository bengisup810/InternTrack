using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace InternTrack
{
    public partial class Form1 : Form
    {
        string baglanti = "Server=localhost;Database=interntrack;Uid=root;Pwd=;";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            Listele();
           
            dataGridView1.Columns["id"].HeaderText = "No";
            dataGridView1.Columns["firma_adi"].HeaderText = "Firma Adı";
            dataGridView1.Columns["pozisyon"].HeaderText = "Pozisyon";
            dataGridView1.Columns["basvuru_tarihi"].HeaderText = "Başvuru Tarihi";
            dataGridView1.Columns["durum"].HeaderText = "Durum";
            dataGridView1.Columns["notlar"].HeaderText = "Notlar";
            dataGridView1.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void Listele()
        {
            using (MySqlConnection con = new MySqlConnection(baglanti))
            {
                con.Open();

                string sorgu = "SELECT * FROM staj_basvurulari";

                MySqlDataAdapter da = new MySqlDataAdapter(sorgu, con);
                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }
        }

        private void txtFirmaAra_TextChanged(object sender, EventArgs e)
        {
          
            using (MySqlConnection con = new MySqlConnection(baglanti))
            {
                con.Open();

                string sorgu = @"SELECT * FROM staj_basvurulari
                         WHERE firma_adi LIKE @firma";

                MySqlDataAdapter da = new MySqlDataAdapter(sorgu, con);
                da.SelectCommand.Parameters.AddWithValue("@firma", "%" + txtFirmaAra.Text + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }
        }

        private void cmbDurumFiltre_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            using (MySqlConnection con = new MySqlConnection(baglanti))
            {
                con.Open();

                string sorgu;

                if (cmbDurumFiltre.Text == "Tümü")
                {
                    sorgu = "SELECT * FROM staj_basvurulari";
                }
                else
                {
                    sorgu = @"SELECT * FROM staj_basvurulari
                      WHERE durum = @durum";
                }

                MySqlDataAdapter da = new MySqlDataAdapter(sorgu, con);

                if (cmbDurumFiltre.Text != "Tümü")
                {
                    da.SelectCommand.Parameters.AddWithValue(
                        "@durum",
                        cmbDurumFiltre.Text
                    );
                }

                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
           
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen silmek istediğiniz başvuruyu seçin.");
                return;
            }

            int id = Convert.ToInt32(
                dataGridView1.SelectedRows[0].Cells["id"].Value
            );

            DialogResult cevap = MessageBox.Show(
                "Bu başvuruyu silmek istediğinize emin misiniz?",
                "Silme Onayı",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (cevap == DialogResult.Yes)
            {
                using (MySqlConnection con = new MySqlConnection(baglanti))
                {
                    con.Open();

                    string sorgu =
                        "DELETE FROM staj_basvurulari WHERE id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(sorgu, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Başvuru silindi.");
                Listele();
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen güncellemek istediğiniz başvuruyu seçin.");
                return;
            }

            int id = Convert.ToInt32(
                dataGridView1.SelectedRows[0].Cells["id"].Value
            );

            Form2 form2 = new Form2(id);
            form2.ShowDialog();

            Listele();
        }
    }
    
    
    
    }

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
    public partial class Form2 : Form
    {
        string baglanti = "Server=localhost;Database=interntrack;Uid=root;Pwd=;";
        int guncellenecekId = 0;

        public Form2(int id)
        {
            InitializeComponent();
        
            guncellenecekId = id;

            using (MySqlConnection con = new MySqlConnection(baglanti))
            {
                con.Open();

                string sorgu = "SELECT * FROM staj_basvurulari WHERE id = @id";

                using (MySqlCommand cmd = new MySqlCommand(sorgu, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            txtFirmaAdi.Text = dr["firma_adi"].ToString();
                            txtPozisyon.Text = dr["pozisyon"].ToString();
                            dtpBasvuruTarihi.Value =
                                Convert.ToDateTime(dr["basvuru_tarihi"]);
                            cmbDurum.Text = dr["durum"].ToString();
                            txtNotlar.Text = dr["notlar"].ToString();
                        }
                    }
                }
            }
        
        }
        

        private void btnKaydet_Click(object sender, EventArgs e)
        {
           
            using (MySqlConnection con = new MySqlConnection(baglanti))
            {
                con.Open();

                string sorgu;

                if (guncellenecekId == 0)
                {
                    
                    sorgu = @"INSERT INTO staj_basvurulari
                      (firma_adi, pozisyon, basvuru_tarihi, durum, notlar)
                      VALUES
                      (@firma, @pozisyon, @tarih, @durum, @notlar)";
                }
                else
                {
                    
                    sorgu = @"UPDATE staj_basvurulari SET
                      firma_adi = @firma,
                      pozisyon = @pozisyon,
                      basvuru_tarihi = @tarih,
                      durum = @durum,
                      notlar = @notlar
                      WHERE id = @id";
                }

                using (MySqlCommand cmd = new MySqlCommand(sorgu, con))
                {
                    cmd.Parameters.AddWithValue("@firma", txtFirmaAdi.Text);
                    cmd.Parameters.AddWithValue("@pozisyon", txtPozisyon.Text);
                    cmd.Parameters.AddWithValue("@tarih", dtpBasvuruTarihi.Value);
                    cmd.Parameters.AddWithValue("@durum", cmbDurum.Text);
                    cmd.Parameters.AddWithValue("@notlar", txtNotlar.Text);

                    if (guncellenecekId != 0)
                    {
                        cmd.Parameters.AddWithValue("@id", guncellenecekId);
                    }

                    cmd.ExecuteNonQuery();
                }
            }

            if (guncellenecekId == 0)
            {
                MessageBox.Show("Başvuru başarıyla eklendi!");
            }
            else
            {
                MessageBox.Show("Başvuru başarıyla güncellendi!");
            }

            this.Close();
        }


    }
    
    
}

using StokTakip.Data;
using StokTakip.Models;
using System.Data;
using System.Data.SqlClient;

namespace StokTakip.Services
{
    public class KullaniciServisi
    {
        private DataConnection dbConnection;

        public KullaniciServisi(DataConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public void KullaniciEkle(Kullanici kullanici)
        {
            try
            {

                dbConnection.Open();
                SqlCommand checkCommand = new SqlCommand("SELECT Count(*) FROM Kullanici WHERE KullaniciAdi=@kullanici", dbConnection.GetConnection());
                checkCommand.Parameters.AddWithValue("@kullanici", kullanici.KullaniciAdi);
                int count = (int)checkCommand.ExecuteScalar();
                if (count > 0)
                {
                    Console.WriteLine("Bu kişi kodu zaten mevcut. Kişi eklenemedi.");
                }
                else
                {
                    dbConnection.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO Kullanici (Isim, Soyisim, Sifre, KullaniciAdi, Mail,KullaniciTuru) VALUES (@isim, @soyisim, @sifre, @kullaniciAdi, @mail, @tur)", dbConnection.GetConnection());

                    command.Parameters.AddWithValue("@isim", kullanici.Isim);
                    command.Parameters.AddWithValue("@soyisim", kullanici.Soyisim);
                    command.Parameters.AddWithValue("@sifre", kullanici.Sifre);
                    command.Parameters.AddWithValue("@kullaniciAdi", kullanici.KullaniciAdi);
                    command.Parameters.AddWithValue("@mail", kullanici.Mail);
                    command.Parameters.AddWithValue("@tur", kullanici.KullaniciTuru);
                    command.ExecuteNonQuery();
                    Console.WriteLine("KİŞİ EKLEME BAŞARILI.");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Hatası: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {                
                Console.WriteLine("Hata oluştu: " + ex.Message);
                throw;
            }
            finally
            {    
                dbConnection.Close();

            }
        }
        public bool GirisYap(string kullaniciAdi, string sifre)
        {
            try
            {
                using (SqlConnection connection = dbConnection.GetConnection())
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Kullanici WHERE KullaniciAdi=@kullaniciAdi AND Sifre=@sifre", connection))
                    {
                        command.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                        command.Parameters.AddWithValue("@sifre", sifre);

                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
                return false;
            }
        }
    }
}

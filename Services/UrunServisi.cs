using StokTakip.Data;
using StokTakip.Models;
using System.Data;
using System.Data.SqlClient;
using static StokTakip.Controllers.HomeController;
using static StokTakip.Controllers.UrunlerController;

namespace StokTakip.Services
{
    public class UrunServisi
    {
        private DataConnection dbConnection;

        public UrunServisi(DataConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }


        public void UrunEkle(Urunler urun)
        {
            try
            {
                dbConnection.Open();


                using (SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM Urunler WHERE UrunKodu = @UrunKodu", dbConnection.GetConnection()))
                {
                    checkCommand.Parameters.AddWithValue("@UrunKodu", urun.UrunKodu);
                    int count = (int)checkCommand.ExecuteScalar();

                    if (count > 0)
                    {
                        Console.WriteLine("Bu ürün kodu zaten mevcut. Ürün eklenemedi.");
                        return;
                    }
                }


                using (SqlCommand command = new SqlCommand("INSERT INTO Urunler (UrunKodu, UrunAdi, Aciklama, Renk, Uzunluk) VALUES (@UrunKodu, @UrunAdi, @Aciklama, @Renk, @Uzunluk)", dbConnection.GetConnection()))
                {
                    command.Parameters.AddWithValue("@UrunKodu", urun.UrunKodu ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@UrunAdi", urun.UrunAdi ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Aciklama", urun.Aciklama ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Renk", urun.Renk ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Uzunluk", urun.Uzunluk);

                    command.ExecuteNonQuery();
                }


                using (SqlCommand checkStockCommand = new SqlCommand("SELECT COUNT(*) FROM Stoklar WHERE StokKodu = @StokKodu", dbConnection.GetConnection()))
                {
                    checkStockCommand.Parameters.AddWithValue("@StokKodu", urun.StokKodu);
                    int stockCount = (int)checkStockCommand.ExecuteScalar();

                    if (stockCount > 0)
                    {
                        Console.WriteLine("Bu stok kodu zaten mevcut. Stok bilgisi güncellenemedi.");
                        return;
                    }
                }


                using (SqlCommand stockCommand = new SqlCommand("INSERT INTO Stoklar (StokKodu, UrunKodu, StokAdeti, SonGuncellemeTarihi, IlkEklemeTarihi) VALUES (@StokKodu, @UrunKodu, @StokAdeti, @SonGuncellemeTarihi, @IlkEklemeTarihi)", dbConnection.GetConnection()))
                {
                    stockCommand.Parameters.AddWithValue("@StokKodu", urun.StokKodu ?? (object)DBNull.Value);
                    stockCommand.Parameters.AddWithValue("@UrunKodu", urun.UrunKodu ?? (object)DBNull.Value);
                    stockCommand.Parameters.AddWithValue("@StokAdeti", urun.StokAdeti);
                    stockCommand.Parameters.AddWithValue("@SonGuncellemeTarihi", DateTime.Now);
                    stockCommand.Parameters.AddWithValue("@IlkEklemeTarihi", DateTime.Now);

                    stockCommand.ExecuteNonQuery();
                }

                Console.WriteLine("ÜRÜN EKLEME BAŞARILI.");

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
        }
        public void UpdateProduct(string productCode, string name, string color, decimal length, int quantity)
        {
            try
            {
                dbConnection.Open();


                using (var updateProductCommand = new SqlCommand("UPDATE Urunler SET UrunAdi = @Name, Renk = @Color, Uzunluk = @Length WHERE UrunKodu = @ProductCode", dbConnection.GetConnection()))
                {
                    updateProductCommand.Parameters.AddWithValue("@ProductCode", productCode);
                    updateProductCommand.Parameters.AddWithValue("@Name", name);
                    updateProductCommand.Parameters.AddWithValue("@Color", color);
                    updateProductCommand.Parameters.AddWithValue("@Length", length);


                    updateProductCommand.ExecuteNonQuery();
                }


                using (var updateStockCommand = new SqlCommand("UPDATE Stoklar SET StokAdeti = @Quantity, SonGuncellemeTarihi = @UpdateDate WHERE UrunKodu = @ProductCode", dbConnection.GetConnection()))
                {
                    updateStockCommand.Parameters.AddWithValue("@ProductCode", productCode);
                    updateStockCommand.Parameters.AddWithValue("@Quantity", quantity);
                    updateStockCommand.Parameters.AddWithValue("@UpdateDate", DateTime.Now);

                    updateStockCommand.ExecuteNonQuery();
                }
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public void UrunSil(string urunKodu)
        {
            try
            {

                dbConnection.Open();


                SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM Urunler WHERE UrunKodu = @urunKodu", dbConnection.GetConnection());
                checkCommand.Parameters.AddWithValue("@urunKodu", urunKodu);
                int count = (int)checkCommand.ExecuteScalar();

                if (count == 0)
                {
                    Console.WriteLine("Ürün bulunamadı.");
                }
                else
                {

                    SqlCommand checkStokCommand = new SqlCommand("SELECT StokAdeti FROM Stoklar WHERE UrunKodu = @urunKodu", dbConnection.GetConnection());
                    checkStokCommand.Parameters.AddWithValue("@urunKodu", urunKodu);
                    int stokAdeti = (int?)checkStokCommand.ExecuteScalar() ?? 0;

                    if (stokAdeti > 0)
                    {

                        Console.WriteLine("Ürün stok adeti 0'dan büyük olduğu için silinemez.");

                    }
                    else
                    {

                        SqlCommand deleteUrunCommand = new SqlCommand("DELETE FROM Urunler WHERE UrunKodu = @urunKodu", dbConnection.GetConnection());
                        deleteUrunCommand.Parameters.AddWithValue("@urunKodu", urunKodu);
                        deleteUrunCommand.ExecuteNonQuery();

                        SqlCommand deleteStokCommand = new SqlCommand("DELETE FROM Stoklar WHERE UrunKodu = @urunKodu", dbConnection.GetConnection());
                        deleteStokCommand.Parameters.AddWithValue("@urunKodu", urunKodu);
                        deleteStokCommand.ExecuteNonQuery();

                        Console.WriteLine("ÜRÜN SİLME İŞLEMİ BAŞARILI.");
                    }
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
        public ProductDetailViewModel GetProductDetailViewModel(string productCode)
        {
            ProductDetailViewModel viewModel = null;

            try
            {
                dbConnection.Open();
                using (var command = new SqlCommand("SELECT u.UrunKodu, u.UrunAdi, u.Renk, u.Uzunluk, s.StokAdeti, s.IlkEklemeTarihi, s.SonGuncellemeTarihi FROM Urunler u INNER JOIN Stoklar s ON u.UrunKodu = s.UrunKodu WHERE u.UrunKodu = @ProductCode", dbConnection.GetConnection()))
                {
                    command.Parameters.AddWithValue("@ProductCode", productCode);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            viewModel = new ProductDetailViewModel
                            {
                                ProductCode = reader["UrunKodu"].ToString(),
                                Name = reader["UrunAdi"].ToString(),
                                Color = reader["Renk"].ToString(),
                                Length = Convert.ToDecimal(reader["Uzunluk"]),
                                Quantity = (int)reader["StokAdeti"],
                                CreateDate = (DateTime)reader["IlkEklemeTarihi"],
                                UpdateDate = (DateTime)reader["SonGuncellemeTarihi"]
                            };
                        }
                    }
                }
                

            }
            finally
            {
                dbConnection.Close();
            }

            return viewModel;
        }
        public List<ProductDetailViewModel> GetProductList()
        {
            List<ProductDetailViewModel> products = new List<ProductDetailViewModel>();

            try
            {
                dbConnection.Open();
                using (var command = new SqlCommand("SELECT u.UrunKodu, u.UrunAdi, u.Renk, u.Uzunluk, s.StokAdeti, s.IlkEklemeTarihi, s.SonGuncellemeTarihi FROM Urunler u INNER JOIN Stoklar s ON u.UrunKodu = s.UrunKodu", dbConnection.GetConnection()))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new ProductDetailViewModel
                            {
                                ProductCode = reader["UrunKodu"].ToString(),
                                Name = reader["UrunAdi"].ToString(),
                                Length = Convert.ToDecimal(reader["Uzunluk"]),
                                Color = reader["Renk"].ToString(),
                                Quantity = (int)reader["StokAdeti"],
                                CreateDate = (DateTime)reader["IlkEklemeTarihi"],
                                UpdateDate = (DateTime)reader["SonGuncellemeTarihi"]
                            });
                        }
                    }
                }
            }
            finally
            {
                dbConnection.Close();
            }

            return products;
        }
        public List<ProductDetailViewModel> GetProductStocks()
        {
            List<ProductDetailViewModel> products = new List<ProductDetailViewModel>();

            try
            {
                dbConnection.Open();
                using (var command = new SqlCommand("select u.UrunAdi,s.StokAdeti from Stoklar s inner join Urunler u on s.UrunKodu=u.UrunKodu", dbConnection.GetConnection()))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new ProductDetailViewModel
                            {
                                Name = reader["UrunAdi"].ToString(),
                                Quantity = (int)reader["StokAdeti"]
                            });
                        }
                    }
                }
            }
            finally
            {
                dbConnection.Close();
            }
            return products;
        }

        public class ProductDetailViewModel
        {
            public string ProductCode { get; set; }
            public string Name { get; set; }
            public string Color { get; set; }
            public decimal Length { get; set; }
            public int Quantity { get; set; }
            public DateTime CreateDate { get; set; }
            public DateTime UpdateDate { get; set; }
        }
    }
}
   
    


    



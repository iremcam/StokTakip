namespace StokTakip.Models
{
    public class Stoklar
    {
        public int Id { get; set; }
        public string StokKodu { get; set; }
        public string UrunKodu { get; set; }
        public int StokAdeti { get; set; }
        public DateTime SonGuncellemeTarihi { get; set; }
        public DateTime IlkEklemeTarihi { get; set; }
        public string Ekleyen { get; set; }

        //tablo ilişkileri
        public virtual Urunler Urun { get; set; }
        public virtual Kullanici Kullanici { get; set; }
    }
}

namespace StokTakip.Models
{
    public class Kullanici
    {
        public int Id { get; set; }
        public string KullaniciAdi { get; set; }
        public string Isim { get; set; }
        public string Soyisim { get; set; }
        public string Mail { get; set; }   
        public string Sifre { get; set; }
        public KullaniciTuru KullaniciTuru { get; set; }

        //Tablo ilişkileri
        public virtual ICollection<Stoklar> Stoklar { get; set; }
    }
}

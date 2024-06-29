namespace StokTakip.Models
{
    public class Urunler
    {
        public int Id { get; set; }
        public string UrunKodu { get; set; }
        public string UrunAdi { get; set; }
        public string Aciklama { get; set; }
        public string Renk { get; set; }
        public decimal Uzunluk { get; set; }
        public int StokAdeti { get; set; }
        public string StokKodu { get; set; }

        //tablo ilişkileri
        public virtual ICollection<Stoklar> Stoklar { get; set; }
    }
}

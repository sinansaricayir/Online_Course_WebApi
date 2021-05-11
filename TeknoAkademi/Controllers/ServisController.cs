using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeknoAkademi.Models;
using TeknoAkademi.ViewModel;

namespace TeknoAkademi.Controllers
{
    public class ServisController : ApiController
    {
        TeknoAkademiDbEntities db = new TeknoAkademiDbEntities();
        SonucModel sonuc = new SonucModel();

        #region Kategori

        [HttpGet]
        [Route("api/kategoriliste")]
        public List<KategoriModel> KategoriListe()
        {
            List<KategoriModel> liste = db.Kategori.Select(x => new KategoriModel()
            {
                kategoriId = x.kategoriId,
                kategoriAd = x.kategoriAd
            }).ToList();
            return liste;
        }
       


        [HttpPost]
        [Route("api/kategoriekle")]
        public SonucModel KategoriEkle(KategoriModel model)
        {
            if (db.Kategori.Count(s => s.kategoriAd == model.kategoriAd) > 0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Kategori bilgileri sistemde mevcuttur";
                return sonuc;
            }
            Kategori yeni = new Kategori();
            yeni.kategoriAd = model.kategoriAd;
            db.Kategori.Add(yeni);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Kategori bilgileri sisteme eklenmiştir.";
            return sonuc;
        }

        [HttpPut]
        [Route("api/kategoriduzenle")]

        public SonucModel KategoriDuzenle(KategoriModel model)
        {
            Kategori kayit = db.Kategori.Where(s => s.kategoriId == model.kategoriId).FirstOrDefault(); if (kayit == null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Kategori bilgileri bulunamadı. Lütfen geçerli bir kategori seçiniz.";
                return sonuc;
            }
            kayit.kategoriAd = model.kategoriAd;
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Kategori bilgileri başarılı bir şekilde düzenlenmiştir.";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/kategorisil/{kategoriid}")]
        public SonucModel KategoriSil(int kategoriId)
        {
            Kategori kayit = db.Kategori.Where(s => s.kategoriId == kategoriId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Kategori bilgileri bulunamadı.";
                return sonuc;
            }
            if (db.Kurslar.Count(s => s.kursKategoriId == kategoriId) > 0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "İçerisinde kurs kaydı bulunan kategori silinemez. Lütfen kurs bilgilerini siliniz.";
                return sonuc;
            }
            db.Kategori.Remove(kayit);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Kategori bilgileri başarılı bir şekilde silindi.";
            return sonuc;
        }

        #endregion


        #region Kurs

        [HttpGet]
        [Route("api/kursliste")]
        public List<KurslarModel> KursListe()
        {
            List<KurslarModel> liste = db.Kurslar.Select(x => new KurslarModel()
            {
                kursId = x.kursId,
                kursAd = x.kursAd,
                kursFiyat = x.kursFiyat,
                kursIcerik = x.kursIcerik,
                kursKategoriId = x.kursKategoriId,
                kursEgitmenId = x.kursEgitmenId,

            }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/kurslistebykatid/{kategoriId}")]
        public List<KurslarModel> KursListeByKatId(int kategoriId)
        {
            List<KurslarModel> liste = db.Kurslar.Where(s => s.kursKategoriId == kategoriId).Select(x => new KurslarModel()
            {
                kursId = x.kursId,
                kursAd = x.kursAd,
                kursFiyat = x.kursFiyat,
                kursIcerik = x.kursIcerik,
                kursKategoriId = x.kursKategoriId,
                kursEgitmenId = x.kursEgitmenId,
            }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/kategoribyid/{kursId}")]
        public KurslarModel KursById(int kursId)
        {
            KurslarModel kayit = db.Kurslar.Where(s => s.kursId == kursId).Select(x => new KurslarModel()
            {
                kursId = x.kursId,
                kursAd = x.kursAd,
                kursFiyat = x.kursFiyat,
                kursIcerik = x.kursIcerik,
                kursKategoriId = x.kursKategoriId,
                kursEgitmenId = x.kursEgitmenId,
            }
            ).FirstOrDefault();
            return kayit;
        }

        [HttpPost]
        [Route("api/kursekle")]
        public SonucModel KursEkle(KurslarModel model)
        {
            if (db.Kurslar.Count(s => s.kursIcerik == model.kursIcerik) > 0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Kurs ilgili kategoride mevcuttur.";
                return sonuc;
            }
            Kurslar yeni = new Kurslar();
            yeni.kursAd = model.kursAd;
            yeni.kursFiyat = model.kursFiyat;
            yeni.kursIcerik = model.kursIcerik;
            yeni.kursKategoriId = model.kursKategoriId;
            yeni.kursEgitmenId = model.kursEgitmenId;
            db.Kurslar.Add(yeni);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Kurs bilgileri başarılı bir şekilde sisteme eklenmiştir.";
            return sonuc;
        }





        [HttpPut]
        [Route("api/kursduzenle")]
        public SonucModel KursDuzenle(KurslarModel model)
        {
            Kurslar kayit = db.Kurslar.Where(s => s.kursId == model.kursId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Girdiğiniz bilgilere ait kurs bulunamadı.";
                return sonuc;
            }
            kayit.kursAd = model.kursAd;
            kayit.kursFiyat = model.kursFiyat;
            kayit.kursIcerik = model.kursIcerik;
            kayit.kursKategoriId = model.kursKategoriId;
            kayit.kursEgitmenId = model.kursEgitmenId;
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Kurs bilgileri başarılı bir şekilde güncellenmiştir.";
            return sonuc;
        }
        [HttpDelete]
        [Route("api/kurssil/{kursId}")]
        public SonucModel KursSil(int kursId)
        {
            Kurslar kayit = db.Kurslar.Where(s => s.kursId == kursId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Kurs bulunamadı.";
                return sonuc;
            }
            db.Kurslar.Remove(kayit);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Kurs bilgileri başarı ile silinmiştir.";
            return sonuc;
        }
        #endregion

        #region Egitmen

        [HttpGet]
        [Route("api/egitmenliste")]
        public List<EgitmenModel> EgitmenListe()
        {
            List<EgitmenModel> liste = db.Egitmen.Select(x => new EgitmenModel()
            {
                egitmenId = x.egitmenId,
                egitmenAd = x.egitmenAd,
                egitmenSoyad = x.egitmenSoyad,
                egitmenBrans = x.egitmenBrans,
                egitmenOzgecmis = x.egitmenOzgecmis,
                egitmenFotograf = x.egitmenFotograf,

            }).ToList();
            return liste;
        }


        [HttpPost]
        [Route("api/egitmenekle")]
        public SonucModel EgitmenEkle(EgitmenModel model)
        {

            Egitmen yeni = new Egitmen();
            yeni.egitmenAd = model.egitmenAd;
            yeni.egitmenSoyad = model.egitmenSoyad;
            yeni.egitmenBrans = model.egitmenBrans;
            yeni.egitmenOzgecmis = model.egitmenOzgecmis;
            yeni.egitmenFotograf = model.egitmenFotograf;
            db.Egitmen.Add(yeni);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Eğitmen bilgileri başarılı bir şekilde sisteme eklenmiştir.";
            return sonuc;
        }





        [HttpPut]
        [Route("api/egitmenduzenle")]
        public SonucModel EgitmenDuzenle(EgitmenModel model)
        {
            Egitmen kayit = db.Egitmen.Where(s => s.egitmenId == model.egitmenId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Girdiğiniz bilgilere ait eğitmen bulunamadı.";
                return sonuc;
            }
            kayit.egitmenAd = model.egitmenAd;
            kayit.egitmenSoyad = model.egitmenSoyad;
            kayit.egitmenBrans = model.egitmenBrans;
            kayit.egitmenOzgecmis = model.egitmenOzgecmis;
            kayit.egitmenFotograf = model.egitmenFotograf;
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Eğitmen bilgileri başarılı bir şekilde güncellenmiştir.";
            return sonuc;
        }
        [HttpDelete]
        [Route("api/egitmensil/{egitmenId}")]
        public SonucModel EgitmenSil(int egitmenId)
        {
            Egitmen kayit = db.Egitmen.Where(s => s.egitmenId == egitmenId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Eğitmen bulunamadı.";
                return sonuc;
            }
            db.Egitmen.Remove(kayit);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Eğitmen bilgileri sistemden silinmiştir.";
            return sonuc;
        }


        [HttpPost]
        [Route("api/egitmenfotoguncelle")]
        public SonucModel EgitmenFotoGuncelle(EgitmenFotoModel model)
        {
            Egitmen egt = db.Egitmen.Where(s => s.egitmenId == model.egitmenId).SingleOrDefault();
            if (egt == null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Kayıt bulunamadı.";
                return sonuc;
            }

            if (egt.egitmenFotograf != "default.jpg")
            {
                string yol = System.Web.Hosting.HostingEnvironment.MapPath("~/Dosyalar/" + egt.egitmenFotograf);
                if (File.Exists(yol))
                {
                    File.Delete(yol);
                }
            }
            string data = model.gorselData;
            string base64 = data.Substring(data.IndexOf(',') + 1);
            base64 = base64.Trim('\0');
            byte[] imgbyte = Convert.FromBase64String(base64);
            string dosyaAdi = egt.egitmenId + model.gorselUzanti.Replace("image/", ".");

            using (var ms = new MemoryStream(imgbyte, 0, imgbyte.Length))
            {
                Image img = Image.FromStream(ms, true);
                img.Save(System.Web.Hosting.HostingEnvironment.MapPath("~/Dosyalar/" + dosyaAdi));
            }
            egt.egitmenFotograf = dosyaAdi;
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Fotoğraf güncellendi.";
            return sonuc;
        }

        #endregion

        #region Üye
        [HttpGet]
        [Route("api/uyeliste")]
        public List<UyeModel> UyeListe()
        {
            List<UyeModel> liste = db.Uye.Select(x => new UyeModel()
            {
                uyeId = x.uyeId,
                uyeAd = x.uyeAd,
                uyeSoyad=x.uyeSoyad,
                uyeTel=x.uyeTel,
                uyeMail=x.uyeMail,
                uyeParola=x.uyeParola,
                uyeYetki=x.uyeYetki
            }).ToList();
            return liste;
        }

        [HttpGet]
        [Route("api/uyebyid/{uyeId}")]
        public UyeModel UyeById(int uyeId)
        {
            UyeModel kayit = db.Uye.Where(s => s.uyeId == uyeId).Select(x => new UyeModel()
            {
                uyeId = x.uyeId,
                uyeAd = x.uyeAd,
                uyeSoyad = x.uyeSoyad,
                uyeTel = x.uyeTel,
                uyeMail = x.uyeMail,
                uyeParola = x.uyeParola,
                uyeYetki = x.uyeYetki
            }).FirstOrDefault();
            return kayit;
        }


        [HttpPost]
        [Route("api/uyeekle")]
        public SonucModel UyeEkle(UyeModel model)
        {
            if (db.Uye.Count(s => s.uyeMail == model.uyeMail) > 0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "İlgili mail adresi sistemde mevcuttur";
                return sonuc;
            }
            Uye yeni = new Uye();
            yeni.uyeAd = model.uyeAd;
            yeni.uyeSoyad = model.uyeSoyad;
            yeni.uyeTel = model.uyeTel;
            yeni.uyeMail = model.uyeMail;
            yeni.uyeParola = model.uyeParola;
            yeni.uyeYetki = model.uyeYetki;
            db.Uye.Add(yeni);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Üye bilgileri başarılı bir şekilde sisteme eklenmiştir.";
            return sonuc;
        }

        [HttpPut]
        [Route("api/uyeduzenle")]

        public SonucModel UyeDuzenle(UyeModel model)
        {
            Uye kayit = db.Uye.Where(s => s.uyeId == model.uyeId).FirstOrDefault(); if (kayit == null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Üye bilgileri bulunamadı. Lütfen geçerli bir üye giriniz.";
                return sonuc;
            }
            kayit.uyeAd = model.uyeAd;
            kayit.uyeSoyad = model.uyeSoyad;
            kayit.uyeTel = model.uyeTel;
            kayit.uyeMail = model.uyeMail;
            kayit.uyeParola = model.uyeParola;
            kayit.uyeYetki = model.uyeYetki;
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Üye bilgileri başarılı bir şekilde düzenlenmiştir.";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/uyesil/{uyeId}")]
        public SonucModel UyeSil(int uyeId)
        {
            Uye kayit = db.Uye.Where(s => s.uyeId == uyeId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Üye bilgileri bulunamadı. Lütfen geçerli bir üye giriniz.";
                return sonuc;
            }           
            db.Uye.Remove(kayit);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Üye bilgileri başarılı bir şekilde silindi.";
            return sonuc;
        }

        #endregion

    }
}

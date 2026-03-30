using System;
using System.Collections.Generic;
using Backend.Entitete;

namespace Backend
{
    public static class MockData
    {
        public static void ResetAndSeed()
        {
            using var db = new BazaContext();

            db.Rezervacije.RemoveRange(db.Rezervacije);
            db.Sobe.RemoveRange(db.Sobe);
            db.SaveChanges();

            var sobe = new List<Soba>();
            for (var i = 0; i < 10; i++)
            {
                var stevilka = (101 + i).ToString();
                var kapaciteta = i % 3 switch
                {
                    0 => 1,
                    1 => 2,
                    _ => 4
                };
                var cena = 65m + (i * 7.5m);
                sobe.Add(new Soba(stevilka, kapaciteta, cena));
            }

            db.Sobe.AddRange(sobe);
            db.SaveChanges();

            var imena = new[]
            {
                "Ana Novak",
                "Marko Kranjc",
                "Nika Horvat",
                "Luka Vidmar",
                "Petra Zupan"
            };

            var rezervacije = new List<Rezervacija>();
            var today = DateOnly.FromDateTime(DateTime.Today);

            for (var i = 0; i < sobe.Count; i++)
            {
                var soba = sobe[i];
                for (var j = 0; j < 5; j++)
                {
                    var od = today.AddDays((i * 20) + (j * 3));
                    var do_ = od.AddDays(2);
                    var opomba = j == 0 ? "Pozna prijava" : null;
                    rezervacije.Add(new Rezervacija(od, do_, imena[j % imena.Length], opomba, soba.Id));
                }
            }

            db.Rezervacije.AddRange(rezervacije);
            db.SaveChanges();
        }
    }
}

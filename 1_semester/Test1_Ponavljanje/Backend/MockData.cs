using System;
using System.Collections.Generic;
using Backend.Entitete;
using Microsoft.EntityFrameworkCore;

namespace Backend
{
    public static class MockData
    {
        public static void ResetAndSeed()
        {
            using var db = new BazaContext();
            db.Database.Migrate();

            db.Sporocila.RemoveRange(db.Sporocila);
            db.Nabiralniki.RemoveRange(db.Nabiralniki);
            db.SaveChanges();

            var nabiralniki = new List<Nabiralnik>
            {
                new() { Email = "ana.novak@posta.si" },
                new() { Email = "marko.kranjc@posta.si" },
                new() { Email = "nika.horvat@posta.si" },
                new() { Email = "luka.vidmar@posta.si" },
                new() { Email = "petra.zupan@posta.si" },
                new() { Email = "miha.kos@posta.si" }
            };

            db.Nabiralniki.AddRange(nabiralniki);
            db.SaveChanges();

            var zadeve = new[]
            {
                "Projekt",
                "Sestanek",
                "Opomnik",
                "Status",
                "Vprašanje",
                "Dogovor"
            };
            var uvodi = new[]
            {
                "Nujno obvestilo",
                "Kava je razglašena za kritično infrastrukturo",
                "Sistem je stabilen (za zdaj)",
                "Pozor, sestanek se skriva v koledarju",
                "To ni drill, to je ponedeljek",
                "Ping iz oddelka za čudne ideje"
            };
            var jedra = new[]
            {
                "printer je pojedel tri liste in zdaj zahteva opravičilo",
                "robot sesalec je prevzel vlogo vodje izmene",
                "nekdo je repo poimenoval final_final_v2_res_final",
                "testi so zeleni, ampak zelo sumljivo samozavestni",
                "kava v kuhinji ima več uptime-a kot naš staging",
                "Excel datoteka je dosegla samozavedanje"
            };
            var zakljucki = new[]
            {
                "Če vidiš raco v hodniku, je to del procesa.",
                "Prosim, ne hrani bugov po 16:00.",
                "To sporočilo je odobril notranji odbor za paniko.",
                "Če ne deluje, ga poglej strogo in poskusi znova.",
                "V skrajnem primeru pokliči osebo, ki je rekla 'to je trivialno'.",
                "Hvala za pozornost in potrpežljivost z realnostjo."
            };

            var sporocila = new List<Sporocilo>();
            var danes = DateOnly.FromDateTime(DateTime.Today);

            for (var i = 0; i < 50; i++)
            {
                var posiljatelj = nabiralniki[i % nabiralniki.Count].Email;
                var prejemnik = nabiralniki[(i + 1) % nabiralniki.Count].Email;
                var uvod = uvodi[i % uvodi.Length];
                var jedro = jedra[(i * 2) % jedra.Length];
                var zakljucek = zakljucki[(i * 3) % zakljucki.Length];

                sporocila.Add(new Sporocilo
                {
                    Zadeva = $"{zadeve[i % zadeve.Length]} #{i + 1}",
                    Vsebina = $"{uvod}: {jedro}. Placeholder #{i + 1}. {zakljucek}",
                    CasPosiljanja = danes.AddDays(-i),
                    PosiljateljEmail = posiljatelj,
                    PrejemnikEmail = prejemnik
                });
            }

            db.Sporocila.AddRange(sporocila);
            db.SaveChanges();
        }
    }
}

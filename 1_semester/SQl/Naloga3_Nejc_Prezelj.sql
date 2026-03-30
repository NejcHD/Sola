USE Bolnica;


#1. Izpišite vse sestavne dele za “ultrazvok”, ki jih nimamo na zalogi
SELECT sd.ImeDela
FROM SestavniDel sd
JOIN Oprema_SestavniDel osd ON sd.idSestavniDel = osd.TK_SestavniDel
JOIN Oprema o ON osd.TK_Oprema = o.idOprema
WHERE o.Ime = 'ultrazvok' AND sd.Zaloga = 0;

#2. Izpišite vse izdelke in njihove sestavne dele. Izdelek naj se izpiše, tudi če nima vpisanih sestavnih delov.
select o.Ime, sd.ImeDela
from SestavniDel sd
left join Oprema_SestavniDel osd on sd.idSestavnidel = osd.Tk_Sestavnidel
left join Oprema o on osd.Tk_oprema = o.idOprema; 

#3. Izpišite dobavnico št. 2 in v njej izpišite količino kupljenih posameznih delov, ceno posameznega dela in izračunajte končni znesek za vsako vrstico v tej dobavnici.
select ds.Kolicina, ds.Cena,  (ds.Kolicina * ds.Cena) as Skupni_Znesek
from Dobavnica_SestavniDel ds
where TK_Dobavnica = 2;

#4. Koliko kosov opreme je bilo prodane v posameznem mesecu?
select sum(ro.Kolicina) as Kolicina , MONTH(r.DatumRacuna) as Mesec
from Racun r 
join Racun_Oprema ro on r.idRacun = ro.Tk_Racun
group by MONTH(r.DatumRacuna);

#5. Katera medicinska oprema je bila največkrat prodana?
SELECT o.Ime, SUM(ro.Kolicina) AS NajboljProdana
FROM Oprema o 
JOIN Racun_Oprema ro ON o.idOprema = ro.TK_Oprema
GROUP BY o.Ime
HAVING NajboljProdana >= ALL (
    SELECT SUM(Kolicina) 
    FROM Racun_Oprema 
    GROUP BY TK_Oprema
);

#6. Katere stranke so kupile več (po vrednosti), kot je povprečje nakupov?

select s.ImeStranke, sum(r.znesek) as Znesek
from Stranka s
join Racun r on s.idStranka = r.TK_Stranka
group by s.ImeStranke
having Znesek > (
	select avg(znesek)
	from Racun
);

select avg(znesek)
	from Racun;


#7. Spremeni težo aparata “Aparat za merjenje pritiska RX8” v “103g”
update Oprema
set teza = 0.103
where Ime = 'Aparat za merjenje pritiska RX8';

SET SQL_SAFE_UPDATES = 0;

SELECT Ime, teza FROM Oprema WHERE Ime = 'Aparat za merjenje pritiska RX8';


#8. Izbriši partnerja z imenom “Svensen, Inc.”.
delete from Partner 
where ImePartnerja = 'Svensen, Inc.';


select ImePartnerja
from Partner;


SET SQL_SAFE_UPDATES = 0;
SET SQL_SAFE_UPDATES = 1;












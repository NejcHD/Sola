import os
import csv
import mysql.connector
import time

# POVEZAVA
conn = mysql.connector.connect(
    host='localhost',
    user='root',
    password='orodja',
    database='trisystem',
    buffered=True
)
cursor = conn.cursor()

glavna_mapa = r"C:\sola\solaUM\2_letnik\2_semester\orodja_za_razvoj_aplikacij\Naloga1\Trysystem\Race-Results"

drzave_cache = {}
tipi_cache = {}
tekmovalci_cache = {}


def napolni_cache():
    print(">>> Polnim spomin (cache) iz obstoječe baze...")
    cursor.execute("SELECT Ime_Drzave, idDrzava FROM Drzava")
    for (ime, id_d) in cursor.fetchall():
        drzave_cache[ime] = id_d
    cursor.execute("SELECT Tip, idTip_Tekmovanja FROM Tip_Tekmovanja")
    for (tip, id_t) in cursor.fetchall():
        tipi_cache[tip] = id_t
    cursor.execute("SELECT Ime_Priimek, Drzava_idDrzava, idTekmovalci FROM Tekmovalci")
    for (ime, d_id, t_id) in cursor.fetchall():
        tekmovalci_cache[f"{ime}_{d_id}"] = t_id


def pocisti(vrednost):
    if vrednost is None: return None
    v = str(vrednost).strip()
    if v in ('---', '--', '-', 'DNS', 'DNF', 'DQ', '', 'None', 'nan', 'null'):
        return None
    return v


def dobi_id_drzave(ime):
    ime = ime if ime else "Unknown"
    if ime in drzave_cache: return drzave_cache[ime]
    cursor.execute("INSERT IGNORE INTO Drzava (Ime_Drzave, Koda_Drzave) VALUES (%s, %s)", (ime, ime[:100]))
    d_id = cursor.lastrowid
    if d_id == 0:
        cursor.execute("SELECT idDrzava FROM Drzava WHERE Ime_Drzave = %s", (ime,))
        d_id = cursor.fetchone()[0]
    drzave_cache[ime] = d_id
    return d_id


def dobi_id_tekmovalca(ime, spol, drzava_id):
    kljuc = f"{ime}_{drzava_id}"
    if kljuc in tekmovalci_cache: return tekmovalci_cache[kljuc]
    cursor.execute("INSERT IGNORE INTO Tekmovalci (Ime_Priimek, Spol, Drzava_idDrzava) VALUES (%s, %s, %s)",
                   (ime, spol, drzava_id))
    tk_id = cursor.lastrowid
    if tk_id == 0:
        cursor.execute("SELECT idTekmovalci FROM Tekmovalci WHERE Ime_Priimek = %s AND Drzava_idDrzava = %s",
                       (ime, drzava_id))
        tk_id = cursor.fetchone()[0]
    tekmovalci_cache[kljuc] = tk_id
    return tk_id


def uvozi_datoteko(pot, podmapa, lokacija, leto, format_tip, spol_fiksni=None):
    if podmapa not in tipi_cache:
        cursor.execute("INSERT IGNORE INTO Tip_Tekmovanja (Tip) VALUES (%s)", (podmapa,))
        cursor.execute("SELECT idTip_Tekmovanja FROM Tip_Tekmovanja WHERE Tip = %s", (podmapa,))
        tipi_cache[podmapa] = cursor.fetchone()[0]
    tip_id = tipi_cache[podmapa]
    drzava_tekme_id = dobi_id_drzave(lokacija)

    cursor.execute(
        "INSERT INTO Tekma (Ime_tekme, Leto, Lokacija, Tip_Tekmovanja_idTip_Tekmovanja, Drzava_idDrzava) VALUES (%s,%s,%s,%s,%s)",
        (f"{podmapa} {lokacija} {leto}", leto, lokacija, tip_id, drzava_tekme_id)
    )
    tekma_id = cursor.lastrowid
    rezultati_batch = []
    enc = 'latin-1' if format_tip == 'ultra' else 'utf-8'

    with open(pot, encoding=enc) as f:
        reader = csv.DictReader(f)
        for vrstica in reader:
            if format_tip == 'ultra':
                ime, d_ime, kat = pocisti(vrstica.get('Competitor')), pocisti(vrstica.get('Country')), pocisti(
                    vrstica.get('Age_Category'))
                s, b, r, fin, rnk = pocisti(vrstica.get('Swim')), pocisti(vrstica.get('Bike')), pocisti(
                    vrstica.get('Run')), pocisti(vrstica.get('Finish')), pocisti(vrstica.get('Rank'))
                bib = None
            else:
                ime, d_ime, kat = pocisti(vrstica.get('name')), pocisti(vrstica.get('country')), pocisti(
                    vrstica.get('division'))
                s, b, r, fin, rnk = pocisti(vrstica.get('swim')), pocisti(vrstica.get('bike')), pocisti(
                    vrstica.get('run')), pocisti(vrstica.get('overall')), pocisti(vrstica.get('overallRank'))
                bib = pocisti(vrstica.get('bib'))

            if not ime: continue


            clean_rnk = pocisti(rnk)
            if clean_rnk and not clean_rnk.isdigit():
                clean_rnk = None  # Če ni številka  bo NULL

            d_id = dobi_id_drzave(d_ime)
            t_id = dobi_id_tekmovalca(ime, spol_fiksni, d_id)
            rezultati_batch.append(
                (bib, s, b, r, fin, clean_rnk, pocisti(vrstica.get('genderRank')), pocisti(vrstica.get('divRank')), kat,
                 tekma_id, tip_id, t_id))

    if rezultati_batch:
        cursor.executemany(
            "INSERT INTO Rezultati (Stevilka, Cas_Plavanja, Cas_Kolesarjenja, Cas_Teka, Skupni_cas, Uvrstitev, Uvrstitev_spol, Uvrstitev_Kategorija, Kategorija, Tekma_idTekma, Tip_Tekmovanja_idTip_Tekmovanja, Tekmovalci_idTekmovalci) VALUES (%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s)",
            rezultati_batch)
    return len(rezultati_batch)



seznam_vseh_datotek = []
for podmapa in ["IRONMAN", "IRONMAN70.3", "Ultra-triathlon"]:
    csv_mapa = os.path.join(glavna_mapa, podmapa, "CSV")
    if os.path.exists(csv_mapa):
        for dat in os.listdir(csv_mapa):
            if dat.endswith(".csv"):
                seznam_vseh_datotek.append((csv_mapa, dat, podmapa))

skupno_za_uvoz = len(seznam_vseh_datotek)
skupni_zacetek = time.time()
napolni_cache()

print(f"\n>>> Začenjam uvoz {skupno_za_uvoz} datotek...")

skupno_vrstic = 0
for i, (pot_mape, ime_dat, podmapa) in enumerate(seznam_vseh_datotek, 1):
    zacetek_dat = time.time()
    f_tip = 'ultra' if podmapa == "Ultra-triathlon" else 'ironman'
    deli = ime_dat.replace('.csv', '').split('_')
    leto = int(deli[-1]) if deli[-1].isdigit() else 0
    lokacija = deli[1] if len(deli) > 1 else 'Unknown'
    spol = 'M' if 'man' in ime_dat.lower() else 'F' if 'woman' in ime_dat.lower() else None

    n = uvozi_datoteko(os.path.join(pot_mape, ime_dat), podmapa, lokacija, leto, f_tip, spol)
    skupno_vrstic += n
    print(f"[{i}/{skupno_za_uvoz}] {ime_dat:40} | Vrstic: {n:5} | Čas: {time.time() - zacetek_dat:.2f}s")

conn.commit()
skupno_trajanje = time.time() - skupni_zacetek
print(f"\n Skupni čas: {skupno_trajanje:.2f}s | Vrstic: {skupno_vrstic}")

cursor.close()
conn.close()
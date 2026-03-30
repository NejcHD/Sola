import os
import csv
import mysql.connector


# Pot do glavne mape z podatki
glavna_mapa = r"C:\sola\solaUM\2_letnik\2_semester\orodja_za_razvoj_aplikacij\Naloga1\Trysystem\Race-Results"

# Gremo skozi vse 3 podmape
for podmapa in ["IRONMAN", "IRONMAN70.3", "Ultra-triathlon"]:
    csv_mapa = os.path.join(glavna_mapa, podmapa, "CSV")

    # os.listdir() vrne seznam vseh datotek v mapi
    datoteke = os.listdir(csv_mapa)

    # Filtriramo samo .csv datoteke
    csv_datoteke = [f for f in datoteke if f.endswith(".csv")]

    print(f"{podmapa}: {len(csv_datoteke)} CSV datotek")
    for d in csv_datoteke[:3]:  # izpišemo prve 3
        print(f"   - {d}")

import csv


def pocisti(vrednost):
    # Če je vrednost prazna ali '---' ali DNS/DNF, vrni None
    # None v Pythonu je kot null v C#
    if vrednost is None:
        return None

    vrednost = vrednost.strip()  # odstrani presledke na začetku/koncu

    if vrednost in ('---', '-', 'DNS', 'DNF', 'DQ', ''):
        return None

    return vrednost

# Vzamemo prvo datoteko iz IRONMAN mape za test
test_datoteka = os.path.join(glavna_mapa, "IRONMAN", "CSV", "im_australia_2005.csv")

# Odpremo datoteko in preberemo vsebino
with open(test_datoteka, encoding='utf-8') as f:
    bralnik = csv.DictReader(f)

    # Preberemo prve 3 vrstice
    for i, vrstica in enumerate(bralnik):

        # Preberemo podatke in jih počistimo
        ime = pocisti(vrstica['name'])
        cas_swim = pocisti(vrstica['swim'])
        cas_bike = pocisti(vrstica['bike'])
        cas_run = pocisti(vrstica['run'])
        skupni = pocisti(vrstica['overall'])

        print(f"Ime: {ime}, Plavanje: {cas_swim}, Kolesarjenje: {cas_bike}, Tek: {cas_run}, Skupaj: {skupni}")

        if i == 2:
            break

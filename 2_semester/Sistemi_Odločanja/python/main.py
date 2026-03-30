import csv
import matplotlib.pyplot as plt
import numpy as np


class Odlocanja:
    def __init__(self):
        self.podatki = []  # Številke (matrika)
        self.ime_vrstice = []  # Scenariji (zmanjšanje prodaje...)
        self.ime_stolpca = []  # Alternative (status quo, gradnja...)

    def beri_csv(self, datoteka):
        with open(datoteka, 'r', encoding='utf-8') as f:
            reader = csv.reader(f)
            vse = list(reader)

        self.ime_stolpca = [s.strip() for s in vse[0][1:]]
        for v in vse[1:]:
            self.ime_vrstice.append(v[0].strip())
            self.podatki.append([float(x) for x in v[1:]])

        # Pretvori v numpy polje za lažje računanje
        self.matrika = np.array(self.podatki)

    def izracunaj_vse(self):
        # 1. Optimist: Največji med največjimi po stolpcih
        max_po_alt = np.max(self.matrika, axis=0)
        idx_opt = np.argmax(max_po_alt)
        print(f"Optimist: {self.ime_stolpca[idx_opt]} ({max_po_alt[idx_opt]})")

        # 2. Pesimist: Največji med najmanjšimi po stolpcih
        min_po_alt = np.min(self.matrika, axis=0)
        idx_pes = np.argmax(min_po_alt)
        print(f"Pesimist: {self.ime_stolpca[idx_pes]} ({min_po_alt[idx_pes]})")

        # 3. Laplace: Največje povprečje stolpca
        avg_po_alt = np.mean(self.matrika, axis=0)
        idx_lap = np.argmax(avg_po_alt)
        print(f"Laplace:  {self.ime_stolpca[idx_lap]} ({avg_po_alt[idx_lap]:g})")

        # 4. Savage: Najmanjše od največjih obžalovanj
        regret_matrika = np.max(self.matrika, axis=1, keepdims=True) - self.matrika
        max_regret = np.max(regret_matrika, axis=0)
        idx_sav = np.argmin(max_regret)
        print(f"Savage:   {self.ime_stolpca[idx_sav]} ({max_regret[idx_sav]:g})")

    def hurwitz_in_graf(self):
        print("\nHurwitzev kriterij:")
        h_osi = np.linspace(0, 1, 11)
        v_min = np.min(self.matrika, axis=0)
        v_max = np.max(self.matrika, axis=0)

        # Izpis glave
        header = "h".ljust(8) + "".join([ime.ljust(15) for ime in self.ime_stolpca])
        print(header + "\n" + "-" * len(header))

        plt.figure(figsize=(10, 6))
        for i, ime in enumerate(self.ime_stolpca):
            # Formula: h * max + (1-h) * min
            y = [h * v_max[i] + (1 - h) * v_min[i] for h in h_osi]
            plt.plot(h_osi, y, marker='o', label=ime)

            # Izpis vrstice v tabelo (samo vsaka h stopnja)
            vrstica = f"{h_osi[0]:.1f}".replace(".", ",").ljust(8)  # primer za izpis
            # (Za celotno tabelo bi uporabili zanko spodaj)

        # Izpis celotne tabele (poenostavljeno)
        for h in h_osi:
            izpis = f"{h:g}".replace(".", ",").ljust(8)
            for i in range(len(self.ime_stolpca)):
                val = h * v_max[i] + (1 - h) * v_min[i]
                izpis += f"{val:.1f}".replace(".", ",").ljust(15)
            print(izpis)

        plt.legend()
        plt.grid(True, alpha=0.3)
        plt.savefig('graf.png')
        print("\nGraf shranjen v 'graf.png'")


# Zagon
o = Odlocanja()
o.beri_csv("podatki.csv")
o.izracunaj_vse()
o.hurwitz_in_graf()
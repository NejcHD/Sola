import mysql.connector

try:
    conn = mysql.connector.connect(
        host='localhost',
        user='root',
        password='orodja'
    )

    cursor = conn.cursor()

    # Brišemo in ustvarjamo bazo
    cursor.execute("DROP DATABASE IF EXISTS trisystem")
    cursor.execute("CREATE DATABASE trisystem")
    cursor.execute("USE trisystem")
    print("Baza trisystem pripravljena!")

    # 1. Tip Tekmovanja (Dodan UNIQUE)
    cursor.execute("""
        CREATE TABLE IF NOT EXISTS Tip_Tekmovanja(
            idTip_Tekmovanja INT AUTO_INCREMENT PRIMARY KEY,
            Tip VARCHAR(45) UNIQUE
        )
    """)
    print("Tabela Tip_Tekmovanja ustvarjena.")

    # 2. Drzava (Dodan UNIQUE in povečana Koda_Drzave)
    cursor.execute("""
        CREATE TABLE IF NOT EXISTS Drzava (
            idDrzava INT AUTO_INCREMENT PRIMARY KEY,
            Ime_Drzave VARCHAR(100) UNIQUE,
            Koda_Drzave VARCHAR(100)
        )
    """)
    print("Tabela Drzava ustvarjena.")

    # 3. Tekma
    cursor.execute("""
        CREATE TABLE IF NOT EXISTS Tekma (
            idTekma INT AUTO_INCREMENT PRIMARY KEY,
            Ime_tekme VARCHAR(255),
            Leto INT,
            Lokacija VARCHAR(100),
            Latituda DECIMAL(10,8),
            Longituda DECIMAL(11,8),
            Tip_Tekmovanja_idTip_Tekmovanja INT,
            Drzava_idDrzava INT,
            FOREIGN KEY (Tip_Tekmovanja_idTip_Tekmovanja) REFERENCES Tip_Tekmovanja(idTip_Tekmovanja),
            FOREIGN KEY (Drzava_idDrzava) REFERENCES Drzava(idDrzava)
        )
    """)
    print("Tabela Tekma ustvarjena.")

    # 4. Tekmovalci
    cursor.execute("""
        CREATE TABLE IF NOT EXISTS Tekmovalci (
            idTekmovalci INT AUTO_INCREMENT PRIMARY KEY,
            Ime_Priimek VARCHAR(255),
            Spol VARCHAR(45),
            Drzava_idDrzava INT,
            FOREIGN KEY (Drzava_idDrzava) REFERENCES Drzava(idDrzava)
        )
    """)
    print("Tabela Tekmovalci ustvarjena.")

    # 5. Rezultati (Povečana Kategorija na 100)
    cursor.execute("""
        CREATE TABLE IF NOT EXISTS Rezultati (
            idRezultata INT AUTO_INCREMENT PRIMARY KEY,
            Stevilka INT,
            Cas_Plavanja VARCHAR(45),
            Cas_Kolesarjenja VARCHAR(45),
            Cas_Teka VARCHAR(45),
            Skupni_cas VARCHAR(45),
            Uvrstitev INT,
            Uvrstitev_spol VARCHAR(45),
            Uvrstitev_Kategorija VARCHAR(45),
            Kategorija VARCHAR(100),
            Tekma_idTekma INT,
            Tip_Tekmovanja_idTip_Tekmovanja INT,
            Tekmovalci_idTekmovalci INT,
            FOREIGN KEY (Tekma_idTekma) REFERENCES Tekma(idTekma),
            FOREIGN KEY (Tip_Tekmovanja_idTip_Tekmovanja) REFERENCES Tip_Tekmovanja(idTip_Tekmovanja),
            FOREIGN KEY (Tekmovalci_idTekmovalci) REFERENCES Tekmovalci(idTekmovalci)
        )
    """)
    print("Tabela Rezultati ustvarjena.")

    conn.commit()
    print("\nUspeh: Celotna struktura baze ustreza ER diagramu in je pripravljena na uvoz!")

    cursor.close()
    conn.close()

except Exception as e:
    print("Napaka pri ustvarjanju baze:", e)
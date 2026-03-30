-- =====================================================
-- SQL SKRIPTA ZA USTVARJANJE OKOLJA (Medicinska oprema)
-- =====================================================

-- 1. USTVARJANJE BAZE
CREATE DATABASE IF NOT EXISTS Bolnica;
USE Bolnica;

-- 2. UPORABNIKI IN PRAVICE
-- Zamenjaj 'tvoje_ime' s svojim dejanskim imenom
DROP USER IF EXISTS 'tvoje_ime'@'localhost';
DROP USER IF EXISTS 'readonly'@'localhost';

CREATE USER 'tvoje_ime'@'localhost' IDENTIFIED BY 'geslo123';
GRANT SELECT, INSERT, UPDATE, DELETE ON Bolnica.* TO 'tvoje_ime'@'localhost';

CREATE USER 'readonly'@'localhost' IDENTIFIED BY 'readonly123';
GRANT SELECT ON *.* TO 'readonly'@'localhost';

-- 3. BRISANJE TABEL (za ponovni zagon skripte)
SET FOREIGN_KEY_CHECKS = 0;
DROP TABLE IF EXISTS Oprema_SestavniDel, Dobavnica_SestavniDel, Racun_Oprema, 
                   SestavniDel, Oprema, Racun, Dobavnica, Partner, Osebje, 
                   Stranka, VrstaOpreme, Proizvajalec;
SET FOREIGN_KEY_CHECKS = 1;

-- 4. USTVARJANJE TABEL
CREATE TABLE Proizvajalec (
    idProizvajalec INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    ImeProizvajalca VARCHAR(45) NOT NULL,
    Drzava VARCHAR(45)
);

CREATE TABLE VrstaOpreme (
    idVrstaOpreme INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    ImeVrste VARCHAR(45) NOT NULL
);

CREATE TABLE Stranka (
    idStranka INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    ImeStranke VARCHAR(45) NOT NULL,
    TipStranke VARCHAR(45),
    Telefon VARCHAR(20),
    Naslov VARCHAR(45)
);

CREATE TABLE Osebje (
    idOsebje INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Ime VARCHAR(45),
    Priimek VARCHAR(45)
);

CREATE TABLE Partner (
    idPartner INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    ImePartnerja VARCHAR(45) NOT NULL,
    Naslov VARCHAR(45)
);

CREATE TABLE Oprema (
    idOprema INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Ime VARCHAR(45) NOT NULL,
    Serijska_stevilka VARCHAR(45),
    teza DECIMAL(10,3), -- DECIMAL za natančnost pri gramih
    Cena DECIMAL(10,2),
    TK_Proizvajalec INT,
    TK_VrstaOpreme INT
);

CREATE TABLE SestavniDel (
    idSestavniDel INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    ImeDela VARCHAR(45) NOT NULL,
    Zaloga INT DEFAULT 0,
    TK_Partner INT
);

CREATE TABLE Racun (
    idRacun INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    StevilkaRacuna VARCHAR(45),
    DatumRacuna DATE,
    znesek DECIMAL(10,2),
    TK_Stranka INT,
    TK_Osebje INT
);

CREATE TABLE Racun_Oprema (
    idRacunOprema INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Kolicina INT,
    Cena DECIMAL(10,2),
    TK_Oprema INT,
    TK_Racun INT
);

CREATE TABLE Dobavnica (
    idDobavnica INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    StevilkaDobavnice INT,
    DatumDobavnice DATE,
    TK_Stranka INT,
    TK_Osebje INT
);

CREATE TABLE Dobavnica_SestavniDel (
    idDobavnica_SestavniDel INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Kolicina INT,
    Cena DECIMAL(10,2),
    TK_Dobavnica INT,
    TK_SestavniDel INT
);

CREATE TABLE Oprema_SestavniDel (
    idOprema_SestavniDel INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    TK_Oprema INT,
    TK_SestavniDel INT
);

-- 5. DODAJANJE OMEJITEV (ALTER TABLE)
ALTER TABLE Oprema ADD CONSTRAINT fk_o_proizvajalec FOREIGN KEY (TK_Proizvajalec) REFERENCES Proizvajalec(idProizvajalec);
ALTER TABLE Oprema ADD CONSTRAINT fk_o_vrsta FOREIGN KEY (TK_VrstaOpreme) REFERENCES VrstaOpreme(idVrstaOpreme);
ALTER TABLE SestavniDel ADD CONSTRAINT fk_sd_partner FOREIGN KEY (TK_Partner) REFERENCES Partner(idPartner);
ALTER TABLE Racun ADD CONSTRAINT fk_r_stranka FOREIGN KEY (TK_Stranka) REFERENCES Stranka(idStranka);
ALTER TABLE Racun ADD CONSTRAINT fk_r_osebje FOREIGN KEY (TK_Osebje) REFERENCES Osebje(idOsebje);
ALTER TABLE Racun_Oprema ADD CONSTRAINT fk_ro_oprema FOREIGN KEY (TK_Oprema) REFERENCES Oprema(idOprema);
ALTER TABLE Racun_Oprema ADD CONSTRAINT fk_ro_racun FOREIGN KEY (TK_Racun) REFERENCES Racun(idRacun);
ALTER TABLE Dobavnica ADD CONSTRAINT fk_d_stranka FOREIGN KEY (TK_Stranka) REFERENCES Stranka(idStranka);
ALTER TABLE Dobavnica ADD CONSTRAINT fk_d_osebje FOREIGN KEY (TK_Osebje) REFERENCES Osebje(idOsebje);
ALTER TABLE Dobavnica_SestavniDel ADD CONSTRAINT fk_dsd_dobavnica FOREIGN KEY (TK_Dobavnica) REFERENCES Dobavnica(idDobavnica);
ALTER TABLE Dobavnica_SestavniDel ADD CONSTRAINT fk_dsd_del FOREIGN KEY (TK_SestavniDel) REFERENCES SestavniDel(idSestavniDel);
ALTER TABLE Oprema_SestavniDel ADD CONSTRAINT fk_osd_oprema FOREIGN KEY (TK_Oprema) REFERENCES Oprema(idOprema);
ALTER TABLE Oprema_SestavniDel ADD CONSTRAINT fk_osd_del FOREIGN KEY (TK_SestavniDel) REFERENCES SestavniDel(idSestavniDel);

-- 6. VSTAVLJANJE PODATKOV
-- Tabele brez FK (vsaj 5 vnosov)
INSERT INTO Proizvajalec (ImeProizvajalca, Drzava) VALUES ('Siemens', 'Nemčija'), ('Philips', 'Nizozemska'), ('GE Healthcare', 'ZDA'), ('Omron', 'Japonska'), ('Medtronic', 'Irska');
INSERT INTO VrstaOpreme (ImeVrste) VALUES ('Diagnostika'), ('Merilniki'), ('Laboratorij'), ('Terapija'), ('Kirurgija');
INSERT INTO Stranka (ImeStranke, TipStranke) VALUES ('UKC Ljubljana', 'Bolnica'), ('ZD Celje', 'Ambulanta'), ('Janez Novak', 'Posameznik'), ('SB Maribor', 'Bolnica'), ('Klinika Medikus', 'Ambulanta');
INSERT INTO Osebje (Ime, Priimek) VALUES ('Marko', 'Novak'), ('Ana', 'Kvac'), ('Peter', 'Horvat'), ('Maja', 'Kos'), ('Luka', 'Zupan');
INSERT INTO Partner (ImePartnerja, Naslov) VALUES ('Svensen, Inc.', 'Oslo'), ('MedSupply', 'Dunaj'), ('BioParts', 'London'), ('EuroMed', 'Berlin'), ('GlobalTech', 'New York');

-- Tabele s FK (vsaj 10 vnosov)
INSERT INTO Oprema (Ime, teza, Cena, TK_Proizvajalec, TK_VrstaOpreme) VALUES 
('ultrazvok', 85.500, 45000.00, 1, 1), 
('Aparat za merjenje pritiska RX8', 0.500, 120.00, 4, 2),
('EKG Monitor', 4.200, 1500.00, 2, 1), 
('Rentgen', 250.000, 80000.00, 3, 1),
('Defibrilator', 6.500, 3200.00, 5, 4),
('Oksimeter', 0.150, 45.00, 4, 2),
('Inkubator', 40.000, 12000.00, 2, 3),
('Infuzijska črpalka', 1.200, 850.00, 5, 4),
('Stetoskop', 0.300, 250.00, 3, 2),
('Laboratorijska centrifuga', 15.000, 2100.00, 1, 3);

INSERT INTO SestavniDel (ImeDela, Zaloga, TK_Partner) VALUES 
('Sonda UZ', 0, 1), ('Zaslon LCD', 5, 2), ('Baterija RX8', 50, 3), ('Senzor tlaka', 20, 1), ('Ohišje UZ', 0, 1),
('Kabel EKG', 12, 4), ('Napajalnik', 8, 5), ('Cevka', 100, 2), ('Filter', 30, 3), ('Tipkovnica', 15, 4);

INSERT INTO Oprema_SestavniDel (TK_Oprema, TK_SestavniDel) VALUES (1,1), (1,5), (2,3), (2,4), (3,6), (4,7), (5,3), (7,9), (8,8), (10,10);

-- Računi (različni meseci za analizo)
INSERT INTO Racun (StevilkaRacuna, DatumRacuna, znesek, TK_Stranka, TK_Osebje) VALUES 
('R2024-01', '2024-01-15', 45000, 1, 1), ('R2024-02', '2024-01-20', 120, 2, 2), 
('R2024-03', '2024-02-05', 120, 1, 3), ('R2024-04', '2024-02-15', 3000, 4, 4), 
('R2024-05', '2024-03-01', 80000, 5, 1), ('R2024-06', '2024-03-20', 120, 2, 2),
('R2024-07', '2024-04-10', 120, 3, 3), ('R2024-08', '2024-04-25', 120, 1, 4),
('R2024-09', '2024-05-05', 120, 4, 1), ('R2024-10', '2024-05-15', 250, 5, 2);

INSERT INTO Racun_Oprema (Kolicina, Cena, TK_Oprema, TK_Racun) VALUES (1,45000,1,1), (1,120,2,2), (1,120,2,3), (1,3000,5,4), (1,80000,4,5), (1,120,2,6), (1,120,2,7), (1,120,2,8), (1,120,2,9), (1,250,9,10);

-- Dobavnice (Potrebujemo Dobavnico št. 2)
INSERT INTO Dobavnica (StevilkaDobavnice, DatumDobavnice, TK_Stranka, TK_Osebje) VALUES (101, '2024-01-01', 1, 1), (102, '2024-01-05', 2, 2), (103, '2024-01-10', 3, 3);
INSERT INTO Dobavnica_SestavniDel (Kolicina, Cena, TK_Dobavnica, TK_SestavniDel) VALUES (10, 25.00, 2, 3), (5, 15.00, 2, 4);

-- 7. PREVERJANJE IZPISA
SELECT 'Oprema' AS Tabela; SELECT * FROM Oprema;
SELECT 'Sestavni deli' AS Tabela; SELECT * FROM SestavniDel;
SELECT 'Racuni' AS Tabela; SELECT * FROM Racun;
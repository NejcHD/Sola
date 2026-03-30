use Bolnica;

#Prozilec
# avtomatsko poveca zalogo v tabeli SestavniDel

DELIMITER //

CREATE TRIGGER PosodobiZalogoPoNakupu
AFTER INSERT ON Dobavnica_SestavniDel
FOR EACH ROW
BEGIN
    UPDATE SestavniDel 
    SET Zaloga = Zaloga + NEW.Kolicina
    WHERE idSestavniDel = NEW.TK_SestavniDel;
END //

DELIMITER ;

select Zaloga 
from SestavniDel 
where idSestavniDel = 1;

INSERT INTO Dobavnica_SestavniDel (TK_Dobavnica, TK_SestavniDel, Kolicina, Cena) 
VALUES (1, 1, 10, 5.5);


#Tranzaction
# ustvarimo racun z prodano opremo, ce se kaj zalomi se ne comit

USE Bolnica;
START TRANSACTION;

INSERT INTO Racun (DatumRacuna, znesek, TK_Stranka) 
VALUES (NOW(), 300.00, 1);

INSERT INTO Racun_Oprema (TK_Racun, TK_Oprema, Kolicina) 
VALUES (LAST_INSERT_ID(), 11, 1);

COMMIT;

SELECT * FROM Racun;
SELECT  LAST_INSERT_ID() as NoviRacunID;

SELECT * FROM Stranka WHERE idStranka = 1;
SELECT * FROM Oprema;

SHOW CREATE TABLE Racun;




#Procedura
# izpis vseh kosev katerij je v zalogi manj kot 5

DELIMITER //

CREATE PROCEDURE PreveriNizkoZalogo()
BEGIN
    SELECT ImeDela, Zaloga 
    FROM SestavniDel 
    WHERE Zaloga < 5;
END //

DELIMITER ;

CALL PreveriNizkoZalogo();

#Dogodek
#  vsak ponedeljek doda zalogi +1 zalogo pri sestavni deli z id = 1

SET GLOBAL event_scheduler = ON;

DELIMITER //

CREATE EVENT dodajKolicino
ON SCHEDULE EVERY 1 week 
STARTS CURRENT_TIMESTAMP 
DO
BEGIN
    UPDATE SestavniDel 
    SET Zaloga = Zaloga + 1 
    WHERE idSestavniDel = 1;
END //

DELIMITER ;


SHOW EVENTS;

ALTER EVENT TedenskoPreverjanjeZaloge ON SCHEDULE AT CURRENT_TIMESTAMP;

select *
from SestavniDel;


DELIMITER //

CREATE EVENT TedenskoPreverjanjeZaloge
ON SCHEDULE EVERY 1 WEEK
STARTS CURRENT_TIMESTAMP 
DO
BEGIN
    CALL PreveriNizkoZalogo();
END //

DELIMITER ;
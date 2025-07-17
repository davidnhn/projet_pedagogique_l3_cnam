-- create_schema.sql
-- Ce script crée l'ensemble des tables et leurs relations

-- Optionnel : supprimer les tables existantes pour repartir d'une base vide
DROP TABLE IF EXISTS utiliser CASCADE;
DROP TABLE IF EXISTS discuter CASCADE;
DROP TABLE IF EXISTS Effectuer CASCADE;
DROP TABLE IF EXISTS explorer CASCADE;
DROP TABLE IF EXISTS accomplir CASCADE;
DROP TABLE IF EXISTS posseder CASCADE;
DROP TABLE IF EXISTS stocker CASCADE;
DROP TABLE IF EXISTS affronter CASCADE;
DROP TABLE IF EXISTS Personnage CASCADE;
DROP TABLE IF EXISTS Sauvegarde CASCADE;
DROP TABLE IF EXISTS Action CASCADE;
DROP TABLE IF EXISTS Combat CASCADE;
DROP TABLE IF EXISTS Zone CASCADE;
DROP TABLE IF EXISTS Quete CASCADE;
DROP TABLE IF EXISTS Inventaire CASCADE;
DROP TABLE IF EXISTS Objet CASCADE;
DROP TABLE IF EXISTS Bot CASCADE;
DROP TABLE IF EXISTS TypePersonnage CASCADE;
DROP TABLE IF EXISTS Joueur CASCADE;

-- Table Joueur
CREATE TABLE Joueur (
   idJoueur SERIAL PRIMARY KEY,
   pseudoJoueur VARCHAR(100) NOT NULL
);

-- Table TypePersonnage
CREATE TABLE TypePersonnage (
   idTypePersonnage SERIAL PRIMARY KEY,
   nomTypePersonnage VARCHAR(100) NOT NULL,
   descriptionTypePersonnage TEXT
);

-- Table Bot
CREATE TABLE Bot (
   idBot SERIAL PRIMARY KEY,
   niveauBot VARCHAR(100),
   dialogueBot TEXT,
   antagonisteBot VARCHAR(50),
   nomBot VARCHAR(100)
);

-- Table Objet
CREATE TABLE Objet (
   idObjet SERIAL PRIMARY KEY,
   nomObjet VARCHAR(100) NOT NULL,
   descriptionNomObjet TEXT,
   effet VARCHAR(200)
);

-- Table Inventaire
CREATE TABLE Inventaire (
   idInventaire SERIAL PRIMARY KEY,
   tailleMaxInventaire INT
);

-- Table Quete
CREATE TABLE Quete (
   idQuete SERIAL PRIMARY KEY,
   nomQuete VARCHAR(100) NOT NULL,
   descriptionQuete TEXT,
   etatQuete VARCHAR(50)
);

-- Table Zone
CREATE TABLE Zone (
   idZone SERIAL PRIMARY KEY,
   nomZone VARCHAR(100) NOT NULL,
   descriptionZone TEXT
);

-- Table Combat (dépend de Zone)
CREATE TABLE Combat (
   idCombat SERIAL PRIMARY KEY,
   resultatCombat VARCHAR(50),
   idZone INT NOT NULL,
   FOREIGN KEY (idZone) REFERENCES Zone(idZone)
);

-- Table Action
CREATE TABLE Action (
   idAction SERIAL PRIMARY KEY,
   nomAction VARCHAR(100) NOT NULL,
   descriptionAction TEXT
);

-- Table Sauvegarde (dépend de Joueur et Zone)
CREATE TABLE Sauvegarde (
   idSauvegarde SERIAL PRIMARY KEY,
   dateSauvegarde TIMESTAMP,
   positionXJoueur INT,
   positionYJoueur INT,
   idJoueur INT NOT NULL,
   idZone INT NOT NULL,
   FOREIGN KEY (idJoueur) REFERENCES Joueur(idJoueur),
   FOREIGN KEY (idZone) REFERENCES Zone(idZone)
);

-- Table Personnage (dépend de Joueur et TypePersonnage)
CREATE TABLE Personnage (
   idPersonnage SERIAL PRIMARY KEY,
   vie INT,
   experience INT,
   nomPersonnage VARCHAR(100) NOT NULL,
   etatPersonnage VARCHAR(50),
   idJoueur INT NOT NULL,
   idTypePersonnage INT NOT NULL,
   FOREIGN KEY (idJoueur) REFERENCES Joueur(idJoueur),
   FOREIGN KEY (idTypePersonnage) REFERENCES TypePersonnage(idTypePersonnage)
);

-- Table associative affronter (Personnage vs Bot)
CREATE TABLE affronter (
   idPersonnage INT,
   idBot INT,
   PRIMARY KEY(idPersonnage, idBot),
   FOREIGN KEY (idPersonnage) REFERENCES Personnage(idPersonnage),
   FOREIGN KEY (idBot) REFERENCES Bot(idBot)
);

-- Table associative stocker (Objet dans Inventaire)
CREATE TABLE stocker (
   idObjet INT,
   idInventaire INT,
   quantiteStocker INT,
   PRIMARY KEY (idObjet, idInventaire),
   FOREIGN KEY (idObjet) REFERENCES Objet(idObjet),
   FOREIGN KEY (idInventaire) REFERENCES Inventaire(idInventaire)
);

-- Table associative posseder (Personnage possède Inventaire)
CREATE TABLE posseder (
   idPersonnage INT,
   idInventaire INT,
   PRIMARY KEY(idPersonnage, idInventaire),
   FOREIGN KEY (idPersonnage) REFERENCES Personnage(idPersonnage),
   FOREIGN KEY (idInventaire) REFERENCES Inventaire(idInventaire)
);

-- Table associative accomplir (Personnage et Quete)
CREATE TABLE accomplir (
   idPersonnage INT,
   idQuete INT,
   PRIMARY KEY(idPersonnage, idQuete),
   FOREIGN KEY (idPersonnage) REFERENCES Personnage(idPersonnage),
   FOREIGN KEY (idQuete) REFERENCES Quete(idQuete)
);

-- Table associative explorer (Personnage explore Zone)
CREATE TABLE explorer (
   idPersonnage INT,
   idZone INT,
   PRIMARY KEY(idPersonnage, idZone),
   FOREIGN KEY (idPersonnage) REFERENCES Personnage(idPersonnage),
   FOREIGN KEY (idZone) REFERENCES Zone(idZone)
);

-- Table associative Effectuer (Personnage effectue Action)
CREATE TABLE Effectuer (
   idPersonnage INT,
   idAction INT,
   PRIMARY KEY(idPersonnage, idAction),
   FOREIGN KEY (idPersonnage) REFERENCES Personnage(idPersonnage),
   FOREIGN KEY (idAction) REFERENCES Action(idAction)
);

-- Table associative discuter (Personnage discute avec Bot)
CREATE TABLE discuter (
   idPersonnage INT,
   idBot INT,
   PRIMARY KEY(idPersonnage, idBot),
   FOREIGN KEY (idPersonnage) REFERENCES Personnage(idPersonnage),
   FOREIGN KEY (idBot) REFERENCES Bot(idBot)
);

-- Table associative utiliser (Personnage utilise Objet)
CREATE TABLE utiliser (
   idPersonnage INT,
   idObjet INT,
   PRIMARY KEY(idPersonnage, idObjet),
   FOREIGN KEY (idPersonnage) REFERENCES Personnage(idPersonnage),
   FOREIGN KEY (idObjet) REFERENCES Objet(idObjet)
);

-- seed.sql
-- Ce script insère des données de test dans les tables

-- Insertion dans Joueur
INSERT INTO Joueur (pseudoJoueur) VALUES
    ('Alice'),
    ('Bob');

-- Insertion dans TypePersonnage
INSERT INTO TypePersonnage (nomTypePersonnage, descriptionTypePersonnage) VALUES
    ('Prof de Maths', 'Spécialiste des équations et des calculs complexes'),
    ('Militaire', 'Expert en tactique de combat et survie'),
    ('Geek', 'Maîtrise des technologies et des références obscures'),
    ('Bollywood', 'Charisme exceptionnel et mouvements de danse légendaires'),
    ('Capuche', 'Discrétion et furtivité, expert en infiltration'),
    ('Serdaigle', 'Intelligence et sagesse, spécialiste des énigmes');

-- Insertion dans Bot
INSERT INTO Bot (niveauBot, dialogueBot, antagonisteBot, nomBot) VALUES
    ('Intermédiaire', 'Bienvenue dans mon royaume aride !', 'Oui', 'Gardien de la Cafet'),
    ('Expert', 'Cette équation sera ta perte !', 'Oui', 'Professeur Tangente'),
    ('Expert', 'Probabilité de survie: 0.01%', 'Oui', 'IA Commando'),
    ('Expert', 'Intrus détecté, élimination en cours', 'Oui', 'Titan Overclock'),
    ('Débutant', 'Bzzt... Je suis un rat snack !', 'Oui', 'Rat Snack'),
    ('Intermédiaire', 'x² + 2x - 4 = 0, résous ou meurs !', 'Oui', 'Dragon Dérivé'),
    ('Intermédiaire', '01001101 01000101 01010101 01010010 01010011', 'Oui', 'Débogueur Spectral'),
    ('Débutant', 'Je peux t''aider, si tu survis...', 'Non', 'Mister B');

-- Insertion dans Objet
INSERT INTO Objet (nomObjet, descriptionNomObjet, effet) VALUES
    ('Boisson Énergétique', 'Restaure l''énergie vitale', 'Restaure 50 points de vie'),
    ('Clavier Magique', 'Une arme redoutable contre les ennemis', 'Augmente les dégâts de 10'),
    ('Anti-virus Légendaire', 'Protège contre les attaques informatiques', 'Réduit les dégâts subis de 15%'),
    ('Code Source', 'Fragment de code permettant de pirater les systèmes ennemis', 'Révèle les faiblesses des adversaires'),
    ('Craie Enchantée', 'Permet d''effacer les équations maléfiques', 'Paralyse les ennemis mathématiques pendant 5 secondes');

-- Insertion dans Inventaire
INSERT INTO Inventaire (tailleMaxInventaire) VALUES
    (20);

-- Insertion dans Quete
INSERT INTO Quete (nomQuete, descriptionQuete, etatQuete) VALUES
    ('L''Énigme Légendaire du Menu', 'Résoudre les commandes laissées par le Gardien de la Cafet', 'Non commencé'),
    ('L''Équation Ultime', 'Résoudre l''équation gardée par le Dragon Dérivé', 'Non commencé'),
    ('Infiltration du Centre de Commande', 'Désactiver le noyau énergétique de l''IA Commando', 'Non commencé'),
    ('La Formule Magique', 'Vaincre le Titan Overclock et récupérer la formule des Proffus', 'Non commencé'),
    ('L''Oasis de Boissons Énergétiques', 'Combattre les Rats Snacks pour atteindre l''oasis', 'Non commencé');

-- Insertion dans Zone
INSERT INTO Zone (nomZone, descriptionZone) VALUES
    ('Le Désert de la Cafet', 'Un désert brûlant où la cafétéria est hostile'),
    ('L''Enfer du Tableau Mathématique', 'Un monde fait de tableaux noirs infinis'),
    ('Le Parc Commando', 'Un parcours d''obstacles plein de pièges'),
    ('La Salle Informatique', 'Une salle obscure avec des serveurs labyrinthiques');

-- Insertion dans Combat (dépend de Zone)
INSERT INTO Combat (resultatCombat, idZone) VALUES
    ('Victoire', 1),
    ('Défaite', 2),
    ('Défaite', 3),
    ('Victoire', 4);

-- Insertion dans Action
INSERT INTO Action (nomAction, descriptionAction) VALUES
    ('Attaquer', 'Infliger des dégâts'),
    ('Défendre', 'Réduire les dégâts subis'),
    ('Pirater', 'Manipuler les systèmes informatiques'),
    ('Résoudre', 'Trouver la solution à une énigme');

-- Insertion dans Sauvegarde (dépend de Joueur et Zone)
INSERT INTO Sauvegarde (dateSauvegarde, positionXJoueur, positionYJoueur, idJoueur, idZone) VALUES
    ('2025-03-29 10:00:00', 100, 150, 1, 1);

-- Insertion dans Personnage (dépend de Joueur et TypePersonnage)
INSERT INTO Personnage (vie, experience, nomPersonnage, etatPersonnage, idJoueur, idTypePersonnage) VALUES
    (100, 0, 'Antoine', 'Actif', 1, 1),
    (90, 0, 'Paola', 'Actif', 1, 2),
    (100, 0, 'Yael', 'Actif', 1, 3),
    (85, 10, 'Deluxan', 'Actif', 2, 4),
    (95, 15, 'David', 'Actif', 2, 5),
    (80, 20, 'Meriem', 'Actif', 2, 6);

-- Insertion dans affronter (relation entre Personnage et Bot)
INSERT INTO affronter (idPersonnage, idBot) VALUES
    (1, 2),
    (2, 5),
    (3, 3),
    (4, 7),
    (5, 4),
    (6, 6);

-- Insertion dans stocker (relation entre Objet et Inventaire)
INSERT INTO stocker (idObjet, idInventaire, quantiteStocker) VALUES
    (1, 1, 3),
    (2, 1, 1),
    (3, 1, 2),
    (4, 1, 1),
    (5, 1, 2);

-- Insertion dans posseder (relation entre Personnage et Inventaire)
INSERT INTO posseder (idPersonnage, idInventaire) VALUES
    (1, 1),
    (2, 1),
    (3, 1),
    (4, 1),
    (5, 1),
    (6, 1);

-- Insertion dans accomplir (relation entre Personnage et Quete)
INSERT INTO accomplir (idPersonnage, idQuete) VALUES
    (1, 2),
    (2, 5),
    (3, 3),
    (4, 4),
    (5, 1),
    (6, 2);

-- Insertion dans explorer (relation entre Personnage et Zone)
INSERT INTO explorer (idPersonnage, idZone) VALUES
    (1, 2),
    (2, 1),
    (3, 3),
    (4, 4),
    (5, 1),
    (6, 2);

-- Insertion dans Effectuer (relation entre Personnage et Action)
INSERT INTO Effectuer (idPersonnage, idAction) VALUES
    (1, 1),
    (1, 4),
    (2, 1),
    (2, 2),
    (3, 3),
    (4, 1),
    (5, 3),
    (6, 4);

-- Insertion dans discuter (relation entre Personnage et Bot)
INSERT INTO discuter (idPersonnage, idBot) VALUES
    (1, 8),
    (2, 8),
    (3, 8),
    (4, 8),
    (5, 8),
    (6, 8);

-- Insertion dans utiliser (relation entre Personnage et Objet)
INSERT INTO utiliser (idPersonnage, idObjet) VALUES
    (1, 5),
    (2, 1),
    (3, 3),
    (4, 2),
    (5, 4),
    (6, 5);

-- Mettre à jour les séquences après les insertions manuelles
SELECT setval(pg_get_serial_sequence('joueur', 'idjoueur'), coalesce(max(idjoueur), 1)) FROM joueur;
SELECT setval(pg_get_serial_sequence('typepersonnage', 'idtypepersonnage'), coalesce(max(idtypepersonnage), 1)) FROM typepersonnage;
SELECT setval(pg_get_serial_sequence('bot', 'idbot'), coalesce(max(idbot), 1)) FROM bot;
SELECT setval(pg_get_serial_sequence('objet', 'idobjet'), coalesce(max(idobjet), 1)) FROM objet;
SELECT setval(pg_get_serial_sequence('inventaire', 'idinventaire'), coalesce(max(idinventaire), 1)) FROM inventaire;
SELECT setval(pg_get_serial_sequence('quete', 'idquete'), coalesce(max(idquete), 1)) FROM quete;
SELECT setval(pg_get_serial_sequence('zone', 'idzone'), coalesce(max(idzone), 1)) FROM zone;
SELECT setval(pg_get_serial_sequence('combat', 'idcombat'), coalesce(max(idcombat), 1)) FROM combat;
SELECT setval(pg_get_serial_sequence('action', 'idaction'), coalesce(max(idaction), 1)) FROM action;
SELECT setval(pg_get_serial_sequence('sauvegarde', 'idsauvegarde'), coalesce(max(idsauvegarde), 1)) FROM sauvegarde;
SELECT setval(pg_get_serial_sequence('personnage', 'idpersonnage'), coalesce(max(idpersonnage), 1)) FROM personnage;

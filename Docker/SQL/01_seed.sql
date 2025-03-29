-- seed.sql
-- Ce script insère des données de test dans les tables

-- Insertion dans Joueur
INSERT INTO Joueur (idJoueur, pseudoJoueur) VALUES
    (1, 'Alice'),
    (2, 'Bob');

-- Insertion dans TypePersonnage
INSERT INTO TypePersonnage (idTypePersonnage, nomTypePersonnage, descriptionTypePersonnage) VALUES
    (1, 'Prof de Maths', 'Spécialiste des équations et des calculs complexes'),
    (2, 'Militaire', 'Expert en tactique de combat et survie'),
    (3, 'Geek', 'Maîtrise des technologies et des références obscures'),
    (4, 'Bollywood', 'Charisme exceptionnel et mouvements de danse légendaires'),
    (5, 'Capuche', 'Discrétion et furtivité, expert en infiltration'),
    (6, 'Serdaigle', 'Intelligence et sagesse, spécialiste des énigmes');

-- Insertion dans Bot
INSERT INTO Bot (idBot, niveauBot, dialogueBot, antagonisteBot, nomBot) VALUES
    (1, 'Intermédiaire', 'Bienvenue dans mon royaume aride !', 'Oui', 'Gardien de la Cafet'),
    (2, 'Expert', 'Cette équation sera ta perte !', 'Oui', 'Professeur Tangente'),
    (3, 'Expert', 'Probabilité de survie: 0.01%', 'Oui', 'IA Commando'),
    (4, 'Expert', 'Intrus détecté, élimination en cours', 'Oui', 'Titan Overclock'),
    (5, 'Débutant', 'Bzzt... Je suis un rat snack !', 'Oui', 'Rat Snack'),
    (6, 'Intermédiaire', 'x² + 2x - 4 = 0, résous ou meurs !', 'Oui', 'Dragon Dérivé'),
    (7, 'Intermédiaire', '01001101 01000101 01010101 01010010 01010011', 'Oui', 'Débogueur Spectral'),
    (8, 'Débutant', 'Je peux t''aider, si tu survis...', 'Non', 'Mister B');

-- Insertion dans Objet
INSERT INTO Objet (idObjet, nomObjet, descriptionNomObjet, effet) VALUES
    (1, 'Boisson Énergétique', 'Restaure l''énergie vitale', 'Restaure 50 points de vie'),
    (2, 'Clavier Magique', 'Une arme redoutable contre les ennemis', 'Augmente les dégâts de 10'),
    (3, 'Anti-virus Légendaire', 'Protège contre les attaques informatiques', 'Réduit les dégâts subis de 15%'),
    (4, 'Code Source', 'Fragment de code permettant de pirater les systèmes ennemis', 'Révèle les faiblesses des adversaires'),
    (5, 'Craie Enchantée', 'Permet d''effacer les équations maléfiques', 'Paralyse les ennemis mathématiques pendant 5 secondes');

-- Insertion dans Inventaire
INSERT INTO Inventaire (idInventaire, tailleMaxInventaire) VALUES
    (1, 20);

-- Insertion dans Quete
INSERT INTO Quete (idQuete, nomQuete, descriptionQuete, etatQuete) VALUES
    (1, 'L''Énigme Légendaire du Menu', 'Résoudre les commandes laissées par le Gardien de la Cafet', 'Non commencé'),
    (2, 'L''Équation Ultime', 'Résoudre l''équation gardée par le Dragon Dérivé', 'Non commencé'),
    (3, 'Infiltration du Centre de Commande', 'Désactiver le noyau énergétique de l''IA Commando', 'Non commencé'),
    (4, 'La Formule Magique', 'Vaincre le Titan Overclock et récupérer la formule des Proffus', 'Non commencé'),
    (5, 'L''Oasis de Boissons Énergétiques', 'Combattre les Rats Snacks pour atteindre l''oasis', 'Non commencé');

-- Insertion dans Zone
INSERT INTO Zone (idZone, nomZone, descriptionZone) VALUES
    (1, 'Le Désert de la Cafet', 'Un désert brûlant où la cafétéria est hostile'),
    (2, 'L''Enfer du Tableau Mathématique', 'Un monde fait de tableaux noirs infinis'),
    (3, 'Le Parc Commando', 'Un parcours d''obstacles plein de pièges'),
    (4, 'La Salle Informatique', 'Une salle obscure avec des serveurs labyrinthiques');

-- Insertion dans Combat (dépend de Zone)
INSERT INTO Combat (idCombat, resultatCombat, idZone) VALUES
    (1, 'Victoire', 1),
    (2, 'Défaite', 2),
    (3, 'Défaite', 3),
    (4, 'Victoire', 4);

-- Insertion dans Action
INSERT INTO Action (idAction, nomAction, descriptionAction) VALUES
    (1, 'Attaquer', 'Infliger des dégâts'),
    (2, 'Défendre', 'Réduire les dégâts subis'),
    (3, 'Pirater', 'Manipuler les systèmes informatiques'),
    (4, 'Résoudre', 'Trouver la solution à une énigme');

-- Insertion dans Sauvegarde (dépend de Joueur et Zone)
INSERT INTO Sauvegarde (idSauvegarde, dateSauvegarde, positionXJoueur, positionYJoueur, idJoueur, idZone) VALUES
    (1, '2025-03-29 10:00:00', 100, 150, 1, 1);

-- Insertion dans Personnage (dépend de Joueur et TypePersonnage)
INSERT INTO Personnage (idPersonnage, vie, experience, nomPersonnage, etatPersonnage, idJoueur, idTypePersonnage) VALUES
    (1, 100, 0, 'Antoine', 'Actif', 1, 1),
    (2, 90, 0, 'Paola', 'Actif', 1, 2),
    (3, 100, 0, 'Yael', 'Actif', 1, 3),
    (4, 85, 10, 'Deluxan', 'Actif', 2, 4),
    (5, 95, 15, 'David', 'Actif', 2, 5),
    (6, 80, 20, 'Meriem', 'Actif', 2, 6);

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

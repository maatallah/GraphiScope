GraphiScope — Spécification technique finale

Décisions utilisateur intégrées
 Version 1.0 — 05/11/2026

Projet : GraphiScope
Désignation : Windows Graphic Assets Manager
Nature : Application locale de gestion, classification, recherche et administration de fichiers graphiques.
Environnement cible : Windows 11.
Interface : Application Web responsive accessible depuis le navigateur du même ordinateur.
Statut : Spécification technique — aucune programmation autorisée à ce stade.
1. Objectif général
Développer une solution complète permettant de centraliser la gestion de fichiers graphiques dispersés dans un ou plusieurs répertoires Windows.

GraphiScope doit pouvoir :
1. Explorer récursivement les répertoires configurés.
2. Identifier et inventorier les fichiers.
3. Collecter leurs caractéristiques techniques.
4. Classifier automatiquement les ressources.
5. Appliquer des règles de classification personnalisées.
6. Permettre la correction manuelle des classifications.
7. Présenter les fichiers dans différentes vues.
8. Rechercher rapidement un élément selon plusieurs critères.
9. Détecter les doublons et les familles de versions.
10. Déplacer, copier, renommer et supprimer des éléments.
11. Contrôler les opérations individuelles et groupées.
12. Fournir des statistiques détaillées sur le stockage.
13. Conserver un historique exploitable des opérations.
14. Fonctionner sans connexion Internet ni service externe.
L'application ne doit pas modifier les fichiers sources pendant les opérations d'inventaire et d'analyse.
2. Décisions fonctionnelles validées
Domaine	Décision retenue
Exécution	Locale, navigateur sur le même PC
Répertoires	Plusieurs racines ou une racine unique
Prévisualisation	Miniatures lorsque techniquement disponibles
Suppression	Corbeille, quarantaine et suppression définitive contrôlée
Classification	Automatique, règles personnalisées et correction manuelle
Capacité	Volume inconnu et évolutif
Système	Windows 11
Dépendance Internet	Aucune pour le fonctionnement normal

Ces décisions constituent le socle obligatoire du développement.

3. Architecture générale
3.1. Architecture proposée
Interface utilisateur
GraphiScope Web UI
HTML • CSS • TypeScript

Responsive • Dashboard • Explorer • Gallery

HTTP REST / JSON — localhost uniquement

Backend applicatif
ASP.NET Core
Scan Engine • Classification • Search Engine

File Operations • Thumbnail Engine • Statistics

SQLite

Index, règles, tags, historique, paramètres


Windows

Fichiers, dossiers, miniatures, opérations




3.2. Technologies retenues
Composant	Technologie
Backend	C# / ASP.NET Core
Runtime	.NET 10 LTS
Frontend	TypeScript
Interface	HTML5 / CSS3
Base de données	SQLite
Accès aux données	Entity Framework Core ou SQL paramétré
API	REST / JSON
Communication temps réel	SignalR
Recherche	SQLite FTS5
Empreintes	SHA-256
Journaux	Logging structuré
Distribution	Application Windows autonome

Le choix de .NET 10 LTS constitue la base technique proposée. Les versions des dépendances devront être figées et vérifiées lors de la préparation de l'environnement de compilation.
3.3. Contraintes architecturales
- Aucun Docker.
- Aucune machine virtuelle.
- Aucun serveur SQL externe.
- Aucun abonnement ou API payante.
- Aucun compte utilisateur distant.
- Aucune obligation d'installer Adobe Creative Cloud.
- Aucune dépendance à Internet pour les fonctions principales.
- Exécution sans privilèges administrateur lorsque les permissions Windows le permettent.
Le backend devra être modulaire : chaque moteur devra pouvoir évoluer sans imposer une réécriture des autres modules.

4. Gestion des répertoires et inventaire
4.1. Gestion des racines
L'utilisateur pourra configurer plusieurs répertoires indépendants.

Exemples :
D:\GRAPHISME
D:\CLIENTS
E:\ARCHIVES
E:\PROJETS


Chaque racine disposera de :
- Un nom personnalisé.
- Un chemin physique.
- Un statut d'activation.
- Une date du dernier scan.
- Un nombre de fichiers.
- Un volume total.
- Un état d'accessibilité.
- Une configuration de scan spécifique.
La suppression d'une racine de la configuration ne devra jamais entraîner la suppression des fichiers physiques.
4.2. Exploration récursive
Le moteur devra :
- Parcourir les sous-répertoires.
- Identifier les fichiers et dossiers.
- Collecter les métadonnées.
- Détecter les erreurs d'accès.
- Éviter les boucles de liens symboliques.
- Ignorer les points de réanalyse non autorisés.
- Détecter les fichiers supprimés ou déplacés depuis le dernier inventaire.
Les chemins seront normalisés selon les conventions Windows, en tenant compte des volumes et de la casse.
4.3. Modes de scan
Trois modes seront proposés :
Mode	Description
Scan initial	Inventaire complet d'une racine
Scan incrémental	Mise à jour des éléments modifiés
Scan ciblé	Analyse d'un dossier ou d'une sélection

Un scan interrompu devra pouvoir être repris sans recommencer inutilement tout le travail.
4.4. Surveillance des modifications
Le système pourra utiliser FileSystemWatcher comme mécanisme de notification des changements.

Attention : ce mécanisme ne garantit pas la réception de tous les événements, particulièrement lors de fortes activités sur les répertoires.

Il devra donc être complété par des scans de réconciliation périodiques ou déclenchés manuellement.

5. Catalogue des formats
Les extensions seront regroupées dans un catalogue configurable.
Famille	Extensions principales
InDesign	INDD, INDT, IDML, ICML
Illustrator	AI, AIT, EPS
Photoshop	PSD, PSB
PDF	PDF
Images	JPG, JPEG, PNG, TIFF, TIF, BMP, WEBP, GIF
Vectoriels	SVG, EPS
CorelDRAW	CDR
Affinity	AFDESIGN, AFPHOTO, AFPUB
Sketch	SKETCH
Polices	OTF, TTF, WOFF, WOFF2
Vidéos	MP4, MOV, AVI
Archives	ZIP, RAR, 7Z

Le catalogue devra également permettre :
- L'ajout d'extensions.
- La création de catégories.
- L'association d'une extension à une catégorie.
- L'activation ou la désactivation d'une famille.
- La définition d'icônes spécifiques.
Les fichiers inconnus ne devront pas être ignorés : ils seront classés dans une catégorie « Non identifiés ».

Les extensions ne constituent pas une preuve de la nature réelle du fichier. Le moteur devra, lorsque cela est possible, distinguer l'extension déclarée et le type détecté.

6. Moteur de classification
6.1. Classification automatique
La classification utilisera plusieurs niveaux d'information :

Niveau 1 — Caractéristiques physiques
- Extension.
- Taille.
- Dates.
- Chemin.
- Nom.
Niveau 2 — Analyse lexicale

Extraction des informations présentes dans le nom :
- Références.
- Numéros.
- Années.
- Noms de clients.
- Types de documents.
- Numéros de versions.
- Indications de statut.
Niveau 3 — Analyse contextuelle

Utilisation du chemin et des relations entre fichiers pour améliorer la classification.

Exemple :
D:\CLIENTS\CLIENT_A\2025\CATALOGUES\
    Catalogue_Printemps_V01.indd
    Catalogue_Printemps_V02.indd
    Catalogue_Printemps_Final.indd


Le système pourra proposer :
- Client : CLIENT_A
- Année : 2025
- Projet : Catalogue Printemps
- Famille : Catalogue
- Version : V01, V02, Final
- Application : InDesign
6.2. Moteur de règles
L'utilisateur pourra créer des règles selon une combinaison de critères.

Exemple :
SI
    Extension = INDD
    ET Nom contient "Catalogue"
    ET Chemin contient "CLIENT_A"

ALORS
    Catégorie = Catalogue
    Application = InDesign
    Client = CLIENT_A


Fonctions requises :
- Création et modification des règles.
- Activation et désactivation.
- Priorités.
- Combinaison AND / OR.
- Opérateurs contient, commence par, termine par, égal, regex.
- Aperçu du nombre de fichiers concernés.
- Simulation avant application.
- Historique des modifications.
6.3. Classification manuelle
L'utilisateur pourra :
- Modifier une catégorie.
- Ajouter ou supprimer des tags.
- Affecter un client.
- Affecter un projet.
- Corriger une référence.
- Corriger une version.
- Verrouiller une classification contre les changements automatiques.
6.4. Confiance et provenance
Chaque résultat de classification devra indiquer :
- Sa valeur.
- Sa source : automatique, règle ou manuel.
- Son niveau de confiance, lorsque pertinent.
- La règle responsable, le cas échéant.
Une correction manuelle ne devra pas être écrasée silencieusement par un scan ultérieur.

7. Modèle de données
La base SQLite devra séparer les données physiques des données de classification.

Tables principales :
Table	Responsabilité
Roots	Répertoires configurés
Files	Inventaire des fichiers
Directories	Inventaire des dossiers
Categories	Catégories
Extensions	Catalogue des extensions
Tags	Étiquettes personnalisées
FileTags	Relations fichiers/tags
ClassificationRules	Règles
ClassificationHistory	Évolution des classifications
DuplicateGroups	Groupes de doublons
VersionGroups	Familles de versions
ScanJobs	Historique des scans
FileOperations	Historique des opérations
OperationItems	Détails des opérations
Thumbnails	Cache des miniatures
Settings	Paramètres globaux

7.1. Métadonnées obligatoires par fichier
- Identifiant interne.
- Racine associée.
- Chemin relatif.
- Chemin complet.
- Nom.
- Nom sans extension.
- Extension.
- Taille en octets.
- Date de création.
- Date de dernière modification.
- Date de dernier scan.
- Catégorie.
- Statut d'existence.
- Type détecté, si disponible.
- Empreinte SHA-256, si calculée.
- Statut de classification.
Les informations de taille seront stockées sous forme numérique en octets pour garantir la précision.
7.2. Gestion des changements physiques
La base ne devra jamais être considérée comme une représentation infaillible du disque.

Avant une opération sensible, le système vérifiera :
- L'existence réelle du fichier.
- Son chemin actuel.
- Sa taille.
- Sa date de modification.
- Son identité physique lorsque disponible.
Un fichier modifié en dehors de GraphiScope devra être réévalué.

8. Interface utilisateur
L'interface devra être conçue pour une utilisation quotidienne, avec une priorité donnée à la rapidité d'accès aux informations.
8.1. Structure générale







dribbble.com


Navigation principale :
1. Dashboard
2. Explorateur
3. Galerie
4. Recherche avancée
5. Collections intelligentes
6. Doublons
7. Versions
8. Analyse du stockage
9. Nettoyage
10. Opérations
11. Règles de classification
12. Paramètres
8.2. Explorateur
Trois modes d'affichage :
- Liste détaillée.
- Grille de vignettes.
- Arborescence.
La liste détaillée devra permettre de choisir les colonnes affichées et leur ordre.

Colonnes proposées :
Colonne	Fonction
Nom	Nom du fichier
Aperçu	Miniature ou icône
Type	Extension et catégorie
Taille	Taille réelle
Création	Date de création
Modification	Dernière modification
Client	Classification
Projet	Classification
Version	Classification
Chemin	Emplacement
Tags	Étiquettes

Les colonnes seront triables.
8.3. Recherche avancée
Le moteur devra permettre de combiner plusieurs critères.

Exemple :
Extension = INDD
Taille > 100 MB
Modification < 01/01/2025
Catégorie = Catalogue
Répertoire = D:\CLIENTS


Fonctions complémentaires :
- Recherche instantanée.
- Recherche par expression.
- Exclusion de critères.
- Sauvegarde de recherches.
- Historique.
- Recherche dans les tags.
- Recherche dans les chemins.
- Recherche plein texte sur les noms.
L'indexation FTS5 servira principalement aux noms, chemins et termes de classification, pas au contenu interne des documents propriétaires.
8.4. Galerie
La galerie affichera les miniatures lorsqu'elles sont disponibles.

Chaque vignette devra comporter :
- Aperçu.
- Nom.
- Extension.
- Taille.
- Date.
- Catégorie.
- Indicateur de sélection.
Les aperçus devront être chargés progressivement, avec pagination ou virtualisation pour éviter de saturer la mémoire du navigateur.
8.5. Fiche détaillée
Un clic sur un fichier ouvrira un panneau de détails comprenant :
- Informations générales.
- Métadonnées.
- Classification.
- Tags.
- Historique.
- Empreinte.
- Famille de versions.
- Doublons éventuels.
- Actions disponibles.
8.6. Ergonomie
Exigences :
- Interface responsive.
- Navigation clavier.
- Sélection multiple avec Ctrl et Shift.
- Glisser-déposer lorsque pertinent.
- Menu contextuel.
- Barre d'actions flottante.
- Raccourcis clavier configurables.
- Notifications non intrusives.
- Indicateurs de progression.
- Possibilité d'annuler une sélection sans perdre les filtres.

9. Moteur de prévisualisation
La prévisualisation est un module indépendant.

Il ne faut pas confondre l'indexation d'un fichier et la génération de son aperçu.
9.1. Niveaux de prise en charge
Format	Prévisualisation envisagée
JPG / PNG / WebP	Native
TIFF / BMP	Via bibliothèque adaptée
SVG	Rendu contrôlé
PDF	Bibliothèque PDF
PSD / PSB	Miniature disponible via décodeur compatible
AI	Aperçu intégré ou gestionnaire Windows si disponible
INDD	Gestionnaire de miniatures Windows/Adobe si disponible
IDML	Aperçu indirect éventuel
CDR / Affinity	Gestionnaire ou convertisseur compatible si disponible
ZIP / 7Z	Icône et informations d'archive

9.2. Stratégie technique
Trois mécanismes successifs :
1. Miniature déjà disponible dans le cache.
2. Génération par un moteur de rendu compatible.
3. Recherche d'une miniature fournie par Windows ou une application installée.
Si aucun mécanisme n'est disponible, afficher une icône de type de fichier.

Interdiction de modifier les fichiers originaux pour générer des miniatures.
9.3. Cache des miniatures
- Stockage séparé des fichiers sources.
- Taille maximale configurable.
- Suppression des miniatures obsolètes.
- Régénération après modification du fichier.
- Génération différée.
- Limitation du nombre de tâches simultanées.

10. Détection des doublons
Le module devra proposer trois niveaux.
Niveau	Méthode	Fiabilité
1	Nom + taille	Indication uniquement
2	Taille + SHA-256	Identification exacte
3	Similarité visuelle	Détection approximative

La détection visuelle sera une fonctionnalité avancée et ne devra jamais être utilisée seule pour décider d'une suppression.
10.1. Interface
Présenter les groupes sous forme comparative :
- Nom.
- Chemin.
- Taille.
- Date.
- Empreinte.
- Aperçu.
- Emplacement.
- État du fichier.
L'utilisateur pourra choisir le fichier à conserver.

Aucune suppression automatique de doublons ne sera autorisée.

11. Gestion des versions
Le moteur recherchera des familles probables à partir de :
- Noms similaires.
- Références communes.
- Numéros de versions.
- Dates.
- Répertoires.
- Extensions.
Exemple :
PROJET_A.ai
PROJET_A_V02.ai
PROJET_A_V03.ai
PROJET_A_FINAL.ai
PROJET_A_FINAL2.ai


Résultat :
Famille : PROJET_A
1. PROJET_A.ai
   
   Version présumée initiale
2. PROJET_A_V02.ai
   
   Version 02
3. PROJET_A_V03.ai
   
   Version 03
4. PROJET_A_FINAL.ai
   
   Final présumé
5. PROJET_A_FINAL2.ai
   
   Final2 présumé
Famille proposée automatiquement

Il faudra éviter de considérer automatiquement le dernier fichier modifié comme la version définitive.

12. Analyse du stockage
12.1. Tableau de bord
Statistiques :
- Nombre total de fichiers.
- Nombre total de dossiers.
- Taille totale.
- Taille par extension.
- Taille par catégorie.
- Taille par racine.
- Fichiers les plus volumineux.
- Dossiers les plus volumineux.
- Fichiers anciens.
- Doublons exacts.
- Volume potentiellement récupérable.
12.2. Storage Heatmap
Représentation de type Treemap.

Chaque rectangle représente un fichier ou un dossier, proportionnellement à sa taille.

Fonctions :
- Navigation hiérarchique.
- Zoom.
- Identification des fichiers volumineux.
- Filtrage par catégorie.
- Accès à la fiche détaillée.
12.3. Analyse temporelle
Le système pourra produire des statistiques :
- Fichiers créés par période.
- Fichiers modifiés par période.
- Évolution de la taille des racines entre deux inventaires.
- Répertoires inactifs.
Il faudra préciser que les dates de création et de modification Windows ne permettent pas de connaître avec certitude l'historique complet d'un fichier.

13. Gestion des opérations physiques
C'est le module le plus sensible du projet.

Toutes les opérations devront passer par un gestionnaire centralisé.
13.1. Opérations disponibles
Opération	Support
Copier	Oui
Déplacer	Oui
Renommer	Oui
Supprimer vers Corbeille	Oui
Mettre en quarantaine	Oui
Supprimer définitivement	Oui, contrôlé
Créer un dossier	Oui
Restaurer	Selon origine et état
Opérations multiples	Oui

13.2. Prévisualisation obligatoire
Avant une opération groupée, présenter :
- Nombre de fichiers.
- Nombre de dossiers.
- Volume concerné.
- Source.
- Destination.
- Conflits.
- Fichiers verrouillés.
- Erreurs prévisibles.
- Espace disque nécessaire.
L'utilisateur devra confirmer l'opération.
13.3. Gestion des conflits
Politique configurable :
- Ignorer.
- Remplacer après confirmation.
- Renommer automatiquement.
- Annuler l'opération concernée.
La stratégie par défaut sera de ne jamais écraser silencieusement un fichier existant.
13.4. Opérations entre volumes
Un déplacement entre deux volumes physiques ne peut pas être traité comme un simple renommage.

Le moteur devra :
1. Copier vers la destination.
2. Vérifier le résultat.
3. Contrôler l'intégrité lorsque nécessaire.
4. Supprimer la source uniquement après vérification.
5. Enregistrer le résultat.
Une interruption ne devra jamais conduire à annoncer un déplacement réussi alors que seule une partie de la copie a été effectuée.
13.5. Corbeille Windows
La suppression vers la Corbeille devra utiliser un mécanisme Windows approprié, en tenant compte du volume et des limites de la Corbeille.
13.6. Quarantaine
La quarantaine sera un répertoire de stockage contrôlé, distinct des racines indexées.

Structure proposée :
GraphiScopeData\
    database\
    thumbnails\
    quarantine\
        2026\
            11\
                operation-id\
                    files\
                    manifest.json
    logs\
    backups\


Le manifeste conservera :
- Chemin d'origine.
- Nom original.
- Date de déplacement.
- Taille.
- Empreinte si disponible.
- Identifiant d'opération.
La restauration devra contrôler les conflits de destination.
13.7. Suppression définitive
Cette opération exigera :
- Une action explicitement choisie.
- Une confirmation renforcée.
- Un récapitulatif précis.
- Une vérification des chemins.
- Une journalisation.
Il faudra préciser à l'utilisateur que la suppression définitive n'est pas une méthode d'effacement sécurisé des données.
13.8. Annulation et récupération
L'application devra proposer une fonction d'annulation pour les opérations réversibles.

Les opérations de déplacement et de renommage pourront être inversées si leurs préconditions sont toujours satisfaites.

En revanche, il ne faut pas promettre une restauration automatique après suppression définitive ou remplacement d'un fichier. L'historique n'est pas une sauvegarde.

14. Collections intelligentes
Fonction permettant de créer des regroupements virtuels.

Exemples :
Tous les fichiers InDesign
Tous les fichiers > 500 Mo
Fichiers modifiés cette année
Catalogues 2025
Logos Illustrator
Fichiers sans classification
Fichiers Final
Fichiers anciens
Doublons potentiels


Une collection sera constituée d'une requête sauvegardée.

Elle ne modifiera pas l'organisation physique des fichiers.

15. Nettoyage intelligent
Le Cleanup Simulator proposera des analyses sans effectuer immédiatement de modifications.

Catégories :
- Dossiers vides.
- Fichiers temporaires.
- Anciennes sauvegardes.
- Doublons exacts.
- Archives anciennes.
- Fichiers non identifiés.
- Fichiers volumineux.
- Fichiers sans activité récente.
Chaque suggestion présentera :
- Motif.
- Fichier concerné.
- Chemin.
- Taille.
- Date.
- Risque estimé.
- Action suggérée.
Le volume récupérable devra être calculé sans additionner plusieurs fois les mêmes fichiers appartenant à plusieurs catégories de nettoyage.

16. Journalisation et audit
Chaque opération physique devra être enregistrée.

Informations :
- Identifiant de l'opération.
- Date et heure.
- Type d'opération.
- Fichiers concernés.
- Source.
- Destination.
- Taille.
- Résultat.
- Erreur éventuelle.
- Possibilité de restauration.
- État d'exécution.
États possibles :
PENDING
RUNNING
COMPLETED
PARTIALLY_COMPLETED
FAILED
CANCELLED


L'utilisateur pourra consulter les opérations, filtrer les erreurs et exporter l'historique.

Les opérations interrompues devront être identifiables et réconciliables au prochain démarrage.

17. Sécurité et protection des données
La sécurité doit être conçue dès le départ, et non ajoutée après le développement.
17.1. Protection de l'application
- Écoute exclusivement sur 127.0.0.1.
- Pas d'exposition réseau par défaut.
- Protection contre les requêtes provenant d'autres sites (CSRF).
- Contrôle des origines HTTP.
- Jeton de session pour les opérations sensibles.
- Validation stricte des entrées API.
- Paramétrage sécurisé des chemins.
- Aucun accès arbitraire à des répertoires non autorisés.
17.2. Protection du système de fichiers
Le moteur devra notamment :
- Vérifier que les chemins restent dans les racines autorisées.
- Empêcher les traversées de chemins (..).
- Contrôler les liens symboliques et jonctions.
- Détecter les fichiers verrouillés.
- Gérer les permissions insuffisantes.
- Éviter les opérations sur les répertoires système.
- Empêcher les collisions de noms.
- Revalider les fichiers juste avant toute modification.
17.3. Intégrité de l'index
- Transactions SQLite.
- Sauvegarde de la base.
- Contrôle de cohérence.
- Gestion des interruptions.
- Vérification des opérations inachevées.
- Protection contre les modifications concurrentes.
17.4. Confidentialité
Les noms de fichiers, chemins et métadonnées peuvent être sensibles.

Par conséquent :
- Aucune télémétrie obligatoire.
- Aucun transfert automatique.
- Aucun service cloud.
- Aucun contenu graphique envoyé à un service externe.
- Aucun accès Internet nécessaire.

18. Performances et capacité
La capacité cible est volontairement évolutive.

Il serait incorrect de concevoir le système uniquement pour quelques milliers de fichiers.
Exigences
1. Scan asynchrone.
2. Limitation de la concurrence.
3. Pagination des résultats.
4. Chargement progressif des miniatures.
5. Indexation incrémentale.
6. Cache des informations fréquemment utilisées.
7. Calcul différé des empreintes.
8. Possibilité d'interrompre un scan.
9. Affichage des statistiques de progression.
10. Absence de chargement massif des fichiers en mémoire.
Politique de calcul des empreintes
Le calcul SHA-256 peut être coûteux pour de gros volumes.

Il sera donc effectué à la demande ou dans le cadre d'un traitement en arrière-plan.

Une modification du fichier invalidera son ancienne empreinte.
Fichiers volumineux
Les fichiers de plusieurs gigaoctets ne devront pas être chargés intégralement en mémoire pour être analysés.

Les lectures et calculs seront effectués par flux (streaming).

19. Organisation physique de l'application
Je propose une installation portable ou autonome, avec séparation entre programme et données.
GraphiScope\
│
├── GraphiScope.exe
│
├── App\
│   ├── Backend\
│   ├── Frontend\
│   └── Dependencies\
│
├── Data\
│   ├── database\
│   │   └── graphiscope.db
│   │
│   ├── thumbnails\
│   ├── quarantine\
│   ├── backups\
│   └── logs\
│
├── Config\
│   ├── appsettings.json
│   ├── extensions.json
│   └── default-rules.json
│
└── README.md


Le chemin de données devra pouvoir être configuré indépendamment de l'emplacement de l'exécutable.

Le programme ne devra jamais enregistrer ses données de fonctionnement dans les répertoires graphiques surveillés.
Démarrage et arrêt
Au lancement :
1. Vérifier la configuration.
2. Vérifier la base de données.
3. Reprendre les tâches interrompues.
4. Démarrer le backend local.
5. Ouvrir le navigateur.
6. Afficher le dashboard.
À l'arrêt :
- Finaliser les écritures.
- Fermer proprement SQLite.
- Arrêter les traitements pouvant être interrompus.
- Enregistrer l'état des tâches.
- Arrêter le serveur.
Une seule instance de l'application devra être active par défaut.

20. API applicative
L'interface communiquera avec le backend par une API REST documentée.

Exemples de familles d'endpoints :
Domaine	Endpoints indicatifs
Racines	/api/roots
Scan	/api/scans
Fichiers	/api/files
Dossiers	/api/directories
Recherche	/api/search
Catégories	/api/categories
Tags	/api/tags
Règles	/api/rules
Miniatures	/api/thumbnails
Doublons	/api/duplicates
Versions	/api/versions
Statistiques	/api/statistics
Opérations	/api/operations
Quarantaine	/api/quarantine
Paramètres	/api/settings

Les opérations longues devront renvoyer un identifiant de tâche et leur progression sera transmise par SignalR.

L'API devra être versionnée et documentée. Les erreurs devront utiliser des réponses structurées, avec un code explicite et un message compréhensible.

21. Tests et validation
Aucune fonctionnalité de gestion physique ne devra être considérée comme terminée sans tests.
21.1. Tests unitaires
- Classification.
- Extraction des informations de noms.
- Moteur de règles.
- Calcul de taille.
- Calcul SHA-256.
- Détection des doublons.
- Identification des versions.
- Validation des chemins.
21.2. Tests d'intégration
- Scan récursif.
- Scan incrémental.
- Fichiers verrouillés.
- Permissions insuffisantes.
- Chemins Unicode.
- Noms longs.
- Fichiers volumineux.
- Liens symboliques.
- Fichiers modifiés pendant le scan.
- Interruptions et reprise.
21.3. Tests des opérations
Un environnement de test isolé sera utilisé.

Scénarios obligatoires :
- Copie.
- Déplacement sur le même volume.
- Déplacement entre deux volumes.
- Renommage.
- Conflit de noms.
- Mise en quarantaine.
- Restauration.
- Suppression vers Corbeille.
- Suppression définitive.
- Interruption pendant une opération.
- Récupération après redémarrage.
21.4. Tests d'interface
- Résolution desktop.
- Résolution tablette.
- Résolution mobile.
- Grandes listes.
- Sélection multiple.
- Filtrage combiné.
- Pagination.
- Chargement des miniatures.
- Affichage des erreurs.
- Accessibilité clavier.
21.5. Tests de charge
Le jeu de données de validation devra inclure plusieurs volumes d'essai, notamment :
- 1 000 fichiers.
- 10 000 fichiers.
- 100 000 fichiers.
- Un scénario dépassant 100 000 fichiers si le matériel le permet.
Ces volumes sont des paliers de validation, pas des limites imposées au programme.

Les tests devront mesurer :
- Durée du scan.
- Consommation mémoire.
- Taille de la base.
- Temps de recherche.
- Temps d'affichage.
- Temps de génération des miniatures.
Les résultats seront documentés sur la machine réelle de validation.

22. Découpage du développement
Je recommande une réalisation progressive, avec validation de chaque étape.
1. Phase 1 — Fondations
   Priorité critique
   Architecture, configuration, lancement local, base SQLite, interface de navigation et gestion des racines.
   
   Livrable : application exécutable et squelette fonctionnel.
2. Phase 2 — Inventaire
   
   Scanner récursif, collecte des métadonnées, scans incrémentaux, indexation et détection des changements.
   
   Livrable : inventaire fiable des répertoires.
3. Phase 3 — Classification
   
   Catalogue des extensions, moteur de règles, extraction lexicale, tags et corrections manuelles.
   
   Livrable : système de classification exploitable.
4. Phase 4 — Interface de gestion
   
   Explorateur, galerie, fiche détaillée, recherche avancée, filtres, tris et sélections multiples.
   
   Livrable : véritable interface de gestion.
5. Phase 5 — Prévisualisation
   
   Moteur de miniatures, cache, affichage progressif et prise en charge des formats compatibles.
   
   Livrable : galerie graphique.
6. Phase 6 — Analyse intelligente
   
   Doublons, familles de versions, collections intelligentes, analyse du stockage et nettoyage simulé.
   
   Livrable : outils d'analyse et d'optimisation.
7. Phase 7 — Opérations physiques
   
   Copie, déplacement, renommage, Corbeille, quarantaine, restauration et suppression contrôlée.
   Validation de sécurité obligatoire
   Livrable : gestionnaire physique opérationnel.
8. Phase 8 — Finalisation
   
   Tests de charge, sécurité, performances, documentation, packaging et validation de la distribution.
   
   Livrable : version stable prête à l'utilisation.

L'ordre est important : le système doit d'abord savoir exactement ce qu'il manipule avant de recevoir l'autorisation de déplacer ou supprimer des fichiers.

23. Périmètre fonctionnel et priorités
Pour éviter que le projet ne devienne trop complexe dès sa première version, les fonctionnalités seront réparties en trois niveaux.
Niveau	Fonctionnalités
P0 — Obligatoire	Inventaire, racines multiples, classification, explorateur, recherche, filtres, tri, sélection multiple, opérations physiques sécurisées, historique
P1 — Important	Miniatures, doublons SHA-256, règles avancées, collections intelligentes, analyse du stockage, quarantaine
P2 — Évolutif	Analyse des dépendances Adobe, similarité visuelle, familles de versions avancées, suggestions intelligentes

Les niveaux P1 et P2 ne doivent pas compromettre la stabilité des fonctionnalités P0.
24. Fonctionnalités volontairement hors périmètre initial
Pour maîtriser les coûts et la complexité :
- Modification du contenu des fichiers graphiques.
- Remplacement d'Adobe InDesign ou Illustrator.
- Édition graphique intégrée.
- Synchronisation cloud.
- Gestion multiutilisateur.
- Intelligence artificielle nécessitant une API payante.
- Analyse sémantique complète des documents propriétaires.
- Gestion commerciale des projets et clients.
Ces fonctionnalités pourront être étudiées ultérieurement.
25. Critères d'acceptation finale
La version 1.0 ne sera considérée comme acceptable que lorsque les conditions suivantes seront vérifiées.
ID	Critère
AC-01	L'application démarre sous Windows 11 sans serveur externe.
AC-02	Plusieurs racines peuvent être configurées.
AC-03	Le scan récursif inventorie correctement les fichiers.
AC-04	Les métadonnées sont correctement enregistrées.
AC-05	Les scans incrémentaux détectent les changements.
AC-06	La classification automatique fonctionne.
AC-07	Les règles personnalisées sont exécutables et modifiables.
AC-08	Les corrections manuelles sont préservées.
AC-09	L'explorateur propose liste, grille et arborescence.
AC-10	Les recherches combinées fonctionnent.
AC-11	Les miniatures sont générées ou récupérées lorsque possible.
AC-12	Les doublons exacts sont identifiés par SHA-256.
AC-13	Les opérations multiples sont prévisualisées.
AC-14	Les déplacements et copies contrôlent leur intégrité.
AC-15	La quarantaine permet la restauration.
AC-16	La suppression définitive nécessite une confirmation explicite.
AC-17	Les opérations sont journalisées.
AC-18	Les interruptions ne corrompent pas l'index.
AC-19	Les protections de chemins et d'accès sont testées.
AC-20	Les performances sont mesurées sur des volumes importants.

26. Points techniques à vérifier avant implémentation
Certains sujets nécessitent un prototype de validation avant de figer définitivement leur solution technique.
Sujet	Vérification attendue
Miniatures INDD	Disponibilité réelle des gestionnaires Adobe/Windows
Miniatures AI	Compatibilité des gestionnaires de prévisualisation
PSD / PSB	Bibliothèque de rendu et limitations
CDR / Affinity	Solutions gratuites et compatibilité
Corbeille Windows	Comportement selon les volumes
Fichiers verrouillés	Détection et gestion des erreurs
Très grands volumes	Mesures de performances SQLite
Noms Windows complexes	Chemins longs, Unicode, caractères spéciaux
Déplacements intervolumes	Résistance aux interruptions

Je recommande de ne pas introduire une dépendance à Adobe pour les fonctions principales. Si Adobe est installé, ses capacités pourront être exploitées en complément.

27. Conditions de démarrage du développement
Avant d'écrire la première ligne de code, je fixe les règles suivantes pour le projet.
1. Commencer par une vérification de l'environnement Windows et des outils disponibles.
2. Vérifier les versions de .NET, SDK et dépendances.
3. Étudier les possibilités réelles de prévisualisation des formats prioritaires.
4. Valider l'architecture et l'organisation du dépôt.
5. Définir les interfaces entre modules.
6. Construire un prototype technique limité pour les sujets présentant des risques.
7. Faire valider les résultats avant le développement complet.
Aucune suppression, aucun déplacement et aucun renommage réel ne seront réalisés pendant les premières phases.

Les essais d'inventaire utiliseront un répertoire de test dédié.
28. Décisions complémentaires que je recommande de retenir
Il reste quelques choix qui ne nécessitent pas de bloquer la spécification, mais qui méritent d'être fixés pour éviter des ambiguïtés lors du développement.
A. Application portable ou installateur ?
Ma recommandation : portable d'abord
- Version portable autonome.
- Possibilité de créer ultérieurement un installateur Windows.


B. Emplacement de la base de données
Ma recommandation : données indépendantes
- Base hors des répertoires surveillés.
- Emplacement configurable.
- Sauvegardes automatiques de la base.


C. Langue de l'interface
Ma recommandation : multilingue dès la conception
- Français par défaut.
- Anglais disponible.
- Architecture prête pour l'ajout de l'arabe.


D. Sauvegarde des données
Ma recommandation : automatique
- Sauvegarde SQLite avant les opérations importantes de maintenance.
- Rétention configurable.
- Restauration contrôlée.


E. Ouverture des fichiers
Ma recommandation : application Windows associée
- Double-clic pour ouvrir le fichier dans son application par défaut.
- Option pour ouvrir le dossier dans l'Explorateur Windows.
- Pas de lancement automatique pendant les scans.


29. Conclusion et statut du cahier des charges
GraphiScope — Spécification technique
V1.0 préparée


29
Sections de spécification
Architecture et technologies

Définies

Fonctionnalités principales

Définies

Sécurité et opérations

Définies

Tests et critères d'acceptation

Définis

Prototypes techniques

À valider

Programmation
Non commencée


Mon conseil : ne commençons pas immédiatement à programmer les huit phases.

La prochaine étape devrait être un audit technique préparatoire en lecture seule, particulièrement consacré aux possibilités de prévisualisation des fichiers Adobe, aux bibliothèques gratuites compatibles avec Windows et à la validation de l'architecture .NET/SQLite.

Cela permettra de distinguer les fonctions réellement réalisables sans logiciel propriétaire supplémentaire de celles qui nécessitent un composant optionnel.

Une fois cet audit validé, nous pourrons établir le plan d'implémentation détaillé, fichier par fichier, sans improvisation architecturale en cours de développement.
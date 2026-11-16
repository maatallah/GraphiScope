# GS-OPS-GS0002-AUDIT-VERDICT-001
## Consolidation du verdict d'audit technique et méthodologique — Tâche GS-0002

**Référence opérationnelle :** GS-OPS-GS0002-AUDIT-VERDICT-001  
**Version du document :** V1.1.0 (Révisée post-mandat `GS-OPS-GS0002-AUDIT-VERDICT-002`)  
**Projet :** GraphiScope  
**Workspace :** `M:\dev\GraphiScope`  
**Date d'établissement :** 07 novembre 2026  
**Statut de la tâche GS-0002 :** `REVIEW` (non accepté)  
**Document audité :** `project-tracking/GS-0002-DECISION-MATRIX.md` (version V1.2.0, 82 116 octets, 481 lignes)  
**Rapport technique de référence :** `GS-OPS-GS0002-ARBITRATION-001`  
**Autorité de gouvernance :** Propriétaire du projet GraphiScope  

---

## 1. IDENTIFICATION ET PORTÉE DE L'AUDIT

### 1.1 Identité du projet et cadre d'exécution
- **Projet :** GraphiScope — Système modulaire local d'exploration, de catalogage et d'analyse de relations documentaires pour Windows.
- **Workspace actif :** `M:\dev\GraphiScope`.
- **Phase active du projet :** Phase A (Spécifications et cadrage).
- **Jalon amont :** GS-0001 validé et clôturé (`DONE`).
- **Jalon courant :** GS-0002 en examen critique (`REVIEW`).

### 1.2 Documents examinés
1. `project-tracking/GS-0002-DECISION-MATRIX.md` : Matrice de décision technique et fonctionnelle soumise à révision (V1.2.0 post-mandat `GS-OPS-GS0002-IMPLEMENT-002`).
2. Rapport d'arbitrage `GS-OPS-GS0002-ARBITRATION-001` : Consolidation technique indépendante des preuves et préparation des arbitrages.
3. Documents de gouvernance et de suivi : `PROJECT_STATE.md`, `TASKS.md`, `DECISIONS.md` (décision `DEC-0001`), `tasks/GS-0001.md`, `tasks/GS-0002.md`, `AGENT_RULES.md`, `GraphiScope.md`.

### 1.3 Objet et limites méthodologiques de l'audit
Le présent audit a pour mission exclusive d'évaluer la solidité conceptuelle, la rigueur technique, l'exactitude factuelle et la sécurité de la matrice `GS-0002-DECISION-MATRIX.md`.

L'audit établit une stricte distinction méthodologique entre quatre niveaux d'affirmation :
1. **Spécification normative :** Ce que prévoient les standards industriels et formats de fichiers (ex. ISO/IEC 29500 pour OpenXML, [MS-CFB] pour OLE2, PKWARE APPNOTE.TXT pour ZIP).
2. **Capacité documentée d'un composant tiers :** Ce qu'une bibliothèque ou un outil tiers annonce savoir réaliser dans sa documentation officielle (ex. OpenXML SDK, ExcelDataReader, WIC, Media Foundation, SQLite).
3. **Implémentation démontrée dans GraphiScope :** Ce qui est effectivement conçu, intégré, encapsulé et géré dans le code source de GraphiScope.
4. **Capacité vérifiée :** Ce qui a fait l'objet de tests empiriques reproductibles sur les postes cibles avec jeux d'essai représentatifs.

> **Principe directeur impératif :** Une possibilité technique théorique ou une capacité annoncée par une bibliothèque tierce **ne constitue en aucun cas une fonctionnalité démontrée de GraphiScope**. Toute affirmation assimilant une possibilité externe à un acquis du projet sans preuve d'implémentation et de test est non conforme.

---

## 2. VERDICT GÉNÉRAL

### 2.1 Formulation du verdict

> **GS-0002 reste au statut REVIEW. La matrice ne peut pas être considérée comme validée en l'état.**

### 2.2 Justification synthétique du verdict
L'audit indépendant confirme que la matrice a progressé entre ses révisions successives, intégrant notamment le périmètre bureautique arrêté par le propriétaire et formalisant des concepts de graphe utiles. Néanmoins, elle ne peut être acceptée en l'état pour les raisons fondamentales suivantes :

1. **Persistance de confusions entre capacités de composants et capacités GraphiScope :** La matrice présente encore des fonctionnalités permises par des bibliothèques externes comme si elles étaient intrinsèquement acquises et validées au sein de l'application.
2. **Faiblesses critiques dans l'algorithmique de sécurité des archives :** La proposition d'assainissement de chemins d'extraction par retrait silencieux de segments malveillants est inadmissible sous l'angle de la sécurité défensive.
3. **Arbitrages et dossiers incomplets :** Certains choix techniques structurants (notamment la distribution de composants multimédias et le dimensionnement de quotas) ont été avancés sans dossier de conformité juridique complet ni justification expérimentale documentée.
4. **Absence de validation formelle des arbitrages propriétaires :** Les orientations proposées lors de l'audit précédent nécessitent des arbitrages souverains explicites du propriétaire avant toute réécriture ou passage en Phase B.

---

## 3. CLASSEMENT RATIONNEL DES CONSTATS

Les constats issus du rapport technique de référence `GS-OPS-GS0002-ARBITRATION-001` sont répartis en trois catégories :

```
┌────────────────────────────────────────────────────────────────────────┐
│                   CLASSEMENT DES CONSTATS D'AUDIT                      │
├──────────────────────────┬─────────────────────────┬───────────────────┤
│ Catégorie A              │ Catégorie B             │ Catégorie C       │
│ Acceptable techniquement │ Conditionnellement      │ Rejeté ou à       │
│                          │ acceptable              │ reformuler        │
├──────────────────────────┼─────────────────────────┼───────────────────┤
│ • Q-001 (Formules Excel) │ • Q-002 (Formats OLE2)  │ • D-3 (Chemins)   │
│ • Q-003 (Heuristiques)   │ • Q-008 (Approche TIFF) │ • Q-005 (FFmpeg)  │
│ • Q-004 (SQLite local)   │ • D-4 (Archives crypto) │ • Q-006 (Quotas)  │
│ • Q-007 (Archives bloc)  │                         │ • Exhaustivité D-1│
│ • Séparation Inspection/ │                         │ • Sécurité 100%   │
│   Extraction             │                         │ • TIFF 99,5%      │
│ • Égalité != Provenance  │                         │ • Capacités non   │
│                          │                         │   démontrées      │
└──────────────────────────┴─────────────────────────┴───────────────────┘
```

### 3.1 Catégorie A — Orientations techniquement acceptables
Ces orientations sont méthodologiquement saines, techniquement fondées et conformes aux orientations du projet :

1. **Arbitrage Q-001 (Formules Excel) :** Lecture et affichage de la formule texte (`<f>`) et de la valeur mise en cache (`<v>`), sans moteur de recalcul en V1, assortis d'un avertissement explicite d'éventuelle désactualisation ou absence.
2. **Arbitrage Q-003 (Relations d'évolution et similarités) :** Modèle épistémique où les détections algorithmiques (D2 pHash, D4 suffixes de versions) produisent des relations au statut `STATUT_PROPOSE` / `CANDIDAT`, requérant une validation explicite de l'utilisateur pour devenir `CONFIRMEE`.
3. **Arbitrage Q-004 (Emplacement SQLite) :** Préférence technique pour un hébergement de la base catalogue SQLite sur volume local sous réserve de validation de l'environnement d'exécution, afin de faciliter la coordination WAL et d'éviter les aléas réseau (l'exigence restant soumise à arbitrage).
4. **Arbitrage Q-007 (Gestion des conteneurs chiffrés sans mot de passe) :** Conservation et inventaire explicite de l'archive dans le catalogue en tant qu'entité à statut `CONTENEUR_CHIFFRE_OPAQUE`, sans tentative d'extraction aveugle ni masquage de son existence.
5. **Séparation étanche entre Inspection et Extraction :** L'inspection et l'indexation de la table des matières d'un conteneur ne doivent en aucun cas entraîner la matérialisation préalable des fichiers sur disque.
6. **Distinction conceptuelle entre égalité binaire et provenance historique :** `MEME_CONTENU` (symétrique, déduit d'un indice cryptographique fort) est formellement dissocié de `COPIE_DE` (orienté, exigeant une antécédence ou une traçabilité d'action).

### 3.2 Catégorie B — Orientations conditionnellement acceptables
Ces orientations sont recevables sur le principe, mais soumises à des conditions techniques strictes qui empêchent leur approbation sans réserve :

1. **Arbitrage Q-002 (Prise en charge des anciens formats binaires Office DOC, XLS, PPT) :**
   - *Orientation proposée :* Exploration d'adaptateurs dédiés (ex. ExcelDataReader, NPOI) pour extraire du contenu tabulaire ou textuel.
   - *Conditions suspensives :* Ne faire aucune promesse d'extraction tabulaire « complète » pour XLS ou XLSB. Présenter les capacités comme des possibilités documentées de composants candidats, soumises à intégration et validation. Distinguer strictement les cellules et valeurs de base des structures avancées (graphiques, macros, commentaires, objets incorporés). Pour DOC et PPT, le niveau de service dégradé (texte brut/métadonnées) demeure une proposition soumise à arbitrage, sans assimiler une capacité tierce à une capacité GraphiScope acquise.
2. **Arbitrage Q-008 (Approche du format TIFF et des répertoires IFD) :**
   - *Orientation proposée :* Utilisation conjointe du codec WIC natif et d'un analyseur de métadonnées IFD.
   - *Conditions suspensives :* Constater que la bibliothèque `LibTiff.NET` n'a pas fait l'objet de publication récente sur NuGet depuis la version 2.4.642 (août 2022) et ne peut être présentée comme un composant d'affichage moderne. Spécifier la prise en charge de BigTIFF (> 4 Go) comme une incertitude technique non garantie en V1 et confier le décodage d'affichage à WIC sous Windows.
3. **Sécurité D-4 (Archives chiffrées et gestion des secrets) :**
   - *Orientation proposée :* Distinction entre archives partiellement inspectables (en-têtes en clair) et conteneurs totalement opaques (en-têtes chiffrés).
   - *Conditions suspensives :* Ne formuler aucune promesse d'effacement garanti des secrets en mémoire fondée simplement sur `SecureString` ou l'écriture de zéros dans un tampon managé. Définir la protection des mots de passe comme une exigence de conception distincte nécessitant une étude d'ingénierie et une validation ultérieure.

### 3.3 Catégorie C — Éléments rejetés ou à reformuler
Ces éléments sont formellement écartés en raison de risques techniques, juridiques ou méthodologiques inacceptables :

1. **Algorithme d'assainissement de chemins D-3 (REJETÉ) :**
   - *Motif :* L'algorithme proposé suggérait d'élaguer ou de supprimer silencieusement les segments suspects (`..`, `:`, noms réservés) pour continuer l'extraction dans un chemin prétendument assaini. Cette approche est une faille de sécurité majeure (risque de collision silencieuse, altération arbitraire de l'arborescence, masquage d'une charge offensive).
   - *Règle impérative :* **Rejet strict et non silencieux.** Tout chemin d'entrée comportant une traversée de répertoire (`..`), un chemin absolu, un chemin UNC, un préfixe de périphérique ou un nom réservé Windows doit provoquer le **rejet immédiat de l'entrée**, l'interdiction de son écriture sur disque et la consignation d'un avertissement d'audit de sécurité.
2. **Arbitrage FFmpeg Q-005 (À REFORMULER EN PROFONDEUR) :**
   - *Motif :* Présenter l'appel out-of-process de FFmpeg comme une garantie automatique d'exonération copyleft sans dossier de conformité complet est juridiquement erroné.
   - *Règle impérative :* Les conséquences juridiques dépendent de la compilation exacte, des options retenues, des dépendances intégrées, du mode de distribution et des obligations de licence (y compris la mise à disposition des sources correspondantes). Aucune conclusion juridique définitive ne doit être affirmée sans analyse documentée. L'arbitrage demeure ouvert entre distribution intégrée documentée et approche BYOB (*Bring Your Own Binary*).
3. **Quotas numériques de décompression Q-006 (À REFORMULER) :**
   - *Motif :* Les chiffres avancés (500 Mo unitaire, 2 Go cumulé, 100:1, 3 niveaux d'imbrication) ont été proposés de façon arbitraire, sans justification expérimentale sur un corpus de test ni mise en relation avec un budget global de ressources de la machine hôte.
   - *Règle impérative :* Ces valeurs doivent être présentées uniquement comme des seuils indicatifs candidats, modifiables par l'utilisateur, à compléter impérativement par la surveillance active de l'espace disque résiduel réel de la partition cible.
4. **Prétention à l'exhaustivité du modèle de menace D-1 (REJETÉE) :**
   - *Motif :* Affirmer qu'une liste de vecteurs constitue un inventaire « exhaustif » et garantit une « sécurité absolue à 100% » est méthodologiquement erroné en sécurité logicielle.
   - *Règle impérative :* Remplacer toute affirmation d'exhaustivité par la notion de *modèle de menaces identifiées à ce jour*, sujet à réévaluation continue.
5. **Chiffre spéculatif de compatibilité TIFF à 99,5% (REJETÉ) :**
   - *Motif :* Aucun échantillonnage statistique n'a été réalisé pour étayer ce pourcentage, qui relève de la spéculation non prouvée.
6. **Assimilation de capacités externes à des capacités démontrées GraphiScope (REJETÉE) :**
   - *Motif :* Toute section présentant une bibliothèque candidate comme une capacité opérationnelle acquise sans code d'intégration ni tests documentés doit être requalifiée en capacité candidate non démontrée dans GraphiScope.

---

## 4. CORRECTIONS TECHNIQUES IMPÉRATIVES

Toute future version de la matrice devra intégrer sans dérogation les corrections techniques suivantes :

### 4.1 Remplacement des statuts de support par quatre niveaux formels
La matrice doit abandonner les formulations subjectives (« support complet », « support natif ») au profit d'une grille de qualification univoque :
- **Niveau 1 — Capacité formelle du format :** Caractéristique définie dans la norme ou spécification du format.
- **Niveau 2 — Capacité documentée du composant tiers :** Fonctionnalité déclarée par le mainteneur de la bibliothèque candidate.
- **Niveau 3 — Implémentation démontrée dans GraphiScope :** Présence d'un adaptateur logiciel testable au sein du code source du projet.
- **Niveau 4 — Capacité vérifiée sur environnement cible :** Comportement validé par une suite de tests documentée sur Windows 10/11 avec jeu de données représentatif.

### 4.2 Découpage ordonné du cycle de vie des archives
Le traitement des conteneurs compressés doit être segmenté en cinq phases hermétiques :
1. **Ouverture et validation du conteneur :** Vérification de l'intégrité de l'en-tête, détection du chiffrement global, lecture sans décompression.
2. **Inspection et inventaire des entrées :** Énumération des métadonnées du catalogue interne (chemins relatifs, tailles compressées/décompressées, horodatages, méthodes).
3. **Extraction sélective sous contrôle strict :** Décompression à la demande d'une entrée unique dans un répertoire temporaire sandboxé, sous quotas contrôlés.
4. **Lecture et analyse du contenu extrait :** Ingestion du flux décompressé par les extracteurs de métadonnées et de texte.
5. **Gestion des archives imbriquées :** Modélisation d'une entrée d'archive comme un conteneur enfant. La profondeur maximale (ex. 3 niveaux) doit être considérée comme une valeur candidate soumise à justification et arbitrage, et non comme une règle définitivement figée.

### 4.3 Politique de chemins et confinement d'extraction Windows
La sécurité lors de l'extraction sur le système de fichiers hôte doit reposer sur les exigences suivantes :
- **Rejet strict des chemins dangereux :** Tout chemin d'entrée comportant un segment strictement égal à `..` (à distinguer d'une simple suite de deux points au sein d'un nom de fichier légitime tel que `doc..txt`), un chemin absolu (`C:\...`), un chemin réseau UNC (`\\server\...`), un préfixe d'espace de noms Win32 (`\\?\`, `\\.\`), un flux de données alternatif ADS (`:`), un octet nul (`\0`) ou un nom de périphérique réservé DOS, doit être **rejeté sans écriture**.
- **Extension de la liste des noms réservés Windows :** La vérification doit couvrir `CON`, `PRN`, `AUX`, `NUL`, `COM1` à `COM9`, `LPT1` à `LPT9`, ainsi que les variantes Unicode exposées telles que `COM¹`, `COM²`, `COM³`, `LPT¹`, `LPT²` et `LPT³`.
- **Sensibilité à la casse :** Ne pas présumer que NTFS est systématiquement insensible à la casse dans toutes ses configurations (le système pouvant être configuré avec sensibilité à la casse par répertoire ou sous-système).
- **Limites des contrôles lexicaux et confinement réel :** Les contrôles lexicaux sur les chaînes de chemins ne suffisent pas à neutraliser les liens symboliques, les jonctions de répertoires, les points de réanalyse et les conditions de concurrence TOCTOU (*Time-of-Check to Time-of-Use*). L'extraction exige une politique de confinement réel interdisant la création ou le suivi de liens sortant de la sandbox, assortie de contrôles adaptés lors de la création et de l'ouverture effectives des fichiers.
- **Résolution des collisions de noms :** Toute stratégie de résolution doit préserver l'identité du nom d'origine, signaler explicitement la collision et empêcher tout écrasement destructeur (la génération automatique de suffixes n'étant qu'une option candidate à arbitrer).
- **Absence de prétention à la sécurité absolue :** Reconnaître les limites des défenses mises en œuvre sans promettre une invulnérabilité universelle.

### 4.4 Modèle conceptuel de données, relations de graphe et SHA-256
- **Qualification du rôle de SHA-256 :** L'égalité de deux empreintes SHA-256 constitue un indice cryptographique extrêmement fort d'égalité de contenu, mais ne doit pas être qualifiée de preuve mathématique absolue. Lorsque l'identité exacte d'octets doit être garantie sans équivoque, une comparaison binaire directe de confirmation peut être nécessaire.
- **Orthogonalité stricte des relations :**
  - `MEME_CONTENU` : Relation symétrique basée sur la correspondance de hash. Ne présume d'aucun lien de filiation.
  - `COPIE_DE` : Relation orientée ($A \rightarrow B$), exigeant une preuve historique formelle (opération tracée ou antécédence validée).
  - `VARIANTE_DE` : Relation basée sur une proximité structurelle ou perceptuelle (D2/D3), soumise à validation humaine.
  - `VERSION_DE` : Relation chronologique d'évolution intentionnelle (D4), ne pouvant être déduite de simples suffixes sans confirmation.
  - `DERIVE_DE` : Relation de transformation de format ou de génération (ex. PDF généré depuis DOCX).
- **Positionnement de W3C PROV-DM :** La recommandation W3C PROV-DM doit être citée comme une source d'inspiration méthodologique éventuelle, et non comme une validation formelle déjà acquise du modèle interne de GraphiScope.
- **Normalisation de `CONTIENT_ARCHIVE` :** L'arborescence des conteneurs imbriqués doit relier l'entité `EntreeLogique` du conteneur parent à l'entité `ArchiveLogique` du conteneur enfant.

### 4.5 Périmètre Microsoft Office
- **Respect absolu du mandat propriétaire :** Extraction de contenu et affichage structuré uniquement au sein de GraphiScope.
- **Exclusion définitive :** Aucun moteur de rendu visuel fidèle de page Word, aucune grille de calcul interactive complète, aucun visualiseur graphique de diapositives PowerPoint, aucune automatisation COM.
- **Macros et contenus actifs :** Interdiction stricte d'exécuter, d'automatiser, d'émuler le comportement ou de lancer le code actif des macros VBA ou XLM. Cette interdiction d'exécution n'interdit pas l'identification, l'inventaire ou l'inspection statique de leur présence dans le document.
- **Formules Excel :** Restitution de la formule textuelle brute et affichage de la valeur en cache `<v>` accompagnée d'une mention d'avertissement. Signalement explicite si `<v>` est absent. Aucun moteur de recalcul embarqué en V1.

### 4.6 Médias audio et vidéo
- Décomposition obligatoire des étapes de traitement : identification du conteneur $\rightarrow$ extraction des métadonnées globales $\rightarrow$ détection des flux audio/vidéo/sous-titres $\rightarrow$ décodage d'une image clé $\rightarrow$ génération de vignette $\rightarrow$ lecture continue effective (cette dernière n'étant pas une exigence V1).
- Prise en compte explicite de l'absence de Media Foundation sur les éditions Windows N/KN.

### 4.7 Conformité juridique et licences des composants
- Ne pas déduire la conformité juridique d'une simple séparation en processus externe. Les conséquences dépendent de la compilation exacte de l'outil, des options activées, des dépendances incluses, des licences applicables et du mode de distribution.
- Exiger un dossier d'identification précis des binaires réellement distribués et une analyse juridique préalable avant toute redistribution.

---

## 5. TABLEAU DES ARBITRAGES PROPRIÉTAIRE Q-001 À Q-008

Le tableau ci-dessous formalise l'état exact des huit points d'arbitrage. **Chacun de ces points demeure soumis à la décision souveraine du propriétaire. Aucune recommandation technique ne vaut décision.**

| ID | Objet technique | Orientation technique proposée | Réserves et limites techniques | Décision attendue du propriétaire | Statut de l'arbitrage |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **Q-001** | Formules et valeurs dans les fichiers Excel | Option A : affichage formule `<f>` + cache `<v>` sans moteur de recalcul en V1. | Valeurs absentes ou potentiellement obsolètes si modifiées hors d'Excel. | Confirmer l'exclusion du moteur de recalcul et le libellé de l'avertissement utilisateur. | **À confirmer** |
| **Q-002** | Formats binaires historiques Office (DOC, XLS, PPT) | Option A : exploration d'adaptateurs dédiés (ExcelDataReader, NPOI). | Capacités non démontrées dans GraphiScope ; extraction avancée non garantie ; service dégradé sur DOC/PPT. | Arbitrer entre l'exploration de ces composants candidats ou l'exclusion des formats pré-2007. | **À confirmer** |
| **Q-003** | Statut des relations d'évolution heuristiques (D2, D3, D4) | Option A : modèle à double état (`STATUT_PROPOSE` vs `STATUT_VALIDE`). | Risque de faux positifs si validation automatique sans intervention humaine. | Confirmer la nécessité d'une validation utilisateur pour figer les variantes et versions. | **À confirmer** |
| **Q-004** | Emplacement de la base SQLite et stockage réseau | Option A : préférence pour une base SQLite locale pour le mode WAL. | Le stockage local ne garantit pas absolument contre les erreurs d'I/O ; contraintes de coordination WAL. | Confirmer ou non l'exigence d'un stockage local pour le catalogue. | **À confirmer** |
| **Q-005** | Modalité de déploiement des outils tiers GPL (FFmpeg) | Option B (BYOB) recommandée par prudence, Option A (redistribution) sous réserve légale. | Incertitudes juridiques sur la redistribution sans dossier complet de compilation et de licences. | Arbitrer entre redistribution intégrée (avec dossier juridique) ou configuration manuelle (BYOB). | **Non arbitré** |
| **Q-006** | Dimensionnement des quotas de sécurité d'archives | Option A : seuils indicatifs configurables (ex. 500 Mo, 2 Go, 100:1, 3 niveaux). | Valeurs non étayées expérimentalement ; nécessité de surveiller le disque résiduel réel. | Approuver le principe de seuils configurables et fixer les valeurs par défaut candidates. | **À documenter** |
| **Q-007** | Traitement des archives chiffrées sans mot de passe | Option A : conservation comme conteneur opaque sans tentative de forçage. | Contenu interne invisible tant que le mot de passe n'est pas fourni. | Confirmer le maintien dans l'inventaire sous statut opaque. | **À confirmer** |
| **Q-008** | Décodage et inspection des images TIFF / IFD | Option A : WIC natif pour les pixels, analyseur streaming pour les répertoires IFD. | Fichiers BigTIFF (> 4 Go) non garantis en V1 ; LibTiff.NET non maintenue activement. | Confirmer la limitation de la prise en charge BigTIFF en V1. | **À confirmer** |

---

## 6. DÉCISIONS PROPRIÉTAIRE DÉJÀ ÉTABLIES

Les décisions suivantes sont fermement acquises et constituent le cadre d'invariance du projet :

1. **Architecture modulaire :** GraphiScope est conçu comme un système modulaire et découplé.
2. **Consultation bureautique Office :** Extraction de contenu et affichage structuré uniquement au sein de l'application.
3. **Exclusion d'automatisation Office :** Aucune ouverture, aucun pilotage COM, aucune dépendance d'exécution envers Microsoft Word, Excel ou PowerPoint installés sur le poste.
4. **Macros Office :** Interdiction d'exécuter les macros VBA/XLM, de les automatiser, d'émuler leur comportement ou de lancer du code actif. Cette règle n'interdit pas leur identification, leur inventaire ou leur inspection statique.
5. **Applications Adobe :** GraphiScope ne doit ni lancer ni automatiser les applications Adobe (Adobe Acrobat, suites Creative Cloud). Cette règle n'interdit pas la détection éventuelle de versions installées, l'évaluation des capacités disponibles ou l'étude des mécanismes de prévisualisation système, et ne constitue pas une décision d'indépendance absolue non confirmée.
6. **Environnement cible :** Windows 10 et Windows 11 constituent l'environnement cible.
7. **Statut de GS-0001 :** La tâche GS-0001 est définitivement validée (`DONE`), clôturée et non réouvrable.
8. **Statut de GS-0002 :** La tâche GS-0002 demeure au statut `REVIEW`.
9. **Interdiction de Phase B :** Tout début de développement applicatif et tout passage en Phase B sont strictement interdits tant que GS-0002 n'est pas formellement accepté par le propriétaire.

---

## 7. CONDITIONS PRÉALABLES À UNE FUTURE CORRECTION DE LA MATRICE

Les points suivants constituent des recommandations techniques et des conditions de préparation d'une future intervention, sans préjudice du pouvoir souverain du propriétaire d'autoriser un périmètre différent ou une correction partielle :

```
┌────────────────────────────────────────────────────────────────────────┐
│        RECOMMANDATIONS PRÉALABLES À LA CORRECTION DE LA MATRICE        │
├────────────────────────────────────────────────────────────────────────┤
│ 1. Formalisation des arbitrages propriétaires Q-001 à Q-008            │
│ 2. Élimination des affirmations d'exhaustivité et de sécurité absolue  │
│ 3. Remplacement de l'assainissement D-3 par le rejet strict des chemins│
│ 4. Définition prudente des niveaux de service par format               │
│ 5. Adoption de la grille de preuve à 4 niveaux (Format -> Vérifié)     │
│ 6. Consolidation du modèle d'entités et de relations                   │
│ 7. Dossier de conformité documenté pour toute redistribution logicielle│
└────────────────────────────────────────────────────────────────────────┘
```

1. **Prononcé formel des arbitrages propriétaires :** Réception des décisions expresses du propriétaire sur les huit points du tableau de la section 5.
2. **Purge méthodologique des affirmations excessives :** Élimination de toute promesse de sécurité absolue, de taux de compatibilité non démontrés et de déclarations d'exhaustivité de menaces.
3. **Adoption de la politique de sécurité des chemins :** Remplacement formel de l'assainissement silencieux par le rejet immédiat et tracé de toute entrée présentant un risque.
4. **Clarification des niveaux de service :** Qualification prudente du niveau d'extraction envisagé pour chaque format, sans promesse de couverture intégrale non démontrée.
5. **Intégration de la grille de preuve à 4 niveaux :** Réécriture des tableaux pour distinguer strictement capacité théorique de composant et implémentation vérifiée GraphiScope.
6. **Consolidation du modèle relationnel :** Formalisation des définitions d'entités et d'arêtes dissociant clairement indice cryptographique de hash et causalité de duplication.
7. **Dossier de licence FFmpeg :** Établissement d'une analyse documentée des options de compilation, dépendances et obligations avant toute décision de redistribution.

---

## 8. GOUVERNANCE ET STATUT FINAL

1. **Statut de la tâche GS-0001 :** `DONE` (validé, clos, non réouvrable).
2. **Statut de la tâche GS-0002 :** `REVIEW` (maintenu en l'état, non accepté).
3. **Portée du présent document :** Le présent verdict d'audit constate la non-conformité de la version actuelle de la matrice et synthétise les conditions de redressement. Il **ne vaut en aucun cas validation ou clôture de la tâche GS-0002**.
4. **Interdiction de développement :** Le présent document **ne constitue pas une autorisation de développement**. La Phase B demeure strictement interdite.
5. **Procédure ultérieure :** Toute modification de la matrice fera l'objet d'un mandat de correction distinct, contrôlé et ciblé, émis exclusivement par le propriétaire.

---
*Fin du document GS-OPS-GS0002-AUDIT-VERDICT-001 (V1.1.0) — En attente des instructions du propriétaire.*

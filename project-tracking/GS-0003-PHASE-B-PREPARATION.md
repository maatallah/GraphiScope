# GraphiScope — Dossier de préparation technique préalable à la Phase B
**Référence du mandat de révision :** `MANDAT STRICT — CORRECTIONS DOCUMENTAIRES CIBLÉES DE GS-0003 V1.5.0 VERS V1.6.0`  
**Référence opérationnelle :** `GS-OPS-GS0003-COR-005`  
**Références des mandats antérieurs :** `GS-OPS-GS0003-PREP-001` (V1.0.0) ; `GS-OPS-GS0003-COR-001` (V1.1.0) ; `GS-OPS-GS0003-FINAL-001` (V1.2.0) ; `GS-OPS-GS0003-FINAL-COR-002` (V1.3.0) ; `GS-OPS-GS0003-FINAL-COR-003` (V1.4.0) ; `GS-OPS-GS0003-COR-004` (V1.5.0)  
**Version du document :** V1.6.0 (Corrections documentaires ciblées de conformité globale — Résolution des anomalies ANO-01 à ANO-08 issues de l'audit contradictoire GS-OPS-GS0003-AUDIT-005 : rétablissement de la question ouverte souveraine O-04 d'origine sur les anciens formats Office binaires DOC/XLS/PPT, réintégration de la portée intégrale de Q-003 sur les relations heuristiques D2/D3/D4 et distinction MEME_CONTENU vs COPIE_DE, mise en conformité de la section 8.2 avec Q-006 par substitution des quotas arbitraires par la surveillance dynamique continue des ressources réelles, restauration de la taxonomie des preuves de référence à 7 catégories en section 18, rappel systématique du second volet de Q-001 interdisant la valeur en cache <v> comme résultat exploitable, rectification de l'intitulé de Q-004 sur l'emplacement du catalogue local dans le profil Windows, et prise en compte des réserves d'ingénierie COR-01 à COR-18)  
**Historique des versions :**
- V1.0.0 (10/11/2026) : Version initiale préparatoire soumise à revue technique.
- V1.1.0 (10/11/2026) : Révision technique et documentaire corrective (résolution intégrale des constats COR-01 à COR-18).
- V1.2.0 (11/11/2026) : Consolidation finale globale, enrichissement transversal des contrôles prioritaires et qualification rigoureuse des niveaux de preuve.
- V1.3.0 (11/11/2026) : Corrections documentaires ciblées (constats COR-R01 à COR-R09).
- V1.4.0 (11/11/2026) : Dernières corrections documentaires ciblées prescrites par le propriétaire (sécurisation composant par composant du bac à sable, scrubbing anti-starvation, intégrité SQLite avant publication, conditionnalité Corbeille SMB, portée des polices per-user).
- V1.5.0 (11/11/2026) : Corrections documentaires ciblées et contrôle global sous le mandat `GS-OPS-GS0003-COR-004` (résolution exhaustive des points COR-01 à COR-18).
- V1.6.0 (11/11/2026) : Corrections documentaires de conformité globale sous le mandat `GS-OPS-GS0003-COR-005` suite à l'audit contradictoire `GS-OPS-GS0003-AUDIT-005` (rétablissement d'O-04 d'origine, portée générale de Q-003 D2/D3/D4, conformité Q-006 sur les ressources sans quotas arbitraires en section 8.2, taxonomie officielle des 7 catégories de preuve en section 18, interdiction explicite de la valeur en cache <v> sous Q-001, libellé exact de Q-004 catalogue local).
**Tâche associée :** `GS-0003`  
**Projet :** GraphiScope  
**Workspace :** `M:\\dev\\GraphiScope`  
**Auteur / Rôle :** Antigravity (Agent d'Exécution et de Consolidation Documentaire)  
**Date d'établissement :** 11 novembre 2026  
**Statut du document :** SOUMIS À LA REVUE DU PROPRIÉTAIRE (`REVIEW`)  
**Nature du document :** Dossier d'ingénierie, conception modulaire et préparation technique des décisions  
**Autorité :** Propriétaire du projet GraphiScope  
**Avertissement impératif de gouvernance :** Le présent document constitue exclusivement un dossier de conception technique préparatoire. Il ne constitue ni une autorisation de développement applicatif, ni la création d'un prototype, ni l'ouverture de la Phase B. Toute entrée en Phase B requiert un arbitrage souverain et un mandat distinct et exprès du propriétaire du projet.


## 1. Résumé exécutif
### 1.1 Objet et finalité du dossier technique V1.6.0
Le présent dossier technique constitue le livrable de préparation d'ingénierie préalable à la future Phase B du projet **GraphiScope**. Il a pour finalité de doter le propriétaire d'un cadre conceptuel complet, rigoureusement documenté, techniquement prudent et directement vérifiable, afin de lui permettre d'arbitrer l'opportunité, le calendrier, les dépendances et les modalités d'un éventuel lancement de la Phase B.

Ce document intègre l'ensemble des arbitrages souverains déjà acquis lors des tâches antérieures `GS-0001` (DONE) et `GS-0002` (DONE avec décision formelle `DEC-0002` du 10 novembre 2026). Il consolide les exigences transversales formalisées dans le référentiel permanent [project-tracking/RESERVES-ET-CONTRAINTES-TRANSVERSALES.md](file:///m:/dev/GraphiScope/project-tracking/RESERVES-ET-CONTRAINTES-TRANSVERSALES.md).

### 1.2 Synthèse des orientations d'ingénierie préparées
Le dossier structure la solution technique autour de 7 axes directeurs :
1. **Architecture locale découplée :** Application hybride sous Windows, associant une interface utilisateur Web moderne hébergée dans un contrôle Chromium WebView2 local, un orchestrateur d'API Kestrel confiné à l'adresse de bouclage `127.0.0.1`, et un sous-processus d'analyse Worker isolé pour les traitements de bas niveau (`HYPOTHESE` s'appuyant sur `SOURCE_OFFICIELLE`).
2. **Modèle de données relationnel à triple niveau :** Dissociation stricte entre l'emplacement physique du fichier sur disque (`PhysicalFile`), son identité immuable de contenu par empreinte cryptographique SHA-256 (`ResourceIdentity`), et l'entité documentaire pérenne portant les métadonnées métier et l'historique des révisions (`LogicalResource`), complétée par une typologie étanche d'états (`DECISION_PROPRIETAIRE`).
3. **Moteur d'inventaire et réconciliation incrémentale :** Stratégie de scan optimisée combinant énumération rapide des métadonnées Win32, hachage asynchrone non bloquant avec détection des modifications concurrentes (statut `UNSTABLE`), et politique de scrubbing périodique équitable avec curseur persistant et double métrique de couverture (`HYPOTHESE`).
4. **Architecture de sécurité et résistance TOCTOU :** Traitement des fichiers d'entrée selon un postulat strict de non-fiabilité (`SAFE-01`), sécurisation du parcours des répertoires par opérations relatives sur handles ouverts pour neutraliser les courses temporelles TOCTOU, et décompression en streaming sous plafonds configurables (`HYPOTHESE`).
5. **Dissociation rigoureuse des mécanismes d'isolation :** Séparation formelle entre la séparation d'espaces d'adressage, l'isolation des crashs (Worker sous Job Objects), la régulation des ressources et le confinement de sécurité (`HYPOTHESE`).
6. **Persistance locale sous SQLite WAL :** Hébergement strict du catalogue dans le profil utilisateur local (`%LOCALAPPDATA%`, `DECISION_PROPRIETAIRE` Q-004), SQLite constituant l'orientation technique privilégiée non obligatoire, sérialisation d'un unique écrivain simultané, et validation obligatoire par `PRAGMA integrity_check` avant publication de toute sauvegarde (`HYPOTHESE`).
7. **Respect scrupuleux des décisions propriétaires :** Intangibilité absolue des décisions Q-001 à Q-008, maintien rigoureusement ouvert des questions O-01 à O-04, et séquencement par jalons B1 à B5 assortis de critères de sortie stricts et de dépendances de sécurité obligatoires (`DECISION_PROPRIETAIRE`).


## 2. Périmètre et exclusions
### 2.1 Périmètre d'ingénierie préparé pour la Phase B
Le présent dossier couvre l'ensemble des études préalables et spécifications d'ingénierie nécessaires à la conception de la Phase B :
- Spécification de l'architecture logicielle globale et des flux de communication inter-processus (IPC) ;
- Modélisation conceptuelle détaillée du catalogue relationnel et des contraintes d'intégrité ;
- Formalisation des algorithmes d'inventaire récursif, de réconciliation incrémentale et de scrubbing équitable ;
- Spécification des mécanismes de sécurité face aux archives piégées (*Zip Slip*, *Zip Bombs*, courses TOCTOU) ;
- Cadrage technique des extracteurs de métadonnées et prévisualiseurs pour 10 familles de formats documentaires et créatifs ;
- Étude des environnements cibles Windows 10 et Windows 11 et qualification des statuts de support éditeur ;
- Analyse critique des bibliothèques candidates, des dépendances transitives et des licences logicielles ;
- Identification des risques techniques et proposition de mesures de réduction associées ;
- Définition d'un séquencement d'implémentation par jalons incrémentaux et de propositions de protocoles de test reproductibles.

### 2.2 Exclusions formelles du projet
Conformément aux règles de gouvernance du projet GraphiScope ([AGENT_RULES.md](file:///m:/dev/GraphiScope/AGENT_RULES.md), section 3) et aux arbitrages de la Phase A, sont formellement exclus du périmètre de la préparation et de la future Phase B :
- **Tout service cloud ou dépendance réseau externe :** GraphiScope est une application 100 % locale, autonome et privée ; aucune transmission télémétrique, aucun stockage distant ni aucun appel d'API cloud n'est toléré (`DECISION_PROPRIETAIRE`).
- **Tout recalcul de formules de feuilles de calcul :** Conformément à Q-001, GraphiScope extrait le texte brut et les formules textuelles telles qu'écrites, sans intégrer de moteur de calcul de formules, et s'interdit formellement de présenter la valeur mise en cache dans la cellule (`<v>`) comme un résultat exploitable ou validé (`DECISION_PROPRIETAIRE`).
- **Toute dépendance à des applications tierces installées :** Aucune interaction via automatisation COM/OLE avec Microsoft Office, Adobe Creative Cloud ou d'autres suites logicielles hôtes (`SAFE-04`).
- **Toute modification ou écriture destructive non sollicitée :** GraphiScope n'altère aucun fichier indexé sans action explicite et confirmation renforcée de l'utilisateur (`DATA-01`).
- **Tout développement anticipé dans la présente tâche :** La tâche GS-0003 est strictement documentaire ; aucun code de production, prototype, build, paquet de dépendance ni opération Git n'est autorisé.


## 3. État des décisions propriétaires applicables
Les huit arbitrages souverains Q-001 à Q-008, validés par le propriétaire du projet le 10 novembre 2026 (`DEC-0002`), sont intangibles et s'appliquent intégralement :

| Réf. | Intitulé de l'arbitrage | Arbitrage propriétaire validé (DEC-0002) | Impact direct sur la conception de la Phase B |
| :---: | :--- | :--- | :--- |
| **Q-001** | **Traitement des formules Excel** | **Option 2 — Formule uniquement et interdiction de la valeur en cache :** Extraction et affichage textuel de la formule présente dans la cellule uniquement. Les formules textuelles brutes sont stockées telles qu'écrites. Aucun recalcul (direct ou externe) n'est effectué. La valeur mise en cache (`<v>`) n'est en aucun cas présentée comme un résultat exploitable ou validé. | Allègement de l'empreinte mémoire, élimination des risques de divergences de calcul avec Excel, neutralisation des boucles de formules complexes. |
| **Q-002** | **Formats binaires Office 97-2003 (`.doc`, `.xls`, `.ppt`)** | **Option 2 — Prise en charge partielle et qualifiée :** Extraction des métadonnées OLE2/CFB et du texte brut accessible sans promesse d'exhaustivité de mise en page. Reconnaissance formelle OLE2, qualification format par format, signalement des éléments non interprétés. | Pas de dépendance lourde obsolète ; gestion sécurisée des flux OLE structurés sans parsing graphique complet. |
| **Q-003** | **Relations heuristiques dans le graphe (niveaux D2/D3/D4)** | **Option 2 — Relations candidates à confirmer, avec limitation du nombre de propositions :** Les relations inférées par proximité arborescente, ressemblance lexicale ou métadonnées partagées sont présentées exclusivement comme candidates (`CANDIDATE`). Aucune confirmation automatique n'est autorisée : confirmation ou rejet souverain par l'utilisateur. Limitation du nombre de candidats sans plafond arbitraire en matrice ; critères explicites et traçables. Distinction stricte entre égalité de contenu (`MEME_CONTENU`) et relation de provenance (`COPIE_DE`). Les archives créatives (packages Freepik/Magnific) constituent un cas d'application pratique et ne se substituent pas à la portée générale de l'arbitrage. | Évite la saturation du catalogue par des liens infondés ; traçabilité absolue et validation humaine des relations documentaires. |
| **Q-004** | **Emplacement de la base de données catalogue** | **Option 1 — Base locale dans le profil utilisateur Windows :** Le catalogue actif est obligatoirement hébergé sur un volume local dans le profil utilisateur Windows (`%LOCALAPPDATA%\GraphiScope\Catalog\`), garantissant l'isolation des utilisateurs et l'indépendance vis-à-vis du répertoire d'installation. L'utilisation de SQLite (avec extension FTS5) constitue une orientation technique privilégiée et non une obligation technologique définitive. Le catalogue n'est pas destiné à être hébergé sur un partage réseau (le mode WAL y étant formellement proscrit en raison des défaillances de verrous de fichiers sur SMB/UNC). | Isolation des profils utilisateurs, indépendance du dossier d'installation, élimination absolue des corruptions de base sur partages réseau distants. |
| **Q-005** | **Embarquement de FFmpeg pour les vignettes vidéo** | **Option 1 — Distribuer FFmpeg avec GraphiScope (sous condition) :** Le principe de distribution est approuvé, mais strictement suspendu à la validation préalable d'un dossier complet de conformité juridique (LGPL v2.1+, interdiction de composants non-free, respect des brevets de codecs). | Conception modulaire par wrapper externe interchangeable ; pas de dépendance bloquante sur le moteur de base. |
| **Q-006** | **Protection des ressources et limites de traitement** | **Option 3 — Aucune limite fonctionnelle prédéfinie ; seule la disponibilité des ressources système constitue la limite :** Aucun quota volumétrique arbitraire, aucun plafond universel prédéfini de taille de fichier, de nombre d'entrées, de profondeur d'archives ou de durée d'analyse. Surveillance continue multi-signaux des ressources réelles (RAM consommée, pression mémoire globale OS, espace disque résiduel, activité CPU/IO) et arrêt propre uniquement en cas de risque critique pour la stabilité du système hôte ou de malveillance avérée. | Dimensionnement des schémas de base de données pour supporter des catalogues massifs sans bridage arbitraire, tout en maintenant les défenses de sécurité nécessaires. |
| **Q-007** | **Archives chiffrées par mot de passe** | **Option 3 — Objets opaques sans déchiffrement :** Les archives protégées sont indexées comme conteneurs opaques sans sollicitation interactive de mot de passe ni tentative de déchiffrement. Métadonnées accessibles extraites en clair, signalement explicite de l'inaccessibilité. | Pas de blocage du moteur d'inventaire asynchrone ; intégrité de l'expérience utilisateur sans invites modales bloquantes. |
| **Q-008** | **Prise en charge des images BigTIFF (> 4 Go)** | **Option 2 — TIFF classique en priorité, BigTIFF selon les capacités disponibles :** Le support de base se concentre sur le standard TIFF 32 bits. BigTIFF (offsets 64 bits) est conditionné aux capacités vérifiées des bibliothèques. L'échec d'aperçu d'un très grand fichier ne bloque pas son inventaire. | Tolérance aux échecs de vignettes sur très grands fichiers (> 4 Go) sans interruption du flux d'indexation. |

Les quatre questions O-01 à O-04 demeurent formellement ouvertes et subordonnées à la décision ultérieure du propriétaire (Section 14).


## 4. Architecture logique proposée
### 4.1 Schéma modulaire d'ensemble
L'architecture de GraphiScope repose sur un découplage strict entre la présentation, l'orchestration métier et l'analyse de bas niveau :

```
+-----------------------------------------------------------------------------------+
|                           PROCESSUS HÔTE PRINCIPAL (WinForms / .NET 8)            |
|                                                                                   |
|  +-----------------------------------------------------------------------------+  |
|  |                 Couche Présentation : Microsoft Edge WebView2               |  |
|  |   - Interface React / TypeScript locale                                     |  |
|  |   - Rendu HTML5 / CSS3 / WebGL confiné                                      |  |
|  |   - Communication sécurisée via canal HTTP local ou WebMessage managé       |  |
|  +-----------------------------------------------------------------------------+  |
|                                         |                                         |
|                                         v (Appels HTTP locaux avec jeton anti-CSRF)|
|  +-----------------------------------------------------------------------------+  |
|  |              Couche Orchestration & API : Serveur Local Kestrel             |  |
|  |   - Écoute strictement liée à 127.0.0.1 sur port dynamique attribué         |  |
|  |   - Validation stricte des en-têtes Host, Origin et jeton de session        |  |
|  |   - Moteur d'inventaire, réconciliation incrémentale et planificateur      |  |
|  |   - Accès exclusif à la base SQLite locale (%LOCALAPPDATA%)                 |  |
|  +-----------------------------------------------------------------------------+  |
+-----------------------------------------|-----------------------------------------+
                                          |
                                          | Canal IPC sécurisé (Anonymous Pipes / Stdio)
                                          | Contrôle d'exécution & surveillance Job Object
                                          v
+-----------------------------------------------------------------------------------+
|                        SOUS-PROCESSUS ISOLÉ : WORKER PROCESS                      |
|                                                                                   |
|   - Confinement de processus : Job Object Win32 (limites mémoire et CPU)          |
|   - Isolation des pannes : crashs de parseurs confinés sans impact sur l'Hôte     |
|   - Droits d'écriture nuls (sauf répertoire temporaire de scratch dédié)          |
|   - Accès réseau strictement coupé (aucune socket ouverte)                        |
|   - Analyse de signatures (Magic Bytes), calcul SHA-256 et décompression stream   |
|   - Décodage d'images et génération de vignettes WebP locales                      |
+-----------------------------------------------------------------------------------+
```

### 4.2 Proposition de défense en profondeur du serveur local Kestrel (COR-10)
La sécurisation du serveur HTTP Kestrel embarqué constitue une proposition d'ingénierie soumise à validation expérimentale en Phase B (`HYPOTHESE`), s'appuyant sur des primitives officielles (`SOURCE_OFFICIELLE`) :

1. **Réfutation formelle de CORS comme protection CSRF :**
   - *Précision technique indispensable :* Le mécanisme CORS (*Cross-Origin Resource Sharing*) est une politique de sécurité implémentée par le navigateur pour contrôler si un script exécuté dans une origine A est autorisé à **lire** les réponses émises par une origine B.
   - **Limite critique :** CORS n'empêche nullement l'émission de requêtes simples en écriture (requêtes HTTP POST, GET ou formulaires HTML) initiées par un site Web tiers malveillant vers `http://127.0.0.1:<port>/`. Ces requêtes atteignent le serveur Kestrel local et exécutent l'action mutative associée même si le navigateur refuse ensuite de livrer la réponse au script attaquant.
   - Par conséquent, la configuration de CORS ne constitue pas une protection contre les attaques par falsification de requête inter-sites (*Cross-Site Request Forgery*, CSRF).

2. **Génération, délivrance et validation du jeton anti-CSRF :**
   - *Génération cryptographique :* Lors de l'initialisation du processus hôte, un jeton cryptographiquement aléatoire de 256 bits est généré via `RandomNumberGenerator.GetBytes(32)` (`SOURCE_OFFICIELLE`).
   - *Délivrance étanche :* Ce jeton n'est jamais stocké dans un cookie de navigateur vulnérable. Il est transmis de manière hermétique au frontend WebView2 soit par injection directe via l'API managée `CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync` sous la forme d'un objet immuable `window.__GRAPHISCOPE_SESSION_TOKEN__`, soit via un en-tête HTTP sécurisé lors du chargement initial de l'application hôte.
   - *Validation systématique :* Un middleware Kestrel personnalisé intercepte toutes les requêtes HTTP entrantes.
   - *Routes assujetties au jeton :* Toute requête mutative (méthodes `POST`, `PUT`, `DELETE`, `PATCH`) ainsi que la requête de négociation et d'établissement de connexion temps réel (handshake initial WebSockets / SignalR) doivent obligatoirement porter le jeton dans l'en-tête personnalisé `X-GraphiScope-Token`. Toute requête dépourvue du jeton valide est rejetée immédiatement avec le code `403 Forbidden`.

3. **Traitement de l'absence de l'en-tête Origin :**
   - Les navigateurs omettent parfois l'en-tête `Origin` sur certaines requêtes de navigation directe de type GET, mais l'incluent obligatoirement sur les requêtes cross-origin et les requêtes mutatives.
   - *Politique Kestrel :* Sur toute route d'API mutative ou sensible, l'absence de l'en-tête `Origin` ou `Referer` provoque le rejet strict de la requête (`403 Forbidden`), interdisant l'exploitation aveugle par des outils locaux ou des formulaires tiers.

4. **Contrôle strict des en-têtes `Host` et `Origin` :**
   - Le middleware valide que l'en-tête `Host` correspond rigoureusement à l'adresse de bouclage et au port alloué (ex. `127.0.0.1:54321`). Tout en-tête `Host` contenant un nom de domaine externe est catégoriquement rejeté (`400 Bad Request`).
   - L'en-tête `Origin` (lorsqu'il est présent) doit strictement correspondre à l'origine locale autorisée de l'application WebView2 (ex. `http://127.0.0.1:<port>` ou le schéma personnalisé `app://graphiscope.local`).

5. **Risque d'attaque par DNS Rebinding et gestion IPv4 / IPv6 :**
   - *Mécanisme de l'attaque :* Un nom de domaine tiers contrôlé par un attaquant (ex. `attacker.com`) peut être configuré avec un TTL DNS nul pour résoudre d'abord sur une IP externe, puis basculer sur l'adresse IP `127.0.0.1`. Le navigateur, considérant l'origine comme `attacker.com`, pourrait émettre des requêtes vers le serveur Kestrel local.
   - *Contre-mesures GraphiScope :*
     - Le filtrage strict de l'en-tête `Host` neutralise immédiatement le DNS rebinding, car le navigateur transmettra `Host: attacker.com`, qui est immédiatement rejeté par Kestrel.
     - L'écoute du serveur Kestrel est liée de manière exclusive à l'adresse littérale `127.0.0.1` (et `[::1]` pour IPv6), en interdisant formellement l'écoute sur l'interface wildcard `0.0.0.0` ou `*`.

6. **Harmonisation entre `127.0.0.1` et `localhost` :**
   - GraphiScope privilégie l'utilisation systématique de l'adresse IP littérale **`127.0.0.1`** dans toute son architecture. Cette approche élimine les aléas de résolution du nom d'hôte `localhost` (qui dépend du fichier `hosts` de Windows et peut basculer de manière imprévisible entre IPv4 et IPv6).

### 4.3 Découpage et isolation du Worker Process (COR-04)
Le découpage du moteur d'analyse répond à l'impératif de cantonner les bibliothèques de décodage tierces dans un sous-processus dédié :
- **Séparation de processus :** L'hôte principal (`GraphiScope.exe`) et le moteur d'analyse (`GraphiScope.Worker.exe`) s'exécutent dans deux processus distincts avec des espaces d'adressage mémoire étanches.
- **Canal de commande et IPC :** Communication bidirectionnelle via des canaux anonymes managés (`AnonymousPipeServerStream` / `AnonymousPipeClientStream`) ou flux standard `stdin`/`stdout`.
- **Délégation des tâches lourdes :** Calcul des empreintes SHA-256, détection des octets magiques, parsing XML/OpenXML, décompression des archives et rendu matriciel des vignettes s'exécutent exclusivement au sein du Worker.


## 5. Modèle de données conceptuel et organisation du catalogue
### 5.1 Entités relationnelles conceptuelles, cardinalités et typologie étanche d'états (COR-02, COR-05)
Le catalogue local de GraphiScope repose sur une modélisation relationnelle claire, dissociant formellement l'emplacement matériel, l'identité de contenu, la pérennité documentaire et les observations d'inventaire :

1. **Définition sans ambiguïté des concepts fondamentaux (COR-02) :**
   - **Occurrence physique (`PhysicalFile`) :** Représente l'existence matérielle d'un fichier à un emplacement donné sur un support de stockage local ou réseau. Elle est caractérisée par son chemin absolu (`FullPath`), son chemin relatif (`RelativePath`), sa taille en octets (`FileSizeBytes`), son horodatage de dernière modification (`LastModifiedUtc`) et ses attributs de fichier Windows.
   - **Identité de contenu (`ResourceIdentity`) :** Représente l'empreinte binaire intrinsèque d'un contenu, déterminée par son hachage cryptographique complet SHA-256 (`PrimaryHashSha256`), son empreinte rapide de pré-filtrage (`QuickHash`), sa longueur exacte en octets et son type MIME normalisé. L'identité de contenu est immuable.
   - **Observation d'un fichier (`FileObservation`) :** Enregistre l'événement factuel d'observation d'un fichier physique lors d'un cycle de scan ou de réconciliation (date/heure de l'observation, taille relevée, horodatage disque constaté, statut de stabilité immédiat).
   - **Ressource logique (`LogicalResource`) :** Représente l'entité documentaire pérenne pour l'utilisateur (ex. « Charte Graphique 2026 »). Elle survit aux déplacements, renommages et évolutions de versions physiques du fichier, et porte les métadonnées métier, les tags utilisateur, la classification et le contexte projet.
   - **Historique des versions (`VersionHistory` / `LogicalResourceVersion`) :** Établit l'association temporelle entre une ressource logique et les identités de contenu successives associées au fil du temps (numéro de version incrémental, date de rattachement, identifiant de l'identité de contenu, commentaire de révision).

2. **Diagramme conceptuel ASCII corrigé et lisible :**

```
+--------------------------+              1..1 +--------------------------+
|       PhysicalRoot       |<------------------|       PhysicalFile       |
|--------------------------|                   |--------------------------|
| RootId (GUID) PK         |                   | FileId (GUID) PK         |
| DisplayName (TEXT)       |                   | RootId (GUID) FK         |
| NormalizedPath (TEXT)    |                   | IdentityId (GUID) FK     |----+
| VolumeSerial (TEXT)      |                   | FullPath (TEXT)          |    |
| RootState (TEXT)         |                   | RelativePath (TEXT)      |    |
| LastScannedAt (DATETIME) |                   | FileSizeBytes (INTEGER)  |    |
+--------------------------+                   | LastModifiedUtc(DATETIME)|    |
                                               | PhysicalState (TEXT)     |    |
                                               | QuarantineState (TEXT)   |    |
                                               +-------------+------------+    |
                                                             |                 |
                                                             | 1..N            |
                                                             v                 |
+--------------------------+              1..N +--------------------------+    |
|     FileObservation      |<------------------|     ResourceIdentity     |<---+
|--------------------------|                   |--------------------------|
| ObservationId (GUID) PK  |                   | IdentityId (GUID) PK     |
| FileId (GUID) FK         |                   | PrimaryHashSha256 (HEX)  |
| ObservedAtUtc (DATETIME) |                   | QuickHash (HEX)          |
| ObservedSize (INTEGER)   |                   | ByteLength (INTEGER)     |
| StabilityStatus (TEXT)   |                   | DetectedMimeType (TEXT)  |
+--------------------------+                   | DetectedFormat (TEXT)    |
                                               +-------------+------------+
                                                             ^
                                                             | 1..N
                                                             |
+--------------------------+              1..1 +-------------+------------+
|     LogicalResource      |<------------------|  LogicalResourceVersion  |
|--------------------------|                   |--------------------------|
| LogicalId (GUID) PK      |                   | VersionId (GUID) PK      |
| PreferredPhysicalFileId  |                   | LogicalId (GUID) FK      |
| CurrentIdentityId (GUID) |                   | IdentityId (GUID) FK     |
| LogicalName (TEXT)       |                   | VersionNumber (INTEGER)  |
| LogicalState (TEXT)      |                   | AttachedAtUtc (DATETIME) |
| ProjectContext (TEXT)    |                   | RevisionComment (TEXT)   |
| ClassificationLevel(INT) |                   +--------------------------+
| UserTag (TEXT)           |
| IsManualOverride(BOOLEAN)|
+------------+-------------+
             |
             | 1..N
             v
+--------------------------+
|     MetadataEntry        |
|--------------------------|
| MetadataId (GUID) PK     |
| LogicalId (GUID) FK      |
| Namespace (TEXT)         |
| Key (TEXT)               |
| ValueText (TEXT)         |
+--------------------------+
```

3. **Typologie étanche des états (COR-05) :**
   Pour éliminer tout risque de confusion ou d'utilisation interchangeable de statuts relevant de niveaux conceptuels distincts, le système sépare strictement :
   - *États des racines et volumes (`RootState`) :*
     - `ONLINE` : support de stockage accessible et prêt aux opérations d'inventaire.
     - `DISCONNECTED` : support amovible débranché ou volume réseau déconnecté. Les enregistrements enfants sont préservés intacts sans modification logique.
     - `UNREACHABLE` : racine signalant une erreur d'accès réseau ou un refus de connexion temporaire.
     - `SCANNING` : racine en cours de parcours d'inventaire ou de réconciliation.
   - *États des occurrences physiques (`PhysicalState`) :*
     - `DISCOVERED` : occurrence détectée lors du scan de métadonnées, en attente de hachage.
     - `STABLE` : occurrence dont le contenu a été lu de bout en bout et dont l'intégrité est validée.
     - `UNSTABLE` : occurrence dont les métadonnées ont divergé pendant la lecture ou soumise à verrouillage concurrent.
     - `MISSING` : occurrence physique introuvable à son chemin alors que la racine est `ONLINE`.
     - `DELETED_CONFIRMED` : occurrence physique formellement supprimée sur disque (par commande utilisateur ou trace d'audit confirmée).
   - *États des ressources logiques (`LogicalState`) :*
     - `ACTIVE` : ressource documentaire normalement accessible et liée à au moins une occurrence physique valide.
     - `ORPHANED_CONTENT` : ressource logique dont toutes les occurrences physiques associées sont actuellement indisponibles ou `MISSING`. Les métadonnées et l'historique restent pleinement consultables.
     - `ARCHIVED` : ressource marquée comme archivée par décision explicite de l'utilisateur.
     - `RESTRICTED` : ressource soumise à restrictions d'accès ou marquée comme suspecte.
   - *États de suppression et quarantaine (`QuarantineState`) :*
     - `NONE` : aucun statut de sécurité particulier.
     - `QUARANTINED` : fichier déplacé ou isolé en raison d'une menace de sécurité détectée lors du parsing.
     - `PENDING_DELETE` : suppression programmée en attente de confirmation.
     - `TRASH_DELETED` : suppression opérée avec succès vers la Corbeille Windows.
     - `PERMANENTLY_DELETED` : suppression définitive sans passage par la Corbeille.

### 5.2 Description détaillée des entités, cardinalités et scénarios d'école (COR-02)
1. **Règles de cardinalité et contraintes d'intégrité :**
   - **`PhysicalFile` -> `ResourceIdentity` (0..1) :** Un fichier physique pointe vers `NULL` lors de sa détection initiale, puis vers exactement une identité de contenu une fois son empreinte calculée.
   - **`ResourceIdentity` -> `PhysicalFile` (1..N) :** Une identité de contenu peut être partagée par de multiples occurrences physiques (fichiers dupliqués bit-à-bit sur le même volume ou des volumes distincts).
   - **`LogicalResource` -> `LogicalResourceVersion` (1..N) :** Une ressource logique possède au moins une version documentaire et conserve la séquence historique de ses révisions.
   - **`LogicalResource` -> `PreferredPhysicalFileId` (0..1) :** Désigne l'occurrence physique préférentielle retenue pour les opérations interactives (ex. instance sur SSD NVMe local rapide préférée à une copie sur disque amovible ou distant).

2. **Résolution rigoureuse des six scénarios d'école :**
   - *Scénario 1 — Remplacement de contenu au même chemin :* L'occurrence physique `PhysicalFile` conserve son identifiant unique `FileId` et son chemin `FullPath`. À l'issue du scan incrémental, la modification de taille ou de date provoque le recalcul de l'empreinte : une nouvelle `ResourceIdentity` est rattachée au `PhysicalFile`. Une nouvelle entrée `LogicalResourceVersion` est créée pour la `LogicalResource` correspondante, enregistrant l'évolution sans écraser l'historique documentaire.
   - *Scénario 2 — Déplacement ou renommage d'un fichier physique :* Le contenu binaire étant intact, `IdentityId` demeure identique. Le moteur met à jour `PhysicalFile.FullPath` et `RelativePath`. La `LogicalResource` conserve son identité pérenne et l'ensemble de ses annotations utilisateur sans aucune rupture.
   - *Scénario 3 — Multiples occurrences du même contenu (Doublons) :* Chaque occurrence matérielle dispose de son propre enregistrement `PhysicalFile`. Toutes pointent vers la même `ResourceIdentity`. Selon l'organisation choisie par l'utilisateur, ces doublons peuvent être rattachés à une même `LogicalResource` (occurrences multiples d'un même document de référence) ou à des ressources logiques distinctes (ex. même maquette réutilisée dans deux dossiers clients distincts).
   - *Scénario 4 — Plusieurs contenus successifs pour une ressource logique (Versioning) :* La ressource logique `LogicalResource` maintient un pointeur vers sa version active (`CurrentIdentityId`) tout en conservant l'accès aux versions antérieures via `LogicalResourceVersion`.
   - *Scénario 5 — Disparition d'une occurrence physique :* Lors d'un scan sur une racine connectée, si un fichier physique n'est plus détecté, son statut passe à `PhysicalState = MISSING`. La `LogicalResource` reste intacte avec l'ensemble de ses tags et passe en statut `LogicalState = ORPHANED_CONTENT` si aucune autre occurrence active ne subsiste. Aucune purge automatique n'est exécutée.
   - *Scénario 6 — Sélection de `PreferredPhysicalFileId` :* Algorithme heuristique basé sur la proximité et la rapidité du support : volume local fixe SSD > volume local fixe HDD > volume amovible USB > partage réseau distant SMB.

### 5.3 Persistance, concurrence et durabilité SQLite en mode WAL (COR-09)
**Cadrage souverain de l'arbitrage Q-004 :**  
Conformément à la décision souveraine **Q-004**, le catalogue de données de GraphiScope réside obligatoirement sur un volume local dans le profil Windows de l'utilisateur (`%LOCALAPPDATA%\GraphiScope\Catalog\`). Ce choix garantit le strict cloisonnement des profils utilisateurs et l'indépendance vis-à-vis du dossier d'installation du logiciel. L'utilisation du moteur relationnel SQLite (associé à son module plein texte FTS5) constitue une **orientation technique privilégiée et non une obligation technologique définitive**. Le catalogue n'est en aucun cas conçu pour être hébergé sur un partage réseau distant (le mode WAL y étant formellement proscrit en raison des défaillances notoires de verrouillage de fichiers sur SMB, NFS ou chemins UNC).

La gestion de la base de données locale du catalogue respecte les contraintes documentées du moteur SQLite (`SOURCE_OFFICIELLE`) :
- **Sérialisation stricte de l'écrivain en mode WAL :**
  - *Précision fondamentale :* Le mode *Write-Ahead Logging* (`PRAGMA journal_mode = WAL;`) permet la concurrence entre plusieurs lecteurs simultanés et un écrivain, mais **il n'autorise strictement qu'un seul écrivain à la fois**.
  - Deux écritures simultanées sur des connexions distinctes provoquent un verrouillage et l'erreur `SQLITE_BUSY`. GraphiScope centralise toutes les opérations d'écriture dans une file de commandes sérialisée au sein de l'hôte principal.
- **Rôle réel de `PRAGMA busy_timeout = 5000;` :**
  - Ce pragma définit exclusivement la **durée maximale d'attente active** (en millisecondes) avant que le pilote SQLite ne renvoie l'erreur de verrouillage `SQLITE_BUSY`. Il ne garantit en aucun cas l'absence d'erreur si la transaction concurrente excède cette durée.
- **Périmètre exact de `PRAGMA integrity_check;` :**
  - Cette commande valide l'**intégrité structurelle de bas niveau des pages B-Tree** : cohérence des pointeurs de pages, ordre croissant des clés d'index, absence de cycles dans les listes de pages libres (*freelist*), intégrité des enregistrements de données face aux contraintes de schéma.
  - **Distinction essentielle :** `PRAGMA integrity_check` ne vérifie en aucun cas la **cohérence logique ou métier** des données applicatives (ex. correspondance entre les chemins enregistrés et la présence réelle des fichiers sur disque, ou présence d'enregistrements logiques orphelins).
- **Rôle de `VACUUM INTO` et protocoles de sauvegarde :**
  - `VACUUM INTO 'chemin_backup.db'` produit un instantané défragmenté et cohérent de la base dans un fichier cible distinct sans interrompre les opérations de lecture (`SOURCE_OFFICIELLE`).
  - *Condition impérative et tests de restauration réels :* Cette commande génère un instantané à un instant T mais ne constitue pas une publication atomique de remplacement d'une base en exploitation. Tout fichier de sauvegarde produit doit faire l'objet d'un contrôle `PRAGMA integrity_check` avant d'être validé. De plus, un protocole de tests de restauration réels (rechargement de la base de secours après arrêt forcé du système) est indispensable pour attester de la viabilité effective du processus de reprise (`HYPOTHESE`).
- **Analyse des modes `synchronous` et arbitrage de durabilité :**
  - *En mode `synchronous = NORMAL` (`HYPOTHESE`) :* SQLite ne force pas la synchronisation disque (`fsync` / `FlushFileBuffers`) à chaque transaction, mais uniquement lors des checkpoints WAL. En cas de coupure brutale d'alimentation de la machine, les transactions les plus récentes présentes uniquement en mémoire tampon volatile peuvent être perdues, bien que la structure B-Tree reste exempte de corruption.
  - *En mode `synchronous = FULL` :* Chaque transaction est synchronisée immédiatement sur disque physique. La durabilité est absolue face aux coupures de courant, mais le débit d'écriture est sensiblement réduit.
  - *Rejet des garanties absolues :* Aucune promesse d'invulnérabilité absolue aux corruptions physiques matérielles ne peut être contractée.


## 6. Stratégie d'inventaire et de réconciliation des fichiers
### 6.1 Typologie des scans, hachage concurrent et politique d'équité du scrubbing (COR-03, COR-06)
Le moteur d'inventaire implémente une séquence structurée conçue pour équilibrer la précision de détection et la charge d'I/O disque :

1. **Scan initial et récursivité arborescente :**
   - Énumération rapide des métadonnées du système de fichiers Windows via `FindFirstFileEx` / `FindNextFile` (`WIN32_FIND_DATAW`) ou `Directory.EnumerateFileSystemEntries` managé.
   - Enregistrement immédiat de l'arborescence physique dans le catalogue (`PhysicalFile` avec statut `DISCOVERED`), puis mise en file d'attente asynchrone des fichiers pour calcul d'empreinte SHA-256 et détection de magic bytes.

2. **Scan incrémental et présomption heuristique :**
   - Comparaison rapide des métadonnées physiques (`LastModifiedUtc`, `FileSizeBytes`) avec les enregistrements `PhysicalFile` existants.
   - **Précision technique fondamentale (COR-03) :** La concordance stricte de la date et de la taille constitue une **présomption heuristique d'absence de modification**, et **non une preuve absolue d'intégrité**.
   - *Limites démontrées :* Des outils de manipulation binaire peuvent modifier le contenu d'un fichier sans en changer la taille exacte, ou réassigner artificiellement l'horodatage initial (`SetFileTime`). De même, certains systèmes de fichiers réseau présentent une granularité d'horodatage grossière (1 à 2 secondes).

3. **Hachage concurrent et traitement de l'instabilité (COR-03) :**
   - *Réfutation des garanties de `FileShare.Read` :* Ouvrir un flux avec `FileShare.Read` n'offre pas une garantie absolue de gel du contenu. Si un autre processus possédait déjà un descripteur ouvert avec accès en écriture avant notre appel, ou si des écritures surviennent sur un serveur distant SMB, le fichier peut être altéré en cours de lecture malgré notre verrou coopératif.
   - *Conditions de validation d'un hachage vérifié :*
     - Relevé initial de `LastModifiedUtc` et `Length` immédiatement avant l'ouverture du flux de données ;
     - Lecture séquentielle de bout en bout du flux d'octets sans interruption ;
     - Relevé final immédiat de `LastModifiedUtc` et `Length` après fermeture du descripteur ;
     - Confrontation stricte : si l'horodatage ou la longueur ont varié, le hachage calculé est rejeté.
   - *Typologie des statuts de hachage (COR-03) :*
     - **Hachage calculé :** Valeur binaire brute issue du flux d'octets lu.
     - **Hachage stable :** Empreinte confirmée par l'absence d'altération des métadonnées pendant l'opération de lecture.
     - **Hachage vérifié :** Empreinte confrontée et réconciliée avec le catalogue `ResourceIdentity` sous contraintes d'intégrité référentielle.
   - *Statut d'instabilité `UNSTABLE` et stratégie de réessai bornée (COR-03) :*
     - En cas de divergence de métadonnées ou d'erreur de partage (`ERROR_SHARING_VIOLATION` / `IOException`), le fichier physique passe immédiatement au statut `PhysicalState = UNSTABLE`.
     - *Stratégie de réessai :* Le moteur applique une temporisation exponentielle bornée (3 réessais maximum : 500 ms, 1 500 ms, 4 500 ms).
     - *Abandon propre et limites de la preuve :* Cette politique de réessai borné constitue un mécanisme de résilience et de découplage pour éviter de bloquer le pipeline d'inventaire ; elle ne démontre nullement à elle seule que le contenu est devenu stable si les écritures concurrentes se poursuivent. Si l'instabilité persiste au-delà des 3 tentatives, le fichier est consigné dans le journal technique d'audit sans bloquer la file d'inventaire, et conservé sous statut `UNSTABLE` sans validation de hachage, pour traitement lors d'un cycle ultérieur.

4. **Politique d'équité du scrubbing et prévention de la famine (COR-06) :**
   - *Séparation étanche des files de traitement :*
     - Pour éviter que l'arrivée continue de nouveaux fichiers non encore hachés ne monopolise le budget et ne provoque la famine (*starvation*) des fichiers anciens, le système dissocie la **File d'ingestion initiale** (fichiers nouvellement découverts) de la **Tâche de fond de scrubbing** (vérification silencieuse périodique).
   - *Limites de `LastHashVerifiedAt ASC NULLS FIRST` :* Utilisé isolément, ce prédicat ferait systématiquement passer tous les nouveaux fichiers `NULL` avant les fichiers anciens. La séparation en deux files garantit que le scrubbing traite exclusivement les fichiers déjà initialisés (`LastHashVerifiedAt IS NOT NULL`).
   - *Fonctionnement du curseur persistant :*
     - Le scrubbing enregistre dans le catalogue un curseur persistant composé du couple `(LastVerifiedTimestamp, LastVerifiedFileId)`.
     - Lors de chaque réactivation de la tâche de fond, le balayage reprend exactement à la clé suivante, interdisant toute réinitialisation en boucle au début du catalogue (`HYPOTHESE`).
   - *Ordonnancement et score de priorité dynamique :*
     - Au sein de la file de scrubbing, les fichiers sont sélectionnés par ancienneté croissante : `ORDER BY LastHashVerifiedAt ASC, FileId ASC`.
     - Un score dynamique pondère l'ancienneté par la criticité métier de la ressource logique associée.
   - *Double métrique de couverture :*
     - GraphiScope distingue formellement deux indicateurs de couverture :
       - **Couverture par nombre de fichiers :** `RatioFichiers = FichiersVérifiésDepuisN_Jours / NombreTotalFichiers`
       - **Couverture par volume de données :** `RatioVolume = OctetsVérifiésDepuisN_Jours / VolumeTotalOctets`
     - Cette dissociation est indispensable car la vérification de 1 000 fichiers de 10 Ko ne représente pas le même coût d'I/O ni le même niveau d'audit que la vérification d'un fichier vidéo de 10 Go.
   - *Unités et paramétrage du budget I/O :*
     - Le budget alloué au scrubbing est configuré en débit maximal (`Mo/s` de bande passante disque, ex. 5 Mo/s par défaut), en plafond quotidien (`Go/jour`, ex. 20 Go/jour), et subordonné à la détection d'inactivité de l'utilisateur (`GetLastInputInfo`).
   - *Absence de garantie absolue :* L'absence de starvation et le délai théorique de couverture intégrale constituent des **objectifs préliminaires d'ingénierie (`OBJECTIF_PRELIMINAIRE`)**, subordonnés au temps de mise sous tension de la machine et au profil d'activité de l'utilisateur.

### 6.2 Traitement des aléas du système de fichiers et gestion des erreurs
- **Fichiers verrouillés :** Tentative d'ouverture avec `FileShare.ReadWrite | FileShare.Delete`. En cas d'échec persistant (verrou exclusif par application tierce type Photoshop ou Word), le fichier passe en `UNSTABLE` sans interrompre l'inventaire.
- **Accès refusés (`UnauthorizedAccessException`) :** Consignation dans `AnalysisState` avec statut `ACCESS_DENIED` et poursuite de l'exploration.
- **Déconnexion de racines :** Si un volume devient inaccessible, la racine passe à `RootState = DISCONNECTED`. Les enregistrements d'occurrences et de ressources logiques associés demeurent intacts dans le catalogue sans aucune suppression logique (`DECISION_PROPRIETAIRE`).

### 6.3 Statut et limites de `FileSystemWatcher`
`FileSystemWatcher` est expressément qualifié d'outil d'alerte opportuniste en temps réel (`SOURCE_OFFICIELLE`, `RESERVES-ET-CONTRAINTES-TRANSVERSALES.md`).
- **Faiblesse intrinsèque :** Sous forte charge d'I/O (décompression massive, renommages en masse), le tampon d'événements du noyau Windows s'épuise (`InternalBufferOverflowException`), provoquant la perte silencieuse d'événements.
- **Garde-fou d'ingénierie :** Dès qu'un débordement est signalé, GraphiScope déclenche automatiquement un scan incrémental de réconciliation sur le répertoire affecté (`HYPOTHESE`).


## 7. Architecture de sécurité et confinement des parseurs
### 7.1 Postulat de non-fiabilité des données d'entrée
Conformément au principe directeur de sécurité `SAFE-01`, tout fichier analysé par GraphiScope est considéré comme **potentiellement hostile, corrompu ou piégé** jusqu'à ce que son analyse soit achevée sans incident.

### 7.2 Confinement du Worker et dissociation formelle des mécanismes d'isolation (COR-04)
Pour éviter toute ambiguïté conceptuelle, l'architecture distingue rigoureusement quatre niveaux de protection :
1. **Séparation de processus :** L'hôte Kestrel et le Worker s'exécutent dans deux espaces d'adressage distincts.
2. **Isolation des pannes (Crash Isolation) :** Un plantage du Worker (faute de pagination, violation d'accès en mémoire native C++, dépassement de capacité de pile) n'entraîne pas la terminaison du processus hôte Kestrel ni la fermeture de l'interface graphique.
3. **Limitation des ressources :** Régulation de la consommation de mémoire vive et de temps processeur via les Job Objects Win32.
4. **Confinement de sécurité (Sandboxing) :** Restrictions strictes des privilèges, de l'accès au système de fichiers et des communications réseau.

- **Spécification des restrictions appliquées au Worker Process (`HYPOTHESE`) :**
  - *Permissions d'accès aux répertoires :* Le Worker ne dispose d'aucun accès direct en écriture sur les volumes sources analysés. Les fichiers à analyser lui sont désignés par des handles de lecture ouverts transmis par l'hôte, ou par des chemins validés.
  - *Droits d'écriture :* L'écriture physique est strictement cantonnée au répertoire de travail temporaire dédié de GraphiScope (`%LOCALAPPDATA%\GraphiScope\Scratch\`).
  - *Coupure réseau absolue :* Le sous-processus Worker ne possède aucune interface réseau active ; l'ouverture de sockets TCP/UDP et les appels HTTP/DNS lui sont formellement interdits au niveau applicatif et système.
  - *Canal IPC :* Communication exclusive via des canaux de flux anonymes (`AnonymousPipe`) avec protocole de messages sérialisé binaire strict.
  - *Héritage des handles Windows :* Lors de la création du sous-processus Worker via `CreateProcessW`, le drapeau d'héritage est positionné à `bInheritHandles = FALSE`. Seuls les handles explicitement requis (canaux IPC) sont transmis via la structure `STARTUPINFOEXW` et la liste d'attributs `PROC_THREAD_ATTRIBUTE_HANDLE_LIST` (`SOURCE_OFFICIELLE`).
  - *Régulation par Job Objects :*
    - `JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE` : garantit l'arrêt immédiat du Worker si l'hôte se termine inopinément ;
    - `JOB_OBJECT_LIMIT_JOB_MEMORY` : plafond maximal de mémoire virtuelle (ex. 1 024 Mo par défaut) provoquant la levée d'une exception contrôlée en cas de tentative d'épuisement mémoire ;
    - `JOB_OBJECT_LIMIT_PROCESS_MEMORY` : plafond par processus individuel (ex. 512 Mo).
- **Limites réelles des mécanismes et qualification de preuve (COR-04) :**
  - *Limites de `Restricted Token` et `Low Integrity Level` :* Sous Windows standard, un jeton restreint ou un niveau d'intégrité bas réduit les droits d'écriture, mais ne bloque pas automatiquement la lecture des objets système non sécurisés ni l'exploitation de failles de privilèges du noyau.
  - *Limites des Job Objects :* Les Job Objects ne constituent pas un bac à sable de sécurité cryptographique ; ils régulent les ressources et le cycle de vie des processus sans empêcher la lecture de fichiers si le compte utilisateur le permet.
  - *Statut de preuve :* Tant que des bancs d'essais d'intrusion dédiés n'ont pas été menés en Phase B, le Worker est formellement qualifié de **mécanisme d'isolation de panne et de régulation de ressources**, et non de bac à sable de sécurité démontré (`HYPOTHESE`).

### 7.3 Gestion adaptative des ressources et seuils configurables (COR-R02)
Les seuils de consommation sont dissociés entre limites de sécurité du Worker et dimensionnement fonctionnel du catalogue (Q-006) :
- Plafond mémoire alloué au Worker : paramètre configurable (`WorkerMemoryCapMb = 1024` par défaut) ;
- Délai d'expiration par fichier (*Timeout*) : interruption automatique du parsing après 30 secondes (`FileAnalysisTimeoutSec = 30`).

### 7.4 Sécurisation des flux IPC
Le protocole d'échange entre l'Hôte et le Worker repose sur des trames typées délimitées par longueur fixe d'en-tête, interdisant toute injection de commande arbitraire.


## 8. Stratégie d'exploration des archives et sécurité des conteneurs
### 8.1 Sécurisation du parcours des chemins, descripteurs ouverts et résistance au TOCTOU (COR-01)
Face aux menaces de traversée de répertoires (*Zip Slip*, CWE-22), de redirection par jonctions/liens symboliques NTFS et de courses temporelles TOCTOU (*Time-Of-Check to Time-Of-Use*, CWE-367), l'architecture implémente une stratégie de défense en profondeur rigoureuse (`HYPOTHESE`) :

1. **Réfutation et limites fondamentales de `FILE_FLAG_OPEN_REPARSE_POINT` (COR-01) :**
   - *Limitation critique documentée de Win32 :* Lorsqu'un processus ouvre un chemin textuel (ex. `C:\Sandbox\DossierParent\FichierCible.txt`) en spécifiant le drapeau `FILE_FLAG_OPEN_REPARSE_POINT`, le sous-système d'E/S de Windows applique ce drapeau **exclusivement au dernier élément du chemin (la feuille)** (`SOURCE_OFFICIELLE`).
   - **Conséquence de sécurité majeure :** Si un répertoire parent intermédiaire (ex. `DossierParent`) est une jonction NTFS ou un lien symbolique pointant vers `C:\Windows\System32`, le noyau Windows traverse automatiquement la jonction parente sans lever d'erreur ! Le drapeau final n'empêche nullement la redirection du chemin parent.

2. **Réfutation de la vérification séquentielle textuelle face au TOCTOU (COR-01) :**
   - Une simple vérification textuelle composant par composant (vérifier que chaque dossier parent existe et n'est pas une jonction, puis appeler `File.Create` sur le chemin complet) est **intrinsèquement vulnérable aux courses temporelles TOCTOU**.
   - Entre l'instant où un processus vérifie un dossier parent et l'instant où il crée le fichier cible, un processus concurrent ou une archive piégée peut substituer le dossier parent par une jonction NTFS (*Symlink Race*).

3. **Distinction entre la protection de `CREATE_NEW` et les handles ouverts (COR-01) :**
   - Le drapeau Win32 `CREATE_NEW` (`CreationDisposition = CREATE_NEW`) garantit de manière atomique que le fichier cible **n'existait pas préalablement**. Si le fichier existe déjà, l'appel échoue immédiatement avec `ERROR_FILE_EXISTS`.
   - **Ce que `CREATE_NEW` ne protège pas :** Il ne protège en rien contre le détournement du chemin parent ! Si le répertoire parent a été redirigé vers un dossier sensible, `CREATE_NEW` créera avec succès un nouveau fichier malveillant dans cet emplacement sensible hors du bac à sable.
   - **Protection réelle apportée par un handle de fichier ouvert :** Seule la détention d'un descripteur de fichier ouvert (`SafeFileHandle`) avec des droits de partage exclusifs garantit que les opérations d'écriture s'exécutent sur l'objet physique immuable verrouillé, sans possibilité de substitution en cours de route.

4. **Architecture de sécurisation par opérations relatives aux handles de répertoires (COR-01) :**
   Pour neutraliser les courses TOCTOU lors de l'extraction, l'architecture retient les principes d'ingénierie suivants :
   - *Conservation des handles de répertoires validés :*
     - Chaque répertoire intermédiaire de l'arborescence depuis la racine du bac à sable (`SandboxDir`) jusqu'au dossier parent immédiat est ouvert séquentiellement via `CreateFileW` avec `FILE_FLAG_OPEN_REPARSE_POINT` et `FILE_FLAG_BACKUP_SEMANTICS`.
     - L'absence d'attribut `FILE_ATTRIBUTE_REPARSE_POINT` est formellement vérifiée sur le handle ouvert du répertoire.
     - Le handle du répertoire est conservé ouvert avec un verrou de partage restreint pendant l'opération sur ses enfants, empêchant son renommage ou son remplacement concurrent par un tiers.
   - *Contrôle d'identité physique de l'objet ouvert (`FileIdInfo`) :*
     - Après chaque ouverture d'un handle de fichier ou de répertoire, GraphiScope appelle l'API Win32 `GetFileInformationByHandleEx` avec la classe d'information `FileIdInfo` (`SOURCE_OFFICIELLE`).
     - Cette structure fournit le numéro de série du volume (`VolumeSerialNumber`) et l'identifiant physique 128 bits du fichier (`FileId`).
     - Le système contrôle que l'objet ouvert réside bien sur le même volume physique que la racine du bac à sable et que son chemin résolu (`GetFinalPathNameByHandleW`) se situe strictement à l'intérieur de l'arborescence autorisée.
   - *Limites techniques et opérations strictement relatives :*
     - Détenir un handle et contrôler `FileIdInfo` ne suffisent pas si les appels d'E/S ultérieurs repassent par des chemins textuels absolus. Les opérations d'écriture doivent s'effectuer exclusivement via le `SafeFileHandle` déjà ouvert ou par des ouvertures relatives sans recalcul de chemin textuel.
     - De plus, l'efficacité dépend des caractéristiques du système de fichiers hôte (pleinement supporté sur NTFS et ReFS, non transposable à des partages distants non-Windows).
   - *Statut d'ingénierie :* Cet ensemble d'opérations constitue une **hypothèse de sécurité d'ingénierie soumise à banc d'essais d'intrusion en Phase B (`HYPOTHESE`)**. Aucune garantie absolue ne peut être proclamée sans tests d'intrusion réels.

```
[Octets bruts d'archive]
           |
           v
[Phase 1 : Filtrage lexical strict préalable]
   |--> Rejet immédiat si chemin absolu (C:\, /, \)
   |--> Rejet immédiat si chemin UNC (\\serveur\partage) ou périphérique (\\.\, \\?\)
   |--> Rejet immédiat si flux alternatif NTFS (présence de :)
   |--> Rejet immédiat si segment parent (..) ou nom DOS réservé (CON, PRN, AUX, NUL...)
           |
           | (Chemin lexicalement valide)
           v
[Phase 2 : Parcours et verrouillage des handles de répertoires parents]
   Pour chaque segment depuis SandboxDir jusqu'au dossier cible :
     1. Ouverture du handle de dossier avec FILE_FLAG_OPEN_REPARSE_POINT
     2. Contrôle d'absence de FILE_ATTRIBUTE_REPARSE_POINT sur le handle
     3. Maintien du handle ouvert pour verrouiller le segment face aux substitutions
           |
           | (Arborescence parente verrouillée et exempte de redirections)
           v
[Phase 3 : Création atomique et écriture exclusive sur SafeFileHandle]
   1. Création avec CREATE_NEW et FILE_FLAG_OPEN_REPARSE_POINT
   2. Validation d'identité d'objet via GetFileInformationByHandleEx (FileIdInfo)
   3. Résolution du chemin canonique par handle (GetFinalPathNameByHandleW)
   4. Écritures opérées exclusivement via le SafeFileHandle validé (anti-TOCTOU)
           |
           v
[EXTRACTION SÉCURISÉE OU REJET CATÉGORIQUE]
```

### 8.2 Détection des cycles, bombes de décompression et limites étendues (COR-07)
L'exploration des conteneurs imbriqués impose une dissociation rigoureuse entre la détection des boucles logiques et la protection contre l'épuisement des ressources :

1. **Moments de calcul SHA-256 pour les conteneurs imbriqués :**
   - *Conteneur physique (niveau 0) :* Calcul de l'empreinte SHA-256 sur le flux brut du conteneur physique (`PhysicalFile`).
   - *Archive imbriquée (niveau N > 0) :* Calcul au fil de l'eau sur le flux d'octets décompressé en mémoire tampon, **avant toute écriture disque et avant toute exploration récursive de la sous-archive** (`HYPOTHESE`). Confrontation à la pile active des ancêtres directs (`AncestorStack<Sha256>`) pour couper les boucles récursives (`RECURSION_CYCLE_DETECTED`).
   - *Distinction avec les doublons :* Un fichier rencontré dont l'empreinte existe dans le catalogue mais **n'appartient pas à la pile des ancêtres actifs** n'est pas un cycle, mais un contenu dupliqué légitime.

2. **Surveillance dynamique continue et qualification des seuils de protection face aux archives piégées (Q-006, COR-07, ANO-04) :**
   Conformément à la décision souveraine **Q-006 (Option 3)**, GraphiScope n'impose **aucun quota fonctionnel arbitraire universel** : le catalogue a vocation à indexer tous les conteneurs et fichiers autorisés par l'utilisateur sans bridage artificiel de volume, de taille unitaire, de nombre d'entrées ou de profondeur.  
   Néanmoins, la sécurité et la stabilité du système hôte imposent des garde-fous techniques rigoureux face à des fichiers potentiellement malveillants ou corrompus. Pour écarter toute confusion entre quotas fonctionnels (proscrits) et protections de sécurité légitimes (requises), les seuils d'ingénierie identifiés sont individuellement qualifiés ci-dessous selon leur nature, leur rôle et leur compatibilité avec Q-006 :

   | Paramètre d'ingénierie | Valeur indicative de référence | Catégorie fonctionnelle (Mandat COR-005) | Risque traité & Justification technique | Dynamisme & Configurabilité | Compatibilité Q-006 |
   | :--- | :---: | :--- | :--- | :--- | :---: |
   | **Ratio d'expansion volumique** | `100:1` | **Protection de sécurité** | Attaque par bombe de décompression (*Zip Bomb* récursive ou overlapping créant un volume gigantesque à partir d'un petit fichier). Permet de suspecter une malveillance sans bloquer les fichiers compressibles légitimes si la progression I/O reste saine. | Paramètre configurable (`SecurityExpansionRatioMax`) ; déclenche une vérification de progression plutôt qu'un arrêt aveugle. | **COMPATIBLE** (Protection anti-malveillance légitime) |
   | **Taille décompressée unitaire** | `500 Mo` (indicatif) | **Paramètre configurable & Seuil technique de bascule** | Épuisement de la mémoire vive du Worker Process lors de la décompression de fichiers monolithiques lourds. | Seuil technique de bascule : au-delà de cette valeur, l'extraction bascule obligatoirement en streaming direct vers le disque de scratch sans allocation d'un buffer mémoire complet. Ne bride aucunement la taille maximale du fichier traité. | **COMPATIBLE** (Régulation I/O sans bridage de taille) |
   | **Décompression des tailles inconnues (`0x08`)** | N/A (Streaming continu) | **Protection de sécurité & Mécanisme de streaming** | Falsification d'en-tête ZIP masquant la taille décompressée réelle via le drapeau Data Descriptor (`0x08`). | Streaming d'octets avec compteur d'avancement et contrôle dynamique de l'espace disque résiduel, sans refuser le fichier. | **COMPATIBLE** (Gestion technique de format) |
   | **Taille des lots d'indexation (*Batching*)** | `10 000 entrées` | **Paramètre configurable d'ordonnancement** | Saturation des files d'attente mémoire lors de l'énumération d'archives contenant des centaines de milliers de fichiers. | Découpage du parcours en lots séquentiels (`batch`) enregistrés en base SQLite par tranches. L'archive entière est explorée sans plafond sur le total d'entrées. | **COMPATIBLE** (Ordonnancement technique sans quota total) |
   | **Budget d'espace scratch par conteneur** | `2 Go` (indicatif) | **Seuil dynamique & Paramètre configurable** | Saturation de la partition temporaire hôte lors de décompressions massives simultanées. | Asservi dynamiquement à l'espace disque libre réel disponible sur la partition scratch (`%LOCALAPPDATA%`), avec nettoyage immédiat des fichiers temporaires au fil de l'analyse. | **COMPATIBLE** (Gestion dynamique des ressources réelles) |
   | **Budget scratch cumulé par tâche** | `5 Go` (indicatif) | **Seuil dynamique & Paramètre configurable** | Dérive volumétrique lors du parcours de branches d'archives fortement imbriquées. | Seuil dynamique recalculé à partir des ressources physiques disponibles sur la partition scratch ; arrêt ordonné si l'espace libre approche d'un seuil critique. | **COMPATIBLE** (Protection dynamique système) |
   | **Profondeur d'imbrication** | `AncestorStack` (sans limite rigide) | **Protection de sécurité anti-boucle** | Boucles récursives infinies (archive A contenant archive B contenant archive A). | La sécurité repose prioritairement sur la comparaison cryptographique SHA-256 dans la pile active des ancêtres (`AncestorStack<Sha256>`). La valeur indicative de profondeur (ex. 5 niveaux) ne constitue qu'un seuil d'alerte diagnostique et non un plafond arbitraire rigide. | **COMPATIBLE** (Anti-boucle sans plafond fonctionnel) |
   | **Watchdog CPU par conteneur** | `30 s CPU` (indicatif) | **Condition d'alerte & Surveillance dynamique** | Boucle infinie dans un parseur ou algorithme de décompression défaillant bloquant un thread à 100 % CPU. | Déclenchement d'un `CancellationToken` coopératif invitant le Worker à rendre la main. Si le traitement progresse régulièrement (débit d'I/O mesurable), le traitement se poursuit sans interruption brutale. | **COMPATIBLE** (Surveillance anti-blocage) |
   | **Durée globale max par archive** | `60 s` (indicatif) | **Condition d'alerte & Paramètre configurable** | Blocage prolongé de la file de tâches d'inventaire sur un conteneur corrompu. | Seuil d'évaluation de l'inactivité I/O. Interruption propre avec bascule en statut d'analyse différée sans perte des données déjà extraites. | **COMPATIBLE** (Gestion de file sans perte) |
   | **Surveillance de l'espace disque résiduel** | `1 Go libre` | **Condition d'arrêt d'urgence du système hôte** | Saturation complète du volume disque Windows provoquant l'instabilité de l'OS hôte ou le crash du système de fichiers. | Mesure continue via l'API Win32 `GetDiskFreeSpaceEx`. Interruption immédiate et ordonnée de toute écriture scratch dès que l'espace libre passe sous ce seuil critique de sécurité vitale. | **COMPATIBLE** (Sauvegarde vitale de l'OS hôte) |

   - **Règle absolue d'architecture (Q-006) :** Aucun fichier légitime n'est rejeté par principe sur la base d'un quota statique préétabli. L'interruption d'une analyse ne peut être provoquée que par la conjonction avérée d'un signal de risque matériel critique (manque d'espace disque résiduel, pression mémoire globale de Windows via `GlobalMemoryStatusEx`) ou d'une signature d'attaque malveillante avérée (boucle récursive de hachage, bombe de décompression).



## 9. Extraction, classification et prévisualisation
### 9.1 Découpage fonctionnel de la chaîne de traitement
La chaîne de traitement progresse séquentiellement :
1. *Détection physique L0 :* Identification du format par signature binaire (Magic Bytes) sur les premiers octets du flux.
2. *Contrôle d'intégrité L1 :* Calcul d'empreinte SHA-256 et relevé des attributs de métadonnées intrinsèques.
3. *Extraction et classification L2-L4 :* Extraction des métadonnées métier et application des règles de classification.
4. *Génération de prévisualisations :* Décodage sécurisé dans le Worker et génération de vignettes WebP en cache local.

### 9.2 Taxonomie des 5 niveaux de classification, règles L4 et surcharges manuelles (COR-15)
Pour garantir la cohérence et l'explicabilité du classement, le modèle de classification est formellement spécifié :
- **Niveau L0 — Détection physique :** Signature binaire d'octets magiques (ex. `0x89 0x50 0x4E 0x47` pour PNG).
- **Niveau L1 — Attributs intrinsèques :** Taille, dates, type MIME normalisé.
- **Niveau L2 — Analyse lexicale de nommage :** Expressions régulières configurables sur le nom de fichier (motifs `_v01`, `_final`, dates ISO `2026-11`).
- **Niveau L3 — Analyse contextuelle d'arborescence :** Exploitation de la hiérarchie des répertoires parents (ex. `CLIENTS/ACME/CATALOGUES/`).
- **Niveau L4 — Règles personnalisées utilisateur et corrections manuelles (COR-15) :**
  - *Distinction formelle entre Règles personnalisées et Corrections manuelles :*
    - **Règles personnalisées :** Règles automatiques configurées par l'utilisateur (critères multi-attributs) appliquées lors des scans automatiques.
    - **Corrections manuelles :** Surcharges ponctuelles affectées directement par l'utilisateur à une ressource logique spécifique via l'interface graphique.
  - *Ordre de priorité absolu :*  
    `Correction manuelle ponctuelle > Règle personnalisée L4 > Contexte arborescence L3 > Analyse lexicale L2 > Attributs L1 > Signature L0`
  - *Portée et persistance :*
    - Toute correction manuelle utilisateur positionne formellement l'attribut `IsManualOverride = TRUE` sur la `LogicalResource`.
    - **Règle absolue de persistance :** Une surcharge manuelle L4 persiste à travers tous les rescans et réconciliations ultérieurs. Elle ne peut être écrasée par aucun algorithme automatique (`DECISION_PROPRIETAIRE`).
  - *Traçabilité complète :* Le catalogue enregistre pour chaque ressource le champ `RuleMatched` (identifiant de la règle appliquée pour les niveaux L0 à L3), ainsi que l'horodatage et l'auteur de la surcharge manuelle pour le niveau L4 (`HYPOTHESE`).
  - *Comportement lors des évolutions physiques :*
    - *Déplacement ou renommage physique :* La classification et les surcharges L4 sont intégralement préservées car elles sont attachées à la `LogicalResource` et non au chemin physique.
    - *Remplacement du contenu binaire :* Si le contenu d'un fichier physique change substantiellement (nouvelle `ResourceIdentity`), le système conserve la surcharge manuelle documentaire tout en signalant une révision de contenu en attente de confirmation (`REVISION_PENDING_CONFIRMATION`), empêchant qu'une annotation manuelle ne soit appliquée silencieusement à un contenu radically différent.

### 9.3 Typologie des relations dans le graphe documentaire et recherche FTS5 (Q-003, ANO-03)
Conformément à la décision souveraine **Q-003**, le graphe de données de GraphiScope modélise l'ensemble des liaisons entre les entités documentaires selon des règles strictes de traçabilité, de limitation et de validation humaine :

1. **Distinction conceptuelle fondamentale entre `MEME_CONTENU` et `COPIE_DE` :**
   - **`MEME_CONTENU` (Égalité stricte de contenu) :** Relation déterministe établie exclusivement par la concordance cryptographique exacte des empreintes SHA-256 sur les flux binaires décompressés. Deux occurrences matérielles partageant la même `ResourceIdentity` sont formellement identiques bit-à-bit.
   - **`COPIE_DE` / Dérivation (Relation de filiation ou provenance) :** Relation historique ou logique indiquant qu'un document est issu d'un autre (ex. duplication avec modification ultérieure, export dans un autre format, version dérivée). **Règle impérative (Q-003) :** Une relation de provenance ou de dérivation ne doit **JAMAIS** être déduite automatiquement de la seule ressemblance de contenu, de nommage ou d'horodatage. Elle constitue une relation candidate nécessitant la confirmation explicite de l'utilisateur.

2. **Classification des relations heuristiques par niveau d'inférence (D2, D3, D4) :**
   - **Niveau D2 — Proximité lexicale et motifs de nommage :** Détection de variantes de révision basées sur des suffixes et conventions normalisées (ex. `_v01`, `_v02`, `_final`, `_revisé`, timestamps ISO). La ressemblance lexicale suggère un lien de parenté logique potentiel.
   - **Niveau D3 — Proximité arborescente et contexte dossier :** Détection d'associations contextuelles issues de la cohabitation dans une même arborescence ou structure de répertoires dédiée (ex. dossier de projet client, dossier d'archives exportées).
   - **Niveau D4 — Métadonnées descriptives partagées et similarité perceptuelle :** Concordance d'identifiants XMP (`DocumentID`, `OriginalDocumentID`, `InstanceID`), métadonnées EXIF/IPTC communes, ou similarité visuelle calculée par hachage perceptuel (`pHash`) sur images matricielles.

3. **Gouvernance et cycle de vie des relations candidates (Q-003) :**
   - **Statut obligatoire `CANDIDATE` :** Toute relation déduite par les heuristiques D2, D3 ou D4 est enregistrée avec le statut `CANDIDATE`. **Aucune confirmation automatique, aucune fusion d'entités et aucune modification silencieuse du graphe n'est permise.**
   - **Limitation du nombre de candidats présentés :** Pour prévenir la surcharge cognitive et la saturation de l'interface utilisateur, le moteur applique une limitation du nombre de candidats suggérés simultanément pour une ressource donnée (triés par score de confiance traçable et score de proximité).
   - **Confirmation ou rejet souverain par l'utilisateur :**
     - Si l'utilisateur confirme la proposition, la relation bascule au statut `CONFIRMED` et s'inscrit de manière pérenne dans le graphe relationnel.
     - Si l'utilisateur rejette la proposition, le rejet est formellement consigné dans le catalogue (`REJECTED`) afin d'interdire au moteur de reproposer cette même liaison lors des scans ultérieurs.
   - **Cas d'application pratique des archives créatives (Freepik / Magnific) :** Les conteneurs d'archives issus de banques d'actifs créatifs regroupent fréquemment un fichier vectoriel maître (`.eps`, `.ai`), un aperçu matriciel d'accompagnement (`.jpg`, `.png`) et un texte de licence (`.txt`). Ces grappes de fichiers constituent une illustration pratique de relations candidates D2/D3 d'accompagnement, sans se substituer à la portée générale de l'arbitrage Q-003.

4. **Module d'indexation plein texte SQLite FTS5 :**
   - Intégration du moteur plein texte SQLite FTS5 avec le tokenizer `unicode61` (gestion des diacritiques, insensibilité à la casse, normalisation Unicode) pour l'indexation instantanée des métadonnées, chemins relatifs, tags utilisateurs et textes bruts extraits.


## 10. Traitement des familles de formats
### 10.1 Couverture exhaustive des formats Office et spécifications techniques (COR-08)
Le traitement des documents bureautiques repose sur le respect scrupuleux des décisions souveraines Q-001 et Q-002 :

1. **Prise en charge exhaustive des formats Office modernes OpenXML (ECMA-376) :**
   - Formats traités : Classeurs (`XLSX`, `XLSM`, `XLTX`, `XLTM`), Documents (`DOCX`, `DOCM`, `DOTX`, `DOTM`), Présentations (`PPTX`, `PPTM`, `PPSX`, `PPSM`, `POTX`, `POTM`).
   - Couverture sans omission des modèles (`DOTX`, `DOTM`, `XLTX`, `XLTM`, `POTX`, `POTM`) et des variantes avec macros autorisées (`DOCM`, `XLSM`, `PPTM`, `PPSM`).

2. **Correction technique sur l'extraction des cellules Excel et `sharedStrings.xml` (COR-08) :**
   - *Spécification exacte du format OpenXML SpreadsheetML :*
     - Dans une feuille de calcul (`xl/worksheets/sheet1.xml`), une cellule contenant du texte est généralement balisée avec l'attribut de type `t="s"` (chaîne partagée).
     - **Correction formelle :** Dans une telle cellule, la balise valeur `<v>42</v>` contient **un index numérique entier** (en base 0) pointant vers l'entrée correspondante de la table globale des chaînes partagées `xl/sharedStrings.xml` (`SOURCE_OFFICIELLE`, ECMA-376).
     - Elle ne contient **en aucun cas directement le texte affichable** ! Pour extraire le texte, le parseur doit obligatoirement charger `xl/sharedStrings.xml` et résoudre l'index `<si>` correspondant.
   - *Limites d'ingénierie spécifiques aux cellules Excel :*
     - *Chaînes inline (`t="inlineStr"`) :* Le texte est contenu directement dans `<is><t>Texte</t></is>` sans passer par `sharedStrings.xml`.
     - *Cellules formules de type chaîne (`t="str"`) :* La balise `<v>` contient la dernière valeur textuelle calculée mise en cache. Conformément à Q-001, cette valeur en cache n'est en aucun cas présentée comme un résultat exploitable ou validé.
     - *Formules partagées (`f t="shared"`) et matricielles (`f t="array"`) :* La formule principale est définie sur la cellule maîtresse ; les cellules dépendantes partagent l'index de formule (`si="0"`).
     - *Dates et nombres formatés :* Excel stocke les dates sous forme de nombres décimaux sériels (nombre de jours depuis le 1er janvier 1900 ou 1904). Le texte affiché dépend de l'attribut de style de cellule `s="ID"` pointant vers la table des styles `xl/styles.xml` (`numFmtId`). L'extraction de texte brut sans moteur de rendu extrait le nombre sériel brut.
     - *Références externes :* Les formules contenant des liaisons externes (ex. `[Budget2025.xlsx]Feuil1!A1`) sont extraites sous leur forme textuelle brute sans résolution du lien externe.

3. **Détection robuste des composants VBA et XLM sans chemins fixes (COR-08) :**
   - *Pratique proscrite :* Se fier à des chemins de fichiers fixes tels que `xl/vbaProject.bin` ou `word/vbaProject.bin`. Un document valide peut nommer son conteneur binaire différemment ou le placer dans un sous-dossier non standard.
   - *Méthode robuste retenue :* L'identification des macros s'appuie rigoureusement sur le modèle de relations OpenXML :
     - Analyse du fichier des types de contenu `[Content_Types].xml` à la recherche du type MIME `application/vnd.ms-office.vbaProject` ;
     - Analyse des fichiers de relations `.rels` à la recherche du type de relation standardisé `http://schemas.microsoft.com/office/2006/relationships/vbaProject`.
   - Tout composant VBA détecté est recensé de manière passive (`HasMacros = TRUE`) sans jamais exécuter de code (`SAFE-04`).

4. **Classeurs binaires Excel XLSB (`[MS-XLSB]`) :**
   - Format binaire sous conteneur ZIP, distinct des formats OLE2 de Q-002. Extraction des métadonnées et flux de formules textuelles via parseur binaire BIFF12 sans recalcul (`HYPOTHESE`).

5. **Formats binaires Office historiques OLE2 / Compound File (Q-002) :**
   - Formats `DOC`, `XLS`, `PPT` traités selon Q-002 : support partiel et qualifié via parseur OLE2 (`MS-CFB`), extraction des flux de texte brut accessibles et métadonnées `SummaryInformation` sans promesse de reconstitution visuelle.

### 10.2 Spécifications d'ingénierie par famille de formats, sécurité SVG et portée des polices (COR-17, COR-R08)
Le tableau ci-dessous récapitule les spécifications et limites par famille de formats :

| Famille | Formats | Composants candidats | Stratégie technique et limites formelles | Niveau de preuve |
| :--- | :--- | :--- | :--- | :--- |
| **Images matricielles** | `JPG`, `PNG`, `WebP`, `BMP`, `GIF` | SixLabors.ImageSharp / SkiaSharp | Décodage confiné dans le Worker. Extraction EXIF/IPTC. Rendu WebP en cache. Choix de bibliothèque ouvert (Question O-03). | `HYPOTHESE` |
| **TIFF & BigTIFF** | `TIF`, `TIFF` | ImageSharp / LibTiff.NET | TIFF 32 bits classique prioritaire. BigTIFF (> 4 Go, offsets 64 bits) conditionné aux capacités vérifiées. Échec de vignette non bloquant (Q-008). | `DECISION_PROPRIETAIRE` |
| **Graphismes vectoriels** | `SVG`, `EPS` | Svg.Skia / Décodeur d'en-tête EPS | **Sécurité SVG stricte (COR-17) :** Interdiction absolue de l'exécution de scripts (`<script>`, gestionnaires `onload`/`onclick`), désactivation formelle des DTD externes (`XmlResolver = null`), filtrage des balises `<foreignObject>` et styles CSS distants (`@import`, `url()`), coupure des accès réseau distants. Pour EPS : extraction de la vignette binaire TIFF/WMF incorporée sans interpréteur PostScript in-process (risque bloquant `SAFE-04`). | `DECISION_PROPRIETAIRE` |
| **Documents Adobe** | `PSD`, `PSB`, `INDD`, `AI` | Parser managé C# / Extraction XMP | **Capacités réelles qualifiées (COR-17) :** PSD/PSB : extraction directe de l'image composite aplatie (`Composite Layer`) sans recomposition des calques complexes. INDD : lecture exclusive des vignettes JPEG incorporées et des paquets XMP, sans lecture native du document InDesign. AI : extraction du flux PDF encapsulé ou des vignettes XMP. | `HYPOTHESE` |
| **Microsoft Office moderne** | `DOCX`, `DOCM`, `DOTX`, `DOTM`, `XLSX`, `XLSM`, `XLTX`, `XLTM`, `PPTX`, `PPTM`, `PPSX`, `PPSM`, `POTX`, `POTM` | DocumentFormat.OpenXml / System.IO.Compression | Extraction textuelle structurée via OpenXML. Résolution des index `sharedStrings.xml` (COR-08). Formules textuelles sans recalcul ni valeur en cache exploitable (Q-001). Repérage passif des composants VBA par relations `.rels`. | `DECISION_PROPRIETAIRE` |
| **Microsoft Office binaire** | `DOC`, `XLS`, `PPT` | Parseur OLE2 / Compound File (`MS-CFB`) | Prise en charge partielle et qualifiée (Q-002). Extraction des métadonnées `SummaryInformation` et flux de texte accessibles sans promesse d'exhaustivité de mise en page. Repérage passif des flux macros. Option O-04 ouverte. | `DECISION_PROPRIETAIRE` |
| **Classeurs binaires Excel** | `XLSB` | Parseur binaire OOXML BIFF12 | Format binaire sous conteneur ZIP (`[MS-XLSB]`), distinct de Q-002. Extraction des formules textuelles sans recalcul. | `HYPOTHESE` |
| **Polices de caractères** | `TTF`, `OTF`, `WOFF`, `WOFF2` | DirectWrite P/Invoke / Parser OpenType C# | Extraction des métadonnées via tables (`name`, `head`, `os2`). Prévisualisation Web confinée via `@font-face`. Installation globale système strictement exclue ; option profil utilisateur Windows ouverte sous O-01 (COR-R08). | `HYPOTHESE` / `DECISION_PROPRIETAIRE` (en attente) |
| **Archives créatives (illustration Q-003)** | `ZIP` (packages Freepik/Magnific) | SharpCompress / Moteur de relations internes | Détection des grappes de fichiers créatifs (vectoriel source + aperçu raster + licence). Établissement de liaisons heuristiques candidates avec statut obligatoire `CANDIDATE` soumis à confirmation/rejet utilisateur (Q-003). | `DECISION_PROPRIETAIRE` |
| **Audio & Vidéo** | `MP4`, `MOV`, `AVI`, `MP3`, `WAV` | TagLib-Sharp / Wrapper FFmpeg conditionnel | Métadonnées de base via TagLib-Sharp. Vignettes vidéo via FFmpeg strictement conditionnées à l'audit complet de conformité juridique (Q-005, Question O-02). | `DECISION_PROPRIETAIRE` |
| **CAO Textile** | `DXF` (AAMA/ASTM D6673), Lectra, Gerber | Étude préliminaire DXF-AAMA / Lectra | Cadrage strict : faisabilité préliminaire sur géométries DXF-AAMA 2D ouvertes. Aucune promesse de support complet sans validation sur échantillons réels. Gerber PCB formellement exclu. | `DECISION_PROPRIETAIRE` |

- **Portée de l'installation des polices (COR-R08) :**
  - *Installation système globale (System-wide) :* Écriture dans `%WINDIR%\Fonts\` et modification de `HKLM`. Cette option est **strictement exclue du projet**.
  - *Installation au niveau du profil utilisateur Windows (Per-user) :* Écriture dans `%LOCALAPPDATA%\Microsoft\Windows\Fonts\` et inscription dans `HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Fonts`. Persistante d'une session à l'autre pour cet utilisateur sans privilèges administrateur. Soumise à l'arbitrage du propriétaire sous la Question O-01.


## 11. Compatibilité et environnement cible
### 11.1 Matrice des quatre environnements cibles
L'architecture de GraphiScope est analysée face à quatre environnements d'exécution :
1. *Poste de travail local Windows 11 (Cible nominale de référence) :* Matériel standard, SSD NVMe, système d'exploitation à jour.
2. *Poste de travail local Windows 10 22H2 (Objectif de compatibilité descendante) :* Matériel hétérogène, SSD SATA / HDD, composant WebView2 Runtime préinstallé ou distribué.
3. *Volumes amovibles (Clés USB, disques externes USB 3.0 / USB-C) :* Systèmes de fichiers NTFS ou exFAT, risques de déconnexion inopinée en cours de scan.
4. *Partages réseau distants d'entreprise (SMB 3.0, NAS Synology/QNAP, DFS) :* Latences réseau variables, verrous de fichiers distants, gestion hétérogène de la Corbeille.

### 11.2 Analyse des cibles Windows 10 et Windows 11 et cycle de support éditeur (COR-11)
Pour garantir la rigueur opérationnelle du projet, l'analyse dissocie rigoureusement trois notions :
1. **La compatibilité technique potentielle :** Le framework .NET 8, le contrôle Microsoft Edge WebView2 Runtime et l'API WinForms s'exécutent techniquement sur Windows 10 (version 1607+) et Windows 11.
2. **Le cycle de vie et le support officiel Microsoft :**
   - *Sources officielles Microsoft Lifecycle (consultées en novembre 2026) :*
     - **Windows 11 (versions 23H2 et 24H2) :** En phase de support actif avec réception continue des correctifs fonctionnels et de sécurité (`SOURCE_OFFICIELLE`).
     - **Windows 10 version 22H2 (éditions Home, Pro, Pro Education, Pro for Workstations) :** Fin de servicing standard intervenue le **14 octobre 2025** (`SOURCE_OFFICIELLE`). Au-delà de cette échéance, Microsoft ne fournit plus de mises à jour de sécurité gratuites pour ces éditions standard (hors contrats spécifiques payants Extended Security Updates - ESU).
   - *Mise en garde essentielle (COR-11) :* La disponibilité technique d'une API sous Windows 10 n'équivaut en aucun cas à la garantie que le système d'exploitation hôte bénéficie encore de correctifs de sécurité actifs face à des vulnérabilités de bas niveau du noyau ou des pilotes.
3. **Les versions effectivement testées par GraphiScope :**
   - À ce stade préparatoire, aucun banc d'essais logiciel n'a été exécuté par GraphiScope.
   - *Statut d'ingénierie :* Le support de Windows 10 22H2 est formellement qualifié d'**objectif de conception et d'hypothèse technique à vérifier expérimentalement sur machines cibles réelles en Phase B (`HYPOTHESE`)**.

### 11.3 Opérations sur les fichiers physiques, Corbeille et suppressions sur volumes réseau (COR-18)
Les opérations administratives de suppression, renommage et déplacement physiques de fichiers s'appuient sur l'interface COM native du Shell Windows `IFileOperation` (`SOURCE_OFFICIELLE`) :
- **Intégration conditionnelle de la Corbeille Windows via `FOF_ALLOWUNDO` :**  
  Le drapeau `FOF_ALLOWUNDO` demande au Shell Windows de déplacer l'élément vers la Corbeille plutôt que de le détruire immédiatement.
- **Réfutation des garanties automatiques et conditionnalité réelle (COR-18) :**
  - *Volumes locaux fixes (NTFS, ReFS) :*
    - Même sur un disque local fixe, le passage en Corbeille n'est **jamais garanti de façon absolue** :
      1. La Corbeille peut être globalement désactivée par une stratégie de groupe (GPO) ou manuellement par l'utilisateur ;
      2. Si la taille du fichier excède l'espace maximal alloué à la Corbeille sur ce lecteur, le Shell bascule silencieusement ou demande confirmation pour une suppression définitive immédiate sans possibilité de récupération ;
      3. Certains montages locaux particuliers (volumes montés sous forme de points de jonction vers des partitions spécifiques) ne supportent pas la Corbeille.
  - *Partages réseau (SMB, DFS) et supports amovibles (exFAT, FAT32) :*
    - **Comportement standard de l'Explorateur Windows :** Sur un partage réseau distant SMB ou une clé USB, l'Explorateur Windows ne gère pas de corbeille locale ; toute suppression demandée par le client est exécutée comme une destruction définitive immédiate sur le serveur.
    - **Environnements réseau spécifiques :** Certains serveurs NAS (ex. serveurs Samba avec module `vfs_recycle` actif, serveurs avec clichés instantanés VSS / Shadow Copies) peuvent conserver une copie côté serveur. Toutefois, cette rétention dépend entièrement de la configuration du serveur distant et n'est pas intégrée à la Corbeille du poste de travail client Windows.
    - **Limites de `GetDriveTypeW` :** Cette API identifie uniquement le type de connexion logique (`DRIVE_REMOTE`), mais ne permet pas de savoir si le serveur distant gère une corbeille ou des snapshots.
- **Règle de prudence absolue GraphiScope :**
  - GraphiScope **ne garantit en aucun cas la réversibilité ni le passage en Corbeille lors de suppressions physiques sur volumes réseau ou amovibles**.
  - Aucune promesse universelle d'annulation de type « Ctrl+Z » ne peut être contractée.
  - *Mesure d'ingénierie obligatoire :* Avant toute suppression physique, l'application doit avertir explicitement l'utilisateur et exiger une **confirmation renforcée** dès que la mise en Corbeille ne peut être formellement démontrée (`HYPOTHESE`).


## 12. Dépendances et conformité juridique
### 12.1 Inventaire détaillé des bibliothèques candidates, dépendances transitives et binaires natifs (COR-12)
La sélection des dépendances candidates pour la Phase B impose une distinction rigoureuse entre la licence déclarée d'un paquet NuGet racine et la **conformité de distribution démontrée de l'ensemble de son arbre de dépendances transitives et binaires natifs** (`HYPOTHESE`) :

| Composant | Rôle technique envisagé | Licence principale | Dépendances transitives et binaires natifs | Analyse de conformité et contraintes d'audit (COR-12) | Statut de validation |
| :--- | :--- | :--- | :--- | :--- | :---: |
| **Microsoft.Data.Sqlite** | Accès à SQLite local | MIT | Dépendance transitive vers `SQLitePCLRaw.bundle_e_sqlite3` qui embarque le binaire natif `e_sqlite3.dll`. | `e_sqlite3.dll` est compilé depuis le code source SQLite (Domaine Public / MIT). Aucune restriction de redistribution commerciale. Liaison dynamique conforme. | `HYPOTHESE` (Faisable) |
| **DocumentFormat.OpenXml** | Parsing des documents Office OpenXML | MIT | Dépendances managées .NET standard (`System.IO.Packaging`). | 100 % C# managé. Conforme pour la redistribution sans contamination copyleft. | `HYPOTHESE` (Faisable) |
| **System.IO.Compression** | Décompression ZIP et streaming d'archives | MIT | Brique officielle intégrée au runtime .NET 8. | Composant officiel Microsoft supporté et maintenu. | `SOURCE_OFFICIELLE` |
| **SharpCompress** | Extraction de formats d'archives multiples (7z, Tar, Rar) | MIT | Dépendances managées C#. | Parseur managé. Attention : le décodage des archives RAR v5 s'appuie sur les spécifications UnRar dont certaines clauses peuvent être restrictives pour la réimplémentation commerciale. Cadrer l'usage sur les formats ouverts (ZIP, 7z). | `HYPOTHESE` (À auditer) |
| **SixLabors.ImageSharp** | Décodage matriciel et rendu de vignettes WebP | Apache 2.0 modifiée (Split License) | 100 % C# managé, aucun binaire natif. | **Contrainte de licence majeure :** SixLabors applique un modèle de licence scindé (*Split License*). L'utilisation sous Apache 2.0 est conditionnée au seuil de chiffre d'affaires annuel de l'utilisateur (inférieur à 1 million USD) ou nécessite l'acquisition d'une licence commerciale payante. À arbitrer sous la Question O-03. | `HYPOTHESE` (Sous condition O-03) |
| **SkiaSharp** | Moteur graphique vectoriel et matriciel alternatif | MIT | **Binaire natif C++ compilé :** Embarque `libSkiaSharp.dll` (moteur Google Skia C++ natif). | Licence MIT permissive. Risque de sécurité accru lié à l'exécution de code binaire C++ natif non managé, imposant un confinement strict au sein du Worker Process. | `HYPOTHESE` (Sous condition O-03) |
| **TagLib-Sharp** | Extraction des métadonnées audio et vidéo | LGPL v2.1 | Dépendances managées C#. | **Obligation LGPL v2.1 stricte :** La distribution de GraphiScope avec TagLib-Sharp impose une **liaison dynamique stricte** (assemblage DLL séparé non fusionné par ILRepack/Costura) afin de permettre à l'utilisateur final de remplacer la bibliothèque par une version modifiée de son choix. | `HYPOTHESE` (Faisable sous DLL séparée) |
| **FFmpeg (Binaire externe)** | Extraction de métadonnées et vignettes vidéo | LGPL v2.1+ / GPL v2+ | Binaire exécutable natif indépendant (`ffmpeg.exe`). | **Décision propriétaire Q-005 bloquante :** L'embarquement est strictement suspendu à la validation préalable de son dossier de conformité juridique (interdiction absolue de composants GPL ou non-free, respect des brevets de codecs). Wrapper externe optionnel dissocié. | `DECISION_PROPRIETAIRE` (Suspendu Q-005, O-02) |
| **PdfiumViewer / PDFium** | Rendu et extraction de pages PDF | Apache 2.0 / BSD | Embarque le binaire natif Google PDFium (`pdfium.dll`). | Dépendance native C++. Nécessite un audit complet de la provenance du binaire compilé et de ses dépendances natives de bas niveau (FreeType, libpng). | `HYPOTHESE` (À qualifier) |

### 12.2 Principes de conformité juridique de la chaîne de distribution
- **Interdiction de toute contamination Copyleft fort (GPL v3) :** Aucun composant imposant la divulgation du code source de l'application hôte n'est admis.
- **Règle de séparation des bibliothèques LGPL (TagLib-Sharp) :** Utilisation exclusive sous forme de bibliothèques liées dynamiquement (DLL séparées).
- **Interdiction des binaires non-free :** Aucun binaire compilé intégrant des composants sous licences restrictives ou commerciales non autorisées (ex. builds FFmpeg incluant `libx264` GPL ou bibliothèques propriétaires brevetées) ne peut être distribué sans accord express (`DECISION_PROPRIETAIRE`).


## 13. Risques techniques et mesures de réduction
Les risques d'ingénierie identifiés pour la Phase B sont récapitulés dans la matrice ci-dessous avec leurs mesures préventives :

| Réf. | Intitulé du risque | Gravité | Probabilité | Mesures d'atténuation et contre-mesures d'ingénierie | Référence transversale |
| :---: | :--- | :---: | :---: | :--- | :---: |
| **RSK-01** | Traversée de répertoires (*Zip Slip*) et redirections par jonctions | Critique | Moyenne | Validation lexicale préalable, sécurisation du parcours par handles ouverts relatifs (anti-TOCTOU), contrôle d'identité `FileIdInfo` et écriture exclusive via `SafeFileHandle` ouvert. | COR-01, COR-R01 |
| **RSK-02** | Épuisement des ressources par archives piégées (*Zip Bombs*) | Élevée | Moyenne | Décompression en streaming, plafonds d'expansion configurables (100:1, 2 Go), timeout CPU 30s et surveillance de l'espace disque résiduel (`GetDiskFreeSpaceEx`). | COR-07, COR-R02 |
| **RSK-03** | Plantage du processus d'analyse sur fichier malformé | Élevée | Élevée | Découpage et isolation du Worker Process sous Job Object Win32 ; redémarrage automatique propre du Worker sans impact sur l'Hôte. | COR-04, COR-R04 |
| **RSK-04** | Instabilité des hachages lors de modifications concurrentes | Moyenne | Élevée | Relevé des métadonnées avant/après lecture, rejet des discordances, passage en statut `UNSTABLE`, temporisation exponentielle bornée (3 réessais). | COR-03, COR-05 |
| **RSK-05** | Famine (*Starvation*) des fichiers lors du scrubbing | Moyenne | Élevée | Séparation des files d'ingestion et de réconciliation, ordonnancement `ORDER BY LastHashVerifiedAt ASC`, curseur persistant et score dynamique. | COR-06, COR-R03 |
| **RSK-06** | Conflits de concurrence et verrous SQLite en mode WAL | Moyenne | Moyenne | Sérialisation de l'unique écrivain dans l'Hôte, `busy_timeout = 5000`, checkpoints passifs programmés et contrôle `PRAGMA integrity_check`. | COR-09, COR-R05 |
| **RSK-07** | Attaques réseau locales sur Kestrel (CSRF, DNS Rebinding) | Élevée | Faible | Écoute exclusive sur `127.0.0.1`, validation stricte des en-têtes `Host`/`Origin`, jetons de session cryptographiques anti-CSRF sur toute route mutative. | COR-10, COR-R04 |
| **RSK-08** | Perte irrémédiable de fichiers lors de suppressions sur partages réseau | Critique | Moyenne | Rejet de toute garantie de Corbeille sur réseau, analyse préalable du type de support (`GetDriveTypeW`), confirmation utilisateur renforcée obligatoire. | COR-18, COR-R07 |
| **RSK-09** | Perte d'événements de modifications sous forte charge E/S | Faible | Élevée | Statut opportuniste de `FileSystemWatcher`, déclenchement automatique d'un scan incrémental de réconciliation dès signalement d'un débordement de tampon. | § 6.3 |
| **RSK-10** | Contamination juridique par dépendances ou binaires natifs | Élevée | Faible | Audit strict des licences transitives, liaison dynamique des DLL LGPL, interdiction des builds FFmpeg non-free et suspension de Q-005. | COR-12, Q-005 |
| **RSK-11** | Dégradation de la réactivité UI lors de l'indexation massive | Moyenne | Moyenne | Pagination virtuelle dans le frontend WebView2, découplage asynchrone des notifications Kestrel via WebSockets/SignalR. | § 16.1 |
| **RSK-12** | Fin de support de Windows 10 22H2 (14 octobre 2025) | Moyenne | Élevée | Qualification en objectif de conception à vérifier expérimentalement sur machines réelles sans API exclusives à Windows 11. | COR-11, COR-R06 |


## 14. Décisions encore nécessaires au propriétaire (Questions O-01 à O-04)
Les quatre questions stratégiques ci-dessous demeurent formellement ouvertes et subordonnées à l'arbitrage souverain du propriétaire du projet :

### Question O-01 — Modalités de prévisualisation et d'installation des polices de caractères
- **Option A (Recommandée pour sécurité maximale) :** Prévisualisation confinée exclusivement dans l'interface Web via `@font-face` et CSS Canvas, sans aucune installation sur le système d'exploitation hôte.
- **Option B :** Ajout d'une option manuelle volontaire d'installation au niveau du profil utilisateur Windows (`%LOCALAPPDATA%\Microsoft\Windows\Fonts\`, `HKCU`), persistante sans privilèges administrateur. L'installation globale système (`%WINDIR%\Fonts\`) restant formellement exclue.

### Question O-02 — Arbitrage sur l'adoption du binaire FFmpeg pour les vignettes vidéo
- **Option A :** Autoriser l'embarquement du binaire officiel `ffmpeg.exe` en sous-processus isolé sous réserve de la validation préalable de son dossier complet de conformité juridique (LGPL v2.1+, interdiction de composants GPL/non-free, respect des brevets de codecs).
- **Option B :** Exclure tout binaire FFmpeg et restreindre GraphiScope à l'extraction des métadonnées passives (via TagLib-Sharp) sans génération de vignettes vidéo.

### Question O-03 — Sélection du moteur de traitement d'images matricielles
- **Option A :** `SixLabors.ImageSharp` (100 % C# managé, surface d'attaque réduite face aux corruptions mémoire, mais soumis à la licence modifiée avec Split License et seuil de CA).
- **Option B :** `SkiaSharp` (moteur Google Skia très performant, licence MIT permissive, mais dépendant du binaire natif C++ `libSkiaSharp.dll` nécessitant un confinement strict du Worker).

### Question O-04 — Traitement des anciens formats binaires Microsoft Office (DOC, XLS, PPT) : métadonnées seules par défaut ou extraction textuelle systématique avec statut dégradé en cas d'erreur
- **Option A :** Extraction des seules métadonnées de résumé OLE (`SummaryInformation`, `DocumentSummaryInformation`) par défaut, sans tentative d'extraction du texte brut des flux applicatifs complexes, afin de réduire la surface d'attaque et prévenir les défaillances de parseurs non sandboxés.
- **Option B :** Tentative systématique d'extraction textuelle et tabulaire brute des flux accessibles, avec bascule automatique vers un statut d'analyse dégradé et signalement explicite à l'utilisateur des éléments non interprétés en cas d'erreur (conformément à Q-002 Option 2).

*Rappel impératif de gouvernance : Les quatre questions O-01 à O-04 demeurent formellement ouvertes et subordonnées à la décision souveraine du propriétaire du projet. L'agent ne s'auto-attribue aucun arbitrage sur ces points.*


## 15. Séquencement recommandé de la future implémentation (Jalons B1 à B5) (COR-16)
La mise en œuvre de la future Phase B est découpée en cinq jalons séquentiels stricts.

**Avertissement formel de gouvernance (COR-16) :** Ce séquencement constitue exclusivement une proposition d'ordonnancement d'ingénierie. Il ne constitue en aucun cas une autorisation de démarrer la Phase B. Chaque jalon ne peut être engagé que sur mandat exprès du propriétaire :

```
+-----------------------------------------------------------------------------------+
| Jalon B1 : Socle architectural, Hôte, Base SQLite locale & Interface WebView2     |
|   - Initialisation solution .NET 8 / WinForms / WebView2                          |
|   - Implémentation du schéma relationnel SQLite (PhysicalFile, ResourceIdentity...)  |
|   - Kestrel confiné à 127.0.0.1 avec jetons anti-CSRF et filtrage Host            |
|   --> Critère de sortie : DDL validé, tests d'intégrité B-Tree, handshake UI étanche |
+-----------------------------------------|-----------------------------------------+
                                          | Dépendance de sécurité obligatoire :
                                          | Socle hôte validé et étanche
                                          v
+-----------------------------------------------------------------------------------+
| Jalon B2 : Sous-processus Worker, Confinement Job Object & Sécurisation Chemins    |
|   - Création du sous-processus Worker (GraphiScope.Worker.exe) sans réseau        |
|   - Régulation par Job Object Win32 (limites mémoire/CPU, fermeture automatique)  |
|   - Algorithme de résistance TOCTOU par opérations relatives aux handles ouverts  |
|   --> Critère de sortie : Crashs Worker isolés sans chute Hôte, banc TOCTOU réussi|
+-----------------------------------------|-----------------------------------------+
                                          | DÉPENDANCE DE SÉCURITÉ ABSOLUE :
                                          | Interdiction formelle d'extraire des archives
                                          | avant validation complète de B2
                                          v
+-----------------------------------------------------------------------------------+
| Jalon B3 : Moteur d'inventaire, Réconciliation, Scrubbing & Gestion des Archives  |
|   - Scan récursif Win32 et calcul SHA-256 avec détection du statut UNSTABLE       |
|   - Politique d'équité du scrubbing avec curseur persistant et double métrique    |
|   - Exploration des conteneurs ZIP/7z en streaming sous plafonds configurables    |
|   --> Critère de sortie : Inventaire 100k fichiers stable, zéro boucle d'archives |
+-----------------------------------------|-----------------------------------------+
                                          | Dépendance technique :
                                          | Moteur de données et fichiers stables
                                          v
+-----------------------------------------------------------------------------------+
| Jalon B4 : Chaîne de parsing multi-formats, Classification L0-L4 & Prévisualisations|
|   - Parseurs OpenXML (résolution sharedStrings), OLE2 partiel, SVG sécurisé       |
|   - Moteur de classification multi-niveaux et verrouillage des surcharges L4      |
|   - Indexation plein texte FTS5 et cache de vignettes WebP                        |
|   --> Critère de sortie : Formats bureautiques et créatifs qualifiés, FTS5 réactif|
+-----------------------------------------|-----------------------------------------+
                                          | Dépendance fonctionnelle :
                                          | Ensemble des composants actifs
                                          v
+-----------------------------------------------------------------------------------+
| Jalon B5 : Intégration Shell conditionnelle, Recette globale & Durcissement final |
|   - Opérations de fichiers via IFileOperation avec confirmation renforcée         |
|   - Campagne de tests d'endurance, de charge et de conformité des dépendances     |
|   - Validation du dossier de recette et livraison du livrable final de Phase B    |
|   --> Critère de sortie : Recette validée par le propriétaire sans réserve bloquante|
+-----------------------------------------------------------------------------------+
```


## 16. Critères d'acceptation et critères de performance (COR-13)
### 16.1 Qualification formelle des objectifs préliminaires et critères d'acceptation
Le dossier technique dissocie formellement les **objectifs préliminaires** (cibles théoriques de débit, de latence et d'empreinte mémoire à mesurer lors des essais) des **critères d'acceptation opérationnels** (conditions formelles de validation requises pour statuer sur la conformité d'un jalon).  
Les critères quantitatifs de performance sont formellement qualifiés d'**objectifs préliminaires à mesurer (`OBJECTIF_PRELIMINAIRE`)**, subordonnés aux caractéristiques matérielles :
- **Débit d'ingestion indicatif :** Objectif de 100 fichiers/seconde en scan initial sur disque SSD NVMe local pour des fichiers de taille moyenne (1 à 5 Mo).
- **Temps de réponse de l'interface utilisateur :** Latence d'affichage inférieure à 200 ms lors de la navigation dans une liste paginée de 10 000 éléments.
- **Empreinte mémoire de repos :** Consommation de l'hôte Kestrel inférieure à 150 Mo en veille.
- **Plafond mémoire du Worker :** Régulation stricte par Job Object à 1 024 Mo de mémoire virtuelle max.

### 16.2 Protocoles d'essais proposés pour la validation de Phase B (NON EXÉCUTÉS)
Pour garantir la reproductibilité des évaluations lors de la Phase B, le tableau ci-dessous formalise les protocoles d'essais proposés pour les 11 domaines critiques du projet.  
**Avertissement formel (COR-13) :** Aucun de ces bancs d'essais n'a été exécuté à ce stade préparatoire. Ils constituent des **propositions de bancs d'essais à exécuter lors de la future Phase B (`HYPOTHESE` / `PROPOSITION_DE_BANC_D_ESSAIS`)** :

| Domaine de test | Corpus de test proposé | Configuration matérielle de référence | Conditions de cache & Répétitions | Métriques & Percentiles relevés | Critères de succès et de rejet (Tolérances) | Statut d'exécution |
| :--- | :--- | :--- | :--- | :--- | :--- | :---: |
| **1. Hachage concurrent et détection d'instabilité (COR-03)** | Corpus de 1 000 fichiers (1 Mo à 100 Mo) avec injection d'écritures concurrentes aléatoires pendant la lecture. | PC de référence 8 cœurs, SSD NVMe PCIe 4.0, 16 Go RAM. | Cold cache (purge cache OS entre passes) ; 5 répétitions complètes. | Taux de détection des discordances, temps de passage en statut `UNSTABLE`. | **Succès :** 100 % des altérations détectées ; aucun hachage corrompu marqué `STABLE` ; abandon propre après 3 réessais sans blocage du fil d'inventaire. | **NON EXÉCUTÉ** |
| **2. Scrubbing et résistance à la famine (COR-06)** | Catalogue de 50 000 fichiers initialisés + flux continu d'ingestion de 10 nouveaux fichiers/sec. | PC 8 cœurs, SSD SATA local, profil d'activité utilisateur simulé. | Exécution continue sur 24 heures simulées. | Évolution de `RatioFichiers` et `RatioVolume`, délai maximal de revisite P95 et P99. | **Succès :** Convergence du ratio de couverture ; progression démontrée du curseur persistant sans stagnation d'aucun fichier ancien au-delà du délai cible. | **NON EXÉCUTÉ** |
| **3. Résilience SQLite et sauvegarde (COR-09)** | Catalogue SQLite WAL de 100 000 enregistrements sous écriture intensive continue. | PC de référence, simulation d'arrêt brutal du processus (`TerminateProcess`). | 20 répétitions de coupure inopinée en pleine écriture de checkpoint. | Résultat de `PRAGMA integrity_check`, temps de restauration après redémarrage. | **Succès :** Zéro corruption de structure B-Tree ; base redémarrée intègre ; sauvegardes créées par `VACUUM INTO` validées à 100 % par `integrity_check`. | **NON EXÉCUTÉ** |
| **4. Débordement FileSystemWatcher (§ 6.3)** | Création et renommage en masse de 20 000 fichiers en moins de 2 secondes. | PC de référence, partition NTFS locale. | 10 répétitions consécutives. | Nombre de débordements signalés (`InternalBufferOverflowException`), délai de réconciliation. | **Succès :** Débordement intercepté proprement ; réconciliation incrémentale déclenchée automatiquement ; 100 % des fichiers réconciliés à terme. | **NON EXÉCUTÉ** |
| **5. Corpus bureautique Office (COR-08)** | 500 fichiers réels mixtes (formats modernes et binaires, avec macros, styles complexes, `sharedStrings`). | Environnement Windows 11 standard. | 3 passes avec analyse de conformité des chaînes extraites. | Exactitude de résolution des index `sharedStrings.xml`, détection des composants VBA. | **Succès :** 100 % des index de chaînes partagées résolus sans décalage ; formules textuelles brutes fidèles sans recalcul ; détection des macros conforme. | **NON EXÉCUTÉ** |
| **6. Compatibilité d'exécution Windows (COR-11)** | Suite de tests d'intégration complète GraphiScope. | 1 VM Windows 11 23H2 + 1 machine physique Windows 10 22H2 réelle. | 3 passes d'installation et d'exécution complète sur chaque environnement. | Taux de réussite des tests unitaires, absence d'appels API absents de Win10. | **Succès :** Démarrage de l'hôte, affichage WebView2 et analyse de fichiers 100 % opérationnels sur les deux systèmes cibles. | **NON EXÉCUTÉ** |
| **7. Sécurité HTTP et Kestrel local (COR-10)** | Fuzzing HTTP automatisé : requêtes cross-origin forgées, absence de jeton, en-têtes Host forgés, DNS rebinding. | Hôte Kestrel actif écoutant sur `127.0.0.1:<port>`. | 10 000 requêtes malveillantes injectées via script de pénétration dédié. | Taux de rejet des requêtes non autorisées (codes 400 et 403). | **Succès :** 100 % des requêtes dépourvues de jeton ou avec `Host` non conforme rejetées ; zéro exécution de commande mutative non autorisée. | **NON EXÉCUTÉ** |
| **8. Suppression physique et Corbeille SMB (COR-18)** | 200 suppressions de fichiers de test sur disque local NTFS, clé USB exFAT et partage distant SMB (Samba). | PC Windows connecté à un NAS Synology SMB 3.0. | 5 répétitions par typologie de support. | Comportement de `IFileOperation`, affichage des alertes utilisateur. | **Succès :** Avertissement explicite et confirmation renforcée systématiquement déclenchés sur partage réseau et support amovible ; zéro suppression silencieuse. | **NON EXÉCUTÉ** |
| **9. Indexation plein texte et recherche FTS5 (§ 9.3)** | Base de données de 50 000 ressources avec textes extraits (environ 500 Mo de texte cumulé). | PC de référence avec moteur SQLite FTS5 actif. | 100 requêtes de recherche textuelle complexes (termes multiples, préfixes, jokers). | Temps de réponse P50, P95 et P99 de la requête SQL FTS5. | **Succès :** Temps de réponse moyen < 50 ms ; P99 < 150 ms ; pertinence des résultats de recherche conforme aux attentes. | **NON EXÉCUTÉ** |
| **10. Analyse des polices de caractères (COR-R08)** | Échantillon de 200 polices de caractères réelles (formats TTF, OTF, WOFF, WOFF2). | Frontend WebView2 sous Windows 11. | 3 passes d'affichage et de prévisualisation via CSS `@font-face`. | Rendu visuel dans le canvas HTML5, temps d'extraction des tables de métadonnées. | **Succès :** Prévisualisation Web confinée sans installation hôte 100 % fonctionnelle ; métadonnées d'en-tête fidèlement extraites. | **NON EXÉCUTÉ** |
| **11. Audit de conformité des dépendances (COR-12)** | Arbre complet des paquets NuGet du projet et binaires natifs associés. | Analyseur automatisé de licences de dépendances transitives. | Audit systématique à chaque génération de dépendances. | Matrice des licences déclarées, détection de composants GPL ou non-free. | **Succès :** Zéro composant copyleft fort non autorisé ; bibliothèques LGPL isolées en DLL dynamiques ; conformité juridique complète validée. | **NON EXÉCUTÉ** |

### 16.3 Proposition d'orientation d'ingénierie pour les tests de sécurité (Non arbitrée)
En complément des protocoles d'essais proposés ci-dessus, une réflexion d'ingénierie porte sur le niveau d'automatisation des bancs de sécurité et d'intrusion :
- **Option d'ingénierie 1 :** Développement d'un harnais de tests automatisés dès le Jalon B2 (injection d'archives piégées, simulation de courses TOCTOU, fuzzing d'en-têtes).
- **Option d'ingénierie 2 :** Validation progressive par bancs d'essais manuels ciblés et tests d'intégration au fil des jalons.  
*Précision de gouvernance :* Cette proposition technique d'ingénierie constitue une simple réflexion méthodologique interne et ne constitue en aucun cas une question ouverte de gouvernance souveraine (ne se substituant nullement à la Question O-04 relative aux formats Office anciens).


## 17. Sources et références normatives officielles (Section 6.6 du mandat)
Les orientations du présent dossier s'appuient rigoureusement sur les spécifications normatives et documentations officielles suivantes :
- **Microsoft Win32 & Sécurité Windows :**
  - Spécification Win32 `CreateFileW`, drapeaux `FILE_FLAG_OPEN_REPARSE_POINT` et `CREATE_NEW` (Microsoft Learn, consulté en novembre 2026).
  - API Win32 `GetFileInformationByHandleEx` et classe d'information `FileIdInfo` (Microsoft Learn).
  - Documentation sur les Job Objects et la structure `JOBOBJECT_EXTENDED_LIMIT_INFORMATION` (Microsoft Learn).
  - Interface COM native `IFileOperation` et drapeau `FOF_ALLOWUNDO` (Microsoft Learn).
  - Documentation du cycle de vie du support éditeur : *Windows 10 Home and Pro Lifecycle*, fin de servicing standard au 14 octobre 2025 (Microsoft Lifecycle Policy, consultée en novembre 2026).
- **SQLite Consortium :**
  - Documentation officielle SQLite WAL : *Write-Ahead Logging* (`sqlite.org/wal.html`).
  - Documentation officielle sur l'intégrité et la prévention des corruptions : *How To Corrupt An SQLite Database File* (`sqlite.org/howtocorrupt.html`).
  - Documentation officielle de l'API de sauvegarde en ligne : *SQLite Online Backup API* (`sqlite.org/backup.html`).
- **Formats bureautiques et industriels :**
  - Standard ECMA-376 5e édition : *Office Open XML File Formats*, notamment SpreadsheetML et la gestion de `sharedStrings.xml`.
  - Spécification Microsoft `[MS-XLSB]` : *Excel (.xlsb) Binary File Format*.
  - Spécification Microsoft `[MS-CFB]` : *Compound File Binary File Format* (OLE2).
  - Spécification normalisée ASTM D6673 / DXF-AAMA pour la CAO textile 2D.
- **Sécurité logicielle et Web :**
  - Mitre CWE-22 (*Improper Limitation of a Pathname to a Restricted Directory - Zip Slip*).
  - Mitre CWE-367 (*Time-of-check Time-of-use Race Condition - TOCTOU*).
  - Standard W3C SVG 1.1 / 2.0 et recommandations de sécurité pour le rendu de contenu XML non fiable.
  - RFC 6454 (*The Web Origin Concept*) et RFC 6265 (*HTTP State Management Mechanism*).


## 18. Synthèse des éléments vérifiés, hypothèses et blocages (COR-14, ANO-05)
Pour garantir une traçabilité sans équivoque, les assertions techniques du dossier sont réparties selon la taxonomie de référence officielle du projet comportant **7 catégories de preuve** :
1. `DECISION_PROPRIETAIRE` : Arbitrage souverain rendu par le propriétaire du projet (autorise, exclut, conditionne — intangible sans décision ultérieure).
2. `SOURCE_OFFICIELLE` : Spécification normative internationale publiée (ISO, ECMA, W3C, IETF RFC) ou documentation technique éditeur officielle versionnée (ex. Microsoft Open Specifications [MS-...]).  
   *(Rappel fondamental de gouvernance : La documentation officielle d'une API ou d'une norme ne démontre pas à elle seule le fonctionnement réel de GraphiScope tant que celui-ci n'est pas développé et testé).*
3. `TESTE_DEV` : Capacité ou composant validé empiriquement sur l'environnement outillé de développement (ne prouve pas le fonctionnement sur machine cible).
4. `TESTE_CIBLE` : Capacité rigoureusement validée sur machine cible Windows 10/11 propre avec corpus représentatif (aucun élément ne porte ce statut en Phase A/préparatoire).
5. `HYPOTHESE` : Déduction technique d'ingénierie crédible ou proposition d'architecture à valider expérimentalement en Phase B.
6. `NON_VERIFIE` : Donnée technique manquante, incomplète ou issue de source secondaire incertaine interdisant toute affirmation péremptoire.
7. `BLOQUANT` : Indicateur transversal de risque technique, contrainte légale prohibitive ou risque de sécurité critique.

### 18.1 Statuts complémentaires de maturité et d'évaluation méthodologique
Les notions suivantes constituent des descripteurs méthodologiques d'avancement et de planification, distincts des catégories de preuve de faisabilité :
- `OBJECTIF_PRELIMINAIRE` : Cible quantitative de performance, de débit ou d'équité de conception à mesurer lors des bancs d'essais réels.
- `PROPOSITION_DE_BANC_D_ESSAIS` : Protocole de test reproductible proposé, formellement non exécuté à ce stade préparatoire.

### 18.2 Tableau de synthèse des volets techniques selon les sept catégories de référence

| Volet technique | Catégorie de preuve (Taxonomie de référence) | Statut réel du volet | Portée et conditions techniques associées |
| :--- | :---: | :---: | :--- |
| **Traitement des formules Excel** | `DECISION_PROPRIETAIRE` | Cadré (Q-001) | Formules textuelles brutes extraites sans recalcul ; valeur en cache (`<v>`) formellement non exploitable ni garantie. |
| **Formats binaires Office OLE2** | `DECISION_PROPRIETAIRE` | Cadré (Q-002) | Prise en charge partielle et qualifiée du texte et métadonnées sans promesse d'exhaustivité visuelle. |
| **Relations heuristiques dans le graphe (D2/D3/D4)** | `DECISION_PROPRIETAIRE` | Cadré (Q-003) | Relations candidates soumises à validation/rejet explicite utilisateur ; distinction stricte `MEME_CONTENU` vs `COPIE_DE`. |
| **Emplacement du catalogue de données** | `DECISION_PROPRIETAIRE` | Cadré (Q-004) | Profil utilisateur local (`%LOCALAPPDATA%`) ; SQLite orientation privilégiée non obligatoire ; proscription du réseau. |
| **Distribution binaire FFmpeg** | `DECISION_PROPRIETAIRE` | Suspendu (Q-005) | Principe approuvé, suspendu à la validation d'un dossier complet de conformité juridique (O-02). |
| **Protection des ressources et limites** | `DECISION_PROPRIETAIRE` | Cadré (Q-006) | Aucun quota volumétrique arbitraire ; régulation par surveillance dynamique continue des ressources réelles. |
| **Archives chiffrées opaques** | `DECISION_PROPRIETAIRE` | Cadré (Q-007) | Indexées comme conteneurs opaques sans invite interactive de mot de passe ni tentative de déchiffrement. |
| **TIFF classique et BigTIFF conditionnel** | `DECISION_PROPRIETAIRE` | Conditionnel (Q-008) | TIFF 32 bits prioritaire ; BigTIFF conditionnel aux capacités vérifiées sans blocage de l'inventaire. |
| **Défense en profondeur Kestrel 127.0.0.1** | `HYPOTHESE` | À tester | Écoute exclusive `127.0.0.1`, validation `Host`/`Origin` et jeton anti-CSRF sur routes mutatives (`SOURCE_OFFICIELLE`). |
| **Résistance TOCTOU sur handles ouverts** | `HYPOTHESE` | À tester | Parcours relatif aux handles de répertoires parents et validation d'identité `FileIdInfo` (`SOURCE_OFFICIELLE`). |
| **Worker Process et isolation de pannes** | `HYPOTHESE` | À qualifier | Séparation d'espaces d'adressage et Job Objects confinant les crashs sans prétendre à un sandboxing absolu. |
| **Scrubbing équitable et curseur persistant** | `HYPOTHESE` | À mesurer | Séparation ingestion/scrubbing, curseur persistant et double métrique de couverture (objectif préliminaire). |
| **Durabilité SQLite en mode WAL** | `HYPOTHESE` | Cadrée | Un seul écrivain simultané, intégrité B-Tree préservée, `PRAGMA integrity_check` avant sauvegarde (`SOURCE_OFFICIELLE`). |
| **Sécurité des chemins et parsing SVG** | `HYPOTHESE` | Spécifiée | Rejet absolu des scripts, DTD externes et styles distants lors du rendu vectoriel (`SOURCE_OFFICIELLE`). |
| **Cellules Excel et sharedStrings.xml** | `SOURCE_OFFICIELLE` | Spécifiée | Résolution obligatoire des index numériques de chaînes partagées sous ECMA-376 (SpreadsheetML). |
| **Cycle de support Windows 10/11** | `SOURCE_OFFICIELLE` | Documenté | Windows 11 support actif ; Windows 10 22H2 fin de servicing standard intervenue au 14/10/2025. |
| **Suppression conditionnelle et Corbeille SMB** | `HYPOTHESE` | Précisée | Absence de garantie sur réseau/amovible ; avertissement et confirmation renforcée imposés (`SOURCE_OFFICIELLE`). |
| **Protocoles de test des 11 domaines critiques** | `HYPOTHESE` | Non exécutés | Protocoles reproductibles formalisés à exécuter lors de la Phase B (propositions de bancs d'essais). |
| **Formats propriétaires fermés (Lectra .mdl/.pdr, Gerber PCB)** | `BLOQUANT` | Exclus | Formats fermés sans spécification publique ou hors périmètre métier (PCB) : strictement exclus. |

---

### CONCLUSION DE LA RÉVISION TECHNIQUE V1.6.0
Le présent dossier technique **GS-0003-PHASE-B-PREPARATION.md V1.6.0** intègre l'ensemble des corrections documentaires de conformité globale prescrites par le mandat `GS-OPS-GS0003-COR-005` consécutivement à l'audit contradictoire `GS-OPS-GS0003-AUDIT-005`.  
Il rétablit fidèlement les arbitrages souverains et questions ouvertes déformés ou omis, purifie les sections de dimensionnement de tout quota arbitraire rigide au profit d'une surveillance dynamique continue des ressources réelles, et restaure la taxonomie officielle à 7 catégories de preuve du projet.

**Statut du dossier révisé :** V1.6.0 — SOUMIS À L'EXAMEN ET À L'ARBITRAGE FORMEL DU PROPRIÉTAIRE DU PROJET GRAPHISCOPE (`REVIEW`).

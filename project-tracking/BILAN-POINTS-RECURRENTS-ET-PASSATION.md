# BILAN DES POINTS RÉCURRENTS ET DOCUMENTATION DE PASSATION — JALON B2 (GS-0006)

**Référence opérationnelle :** `GS-OPS-GS0006-PASSATION-001`  
**Date d'établissement :** 16 novembre 2026  
**Auteur :** Antigravity (Google DeepMind)  
**Destinataires :** Propriétaire du projet GraphiScope, agents repreneurs  
**Projet :** GraphiScope (`M:\dev\GraphiScope`)  
**Jalon :** GS-0006 — Phase B : Préparation documentaire du Jalon B2 (*Worker isolé sous Windows Job Objects et décompression d'archives*)  
**Statut actuel de GS-0006 :** **`REVIEW`** (aucun passage à `DONE` ou `IN_PROGRESS` sans décision explicite du propriétaire)  
**Autorisation d'implémentation B2 / B3 :** **NON** (strictement interdite)  

---

## 1. Contexte et objectif de la passation

Le présent document a pour objet d'établir une synthèse exhaustive, objective et pérenne des questions techniques et de gouvernance qui ont fait l'objet de multiples cycles de corrections successifs (mandats `GS-OPS-GS0006-B2-PREPARATION-CORRECTION-001` à `011`).

### Pourquoi ces points ont-ils récidivé au fil des mandats ?
L'analyse rétrospective de l'historique montre que les rechutes des agents successifs résultaient systématiquement de quatre dérives méthodologiques récurrentes :
1. **L'affirmation d'absolus non vérifiables :** Utilisation de termes hyperboliques (« 100 % étanche », « intégrité prouvée à 100 % », « le Worker ne peut jamais bloquer », « bac à sable hermétique ») masquant les frontières réelles des mécanismes de l'OS.
2. **La confusion entre architecture logicielle et barrière du système d'exploitation :** Confondre l'absence d'appel à une API dans le code managé (pas de Kestrel, pas de `System.Net.Http`, pas de SQLite dans le Worker) avec une impossibilité technique imposée par le noyau Windows NT (qui, en cas de compromission native post-RCE sous le même compte utilisateur, autoriserait l'accès à WinSock ou aux fichiers du profil).
3. **La confusion entre propositions d'ingénierie et décisions souveraines :** Présenter des choix techniques d'agent (comme l'introduction d'Entity Framework Core) comme s'il s'agissait de décisions déjà approuvées par le propriétaire, alors que la décision souveraine `DEC-0005` a acté l'utilisation exclusive de `Microsoft.Data.Sqlite` pur.
4. **L'inventaire déclaratif sans preuve de contenu :** Classer des composants comme `ADAPTE` sur la seule base d'une vague analogie fonctionnelle, voire inventer des chemins sources inexistants (cas de `src/GraphiScope.Core/Persistence/Exceptions.cs` qui n'a jamais figuré dans le manifeste officiel B1).

Le dossier consolidé terminal `project-tracking/GS-0006-B2-PREPARATION-PROPOSAL.md` en version **V2.0.0** assainit l'ensemble de ces points. Le présent document explicite l'état exact de chacun d'eux afin d'interdire toute réouverture infondée ou régression lors des prochaines interventions.

---

## 2. Inventaire détaillé des points récurrents et leur état actuel

### Point 1 : Statut du dossier GS-0002 et portée des décisions souveraines
- **Mandats concernés :** COR-07, COR-36, Mandat 010, Mandat 011 (§4.1).
- **Problème récurrent :** Tendance des agents à affirmer que le dossier documentaire GS-0002 aurait été globalement accepté ou clôturé, ou à confondre les décisions individuelles avec le livrable global.
- **État actuel dans V2.0.0 :**
  * Le dossier global GS-0002 demeure explicitement au statut **`REVIEW`** tant que l'acceptation formelle globale du propriétaire n'a pas été prononcée.
  * Les huit arbitrages souverains individuels **Q-001 à Q-008** rendus par le propriétaire sont pleinement confirmés, valides et intangibles (`DEC-0002`).
  * Les décisions de clôture de GS-0003 (`DEC-0003`) et de B1 (`DEC-0005`) ne constituent en aucun cas une autorisation d'implémenter B2.
  * Les décisions souveraines `B2-ARB-01` (Option A : stdin/stdout délimité en longueur) et `B2-ARB-02` (Option A : BCL .NET 10 exclusive) sont préservées sans réouverture.

### Point 2 : Correspondance et qualification des 37 chemins cibles proposés pour B2
- **Mandats concernés :** COR-22, COR-31, COR-36, COR-41, Mandat 011 (§4.2).
- **Problème récurrent :** Affirmations trompeuses de « 37 composants approuvés » ou de « 14 fichiers B1 réutilisés ». En particulier, la ligne n° 3 `CatalogException.cs` prétendait provenir d'un fichier source `src/GraphiScope.Core/Persistence/Exceptions.cs` qui n'a jamais existé dans le manifeste officiel B1 (37 fichiers). De plus, plusieurs lignes étaient classées `ADAPTE` sans aucune preuve de correspondance textuelle ou conceptuelle directe.
- **État actuel dans V2.0.0 :**
  * Les 37 chemins sont rigoureusement qualifiés comme **chemins cibles proposés pour B2** et non comme des composants approuvés.
  * La ligne n° 3 `CatalogException.cs` est formellement reclassée en **`NOUVEAU`**.
  * Seules 3 lignes présentant une correspondance démontrée avec réutilisation de code source B1 sont classées **`ADAPTE`** :
    1. `src/GraphiScope.Core/Services/PathConfinementService.cs` (issu de `src/GraphiScope.Core/Security/PathConfinementService.cs`) ;
    2. `src/GraphiScope.Host/Program.cs` (issu de `src/GraphiScope.Server/Program.cs`) ;
    3. `src/GraphiScope.Host/Services/HostScanningCoordinator.cs` (issu de `src/GraphiScope.Core/Services/InventoryCoordinator.cs`).
  * Bilan chiffré consolidé : **0 `REUTILISE_IDENTIQUE` (0 %)**, **3 `ADAPTE` (8,1 %)**, **34 `NOUVEAU` (91,9 %)**, **0 `NON_DEMONTRE`** (total : 37 chemins cibles proposés pour B2).

### Point 3 : Statut d'Entity Framework Core (EF Core)
- **Mandats concernés :** COR-36, COR-44, Mandat 011 (§4.1, §4.2, §4.10).
- **Problème récurrent :** Présentation d'EF Core et du projet `GraphiScope.Data` comme une décision arrêtée du propriétaire.
- **État actuel dans V2.0.0 :**
  * Le jalon B1 accepté sous `DEC-0005` utilise exclusivement `Microsoft.Data.Sqlite` direct en ADO.NET pur.
  * L'introduction d'EF Core (chemins cibles 23 à 29) constitue une **simple proposition d'ingénierie d'agent**, soumise à l'arbitrage futur du propriétaire.
  * L'alternative native en `Microsoft.Data.Sqlite` direct (sans EF Core, maintien de la table `ExtractionMetadata` par script DDL dans `SqliteSchemaManager.cs`) est explicitement documentée comme solution de référence en continuité avec B1.

### Point 4 : Intégrité historique des 37 fichiers B1
- **Mandats concernés :** COR-07, COR-37, COR-43, Mandat 011 (§4.3).
- **Problème récurrent :** Prétention d'une garantie cryptographique absolue d'intégrité rétroactive à la date de `DEC-0005` (13 novembre 2026), ou qualification de « B1 intact à 100 % ».
- **État actuel dans V2.0.0 :**
  * Aucune empreinte SHA-256 individuelle n'ayant été consignée lors de la décision souveraine `DEC-0005`, il est formellement admis qu'aucune preuve cryptographique rétroactive ne peut être établie à cette date.
  * Les empreintes calculées le 14 novembre 2026 constituent la première **photographie de référence** pour le suivi d'intégrité futur.
  * L'intégrité opérationnelle de B1 repose sur la présence physique des 37 fichiers, leur compilation Release sous .NET 10 sans erreur ni avertissement, et le succès des 30 tests xUnit automatisés.

### Point 5 : Découplage des grandeurs mémoire et fonctionnement IPC sous Q-006
- **Mandats concernés :** COR-13, COR-18, COR-23, COR-27, COR-32, COR-38, COR-39, COR-42, Mandat 011 (§4.4).
- **Problème récurrent :**
  * Confusion entre la limite de trame filaire (4 Mo) et la mémoire globale du processus ;
  * Présentation du Job Object comme une protection mémoire du Host ;
  * Affirmation non prouvée d'une taille fixe de 4 Ko garantie pour les buffers de pipes anonymes ;
  * Description erronée de `Utf8JsonReader` (présenté à tort comme construisant les objets managés du domaine) ;
  * Croyance qu'une troncature aval lors de l'émission IPC neutralise les allocations amont subies par le Worker.
- **État actuel dans V2.0.0 :**
  * Découplage strict de **9 composantes mémoire distinctes** (trame filaire, sérialisation, buffers applicatifs, buffers noyau `npfs.sys`, désérialisation, rétention `ArrayPool`, objets managés, commit privé, working set).
  * La limite filaire de 4 Mo ne borne pas la mémoire globale du processus.
  * Le Job Object encadre le Worker et protège le système d'exploitation, mais ne protège pas la mémoire du Host.
  * Les conséquences d'un dépassement de mémoire Job Object sont requalifiées : aucun code Win32 précis ni conversion systématique en `OutOfMemoryException` catchable ne sont garantis sous CoreCLR (risque de terminaison abrupte).
  * Les buffers de pipes anonymes sous `npfs.sys` ne possèdent aucune taille fixe universelle de 4 Ko.
  * `Utf8JsonReader` est qualifié comme lecteur bas niveau en flux continu ne construisant aucun objet managé.
  * La matérialisation mémoire amont par la BCL (`ZipArchive` matérialisant tous les `ZipArchiveEntry` dès le premier accès au Central Directory ; `TarReader` matérialisant l'intégralité du bloc PAX) survient dans le Worker avant toute intervention applicative et ne peut pas être éliminée par la troncature IPC aval.

### Point 6 : Contradiction mathématique sur les calculs d'offsets ZIP / SFX
- **Mandats concernés :** COR-16, COR-21, COR-30, COR-35, Mandat 011 (§4.5).
- **Problème récurrent :** La section 9.4 imposait l'égalité stricte $O_{cd} + S_{cd} = P_{eocd}$ pour valider un candidat EOCD, ce qui avait pour conséquence mathématique directe d'éliminer toutes les archives auto-extractibles (SFX) où un préfixe exécutable $\Delta > 0$ décale précisément l'EOCD.
- **État actuel dans V2.0.0 :**
  * Séquence cohérente modélisée : calcul du décalage candidat $\Delta = P_{eocd} - (O_{cd} + S_{cd})$.
  * Si $\Delta = 0$ : archive classique. Si $\Delta > 0$ : candidat d'archive avec préfixe SFX. Si $\Delta < 0$ : candidat rejeté (incohérence).
  * Pour $\Delta \ge 0$, vérification structurelle à l'offset ajusté $O_{cd\_phys} = O_{cd} + \Delta$ dans les limites physiques du fichier et contrôle de la signature `0x02014b50`.
  * Interdiction formelle d'exiger $O_{cd} + S_{cd} = P_{eocd}$ comme filtre préalable d'élimination.

### Point 7 : Structures ZIP64, positionnement et arithmétique `checked`
- **Mandats concernés :** COR-30, COR-35, Mandat 011 (§4.5).
- **Problème récurrent :** Assimilation erronée des positions de fin du Central Directory ZIP64 à celle de l'EOCD classique ; confusion entre arithmétique `checked` et validation sémantique.
- **État actuel dans V2.0.0 :**
  * Positionnement exact clarifié : l'EOCD Locator ZIP64 se situe à $P_{eocd} - 20$ et pointe vers l'EOCD64 Record. Le Central Directory 64 bits se termine au début de l'EOCD64 Record.
  * L'arithmétique `checked` empêche les débordements d'entiers mais ne dispense pas de vérifier la cohérence sémantique des bornes, des chevauchements et de la taille totale physique du fichier.

### Point 8 : Incompatibilité polynomiale SSE4.2 `CRC32` vs PKZIP IEEE 802.3
- **Mandats concernés :** COR-35, COR-40, Mandat 011 (§4.5).
- **Problème récurrent :** Prétention erronée que l'instruction CPU x86 SSE4.2 `CRC32` accélérerait le checksum ZIP, et dépendance supposée de `PCLMULQDQ` envers AVX/AVX-512.
- **État actuel dans V2.0.0 :**
  * Incompatibilité polynomiale formellement documentée : SSE4.2 calcule Castagnoli (`0x1EDC6F41`), alors que PKZIP exige IEEE 802.3 (`0xEDB88320`).
  * L'accélération matérielle du CRC-32 IEEE relève de l'instruction `PCLMULQDQ`, qui ne dépend pas d'AVX/AVX-512 (introduite sur Westmere en registres 128 bits XMM).
  * Contrôle actif en streaming jusqu'à EOF absolu avec vérification de la longueur réelle décompressée.

### Point 9 : Politique des entrées TAR et allocations amont PAX
- **Mandats concernés :** COR-04, COR-42, Mandat 011 (§4.6).
- **Problème récurrent :** Contradiction entre l'affirmation que « toutes les entrées non régulières sont rejetées » et l'acceptation normale des répertoires.
- **État actuel dans V2.0.0 :**
  * Énumération sans ambiguïté : entrées acceptées pour extraction = `RegularFile`, `V7RegularFile` (fichiers) et `Directory` (répertoires intermédiaires).
  * Entrées spéciales catégoriquement rejetées et ignorées = `SymbolicLink`, `HardLink`, `CharacterDevice`, `BlockDevice`, `Fifo` (`IGNORED_SPECIAL_ENTRY`).
  * Le risque de saturation mémoire par allocations amont de blocs PAX géants par `TarReader.GetNextEntry()` est formellement classé `NON_VERIFIE` / `RISQUE_BCL_PAX`.

### Point 10 : Confinement du système de fichiers et portée réelle de `FILE_SHARE_DELETE`
- **Mandats concernés :** COR-14, COR-19, COR-24, COR-28, COR-33, Mandat 011 (§4.8).
- **Problème récurrent :** Affirmation que l'omission de `FILE_SHARE_DELETE` sur un handle de répertoire empêcherait toute modification, création ou suppression dans son arborescence.
- **État actuel dans V2.0.0 :**
  * Portée exacte circonscrite : sous NTFS local, l'omission de `FILE_SHARE_DELETE` empêche un tiers d'ouvrir ce descripteur de répertoire avec le droit `DELETE` ou sans partager la suppression (bloquant `RemoveDirectoryW` ou `MoveFileExW` sur le répertoire lui-même).
  * Limites explicites documentées : elle n'annule pas les handles préexistants, n'empêche pas l'ajout de nouveaux fichiers dans le dossier par un tiers autorisé, et ne neutralise pas les privilèges administrateur ou les filtres noyau.

### Point 11 : Portée réelle des DACL du répertoire scratch
- **Mandats concernés :** COR-10, COR-33, Mandat 011 (§4.8).
- **Problème récurrent :** Présentation de la DACL restreinte (`SE_DACL_PROTECTED`) comme un cloisonnement absolu.
- **État actuel dans V2.0.0 :**
  * La DACL protège le dossier scratch contre les accès d'autres comptes utilisateurs sur une machine partagée.
  * **Elle n'isole pas le Worker des autres processus exécutés sous la même session utilisateur (`TokenUser`)**, qui possèdent les mêmes droits NTFS.

### Point 12 : Isolation Worker, frontières réseau et persistance SQLite
- **Mandats concernés :** COR-02, COR-17, COR-34, COR-44, Mandat 011 (§4.8).
- **Problème récurrent :** Présentation du Worker comme « hermétiquement isolé du réseau et de la base de données ».
- **État actuel dans V2.0.0 :**
  * L'absence d'accès SQLite et d'accès réseau relève de l'architecture logicielle (aucun code, aucun composant de données ni bibliothèque réseau dans le Worker).
  * **Au niveau du système d'exploitation, l'isolation n'est pas absolue :** En cas de compromission native sévère (post-RCE), le Worker tournant sous la session de l'utilisateur n'est bloqué ni pour ouvrir des sockets TCP WinSock (absence d'AppContainer ou de pare-feu WFP), ni pour accéder aux fichiers du profil (y compris le catalogue SQLite). Ce risque résiduel est formellement classé `NON_VERIFIE`.
  * Le modèle de menace est borné, réaliste et testable.

### Point 13 : Chaîne de lancement du Worker, Job Object et drainage de stderr
- **Mandats concernés :** COR-15, COR-20, COR-25, COR-29, COR-34, Mandat 011 (§4.7).
- **Problème récurrent :** Chemins d'échec Win32 non décrits, portée d'`ActiveProcessLimit=1` surévaluée, affirmation inconditionnelle que le Worker ne peut « jamais bloquer sur stderr ».
- **État actuel dans V2.0.0 :**
  * Séquence ordonnée avec traitement systématique des échecs : `CreateProcessW` suspendu -> `AssignProcessToJobObject` -> `SetInformationJobObject` -> démarrage drainage `stderr` -> `ResumeThread`. En cas d'échec à n'importe quelle étape : abattage forcé par `TerminateProcess`, attente, fermeture des handles et abandon sécurisé.
  * `ActiveProcessLimit=1` limite le nombre de processus actifs simultanés dans le Job (évite les forks concurrents), mais n'est pas un blocage universel de l'OS.
  * Le drainage de `stderr` est une **garantie conditionnelle** au fonctionnement normal de la boucle de lecture asynchrone de l'hôte. En cas d'incident du lecteur, le tampon de pipe peut se remplir et bloquer le Worker ; l'hôte surveille la réactivité par timeout et abat le Job Object en cas de blocage.

### Point 14 : Compatibilité Windows et systèmes de fichiers
- **Mandats concernés :** COR-24, COR-44, Mandat 011 (§4.9).
- **Problème récurrent :** Présentation de la compatibilité Windows 10/11 ou NTFS/ReFS comme des faits démontrés à partir de la seule vérification de `FILE_PERSISTENT_ACLS`.
- **État actuel dans V2.0.0 :**
  * Les versions de Windows et les systèmes de fichiers sont documentés comme une **matrice d'objectifs de test à valider sur cible physique**.
  * `FILE_PERSISTENT_ACLS` est une condition nécessaire mais non suffisante (les verrous Oplocks/Leasing et la sémantique POSIX diffèrent sur SMB).
  * Le répertoire scratch est formellement interdit sur partage réseau SMB ou FAT32/exFAT.

### Point 15 : Règle financière transversale de 0 € (`DEC-0004`)
- **Mandats concernés :** COR-05, COR-44, Mandat 011 (§4.10).
- **Problème récurrent :** Affirmation de gratuité totale à 100 % sur la base d'une simple énumération de licences déclaratives.
- **État actuel dans V2.0.0 :**
  * La règle souveraine de 0 € s'applique impérativement à toute la chaîne : dépendances directes et transitives, binaires natifs redistribués, SDK, runtime, absence d'abonnement ou de frais par poste/volume.
  * Un audit complet de la chaîne transitive (`dotnet list package`) devra être exécuté et documenté au stade de l'implémentation effective, sans présumer de sa validation anticipée.

---

## 3. Synthèse des réserves résiduelles d'architecture (Catégorie B)

Le dossier consolidé V2.0.0 est formellement classé en **CATÉGORIE B — PROPOSITION TECHNIQUE CONFORME SOUS RÉSERVES QUALIFIÉES** en raison des 6 réserves résiduelles circonscrites :

| N° | Réserve résiduelle | Niveau | Conséquence technique | Conduite recommandée pour B2 |
| :---: | :--- | :---: | :--- | :--- |
| **1** | **Isolation réseau post-RCE** | `NON_VERIFIE` | Le Job Object ne bloquant pas les sockets TCP, une RCE native pourrait communiquer sur le réseau. | Défense en profondeur acceptée en B2 ; étudier AppContainer ou règles de pare-feu WFP en phase ultérieure. |
| **2** | **Allocations amont BCL (ZIP/PAX)** | `RISQUE_BCL` | `ZipArchive` (Central Directory) et `TarReader` (PAX) allouent les structures en RAM avant intervention de GraphiScope. | Dimensionner le commit limite du Job Object pour absorber cette phase ; gérer la terminaison du Worker sur archives hostiles. |
| **3** | **Stockage distant SMB** | `NON_VERIFIE` | Les verrous `FILE_SHARE_DELETE` et la suppression POSIX ne sont pas garantis sur SMB. | Interdire formellement le répertoire scratch sur tout volume non local NTFS ou ReFS. |
| **4** | **Baseline cryptographique B1** | `DOCUMENTAIRE` | Absence d'empreintes individuelles à DEC-0005 empêchant toute preuve cryptographique rétroactive. | Utiliser la baseline SHA-256 du 14 novembre 2026 comme référence de non-régression opposable. |
| **5** | **Gestion d'allocation Job Object** | `SYSTEME` | Le refus d'allocation sous commit limite peut provoquer un crash du runtime au lieu d'une exception gérée. | Concevoir le Host pour détecter la mort du Worker sans présumer d'un message d'erreur applicatif ordonné. |
| **6** | **Drainage conditionnel stderr** | `SYSTEME` | En cas de défaillance de la tâche de lecture asynchrone hôte, le tube `stderr` peut saturer et suspendre le Worker. | Mettre en place un timeout d'inactivité global sur l'hôte avec abattage forcé du Job en cas de non-réponse. |

---

## 4. Consignes impératives pour les agents repreneurs

Tout agent reprenant ce projet après la présente mission doit se conformer strictement aux directives suivantes :

1. **NE PAS relancer de nouvelle boucle de correction documentaire automatique (COR-46, COR-47...) :**  
   Le dossier V2.0.0 est consolidé et terminal pour la phase d'étude B2. Toute anomalie ou réserve supplémentaire doit être consignée au journal, sans générer de nouveau mandat documentaire sans ordre du propriétaire.
2. **NE PAS démarrer l'implémentation logicielle des jalons B2 ou B3 sans mandat formel :**  
   Le statut de GS-0006 est `REVIEW`. Seul le propriétaire peut autoriser le passage à un jalon d'implémentation.
3. **NE PAS altérer les fichiers de code B1 existants :**  
   Les 37 fichiers de B1 sont formellement validés (`DEC-0005`) et constituent la base stable du cœur de GraphiScope.
4. **NE PAS transformer les propositions d'agent en décisions souveraines :**  
   Conserver EF Core comme simple proposition et maintenir l'alternative SQLite native directe documentée.
5. **NE PAS affirmer d'étanchéité absolue :**  
   Respecter le modèle de menace borné et documenté sans promettre des garanties matérielles que le noyau Windows NT ne fournit pas en espace utilisateur standard.


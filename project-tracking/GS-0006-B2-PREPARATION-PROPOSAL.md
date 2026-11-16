# DOSSIER DE CADRAGE TECHNIQUE PRÉPARATOIRE — JALON B2 DE GRAPHISCOPE (CONSOLIDATION TERMINALE V2.0.0)
**Référence opérationnelle :** `GS-OPS-GS0006-B2-PREPARATION-PROPOSAL-011`  
**Mandats associés :** `GS-OPS-GS0006-B2-PREPARATION-001`, `GS-OPS-GS0006-B2-PREPARATION-CORRECTION-001` à `007`, audit `GS-OPS-GS0006-B2-PREPARATION-AUDIT-008`, mandats `GS-OPS-GS0006-B2-PREPARATION-CORRECTION-009` et `010`, et mandat de consolidation terminale `GS-OPS-GS0006-B2-PREPARATION-CORRECTION-011`  
**Projet :** GraphiScope  
**Workspace :** `M:\dev\GraphiScope`  
**Jalon :** GS-0006 — Préparation documentaire B2 (Worker isolé sous Windows Job Objects et décompression d'archives)  
**Document de référence initial :** `GS-0003-PHASE-B-PREPARATION.md` V1.6.0  
**Statut du document :** PROPOSITION DE CADRAGE TECHNIQUE SOUMISE AU PROPRIÉTAIRE (STATUT : REVIEW)  
**Nature de la mission :** Passe de correction consolidée et terminale du dossier préparatoire B2 en application stricte du mandat `GS-OPS-GS0006-B2-PREPARATION-CORRECTION-011` : alignement strict de la gouvernance et des statuts de décisions (maintien de GS-0002 en `REVIEW`, distinction nette des décisions souveraines Q-001 à Q-008, préservation des arbitrages B2-ARB-01/02) ; rectification rigoureuse de la table de correspondance des 37 chemins cibles proposés pour B2 avec exigence de preuve de contenu et reclassement formel de la ligne erronée `CatalogException.cs` en `NOUVEAU` ; suppression de toute garantie cryptographique rétroactive sur B1 et qualification de la baseline du 14 novembre 2026 ; découplage des grandeurs mémoire et qualification sans complaisance des mécanismes IPC, Job Objects et BCL (ZIP, TAR/PAX) ; élimination de la contradiction mathématique sur la détection des archives auto-extractibles (SFX) et prise en compte des structures ZIP64 ; formalisation des chemins d'échec du Worker et drainage conditionnel de `stderr` ; bornage réaliste du threat model et explicitation des risques résiduels (réseau, fichiers utilisateur) ; requalification des systèmes de fichiers et environnements cibles ; et révision des critères d'acceptation dans un cadre vérifiable sans affirmation abusive d'étanchéité absolue.  
**Autorité mandante :** Propriétaire du projet GraphiScope  
**Date d'établissement :** 16 novembre 2026  

---

> [!IMPORTANT]
> **AVERTISSEMENT DE GOUVERNANCE STRICTE :**  
> Le présent document constitue exclusivement une **proposition de cadrage technique préparatoire**. Il n'autorise, n'amorce et ne constitue en aucun cas une implémentation logicielle du jalon B2 ou B3. Aucun fichier de code applicatif n'a été créé, modifié, déplacé ou supprimé. Le statut de la tâche antérieure `GS-0005` (B1) demeure strictement `DONE` et son acceptation formelle (`DEC-0005`) est pleinement acquise au titre du périmètre B1. L'acceptation de GS-0003 ou de GS-0005 ne vaut en aucun cas autorisation d'implémenter B2. Le dossier GS-0002 demeure au statut `REVIEW` au plan global tant que l'acceptation formelle du propriétaire n'a pas été prononcée, ses arbitrages individuels Q-001 à Q-008 demeurant pleinement valides et confirmés. Le statut de la présente tâche `GS-0006` demeure impérativement `REVIEW` jusqu'à décision d'acceptation formelle et souveraine du propriétaire.

---

## TABLE DES MATIÈRES
1. Résumé exécutif
2. Méthode de raisonnement et qualification formelle des connaissances
3. Tableau de correspondance des corrections consolidées
4. Délimitation stricte du périmètre : Traitement unitaire B2 vs Crawling récursif B3 et statut fonctionnel d'AncestorStack
5. Architecture technique de référence et responsabilités respectives Hôte / Worker
6. Politique adaptative des ressources système, cycle réel des allocations mémoire et protocole IPC conforme à Q-006
7. Confinement du système de fichiers, points de réanalyse et portée réelle des verrous sous NTFS
8. Traitement rigoureux des formats d'archives (ZIP, TAR, GZ) et politique des entrées TAR/PAX
9. Validation structurelle ZIP/ZIP64, calcul d'intégrité CRC-32, offsets SFX et limitations BCL
10. Création suspendue, flux standards, isolation opérationnelle, communication IPC et cycle de vie du Worker
11. Threat model du Worker, analyse des permissions effectives et frontières de sécurité
12. Inventaire exhaustif des 37 chemins cibles proposés pour B2, correspondance avec B1 et baseline SHA-256
13. Analyse des dépendances, statut d'Entity Framework Core et règle financière de 0 €
14. Compatibilité documentaire Windows 10/11 x64 et systèmes de fichiers
15. Registre qualitatif des risques techniques réels et réévaluation des niveaux résiduels
16. Matrice des preuves, sources officielles normatives et inventaire des points `NON_VERIFIE`
17. Stratégie de validation, tests unitaires et bancs d'essais sur cible
18. Décisions souveraines acquises et gouvernance
19. Critères objectifs d'acceptation du jalon B2
20. Conclusion formelle et recommandation de gouvernance (Catégorie B)

---

## 1. RÉSUMÉ EXÉCUTIF
Le présent dossier consolide le cadrage préparatoire du **jalon B2** de GraphiScope :  
*« Worker isolé sous Windows Job Objects et décompression d'archives »*.

Conformément au référentiel validé `GS-0003-PHASE-B-PREPARATION.md` V1.6.0 et aux décisions souveraines du propriétaire (`DEC-0001` à `DEC-0005`, Q-001 à Q-008, B2-ARB-01/02), le jalon B2 constitue le **verrou de sécurité amont obligatoire** avant toute extraction ou analyse approfondie de conteneurs de données. Il matérialise le principe directeur `SAFE-01` (« toute donnée d'entrée est réputée hostile jusqu'à preuve du contraire ») en confinant l'analyse et la décompression des archives dans un processus satellite dédié (`GraphiScope.Worker.exe`), régulé par les Job Objects du noyau Windows NT et privé de tout accès direct au catalogue de persistance SQLite.

La présente version consolidée et terminale **V2.0.0** résout l'ensemble des contradictions, approximations et affirmations non étayées identifiées dans les versions antérieures, conformément au mandat `GS-OPS-GS0006-B2-PREPARATION-CORRECTION-011` :
1. **Gouvernance et statut des décisions :** Maintien explicite du dossier global GS-0002 au statut `REVIEW`, tout en distinguant les décisions souveraines individuelles Q-001 à Q-008 qui sont pleinement confirmées. L'acceptation de GS-0003 et de GS-0005 ne constitue en rien une autorisation d'implémenter B2. Les décisions propriétaires B2-ARB-01 (Option A) et B2-ARB-02 (Option A) sont intégralement préservées sans réouverture.
2. **Correspondance rigoureuse des 37 chemins cibles proposés pour B2 :** Confrontation chemin par chemin avec le manifeste exhaustif de B1 (37 fichiers physiques). Correction formelle de la ligne `CatalogException.cs` (le chemin source `Persistence/Exceptions.cs` n'ayant jamais existé dans B1) reclassée en `NOUVEAU`. Élimination des classifications `ADAPTE` infondées : seules les 3 lignes présentant une correspondance textuelle et conceptuelle prouvée sont maintenues en `ADAPTE`, portant le bilan objectif à **0 `REUTILISE_IDENTIQUE` (0 %)**, **3 `ADAPTE` (8,1 %)**, **34 `NOUVEAU` (91,9 %)** et **0 `NON_DEMONTRE`**. Les 37 chemins sont qualifiés comme **chemins cibles proposés pour B2** et non comme composants approuvés. Entity Framework Core demeure une simple proposition d'agent non validée par le propriétaire.
3. **Qualification exacte de l'intégrité de B1 :** Suppression de toute garantie cryptographique rétroactive ou présomption abusive d'intégrité absolue à DEC-0005, l'absence d'empreintes SHA-256 individuelles enregistrées à cette date empêchant formellement toute preuve cryptographique rétroactive. Les empreintes du 14 novembre 2026 sont arrêtées comme une **photographie de référence** pour le suivi futur, tandis que l'intégrité opérationnelle de B1 repose sur la présence physique des fichiers, leur compilation sans erreur et le succès des 30 tests xUnit automatisés.
4. **Découplage strict des grandeurs mémoire et limitations IPC :** Découplage complet entre taille de trame filaire ($\le 4\text{ Mo}$), sérialisation, buffers applicatifs, buffers noyau de pipes (`npfs.sys`), désérialisation, objets managés, rétention `ArrayPool`, mémoire engagée privée (*Commit*) et mémoire résidente (*Working Set*). La limite de trame IPC à 4 Mo n'est pas une limite mémoire globale. Le Job Object protège les ressources du système face au Worker, mais ne protège pas la mémoire du Host. Retrait de l'affirmation non démontrée sur une taille fixe garantie de 4 Ko pour les buffers de pipes anonymes. Qualification exacte de `Utf8JsonReader` (lecteur bas niveau ne construisant pas d'objets managés). Prise en compte explicite de la matérialisation mémoire amont incontournable subie dans le tas du Worker lors de la lecture différée du Central Directory par `ZipArchive` et du parsing des attributs étendus PAX par `TarReader`.
5. **Résolution de la contradiction mathématique sur les calculs d'offsets ZIP / SFX :** Élimination de l'exigence contradictoire $O_{cd} + S_{cd} = P_{eocd}$ qui rejetait indûment les archives auto-extractibles (SFX). Modélisation d'une séquence cohérente : calcul du décalage candidat $\Delta = P_{eocd} - (O_{cd} + S_{cd})$, validation des structures ajustées à $O_{cd} + \Delta$ dans les limites physiques du fichier. Gestion distincte du ZIP64 EOCD Locator ($P_{eocd} - 20$) et de l'EOCD64 Record. Distinction entre arithmétique `checked` et validation sémantique des bornes. Séparation stricte entre CRC-32 IEEE 802.3 (`0xEDB88320`) et SSE4.2 CRC32C Castagnoli (`0x1EDC6F41`), et précision que `PCLMULQDQ` ne dépend pas d'AVX/AVX-512.
6. **Politique TAR/PAX clarifiée :** Énumération explicite des entrées régulières et répertoires acceptés pour l'extraction vs rejet strict de toutes les entrées spéciales (`SymbolicLink`, `HardLink`, `CharacterDevice`, `BlockDevice`, `Fifo`), ignorées et marquées `IGNORED_SPECIAL_ENTRY`. Maintien du risque d'allocation amont PAX comme non éliminé par la troncature aval IPC.
7. **Chaîne de lancement du Worker et gestion d'erreurs :** Description exhaustive des chemins d'échec pour `CreateProcessW` suspendu, `AssignProcessToJobObject`, `SetInformationJobObject` et `ResumeThread`, avec terminaison propre et fermeture des descripteurs. Requalification d'`ActiveProcessLimit=1` (limitation du nombre de processus actifs simultanés dans le job, non interdiction universelle de créer des processus). Drainage de `stderr` requalifié en garantie conditionnelle au fonctionnement normal du lecteur asynchrone, avec gestion du blocage potentiel.
8. **Confinement du système de fichiers et modèle de menace borné :** Portée exacte de l'omission de `FILE_SHARE_DELETE` (interdit l'ouverture avec droit `DELETE` ou sans partage sous NTFS local, mais n'empêche pas l'ajout de fichiers ni les actions d'un administrateur). Distinction entre isolation inter-utilisateurs par DACL et absence d'isolation entre processus d'une même session utilisateur. Distinction entre absence de code SQLite/réseau dans le Worker et absence de restriction OS si le Worker subit une exécution de code arbitraire natif (RCE).
9. **Compatibilité Windows et matrices d'essais :** Requalification des systèmes de fichiers et versions de Windows comme matrice d'objectifs de test et non comme faits d'ores et déjà validés. L'indicateur `FILE_PERSISTENT_ACLS` n'est pas le seul critère de compatibilité.
10. **Règle financière 0 € :** Rappel de la gratuité financière absolue (0 €) exigée par DEC-0004 sur l'ensemble de la chaîne, y compris dépendances transitives, outillage, SDK et redistribution, devant faire l'objet d'un audit formel préalable lors du mandat d'implémentation.

---

## 2. MÉTHODE DE RAISONNEMENT ET QUALIFICATION FORMELLE DES CONNAISSANCES
L'ensemble des analyses techniques du présent dossier applique scrupuleusement la démarche d'ingénierie et de preuve en sept étapes :
1. **Compréhension du problème :** Identification de la cause technique sous-jacente, impact architectural et respect des exigences souveraines du propriétaire.
2. **Identification des hypothèses implicites :** Élimination des suppositions non vérifiées relatives aux comportements OS, aux allocations de la BCL ou aux garanties cryptographiques.
3. **Qualification formelle des connaissances :** Typologie stricte appliquée à chaque assertion technique :
   - `DECISION_PROPRIETAIRE` : Décision souveraine du propriétaire (`DEC-0001` à `DEC-0005`, Q-001 à Q-008, B2-ARB-01/02).
   - `SOURCE_OFFICIELLE` : Information étayée par une documentation officielle vérifiable issue d'une version stable figée (Microsoft Learn, Intel SDM, PKWARE APPNOTE, code source .NET 10 LTS tag `v10.0.0`).
   - `TESTE_DEV` : Propriété vérifiée empiriquement sur l'environnement de développement.
   - `TESTE_CIBLE` : Propriété vérifiée sur un PC cible physique représentatif.
   - `HYPOTHESE` : Paramètre ou modèle de dimensionnement de conception en attente de qualification instrumentée.
   - `NON_VERIFIE` : Propriété ou comportement non encore formellement prouvé sur cible.
   - `RISQUE` : Scénario défavorable documenté avec ses mécanismes de détection et d'atténuation.
   - `LIMITATION` : Frontière technique documentée d'un mécanisme ou d'une API.
   - `BLOQUANT` : Défaut empêchant la validation du cadrage.
4. **Examen de la séquence opérationnelle réelle :** Description pas à pas de l'ordre réel d'exécution, des handles créés, des points de défaillance possibles, des vérifications préalables et postérieures, de la concurrence, des interruptions et des nettoyages.
5. **Analyse approfondie des cas défavorables :** Évaluation des entrées hostiles, saturations, erreurs d'E/S, arrêts inopinés et corruptions de structures.
6. **Vérification rigoureuse des garanties :** Démonstration du mécanisme exact fournissant la protection, des conditions requises, des cas non couverts et des limites réelles.
7. **Traitement des problèmes émergents :** Prise en compte et résolution documentée de tout problème technique découvert, sans dérivation de périmètre.

---

## 3. TABLEAU DE CORRESPONDANCE DES CORRECTIONS CONSOLIDÉES
Le tableau ci-dessous synthétise la prise en compte des réserves antérieures et l'intégration des corrections substantielles exigées par le mandat `GS-OPS-GS0006-B2-PREPARATION-CORRECTION-011` :

| Groupe de corrections | Nature du problème traité | Résolution technique consolidée (V2.0.0) | Statut |
| :--- | :--- | :--- | :---: |
| **Gouvernance (§4.1)** | Statut ambigu de GS-0002 et confusion entre autorisations B1 et B2 | GS-0002 maintenu explicitement en `REVIEW` ; distinction formelle des décisions Q-001 à Q-008 ; rappel que DEC-0003/DEC-0005 n'autorisent pas B2 ; B2-ARB-01/02 préservés. | `CORRIGE` |
| **Correspondance 37 chemins (§4.2)** | Ligne `CatalogException.cs` erronée, classifications `ADAPTE` sans preuve | Correction de la ligne 3 (`CatalogException.cs`) reclassée en `NOUVEAU` (source inexistante) ; seules 3 lignes avec preuve de contenu maintenues en `ADAPTE` (totaux : 0 `REUTILISE_IDENTIQUE`, 3 `ADAPTE`, 34 `NOUVEAU`) ; qualification en « chemins cibles proposés pour B2 » ; EF Core maintenu comme proposition d'agent. | `CORRIGE` |
| **Intégrité B1 (§4.3)** | Affirmations d'intégrité absolue rétroactive à DEC-0005 | Suppression des prétentions de preuve cryptographique rétroactive ; reconnaissance de l'absence de SHA-256 à DEC-0005 ; hashes du 14/11/2026 définis comme photographie de référence ; vérifications réelles documentées (compilation, tests). | `CORRIGE` |
| **IPC et mémoire (§4.4)** | Confusion limites de trame vs processus, Job Object vs Host, buffers pipes | Découplage des 9 composantes mémoire ; trame 4 Mo découplée du processus ; Job Object protégeant l'OS et non le Host ; requalification des erreurs Job Object ; suppression du défaut garanti de 4 Ko pour pipes ; `Utf8JsonReader` qualifié ; matérialisation amont BCL admise. | `CORRIGE` |
| **ZIP, ZIP64, CRC (§4.5)** | Contradiction mathématique SFX ($O_{cd} + S_{cd} = P_{eocd}$), ZIP64, SSE4.2 | Calcul du décalage candidat $\Delta = P_{eocd} - (O_{cd} + S_{cd})$ sans rejet préalable ; structures ZIP64 EOCD Locator/Record distinguées ; arithmétique `checked` vs validation sémantique ; SSE4.2 CRC32C vs IEEE 802.3 PKZIP ; PCLMULQDQ dissocié d'AVX ; Q-007 maintenu. | `CORRIGE` |
| **TAR/PAX (§4.6)** | Contradiction rejet entrées spéciales vs répertoires, allocations PAX | Énumération claire : fichiers réguliers et répertoires acceptés vs liens symboliques, hardlinks, pipes et périphériques rejetés ; risque d'allocation amont par `TarReader` maintenu `NON_VERIFIE`. | `CORRIGE` |
| **Worker et Job Object (§4.7)** | Chemins d'échec Win32 incomplets, portée `ActiveProcessLimit=1`, stderr | Séquence de lancement avec gestion d'échec et nettoyage pour chaque API ; `ActiveProcessLimit=1` qualifié comme limite concurrente ; drainage stderr conditionné au lecteur sain avec gestion d'incident ; Job Object qualifié de défense en profondeur. | `CORRIGE` |
| **Confinement scratch (§4.8)** | Portée excessive attribuée à `FILE_SHARE_DELETE` et DACL, frontière OS | Portée exacte de `FILE_SHARE_DELETE` et limites (créations concurrentes, admin) documentées ; DACL inter-utilisateurs vs même session ; distinction absence de code vs restrictions OS ; modèle de menace borné sans prétention d'étanchéité absolue. | `CORRIGE` |
| **Compatibilité Windows (§4.9)** | Assimilation de cibles à des faits démontrés, raccourci `FILE_PERSISTENT_ACLS` | Systèmes de fichiers et OS présentés comme matrice d'objectifs de test à valider sur cible ; indicateur d'ACL nécessaire mais non suffisant ; distinction stricte conception vs tests réels. | `CORRIGE` |
| **Architecture et Coût (§4.10)** | Affirmation de gratuité 100% sans audit complet, statut d'EF Core | Règle 0 € rappelée sur l'ensemble de la chaîne de dépendances directes et indirectes avec obligation d'audit complet à l'implémentation ; EF Core maintenu comme simple proposition d'agent face à SQLite direct. | `CORRIGE` |

---

## 4. DÉLIMITATION DU PÉRIMÈTRE : TRAITEMENT UNITAIRE B2 VS CRAWLING RÉCURSIF B3 ET STATUT D'ANCESTORSTACK
### 4.1 Principe fondamental de délimitation
Le jalon B2 est strictement dédié à **l'extraction et l'analyse unitaire d'un conteneur d'archive physique isolé**.  
Le jalon B3 est dédié au **crawling récursif du système de fichiers, à la descente en profondeur dans les conteneurs imbriqués et à l'alimentation transactionnelle en cascade du catalogue SQLite**.

| Caractéristique opérationnelle | Jalon B2 (Traitement unitaire) | Jalon B3 (Crawling récursif) |
| :--- | :--- | :--- |
| **Périmètre d'entrée** | Une seule archive physique désignée à la fois. | Multiples racines d'arborescence, partages réseau, disques locaux. |
| **Conteneurs imbriqués** | Si une entrée extraite est une sous-archive (`.zip`, `.tar`), elle est inventoriée comme simple fichier avec indicateur de format ; **aucune décompression récursive n'est déclenchée en B2**. | Décompression récursive automatique en cascade des sous-archives selon les règles de profondeur définies. |
| **Accès persistance SQLite** | **Aucun accès direct.** Le Worker transmet ses métadonnées à l'hôte via IPC. | L'hôte orchestre l'insertion des éléments de catalogue et des relations parent-enfant dans SQLite. |
| **Répertoire scratch** | Un répertoire temporaire unique dédié créé par archive unitaire et détruit dès la fin du traitement de cette archive. | Gestion coordonnée des répertoires scratch à travers les niveaux de récursion. |

### 4.2 Statut fonctionnel et rôle exact d'AncestorStack (Chemin proposé n° 33)
Le composant `AncestorStack.cs` (`src/GraphiScope.Core/Worker/AncestorStack.cs`) figure parmi les 37 chemins cibles proposés pour B2 :
1. **Rôle effectif en B2 :**
   - `AncestorStack` est conçu comme une structure de données immuable représentant l'identité physique de l'archive en cours d'analyse unitaire (`VolumeSerialNumber`, `FileIndexHigh`, `FileIndexLow` / `FILE_ID_128`, chemin normalisé).
   - En B2, sa taille est au plus de profondeur 1 (l'archive racine traitée). Sa fonction exclusive est d'empêcher toute tentative d'auto-extraction circulaire où une archive tenterait d'écrire ou de pointer sur son propre conteneur parent.
2. **Réserve formelle pour B3 :**
   - L'empilement multi-niveaux d'ancêtres, la propagation des chaînes d'héritage d'archives imbriquées (ex. `archive1.zip -> archive2.tar -> archive3.gz`), et l'insertion des liens hiérarchiques dans les tables SQLite sont **formellement réservés au jalon B3**.
   - Le composant `AncestorStack` ne réalise aucun appel SQLite, aucune opération réseau et aucune instanciation de processus en B2.

---

## 5. ARCHITECTURE TECHNIQUE DE RÉFÉRENCE ET RESPONSABILITÉS RESPECTIVES

```
+---------------------------------------------------------------------------------------+
| PROCESSUS HÔTE (GraphiScope.Host.exe / GraphiScope.Server.exe)                        |
| - Runtime : .NET 10 LTS x64 (Kestrel, Persistance SQLite locale, Confinement, UI)     |
| - Écoute Kestrel exclusive : 127.0.0.1 (Loopback) avec jeton CSRF et Host/Origin check|
| - Accès SQLite exclusif : %LOCALAPPDATA%\GraphiScope\Catalog\graphiscope.db           |
|                                                                                       |
| [HostScanningCoordinator]                                                             |
|       |                                                                               |
|       v                                                                               |
| [WorkerClient]                                                                        |
|   - Mesure ullAvailPhys et CommitMargin (psapi / GlobalMemoryStatusEx)                |
|   - Configure JobObjectManager avec limite de commit privé adaptée                    |
|   - Crée tubes anonymes, retire HANDLE_FLAG_INHERIT sur l'hôte                        |
|   - Lance GraphiScope.Worker.exe suspendu (CreateProcessW CREATE_SUSPENDED)           |
|   - Assigne le Worker au Job Object avant ResumeThread (abattage si échec)            |
|   - Démarre la tâche de drainage asynchrone borné de stderr (flux d'octets, 64 Ko)    |
|   - Dialogue via IPC en trames binaires délimitées en longueur (B2-ARB-01 Option A)   |
|   - Réceptionne les métadonnées et alimente le catalogue SQLite                       |
+---------------------------------------------------------------------------------------+
            | stdin (commandes délimitées)          ^ stdout (lots d'inventaire délimités)
            | (handle hérité exclusif)              | (handle hérité exclusif)
            v                                       |
+---------------------------------------------------------------------------------------+
| PROCESSUS SATELLITE (GraphiScope.Worker.exe) — CONFINÉ SOUS JOB OBJECT NT             |
| - Runtime : .NET 10 LTS x64 Console autonome (Dédié extraction / analyse unitaire)    |
| - AUCUN accès direct à SQLite, AUCUNE dépendance Kestrel, AUCUN code réseau           |
| - Régulation Job Object : KILL_ON_JOB_CLOSE, ACTIVE_PROCESS_LIMIT=1, DIE_ON_UNHANDLED|
| - Traitement unitaire : Une archive physique à la fois                                |
|                                                                                       |
| 1. [ZipHeaderReader] : Préinspection binaire structurelle (EOCD, ZIP64, CDFH)        |
|    - Vérifie intégrité des en-têtes sans allouer de payload                           |
|    - Détecte chiffrement (Bit 0, Bit 6, Méthode 99 AES) -> PROTECTED_OPAQUE (Q-007)  |
|    - Calcule décalage candidat Delta SFX sans rejet préalable                         |
|                                                                                       |
| 2. [ArchiveExtractor] : Décompression en streaming BCL (System.IO.Compression)        |
|    - Traite ZIP/Zip64/TAR/GZ exclusivement via la BCL .NET 10 (B2-ARB-02 Option A)    |
|    - Écrit dans le scratch via NtCreateFile(RootDirectory) sans FILE_SHARE_DELETE    |
|    - Rejette tout point de réanalyse (FILE_OPEN_REPARSE_POINT, tag REPARSE_POINT)    |
|    - Valide chemin canonique final par GetFinalPathNameByHandleW et numéro de série   |
|    - Capture les erreurs Win32 ERROR_DISK_FULL (0x70) et ERROR_HANDLE_DISK_FULL (0x27)|
|    - Valide le CRC-32 IEEE en streaming sur flux complet jusqu'à EOF                  |
|    - Nettoie le scratch via FileDispositionInfoEx (sémantique POSIX)                  |
+---------------------------------------------------------------------------------------+
```

---

## 6. POLITIQUE ADAPTATIVE DES RESSOURCES SYSTÈME, CYCLE RÉEL DES ALLOCATIONS MÉMOIRE ET PROTOCOLE IPC CONFORME À Q-006

### 6.1 Principe fondamental de non-arbitraire (Q-006)
La décision souveraine **`Q-006`** dispose qu'aucun quota numérique universel fixe (ni taille maximale de fichier, ni nombre d'entrées, ni profondeur, ni volume total, ni délai d'attente fixe) ne doit être imposé de manière arbitraire.
- **Statut des formules antérieures :** Toute formule de calcul ou pourcentage (ex. 25 % ou 50 %) mentionné dans les versions de travail antérieures ou dans le journal d'exécution n'a qu'une valeur purement illustrative. Aucune formule fixe ne constitue une règle normative de GraphiScope.
- **Régulation adaptative :** Le dimensionnement des limites de ressources est une grandeur dynamique déterminée lors de l'initialisation du service hôte à partir des mesures physiques de l'environnement (`GlobalMemoryStatusEx`, `GetPerformanceInfo`) et injectée de manière configurable via `WorkerConfiguration.cs`.

### 6.2 Analyse approfondie des grandeurs mémoire sous Windows NT
1. **Mémoire physique (RAM) :**
   - Interrogée via `GlobalMemoryStatusEx` (`kernel32.dll`) remplissant la structure `MEMORYSTATUSEX`.
   - `ullTotalPhys` : Capacité physique totale de la mémoire vive installée sur la machine hôte.
   - `ullAvailPhys` : Quantité physique de RAM non allouée à l'instant de la mesure. Grandeur volatile soumise aux fluctuations du cache système (`Standby list`) et aux allocations concurrentes.
2. **Charge validée globale du système (*System Commit*) :**
   - Interrogée via `GetPerformanceInfo` (`psapi.dll`) remplissant la structure `PERFORMANCE_INFORMATION`.
   - `CommitTotal` : Nombre total de pages système de 4 Ko actuellement engagées par l'ensemble des processus et le noyau.
   - `CommitLimit` : Nombre maximal de pages système que l'ensemble du système d'exploitation peut engager sans agrandir les fichiers de pagination (`pagefile.sys`).
   - Marge de commit disponible système :  
     $$M_{commit\_margin} = (\text{CommitLimit} - \text{CommitTotal}) \times \text{PageSize}$$
   - **Règle fondamentale de distinction :** `ullAvailPhys` et $M_{commit\_margin}$ mesurent deux réalités physiques et logiques différentes qui ne doivent jamais être fusionnées en une variable unique. Si $M_{commit\_margin}$ est épuisé, une allocation échoue même s'il reste de la RAM physique libre ; si la RAM physique est saturée alors que le commit est large, les allocations réussissent mais provoquent une dégradation massive par pagination sur disque (*swapping*).
3. **Portée réelle de `JOB_OBJECT_LIMIT_JOB_MEMORY` et qualification des erreurs :**
   - L'indicateur `JOB_OBJECT_LIMIT_JOB_MEMORY` (configuré via `SetInformationJobObject` avec `JobMemoryLimit`) applique un **plafond d'engagement privé (*Private Commit Limit*)** cumulé à l'échelle du Job Object.
   - **Ce qu'il fait (`SOURCE_OFFICIELLE`) :** Dès que la somme de la mémoire engagée privée du Worker dépasse la valeur `JobMemoryLimit`, le noyau NT refuse toute allocation virtuelle ultérieure (`VirtualAlloc`).
   - **Qualification objective des conséquences (`LIMITATION`) :** Le dépassement du quota de Job Object entraîne classiquement le code Win32 `ERROR_COMMITMENT_LIMIT` (code 1455) ou `ERROR_NOT_ENOUGH_MEMORY` (code 8). Toutefois, sous le runtime CoreCLR .NET 10, **ce refus d'allocation ne se traduit pas systématiquement par une `OutOfMemoryException` catchable de manière ordonnée**. Selon le composant appelant (runtime interne, threadpool, compilation JIT, ou BCL), le dépassement peut provoquer un crash immédiat du runtime avec code de sortie anormal, un arrêt brutal par le noyau ou une terminaison du processus sans exécution des blocs `finally`. Le Host doit donc être conçu pour gérer la mort brutale du Worker sans présumer d'une exception gérée.
   - **Ce qu'il ne fait PAS (`LIMITATION`) :** Ce n'est pas une limite de mémoire physique résidente (*Working Set*). De plus, **le Job Object encadre exclusivement le Worker et ne constitue en aucun cas une protection de la mémoire du Host**.

### 6.3 Mesure et gestion adaptative du stockage disque
1. **Grandeurs de `GetDiskFreeSpaceExW` et quotas :**
   - Utilisation impérative de **`lpFreeBytesAvailableToCaller`** qui prend en compte les quotas NTFS assignés à l'utilisateur exécutant, et non `lpTotalNumberOfFreeBytes`.
2. **Capture des erreurs d'E/S de saturation disque :**
   - Une vérification périodique de l'espace disque libre ne peut garantir la disponibilité continue du stockage (un processus tiers pouvant saturer le volume à tout instant).
   - La protection repose sur la **capture systématique des erreurs d'E/S au niveau de `WriteFile` / `FileStream.Write`** :
     * `ERROR_DISK_FULL` (code 112 / 0x70) ;
     * `ERROR_HANDLE_DISK_FULL` (code 39 / 0x27).
   - En cas d'erreur de disque plein, le flux d'écriture est immédiatement interrompu, les descripteurs de fichiers sont fermés, et la purge complète du répertoire temporaire scratch est déclenchée via `FileDispositionInfoEx`. L'archive est cataloguée avec le statut `EXTRACTION_ABORTED_STORAGE_EXHAUSTED`.

### 6.4 Découplage strict des grandeurs mémoire et cycle réel des allocations IPC
Le protocole IPC applique **`B2-ARB-01 Option A`** (trames binaires délimitées en longueur sur tubes anonymes standards).

#### 6.4.1 Découplage des neuf composantes mémoire de l'architecture IPC
Pour éradiquer toute confusion entre limite de trame et mémoire globale du processus, neuf composantes mémoire distinctes sont formellement dissociées :
1. **Taille du contenu filaire de la trame (`PayloadSize` $\le 4\text{ Mo}$) :** La taille brute des octets circulant sur le tube. Plafonnée à 4 Mo (`MaxIpcMessageSizeBytes`). **Ce plafond filaire ne constitue en aucun cas une limite sur la mémoire globale du Worker ou du Host.**
2. **Mémoire temporaire de sérialisation (Worker) :** Mémoire allouée lors de la sérialisation JSON via `ArrayBufferWriter<byte>` ou buffer loué avant émission.
3. **Mémoire des buffers applicatifs :** Buffers de travail manipulés par le code de GraphiScope pour l'extraction et la lecture en flux.
4. **Mémoire des buffers du noyau Windows NT (Pipe Buffers) :**  
   Dans le noyau Windows NT, le pilote de système de fichiers de tubes (`npfs.sys`) gère un tampon interne pour chaque tube créé par `CreatePipe`.  
   *Correction technique formelle :* **Il n'existe aucune taille fixe de 4 Ko universellement garantie par défaut pour les tubes anonymes sous Windows NT.** La taille allouée par le noyau dépend des paramètres de création, de la mémoire disponible du pool non pagé (*Non-Paged Pool*) et de l'implémentation de la version de Windows NT. Cette mémoire réside en espace noyau et suspend l'émetteur de `WriteFile` dès qu'elle est pleine jusqu'à consommation par `ReadFile`.
5. **Mémoire de réception et désérialisation (Hôte) :** Tampon loué auprès de `ArrayPool<byte>.Shared` pour recevoir les octets filaires après validation de la longueur $L \le 4\text{ Mo}$.
6. **Rétention mémoire par les pools (`ArrayPool<byte>.Shared`) :**  
   Les tableaux restitués à `ArrayPool<byte>.Shared` sont conservés en mémoire vive dans des seaux (*buckets*) par le runtime CoreCLR pour réutilisation ultérieure. Cette mémoire demeure engagée dans la mémoire résidente du processus même après traitement de la trame.
7. **Rôle réel de `Utf8JsonReader` et construction des objets managés :**  
   *Correction technique formelle :* **`Utf8JsonReader` est une structure ref C# de parsing bas niveau en flux continu qui ne construit, à elle seule, aucun objet applicatif.** Elle se borne à identifier les jetons JSON (noms de propriétés, valeurs scalaires, délimiteurs). La construction effective des objets du domaine managé (chaînes UTF-16, listes de `ArchiveEntryMetadata`, DTOs) nécessite du code d'assemblage applicatif ou l'appel à `JsonSerializer.Deserialize`, qui alloue de nouveaux objets dans le tas managé.
8. **Mémoire des objets managés désérialisés (Hôte) :**  
   Les objets C# construits occupent de la mémoire dans le tas managé de l'hôte. En mémoire managée 64 bits, une chaîne de caractères UTF-16 occupe $26 + 2 \times \text{Length}$ octets. Par conséquent, une collection de métadonnées désérialisée peut occuper 2 à 3 fois plus de place en RAM managée que sa représentation JSON UTF-8 sur le fil.
9. **Mémoire engagée privée (*Private Commit*) et mémoire résidente (*Working Set*) :**  
   La mémoire engagée totale du Worker englobe le runtime CoreCLR, le tas GC, la BCL, les piles de threads et les tampons d'E/S, encadrée par `JOB_OBJECT_LIMIT_JOB_MEMORY`. Le Working Set représente la fraction de cette mémoire actuellement présente en RAM physique.

#### 6.4.2 Budget de lotissement et traitement des éléments volumineux
- **Budget de lotissement indicatif ($B_{payload} = 512\text{ Ko}$ à $1\text{ Mo}$) :** Les valeurs 512 Ko–1 Mo ne sont pas des plafonds universels immuables, mais des **objectifs de traitement indicatifs** pour regrouper les entrées en lots homogènes sans surcharger les tampons d'E/S.
- **Éléments volumineux et troncature :** Si une entrée individuelle présente des métadonnées volumineuses excédant le budget mais $\le 4\text{ Mo}$, elle est émise dans sa propre trame dédiée. Si les métadonnées excèdent 4 Mo, le Worker refuse d'émettre une trame hors limite, applique une troncature sécurisée des champs textuels (chemin tronqué à 4 096 octets avec le marqueur `[TRUNCATED_PATH]`, commentaire ignoré) et émet un avertissement `WarningFlags.METADATA_OVERSIZED_TRUNCATED`.
- **Limite de la troncature aval :** **Une troncature effectuée après coup lors de l'émission IPC ne neutralise pas le risque d'une allocation excessive survenue en amont dans le tas du Worker.** Si une archive contient des en-têtes TAR/PAX géants ou un Central Directory ZIP massif, l'allocation est subie par la BCL avant toute intervention du code de GraphiScope.

---

## 7. CONFINEMENT DU SYSTÈME DE FICHIERS, POINTS DE RÉANALYSE ET PORTÉE RÉELLE DES VERROUS SOUS NTFS

### 7.1 Portée réelle et limites documentées de `FILE_SHARE_DELETE` sous Windows NT
La sémantique documentée du système de fichiers NTFS (`SOURCE_OFFICIELLE`) établit les faits suivants :
1. **Ce que l'omission de `FILE_SHARE_DELETE` garantit effectivement :**
   - Lorsqu'un handle sur un répertoire $D$ est ouvert par GraphiScope avec `ShareAccess = FILE_SHARE_READ | FILE_SHARE_WRITE` (exclusion de `FILE_SHARE_DELETE`), le pilote du système de fichiers local (`NTFS.sys`) rejette toute tentative ultérieure émanant d'un autre processus d'ouvrir ce même répertoire $D$ avec le droit d'accès `DELETE` ou sans autoriser le partage de suppression.
   - Par conséquent, sous NTFS local, aucun processus tiers standard ne peut invoquer `RemoveDirectoryW(D)` ou `MoveFileExW(D, ..., MOVEFILE_REPLACE_EXISTING)` pendant que ce handle est maintenu ouvert (renvoi immédiat de `ERROR_SHARING_VIOLATION`).
2. **Ce que l'omission de `FILE_SHARE_DELETE` ne garantit PAS (`LIMITATION`) :**
   - **Handles préexistants :** Elle n'annule pas les droits d'un handle qui aurait été ouvert sur ce répertoire par un tiers *avant* l'ouverture par GraphiScope.
   - **Créations et modifications dans le répertoire :** **Elle n'empêche nullement un autre processus disposant des droits NTFS d'ouvrir des fichiers à l'intérieur de $D$, d'y créer de nouveaux sous-répertoires ou de modifier son contenu.**
   - **Privilèges d'administration et pilotes noyau :** Elle ne neutralise pas les opérations exécutées avec des privilèges administrateur (`SeBackupPrivilege`, `SeRestorePrivilege`) ou par un pilote de filtre de système de fichiers (*Kernel Minifilter Driver*).

### 7.2 Séquence opérationnelle d'extraction confinée en 12 étapes
Pour assurer le confinement le plus rigoureux réalisable en mode utilisateur sous Windows NT, la séquence suivante est appliquée :
1. Création sécurisée de la racine scratch `%TEMP%\GraphiScope_Scratch_<UUID>` avec descripteur de sécurité explicite (`SE_DACL_PROTECTED`) limitant les droits au compte utilisateur courant (`TokenUser`) et à `SYSTEM`.
2. Ouverture et maintien ouvert du handle de racine `hScratchDir` sans `FILE_SHARE_DELETE` pendant toute la durée du traitement de l'archive.
3. Validation syntaxique préalable du chemin relatif (rejet des deux-points `:`, des séquences `..`, des noms réservés MS-DOS et des préfixes UNC).
4. Découpage du chemin en segments relatifs simples sans barre oblique.
5. Pour chaque segment de répertoire intermédiaire :
   - Invocation de `NtCreateFile` avec `RootDirectory = hParent` et l'option `FILE_OPEN_REPARSE_POINT` (0x00200000) ;
   - Interrogation immédiate de `GetFileInformationByHandleEx(FileAttributeTagInfo)` : si le drapeau `FILE_ATTRIBUTE_REPARSE_POINT` est présent, **interruption immédiate** et levée de `ConfinementException` ;
   - Maintien ouvert du handle parent sans `FILE_SHARE_DELETE`.
6. Création exclusive de la feuille : `NtCreateFile(RootDirectory=hFinalParent, ObjectName="file.ext", CreateDisposition=FILE_CREATE, ShareAccess=FILE_SHARE_READ)` (échec immédiat si collision de nom).
7. Contrôle d'identité post-création sur descripteur ouvert `hFile` :
   - Comparaison canonique : `GetFinalPathNameByHandleW(hFile)` doit débuter strictement par le préfixe canonique du dossier scratch ;
   - Comparaison d'identité de volume : `dwVolumeSerialNumber` de `hFile` doit être identique à celui de `hScratchDir`.
8. Enveloppement managé dans `SafeFileHandle` et `FileStream`.
9. Décompression et écriture en streaming par blocs de 64 Ko.
10. Calcul et vérification active du checksum CRC-32 IEEE en streaming jusqu'à `EOF`.
11. Clôture ordonnée du flux (`Flush` / `Dispose`) et fermeture des handles parents intermédiaires.
12. En cas d'erreur ou à la clôture de l'archive : suppression POSIX immédiate via `FileDispositionInfoEx` (`FILE_DISPOSITION_FLAG_POSIX_SEMANTICS | FILE_DISPOSITION_FLAG_DELETE`).

### 7.3 Analyse de `NtCreateFile` et `RootDirectory`
- `NtCreateFile` est l'appel système natif de création de fichier du sous-système Windows NT (`ntdll.dll`).
- **Sémantique de `RootDirectory` :** Lorsqu'un descripteur de répertoire valide est fourni dans `OBJECT_ATTRIBUTES.RootDirectory` et que `ObjectName` est un nom relatif simple sans antislash, le gestionnaire d'objets du noyau résout le nom **exclusivement dans l'espace de nommage de ce répertoire parent**.
- L'itération segment par segment empêche la traversée automatique des points de réanalyse intermédiaires que le noyau NT effectuerait lors de la résolution globale d'un chemin multi-segments.
- **Réserve de qualification (`LIMITATION`) :** Bien que chaque API soit officiellement documentée, cette combinaison applicative constitue un dispositif de défense en profondeur sous NTFS local et **ne peut être qualifiée de bac à sable hermétique absolu**. Elle ne couvre pas toutes les conditions de course matérielles, les hardlinks préexistants ou les interférences au niveau de la session.

### 7.4 Limites réelles des listes de contrôle d'accès (DACL) du répertoire temporaire
- **Ce que la DACL isole (`SOURCE_OFFICIELLE`) :** Elle interdit aux autres comptes utilisateurs de la machine et aux services non privilégiés d'accéder au dossier scratch ou de manipuler ses entrées.
- **Ce que la DACL n'isole PAS (`LIMITATION`) :**  
  **Elle n'isole pas le Worker des autres processus exécutés sous la même session utilisateur.** Un logiciel tiers s'exécutant sous le même compte que GraphiScope possède le même SID `TokenUser` et dispose donc des droits NTFS d'accès au répertoire scratch.

---

## 8. TRAITEMENT RIGOUREUX DES FORMATS D'ARCHIVES (ZIP, TAR, GZ) ET POLITIQUE DES ENTRÉES TAR/PAX

### 8.1 Conformité stricte à l'arbitrage B2-ARB-02 (Option A)
Le jalon B2 applique exclusivement **l'Option A** validée par le propriétaire :
- **Moteur de décompression :** Décompression assurée exclusivement par les bibliothèques officielles de la BCL .NET 10 LTS :
  * `System.IO.Compression.ZipArchive` pour les formats ZIP et ZIP64 ;
  * `System.Formats.Tar.TarReader` pour les conteneurs TAR ;
  * `System.IO.Compression.GZipStream` pour les conteneurs GZ (`.tar.gz`).
- **Préinspection propriétaire séparée :** Les contrôles de cohérence binaire amont (`ZipHeaderReader`) sont physiquement séparés du moteur de décompression BCL et n'allouent aucun payload.

### 8.2 Politique des entrées TAR et allocations amont des en-têtes PAX
1. **Entrées acceptées pour l'extraction unitaire :**
   - `TarEntryType.RegularFile` et `TarEntryType.V7RegularFile` : Fichiers standards réguliers extraits sur disque scratch.
   - `TarEntryType.Directory` : Répertoires intermédiaires créés pour reconstituer l'arborescence confinée.
2. **Entrées spéciales formellement rejetées et ignorées (`DECISION_PROPRIETAIRE`) :**
   - `TarEntryType.SymbolicLink` (liens symboliques) ;
   - `TarEntryType.HardLink` (liens physiques) ;
   - `TarEntryType.CharacterDevice` et `TarEntryType.BlockDevice` (périphériques) ;
   - `TarEntryType.Fifo` (tubes nommés).
   - Toute entrée de type spécial est immédiatement ignorée sans création de descripteur sur le disque scratch, et consignée dans les métadonnées avec le statut `IGNORED_SPECIAL_ENTRY`.
3. **Analyse contradictoire des allocations amont dans `TarReader` (.NET 10) :**  
   L'examen de l'implémentation officielle de `System.Formats.Tar.TarReader` (.NET 10 runtime `10.0.15` / release `v10.0.0`) établit que lors de l'appel à `GetNextEntry()` ou `GetNextEntryAsync()`, le lecteur BCL lit et parse **l'intégralité du bloc d'en-tête étendu PAX en mémoire managée** avant de construire et de renvoyer l'instance de `PaxTarEntry`.  
   *Conséquence technique majeure :* Si une archive TAR hostile contient un bloc d'attributs étendus PAX de plusieurs mégaoctets, **cette allocation mémoire survient à l'intérieur de la BCL avant même que le code de GraphiScope ne reçoive l'objet pour inspecter ou tronquer ses métadonnées**. La troncature opérée par GraphiScope lors de l'émission IPC protège le canal de communication, mais ne peut empêcher cette allocation préalable dans le tas du Worker. Ce risque est formellement classé **`NON_VERIFIE` / `RISQUE_BCL_PAX`**.

---

## 9. VALIDATION STRUCTURELLE ZIP/ZIP64, CALCUL D'INTÉGRITÉ CRC-32, OFFSETS SFX ET LIMITATIONS BCL

### 9.1 Rectification technique sur l'instruction SSE4.2 et CRC-32 ZIP
1. **Incompatibilité polynomiale matérielle :**  
   L'instruction x86/x64 `CRC32` introduite avec le jeu d'instructions SSE4.2 calcule exclusivement le polynôme **CRC-32C (Castagnoli, polynôme `0x1EDC6F41`)**.  
   Or, la spécification PKZIP (APPNOTE.TXT Section 4.4.4) impose de manière immuable le polynôme standard **CRC-32 IEEE 802.3 / ISO 3309 (polynôme `0xEDB88320`)**.  
   Ces deux algorithmes sont mathématiquement incompatibles : une instruction matérielle SSE4.2 `CRC32` ne peut pas produire le checksum normalisé d'une archive ZIP.
2. **Accélération matérielle réelle du CRC-32 IEEE :**  
   L'accélération matérielle du CRC-32 IEEE 802.3 repose sur l'instruction de multiplication sans retenue `PCLMULQDQ` ou sur des algorithmes par tables (*Slice-by-8*).  
   *Précision normative :* **L'instruction `PCLMULQDQ` ne dépend pas directement d'AVX ou d'AVX-512.** Elle a été introduite sur l'architecture Intel Westmere et fonctionne avec les registres XMM 128 bits classiques sans exiger la présence d'AVX (bien que des formes vectorisées 256/512 bits `VPCLMULQDQ` existent sous AVX-512 / AVX10).

### 9.2 Absence de validation automatique du CRC-32 dans `ZipArchiveEntry` (.NET 10)
- Dans la BCL .NET 10 (`System.IO.Compression.ZipArchiveEntry`), la propriété `Crc32` retourne la métadonnée uint32 lue dans l'en-tête central.
- Le flux de décompression renvoyé par `entry.Open()` (`DeflateStream`) **ne calcule ni ne vérifie automatiquement le CRC-32 lors de la lecture**.
- La validation d'intégrité doit impérativement être assurée par l'application au fil de l'eau (composant `SafeArchiveEntryStream`), en accumulant le checksum incrémental jusqu'à `EOF` absolu avant comparaison avec `entry.Crc32`. Le nombre réel d'octets lus doit également être contrôlé par rapport à la taille décompressée attendue.

### 9.3 Comportement interne de `ZipArchive` (.NET 10) : lecture différée et matérialisation
L'inspection du code source officiel de la BCL .NET 10 (`System.IO.Compression.ZipArchive.cs`, runtime `10.0.15` / release tag `v10.0.0`) révèle la séquence interne exacte :
1. **Construction de l'instance :** Le constructeur `ZipArchive(...)` vérifie la validité des paramètres et la capacité de recherche du flux (`CanSeek`), **mais n'exécute pas immédiatement la lecture du répertoire central**.
2. **Lecture différée (*Deferred Reading*) :** La lecture effective du Central Directory est différée jusqu'au premier accès à la propriété `.Entries` ou à la méthode `.GetEntry(name)`. Cet appel déclenche la méthode interne `EnsureCentralDirectoryRead()`.
3. **Matérialisation intégrale en mémoire managée :** Une fois déclenchée, `ReadCentralDirectory()` parcourt séquentiellement tous les en-têtes CDFH et alloue immédiatement un objet `ZipArchiveEntry` pour chaque fichier contenu dans l'archive, instanciant les chaînes de noms et commentaires dans le tas managé.
4. **Limitation mémoire intrinsèque :** Pour une archive contenant un très grand nombre de fichiers (ex. 500 000 entrées), la BCL matérialise 500 000 instances d'objets `ZipArchiveEntry` en RAM. **Cette consommation mémoire est intrinsèque à la BCL et ne peut pas être éliminée par le découpage en lots IPC.**

### 9.4 Balayage EOCD et résolution de la contradiction mathématique sur les offsets SFX
Dans les versions antérieures, une contradiction mathématique imposait l'égalité $O_{cd} + S_{cd} = P_{eocd}$ pour valider un candidat EOCD, ce qui avait pour effet d'éliminer précisément toutes les archives auto-extractibles (SFX) où un stub exécutable de taille $\Delta > 0$ décale l'EOCD !

**Séquence opérationnelle cohérente et vérifiée :**
1. **Balayage arrière de l'EOCD classique :** Recherche de la signature EOCD `0x06054b50` dans les 65 557 derniers octets du flux.
2. **Extraction des grandeurs candidates :** Pour chaque signature candidate découverte à l'offset physique $P_{eocd}$ :
   - Lecture de la taille du Central Directory $S_{cd}$ (4 octets) ;
   - Lecture de l'offset de début du Central Directory $O_{cd}$ (4 octets).
3. **Calcul du décalage candidat $\Delta$ :**
   $$\Delta = P_{eocd} - (O_{cd} + S_{cd})$$
   - Si $\Delta = 0$ : Archive ZIP standard non préfixée.
   - Si $\Delta > 0$ : Candidat d'archive avec préfixe (ex. stub exécutable SFX de taille $\Delta$).
   - Si $\Delta < 0$ : Incohérence structurelle (le répertoire central déborderait au-delà de l'EOCD), candidat immédiatement écarté.
4. **Vérification structurelle à l'offset ajusté :**  
   Pour tout candidat avec $\Delta \ge 0$, le lecteur vérifie les structures physiques réelles :
   - Calcul de l'offset physique ajusté du Central Directory : $O_{cd\_phys} = O_{cd} + \Delta$ ;
   - Contrôle des limites physiques : $O_{cd\_phys} + S_{cd} \le P_{eocd} \le \text{FileLength}$ ;
   - Contrôle de la signature de tête du Central Directory : lecture des 4 octets à $O_{cd\_phys}$ qui doivent correspondre exactement à la signature de Central Directory File Header (`0x02014b50`).
5. **Validation sémantique :** Si la signature à $O_{cd\_phys}$ est valide, le candidat EOCD est confirmé. Dans le cas contraire, le balayage arrière se poursuit.

### 9.5 Prise en charge de la spécification ZIP64 et arithmétique contrôlée
1. **Positionnement distinct de l'EOCD64 Locator et du Record :**  
   Dans une archive ZIP64, l'EOCD Locator ZIP64 se situe **immédiatement avant l'EOCD classique** (à l'offset $P_{eocd} - 20$) et porte la signature `0x07064b50`.  
   Cet indicateur contient l'offset de début de l'EOCD64 Record (`0x06064b50`), qui à son tour contient la taille $S_{cd64}$ et l'offset de début $O_{cd64}$ du Central Directory 64 bits.  
   **Le Central Directory ZIP64 se termine à l'offset de début de l'EOCD64 Record**, et ces positions ne doivent jamais être confondues avec celle de l'EOCD classique.
2. **Arithmétique `checked` vs validation sémantique :**  
   L'exécution des calculs d'offsets sous le mot-clé C# `checked` garantit l'interception des débordements d'entiers (`OverflowException`). Toutefois, **un calcul arithmétique `checked` ne constitue pas à lui seul une validation sémantique**. La validation sémantique impose de vérifier explicitement que les offsets ajustés ne se chevauchent pas anormalement, pointent sur des signatures d'en-têtes valides et demeurent strictement bornés par la taille physique totale du fichier sur disque (`fileStream.Length`).

### 9.6 Chiffrement et maintien du conteneur opaque (Q-007)
Détection binaire des indicateurs de chiffrement (Bits 0, 6, 13 du General Purpose Bit Flag, Méthode de compression 99 WinZip AES `0x0063`, Extra Field `0x9901`).  
Conformément à la décision souveraine `Q-007`, l'archive est cataloguée avec le statut `PREINSPECTION_PROTECTED_OPAQUE` sans sollicitation de mot de passe ni tentative de déchiffrement.

---

## 10. CRÉATION SUSPENDUE, FLUX STANDARDS, ISOLATION OPÉRATIONNELLE, COMMUNICATION IPC ET CYCLE DE VIE DU WORKER

### 10.1 Création des tubes anonymes et désactivation de l'héritage hôte
1. **Création avec héritabilité initiale :** `SECURITY_ATTRIBUTES sa = { sizeof(sa), NULL, TRUE };` pour créer via `CreatePipe` les trois canaux standards (`stdin`, `stdout`, `stderr`).
2. **Désactivation immédiate de l'héritage hôte (`SetHandleInformation`) :**
   - `SetHandleInformation(hStdinWrite, HANDLE_FLAG_INHERIT, 0)` ;
   - `SetHandleInformation(hStdoutRead, HANDLE_FLAG_INHERIT, 0)` ;
   - `SetHandleInformation(hStderrRead, HANDLE_FLAG_INHERIT, 0)`.
   - Seuls les descripteurs clients du Worker (`hStdinRead`, `hStdoutWrite`, `hStderrWrite`) demeurent héritables.

### 10.2 Configuration de `STARTUPINFOEXW` et redirection explicite des flux
1. **Allocation de la liste d'attributs de processus :**
   - Interrogation de taille via `InitializeProcThreadAttributeList(NULL, 1, 0, &cbSize)` ;
   - Enregistrement de la liste explicite : `HANDLE hInheritList[3] = { hStdinRead, hStdoutWrite, hStderrWrite }` via `UpdateProcThreadAttribute(..., PROC_THREAD_ATTRIBUTE_HANDLE_LIST, ...)`.
2. **Association explicite des descripteurs standards (`STARTF_USESTDHANDLES`) :**
   - `siEx.StartupInfo.cb = sizeof(STARTUPINFOEXW)` ;
   - `siEx.StartupInfo.dwFlags = STARTF_USESTDHANDLES | STARTF_USESHOWWINDOW` ;
   - `siEx.StartupInfo.wShowWindow = SW_HIDE` ;
   - `siEx.StartupInfo.hStdInput = hStdinRead` ;
   - `siEx.StartupInfo.hStdOutput = hStdoutWrite` ;
   - `siEx.StartupInfo.hStdError = hStderrWrite`.

### 10.3 Séquence réelle d'assignation au Job Object et description des chemins d'échec
La séquence de création et de configuration sous le noyau Windows NT respecte scrupuleusement l'ordre suivant, avec traitement systématique des chemins d'échec :
```
[Hôte : Préparation]
       |
       |-- 1. CreatePipe (stdin, stdout, stderr) avec héritabilité
       |-- 2. SetHandleInformation sur handles hôtes (retrait INHERIT)
       |-- 3. InitializeProcThreadAttributeList & UpdateProcThreadAttribute
       |-- 4. CreateProcessW(CREATE_SUSPENDED | EXTENDED_STARTUPINFO_PRESENT)
       |      |
       |      +---> ÉCHEC : Log Win32 error, nettoyage handles pipes, ABANDON.
       |
       |-- 5. Fermeture des copies hôtes des handles clients du Worker
       |-- 6. DeleteProcThreadAttributeList
       |
       |-- 7. AssignProcessToJobObject(hJob, pi.hProcess)
       |      |
       |      +---> ÉCHEC : TerminateProcess(pi.hProcess, 1), WaitForSingleObject,
       |                    fermeture pi.hThread, pi.hProcess, ABANDON.
       |
       |-- 8. SetInformationJobObject(hJob, JobObjectExtendedLimitInformation)
       |      |
       |      +---> ÉCHEC : TerminateProcess(pi.hProcess, 1), WaitForSingleObject,
       |                    fermeture pi.hThread, pi.hProcess, ABANDON.
       |
       |-- 9. Démarrage de la tâche de drainage asynchrone borné de stderr
       |
       |-- 10. ResumeThread(pi.hThread)
       |       |
       |       +---> ÉCHEC : TerminateProcess(pi.hProcess, 1), WaitForSingleObject,
       |                     fermeture pi.hThread, pi.hProcess, ABANDON.
       |
       +-- 11. CloseHandle(pi.hThread) (l'hôte conserve pi.hProcess et hJob).
```
1. **Échec de `CreateProcessW` :** Nettoyage immédiat de tous les descripteurs de pipes, libération de la liste d'attributs et signalement d'erreur sans fuite de ressources.
2. **Échec de `AssignProcessToJobObject` :** Si l'assignation échoue, l'hôte invoque immédiatement `TerminateProcess(pi.hProcess, 1)`, attend la terminaison effective avec `WaitForSingleObject(pi.hProcess, 5000)`, ferme `pi.hThread`, `pi.hProcess` et les pipes. **Le thread suspendu n'est jamais repris.**
3. **Échec de `SetInformationJobObject` :** Si la configuration des limites échoue, abattage forcé immédiat du processus suspendu via `TerminateProcess`, attente, fermeture des descripteurs et abandon.
4. **Échec de `ResumeThread` :** En cas d'erreur lors de la reprise du thread, abattage forcé immédiat du processus, attente et nettoyage.
5. **Portée d'`ActiveProcessLimit = 1` (`LIMITATION`) :** Cet indicateur restreint le nombre maximal de processus simultanément actifs dans le Job Object à 1. **Il ne constitue pas une interdiction universelle de créer des processus**, mais garantit que si le Worker tentait de spawner un processus enfant, cet appel échouerait tant que le processus père est actif.

### 10.4 Drainage de `stderr` et garantie conditionnelle
- **Garantie conditionnelle (`LIMITATION`) :** L'affirmation selon laquelle le Worker ne peut « jamais » bloquer sur `stderr` est formellement requalifiée en **garantie conditionnelle au fonctionnement normal de la boucle de lecture asynchrone**. Si le thread de drainage de l'hôte crashe, subit un deadlock ou si la boucle cesse de consommer les octets, le tampon du pipe `stderr` géré par `npfs.sys` finit par se remplir, provoquant le blocage du Worker lors de son prochain appel à `WriteFile(hStderr)`.
- **Mécanismes d'atténuation :**
  * Tâche dédiée de drainage asynchrone permanent en flux d'octets (`Stream.ReadAsync`) avec tampon circulaire borné à 64 Ko ;
  * Décodage UTF-8 incrémental avec gestion des caractères fragmentés ;
  * Troncature automatique des lignes sans saut de ligne excédant 4 096 caractères (`[TRUNCATED]`) ;
  * En cas de blocage ou d'expiration de timeout sur l'IPC principal, l'hôte annule la tâche de drainage, ferme le descripteur de pipe `stderr` et procède à l'abattage ordonné du Worker.

---

## 11. THREAT MODEL DU WORKER, ANALYSE DES PERMISSIONS EFFECTIVES ET FRONTIÈRES DE SÉCURITÉ

### 11.1 Analyse des permissions effectives sous session Windows
Une analyse rigoureuse impose de borner explicitement les frontières de sécurité :
1. **Jeton d'accès utilisateur partagé (`TokenUser`) :**  
   Le processus `GraphiScope.Worker.exe` est instancié dans la même session utilisateur Windows que l'hôte. Par conséquent, **au niveau du système de fichiers NTFS et du registre Windows, le Worker s'exécute avec les mêmes droits en lecture/écriture que l'utilisateur connecté**. Les Job Objects ne restreignent en aucun cas les listes de contrôle d'accès NTFS ni l'accès aux autres fichiers de l'utilisateur.
2. **Ce que le Job Object assure effectivement (`SOURCE_OFFICIELLE`) :**
   - Fermeture ordonnée et abattage garanti du Worker à la clôture de l'hôte (`JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE`) ;
   - Plafond d'engagement de mémoire privée cumulée (`JOB_OBJECT_LIMIT_JOB_MEMORY`) ;
   - Limitation du nombre de processus simultanément actifs dans le Job (`ActiveProcessLimit = 1`) ;
   - Suppression des boîtes de dialogue de crash bloquantes (`JOB_OBJECT_LIMIT_DIE_ON_UNHANDLED_EXCEPTION`).
3. **Ce que le Job Object n'assure PAS (`LIMITATION`) :**
   - **Aucun filtrage des sockets réseau :** Le Job Object ne bloque pas les communications TCP/IP.
   - **Aucune restriction d'accès aux fichiers du profil :** Le Worker peut théoriquement ouvrir tout fichier accessible en lecture par l'utilisateur connecté.
   - **Aucune virtualisation du registre Windows.**

### 11.2 Frontière réseau et risque résiduel post-RCE
Il convient de distinguer trois niveaux de sécurité :
1. *Absence de dépendance réseau dans le code applicatif :* Le projet `GraphiScope.Worker` ne référence aucune bibliothèque réseau (pas de Kestrel, pas de `System.Net.Http`, pas d'utilisation de sockets managés).
2. *Absence d'appel réseau par les composants internes :* Les composants d'inspection binaire et de décompression n'émettent aucun appel réseau.
3. *Interdiction effective des connexions réseau au niveau OS (Sandboxing) :* **NON ASSURÉE.**  
   En cas de compromission sévère du Worker par exécution de code arbitraire natif (post-RCE exploitant une vulnérabilité hypothétique dans un décompresseur natif), le code hostile disposerait des privilèges de la session utilisateur et pourrait invoquer directement l'API WinSock (`ws2_32.dll`) pour ouvrir des connexions sortantes. Une interdiction matérielle exigerait un conteneur Windows AppContainer ou des règles de filtrage de paquets WFP (*Windows Filtering Platform*), non intégrés dans le périmètre B2. Ce risque résiduel est formellement documenté et classé **`NON_VERIFIE`**.

### 11.3 Accès au catalogue SQLite et risque résiduel
- *Au niveau applicatif :* Le Worker ne référence pas `Microsoft.Data.Sqlite`, n'intègre aucun composant d'accès aux données et ne reçoit jamais le chemin du catalogue.
- *Au niveau du système d'exploitation (`LIMITATION`) :* Exécuté sous le même compte utilisateur que l'hôte, le Worker possède techniquement les droits d'accès NTFS au fichier `%LOCALAPPDATA%\GraphiScope\Catalog\graphiscope.db`. La séparation repose sur l'architecture logicielle et l'absence de code d'accès dans le Worker, et non sur un cloisonnement imposé par le système d'exploitation.

---

## 12. INVENTAIRE EXHAUSTIF DES 37 CHEMINS CIBLES PROPOSÉS POUR B2, CORRESPONDANCE AVEC B1 ET BASELINE SHA-256

### 12.1 Qualification de l'intégrité de B1 et baseline SHA-256 de référence
1. **État actuel vérifié de B1 :** Les 37 fichiers applicatifs de B1 existent physiquement dans `M:\dev\GraphiScope`, compilent sans avertissement sous .NET 10 LTS Release et passent 30 tests xUnit sur 30 avec 100 % de succès.
2. **Limites de la preuve historique :** **Aucune empreinte SHA-256 individuelle n'ayant été consignée lors de la décision souveraine `DEC-0005`**, l'absence de base de comparaison cryptographique d'époque empêche formellement de certifier de manière rétroactive l'identité cryptographique exacte des fichiers depuis cette date. L'intégrité opérationnelle de B1 repose sur la cohérence de la chaîne documentaire, les horodatages NTFS et les validations de tests.
3. **Baseline SHA-256 de référence (14 novembre 2026) :** Les empreintes calculées le 14 novembre 2026 constituent la **photographie de référence** pour le suivi d'intégrité futur :

| N° | Chemin relatif exact du fichier B1 | Taille (o) | Date dernière écriture | Empreinte SHA-256 de référence (14/11/2026) |
| :---: | :--- | :---: | :---: | :--- |
| 1 | `GraphiScope.sln` | 2 014 | 2026-11-12 16:13:07 | `37EEFBF6F6027EABCC28973A9E7902E2A47D32ADA1245768A6787F162CF69E24` |
| 2 | `src/GraphiScope.Core/Domain/Enums.cs` | 1 158 | 2026-11-12 20:50:38 | `23F9F08F4116BFBB6E1B0186116F15D3196A6E650304B25027A2AD58058B5391` |
| 3 | `src/GraphiScope.Core/Domain/Exceptions.cs` | 1 222 | 2026-11-12 16:05:39 | `59A219A06CEA9E3E28E3227350F7D3301085BADDB81C934F44BC2A535ABDA1FA` |
| 4 | `src/GraphiScope.Core/Domain/FileObservation.cs` | 909 | 2026-11-12 16:05:39 | `7A5DEAE66F78DA70026D98A3A4F1C7A16659E4EEF39317912BE75E3197AFD4CE` |
| 5 | `src/GraphiScope.Core/Domain/LogicalResource.cs` | 1 128 | 2026-11-12 16:05:40 | `E0A82879EB2B1D38E5FBBF5A8F375010D4D4595BF58356952A2E6EC97AED561D` |
| 6 | `src/GraphiScope.Core/Domain/LogicalResourceVersion.cs` | 870 | 2026-11-12 16:05:40 | `72E5B41130BB82FABEA0094CC33B4E336737198BEAB3301966C8F8FCBDA701C5` |
| 7 | `src/GraphiScope.Core/Domain/MetadataEntry.cs` | 770 | 2026-11-12 16:05:40 | `4AD67131C17E338A99A0161BC5CB4A2FA4AC844D45E857367EF29D49B43E800D` |
| 8 | `src/GraphiScope.Core/Domain/PhysicalFile.cs` | 1 617 | 2026-11-12 16:05:41 | `92BD40C8326451973CC26441877C19593D077C10BEE5F567239C4F2DE85D7AF8` |
| 9 | `src/GraphiScope.Core/Domain/PhysicalRoot.cs` | 963 | 2026-11-12 16:05:41 | `8ACC28DAA8990D490558D1559D4A5DFF7740E4533648356B87E2FA8329550A6A` |
| 10 | `src/GraphiScope.Core/Domain/ResourceIdentity.cs` | 972 | 2026-11-12 16:13:08 | `2F37A870CC0DA7C1B0E64F2C73B2D86375C1FFCB5A763848FC23252A6001F445` |
| 11 | `src/GraphiScope.Core/Domain/ValueObjects.cs` | 2 749 | 2026-11-12 16:05:42 | `9DC48C539657A57CE1A3D5DF7E7AB9961B89163DE241595A1551F07412FF4000` |
| 12 | `src/GraphiScope.Core/GraphiScope.Core.csproj` | 453 | 2026-11-12 20:50:38 | `137BB26B1D7B15004F83C2E770E2924390809551620B7687EEE94EFC68158DD1` |
| 13 | `src/GraphiScope.Core/Persistence/IDbConnectionFactory.cs` | 896 | 2026-11-12 16:05:43 | `01004A9A7E77E7562ABD02872F51CA77E92635C2CF1E91FA3D6829DAFD4BC2BB` |
| 14 | `src/GraphiScope.Core/Persistence/Repositories/FileInventoryRepository.cs` | 13 597 | 2026-11-12 16:05:44 | `15E03FCA472028F44FD36A15304D725AE7694064A15B4F064D44318915BBB102` |
| 15 | `src/GraphiScope.Core/Persistence/Repositories/IFileInventoryRepository.cs` | 1 394 | 2026-11-12 16:05:44 | `55B787B19964E9E877E766B4543981AA88E8EB569E2CB1C468F2938A4DB97DE3` |
| 16 | `src/GraphiScope.Core/Persistence/Repositories/IStorageRootRepository.cs` | 759 | 2026-11-12 16:05:44 | `BE278EBCA4942E7CD40CDE25C5FCCE76B5AC67D321A821E0D9DC4CCA1104FC50` |
| 17 | `src/GraphiScope.Core/Persistence/Repositories/StorageRootRepository.cs` | 5 900 | 2026-11-12 16:05:44 | `14C9759B3B5C4D3206E952256B648226C5FDE49A86A9A40A1655B7FF63D69E28` |
| 18 | `src/GraphiScope.Core/Persistence/SqliteConnectionFactory.cs` | 2 845 | 2026-11-13 08:53:44 | `ABD185B3E71B5DF3195DACD999A897BC3060175D0A67AEF09D2DB6BD402858A0` |
| 19 | `src/GraphiScope.Core/Persistence/SqliteSchemaManager.cs` | 6 708 | 2026-11-12 16:05:45 | `03381A56013D12BCD4E626A693B71BB976C78A776A9FB242930A49EC11825975` |
| 20 | `src/GraphiScope.Core/Security/PathConfinementService.cs` | 8 730 | 2026-11-12 20:58:04 | `FCA15BD9754FBE3621B9B8FC8F984127078748E18B1949BF6F05438A317B98BE` |
| 21 | `src/GraphiScope.Core/Services/FileSystemCrawler.cs` | 3 913 | 2026-11-12 16:13:08 | `93ED4CC9430A523EC60614B72107A1C826563AAABE48C8DC6C0AB687E4AD5AB7` |
| 22 | `src/GraphiScope.Core/Services/HashService.cs` | 1 901 | 2026-11-12 16:05:47 | `E997CBEC38572312EFBB2DAB078D2993414E90F28AB6E3D3133AB6FAE6E4CA46` |
| 23 | `src/GraphiScope.Core/Services/IFileSystemCrawler.cs` | 539 | 2026-11-12 16:13:08 | `CEED12123545A2A3F28AF19BB3FB5946A1F63879516321528F5D5B817872F979` |
| 24 | `src/GraphiScope.Core/Services/IHashService.cs` | 686 | 2026-11-12 16:05:47 | `1E27EFC571FC4A479A20687E0B42275A0554F20FA960FAD1DE2381BFC40EBB6F` |
| 25 | `src/GraphiScope.Core/Services/IInventoryCoordinator.cs` | 979 | 2026-11-12 16:05:47 | `A0EC6CFD2E572737AA2681790EB5EA29C990383CB607AD0F4932444199BE8CC3` |
| 26 | `src/GraphiScope.Core/Services/InventoryCoordinator.cs` | 6 423 | 2026-11-12 20:38:37 | `3C76D096BBFF567C41A8BB636241421FC126479C1F79AD6E9FF2A4268C405619` |
| 27 | `src/GraphiScope.Server/Endpoints/InventoryEndpoints.cs` | 2 345 | 2026-11-12 20:50:39 | `C45769141CE8FE54A5917CED8B6F718ADA71D667F32EF296ED44E747D9D6B990` |
| 28 | `src/GraphiScope.Server/Endpoints/StatusEndpoints.cs` | 920 | 2026-11-12 16:05:48 | `5E861C830E34DD75981628AB0EA2FF95EF899EAAA9FAD1C725304D3F60152D1F` |
| 29 | `src/GraphiScope.Server/GraphiScope.Server.csproj` | 498 | 2026-11-12 16:05:49 | `33D7C4094CC3945671DD09AB68B485FCC91ABB5529BE62C8BA0A11CDC6049418` |
| 30 | `src/GraphiScope.Server/Program.cs` | 2 047 | 2026-11-12 16:05:49 | `C3FAE36365BCA46FADEB0A7C8691F0BC38AF36E87641CDD1C42A917F9AFFE4F8` |
| 31 | `src/GraphiScope.Server/Security/CsrfProtectionMiddleware.cs` | 1 552 | 2026-11-12 16:05:50 | `15DB4AE20ABDA5F5CE2B449E067E9C980A60213770EF992C2CA3978171EFC5B0` |
| 32 | `src/GraphiScope.Server/Security/CsrfTokenProvider.cs` | 1 096 | 2026-11-12 16:05:50 | `60DF1A6BB4D2F2F45202DBF6388F3D6A10FC4C383DB60933F8A4947E3E5620CE` |
| 33 | `src/GraphiScope.Server/Security/LocalHostValidationMiddleware.cs` | 2 134 | 2026-11-12 16:05:51 | `5C81669A47E9C0D2C6F3300767624FFB7AAAE656E9EA49ADC4FECB926AF79205` |
| 34 | `tests/GraphiScope.Tests/DomainTests.cs` | 2 807 | 2026-11-12 16:05:51 | `2BFB285A02A5C69C97D7A6075C242DC31610DAA51D2A6DAAD7B4F053F6C01353` |
| 35 | `tests/GraphiScope.Tests/GraphiScope.Tests.csproj` | 1 128 | 2026-11-12 20:37:29 | `C84E43B9F9A54D13432CF581E9F5B241D3DDDE3594DC61C584A6F49B762DF0A9` |
| 36 | `tests/GraphiScope.Tests/ServerSecurityTests.cs` | 14 453 | 2026-11-13 08:53:45 | `BE332D6BC08D896802961C997A3CBDC8AFE5D58426CCF61598A2478416B95C0B` |
| 37 | `tests/GraphiScope.Tests/SqlitePersistenceTests.cs` | 12 722 | 2026-11-13 08:53:45 | `DE43D2C96BE929988FF65F86421905FF7857A45648A888F7D4099D1D7653F594` |

---

### 12.2 Correspondance exacte entre le manifeste B1 et les 37 chemins cibles proposés pour B2
En application stricte de la section 4.2 du mandat `GS-OPS-GS0006-B2-PREPARATION-CORRECTION-011` :
- Les 37 chemins sont qualifiés comme **chemins cibles proposés pour B2** et non comme composants approuvés.
- Toute affirmation de réutilisation non démontrée est corrigée. En particulier, la ligne n° 3 (`CatalogException.cs`) annonçait à tort un fichier source `src/GraphiScope.Core/Persistence/Exceptions.cs` qui n'a jamais figuré dans le manifeste officiel B1. Le manifeste B1 étant exhaustif, cette cible est classée `NOUVEAU`.
- La classification `ADAPTE` n'est attribuée qu'en présence d'une **preuve de correspondance de contenu** textuelle et conceptuelle, et non sur une simple analogie fonctionnelle.

| N° | Chemin cible proposé pour B2 | Fichier source B1 présumé | Statut normatif | Justification technique et preuve de correspondance |
| :---: | :--- | :--- | :---: | :--- |
| 1 | `src/GraphiScope.Core/Exceptions/GraphiScopeException.cs` | *Aucun* | `NOUVEAU` | Classe de base d'exception proposée pour B2 ; absente de B1 (`Domain/Exceptions.cs` ne contient pas de classe `GraphiScopeException`). |
| 2 | `src/GraphiScope.Core/Exceptions/ConfinementException.cs` | *Aucun* | `NOUVEAU` | Exception spécialisée de violation de confinement créée pour B2 ; absente de B1 (qui levait `InvalidOperationException` ou `NetworkStorageNotSupportedException`). |
| 3 | `src/GraphiScope.Core/Exceptions/CatalogException.cs` | *Aucun* | `NOUVEAU` | Exception spécialisée proposée pour B2 ; le fichier source `Persistence/Exceptions.cs` annoncé dans les versions antérieures ne figure pas dans le manifeste officiel B1. |
| 4 | `src/GraphiScope.Core/Models/CatalogItem.cs` | *Aucun* | `NOUVEAU` | Modèle DTO unifié d'élément d'inventaire proposé pour B2 (B1 utilisait `Domain/PhysicalFile.cs`). |
| 5 | `src/GraphiScope.Core/Models/ScanContext.cs` | *Aucun* | `NOUVEAU` | Structure de contexte d'analyse unitaire et d'annulation créée pour le Worker B2. |
| 6 | `src/GraphiScope.Core/Models/Enums/ConfinementViolationType.cs`| *Aucun* | `NOUVEAU` | Énumération spécialisée des types de violation de confinement créée pour B2 ; absente de `Domain/Enums.cs`. |
| 7 | `src/GraphiScope.Core/Services/IPathConfinementService.cs` | *Aucun* | `NOUVEAU` | Interface contractuelle de confinement créée pour B2 (B1 instanciait directement la classe concrète). |
| 8 | `src/GraphiScope.Core/Services/PathConfinementService.cs` | `src/GraphiScope.Core/Security/PathConfinementService.cs` | `ADAPTE` | Relocalisation du service de confinement depuis `Security/` vers `Services/`, implémentation de `IPathConfinementService` et réutilisation démontrée du code Win32 existant. |
| 9 | `src/GraphiScope.Core/Worker/IWorkerProtocol.cs` | *Aucun* | `NOUVEAU` | Contrat du protocole IPC de trames binaires créé pour B2. |
| 10 | `src/GraphiScope.Core/Worker/WorkerMessage.cs` | *Aucun* | `NOUVEAU` | Structure des messages de commande et réponse IPC créée pour B2. |
| 11 | `src/GraphiScope.Core/Worker/ArchiveInspectionResult.cs` | *Aucun* | `NOUVEAU` | DTO de résultat d'inspection d'archive créé pour B2. |
| 12 | `src/GraphiScope.Core/Worker/ArchiveEntryMetadata.cs` | *Aucun* | `NOUVEAU` | Structure de métadonnées d'une entrée d'archive créée pour B2. |
| 13 | `src/GraphiScope.Core/Worker/ArchiveFormat.cs` | *Aucun* | `NOUVEAU` | Énumération des formats d'archive supportés créée pour B2. |
| 14 | `src/GraphiScope.Core/Worker/WorkerConfiguration.cs` | *Aucun* | `NOUVEAU` | Modèle de configuration et de dimensionnement adaptatif des limites créé pour B2. |
| 15 | `src/GraphiScope.Core/Worker/IWorkerClient.cs` | *Aucun* | `NOUVEAU` | Interface client de gestion du satellite créée pour l'hôte en B2. |
| 16 | `src/GraphiScope.Core/Worker/JobObjectManager.cs` | *Aucun* | `NOUVEAU` | Encapsulation P/Invoke des Job Objects Win32 créée pour B2. |
| 17 | `src/GraphiScope.Core/Worker/WorkerClient.cs` | *Aucun* | `NOUVEAU` | Client orchestrateur du processus satellite et du drainage IPC créé pour B2. |
| 18 | `src/GraphiScope.Core/Worker/ArchiveExtractor.cs` | *Aucun* | `NOUVEAU` | Moteur d'extraction en streaming BCL et écriture scratch confinée créé pour B2. |
| 19 | `src/GraphiScope.Core/Worker/ZipHeaderReader.cs` | *Aucun* | `NOUVEAU` | Préinspecteur binaire PKZIP/ZIP64 et détection du chiffrement créé pour B2. |
| 20 | `src/GraphiScope.Core/Worker/SafeArchiveEntryStream.cs` | *Aucun* | `NOUVEAU` | Flux d'écriture régulée et de calcul CRC-32 en streaming créé pour B2. |
| 21 | `src/GraphiScope.Core/Worker/WorkerProcessEntryPoint.cs` | *Aucun* | `NOUVEAU` | Point d'entrée exécutable autonome (`Main`) du Worker créé pour B2. |
| 22 | `src/GraphiScope.Core/Worker/NativeMethods.cs` | *Aucun* | `NOUVEAU` | Déclarations P/Invoke Win32/NT (`NtCreateFile`, `CreateProcessW`, Job APIs) créées pour B2. |
| 23 | `src/GraphiScope.Data/CatalogDbContext.cs` | *Aucun* | `NOUVEAU` | Proposition technique d'agent pour le contexte EF Core SQLite (non validée souverainement, alternative sans EF Core documentée). |
| 24 | `src/GraphiScope.Data/Repositories/CatalogRepository.cs` | *Aucun* | `NOUVEAU` | Proposition de dépôt sous EF Core ; aucune correspondance de contenu démontrée avec le dépôt ADO.NET `FileInventoryRepository.cs` de B1. |
| 25 | `src/GraphiScope.Data/Services/CatalogPurgeService.cs` | *Aucun* | `NOUVEAU` | Service de purge et de nettoyage des tables d'extraction proposé pour B2 (inexistant en B1). |
| 26 | `src/GraphiScope.Data/Services/ICatalogPurgeService.cs` | *Aucun* | `NOUVEAU` | Interface contractuelle de purge créée pour B2 (inexistante en B1). |
| 27 | `src/GraphiScope.Data/Entities/CatalogItemEntity.cs` | *Aucun* | `NOUVEAU` | Proposition d'entité relationnelle EF Core ; aucune correspondance textuelle directe avec le domaine DDD `LogicalResource.cs` de B1. |
| 28 | `src/GraphiScope.Data/Entities/ExtractionMetadataEntity.cs` | *Aucun* | `NOUVEAU` | Entité de stockage des métadonnées d'archives (formats, CRC, chiffrement) créée pour B2. |
| 29 | `src/GraphiScope.Data/Migrations/..._AddExtractionMetadata.cs`| *Aucun* | `NOUVEAU` | Gabarit de migration de schéma EF Core proposé pour B2. |
| 30 | `src/GraphiScope.Host/Program.cs` | `src/GraphiScope.Server/Program.cs` | `ADAPTE` | Adaptation du point d'entrée serveur Kestrel B1 pour enregistrer les services hôtes du Worker B2. |
| 31 | `src/GraphiScope.Host/appsettings.json` | *Aucun* | `NOUVEAU` | Fichier de configuration locale de l'hôte créé pour B2 (inexistant dans les 37 fichiers B1). |
| 32 | `src/GraphiScope.Host/Services/HostScanningCoordinator.cs` | `src/GraphiScope.Core/Services/InventoryCoordinator.cs` | `ADAPTE` | Adaptation de l'orchestration d'inventaire de B1 pour articuler l'analyse d'archives unitaires via WorkerClient. |
| 33 | `src/GraphiScope.Core/Worker/AncestorStack.cs` | *Aucun* | `NOUVEAU` | Structure LIFO immuable de détection des auto-extractions unitaires créée pour B2. |
| 34 | `tests/GraphiScope.Core.Tests/PathConfinementServiceTests.cs` | *Aucun* | `NOUVEAU` | Suite de tests dédiée de confinement de chemins créée pour B2 ; absente de `ServerSecurityTests.cs` (qui valide Kestrel/CSRF). |
| 35 | `tests/GraphiScope.Core.Tests/Worker/JobObjectManagerTests.cs` | *Aucun* | `NOUVEAU` | Suite de tests automatisés de l'assignation et des limites Job Objects créée pour B2. |
| 36 | `tests/GraphiScope.Core.Tests/Worker/ArchiveExtractorTests.cs` | *Aucun* | `NOUVEAU` | Suite de tests automatisés de décompression et sécurité scratch créée pour B2. |
| 37 | `tests/GraphiScope.Core.Tests/Worker/ZipHeaderReaderTests.cs` | *Aucun* | `NOUVEAU` | Suite de tests automatisés de préinspection PKZIP, SFX et chiffrement créée pour B2. |

### 12.3 Bilan chiffré consolidé
- **`REUTILISE_IDENTIQUE` :** **0 chemin (0,0 %)** — Aucun fichier B1 n'est repris tel quel sans modification de chemin ou de contenu.
- **`ADAPTE` :** **3 chemins (8,1 %)** — Fichiers B1 sources formellement prouvés (`PathConfinementService.cs`, `Program.cs`, `HostScanningCoordinator.cs`).
- **`NOUVEAU` :** **34 chemins (91,9 %)** — Composants nouveaux spécifiques aux exigences de B2.
- **`NON_DEMONTRE` :** **0 chemin (0,0 %)** — Chaque chemin fait l'objet d'une qualification transparente.
- **Total des chemins cibles proposés pour B2 :** **Exactement 37 chemins (100,0 %)**.

---

## 13. ANALYSE DES DÉPENDANCES, STATUT D'ENTITY FRAMEWORK CORE ET RÈGLE FINANCIÈRE DE 0 €

### 13.1 Statut exact d'Entity Framework Core : Proposition technique d'agent vs Choix propriétaire
1. **B1 repose sur `Microsoft.Data.Sqlite` pur :** Le jalon B1 accepté sous `DEC-0005` utilise exclusivement le pilote direct ADO.NET `Microsoft.Data.Sqlite` (10.0.15) encapsulant SQLite 3.53.3, sans aucun ORM.
2. **EF Core constitue une simple proposition technique :** L'introduction du projet `GraphiScope.Data`, de `CatalogDbContext` et des migrations dans les chemins proposés 23 à 29 constitue une **proposition d'ingénierie soumise par l'agent**, et **en aucun cas une décision souveraine du propriétaire ni une orientation déjà acquise**.
3. **Alternative native sans EF Core :**  
   Pour préserver la continuité architecturale stricte de B1, GraphiScope peut poursuivre intégralement en `Microsoft.Data.Sqlite` pur (création de la table `ExtractionMetadata` par script DDL dans `SqliteSchemaManager.cs`), éliminant l'ajout de dépendances d'ORM.  
   **La décision finale d'adopter EF Core ou de maintenir SQLite pur relève souverainement du propriétaire lors d'un éventuel mandat d'implémentation.**

### 13.2 Règle financière transversale de 0 € (`DEC-0004`)
1. **Exigence souveraine :** Gratuité financière absolue et obligatoire (coût total de 0 €) applicable à :
   - L'ensemble des bibliothèques et frameworks (dépendances directes et transitives) ;
   - Les binaires natifs redistribués ;
   - Les SDK, compilateurs et outils d'exécution ;
   - La redistribution et l'utilisation sans frais par poste ou par volume ;
   - L'absence de tout abonnement ou service externe payant indispensable.
2. **Vérification requise à l'implémentation :** Une simple liste de licences ne suffit pas à prouver l'absence de coût caché. Un audit formel de la chaîne transitive (`dotnet list package`) et l'examen des clauses de redistribution devront être réalisés au stade de l'implémentation, sans prétendre que cette vérification est d'ores et déjà acquise.

---

## 14. COMPATIBILITÉ DOCUMENTAIRE WINDOWS 10/11 X64 ET SYSTÈMES DE FICHIERS

### 14.1 Versions du système d'exploitation ciblées
- **Matrice de validation prévue (non encore exécutée sur cible) :**
  * Windows 10 x64 : Versions 21H2, 22H2 (Builds >= 19044) ;
  * Windows 11 x64 : Versions 21H2, 22H2, 23H2, 24H2 ;
  * Architectures cibles : Exclusivement x64 (64 bits).

### 14.2 Matrice technique de compatibilité des systèmes de fichiers
*Avertissement méthodologique :* La compatibilité ne peut se déduire du seul indicateur `FILE_PERSISTENT_ACLS`. Le tableau ci-dessous distingue les fonctionnalités supportées par spécification de celles devant faire l'objet d'essais physiques :

| Fonctionnalité système requise | NTFS Local | ReFS Local | FAT32 / exFAT | Partage SMB distant |
| :--- | :---: | :---: | :---: | :---: |
| **`NtCreateFile` avec `RootDirectory`** | Supporté | Supporté | Supporté | Variable selon serveur SMB |
| **`FILE_OPEN_REPARSE_POINT` (0x00200000)** | Supporté | Supporté | Non supporté | Variable |
| **Verrouillage sans `FILE_SHARE_DELETE`** | Garanti noyau | Garanti noyau | Partiel | Non garanti (Oplocks/Leasing) |
| **`FileDispositionInfoEx` POSIX Delete** | Supporté | Supporté | Non supporté | Variable |
| **Identité physique `FileIdInfo` (128 bits)** | Supporté | Supporté | Non supporté | Non supporté universellement |
| **DACL sécurisée `SE_DACL_PROTECTED`** | Supporté | Supporté | Non supporté | Non supporté |
| **Éligibilité au répertoire scratch B2** | **CIBLÉ (À VALIDER)**| **CIBLÉ (À VALIDER)**| **REFUSÉ** | **REFUSÉ (NON_VERIFIE)** |

> [!IMPORTANT]
> **RÈGLE ARCHITECTURALE DU RÉPERTOIRE SCRATCH :**  
> Le répertoire scratch de décompression doit résider **exclusivement sur un volume local formaté en NTFS ou ReFS**. Si le volume ne supporte pas les ACL persistantes ou s'il s'agit d'un lecteur réseau distant, l'initialisation du scratch est immédiatement refusée avec l'erreur `UNSUPPORTED_STORAGE_CONFIGURATION`.

---

## 15. REGISTRE QUALITATIF DES RISQUES TECHNIQUES RÉELS ET RÉÉVALUATION DES NIVEAUX RÉSIDUELS
Conformément aux exigences de rigueur, les niveaux de risque résiduel sont réévalués objectivement, sans prétention de neutralisation absolue :

| Risque technique | Gravité brute | Probabilité | Niveau résiduel réévalué | Mécanisme de prévention | Mécanisme de détection | Conduite en cas d'échec / Limite connue |
| :--- | :---: | :---: | :---: | :--- | :--- | :--- |
| **Échappement de scratch par point de réanalyse (TOCTOU)** | Critique | Faible | **Moyen** | `FILE_OPEN_REPARSE_POINT` sur chaque segment, omission de `FILE_SHARE_DELETE`. | Contrôle `FILE_ATTRIBUTE_REPARSE_POINT` et comparaison canonique `GetFinalPathNameByHandleW`. | Interruption immédiate, suppression par handle, `ConfinementException`. Protection locale NTFS, pas un bac à sable matériel. |
| **Saturation mémoire Worker par allocations amont BCL (ZIP/PAX)** | Élevée | Moyenne | **Moyen** | Plafond mémoire Job Object (`JobMemoryLimit`). | Refus d'allocation NT 1455 / crash géré du Worker. | La BCL matérialise le CD ZIP et lit les en-têtes PAX en amont. Le Worker avorte sans affecter l'hôte. L'hôte détecte la rupture et clôture le Job. |
| **Saturation d'espace disque par décompression massive** | Élevée | Moyenne | **Moyen** | Surveillance `lpFreeBytesAvailableToCaller` et streaming régulé. | Capture Win32 `ERROR_DISK_FULL` (0x70) et `ERROR_HANDLE_DISK_FULL` (0x27). | Arrêt immédiat de l'écriture, purge complète des fichiers temporaires via `FileDispositionInfoEx`. Risque de saturation concurrente par processus tiers. |
| **Blocage du Worker par tube stderr saturé** | Moyenne | Faible | **Faible** | Tâche dédiée de drainage asynchrone permanent en arrière-plan. | Tampon circulaire borné à 64 Ko et troncature systématique des lignes à 4 096 caractères. | Garantie conditionnelle : si le lecteur asynchrone crashe ou freeze, le Worker peut bloquer. L'hôte détecte l'inactivité par timeout et abat le Job. |
| **Désynchronisation ou trame IPC malformée / volumineuse** | Moyenne | Faible | **Faible** | Validation préalable $0 < L \le 4\text{ Mo}$, Magic `0x47, 0x53`, découpage en lots $B_{payload}$, troncature sécurisée. | Contrôle du Magic, contrôle de longueur avant allocation, validation JSON UTF-8 stricte. | Rejet immédiat sans allocation si $L > 4\text{ Mo}$, déconnexion, abattage du Worker via le Job Object. |
| **Exécution de code arbitraire natif post-RCE dans le Worker** | Critique | Très faible | **Élevé** | Processus satellite isolé, aucune dépendance SQLite, aucune bibliothèque réseau, Job Object. | Surveillance du processus hôte et détection des pannes de tubes. | **`NON_VERIFIE` :** Pas de pare-feu WFP ni d'AppContainer. Un attaquant natif pourrait ouvrir des sockets TCP ou accéder aux fichiers utilisateur accessibles. |

---

## 16. MATRICE DES PREUVES, SOURCES OFFICIELLES NORMATIVES ET INVENTAIRE DES POINTS `NON_VERIFIE`

### 16.1 Répertoire normatif et documentaire
Chaque affirmation technique fondamentale est rattachée à sa source officielle vérifiable issue de versions stables figées :
1. **`NtCreateFile`, `RootDirectory` et `FILE_OPEN_REPARSE_POINT` :**
   - *Organisme :* Microsoft Corporation (WDK & Win32 API).
   - *Documentation officielle :* Microsoft Learn, *NtCreateFile function (winternl.h)* & *ZwCreateFile routine (wdm.h / ntifs.h)*. Indicateur `FILE_OPEN_REPARSE_POINT = 0x00200000`.
   - *Conclusion exacte (`SOURCE_OFFICIELLE`) :* Confinement de la résolution dans l'espace du répertoire parent ; ouverture du point de réanalyse sans traversée automatique.
2. **Partage des handles, omission de `FILE_SHARE_DELETE` et sémantique POSIX :**
   - *Organisme :* Microsoft Corporation.
   - *Documentation officielle :* Microsoft Learn, *CreateFileW function (fileapi.h)* & *FILE_DISPOSITION_INFO_EX structure (winbase.h)*.
   - *Conclusion exacte (`SOURCE_OFFICIELLE`) :* Rejet de toute ouverture concurrente avec droit `DELETE` sous NTFS local ; suppression immédiate du namespace via `FILE_DISPOSITION_FLAG_POSIX_SEMANTICS`.
3. **Plafonnement de commit privé par Windows Job Objects :**
   - *Organisme :* Microsoft Corporation.
   - *Documentation officielle :* Microsoft Learn, *JOBOBJECT_EXTENDED_LIMIT_INFORMATION structure (winnt.h)*, membre `JobMemoryLimit`, drapeau `JOB_OBJECT_LIMIT_JOB_MEMORY (0x00000200)`.
   - *Conclusion exacte (`SOURCE_OFFICIELLE`) :* Bride l'engagement privé virtuel cumulé (*Private Commit*) et non la mémoire résidente physique.
4. **Héritage restreint des descripteurs (`PROC_THREAD_ATTRIBUTE_HANDLE_LIST`) :**
   - *Organisme :* Microsoft Corporation.
   - *Documentation officielle :* Microsoft Learn, *UpdateProcThreadAttribute function (processthreadsapi.h)*, attribut `0x00020002`.
   - *Conclusion exacte (`SOURCE_OFFICIELLE`) :* Limite l'héritage strictement à l'array des 3 handles client du Worker, évitant toute fuite de descripteur.
5. **Comportement mémoire interne de `ZipArchive` (.NET 10 stable v10.0.0) :**
   - *Organisme :* .NET Foundation / Microsoft Corporation.
   - *Code source officiel (.NET 10 release v10.0.0) :* Référentiel GitHub `dotnet/runtime`, release tag `v10.0.0`, `src/libraries/System.IO.Compression/src/System/IO/Compression/ZipArchive.cs`.
   - *Conclusion exacte (`SOURCE_OFFICIELLE`) :* Lecture différée du Central Directory déclenchée au premier accès à `.Entries`, matérialisant tous les $N$ objets en mémoire managée.
6. **Absence de validation automatique du CRC-32 dans `ZipArchiveEntry` :**
   - *Organisme :* .NET Foundation / Microsoft Corporation.
   - *Documentation officielle :* Microsoft Learn, *ZipArchiveEntry.Crc32 Property*.
   - *Conclusion exacte (`SOURCE_OFFICIELLE`) :* La propriété retourne la métadonnée brute ; la vérification doit être assurée en streaming jusqu'à `EOF` par l'application.
7. **Incompatibilité polynomiale SSE4.2 `CRC32` vs PKZIP IEEE 802.3 :**
   - *Organisme :* Intel Corporation & PKWARE Inc.
   - *Sources :* Intel SDM Vol. 2A (Castagnoli `0x1EDC6F41`) & PKWARE APPNOTE.TXT Section 4.4.4 (IEEE 802.3 `0xEDB88320`).
   - *Conclusion exacte (`SOURCE_OFFICIELLE`) :* Incompatibilité mathématique ; PCLMULQDQ est l'instruction appropriée pour l'accélération matérielle IEEE et ne dépend pas d'AVX/AVX-512.
8. **Spécification normative PKZIP (ZIP64, Data Descriptor, Chiffrement) :**
   - *Organisme :* PKWARE Inc.
   - *Spécification officielle :* *.ZIP File Format Specification (APPNOTE.TXT)*, Version 6.3.10.

### 16.2 Inventaire exhaustif des éléments classés `NON_VERIFIE`
| Sujet technique | Justification formelle de la qualification `NON_VERIFIE` | Impact architectural sur le jalon B2 |
| :--- | :--- | :--- |
| **Comportement des verrous NT sur partages réseau SMB** | La sémantique de verrouillage `FILE_SHARE_DELETE` et la suppression POSIX dépendent du serveur distant (Samba, NAS) et ne sont garanties que par `NTFS.sys` / `ReFS.sys` en local. | **Le répertoire scratch est strictement interdit sur volume SMB.** Scratch local obligatoire. |
| **Cloisonnement réseau post-RCE du Worker** | Les Job Objects ne bloquent pas les sockets TCP. Sans pare-feu Windows WFP ni AppContainer, un attaquant natif pourrait émettre des paquets réseau. | **Accepté comme compromis d'architecture de défense en profondeur.** Durcissement différé. |
| **Performances réelles du calcul CRC-32 sur machine cible** | En l'absence de banc de mesure instrumenté sur cible représentative sous charge d'archives volumineuses, les débits exacts demeurent non mesurés. | **Validation instrumentée requise lors des bancs d'essais sur cible B2.** |
| **Comportement sur volumes FAT32 / exFAT** | FAT32 ne supportant ni les ACL, ni la sémantique POSIX, ni `FileIdInfo`, le comportement exact n'a pas été qualifié. | **Initialisation du scratch formellement refusée sur FAT32/exFAT.** |
| **Comportement mémoire face aux en-têtes PAX TAR géants** | `TarReader` matérialise l'intégralité du bloc PAX en mémoire avant de renvoyer l'objet, exposant le Worker à une allocation excessive amont. | **Risque BCL identifié ; prise en charge d'archives hostiles à tester sur cible.** |

---

## 17. STRATÉGIE DE VALIDATION, TESTS UNITAIRES ET BANCS D'ESSAIS SUR CIBLE
*Avertissement :* Les essais ci-dessous constituent la **stratégie de validation prévue pour l'implémentation future** et ne doivent pas être confondus avec des preuves d'ores et déjà acquises au stade préparatoire.
1. **Tests unitaires de composants cœur (xUnit) :**
   - `JobObjectManagerTests` : Validation de l'assignation du Job, application des limites, vérification de l'abattage forcé sur clôture.
   - `ZipHeaderReaderTests` : Parsing de jeux d'archives de test (archives valides, archives ZIP64 volumineuses, archives corrompues, archives tronquées, archives auto-extractibles SFX avec décalage $\Delta$, archives chiffrées WinZip AES).
   - `ArchiveExtractorTests` : Vérification de l'extraction par flux, calcul et comparaison du CRC-32, gestion des erreurs d'E/S.
2. **Tests d'intrusion et de sécurité du confinement (Tests hostiles) :**
   - Jeu d'archives malveillantes contenant des traversées de répertoires (`../../evil.txt`), des chemins absolus (`C:\Windows\win.ini`), des noms réservés (`CON.txt`, `NUL`), et des flux alternatifs NTFS (`file.txt:ads`).
   - Injection de points de réanalyse : tentative de décompression dans un dossier contenant une jonction vers un dossier système -> détection et rejet immédiat par `FILE_OPEN_REPARSE_POINT`.
   - Simulation de tentative de suppression concurrente de dossier parent pendant l'écriture -> vérification du blocage via l'omission de `FILE_SHARE_DELETE`.
3. **Tests de résistance aux bombes de décompression :**
   - Archive récursive à expansion massive -> vérification de l'arrêt ordonné dès dépassement de l'expansion ou saturation de l'espace disque, sans crash de l'OS hôte.
4. **Bancs de qualification instrumentés sur machine cible physique :**
   - Mesure réelle des empreintes mémoire privées et Working Set CoreCLR via les compteurs de performance Windows ;
   - Validation du débit d'extraction sur disques SSD NVMe et disques durs magnétiques ;
   - Validation de la libération effective de l'espace disque après clôture des handles (`FileDispositionInfoEx`).

---

## 18. DÉCISIONS SOUVERAINES ACQUISES ET GOUVERNANCE
- **`DEC-0001` :** Initialisation du registre opérationnel local persistant.
- **`DEC-0002` (et Q-001 à Q-008) :** Confirmation pleine et entière des huit arbitrages souverains individuels Q-001 à Q-008. Le dossier documentaire global GS-0002 demeure au statut `REVIEW`.
- **`DEC-0003` :** Acceptation formelle du livrable préparatoire général GS-0003 V1.6.0. **DEC-0003 ne constitue en aucun cas une approbation de GS-0006 ni une autorisation d'implémenter B2.**
- **`DEC-0004` :** Règle financière transversale absolue de 0 € et gratuité totale de GraphiScope.
- **`DEC-0005` :** Acceptation formelle pleine et entière du jalon B1 (statut `DONE` strictement et définitivement acquis au titre de B1). **DEC-0005 ne constitue en aucun cas une autorisation d'implémenter B2.**
- **`Q-004` :** Emplacement de la base de données SQLite strictement confiné au profil local de l'utilisateur (`%LOCALAPPDATA%`).
- **`Q-006` :** Aucune limite fonctionnelle arbitraire ; régulation par surveillance dynamique des ressources système réelles.
- **`Q-007` :** Conservation des archives chiffrées comme conteneurs opaques intacts sans demande de mot de passe ni déchiffrement.
- **`B2-ARB-01 Option A` :** Communication IPC Worker exclusive via `stdin`/`stdout` redirigés et trames binaires délimitées en longueur.
- **`B2-ARB-02 Option A` :** Décompression des archives (ZIP, ZIP64, TAR, GZ) exclusivement assurée par la BCL officielle de .NET 10 LTS.

---

## 19. CRITÈRES OBJECTIFS D'ACCEPTATION DU JALON B2
Le jalon B2 sera éligible à l'acceptation formelle par le propriétaire si et seulement si l'implémentation ultérieure remplit l'ensemble des critères objectifs suivants :
1. Les **37 chemins cibles proposés** sont créés ou adaptés conformément à l'architecture cible validée, sans aucune altération de l'existant B1.
2. La solution `GraphiScope.sln` compile sous .NET 10 LTS x64 Release avec strictement **0 avertissement et 0 erreur**.
3. La suite complète de tests automatisés (unitaires, intégration et sécurité hostile) est exécutée avec **100 % de succès**.
4. La règle financière absolue de 0 € est scrupuleusement respectée (0 dépendance commerciale payante au rapport d'audit complet).
5. Les mécanismes de protection du scratch face aux attaques par traversée et points de réanalyse sont démontrés opérationnels sous un **modèle de menace explicitement borné et testable**.
6. La régulation adaptative de la mémoire Job Object et de l'espace disque est démontrée opérationnelle sans quota fixe arbitraire.
7. L'absence d'accès direct du Worker à SQLite et aux bibliothèques réseau est confirmée par revue de code et analyse de dépendances, avec documentation des limites de l'isolation OS.
8. Les archives chiffrées sont cataloguées de manière opaque sans invite utilisateur, conformément à `Q-007`.

---

## 20. CONCLUSION FORMELLE ET RECOMMANDATION DE GOUVERNANCE (CATÉGORIE B)
En application stricte des principes directeurs du mandat `GS-OPS-GS0006-B2-PREPARATION-CORRECTION-011`, le dossier préparatoire B2 s'interdit toute affirmation d'étanchéité absolue ou de validation anticipée dès lors que des éléments demeurent formellement classés `NON_VERIFIE`.

**Classification formelle du dossier de cadrage consolidé V2.0.0 :**
# CATÉGORIE B — PROPOSITION TECHNIQUE CONFORME SOUS RÉSERVES QUALIFIÉES

### Synthèse des réserves résiduelles qualifiées :
1. *Réserve d'isolation réseau post-RCE :* Le Job Object n'assurant pas de filtrage réseau TCP, l'étanchéité en cas de compromission native dépendra d'une étude ultérieure de conteneur AppContainer ou de pare-feu WFP.
2. *Réserve des allocations amont BCL :* La BCL .NET 10 matérialise le Central Directory ZIP dès le premier accès et lit l'intégralité des en-têtes PAX dans `TarReader` avant intervention applicative.
3. *Réserve de stockage distant :* Les verrous NTFS et la suppression POSIX n'étant pas universellement garantis sur partages SMB, le scratch est strictement restreint aux volumes locaux NTFS/ReFS.
4. *Réserve de baseline cryptographique B1 :* L'ancrage SHA-256 est établi au 14 novembre 2026 sans prétention de preuve cryptographique rétroactive à DEC-0005.
5. *Réserve d'interception d'allocation Job Object :* Le dépassement du commit limite n'est pas garanti de se traduire de façon systématique par une `OutOfMemoryException` catchable sans risque de terminaison abrupte par le noyau.
6. *Réserve de drainage stderr :* Le non-blocage de `stderr` est conditionné au fonctionnement continu et non perturbé du lecteur asynchrone de l'hôte.

> [!CAUTION]
> Conformément au mandat `GS-OPS-GS0006-B2-PREPARATION-CORRECTION-011`, la présente conclusion constitue une recommandation d'ingénierie et ne vaut en aucun cas acceptation formelle par le propriétaire, ni autorisation d'implémentation logicielle, ni autorisation d'ouverture des jalons B2 ou B3. Le statut de GS-0006 demeure impérativement `REVIEW`.

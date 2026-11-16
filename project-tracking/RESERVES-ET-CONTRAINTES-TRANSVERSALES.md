# GraphiScope — Réserves, contraintes et précautions transversales permanentes

**Référence du document :** `GS-REF-TRANSVERSAL-001`  
**Mandat de création :** Mandat de clôture documentaire de GS-0002 (`GS-OPS-GS0002-CLOSURE-001`)  
**Date d'établissement :** 10 novembre 2026  
**Autorité :** Décision du propriétaire du projet GraphiScope  
**Statut :** RÉFÉRENTIEL PERMANENT EN VIGUEUR — OPPOSABLE À TOUTE TÂCHE ET TOUT AGENT  
**Fichier :** `project-tracking/RESERVES-ET-CONTRAINTES-TRANSVERSALES.md`  

---

## 1. Objet, portée et statut du document

1. **Objet :** Le présent document constitue le support permanent recensant de manière consolidée l'ensemble des décisions souveraines acquises, des réserves techniques transversales, des contraintes d'ingénierie et des règles de gouvernance applicables au projet GraphiScope.
2. **Portée transversale et opposabilité :** Ce document s'impose impérativement à toute intervention, à tout agent (humain ou automatisé), et à toutes les phases du projet, y compris la Phase A documentaire, les phases futures de cadrage, et toute phase ultérieure qui viendrait à être formellement autorisée par le propriétaire.
3. **Articulation avec les autres documents :** Le présent référentiel ne remplace ni les spécifications fonctionnelles maîtresses de GraphiScope, ni les fiches détaillées de la matrice de décisions techniques et fonctionnelles issue de `GS-0002` ([GS-0002-DECISION-MATRIX.md](file:///m:/dev/GraphiScope/project-tracking/GS-0002-DECISION-MATRIX.md), version validée V1.11.1). Il sert de cadre transversal contraignant pour garantir la non-régression documentaire, technique et architecturale du projet.

---

## 2. Décisions propriétaires souveraines acquises (Q-001 à Q-008)

Les huit décisions suivantes ont été formellement arbitrées et validées par le propriétaire du projet le 10 novembre 2026. Elles constituent des choix d'orientation fermes qui ne peuvent être ni réinterprétés, ni déformés, ni rouverts sans un arbitrage propriétaire explicite :

- **Q-001 — Formules Excel :** Extraction et affichage textuel de la formule présente dans la cellule uniquement (`SUM` constituant le nom de fonction interne invariant sous la norme SpreadsheetML/OOXML). Aucune valeur mise en cache n'est présentée comme résultat exploitable ou validé. Aucun recalcul des formules n'est effectué, ni directement par GraphiScope, ni par un moteur externe de calcul.
- **Q-002 — Anciens formats binaires Office (DOC, XLS, PPT) :** Prise en charge partielle et qualifiée. Reconnaissance des formats, extraction des métadonnées et des contenus textuels directement accessibles, qualification rigoureuse format par format, et signalement explicite des éléments non interprétés. Aucune promesse de complétude sans test formel ; aucune bibliothèque imposée comme choix figé.
- **Q-003 — Relations heuristiques dans le graphe :** Les relations inférées par proximité, nommage ou ressemblance (niveaux D2/D3/D4) sont présentées exclusivement comme des propositions candidates à confirmer, limitées en nombre et classées selon des critères explicites et traçables. Aucune confirmation, suppression ou fusion automatique n'est autorisée : la confirmation ou le rejet final relève souverainement de l'utilisateur.
- **Q-004 — Emplacement du catalogue de données :** Base de données locale implantée dans le profil Windows de l'utilisateur (`%LOCALAPPDATA%\GraphiScope\Catalog\`), garantissant l'isolation des données utilisateur et l'indépendance vis-à-vis du répertoire d'installation. L'utilisation de SQLite (avec extension FTS5) constitue une orientation technique privilégiée et non une obligation technologique définitive.
- **Q-005 — Composants FFmpeg :** Distribution de FFmpeg approuvée en principe, sous réserve expresse de conformité juridique (licences LGPL/GPL) et de la disponibilité préalable d'un dossier de conformité exhaustif validant les binaires exacts distribués, leurs options de compilation précises et la fourniture des codes sources correspondants.
- **Q-006 — Protection des ressources et limites de traitement :** Protection adaptative fondée sur les ressources physiques réelles consommées (RAM, CPU, stockage, handles). Aucune limite numérique arbitraire n'est imposée comme politique générale (aucun plafond arbitraire de taille de fichier, de nombre d'entrées, de profondeur d'archives imbriquées, de volume ou de durée). Surveillance active des ressources et arrêt propre en cas de risque critique, avec maintien strict des protections de sécurité (anti-boucles, anti-malveillance).
- **Q-007 — Archives chiffrées :** Les archives chiffrées sont conservées et traitées comme un contenu opaque. Détection du format et du chiffrement, extraction des seules métadonnées accessibles en clair, signalement explicite de l'inaccessibilité du contenu, conservation de l'archive originale intacte, sans aucune demande interactive de mot de passe ni tentative de déchiffrement.
- **Q-008 — Formats TIFF et BigTIFF :** Prise en charge du TIFF classique prioritaire. La prise en charge de l'extension BigTIFF (adresses 64 bits, fichiers > 4 Go) est conditionnée aux capacités effectivement validées des composants logiciels retenus, avec signalement explicite des variantes non supportées. Un échec d'aperçu sur un fichier volumineux ou BigTIFF ne doit en aucun cas empêcher son inventaire.

---

## 3. Réserves techniques transversales permanentes

### 3.1 Preuve et maturité technique
1. **Distinction entre norme, bibliothèque et intégration :**
   - L'existence d'une spécification ou d'une norme publiée ne démontre en rien qu'un composant logiciel donné sait traiter correctement le format.
   - La capacité théorique d'une bibliothèque tierce ne démontre pas que GraphiScope a intégré ou peut exploiter cette capacité sans régression.
   - Une fonctionnalité intégrée dans le code ne devient une capacité validée qu'après la réalisation des tests appropriés sur jeux d'essais représentatifs.
2. **Dissociation rigoureuse des environnements :**
   - Distinguer systématiquement : la documentation théorique, l'hypothèse d'ingénierie, les tests sur poste de développement outillé, et les tests de validation sur environnement cible Windows standard.
   - Le poste de développement ne fait jamais foi pour attester du comportement sur machine cible.
3. **Maintien de la grille des niveaux de preuve :**
   - Conserver impérativement la qualification formelle issue de GS-0002 : `VERIFIE_SUR_CIBLE`, `TESTE_DEV`, `DOCUMENTE_CONFORME`, `HYPOTHESE_NON_VERIFIEE`, `INCOMPATIBLE`, `INDETERMINE`.

### 3.2 Sécurité des archives et traitement des chemins
1. **Rejet préalable impératif :** Tout chemin malveillant ou suspect au sein d'une archive doit être rejeté avant toute opération d'extraction physique ou logique.
2. **Proscription absolue de la sanitisation silencieuse :** Il est strictement interdit d'effectuer une correction automatique ou une sanitisation silencieuse des chemins dangereux. Les chemins absolus, les chemins UNC, les préfixes de périphériques (`\\.\`), les flux alternatifs NTFS (ADS), les segments de remontée (`..`), ainsi que les noms réservés Windows (`CON`, `PRN`, `AUX`, `NUL`, `COM1..9`, `LPT1..9` avec ou sans extension, espaces ou points terminaux) doivent provoquer un rejet franc et explicite de l'entrée concernée.
3. **Identité des archives imbriquées et détection des cycles :**
   - Ne jamais assimiler une combinaison signature + taille déclarée + CRC32 à une preuve fiable d'identité de contenu : il s'agit d'une simple empreinte indicative de correspondance candidate en repli.
   - Le calcul de l'empreinte cryptographique SHA-256 sur le flux décompressé constitue l'identifiant primaire présumé (Niveau 2).
   - Les empreintes indicatives (CRC32, taille) ne doivent jamais constituer seules une preuve de cycle ni un motif isolé d'arrêt d'extraction.
4. **Surveillance face aux identités incertaines :** En cas d'identité incertaine ou d'impossibilité de calcul cryptographique immédiat, la surveillance dynamique des ressources et les garde-fous du watchdog doivent rester pleinement opérationnels pour prévenir tout épuisement système.
5. **Absence de confinement OS actif en Phase A :** Ne jamais présenter un mécanisme d'isolation ou de sandbox comme effectif avant sa conception complète, son implémentation et sa validation sur machine cible.

### 3.3 Ressources et stabilité
1. **Protection multicritère :** La surveillance de l'exécution doit impérativement reposer sur la corrélation dynamique de signaux multiples (mémoire vive allouée, pression mémoire globale de Windows, débit réel de progression, charge CPU, activité I/O, espace disque résiduel, épuisement des handles système).
2. **Interdiction des arrêts sur signal isolé :** Ne jamais arrêter un traitement sur la seule base d'une forte charge CPU ou d'une période d'inactivité I/O ponctuelle (ces états pouvant correspondre à des phases légitimes de calcul intensif ou de décodage algorithmique).
3. **Contrôle et escalade :** Prévoir des mécanismes coopératifs d'interruption (`CancellationToken`), complétés par des dispositifs de surveillance externe (Windows Job Objects pour quotas mémoires/processus) et une escalade progressive vers la terminaison ordonnée puis forcée en cas de blocage critique.
4. **Priorité absolue :** La stabilité globale de l'application GraphiScope et du système d'exploitation Windows prime en toutes circonstances sur l'achèvement d'une tâche d'inventaire ou d'analyse d'un fichier spécifique.

### 3.4 Métadonnées et relations
1. **Non-fiabilité intrinsèque des métadonnées déclaratives :** Les métadonnées descriptives ou déclaratives embarquées (notamment les blocs XMP, les tags EXIF ou les propriétés documentaires) ne constituent en aucun cas des preuves formelles d'authenticité, d'intégrité ou de traçabilité.
2. **Nature hypothétique des relations inférées :** Toute relation déduite par ressemblance de nommage, proximité arborescente, métadonnées partagées ou séquence temporelle reste une simple hypothèse de travail (niveau candidat).
3. **Interdiction de modification unilatérale :** Aucune fusion d'entités, aucune suppression de fichier et aucune validation définitive de lien heuristique ne doit être effectuée sans le consentement explicite et informé de l'utilisateur.

### 3.5 Formats Microsoft Office
1. **Consultation et extraction uniquement :** Le rôle de GraphiScope vis-à-vis des formats bureautiques Office se limite strictement à l'inventaire, à l'extraction structurée des textes et métadonnées accessibles, et à la présentation synthétique de leur organisation.
2. **Interdiction formelle d'automatisation ou de rendu fidèle :**
   - Aucun moteur d'automatisation Office (Automation COM, Office Interop) ne doit être requis ou utilisé.
   - Aucune dépendance obligatoire envers la présence de la suite Microsoft Office sur le poste utilisateur.
   - Aucun rendu graphique « pixel-perfect », aucune conversion de format vers PDF, et aucune exécution de macros (VBA ou macros Excel 4.0 XLM) ne sont autorisés.
3. **Visibilité des limites :** Les limites inhérentes à chaque format (formats legacy binaires `DOC/XLS/PPT`, classeurs binaires `XLSB`, etc.) doivent rester explicitement signalées à l'utilisateur.
4. **Absence de garantie antivirale :** L'inspection statique ou le repérage de flux macro (`vbaProject.bin`, feuilles XLM) ne doit jamais être présenté comme une analyse de sécurité exhaustive ou une garantie d'innocuité antivirus.

### 3.6 Polices de caractères, images et médias
1. **Données de polices non fiables :** Les fichiers de polices de caractères (`TTF`, `OTF`, `WOFF`, `EOT`) constituent des données binaires complexes historiquement exposées à des vulnérabilités de parsing. Elles doivent être traitées comme des données non fiables, y compris lorsqu'elles sont passées aux API natives du système d'exploitation (DirectWrite / `dwrite.dll`) ou au moteur de rendu du navigateur (`@font-face`). L'installation de polices sur le système hôte demeure exclue et non arbitrée.
2. **BigTIFF conditionnel :** La prise en charge de BigTIFF dépend strictement des bibliothèques effectivement sélectionnées, compilées et validées.
3. **Audio / Vidéo :** La prise en charge des flux multimédias et de leurs codecs dépend exclusivement des composants effectivement distribués et validés sur le plan légal et technique.
4. **Conditionnalité FFmpeg :** La distribution de FFmpeg demeure strictement suspendue à l'établissement et à la validation formelle de son dossier de conformité juridique (Q-005).

### 3.7 Architecture locale et environnement
1. **Fonctionnement local strict :** L'application est fondamentalement conçue comme un logiciel local pour postes Windows (10 et 11), sans dépendance obligatoire envers une infrastructure cloud ou des API tierces en ligne.
2. **Implantation du catalogue :** Le catalogue et ses bases de données locales doivent résider dans le profil de l'utilisateur Windows (`%LOCALAPPDATA%`), assurant le cloisonnement des sessions et la conformité aux droits d'accès Windows.
3. **Indépendance du cœur d'inventaire :** Le cœur d'inventaire (analyse arborescente, calcul d'empreintes, indexation des métadonnées fondamentales) doit fonctionner de manière totalement autonome et ne jamais être bloqué par les composants annexes de rendu graphique ou de prévisualisation.
4. **Surveillance du système de fichiers :** L'API `FileSystemWatcher` de Windows ne peut en aucun cas constituer le mécanisme unique ou exclusif de synchronisation en temps réel (en raison des risques avérés de débordement de tampon sous forte charge I/O). Une procédure de réconciliation périodique et manuelle par balayage récursif reste indispensable.
5. **Adéquation aux cibles réelles :** Toute déclaration de compatibilité, de performance ou de stabilité doit être étayée par des campagnes d'essais adaptées aux postes cibles représentatifs.

### 3.8 Domaines spécialisés
1. **CAO / Patronage textile (Lectra / Gerber Technology / AccuMark) :** La faisabilité technique globale d'ingestion de ces formats reste entièrement à démontrer. Seule une étude préliminaire sur le format d'échange ouvert 2D DXF-AAMA est documentée ; aucune promesse de prise en charge complète ne doit être annoncée sans validation concrète sur des fichiers réels.
2. **Exclusion stricte de Gerber PCB :** Le format de fabrication électronique Gerber PCB (`RS-274X`, `Gerber X2`) demeure strictement hors périmètre fonctionnel du projet GraphiScope.
3. **Couverture des actifs créatifs :** Les banques de ressources graphiques, les fichiers vectoriels complexes, les archives issues de plateformes créatives (telles que Freepik / Magnific) et les polices réparties sur les disques utilisateurs constituent des cibles fonctionnelles maîtresses de GraphiScope qui doivent rester intégrées dans les réflexions d'ingénierie.

---

## 4. Gouvernance et limites d'autorisation

1. **Absence d'autorisation de Phase B :** L'acceptation formelle et la clôture de la tâche GS-0002 ne confèrent aucune autorisation d'entrer en Phase B, ni d'engager des travaux d'implémentation.
2. **Nécessité d'un mandat distinct :** Aucun développement logiciel, prototype applicatif, compilation, chaîne de build ou installation de dépendance de production ne peut démarrer sans un mandat formel, distinct et explicite du propriétaire du projet.
3. **Obligation de consultation préalable :** Tout agent intervenant sur le projet est tenu de lire `AGENT_RULES.md`, `PROJECT_STATE.md`, `DECISIONS.md`, la fiche de la tâche attribuée, ainsi que le présent document permanent de réserves transversales avant toute modification.
4. **Inviolabilité des arbitrages :** Aucun agent n'a l'autorité de transformer une hypothèse technique en décision propriétaire, ni de substituer ses préférences architecturales aux arbitrages souverains validés.
5. **Périmètre strict :** Aucune modification documentaire, technique ou structurelle hors du périmètre explicite d'un mandat n'est tolérée.
6. **Interdiction des actions silencieuses et des opérations Git non autorisées :** Aucune modification silencieuse, suppression ou restructuration d'éléments de suivi n'est admise. Aucune commande Git ne doit être exécutée sans autorisation formelle.
7. **Journal d'exécution append-only :** Le fichier [project-tracking/EXECUTION_LOG.md](file:///m:/dev/GraphiScope/project-tracking/EXECUTION_LOG.md) est strictement alimenté en mode ajout à la fin (*append-only*), sans modification ni réécriture de l'historique antérieur.
8. **Traçabilité :** Toute nouvelle réserve, constat d'audit ou décision doit être rigoureusement documenté, sourcé, justifié et daté.

---

## 5. Règles de gestion et d'évolution du document

1. **Stabilité des décisions acquises :** Les décisions propriétaires acquises (Section 2) ne peuvent être modifiées, amendées ou annulées que par une nouvelle décision formelle et explicite du propriétaire du projet.
2. **Enrichissement des réserves :** Le corpus des réserves techniques transversales (Section 3) a vocation à être complété à mesure que de nouveaux risques, contraintes de formats ou limites d'outils sont identifiés lors des phases ultérieures autorisées.
3. **Traçabilité des modifications :** Toute mise à jour du présent document doit être datée, justifiée par un mandat ou un constat technique vérifiable, et faire l'objet d'une mention au journal d'exécution.
4. **Conservation des réserves levées :** Lorsqu'une réserve technique est résolue ou levée par des tests formels probants sur machine cible, la mention de la réserve ne doit pas être purement et simplement effacée du document. Elle doit être déplacée dans une section dédiée d'archives des réserves levées, accompagnée des références de la preuve matérielle, du rapport d'essai et de la décision correspondante.
5. **Primauté en cas de divergence :** En cas d'ambiguïté ou de divergence entre une hypothèse formulée dans une note de travail et une disposition du présent document, la décision propriétaire et les réserves du présent document prévalent de plein droit.
6. **Complémentarité :** Le présent document ne se substitue pas aux documents de conception détaillés mais constitue le filtre de conformité transversal permanent pour GraphiScope.


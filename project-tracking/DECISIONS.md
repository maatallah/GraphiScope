# DECISIONS

## 1. Règles de distinction

Les décisions validées par le propriétaire sont documentées ici.

Les propositions d'agents, idées ou recommandations techniques ne doivent pas être confondues avec des décisions validées.

## 2. Décisions validées

### DEC-0001 — Initialisation du système local de suivi des tâches et de gouvernance des agents

- Date de décision : date exacte non enregistrée
- Sujet : mise en place d'un système local, persistant, lisible et reproductible pour le suivi des tâches
- Contenu exact : création d'un registre opérationnel local basé sur fichiers texte, sans dépendance cloud ni base de données externe
- Périmètre : seul le système de suivi local et ses fichiers de gouvernance autorisés
- Statut : VALIDATED_OWNER
- Remplace : aucune
- Remplacée par : aucune
- Référence : mandat opérationnel GS-OPS-INIT-001

Aucune date exacte n'a été établie à partir d'une preuve fiable dans le système de suivi. La date précise ne doit pas être inventée ni rétroactivement attribuée.

### DEC-0002 — Validation et clôture formelle de la tâche GS-0002 et de la matrice de décisions techniques et fonctionnelles V1.11.1

- Date de décision : 10 novembre 2026
- Sujet : validation formelle de la matrice de décisions techniques et fonctionnelles issue de GS-0002 (version V1.11.1), clôture de la tâche GS-0002 au statut DONE, confirmation des huit arbitrages souverains Q-001 à Q-008 et institution du référentiel transversal permanent des réserves et contraintes
- Contenu exact :
  1. Acceptation formelle de la matrice `project-tracking/GS-0002-DECISION-MATRIX.md` en version V1.11.1 comme cadre documentaire de référence pour la Phase A.
  2. Clôture formelle de la tâche GS-0002 au statut DONE.
  3. Confirmation pleine, entière et définitive des huit arbitrages souverains Q-001 à Q-008 rendus par le propriétaire.
  4. Création du référentiel permanent `project-tracking/RESERVES-ET-CONTRAINTES-TRANSVERSALES.md` et imposition de sa consultation obligatoire dans `AGENT_RULES.md`.
  5. Rappel formel : cette validation documentaire ne vaut en aucun cas autorisation d'ouverture de la Phase B, de développement applicatif, de prototypage ni d'opérations Git.
- Périmètre : cadrage documentaire Phase A, tâche GS-0002, matrice V1.11.1, référentiel transversal
- Statut : VALIDATED_OWNER
- Remplace : aucune
- Remplacée par : aucune
- Référence : mandat opérationnel GS-OPS-GS0002-CLOSURE-001

### DEC-0003 — Acceptation et clôture de GS-0003

- Date de décision : 11 novembre 2026
- Sujet : acceptation formelle du livrable documentaire GS-0003 V1.6.0 et clôture administrative de la tâche GS-0003
- Contenu exact :
  1. Le propriétaire accepte formellement `project-tracking/GS-0003-PHASE-B-PREPARATION.md` en version V1.6.0.
  2. Cette acceptation s'appuie sur le rapport de mission COR-005 et l'audit contradictoire AUDIT-006 (`GS-OPS-GS0003-AUDIT-006`).
  3. GS-0003 est déclaré DONE au titre de son périmètre documentaire et préparatoire.
  4. Les rapports COR-005 et AUDIT-006 sont conservés comme pièces historiques de référence.
  5. Les arbitrages souverains Q-001 à Q-008 conservent pleinement leur portée et leur signification.
  6. Les questions ouvertes O-01 à O-04 demeurent ouvertes et ne sont pas transformées en décisions.
  7. Les réserves, hypothèses, limites et éléments non vérifiés figurant dans le livrable accepté conservent leur statut.
  8. La clôture de GS-0003 ne constitue pas une validation expérimentale ou une preuve d'implémentation des mécanismes décrits.
  9. La Phase B n'est pas autorisée par DEC-0003.
  10. Toute activité ultérieure de développement, de réalisation, de prototype ou de build nécessite un mandat distinct et une autorisation explicite du propriétaire.
- Périmètre : préparation technique préalable à la Phase B, tâche GS-0003, document V1.6.0, clôture administrative et documentaire
- Statut : VALIDATED_OWNER
- Remplace : aucune
- Remplacée par : aucune
- Référence : mandat opérationnel GS-OPS-GS0003-CLOSURE-001

### DEC-0004 — Arbitrages souverains des questions ouvertes O-01 à O-04 et règle financière générale transversale

- Date de décision : 12 novembre 2026
- Sujet : consignation documentaire des arbitrages souverains du propriétaire sur les questions ouvertes O-01 à O-04 issues de la préparation technique GS-0003, et institution de la règle financière générale et impérative d'absence de toute dépense obligatoire pour l'ensemble des composantes du projet GraphiScope
- Contenu exact :
  1. **Arbitrage O-01 (Gestion des polices) :** Option A retenue, avec `stb_truetype` comme alternative facultative si applicable. Prévisualisation des polices en mémoire, sans installation dans Windows. Utilisation de `@font-face` comme mécanisme principal de prévisualisation dans le navigateur local. Possibilité d'étudier `stb_truetype` comme solution facultative de rasterisation, uniquement si son emploi est approprié et techniquement vérifié (ne constitue pas un substitut direct à `@font-face` ni au moteur typographique du navigateur). Aucune installation automatique de polices. Aucune écriture dans `%WINDIR%\Fonts`. Aucune modification du registre Windows pour enregistrer des polices. Une éventuelle installation per-user relève d'une étude distincte, avec consentement explicite de l'utilisateur. Les polices malformées, non exploitables ou soumises à des conditions incompatibles avec l'opération envisagée doivent être signalées sans bloquer l'inventaire. *Réserve technique obligatoire :* La sécurité de `stb_truetype` face aux polices non fiables, la validation des offsets internes et les protections mémoire doivent faire l'objet d'une évaluation distincte ; son utilisation ne doit pas devenir une dépendance fonctionnelle obligatoire ; cette décision ne vaut en aucun cas validation de sécurité de la bibliothèque.
  2. **Arbitrage O-02 (FFmpeg) :** Utilisation interne conditionnelle et distribution future sous réserve expresse de conformité complète. FFmpeg peut être utilisé en interne, sous réserve du respect des licences applicables. Son architecture doit rester facultative et indépendante du cœur de GraphiScope. La distribution future de GraphiScope avec FFmpeg est conditionnée à un dossier complet de conformité juridique, technique et documentaire. GraphiScope doit pouvoir être distribué sans FFmpeg si ces conditions ne sont pas réunies. Les licences des dépendances, les notices, les obligations relatives aux sources et les éventuelles contraintes liées aux brevets des codecs doivent être examinées. La configuration binaire envisagée ne doit pas être déclarée conforme à la redistribution sans vérification effective. *Distinctions obligatoires :* L'utilisation interne ne doit pas être confondue avec la distribution à des clients ou le transfert à un sous-traitant indépendant ; LGPL ne signifie pas absence de toute obligation ; `--enable-gpl` et `--enable-nonfree` nécessitent une vigilance particulière (certaines configurations peuvent être incompatibles avec la redistribution envisagée) ; aucune conclusion juridique définitive ne doit être présentée comme acquise sans examen de la version, de la configuration et de l'ensemble des dépendances concernées.
  3. **Arbitrage O-03 (Bibliothèque graphique) :** Option A retenue, SkiaSharp comme candidate principale pour le module graphique. ImageSharp n'est pas retenue comme candidate principale. Le choix de SkiaSharp reste soumis à la vérification de ses dépendances natives (`libSkiaSharp.dll`), de leurs licences, de leurs obligations de redistribution et de leurs conditions d'utilisation. Le module graphique doit rester indépendant du cœur d'inventaire. Une défaillance du moteur graphique ne doit pas empêcher l'inventaire des fichiers.
  4. **Arbitrage O-04 (Anciens formats binaires Office DOC, XLS, PPT) :** Option B retenue, extraction du contenu accessible. Extraction des métadonnées disponibles pour DOC, XLS et PPT. Extraction textuelle ou tabulaire lorsque les structures internes sont accessibles. Signalement explicite des contenus inaccessibles et des erreurs d'extraction avec attribution d'un statut dégradé lorsque cela est nécessaire. Aucune garantie d'extraction exhaustive ou de restitution fidèle. *Limites maintenues :* La décision propriétaire Q-002 reste inchangée (prise en charge partielle et qualifiée, limites documentées par format, aucune promesse de complétude, aucun rendu, aucune conversion, aucune automatisation Office, aucune exécution de macros). Cette décision ne doit pas être interprétée comme une autorisation d'ajouter des fonctionnalités Office au-delà de l'extraction et de la consultation structurée déjà définies.
  5. **Règle financière générale, impérative et applicable à tout GraphiScope :** Le propriétaire impose l'absence de toute dépense obligatoire pour l'ensemble des composantes du projet (bibliothèques et frameworks, dépendances directes et indirectes, moteurs de rendu et composants natifs, SDK, compilateurs et outils de développement, outils de construction et de déploiement, licences d'exécution et de redistribution, services externes, API et hébergement, maintenance nécessaire au fonctionnement des fonctions essentielles). Sont formellement exclus : les licences commerciales payantes obligatoires, les abonnements obligatoires, les frais obligatoires par utilisateur, poste, installation ou distribution, les services payants indispensables, et toute dépendance dont l'utilisation normale impose un paiement. Cette règle n'interdit pas une dépense facultative décidée ultérieurement par le propriétaire ; elle interdit qu'une dépense soit une condition nécessaire au développement, à l'utilisation ou à la distribution de GraphiScope. *Contrôle obligatoire transversal :* La gratuité apparente du composant principal ne suffit pas, toute la chaîne de dépendances doit être examinée. Une composante dont les conditions financières ou de redistribution ne sont pas établies doit être classée `NON_VERIFIE` et ne peut pas être déclarée conforme. Pour chaque composante, distinguer : licence gratuite sans frais obligatoires, licence libre avec obligations de conformité (notices, sources, redistribution), licence dont l'éligibilité dépend de critères particuliers, licence commerciale payante, dépendance ou condition financière non vérifiée. Aucune dépendance ne doit être déclarée acceptable uniquement parce que son utilisation est gratuite dans un contexte de développement.
  6. **Portée et limites de gouvernance :** Les arbitrages O-01 à O-04 deviennent des décisions propriétaires consignées (`DEC-0004`), mais cela ne constitue pas une validation technique, expérimentale ou de sécurité de leur faisabilité sur les machines cibles. Les statuts de preuve demeurent strictement distincts (`DECISION_PROPRIETAIRE`, `SOURCE_OFFICIELLE`, `TESTE_DEV`, `TESTE_CIBLE`, `HYPOTHESE`, `NON_VERIFIE`, `BLOQUANT`). Aucune bibliothèque n'est marquée comme testée si aucun test correspondant n'a été exécuté et documenté. La sélection d'une candidate ne vaut pas validation de sa compatibilité. **La Phase B demeure strictement non autorisée par DEC-0004.** Toute activité ultérieure de développement, de réalisation, de prototype ou de build nécessite un mandat distinct et une autorisation explicite du propriétaire.
- Périmètre : questions ouvertes O-01 à O-04, gouvernance technique transversale, sélection technologique, règle financière globale
- Statut : VALIDATED_OWNER
- Remplace : statut ouvert des questions O-01 à O-04
- Remplacée par : aucune
- Référence : mandat opérationnel GS-OPS-DECISIONS-O01-O04-001 (conversation du 12 novembre 2026)

### DEC-0005 — Acceptation formelle et clôture du jalon B1 de GraphiScope (tâche GS-0005)

- Date de décision : 13 novembre 2026
- Sujet : acceptation formelle du jalon B1 (Fondations Cœur, Persistance et Confinement) et clôture de la tâche GS-0005 au statut DONE
- Contenu exact :
  1. Le propriétaire accepte formellement le jalon B1 de GraphiScope sous la référence `GS-OPS-GS0005-B1-IMPLEMENTATION-001`, complétée par les rapports `GS-OPS-GS0005-B1-VERIFICATION-CORRECTION-001` et `GS-OPS-GS0005-B1-VERIFICATION-COMPLEMENTAIRE-001`.
  2. Le livrable applicatif B1 (solution `GraphiScope.sln` sous .NET 10 LTS, nomenclature stricte de 37 fichiers approuvés, compilation Release à 0 avertissement et 0 erreur, banc d'essai automatisé de 30 tests xUnit réussis sur 30) est formellement accepté.
  3. La tâche GS-0005 passe formellement du statut `REVIEW` au statut `DONE`.
  4. Les arbitrages d'implémentation B1 sont pleinement confirmés :
     - ARB-01 : persistance SQLite WAL avec `synchronous = NORMAL`, transactions par lots fermés et checkpoints explicites périodiques `TRUNCATE`.
     - ARB-02 : serveur local Kestrel confiné sur loopback `127.0.0.1` (port dynamique éphémère), validation stricte du header `Host`, rejet inconditionnel de `Origin: null` (HTTP 403), protection anti-CSRF default-deny par jeton cryptographique 256 bits (`X-GraphiScope-CSRF`).
     - ARB-03 : nomenclature fermée et respectée de 37 fichiers applicatifs (aucun 38e fichier créé).
     - Règle financière transversale de 0 € scrupuleusement respectée (licences libres auditées MIT, Apache-2.0 et Domaine public pour le moteur natif SQLite 3.53.3).
  5. Confinement du catalogue SQLite : confirmation que le chemin final du catalogue est rigoureusement et hermétiquement confiné à `%LOCALAPPDATA%\GraphiScope\Catalog\` (Q-004) et que toute tentative d'évasion (chemin absolu extérieur, remontée `..`, chemin UNC ou lecteur distant) est bloquée.
  6. Qualification de la réserve SMB : la réserve relative à l'inventaire effectif sur un partage SMB physique distant demeure explicitement classée `NON_VERIFIE`. Elle devra être prise en compte lors des essais sur cible et ne bloque pas l'acceptation des fondations logicielles B1.
  7. Aucun changement technique supplémentaire n'est autorisé dans le cadre de cette clôture.
  8. Aucun travail sur les jalons B2, B3, B4 ou B5 n'est autorisé par la présente décision. Le démarrage d'un jalon ultérieur nécessitera un mandat propriétaire distinct.
  9. Cette acceptation clôt exclusivement le périmètre B1. Elle ne constitue pas une validation globale de GraphiScope ni une autorisation générale de poursuivre l'implémentation.
- Périmètre : Phase B — Jalon B1 (Fondations Cœur, Persistance et Confinement), tâche GS-0005, livrables applicatifs B1
- Statut : VALIDATED_OWNER
- Remplace : aucune
- Remplacée par : aucune
- Référence : Décision propriétaire du 13 novembre 2026 (mandat GS-OPS-GS0005-B1-IMPLEMENTATION-001 et compléments)

### DEC-0006 — Acceptation formelle du jalon GS-0006 (Préparation documentaire du Jalon B2) avec réserves

- Date de décision : 16 novembre 2026
- Sujet : acceptation formelle du livrable documentaire GS-0006 V2.0.0 comme document de cadrage préparatoire du jalon B2 sous réserve expresse de la conservation des réserves techniques R-01 à R-05, et clôture de la tâche GS-0006 au statut DONE
- Contenu exact :
  1. Le propriétaire accepte formellement le livrable documentaire `project-tracking/GS-0006-B2-PREPARATION-PROPOSAL.md` en version V2.0.0 (95 419 octets, 742 lignes, SHA-256 : `5CE1BAEB80DE046494DA9DE838114CD5A34E6EFBA092264643ECBF19C0FDB0F7`).
  2. La tâche GS-0006 passe formellement du statut `REVIEW` au statut `DONE`.
  3. L'acceptation est assortie de la conservation explicite et du maintien opposable des cinq réserves techniques suivantes :
     - **R-01 — Mémoire et ArrayPool :** Les formulations relatives à la mémoire résidente, à la rétention des tableaux par `ArrayPool<byte>.Shared` et aux estimations de consommation mémoire restent insuffisamment qualifiées.
     - **R-02 — Impact du Worker sur l'hôte :** L'affirmation selon laquelle le Worker avorte sans affecter l'hôte est trop catégorique ; les mécanismes Job Object ne garantissent pas l'absence de pression sur les ressources globales du système.
     - **R-03 — Confinement du système de fichiers :** Les garanties liées aux handles, aux mécanismes de partage Windows et à `RootDirectory` doivent être interprétées dans leurs limites démontrées et ne constituent pas une preuve d'isolation hermétique.
     - **R-04 — Politique TAR :** La politique de rejet des entrées TAR spéciales et non prises en charge conserve une réserve de spécification.
     - **R-05 — Historique des métriques :** L'écart historique de comptage des lignes entre les versions du dossier n'est pas réconcilié (la seule normalisation LF/CRLF ne suffisant pas à justifier un changement du nombre de lignes logiques).
  4. Ces réserves restent ouvertes, visibles et opposables ; elles ne sont ni considérées comme corrigées, ni comme définitivement résolues.
  5. Les éléments non démontrés ne peuvent pas être présentés ultérieurement comme des garanties techniques acquises.
  6. La clôture de GS-0006 ne vaut en aucun cas validation de l'implémentation B2, ni validation expérimentale des mécanismes non testés, ni preuve d'isolation hermétique.
  7. **Aucune autorisation de développement B2 ou B3 n'est accordée par cette décision.**
  8. Tout développement ultérieur nécessitera un mandat distinct et explicite du propriétaire.
- Périmètre : Phase B — Préparation documentaire du Jalon B2 (Worker isolé sous Windows Job Objects et décompression d'archives), tâche GS-0006, livrable V2.0.0
- Statut : VALIDATED_OWNER
- Remplace : statut REVIEW de GS-0006
- Remplacée par : aucune
- Référence : Décision propriétaire DEC-0006 du 16 novembre 2026 (Option A — Acceptation documentaire avec réserves)

## 3. Décisions non validées ou non encore acceptées

Aucune proposition d'agent n'a été promue en décision validée dans ce cycle d'initialisation.

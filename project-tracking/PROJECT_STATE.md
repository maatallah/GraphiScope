# PROJECT_STATE

## 1. Identité du projet

- projet : GraphiScope
- environnement : Windows
- objectif opérationnel : gouvernance locale des tâches et reprise fiable entre agents
- statut général : système local de suivi initialisé, consolidé et validé par le propriétaire ; GS-0001 (DONE), GS-0002 (DONE), GS-0003 (DONE), GS-0004 (DONE) et GS-0005 (DONE) formellement closes ; jalon B1 (Fondations Cœur, Persistance et Confinement) formellement accepté par le propriétaire sous décision DEC-0005 et mandat de clôture GS-OPS-GS0005-B1-CLOSURE-001 (37 fichiers approuvés, compilation .NET 10 LTS Release réussie à 0 avertissement et 0 erreur, 30 tests xUnit validés sur 30 avec 100% de succès sur vraie pile HTTP loopback et SQLite 3.53.3, confinement étanche du catalogue, réserve SMB physique maintenue NON_VERIFIE pour essais sur cible) ; jalons B2 à B5 strictement non autorisés sans mandat distinct ; référentiel technique V1.6.0, référentiel permanent des réserves transversales et règle financière transversale de 0 € en vigueur

## 2. Faits vérifiés

- Le workspace réel disponible est le dossier `M:\dev\GraphiScope`.
- Les fichiers visibles au lancement de l'inventaire sont : `GraphiScope.md`, `prompt0.md`, et le dossier `project-tracking/` créé dans le cadre du mandat d'initialisation.
- Aucun dépôt Git n'a été détecté dans le workspace.
- Le système de suivi local est un mécanisme de fichiers texte, sans dépendance cloud ni base de données externe.
- Aucun build, installation, développement de produit ni Phase B n'ont été autorisés.
- `GS-0001` est en statut DONE.
- `GS-0002` est en statut DONE, formellement accepté et clôturé le 10 novembre 2026 par le propriétaire sur la base de la matrice V1.11.1 (mandat GS-OPS-GS0002-CLOSURE-001).
- `GS-0003` est en statut DONE, clôturé après acceptation formelle par le propriétaire le 11 novembre 2026 (décision DEC-0003, mandat GS-OPS-GS0003-CLOSURE-001) du livrable documentaire `project-tracking/GS-0003-PHASE-B-PREPARATION.md` en version V1.6.0 (appuyée sur le rapport COR-005 et l'audit contradictoire AUDIT-006).
- Le document permanent `project-tracking/RESERVES-ET-CONTRAINTES-TRANSVERSALES.md` est créé et son obligation de consultation est intégrée dans `AGENT_RULES.md`.
- La validation finale du propriétaire a été donnée et enregistrée pour GS-0001 (DEC-0001), GS-0002 (DEC-0002), GS-0003 (DEC-0003), GS-0004 (DEC-0004) et GS-0005 (DEC-0005, acceptation formelle du jalon B1).
- Les arbitrages souverains sur les questions ouvertes O-01 à O-04 et la règle financière générale transversale d'absence de toute dépense obligatoire ont été formellement rendus le 12 novembre 2026 par le propriétaire et consignés sous la décision DEC-0004 (mandat GS-OPS-DECISIONS-O01-O04-001).
- La tâche documentaire `GS-0004` est en statut DONE, formellement acceptée et clôturée administrativement par le propriétaire le 12 novembre 2026 après acceptation du rapport GS-OPS-DECISIONS-O01-O04-001 (mandat GS-OPS-GS0004-CLOSURE-001).
- Le propriétaire a formellement autorisé le démarrage de la Phase B le 12 novembre 2026 sous le mandat GS-OPS-GS0005-START-B1-001, restreint exclusivement au jalon B1 ; les jalons B2, B3, B4 et B5 demeurent strictement non autorisés.
- Le SDK officiel .NET 10 LTS (version 10.0.404) a été installé avec succès en espace utilisateur local (%LOCALAPPDATA%\Microsoft\dotnet).
- Les 37 fichiers applicatifs approuvés de B1 (ensembles C.1 à C.5) ont été intégralement créés et sont présents dans le workspace.
- La solution GraphiScope.sln a été compilée sans avertissement ni erreur sous .NET 10 LTS en configuration Release (dotnet build GraphiScope.sln -c Release).
- La suite de tests automatisés xUnit (GraphiScope.Tests) a été exécutée avec 100% de succès : 30 tests exécutés, 30 réussis, 0 échec (dotnet test GraphiScope.sln -c Release --no-build).
- Les preuves des trois réserves bloquantes levées sous GS-OPS-GS0005-B1-VERIFICATION-CORRECTION-001 sont établies :
  1. Axe A : Vulnérabilité NU1903 (GHSA-2m69-gcr7-jv3q / CVE-2025-6965) éliminée par mise à niveau vers Microsoft.Data.Sqlite 10.0.15 intégrant le moteur natif SQLite 3.53.3 (>= 3.50.2) ; 0 vulnérabilité au rapport dotnet list package --vulnerable.
  2. Axe B : Séparation stricte validée entre le confinement local du catalogue SQLite (interdiction formelle de réseau UNC/distant) et les racines d'inventaire acceptant les partages SMB/UNC.
  3. Axe C : Sécurité Kestrel et protection CSRF prouvées de bout en bout sur une vraie socket TCP loopback (127.0.0.1:port_dynamique, rejet Origin: null, rejet origine distante, rejet host falsifié anti-DNS rebinding, cycle complet CSRF).
- Les preuves des arbitrages souverains sont formellement confirmées : ARB-01 (SQLite WAL, synchronous=NORMAL, transactions groupées, wal_checkpoint TRUNCATE), ARB-02 (Kestrel loopback 127.0.0.1, rejet Origin: null, protection anti-CSRF 256 bits, pas de mutation sur GET), ARB-03 (exactement 37 fichiers applicatifs) et règle financière transversale 0 € (exclusion de FluentAssertions v8+ et de toute dépendance payante).
- Le propriétaire a formellement accepté le jalon B1 le 13 novembre 2026 par décision souveraine DEC-0005 (mandat de clôture GS-OPS-GS0005-B1-CLOSURE-001) sur la base des rapports GS-OPS-GS0005-B1-IMPLEMENTATION-001, GS-OPS-GS0005-B1-VERIFICATION-CORRECTION-001 et GS-OPS-GS0005-B1-VERIFICATION-COMPLEMENTAIRE-001. La tâche GS-0005 est clôturée au statut DONE. La réserve relative à l'inventaire effectif sur un partage SMB physique distant demeure explicitement classée NON_VERIFIE pour les futurs essais sur machine cible.

## 3. État documentaire

- système local de suivi : initialisé, consolidé et validé
- état de GS-0001 : DONE
- état de GS-0002 : DONE
- état de GS-0003 : DONE (GS-0003 clôturé après acceptation propriétaire du livrable documentaire V1.6.0)
- état de GS-0004 : DONE (GS-0004 clôturée après acceptation propriétaire du rapport GS-OPS-DECISIONS-O01-O04-001)
- état de GS-0005 : DONE (jalon B1 formellement accepté par le propriétaire sous décision DEC-0005 et mandat de clôture GS-OPS-GS0005-B1-CLOSURE-001 ; 37 fichiers approuvés, 30/30 tests xUnit validés sur vraie pile HTTP loopback et SQLite 3.53.3, 0 vulnérabilité, 0 avertissement, réserve SMB physique maintenue NON_VERIFIE pour essais sur cible)
- validation finale du propriétaire : validée pour GS-0001, GS-0002, GS-0003, GS-0004 et GS-0005 ; livrable documentaire GS-0003 V1.6.0 formellement accepté (décision DEC-0003) ; arbitrages O-01 à O-04 et règle financière générale consignés (décision DEC-0004) ; jalon B1 et livrables applicatifs formellement acceptés (décision DEC-0005)
- état des questions O-01 à O-04 : arbitrées souverainement au plan documentaire (DEC-0004)
- règle financière générale transversale : en vigueur (absence de dépense obligatoire sur toute la chaîne de dépendances, respectée à 100%)
- nouvelle tâche autorisée : aucune à ce stade (toutes les tâches autorisées GS-0001 à GS-0005 sont DONE)
- Phase B : jalon B1 formellement clos ; jalons B2 à B5 strictement non autorisés sans mandat distinct
- prochaine action : en attente du mandat formel et explicite du propriétaire pour le cadrage et l'ouverture éventuelle du jalon B2 (Worker isolé sous Job Objects Win32 et décompression d'archives)

## 4. Distinction entre faits, rapports et décisions

### 4.1 Faits vérifiés

Les faits vérifiés sont ceux listés dans les sections ci-dessus.

### 4.2 Résultats rapportés par un agent

Les résultats de l'initialisation et la qualité du système de suivi sont des éléments rapportés par un agent dans le cadre d'un mandat documentaire, mais ils ne valent pas validation technique de production.

### 4.3 Décisions du propriétaire

Les décisions validées du propriétaire sont uniquement celles enregistrées dans `DECISIONS.md`.

### 4.4 Hypothèses et points non vérifiés

Les conclusions de la Phase A documentaire restent des éléments de contexte et ne doivent pas être transformées en décisions de conception définitives pour la production.

## 5. Point de suivi actuel

Le système local de suivi des tâches et de gouvernance des agents a été initialisé conformément au mandat opérationnel. Il est conçu pour permettre une reprise fiable par d'autres agents, avec une séparation claire entre :
- proposition de tâche ;
- autorisation du propriétaire ;
- exécution autorisée ;
- validation et revue.

## 6. Limites et risques

- La validation finale du propriétaire a été donnée pour le système local de suivi (GS-0001), pour la consolidation documentaire de GS-0002 (DEC-0002), pour la clôture administrative de GS-0003 (DEC-0003), pour la clôture administrative de GS-0004 (GS-OPS-GS0004-CLOSURE-001) et pour l'acceptation formelle et clôture de B1 / GS-0005 (DEC-0005).
- La clôture de GS-0004 confirme l'acceptation et la consignation documentaire des arbitrages O-01 à O-04 et de la règle financière générale ; elle ne constitue en aucun cas une validation technique, juridique, expérimentale ou de sécurité des solutions sélectionnées sur les machines cibles.
- Les réserves techniques classées NON_VERIFIE (notamment sur stb_truetype, FFmpeg, SkiaSharp, OLE2 et l'inventaire effectif sur partage SMB physique distant) demeurent présentes et non résolues.
- La règle financière générale et impérative d'absence de toute dépense obligatoire s'applique à l'ensemble des composantes, outils et dépendances directes ou indirectes de GraphiScope.
- Aucune validation sur machine cible, échantillons réels ou composants de production n'a été effectuée à ce stade.
- L'acceptation du jalon B1 clôt formellement et exclusivement les fondations cœur, persistance et confinement.
- Les jalons B2, B3, B4 et B5 demeurent formellement non autorisés et nécessiteront un mandat distinct avant tout démarrage.
- L'implémentation applicative du jalon B1 a été strictement réalisée dans le respect des 37 fichiers approuvés. Aucun développement lié aux jalons B2, B3, B4 ou B5 n'a été entamé.

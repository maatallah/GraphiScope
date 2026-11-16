# TASKS

## 1. Statuts

- PROPOSED
- READY
- IN_PROGRESS
- BLOCKED
- REVIEW
- DONE
- CANCELLED

## 2. Priorités

- P0 : indispensable
- P1 : important
- P2 : secondaire
- P3 : futur

## 3. Règles de gestion

- Une tâche proposée n'est pas une tâche autorisée.
- Une tâche ne passe en READY que par décision du propriétaire.
- Une tâche active ne doit pas être reprise sans vérification du dernier état connu.
- Les tâches doivent être documentées dans une fiche détaillée dans `project-tracking/tasks/`.
- Une tâche ne peut être mise à DONE sans critères d'acceptation, preuves et validation nécessaires.
- Un retour de revue impose une reprise en IN_PROGRESS ou en BLOCKED selon la nature du correctif, puis une nouvelle revue avant toute validation.

## 4. Registre des tâches

| ID | Titre | Priorité | Statut | Responsable | Dépendances |
| --- | --- | --- | --- | --- | --- |
| GS-0001 | Initialisation du système local de suivi | P0 | DONE | Gouvernance / agent responsable | Aucune |
| GS-0002 | Consolidation documentaire et correction de la matrice de décisions techniques et fonctionnelles | P0 | DONE | Audit documentaire / agent responsable | GS-0001 DONE |
| GS-0003 | Préparation technique préalable à la Phase B de GraphiScope | P0 | DONE | Conception technique / agent responsable | GS-0001 DONE, GS-0002 DONE |
| GS-0004 | Consignation documentaire des arbitrages O-01 à O-04 et de la règle financière générale | P0 | DONE | Gouvernance / agent responsable | GS-0001 DONE, GS-0002 DONE, GS-0003 DONE |
| GS-0005 | Phase B — Jalon B1 : Fondations Cœur, Persistance et Confinement | P0 | DONE | Architecture & Développement B1 / agent responsable | GS-0001 DONE, GS-0002 DONE, GS-0003 DONE, GS-0004 DONE |
| GS-0006 | Phase B — Préparation documentaire du Jalon B2 : Worker isolé sous Windows Job Objects et décompression d'archives | P0 | REVIEW | Conception technique & Cadrage B2 / agent responsable | GS-0001 DONE, GS-0002 DONE, GS-0003 DONE, GS-0004 DONE, GS-0005 DONE |

## 5. Tableau des transitions autorisées

| Statut initial | Statut suivant | Autorisation |
| --- | --- | --- |
| PROPOSED | READY | Propriétaire uniquement |
| PROPOSED | CANCELLED | Propriétaire |
| READY | IN_PROGRESS | Agent autorisé, après vérification préalable |
| IN_PROGRESS | BLOCKED | Agent, avec justification |
| IN_PROGRESS | REVIEW | Agent, avec preuves et passation |
| BLOCKED | IN_PROGRESS | Après résolution documentée et dans le périmètre autorisé |
| REVIEW | IN_PROGRESS | Après retour de revue et corrections demandées |
| REVIEW | DONE | Propriétaire, après acceptation |
| READY | CANCELLED | Propriétaire |
| IN_PROGRESS | CANCELLED | Propriétaire, après examen de l'état réel |

Aucune transition non définie ne doit être effectuée silencieusement.

## 6. Fiche détaillée

La fiche de travail détaillée de GS-0001 est disponible dans `project-tracking/tasks/GS-0001.md`.
La fiche de travail détaillée de GS-0002 est disponible dans `project-tracking/tasks/GS-0002.md`.
La fiche de travail détaillée de GS-0003 est disponible dans `project-tracking/tasks/GS-0003.md`.
La fiche de travail détaillée de GS-0004 est disponible dans `project-tracking/tasks/GS-0004.md`.
La fiche de travail détaillée de GS-0005 est disponible dans `project-tracking/tasks/GS-0005.md`.
La fiche de travail détaillée de GS-0006 est disponible dans `project-tracking/tasks/GS-0006.md`.

## 7. Distinction entre proposition et autorisation

- PROPOSED : la tâche mérite d'être examinée.
- READY : la tâche est explicitement autorisée par le propriétaire pour l'exécution du périmètre décrit.
- IN_PROGRESS : la tâche est exécutée dans le périmètre autorisé.
- REVIEW : la tâche est soumise à validation ou à revue de cohérence.
- DONE : la tâche a été acceptée par le propriétaire après exécution et validation conformes.

## 8. Tâches proposées

Aucune proposition de tâche supplémentaire n'est enregistrée dans ce cycle d'initialisation.

# AGENT_RULES

## 1. Règle fondamentale

Toute intervention doit commencer par la lecture de ce document, puis des fichiers de suivi du projet, du référentiel transversal permanent (`project-tracking/RESERVES-ET-CONTRAINTES-TRANSVERSALES.md`), puis de la fiche de tâche assignée.

Les règles ci-dessous s'appliquent à tous les agents, modèles, outils et environnements.

## 2. Vérification obligatoire avant toute action

Avant toute opération, un agent doit :
- lire l'état courant du projet ;
- lire les décisions validées du propriétaire ;
- consulter le référentiel permanent des réserves et contraintes transversales (`project-tracking/RESERVES-ET-CONTRAINTES-TRANSVERSALES.md`) ;
- vérifier le workspace réel ;
- vérifier l'absence ou la présence d'un dépôt Git ;
- vérifier les fichiers préexistants et les modifications déjà présentes ;
- vérifier le périmètre exact autorisé pour la tâche ;
- vérifier les dépendances, blocages et risques.

### 2.1 Consultation obligatoire du document des réserves et contraintes transversales

La consultation du document permanent `project-tracking/RESERVES-ET-CONTRAINTES-TRANSVERSALES.md` est une obligation impérative pour tout agent dans les situations suivantes :
- au démarrage de toute nouvelle tâche ;
- avant toute proposition d'architecture ou de technologie ;
- avant toute modification documentaire ou technique ;
- lors de la reprise d'une tâche interrompue ;
- lors du transfert de contexte entre agents.

Chaque agent doit confirmer explicitement dans son compte rendu de mission ou livrable qu'il a consulté ce document et qu'il a identifié l'ensemble des contraintes, arbitrages et réserves applicables à sa tâche.

## 3. Respect strict du périmètre

Un agent doit exécuter uniquement le travail explicitement autorisé.

Il ne doit ni :
- développer une fonctionnalité non autorisée ;
- démarrer une phase non autorisée ;
- modifier un fichier hors du périmètre ;
- supprimer ou renommer un fichier sans autorisation ;
- utiliser Git pour modifier l'état du dépôt si le projet n'est pas un dépôt valide ou si l'activation n'a pas été donnée ;
- remplacer les décisions du propriétaire par des suppositions ou des préférences d'agent.

## 4. Préservation des éléments existants

Les fichiers déjà présents dans le workspace doivent être conservés sans modification inutile.

Une création ou une modification doit être justifiée par le mandat, la tâche autorisée et la cohérence avec le système de suivi.

Aucune opération destructrice n'est autorisée sans validation explicite.

## 5. Traçabilité et preuve

Tout travail significatif doit être consigné dans le journal d'exécution.

Les preuves doivent être documentées sous forme de faits vérifiables :
- contenu réel des fichiers ;
- liste des fichiers créés ;
- état des dossiers ;
- absence ou présence de Git ;
- résultats de vérification ;
- écarts ou blocages constatés.

Aucun agent ne doit masquer un écart, un conflit ou une erreur.

## 6. Vérification avant déclaration de succès

Avant de déclarer une tâche comme réussie, un agent doit vérifier :
- que les fichiers concernés existent réellement ;
- que le périmètre est respecté ;
- que les règles de création ou de modification sont respectées ;
- que les dépendances sont satisfaites ;
- que l'état du projet est cohérent avec l'environnement réel ;
- que les preuves sont disponibles et lisibles ;
- que les points de blocage sont signalés s'il y en a.

## 7. Gestion des conflits et des passages de relais

En cas de conflit entre agents, de double responsabilité ou d'ambiguïté de périmètre :
- suspendre les modifications ;
- signaler le conflit ;
- demander clarification ;
- conserver des preuves de l'état observé.

Un agent de remplacement doit lire l'état courant, les décisions et la fiche de tâche avant de reprendre le travail.

## 8. Propositions de tâches

Les agents peuvent proposer des tâches nouvelles, mais elles restent des propositions tant qu'elles n'ont pas été validées par le propriétaire.

Les propositions doivent :
- être inscrites dans le registre des tâches ;
- disposer d'une fiche détaillée ;
- distinguer clairement le besoin du périmètre autorisé ;
- ne pas se substituer à une décision validée.

### 8.1 Gouvernance des tâches

- Un agent peut créer une tâche au statut PROPOSED.
- Il peut compléter ou corriger sa propre proposition tant qu'elle n'est pas autorisée.
- Seul le propriétaire peut faire passer une tâche à READY.
- READY signifie que le propriétaire a autorisé l'exécution du périmètre décrit dans la fiche.
- Un agent ne peut jamais s'auto-attribuer une autorisation.
- Une modification importante du périmètre d'une tâche READY nécessite une nouvelle validation du propriétaire.

## 9. Phase et autorisation

Aucune phase non autorisée ne doit être démarrée.

Le statut PROPOSED ne confère pas l'autorisation d'exécution.
Le statut READY n'est attribuable que par le propriétaire.

### 9.1 Transitions de statuts

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

Un statut DONE ne signifie pas nécessairement que toutes les phases suivantes sont autorisées.

### 9.2 Coordination et concurrence

- Une seule session d'agent doit être considérée comme responsable des modifications d'une tâche à un instant donné.
- Avant toute écriture, l'agent doit vérifier qu'aucun autre agent n'intervient sur la même tâche.
- Toute prise en charge doit être enregistrée avec l'identifiant de l'agent, la tâche, la date et l'heure réelles de début.
- Toute fin de session doit laisser un état de passation.
- En cas de doute sur une intervention concurrente, suspendre les écritures.

Si un mécanisme de verrouillage est mis en place, son fonctionnement, son propriétaire, son expiration éventuelle et les règles de récupération doivent être documentés. Un fichier Markdown ne garantit pas, à lui seul, une exclusion mutuelle technique.

### 9.3 Journal append-only

- Les événements sont ajoutés à la fin du journal.
- Les entrées historiques ne sont ni supprimées ni réécrites pour masquer une erreur.
- Une correction est enregistrée par une nouvelle entrée qui référence l'événement concerné.
- Les horodatages doivent correspondre à des valeurs réellement connues.
- Si l'heure exacte n'est pas disponible, elle doit être signalée explicitement.
- Les faits observés doivent être distingués des déclarations de l'agent.

### 9.4 Reprise par un autre agent

Avant de reprendre une tâche, le nouvel agent doit :
1. relire AGENT_RULES.md ;
2. relire PROJECT_STATE.md et DECISIONS.md ;
3. relire TASKS.md ;
4. relire project-tracking/RESERVES-ET-CONTRAINTES-TRANSVERSALES.md ;
5. lire la fiche détaillée de la tâche ;
6. examiner les dernières entrées du journal ;
7. vérifier l'état réel du workspace ;
8. identifier les modifications non documentées ;
9. établir une nouvelle passation si nécessaire.

Il ne doit pas considérer le compte rendu précédent comme une preuve suffisante.

## 10. Procédure de passation

Un agent qui quitte une tâche doit laisser une passation exploitable, comprenant :
- l'état courant ;
- les éléments déjà validés ;
- les blocs et risques ;
- les fichiers créés ou modifiés ;
- les preuves disponibles ;
- les instructions encore applicables.

## 11. Exigences de conformité

Les agents doivent se conformer à la fois :
- au mandat explicite du propriétaire ;
- au système de suivi local et persistant ;
- au périmètre défini par la tâche assignée ;
- aux contraintes de sécurité et de non-régression du projet.

## 12. Cas de non-autorisation

Si l'environnement réel est incohérent avec le suivi, ou si un fichier a été modifié en dehors du périmètre, l'agent doit :
- le signaler immédiatement ;
- demander clarification ;
- ne pas poursuivre les modifications tant que le conflit n'est pas réglé.

## 13. Règles de clôture

Une tâche ne peut être considérée comme terminée que si :
- l'état réel et le suivi sont cohérents ;
- les preuves sont visibles ;
- le périmètre autorisé a été respecté ;
- les risques et limites sont documentés ;
- la passation est exploitable par un autre agent.

Le travail ne doit pas être présenté comme validé si la validation du propriétaire n'a pas été obtenue.

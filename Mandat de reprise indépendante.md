GS-OPS-GS0002-HANDOVER-001

Mandat de reprise indépendante — GraphiScope / GS-0002

Nature de la mission : reprise indépendante, inventaire et audit documentaire en lecture seule
Projet : GraphiScope
Workspace de référence : M:\dev\GraphiScope
Tâche concernée : GS-0002
Agent précédent : agent dont les dernières propositions ont été jugées non conformes
Statut attendu à l'issue de cette mission : GS-0002 = REVIEW
Autorisation de correction : aucune à ce stade
Autorisation de développement : aucune

1. Position initiale : tu reprends un dossier existant, tu ne démarres pas un nouveau projet

Tu interviens comme nouvel agent chargé de reprendre indépendamment le dossier documentaire GS-0002.

Tu ne dois pas considérer les affirmations, rapports, conclusions ou déclarations de réussite de l'agent précédent comme des faits établis.

Tu dois vérifier directement l'état réel du workspace et des documents.

Les propositions de l'agent précédent, communiquées dans les échanges de conversation, constituent uniquement du matériel de travail non validé. Elles ne sont ni des décisions du propriétaire, ni une preuve de modification effective des fichiers, ni une autorisation d'intégration.

Tu ne dois pas reprendre automatiquement ses conclusions, ses classifications, ses tableaux ou ses modèles conceptuels.

Ton premier objectif est de déterminer précisément ce qui existe réellement, où cela existe et dans quel état.

Tu ne dois pas tenter de terminer GS-0002 dès ton arrivée.

2. Règle absolue : phase initiale strictement en lecture seule

Pendant toute cette mission :

Aucun fichier ne doit être créé.

Aucun fichier ne doit être modifié.

Aucun fichier ne doit être supprimé.

Aucun fichier ne doit être déplacé ou renommé.

Aucun fichier ne doit être remplacé ou écrasé.

Aucun dossier ne doit être créé.

Aucun contenu ne doit être ajouté aux journaux de suivi.

Aucun statut ne doit être modifié.

Aucun changement ne doit être effectué dans les documents de référence.

Aucun changement ne doit être effectué dans les fichiers de configuration.

Sont également interdits :

Toute programmation.

Tout prototype.

Toute installation de dépendance ou de logiciel.

Toute compilation ou construction de projet.

Toute exécution de tests impliquant une modification de l'environnement.

Toute génération de livrable physique.

Toute sélection définitive de bibliothèques ou de composants de production.

Toute modification de la spécification maître V2.1.

Toute modification des décisions du propriétaire.

Toute intervention sur GS-0001.

Toute opération Git, y compris add, commit, reset, checkout, restore, clean, stash, merge, rebase, pull ou push.

N'utilise aucune commande dont les effets secondaires ne sont pas parfaitement maîtrisés.

Tu peux consulter les fichiers, relever leurs métadonnées, comparer leurs contenus et préparer des propositions dans ta réponse.

Si tu constates une anomalie, tu la signales. Tu ne la corriges pas.

Aucune modification ne sera autorisée sans une nouvelle instruction explicite du propriétaire, après examen de ton rapport.

3. Première étape : identifier le workspace réel

Vérifie le dossier :

M:\dev\GraphiScope

Confirme :

Que ce chemin existe.

Que tu travailles effectivement dans ce dossier.

Que le contenu visible correspond au projet GraphiScope.

Que les documents et dossiers de suivi sont présents.

Que le dossier contient ou non un dépôt Git, sans effectuer aucune opération Git.

Ne suppose pas que le workspace est identique à celui décrit par les agents précédents.

Ne confonds pas ce projet avec un autre workspace ou un autre projet.

Présente un inventaire réel des éléments pertinents :

nom ;

chemin relatif ;

type ;

taille si disponible ;

date de modification si disponible ;

présence ou absence ;

rôle apparent.

Distingue les éléments effectivement observés des informations rapportées dans les conversations.

4. Deuxième étape : lire intégralement les documents de gouvernance

Avant toute analyse du contenu de GS-0002, examine les documents suivants s'ils existent :

AGENT_RULES.md

project-tracking/PROJECT_STATE.md

project-tracking/TASKS.md

project-tracking/DECISIONS.md

project-tracking/EXECUTION_LOG.md

project-tracking/tasks/GS-0001.md

project-tracking/tasks/GS-0002.md

Ne considère pas qu'un document est absent simplement parce qu'un agent précédent a déclaré ne pas l'avoir trouvé.

Ne considère pas non plus qu'un document existe parce qu'il est mentionné dans une conversation.

Pour chaque document :

confirme sa présence ou son absence ;

indique son chemin exact ;

lis son contenu ;

relève son statut et ses consignes pertinentes ;

identifie les éventuelles contradictions entre les documents.

Ne modifie aucun de ces documents.

4.1 Rappel de gouvernance à respecter

Les règles suivantes doivent être vérifiées dans les documents réels et respectées :

Le propriétaire est seul habilité à prendre les décisions de périmètre.

Le propriétaire est seul habilité à autoriser les phases de réalisation.

Le propriétaire est seul habilité à promouvoir une tâche vers READY, à accepter un livrable et à clôturer une tâche.

READY signifie autorisation du propriétaire, et non simple disponibilité technique.

REVIEW signifie que le travail est présenté pour examen, sans acceptation définitive.

Un agent ne peut pas déclarer son propre travail accepté.

L'historique d'exécution est append-only.

Les décisions du propriétaire ne doivent pas être réinterprétées ou modifiées par l'agent.

Aucun nouveau travail ne doit être ajouté au périmètre sans autorisation.

GS-0001 est réputée clôturée dans l'historique de travail communiqué, mais tu dois confirmer son état réel dans les fichiers.

Ne rouvre pas GS-0001, ne la corrige pas et ne lui ajoute aucune action.

GS-0002 reste en REVIEW tant que le propriétaire ne l'a pas explicitement acceptée et clôturée.

5. Troisième étape : retrouver les documents de référence réels

Examine les documents suivants s'ils existent :

GraphiScope.md

prompt0.md

Toute copie physique de la spécification maître V2.1.

Toute annexe ou document complémentaire explicitement lié à la spécification.

Les éventuels documents d'audit ou de décision antérieurs.

Attention :

La spécification maître V2.1 a été élaborée et communiquée dans une conversation antérieure. Cela ne prouve pas qu'un fichier physique portant ce contenu existe dans le workspace.

Tu dois établir la distinction suivante :

Élément

Ce que tu dois déterminer

Document physique

Fichier effectivement présent sur disque

Document conversationnel

Contenu communiqué dans une conversation

Document cité

Document mentionné sans preuve de présence

Document absent

Fichier recherché et non trouvé dans le périmètre inspecté

Version incertaine

Plusieurs versions existent sans autorité clairement établie

Ne reconstruis pas un document absent à partir de souvenirs ou de fragments conversationnels.

Ne remplace pas un document physique par une version conversationnelle.

6. Quatrième étape : localiser la matrice GS-0002

Deux emplacements ont été utilisés ou mentionnés dans les travaux précédents :

project-tracking/GS-0002-DECISION-MATRIX.md

project-tracking/deliverables/GS-0002-DECISION-MATRIX.md

Tu dois examiner les deux emplacements, ainsi que leur contenu réel.

Pour chacun, indique :

existence ou absence ;

chemin exact ;

taille ;

date de modification ;

titre et version inscrits dans le document ;

structure des sections ;

état apparent ;

éventuelles différences avec l'autre emplacement.

Si les deux fichiers existent :

ne choisis pas arbitrairement l'un d'eux ;

ne fusionne pas leurs contenus ;

ne déplace aucun fichier ;

ne supprime aucune version ;

compare leurs contenus et relève les différences ;

propose une interprétation de leur relation, sans la transformer en décision.

Si un seul fichier existe, identifie-le comme document physique actuellement disponible, sans conclure qu'il est nécessairement la version approuvée.

Si aucun des deux n'existe, signale-le clairement.

Ne crée pas le dossier deliverables et ne crée pas de matrice de remplacement.

7. Cinquième étape : comprendre exactement le périmètre approuvé par le propriétaire

Les éléments ci-dessous sont des décisions et orientations du propriétaire communiquées au cours du travail GraphiScope. Tu dois les confronter aux documents physiques disponibles et signaler les éventuelles divergences.

Ne les transforme pas en nouvelles décisions.

7.1 Architecture et principes généraux

Ambition : plateforme modulaire.

Application locale sous Windows.

Architecture candidate : interface web locale et backend local.

Base SQLite et FTS5 à valider techniquement.

Inventaire, identification, classification et recherche indépendants de la réussite des aperçus.

Architecture commune avec modules et fournisseurs spécialisés.

Distinguer les environnements de développement, de construction et d'exécution cible.

Le poste de développement ne constitue pas une preuve de compatibilité avec les postes cibles.

7.2 Graphisme, PAO et images

Le périmètre couvre notamment :

PDF ;

EPS ;

SVG ;

TIFF ;

PNG ;

JPEG ;

formats Adobe AI/AIT ;

PSD/PSB ;

INDD/INDT.

Il faut distinguer, selon le format et le fournisseur :

identification ;

validation ;

extraction des métadonnées ;

extraction du contenu ;

miniature native ;

miniature générée ;

rendu ;

aperçu ;

consultation interne ;

dépendances et ressources liées.

7.3 Adobe

Les applications Adobe peuvent être détectées, avec leurs versions et les capacités de leurs fournisseurs d'aperçu éventuels.

Limites impératives :

ne pas lancer les applications Adobe ;

ne pas automatiser les applications Adobe ;

ne pas considérer qu'une application installée garantit un aperçu ;

ne pas confondre détection d'une application, présence d'un composant de prévisualisation et capacité réelle de rendu.

7.4 Audio et vidéo

Le périmètre audio/vidéo est fonctionnellement complet.

Il ne doit pas être réduit à la seule lecture des métadonnées ou à la génération de miniatures.

Étudier distinctement :

identification des codecs et conteneurs ;

validation technique ;

métadonnées ;

durée, dimensions, pistes et flux ;

couverture audio intégrée ;

miniature générée ;

extraction d'image-clé vidéo ;

décodage ;

aperçu ;

lecture effective ;

dépendances codecs et limitations de l'environnement.

La lecture effective reste conditionnée aux capacités réellement disponibles sur le poste cible.

7.5 Polices

Le périmètre couvre les deux catégories :

polices installées dans Windows ;

fichiers de polices non installés, présents dans des emplacements arbitraires.

Il faut distinguer la détection, les métadonnées, les familles, les styles, les glyphes, les spécimens, la consultation et les conditions d'installation.

Une police non installée ne doit pas être considérée comme installée simplement parce que son fichier est indexé.

7.6 Archives Magnific et Freepik

Ce domaine est particulièrement important.

L'utilisateur doit pouvoir explorer le contenu des archives sans extraction générale préalable.

Le système doit étudier :

l'identification des archives ;

l'inventaire de leurs entrées ;

la navigation dans leur arborescence ;

l'identification des ressources vectorielles et raster ;

les métadonnées accessibles ;

les miniatures et aperçus possibles ;

les archives imbriquées ;

les copies extraites ;

les copies binaires exactes ;

les variantes ;

les dérivés ;

les ressources liées et référencées ;

la provenance ;

les relations entre les ressources ;

l'extraction sélective contrôlée ;

la conservation de l'archive originale ;

les polices et autres dépendances éventuelles ;

les informations de licence et les avertissements d'usage.

La sécurité des archives doit inclure au minimum l'analyse des risques de :

path traversal ;

archive bombs ;

chemins dupliqués ;

entrées ambiguës ;

archives imbriquées ;

archives chiffrées ;

archives corrompues ;

liens symboliques ou équivalents ;

noms de fichiers malformés.

Aucune extraction globale ne doit être supposée comme mécanisme normal.

7.7 Microsoft Office

Office : métadonnées uniquement.

Ne pas élargir le périmètre vers la prévisualisation complète, l'édition ou l'automatisation des applications Office.

7.8 Lectra / Gerber

Étude de faisabilité uniquement pour l'écosystème textile et habillement.

Ne pas confondre Gerber Technology avec Gerber PCB.

Aucune intégration de production n'est autorisée par cette étude.

7.9 Doublons et relations

Distinguer au minimum :

D1 : doublon binaire exact ;

D2 : similarité ;

D3 : variante ;

D4 : version.

Aucune suppression automatique de fichier n'est autorisée.

8. Sixième étape : audit critique du contenu réel de la matrice

Une fois le fichier physique localisé et lu, compare son contenu aux exigences ci-dessous.

Ne pars pas du principe que les corrections annoncées par l'agent précédent sont réellement présentes.

Ne valide aucune section uniquement parce qu'elle porte un titre approprié.

8.1 Statuts de preuve

La matrice doit distinguer les sept statuts suivants :

DECISION_PROPRIETAIRE

SOURCE_OFFICIELLE

TESTE_DEV

TESTE_CIBLE

HYPOTHESE

NON_VERIFIE

BLOQUANT

Vérifie :

Que ces statuts sont définis.

Qu'ils sont utilisés de manière cohérente.

Qu'ils sont attribués au niveau approprié.

Qu'une capacité ne reçoit pas abusivement le statut d'une autre capacité.

Que les faits documentaires ne sont pas présentés comme des tests.

Que les tests du poste de développement ne sont pas présentés comme des tests des postes cibles.

Que les hypothèses restent explicitement identifiées.

Un statut global à la fin d'une ligne ne suffit pas lorsque les différentes capacités de cette ligne n'ont pas le même niveau de preuve.

8.2 Distinction obligatoire des capacités

Vérifie que les tableaux des sections 3 et 6 distinguent explicitement, lorsque pertinent :

Identification du format.

Validation structurelle ou technique.

Extraction des métadonnées.

Extraction du contenu.

Miniature native fournie par le fichier ou son écosystème.

Miniature générée par GraphiScope.

Rendu ou prévisualisation.

Consultation interne sans extraction complète.

Décodage.

Lecture effective.

Installation.

Extraction contrôlée.

Ne permets pas les assimilations suivantes :

identification ≠ validation ;

métadonnées ≠ extraction du contenu ;

miniature native ≠ miniature générée ;

miniature ≠ rendu ;

aperçu ≠ lecture ;

décodage ≠ lecture effective ;

consultation interne ≠ extraction sur disque ;

détection d'une police ≠ installation de la police ;

présence d'Adobe ≠ capacité garantie de rendu.

Une cellule « Oui » doit être suffisamment précise pour indiquer ce qui est réellement possible, dans quelles conditions et avec quel niveau de preuve.

8.3 Vérification par famille de formats

Vérifie notamment les distinctions suivantes :

PDF

identification et validation ;

extraction du texte ;

extraction des images ;

métadonnées ;

miniature ;

rendu de page ;

consultation.

EPS / SVG

identification ;

validation ;

métadonnées ;

miniature ;

rendu ;

limites liées aux ressources et aux fonctionnalités spécifiques.

TIFF / PNG / JPEG

identification ;

validation ;

métadonnées ;

dimensions ;

gestion des pages ou images multiples lorsque pertinent ;

miniature native ou générée ;

rendu.

Adobe AI/AIT / PSD/PSB / INDD/INDT

détection du format ;

présence éventuelle d'un aperçu incorporé ;

métadonnées ;

rendu indépendant éventuel ;

dépendance à un fournisseur ;

dépendance à une version ou à une application ;

limites de compatibilité.

Ne regroupe pas tous les formats Adobe sous une affirmation uniforme.

Audio

métadonnées ;

couverture intégrée ;

miniature ou visualisation générée ;

décodage ;

lecture effective.

Vidéo

métadonnées ;

flux audio/vidéo ;

extraction d'image-clé ;

miniature générée ;

rendu ;

décodage ;

lecture effective.

Polices

fichiers installés et non installés ;

métadonnées ;

familles et styles ;

glyphes ;

spécimens ;

consultation sans installation ;

conditions d'installation.

Archives

identification ;

validation ;

inventaire ;

navigation ;

consultation interne ;

aperçu des entrées ;

extraction sélective ;

extraction complète, si envisagée, avec conditions et risques.

8.4 Modèle conceptuel des archives

Vérifie que la section 5 distingue explicitement les objets suivants :

archive physique ;

entrée d'archive ;

fichier extrait ;

copie binaire exacte ;

variante ;

dérivé ;

ressource référencée ou dépendante.

Une entrée d'archive ne doit pas être assimilée à un fichier physique autonome.

Une entrée doit être localisable par une clé composée au minimum de :

identité persistante de l'archive ;

chemin interne exact ;

occurrence ou index complémentaire lorsque plusieurs entrées portent le même chemin.

Vérifie également que le document distingue :

identifiant persistant de l'objet dans le catalogue ;

identité binaire ;

identité logique ;

emplacement ou locator ;

identité d'une entrée dans une archive.

Le modèle doit expliquer le comportement en cas de :

renommage de l'archive ;

déplacement de l'archive ;

modification du contenu de l'archive ;

reconstruction de l'archive ;

changement d'ordre des entrées ;

présence de chemins internes dupliqués ;

extraction répétée d'une même entrée.

Ne présente pas un chemin ou un index comme une identité permanente s'il ne l'est pas.

8.5 Relations entre les objets

Vérifie que la matrice définit au minimum :

CONTIENT

EXTRAIT_DE

COPIE_DE

VARIANTE_DE

DERIVE_DE

REFERENCE

CONTIENT_ARCHIVE

Pour chaque relation, examine :

direction ;

cardinalité ;

objet source ;

objet cible ;

preuve nécessaire ;

caractère automatique ou manuel ;

niveau de certitude ;

conditions de création ;

limites.

Ne considère pas automatiquement VARIANTE_DE et DERIVE_DE comme des relations N:1.

Un dérivé peut avoir plusieurs sources. Une variante peut également résulter d'une combinaison ou d'un historique plus complexe.

Si une relation est ambiguë ou si sa cardinalité ne peut pas être déterminée, indique-le explicitement au lieu d'imposer une règle simplificatrice.

Une empreinte identique prouve une identité binaire selon l'algorithme et les conditions utilisés. Elle ne prouve pas à elle seule l'historique de copie ou la provenance.

8.6 Scénario concret Magnific / Freepik

Vérifie que la matrice contient un scénario suffisamment précis pour représenter le flux suivant :

Une archive Freepik est cataloguée sans extraction générale.

Ses entrées sont inventoriées.

Une entrée vectorielle est identifiée.

Une image raster est identifiée dans la même archive.

Une police ou une ressource liée est détectée, si elle existe.

Les aperçus possibles sont générés ou consultés sans extraction globale.

Une seule entrée est extraite à la demande.

Le fichier extrait est relié à son entrée d'origine.

Une copie binaire exacte située ailleurs est reconnue comme telle.

Une variante est identifiée sans être assimilée à un doublon exact.

Un dérivé est relié à sa ou ses sources.

L'archive originale est conservée.

La provenance reste consultable.

Ce scénario doit correspondre au modèle d'objets et aux relations de la matrice.

Un simple paragraphe narratif ne suffit pas si les objets, leurs identifiants et leurs relations restent indéterminés.

9. Septième étape : audit documentaire des bibliothèques et composants

Ne sélectionne aucun composant de production.

Vérifie si la matrice distingue correctement :

nom exact du composant ;

version stable ;

version préliminaire éventuelle ;

date de publication ;

état de maintenance ;

licence ;

dépendances natives ;

dépendances transitives ;

plateformes supportées ;

fonctions réellement disponibles ;

limitations ;

source officielle correspondante.

Les familles déjà étudiées comprennent notamment :

.NET ;

Microsoft.Data.Sqlite ;

SQLite et FTS5 ;

SQLitePCLRaw et les bundles éventuels ;

SkiaSharp ;

FFmpeg ;

fontTools ;

FreeType ;

PDFium ;

Svg.Skia ;

SharpZipLib ;

LibTIFF ;

Open XML SDK ;

autres solutions spécialisées pour les formats concernés.

Ne valide pas une bibliothèque à partir de son seul nom ou d'une page générique.

Ne considère pas qu'une page d'accueil ou une documentation générale prouve une version, une licence ou une compatibilité.

Les affirmations de maintenance, de compatibilité multiplateforme et de redistribution doivent être étayées.

Lorsqu'une information ne peut pas être établie, utilise NON_VERIFIE.

Ne fabrique jamais de numéro de version, de date, de licence ou de référence.

Cette mission est un audit documentaire : aucune installation, compilation ou expérimentation technique n'est autorisée.

10. Huitième étape : examiner les erreurs de méthode du travail précédent

Les problèmes suivants ont été observés dans les réponses précédentes. Ils doivent être considérés comme des points de vigilance rapportés, et non comme des faits physiques déjà confirmés.

Tu dois vérifier si les mêmes problèmes subsistent dans le fichier réel :

Les capacités étaient parfois présentées avec un simple « Oui » sans préciser ce qui était réellement pris en charge.

Les statuts de preuve étaient parfois globaux au lieu d'être associés à chaque capacité.

Des capacités documentées étaient présentées comme si elles étaient intégrées ou testées.

Les cardinalités de VARIANTE_DE et DERIVE_DE restaient N:1 sans justification suffisante.

Le scénario Freepik était décrit narrativement sans modèle d'objets et de relations suffisamment précis.

La section 6 était parfois une simple liste de fonctionnalités, et non une section fonctionnelle cohérente.

Des informations sur les versions, dates, licences et dépendances restaient génériques ou non vérifiées.

Certaines références documentaires ne correspondaient pas directement à l'affirmation qu'elles étaient censées soutenir.

Les agents ont parfois annoncé des corrections qui n'étaient pas visibles dans le contenu communiqué.

Des réponses ne respectaient pas l'ordre ou la totalité des livrables demandés.

Tu dois vérifier les faits, pas répéter ces critiques mécaniquement.

11. Livrables attendus dans ta réponse uniquement

Tu ne dois créer aucun fichier.

Présente ton rapport dans l'ordre suivant.

Livrable 1 — État réel du workspace

chemin confirmé ;

structure pertinente ;

fichiers présents ;

fichiers absents ;

éventuelles anomalies ;

présence ou absence de dépôt Git ;

éléments non vérifiables.

Livrable 2 — État réel du suivi

état de GS-0001 ;

état de GS-0002 ;

cohérence entre TASKS.md, les fiches de tâche et PROJECT_STATE.md ;

dernières entrées pertinentes de EXECUTION_LOG.md ;

éventuelles contradictions ;

aucune modification proposée comme déjà effectuée.

Livrable 3 — Inventaire des documents de référence

documents physiques ;

documents conversationnels connus ;

versions concurrentes ;

documents manquants ;

autorité documentaire de chaque élément.

Livrable 4 — État réel de la matrice GS-0002

emplacement exact ;

version ;

date ;

contenu ;

différences entre les éventuels exemplaires ;

sections présentes et absentes.

Livrable 5 — Tableau d'audit de conformité

Pour chaque exigence importante, indique :

Exigence

État observé

Preuve précise

Écart

Gravité

Utilise uniquement les états :

CONFORME ;

PARTIEL ;

NON CONFORME ;

ABSENT ;

NON VERIFIABLE.

Ne transforme pas une absence de preuve en preuve de conformité.

Livrable 6 — Analyse détaillée des sections 3, 5 et 6

Présente séparément :

capacités et preuves ;

identité des objets d'archive ;

relations et cardinalités ;

scénario Freepik/Magnific ;

cohérence fonctionnelle de la section 6.

Cite les titres, sous-sections, tableaux ou extraits exacts du document physique.

Si les numéros de ligne sont disponibles, donne-les. Sinon, indique les titres exacts et les extraits courts permettant de retrouver les passages.

Livrable 7 — Liste des corrections nécessaires

Classe les corrections en :

indispensables avant acceptation ;

importantes ;

facultatives.

Pour chacune, précise :

emplacement ;

problème ;

résultat attendu ;

règle à respecter ;

dépendances éventuelles avec d'autres sections.

Ne modifie pas le document.

Livrable 8 — Proposition de plan de reprise

Présente une séquence limitée et rationnelle de correction, sans multiplier les cycles inutiles.

Identifie les points qui peuvent être corrigés ensemble et ceux qui nécessitent une décision du propriétaire.

Ne crée pas de nouvelle tâche.

Livrable 9 — Questions réellement bloquantes

Ne pose que les questions auxquelles les documents existants ne permettent pas de répondre et dont la réponse relève effectivement du propriétaire.

Ne transforme pas des questions techniques de recherche documentaire en questions adressées au propriétaire.

Si aucune question n'est bloquante, indique-le.

Livrable 10 — Conclusion et limites de ton audit

Indique clairement :

ce que tu as personnellement vérifié ;

ce que tu n'as pas pu vérifier ;

les documents effectivement lus ;

les éventuelles informations provenant uniquement des conversations ;

les écarts qui restent ouverts ;

les actions qui nécessiteraient une autorisation distincte.

12. Règles de preuve et de rédaction

Ton rapport doit être vérifiable.

Pour chaque affirmation importante :

indique le fichier concerné ;

cite la section ou le passage ;

distingue le contenu constaté de ton interprétation ;

distingue les faits des hypothèses ;

précise les limites de l'inspection.

Ne dis pas :

« corrigé » si tu n'as pas constaté la correction dans le fichier ;

« vérifié » si tu n'as fait que lire une affirmation ;

« testé » si aucun test n'a été effectué ;

« conforme » si une exigence importante reste ambiguë ;

« validé » si le propriétaire ne l'a pas accepté.

Ne prétends pas avoir inspecté un fichier que tu n'as pas pu ouvrir.

Ne déduis pas l'état d'un fichier de la réponse d'un agent précédent.

Ne confonds pas :

contenu proposé ;

contenu écrit sur disque ;

contenu relu ;

contenu vérifié ;

contenu accepté par le propriétaire.

13. Conditions de fin de mission

À la fin de cette mission :

aucun fichier n'aura été modifié ;

aucun fichier n'aura été créé ;

aucun statut n'aura été changé ;

aucune décision du propriétaire n'aura été modifiée ;

aucune nouvelle tâche n'aura été créée ;

GS-0001 n'aura pas été rouverte ;

aucune phase de développement n'aura été autorisée ;

aucune bibliothèque de production n'aura été sélectionnée.

GS-0002 doit rester en REVIEW.

Tu dois attendre l'examen du propriétaire avant toute correction physique.

Ne considère pas la remise de ton rapport comme une acceptation du livrable.

Ne commence pas spontanément une seconde étape.

14. Instruction finale

Ta mission n'est pas de défendre le travail de l'agent précédent, ni de le condamner sans vérification.

Ta mission est de reprendre le dossier depuis les faits observables, d'identifier exactement l'état réel de la documentation et de fournir au propriétaire une base fiable pour décider de la suite.

Priorité absolue : exactitude, traçabilité, respect du périmètre et absence de toute modification non autorisée.

Commence par l'inventaire en lecture seule. Ne corrige rien. Ne crée rien. Ne prends aucune décision à la place du propriétaire.
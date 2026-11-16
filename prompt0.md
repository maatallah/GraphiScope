GraphiScope — Technical Validation & Prototype Assessment

Mission type: Technical feasibility assessment
Target environment: Windows 11
Execution mode: Forensic inventory + isolated prototypes
Implementation status: NO production application development authorized

1. OBJECTIVE

Conduct a technical validation of the proposed GraphiScope architecture before any production development begins.

GraphiScope is intended to be a local Windows application, accessed through a browser running on the same PC. Its purpose is to inventory, classify, preview, search, analyze, and manage graphic and publishing files such as:

Adobe InDesign: .indd, .indt

Adobe Illustrator: .ai, .ait, .eps

Adobe Photoshop: .psd, .psb

PDF: .pdf

SVG: .svg

CorelDRAW: .cdr

Affinity formats: .afdesign, .afphoto

Other graphic formats discovered during the assessment

The proposed architecture to validate is:

Layer

Proposed technology

Runtime

.NET 10 LTS

Backend

ASP.NET Core

Frontend

React + TypeScript + Vite

Database

SQLite + FTS5

Database access

Controlled SQL repositories

File monitoring

FileSystemWatcher + periodic reconciliation

Windows thumbnails

Windows Shell / COM

PSD/PSB rendering

ImageMagick

SVG rendering

Svg.Skia

PDF rendering

PDFium

Progress reporting

SignalR

Windows file operations

Native Windows APIs / IFileOperation

Deployment

Portable self-contained Win-x64

Network

Localhost only; no cloud dependency

The objective is to establish what is actually available, what works reliably, what requires additional components, and what must be postponed or excluded.

Do not assume that the proposed architecture is correct. Verify it and report any necessary changes.

2. ABSOLUTE EXECUTION RULES

2.1 No production development

You are NOT authorized to:

Create the GraphiScope application.

Modify or generate production application source code.

Create the final project structure.

Implement the backend, frontend, database, or UI.

Modify existing source files, configuration files, scripts, or documentation.

Install software, SDKs, libraries, extensions, codecs, or applications.

Change Windows settings, registry entries, environment variables, PATH, services, or system policies.

Register or unregister Shell extensions.

Modify file associations.

Modify existing graphic files.

Delete, rename, move, or overwrite user files.

Commit, stage, reset, stash, checkout, push, or otherwise alter Git state.

2.2 Read-only environment inventory

The initial environment investigation must be strictly read-only.

You may inspect:

Windows version and architecture.

CPU, RAM, available storage, and GPU information.

Installed .NET SDKs and runtimes.

Node.js and npm availability.

Installed browsers.

Installed graphic applications and their versions.

Installed codecs, Shell extensions, and thumbnail handlers.

Relevant environment variables.

Available development tools.

Existing libraries and their versions.

Do not assume that an application is installed merely because its file format is supported by a library.

2.3 Isolated prototype authorization

After completing and reporting the read-only inventory, you may create small, disposable prototype projects exclusively inside a newly created, clearly identified temporary working directory.

Conditions:

Never create files inside an existing application repository.

Never modify existing user files.

Never use production data as a test destination.

Never overwrite existing files.

Use copies of test samples.

If no suitable samples are available, report the missing samples instead of searching user directories indiscriminately.

Do not install missing dependencies.

Use only tools and libraries already available locally.

If a required dependency is unavailable, document the limitation and stop that specific test.

Clearly distinguish an executed test from a theoretical assessment.

The temporary directory must be reported explicitly before prototype execution.

3. PHASE 1 — ENVIRONMENT INVENTORY

Produce an evidence-based inventory of the actual machine.

3.1 Operating system and hardware

Collect:

Windows edition, version, build, and architecture.

CPU model and logical processor count.

Installed RAM and available memory.

GPU model and driver information.

Available disk space on relevant volumes.

Filesystem types.

Long-path configuration status.

3.2 Development environment

Inspect:

Installed .NET SDKs.

Installed .NET runtimes.

ASP.NET Core runtime availability.

Node.js version.

npm version.

Available C# compilers.

Git version.

PowerShell version.

Available browsers.

Report whether the proposed .NET 10 / React / TypeScript architecture can be built without installing additional software.

Do not install anything.

3.3 Graphic applications

Identify installed applications and versions, where detectable:

Adobe InDesign

Adobe Illustrator

Adobe Photoshop

CorelDRAW

Affinity Designer

Affinity Photo

Other relevant applications

Do not launch applications or modify their configuration without explicit authorization.

3.4 Windows Shell thumbnail infrastructure

Investigate the actual availability of Windows Shell thumbnail providers.

For each relevant extension, determine whether:

A file association exists.

A Shell thumbnail handler appears registered.

The relevant COM component can be identified.

The provider can be queried safely.

A thumbnail can actually be retrieved from a dedicated test copy.

Do not treat registry registration as proof of functional thumbnail generation.

Target extensions:

.indd, .indt, .ai, .ait, .psd, .psb, .pdf, .eps, .svg, .cdr, .afdesign, .afphoto

Record the distinction between:

No handler found.

Handler found but unavailable.

Handler invoked but failed.

Thumbnail successfully retrieved.

Thumbnail retrieved but unusable or incomplete.

4. PHASE 2 — PROTOTYPE A: WINDOWS THUMBNAIL EXPLORER

Objective

Determine which graphic formats can produce usable thumbnails through the Windows Shell on this actual machine.

Required tests

For every available test format:

Query the Shell thumbnail provider.

Record provider identification where possible.

Request a thumbnail at multiple practical sizes.

Measure retrieval time.

Record image dimensions.

Record whether the result is blank, generic, incomplete, or usable.

Record errors and timeouts.

Test a corrupted or invalid sample only if a dedicated test sample is available.

Special attention

InDesign

Determine whether the installed environment can retrieve embedded InDesign previews or generate previews through a Shell provider.

Do not assume all INDD files contain embedded previews.

Illustrator

Determine whether AI thumbnails are available through the Shell.

Distinguish AI files with PDF-compatible content from files without it, only where this can be established safely.

Photoshop

Test PSD and PSB separately.

Deliverable

A results matrix:

Extension

Handler detected

Thumbnail retrieved

Usable

Time

Notes

Provide a clear conclusion for each extension:

Reliable

Conditional

Unavailable

Not tested

5. PHASE 3 — PROTOTYPE B: GRAPHIC PREVIEW BENCH

Objective

Evaluate locally available rendering engines and determine their practical suitability for GraphiScope.

Candidate technologies:

ImageMagick

PDFium

Svg.Skia

Windows Imaging Component (WIC)

Other already-installed engines, if relevant

Required checks

For each engine:

Determine whether it is already available.

Identify its exact version.

Identify its license.

Determine supported formats.

Test only with dedicated sample copies.

Measure rendering time.

Measure memory usage where practical.

Record output dimensions and quality.

Record errors and unsupported features.

Format-specific requirements

PSD / PSB

Determine whether ImageMagick can render representative PSD and PSB files.

Check transparency, dimensions, and whether the rendered result corresponds to the expected composite preview.

PDF

Determine whether PDFium is available and can render representative PDF pages.

SVG

Determine whether Svg.Skia is available.

Consider:

Missing fonts.

Embedded images.

External resource references.

Complex SVG structures.

Extremely large dimensions.

Do not allow unrestricted external resource loading during tests.

EPS / PostScript

Investigate whether a usable local renderer exists.

Ghostscript must not be installed or redistributed as part of this assessment.

Explicitly document its licensing implications and whether an alternative is available.

AI / INDD

Determine whether any available engine can render representative files.

Do not claim universal support based on a single successful file.

Deliverable

A comparison matrix:

Engine

Version

License

Formats tested

Results

Performance

Recommendation

Clearly separate:

Native support.

Conditional support.

Support requiring external software.

Unsupported formats.

6. PHASE 4 — PROTOTYPE C: FILE SYSTEM STRESS TEST

Objective

Assess whether the proposed inventory architecture can handle large and evolving file collections.

Test scenarios

Use generated synthetic files and directory structures in the temporary prototype directory.

Target scenarios:

10,000 files.

100,000 files.

More than 100,000 files, only if hardware and available storage make it reasonable.

Do not generate large real graphic files unnecessarily.

Measure

Initial enumeration duration.

Database insertion duration.

Database size.

Memory consumption.

CPU consumption.

Search performance.

Filter and sort performance.

Incremental update performance.

Recovery after simulated interruption.

Duplicate detection performance on synthetic data.

Behavior when files are renamed, moved, modified, or deleted.

SQLite validation

Check:

SQLite version actually used.

FTS5 availability.

WAL mode behavior.

Transaction performance.

Concurrent reader/writer behavior.

Recovery and integrity checks.

Backup strategy compatibility with WAL.

Pay particular attention to the SQLite WAL-reset issue documented for versions before SQLite 3.51.3, including applicable backports.

Do not assume that a package version and its bundled native SQLite version are identical.

FileSystemWatcher validation

Test:

Creation.

Modification.

Rename.

Deletion.

High event volume.

Buffer overflow behavior, where safely reproducible.

Determine whether periodic reconciliation is necessary.

The final recommendation must explicitly address the fact that FileSystemWatcher is not a reliable permanent source of truth.

7. PHASE 5 — PROTOTYPE D: WINDOWS FILE OPERATIONS

Objective

Validate the safety and feasibility of the proposed file-management operations.

Use only dedicated temporary directories and synthetic test files.

Required operations

Test:

Copy.

Move.

Rename.

Delete to Windows Recycle Bin.

Restore from quarantine.

Restore from a simulated backup.

Destination conflict handling.

Interrupted copy handling.

Cross-volume copy, only if a suitable second volume is available.

Safety requirements

Never test against real user directories.

Never perform irreversible deletion.

Never modify Windows Recycle Bin contents outside the dedicated test scenario.

For cross-volume moves, validate the following sequence:

Copy source to a temporary destination.

Verify file size.

Verify SHA-256.

Publish the destination file.

Only then remove the source.

Verify the final state.

Investigate IFileOperation availability and the requirements of its COM STA execution model.

Record which metadata can be preserved and which cannot be guaranteed:

Timestamps.

Attributes.

ACLs.

Alternate Data Streams.

Other relevant filesystem metadata.

Deliverable

A test matrix:

Operation

Test result

Data integrity

Error handling

Limitations

8. SECURITY AND ARCHITECTURE REVIEW

Assess the proposed local web application architecture.

In particular:

Binding exclusively to 127.0.0.1.

Host and Origin validation.

CSRF protection.

Local session token.

SignalR/WebSocket protection.

No permissive CORS configuration.

Path traversal prevention.

Access restrictions to configured root directories.

Handling of symbolic links and junctions.

Protection against malicious or corrupted graphic files.

Thumbnail process isolation.

Rendering timeouts.

Memory and image-dimension limits.

Safe handling of external SVG resources.

Protection against unintended file operations.

Identify any architectural weaknesses and recommend concrete mitigations.

Do not implement security features.

9. DEPENDENCY AND LICENSING REVIEW

For every recommended external component, record:

Exact component name.

Version evaluated.

License type.

Official license URL.

Whether commercial use is permitted.

Redistribution requirements.

Required notices or attribution.

Native dependencies.

Runtime dependencies.

Known restrictions.

Pay particular attention to:

ImageMagick.

PDFium and its bundled components.

Svg.Skia.

Ghostscript.

SQLite.

.NET runtime.

Any alternative rendering engine discovered.

Do not classify a component as free for commercial redistribution without evidence.

Flag any dependency whose licensing implications remain uncertain.

10. FINAL ARCHITECTURE VALIDATION

Reassess the proposed architecture using actual findings.

For each component, assign one status:

APPROVED — validated and suitable.

APPROVED WITH CONDITIONS — usable subject to explicit limitations.

REQUIRES CHANGE — proposed approach should be replaced.

DEFERRED — not necessary for the initial release.

REJECTED — unsuitable for the project.

Produce a final architecture table covering:

.NET 10.

ASP.NET Core.

React / TypeScript / Vite.

SQLite / FTS5.

FileSystemWatcher.

Windows Shell thumbnails.

ImageMagick.

PDFium.

Svg.Skia.

INDD preview strategy.

AI preview strategy.

EPS preview strategy.

IFileOperation.

SignalR.

Portable self-contained deployment.

Do not retain a technology merely because it was proposed initially.

11. REQUIRED REPORT

Produce a detailed Markdown report named:

GraphiScope-Technical-Validation-Report.md

Save it only inside the dedicated temporary assessment directory.

The report must contain:

Executive summary.

Actual environment inventory.

Exact tools and versions detected.

Prototype execution conditions.

Prototype A results.

Prototype B results.

Prototype C results.

Prototype D results.

Security assessment.

Dependency and licensing assessment.

Final architecture recommendation.

Known limitations.

Unresolved questions.

Go / No-Go recommendation.

Recommended next steps before production development.

Evidence requirements

For every important conclusion, identify its evidence:

Exact command executed.

Relevant command output.

File or component inspected.

Test sample identifier.

Measured result.

Error message, if applicable.

Clearly label each statement as:

Verified by local execution.

Verified by local inspection.

Supported by official documentation.

Inferred.

Not verified.

Never fabricate test results or imply that an unexecuted test succeeded.

Include a concise summary table of all tested formats and their actual preview status.

12. FINAL EXECUTION SEQUENCE

Follow this order strictly:

Step 1 — Read-only inventory

Inspect the actual environment. Do not create or modify anything.

Step 2 — Inventory report

Present the detected environment, available tools, missing dependencies, and proposed prototype feasibility.

Step 3 — Prototype plan

Identify exactly which prototypes can run without installing software or modifying the system.

Identify missing samples and unavailable dependencies.

Step 4 — Isolated prototype execution

Only after Steps 1–3, execute the feasible prototypes in a dedicated temporary directory.

Step 5 — Final report

Produce the complete Markdown report with evidence and recommendations.

Step 6 — Stop

Do not start GraphiScope production development.

Do not create a production repository, solution, application, UI, database schema, or deployment package.

Wait for explicit user authorization before proceeding to any implementation phase.

13. FINAL RESPONSE FORMAT

At completion, report:

Temporary assessment directory.

Whether the initial environment inventory completed.

Which prototypes were actually executed.

Which prototypes could not be executed and why.

Report file location.

Most important technical findings.

Final GO / CONDITIONAL GO / NO-GO recommendation.

Any action that requires user authorization.

The success criterion is an evidence-based technical decision, not the quantity of code produced.
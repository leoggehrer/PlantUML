# Bedienungsanleitung für PlantUML.ConApp

## Projektbeschreibung

**PlantUML.ConApp** ist ein C#-Projekt, das verwendet wird, um UML-Diagramme zu erstellen. Entsprechend der Einstellungen können folgende UML-Diagramme erstellt werden:

- Klassendiagramm
- Aktivitätsdiagramm
- Sequenzdiagramm

## Voraussetzungen

- **.NET SDK**: Stellen Sie sicher, dass das .NET SDK installiert ist. Sie können es von der [offiziellen .NET-Website](https://dotnet.microsoft.com/download) herunterladen.
- **IDE**: Eine integrierte Entwicklungsumgebung (IDE) wie Visual Studio oder Visual Studio Code wird empfohlen.
- Installierte **Extensions** für Visual Studio Code:
  - C# Dev Kit
  - Markdown All in One
  - PlantUML

## Installation - GitHub-Repository

1. **Projekt-Repository klonen**:

   ```bash
   git clone https://github.com/leoggehrer/PlantUML.git
   ```

2. **In das Projektverzeichnis wechseln**:

   ```bash
   cd PlantUML.Watcher/PlantUML.Watcher
   ```

3. **Abhängigkeiten wiederherstellen**:

   ```bash
   dotnet restore
   ```

**Verwendung:**

1. **Projekt kompilieren und ausführen**:

   ```bash
   dotnet run
   ```

## Installation - Nuget-Packages

1. Erstellen einer Konsolenanwendung (--name kann frei gewählt werden).

   ```bash
   dotnet new console --framework net8.0 --use-program-main --name UMLCreator.ConApp
   ```

2. Anpassen der Projektdatei (UMLCreator.ConApp.csproj)

   ```xml
   <Project Sdk="Microsoft.NET.Sdk">

   <PropertyGroup>
      <OutputType>Exe</OutputType>
      <TargetFramework>net8.0</TargetFramework>
      <ImplicitUsings>enable</ImplicitUsings>
      <Nullable>enable</Nullable>
   </PropertyGroup>

   <ItemGroup>
      <PackageReference Include="PlantUML.Console" Version="1.0.0" />
   </ItemGroup>

   </Project>
   ```

3. Anpassen der Programmdatei (*Program.cs*)

   ```csharp
   namespace UMLCreator.ConApp;

   class Program
   {
      static void Main(string[] args)
      {
         var app = new PlantUML.ConApp.PlantUMLApp();

         app.Run(args);
      }
   }
   ```

**Verwendung:**

1. **In das Projektverzeichnis wechseln**:

   ```bash
   cd UMLCreator.ConApp
   ```

2. **Projekt kompilieren und ausführen**:

   ```bash
   dotnet run
   ```

## Verwendung von UMLCreator.ConApp oder PlantUML.ConApp

Nachdem das Programm gestartet ist, wird eine Konsole mit einem Info-Bereich, einem Aktions-Bereich und einem Auswahl-Bereich angezeigt. Die folgende Ausgabe zeigt die Konsole nach dem Start:

```bash
========
PlantUML
========

Projets path:     C:\Users\ggehr\source\repos\leoggehrer\2324-34_ABIF_ACIF_POSE
Target path:      C:\Users\ggehr\source\repos\leoggehrer\2324-34_ABIF_ACIF_POSE
Diagram folder:   diagrams
Diagram complete: True
Diagram builder:  All [All|Activity|Class|Sequence]

[-----] -----------------------------------------------------------------
[    1] False...............Change force flag
[    2] 1...................Change max sub path depth
[    3] 1...................Change max generation depth
[    4] 0...................Change the page index
[    5] 10..................Change the page size
[    6] Projects path.......Change projects path
[    7] Target path.........Change target path
[-----] -----------------------------------------------------------------
[    8] Create..............Change create complete diagram
[    9] Folder..............Change diagram folder
[   10] Builder.............Change diagram builder
[-----] -----------------------------------------------------------------
[   11] Path................\AddBigInteger.ConApp
[   12] Path................\BigInteger.ConApp
[   13] Path................\BinaryAdder.ConApp
[   14] Path................\BusinessCard.ConApp
[   15] Path................\BusinessRun.ConApp
[   16] Path................\Calculator.ConApp
[   17] Path................\CalculatorWithSwitch.ConApp
[   18] Path................\CaptainHook.ConApp
[   19] Path................\CashMaschine.ConApp
[   20] Path................\ClassManagement.ConApp
[-----] -----------------------------------------------------------------
[     ] 1..10/75
[-----] -----------------------------------------------------------------
[    +] Next................Load next page
[    -] Previous............Load previous page
[-----] -----------------------------------------------------------------
[  x|X] Exit................Exits the application

Choose [n|n,n|a...all|x|X]:
```

### Die wichtigsten Elemente der Ausgabe

Im oberen Bereich befindet sich der Info-Bereich und zeigt die Einstellungen der Anwendung an.

- *Project path*...gibt den Pfad an, in welchem die CSharp Projekte gesucht werden
- *Target path*...gibt den Pfad an, in welchem die Generierung der Diagramme erfolgt
- *Digram folder*...gibt den Namen des Ordners an, in welchem die Generierung der Diagramme erfolgt
- *Diagram complete*...zeigt an, dass aus den Teildiagrammen ein Gesamtdiagramm generiert wird
- *Diagram builder*...zeigt die Auswahl der Diagramme an

Im nachfolgenden Abschnitt können die Anwendungseinstellungen geändert werden.

- [ 1]...zeigt an oder legt fest, dass die Diagramme überschriben werden
- [ 2]...zeigt an oder legt fest, die Suchtiefe im '*Project path*' nach C# Projekten
- [ 3]...zeigt an oder legt fest, die Generierungstiefe im C# Projekt
- [ 6]...Änderung des '*Projects path*' (In diesem Verzeichnis werden C#-Projekte gesucht)
- [ 7]...Änderung des '*Target path*' (In diesem Verzeichnis werden die Diagramme generiert)
- [ 8]...legt fest, ob aus den Teildiagrammen ein Gesamtdiagramm erstellt wird
- [ 9]...legt den Namen des Ordners fest, in welchen die Generierung der Diagramme erfolgt
- [10]...legt fest, welche Diagrammtypen erstellt werden

Im nächsten Abschnitt kann die Generierung der Diagramme aktivit werden.

Wird zum Beispiel '**12**' (\BigInteger.ConApp) ausgewählt, dann wird der Quellcode in diesem Projekt ausgelesen. In vorliegenden Fall ist die Tief '**1**' (Generierungstiefe) angegeben. Das bedeuted, dass nur die Quelldateien vom Projekt-Ordner für die Diagrammgenerierung herangezogen werden (Unterverzeichnisse sind ausgeschlossen). Der Pfad in welchen die Diagramme abgelegt werden ist wie folgt aufgebaut:

- *Target path*/Project path/Digram folder

Im konkreten Fall ergibt sich der folgende Pfad:

- C:\Users\ggehr\source\repos\leoggehrer\2324-34_ABIF_ACIF_POSE\BigInteger.ConApp\diagrams

In diesem Pfad werden alle Diagramme entsprechend der Benennungskonvntionen abgelegt:

- Klassendiagramme
  - **cd_Klasse.puml**

- Aktivitätsdiagramme
  - **ac_Klasse_Methode.puml**
  
- Sequenzdiagramme
  - **sq_Klasse_Methode.puml**

Falls mehrere Überladung von Methoden des gleichen Namens existieren, wird die Konvention mit einer Nummerierung ergänzt (z.B.: **'Klasse_Methode_1.puml'**).


# Bedienungsanleitung für PlantUML.Watcher

## Projektbeschreibung

**PlantUML.Watcher** ist ein C#-Projekt, das verwendet wird, um UML-Diagramme automatisch zu erstellen und zu überwachen. Das Projekt überwacht die Änderungen in einem Verzeichnis (ein C# Projekt) und aktualisiert die UML-Diagramme entsprechend. Dadurch kann schon während der Entwicklung der Quellcode mit den UML-Diagrammen dokumentiert werden. Entsprechend der Einstellungen können folgende UML-Diagramme erstellt werden:

- Klassendiagramm
- Aktivitätsdiagramm
- Sequenzdiagramm

## Voraussetzungen

- **.NET SDK**: Stellen Sie sicher, dass das .NET SDK installiert ist. Sie können es von der [offiziellen .NET-Website](https://dotnet.microsoft.com/download) herunterladen.
- **IDE**: Eine integrierte Entwicklungsumgebung (IDE) wie Visual Studio oder Visual Studio Code wird empfohlen.

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
    dotnet new console --framework net8.0 --use-program-main --name UMLWatcher.ConApp
    ```

2. Anpassen der Projektdatei (UMLWatcher.ConApp.csproj)

   ```xml
   <Project Sdk="Microsoft.NET.Sdk">

   <PropertyGroup>
      <OutputType>Exe</OutputType>
      <TargetFramework>net8.0</TargetFramework>
      <ImplicitUsings>enable</ImplicitUsings>
      <Nullable>enable</Nullable>
   </PropertyGroup>

   <ItemGroup>
      <PackageReference Include="PlantUML.LiveGeneration" Version="1.0.1" />
   </ItemGroup>

   </Project>
   ```

3. Anpassen der Programmdatei (*Program.cs*)

   ```csharp
   namespace UMLWatcher.ConApp;

   class Program
   {
      static void Main(string[] args)
      {
         var app = new PlantUML.Watcher.UMLWatcherApp();

         app.Run(args);
      }
   }
   ```

**Verwendung:**

1. **In das Projektverzeichnis wechseln**:

   ```bash
   cd UMLWatcher.ConApp
   ```

2. **Projekt kompilieren und ausführen**:

   ```bash
   dotnet run
   ```

## Programmeinstellungen


3. **Diagramm-Erstellung**:

   Die Diagramme werden automatisch erstellt und aktualisiert, wenn Änderungen im überwachten Verzeichnis erkannt werden. Es werden verschiedene Diagrammtypen wie Aktivitätsdiagramme, Klassendiagramme und Sequenzdiagramme unterstützt.

## Bekannte Probleme und Lösungen

- **Problem**: Das Projekt kompiliert nicht.
  **Lösung**: Stellen Sie sicher, dass alle Abhängigkeiten korrekt installiert sind und dass Sie die richtige .NET SDK-Version verwenden.

- **Problem**: UML-Diagramme werden nicht aktualisiert.
  **Lösung**: Überprüfen Sie, ob das überwachte Verzeichnis korrekt angegeben ist und ob Schreibberechtigungen vorhanden sind.

## Dateien und Struktur

- **ActivityDiagramBuilder.cs**: Enthält die Logik zur Erstellung von Aktivitätsdiagrammen.
- **ClassDiagramBuilder.cs**: Enthält die Logik zur Erstellung von Klassendiagrammen.
- **DiagramBuilderType.cs**: Definiert die verschiedenen Typen von Diagrammerstellern.
- **FolderWatcher.cs**: Überwacht das Verzeichnis auf Änderungen.
- **Program.cs**: Der Einstiegspunkt des Programms.
- **SequenceDiagramBuilder.cs**: Enthält die Logik zur Erstellung von Sequenzdiagrammen.
- **UMLDiagramBuilder.cs**: Basisklasse für die verschiedenen Diagrammersteller.
- **UMLWatcher.cs**: Überwacht die Dateien und erstellt UML-Diagramme.
- **UMLWatcherApp.cs**: Anwendungskonfiguration und -start.

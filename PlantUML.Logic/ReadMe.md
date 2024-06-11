# Bedienungsanleitung für PlantUML.Logic

## Projektbeschreibung

**PlantUML.Logic** ist ein C#-Projekt (Klassenbiliothek), das verwendet wird, um UML-Diagramme zu erstellen. Für die Generierung der Diagramme wird der entsprechende C#-Quellcode verwendet. Das System kann folgende UML-Diagramme generieren:

- Klassendiagramm
- Aktivitätsdiagramm
- Sequenzdiagramm
- Objektdiagramm

## Voraussetzungen

- **.NET SDK**: Stellen Sie sicher, dass das .NET SDK installiert ist. Sie können es von der [offiziellen .NET-Website](https://dotnet.microsoft.com/download) herunterladen.
- **IDE**: Eine integrierte Entwicklungsumgebung (IDE) wie Visual Studio oder Visual Studio Code wird empfohlen.
- Installierte **Extensions** für Visual Studio Code:
  - C# Dev Kit
  - Markdown All in One
  - PlantUML

## Installation - Nuget-Packages

1. Erstellen einer Konsolenanwendung (--name kann frei gewählt werden).

   ```bash
   dotnet new console --framework net8.0 --use-program-main --name PlantUML.ObjectDiagramSample
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
      <PackageReference Include="PlantUML.Creator" Version="1.0.2" />
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
2. **Projekt kompilieren und ausf�hren**:

   ```bash
   dotnet run
   ```

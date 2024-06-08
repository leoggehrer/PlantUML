# Bedienungsanleitung für PlantUML.Watcher

## Projektbeschreibung

**PlantUML.Watcher** ist ein C#-Projekt, das verwendet wird, um UML-Diagramme automatisch zu erstellen und zu überwachen. Das Projekt überwacht die Änderungen in einem Verzeichnis (ein C# Projekt) und aktualisiert die UML-Diagramme entsprechend. Dadurch kann schon während der Entwicklung der Quellcode mit den UML-Diagrammen dokumentiert werden. Entsprechend der Einstellungen können folgende UML-Diagramme erstellt werden:

- Klassendiagramm
- Aktivitätsdiagramm
- Sequenzdiagramm

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
      <PackageReference Include="PlantUML.LiveGeneration" Version="1.0.2" />
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

## Verwendung von PlantUML.Watcher

Eine detaillierte Beschreibung über die Verwendung des Packages finden Sie unter dem Link:

https://github.com/leoggehrer/PlantUML/tree/master/PlantUML.Watcher

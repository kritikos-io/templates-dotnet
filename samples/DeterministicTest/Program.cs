using System.Reflection;
using System.Reflection.Metadata;

var assembly = typeof(Program).Assembly;

Console.WriteLine("=== Deterministic Build Check ===");
Console.WriteLine();

var name = assembly.GetName();
Console.WriteLine($"Assembly:      {name.Name}");
Console.WriteLine($"Version:       {name.Version}");

var infoVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
Console.WriteLine($"Info Version:  {infoVersion?.InformationalVersion}");

var fileVersion = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
Console.WriteLine($"File Version:  {fileVersion?.Version}");

var mvid = assembly.ManifestModule.ModuleVersionId;
Console.WriteLine($"MVID:          {mvid}");

Console.WriteLine();
Console.WriteLine("=== DeterministicSourcePaths Check ===");
Console.WriteLine();

var assemblyPath = assembly.Location;
if (!string.IsNullOrEmpty(assemblyPath))
{
  var pdbPath = Path.ChangeExtension(assemblyPath, ".pdb");
  if (File.Exists(pdbPath))
  {
    using var pdbStream = File.OpenRead(pdbPath);
    using var pdbProvider = MetadataReaderProvider.FromPortablePdbStream(pdbStream);
    var pdbReader = pdbProvider.GetMetadataReader();

    Console.WriteLine("Source paths found in PDB:");
    foreach (var docHandle in pdbReader.Documents)
    {
      var doc = pdbReader.GetDocument(docHandle);
      var docName = pdbReader.GetString(doc.Name);
      Console.WriteLine($"  {docName}");
    }

    Console.WriteLine();
    Console.WriteLine("If DeterministicSourcePaths=true, paths will be mapped");
    Console.WriteLine("(e.g. /_/src/...) instead of showing full local paths.");
  }
  else
  {
    Console.WriteLine($"PDB not found at: {pdbPath}");
    Console.WriteLine("Build with debug symbols to check source paths.");
  }
}
else
{
  Console.WriteLine("Assembly location not available (single-file publish?).");
}

Console.WriteLine();
Console.WriteLine("--- Instructions ---");
Console.WriteLine("1. Build twice without changes, compare MVID for Deterministic.");
Console.WriteLine("2. Check PDB source paths for DeterministicSourcePaths.");

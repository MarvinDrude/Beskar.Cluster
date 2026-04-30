using Beskar.Cluster.Markdown.Models;

Console.WriteLine("Hello, World!");

var res = MarkdownLexResult.Create(
   """
   Hallo
   ---
   > Welt
   > A
   
   Wie geht es dir?
   
   ## s
   ### a
   
   1. s
   2. s
   
   **Bold**
   """);
var debugString = res.CreateDebugString();

_ = "";
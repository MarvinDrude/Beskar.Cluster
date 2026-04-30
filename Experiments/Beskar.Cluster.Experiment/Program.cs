using Beskar.Cluster.Markdown.Models;

Console.WriteLine("Hello, World!");

var res = MarkdownLexResult.Create(
   """
   Hallo
   ---
   > Welt
   > A
   
   Hier `aaa` toll?
   
   Wie geht es dir?
   
   ```csharp
   class Program 
   {
   }
   ```
   
   ## s
   ### a
   
   1. s
   2. s
   
   **Bold**
   """);
var debugString = res.CreateDebugString();

var html = res.CreateHtml(new object());

_ = "";
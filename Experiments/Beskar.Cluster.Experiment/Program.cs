using Beskar.Cluster.Markdown.Models;

Console.WriteLine("Hello, World!");

var res = MarkdownLexResult.Create(
   """
   ---
   # Header with **Bold** and `Code`
   
   #### Subheader > a `a` **b**
   
   > Blockquote start
   > > Nested Blockquote
   > > 1. List inside quote
   > > 2. Second item
   > Back to first level
   
   - aa
   - dsa
   
   * aaaaa
   * bbb
   
   ```typescript
   // Testing your new CodeBlock logic
   const x = "No **Markdown** parsing here";
   ```
   1. Item with nested list
   2. Item with [Link](https://example.com)
   
   Text with *Italic* and **Bold** and ***Both***.
   An empty block follows:
   
   > 
   
   ## Mixed Inlines [Link] `Code` **Bold**
   
   ---
   """);
var debugString = res.CreateDebugString();
var astResult = MarkdownAstResult.Create(res);
var astDebugString = astResult.CreateDebugString();


_ = "";
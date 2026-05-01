using Beskar.Cluster.Markdown.Parsing;
using Me.Memory.Buffers;

namespace Beskar.Cluster.Markdown.Models;

public sealed class MarkdownAstResult
{
   public required string RawText { get; init; }
   public required MarkdownNode[] Nodes { get; init; }

   public string CreateDebugString()
   {
      var writer = new TextWriterIndentSlim(stackalloc char[512], stackalloc char[64]);
      try
      {
         RenderNode(ref writer, 0);
         return writer.ToString();
      }
      finally
      {
         writer.Dispose();
      }
   }
   
   private void RenderNode(ref TextWriterIndentSlim writer, int nodeIndex)
   {
      if (nodeIndex == -1 || nodeIndex >= Nodes.Length) return;
      var node = Nodes[nodeIndex];
   
      var textSnippet = RawText.AsSpan(node.Position.Index, node.Position.Length).ToString()
         .Replace("\r\n", "\\n").Replace("\n", "\\n");

      writer.WriteLineInterpolated($"└─ [{nodeIndex}] {node.Type} (\"{textSnippet}\")");

      if (node.FirstChildIndex != -1)
      {
         writer.UpIndent();
         RenderNode(ref writer, node.FirstChildIndex);
         writer.DownIndent();
      }

      if (node.NextSiblingIndex != -1)
      {
         RenderNode(ref writer, node.NextSiblingIndex);
      }
   }
   
   public static MarkdownAstResult Create(MarkdownLexResult result)
   {
      var builder = new MarkdownAstBuilder(result.RawText, result.Tokens.AsSpan(), stackalloc MarkdownNode[64]);
      try
      {
         builder.Build();
         return new MarkdownAstResult
         {
            RawText = result.RawText, 
            Nodes = builder.WrittenNodes.ToArray()
         };
      }
      finally
      {
         builder.Dispose();
      }
   }
}
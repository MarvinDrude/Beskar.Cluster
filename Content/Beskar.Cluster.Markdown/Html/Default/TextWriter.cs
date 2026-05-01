using Beskar.Cluster.Markdown.Enums;
using Beskar.Cluster.Markdown.Html.Interfaces;
using Beskar.Cluster.Markdown.Parsing;
using Me.Memory.Buffers;

namespace Beskar.Cluster.Markdown.Html.Default;

internal sealed class TextWriter<TContext> : IMarkdownTextWriter<TContext>
{
   public bool IsParagraphOpen { get; set; }

   public void Write(
      MarkdownHtmlContext<TContext> host, 
      ref MarkdownReader reader, 
      ref TextWriterIndentSlim writer)
   {
      if (host.BlockType is MarkdownTokenType.None && !IsParagraphOpen)
      {
         writer.WriteLine("<p>");
         IsParagraphOpen = true;
      }
      
      var content = reader.Value;
      if (content.IsEmpty) return;
      
      writer.Write(content);
   }

   public void Clear()
   {
      IsParagraphOpen = false;
   }
}
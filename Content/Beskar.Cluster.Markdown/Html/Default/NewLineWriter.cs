using Beskar.Cluster.Markdown.Enums;
using Beskar.Cluster.Markdown.Html.Interfaces;
using Beskar.Cluster.Markdown.Parsing;
using Me.Memory.Buffers;

namespace Beskar.Cluster.Markdown.Html.Default;

internal sealed class NewLineWriter<TContext> : IMarkdownHtmlWriter<TContext>
{
   public void Write(MarkdownHtmlContext<TContext> host, ref MarkdownReader reader, ref TextWriterIndentSlim writer)
   {
      var textWriter = host.Html.TextWriter;
      if (textWriter.IsParagraphOpen && reader.Peek().Type == MarkdownTokenType.NewLine)
      {
         writer.WriteLine("</p>");
         textWriter.IsParagraphOpen = false;
         reader.Read();
      }
      
      if (host.BlockType is MarkdownTokenType.None)
      {
         writer.Write("<br/>\n");
      }
      else
      {
         writer.WriteLine("\n");
      }
   }
   
   public void Clear()
   {
      
   }
}
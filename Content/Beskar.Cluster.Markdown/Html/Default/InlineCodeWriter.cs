using Beskar.Cluster.Markdown.Enums;
using Beskar.Cluster.Markdown.Html.Interfaces;
using Beskar.Cluster.Markdown.Parsing;
using Me.Memory.Buffers;

namespace Beskar.Cluster.Markdown.Html.Default;

internal sealed class InlineCodeWriter<TContext> : IMarkdownHtmlWriter<TContext>
{
   public void Write(MarkdownHtmlContext<TContext> host, ref MarkdownReader reader, ref TextWriterIndentSlim writer)
   {
      writer.Write("<code>");
      if (reader.Read() && reader.Current.Type == MarkdownTokenType.Text)
      {
         writer.Write(reader.Value);
      }
      
      if (reader.Read() && reader.Current.Type == MarkdownTokenType.InlineCode)
      {
         writer.Write("</code>");
      }
   }
   
   public void Clear()
   {
      
   }
}
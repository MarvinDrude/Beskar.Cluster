using Beskar.Cluster.Markdown.Enums;
using Beskar.Cluster.Markdown.Extensions;
using Beskar.Cluster.Markdown.Html.Interfaces;
using Beskar.Cluster.Markdown.Parsing;
using Me.Memory.Buffers;

namespace Beskar.Cluster.Markdown.Html.Default;

internal sealed class CodeBlockWriter<TContext> : IMarkdownHtmlWriter<TContext>
{
   public void Write(MarkdownHtmlContext<TContext> host, ref MarkdownReader reader, ref TextWriterIndentSlim writer)
   {
      writer.Write("<pre><code>");

      while (reader.Read())
      {
         if (reader.Current.Type == MarkdownTokenType.CodeBlock) break;
         if (reader.Current.Type == MarkdownTokenType.EndOfFile) break;

         if (reader.Current.Type == MarkdownTokenType.Text)
         {
            reader.Value.WriteHtmlEncoded(ref writer);
         }
         else if (reader.Current.Type == MarkdownTokenType.NewLine)
         {
            writer.WriteLine();
         }
         else
         {
            writer.Write(reader.Value);
         }
      }

      writer.WriteLine("</code></pre>");
   }

   public void Clear()
   {
      
   }
}
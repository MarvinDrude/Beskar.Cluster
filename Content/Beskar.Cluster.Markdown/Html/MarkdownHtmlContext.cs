using Beskar.Cluster.Markdown.Enums;

namespace Beskar.Cluster.Markdown.Html;

public sealed class MarkdownHtmlContext<TContext>
{
   public required MarkdownHtml<TContext> Html { get; init; }
   
   public required TContext Context { get; init; }

   internal BlockType BlockType { get; set; } = BlockType.None;
}
namespace Beskar.Cluster.Markdown.Html;

public sealed class MarkdownHtmlContext<TContext>
{
   public required MarkdownHtml<TContext> Html { get; init; }
   
   public required TContext Context { get; init; }
}
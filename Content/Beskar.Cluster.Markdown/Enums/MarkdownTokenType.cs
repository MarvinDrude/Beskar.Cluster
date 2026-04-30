namespace Beskar.Cluster.Markdown.Enums;

public enum MarkdownTokenType : byte
{
   // Meta
   None = 1,
   EndOfFile,
   NewLine,
   
   // Blocks
   Heading1,
   Heading2,
   Heading3,
   Heading4,
   Heading5,
   Heading6,
   BlockQuote,
   CodeBlock,
   UnorderedList,
   OrderedList,
   HorizontalRule,
   
   // Inlines
   Text,
   Bold,
   Italic,
   Strikethrough,
   InlineCode,
   LinkOpen,  // [
   LinkClose, // ]
   UrlOpen,   // (
   UrlClose,  // )
   ImageOpen, // !
   
   // Custom
   CustomTagOpen,
   CustomTagClose,
}
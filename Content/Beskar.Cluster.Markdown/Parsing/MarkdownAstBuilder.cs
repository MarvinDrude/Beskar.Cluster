using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Beskar.Cluster.Markdown.Enums;
using Beskar.Cluster.Markdown.Models;
using Me.Memory.Buffers;

namespace Beskar.Cluster.Markdown.Parsing;

[StructLayout(LayoutKind.Auto)]
public ref struct MarkdownAstBuilder(
   ReadOnlySpan<char> rawMarkdownText,
   ReadOnlySpan<MarkdownToken> tokens,
   Span<MarkdownNode> initialNodeBuffer)
   : IDisposable
{
   public ReadOnlySpan<MarkdownNode> WrittenNodes => _nodes.WrittenSpan;
   
   private MarkdownReader _reader = new(tokens, rawMarkdownText);
   private BufferWriter<MarkdownNode> _nodes = new(initialNodeBuffer);
   
   public void Build()
   {
      var lastRootIndex = -1;
      
      while (_reader.Read())
      {
         if (_reader.Current.Type is MarkdownTokenType.NewLine) continue;
         if (_reader.Current.Type is MarkdownTokenType.EndOfFile) break;

         var currentIndex = ParseBlock();
         if (lastRootIndex != -1)
         {
            UpdateNextSibling(lastRootIndex, currentIndex);
         }
         
         lastRootIndex = currentIndex;
      }
   }

   private int ParseBlock()
   {
      var currentToken = _reader.Current;
      var blockType = currentToken.Type;
      
      switch (currentToken.Type)
      {
         case MarkdownTokenType.OrderedList 
           or MarkdownTokenType.UnorderedList:
            return ParseList();
         case MarkdownTokenType.BlockQuote:
            return ParseBlockQuote();
         case MarkdownTokenType.CodeBlock:
            return ParseCodeBlock();
      }

      if (IsParagraphPromotionReady(blockType))
      {
         var paragraphIndex = AddNode(MarkdownTokenType.Paragraph, currentToken.ValuePostion);
         var textChildIndex = AddNode(blockType, currentToken.ValuePostion);
         
         UpdateFirstChild(paragraphIndex, textChildIndex);
         
         if (IsNestingToken(blockType))
         {
            ParseInlines(textChildIndex, GetClosingToken(blockType));
         }
         
         ParseInlines(paragraphIndex, MarkdownTokenType.None, textChildIndex);
         return paragraphIndex;
      }

      var nodeIndex = AddNode(blockType, currentToken.ValuePostion);

      if (IsInlineContainer(blockType))
      {
         ParseInlines(nodeIndex, GetClosingToken(blockType));
      }

      return nodeIndex;
   }

   private void ParseInlines(int parentIndex, 
      MarkdownTokenType closingType = MarkdownTokenType.None, 
      int lastInlineIndex = -1)
   {
      var lastInline = lastInlineIndex;

      while (true)
      {
         var nextType = _reader.Peek().Type;
         
         if (nextType is MarkdownTokenType.NewLine or MarkdownTokenType.EndOfFile)
         {
            break;
         }

         if (closingType is not MarkdownTokenType.None && nextType == closingType)
         {
            _reader.Read();
            return; 
         }
         
         _reader.Read();
         
         var token = _reader.Current;
         var currentIndex = AddNode(token.Type, token.ValuePostion);
         LinkChild(parentIndex, ref lastInline, currentIndex);

         if (IsNestingToken(token.Type))
         {
            var closer = GetClosingToken(token.Type);
            ParseInlines(currentIndex, closer);
         }
      }
   }

   private int ParseList()
   {
      var listToken = _reader.Current;
      var listNodeIndex = AddNode(listToken.Type, listToken.ValuePostion);
      var lastItemIndex = -1;

      while (true)
      {
         var itemNodeIndex = AddNode(MarkdownTokenType.ListItem, _reader.Current.ValuePostion);
         LinkChild(listNodeIndex, ref lastItemIndex, itemNodeIndex);
         
         var lastChildInsideItem = -1;
         
         ParseInlines(itemNodeIndex, MarkdownTokenType.None, lastChildInsideItem);
         lastChildInsideItem = GetLastChildIndex(itemNodeIndex);
         
         var nextToken = _reader.Peek();
         if (IsIndented(nextToken)) 
         {
            _reader.Read();
            var nestedListIndex = ParseList();
            LinkChild(itemNodeIndex, ref lastChildInsideItem, nestedListIndex);
         }
         
         while (_reader.Peek().Type is MarkdownTokenType.NewLine) 
            _reader.Read();
         
         if (_reader.Peek().Type != listToken.Type)
            break;
         
         _reader.Read();
      }
      
      return listNodeIndex;
   }
   
   private int GetLastChildIndex(int parentIndex)
   {
      var node = _nodes.WrittenSpan[parentIndex];
      var current = node.FirstChildIndex;
      if (current == -1) return -1;

      while (_nodes.WrittenSpan[current].NextSiblingIndex != -1)
      {
         current = _nodes.WrittenSpan[current].NextSiblingIndex;
      }
      return current;
   }
   
   private int ParseBlockQuote()
   {
      var firstToken = _reader.Current;
      var quoteIndex = AddNode(MarkdownTokenType.BlockQuote, firstToken.ValuePostion);
      var lastChildIndex = -1;

      while (true)
      {
         _reader.Read();
         var childIndex = ParseBlock();
         LinkChild(quoteIndex, ref lastChildIndex, childIndex);

         while (_reader.Peek().Type is MarkdownTokenType.NewLine) 
            _reader.Read();

         if (_reader.Peek().Type != MarkdownTokenType.BlockQuote)
            break;
         
         _reader.Read();
      }

      return quoteIndex;
   }

   private int ParseCodeBlock()
   {
      var openingToken = _reader.Current;
      var codeBlockIndex = AddNode(MarkdownTokenType.CodeBlock, openingToken.ValuePostion);
      
      var span = _reader.Value;
      
      var firstNewLine = span.IndexOf('\n');
      var lastBackticks = span.LastIndexOf("```");
      
      var lastChildIndex = -1;
      var firstLine = span[..firstNewLine].Trim(['`', ' ', '\r']);
      
      if (!firstLine.IsEmpty)
      {
         var langNode = AddNode(MarkdownTokenType.Metadata, new RawMarkdownPosition 
         { 
            Index = openingToken.ValuePostion.Index + 3, 
            Length = firstLine.Length 
         });
         
         LinkChild(codeBlockIndex, ref lastChildIndex, langNode);
      }

      var contentStart = firstNewLine + 1;
      var contentLength = lastBackticks - contentStart;
      
      var contentNode = AddNode(MarkdownTokenType.Text, new RawMarkdownPosition 
      { 
         Index = openingToken.ValuePostion.Index + contentStart,
         Length = contentLength 
      });
      
      LinkChild(codeBlockIndex, ref lastChildIndex, contentNode);
      return codeBlockIndex;
   }
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   private int AddNode(MarkdownTokenType type, RawMarkdownPosition pos)
   {
      var index = _nodes.WrittenSpan.Length;
      _nodes.Add(new MarkdownNode 
      { 
         Type = type, 
         Position = pos, 
         FirstChildIndex = -1, 
         NextSiblingIndex = -1 
      });
      
      return index;
   }
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   private void UpdateFirstChild(int parentIndex, int childIndex)
   {
      ref var node = ref MemoryMarshal.GetReference(_nodes.WrittenSpan.Slice(parentIndex, 1));
      node.FirstChildIndex = childIndex;
   }
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   private void UpdateNextSibling(int previousIndex, int siblingIndex)
   {
      ref var node = ref MemoryMarshal.GetReference(_nodes.WrittenSpan.Slice(previousIndex, 1));
      node.NextSiblingIndex = siblingIndex;
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   private bool IsClosingTag(MarkdownTokenType expectedType)
   {
      var current = _reader.Current.Type;
      if (current == expectedType)
         return true;

      if (current is MarkdownTokenType.NewLine or MarkdownTokenType.EndOfFile)
         return true;

      return false;
   }

   public void Dispose()
   {
      _nodes.Dispose();
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   private static bool IsBlockTerminator(MarkdownTokenType type)
   {
      return type switch
      {
         MarkdownTokenType.NewLine => true,
         MarkdownTokenType.EndOfFile => true,

         MarkdownTokenType.HorizontalRule => true,
         MarkdownTokenType.Heading1 => true,
         MarkdownTokenType.Heading2 => true,
         MarkdownTokenType.Heading3 => true,
         MarkdownTokenType.Heading4 => true,
         MarkdownTokenType.Heading5 => true,
         MarkdownTokenType.Heading6 => true,

         MarkdownTokenType.CodeBlock => true,
         MarkdownTokenType.OrderedList => true,
         MarkdownTokenType.UnorderedList => true,

         _ => false
      };
   }
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   private bool IsIndented(MarkdownToken token)
   {
      if (token.Type is MarkdownTokenType.EndOfFile) return false;

      var span = _reader.Source;
      var index = token.ValuePostion.Index;
   
      var spaceCount = 0;
      for (var i = index - 1; i >= 0; i--)
      {
         if (span[i] == ' ') spaceCount++;
         else if (span[i] == '\n' || span[i] == '\r') break;
         else return false;
      }

      return spaceCount >= 3;
   }
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   private static MarkdownTokenType GetClosingToken(MarkdownTokenType type) =>
      type switch
      {
         MarkdownTokenType.LinkOpen => MarkdownTokenType.LinkClose,
         MarkdownTokenType.CustomTagOpen => MarkdownTokenType.CustomTagClose,
         _ => type
      };
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   private static bool IsNestingToken(MarkdownTokenType type) =>
      type is MarkdownTokenType.Bold or MarkdownTokenType.Italic 
         or MarkdownTokenType.Strikethrough or MarkdownTokenType.LinkOpen 
         or MarkdownTokenType.CustomTagOpen or MarkdownTokenType.InlineCode;
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   private void LinkChild(int parentIndex, ref int lastChildIndex, int currentChildIndex)
   {
      if (lastChildIndex == -1)
      {
         UpdateFirstChild(parentIndex, currentChildIndex);
      }
      else
      {
         UpdateNextSibling(lastChildIndex, currentChildIndex);
      }
      
      lastChildIndex = currentChildIndex;
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   private static bool IsParagraphPromotionReady(MarkdownTokenType type) =>
      type is MarkdownTokenType.Text or MarkdownTokenType.Bold
         or MarkdownTokenType.Italic or MarkdownTokenType.Strikethrough
         or MarkdownTokenType.InlineCode;
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   private static bool IsInlineContainer(MarkdownTokenType type) =>
      type is MarkdownTokenType.Paragraph 
         or MarkdownTokenType.Bold 
         or MarkdownTokenType.Italic 
         or MarkdownTokenType.Strikethrough
         or MarkdownTokenType.InlineCode
         or MarkdownTokenType.LinkOpen 
         or MarkdownTokenType.Heading1 
         or MarkdownTokenType.Heading2 
         or MarkdownTokenType.Heading3 
         or MarkdownTokenType.Heading4 
         or MarkdownTokenType.Heading5 
         or MarkdownTokenType.Heading6;
}
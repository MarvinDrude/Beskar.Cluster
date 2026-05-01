using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Beskar.Cluster.Markdown.Enums;
using Beskar.Cluster.Markdown.Models;
using Me.Memory.Buffers;

namespace Beskar.Cluster.Markdown.Parsing;

[StructLayout(LayoutKind.Auto)]
public ref struct MarkdownLexer(
   ReadOnlySpan<char> rawMarkdownText,
   Span<MarkdownToken> initialTokenBuffer)
   : IDisposable
{
   public ReadOnlySpan<MarkdownToken> WrittenTokens 
      => _tokenWriter.WrittenSpan;
   
   private readonly ReadOnlySpan<char> _rawInput = rawMarkdownText;

   private BufferWriter<MarkdownToken> _tokenWriter = new(initialTokenBuffer);
   private int _currentPosition = 0;

   public void ScanTokens()
   {
      while (_currentPosition < _rawInput.Length)
      {
         var cha = _rawInput[_currentPosition];
         var start = _currentPosition;

         var isStartOfLine = start == 0 || 
           _rawInput[start - 1] == '\n' || 
           _rawInput[start - 1] == '\r';
         
         switch (cha)
         {
            case '\r':
               if (Peek(1) == '\n')
               {
                  _currentPosition += 2;
                  AddToken(MarkdownTokenType.NewLine, start);
               }
               else
               {
                  _currentPosition++;
                  HandleText(start);
               }
               continue;
            
            case '\n':
               _currentPosition++;
               AddToken(MarkdownTokenType.NewLine, start);
               continue;
            
            case '*':
               if (Peek(1) == '*')
               {
                  _currentPosition += 2;
                  AddToken(MarkdownTokenType.Bold, start);
               }
               else
               {
                  _currentPosition++;
                  AddToken(MarkdownTokenType.Italic, start);
               }
               continue;
            
            case '#' when isStartOfLine:
               HandleHeading(start);
               continue;
            
            case '>' when isStartOfLine:
               _currentPosition++;
               if (Peek(0) == ' ') _currentPosition++;
               AddToken(MarkdownTokenType.BlockQuote, start);
               continue;
            
            case '`':
               if (Peek(1) == '`' && Peek(2) == '`') HandleCodeBlock(start);
               else { _currentPosition++; AddToken(MarkdownTokenType.InlineCode, start); }
               continue;
            
            case '-':
               if (isStartOfLine && Peek(1) == '-' && Peek(2) == '-') HandleHorizontalRule(start);
               else if (isStartOfLine && Peek(1) == ' ') { _currentPosition++; AddToken(MarkdownTokenType.UnorderedList, start); }
               else HandleText(start); // Treat as normal text if not a marker
               continue;
               
            case '[':
               if (Peek(1) == '[') HandleCustomTag(start, true);
               else { _currentPosition++; AddToken(MarkdownTokenType.LinkOpen, start); }
               continue;
            
            case ']':
               if (Peek(1) == ']') HandleCustomTag(start, false);
               else { _currentPosition++; AddToken(MarkdownTokenType.LinkClose, start); }
               continue;
            
            case '~':
               if (Peek(1) == '~') { _currentPosition += 2; AddToken(MarkdownTokenType.Strikethrough, start); }
               else HandleText(start);
               continue;
            
            case '!':
               _currentPosition++;
               AddToken(MarkdownTokenType.ImageOpen, start);
               continue;
            
            case '(':
               _currentPosition++;
               AddToken(MarkdownTokenType.UrlOpen, start);
               continue;
            
            case ')':
               _currentPosition++;
               AddToken(MarkdownTokenType.UrlClose, start);
               continue;
            
            case '0': case '1': case '2': case '3': case '4':
            case '5': case '6': case '7': case '8': case '9':
               if (isStartOfLine) HandleOrderedList(start);
               else HandleText(start);
               continue;
            
            default:
               var prevPos = _currentPosition;
               HandleText(start);
               if (_currentPosition == prevPos) // safeguard against endless loop
               {
                  _currentPosition++;
                  AddToken(MarkdownTokenType.Text, start);
               }
               continue;
         }
      }
      
      AddToken(MarkdownTokenType.EndOfFile, _currentPosition);
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   private void AddToken(MarkdownTokenType type, int start)
   {
      var length = _currentPosition - start;
      var position = new RawMarkdownPosition(start, length);
      
      _tokenWriter.Add(new MarkdownToken(type, position));
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   private char Peek(int offset)
   {
      return _currentPosition + offset < _rawInput.Length
         ? _rawInput[_currentPosition + offset]
         : '\0';
   }

   private void HandleHeading(int start)
   {
      var count = 0;
      while (Peek(count) == '#' && count < 6) 
         count++;
   
      if (Peek(count) == ' ')
      {
         _currentPosition += count + 1; 
         var type = (MarkdownTokenType)((int)MarkdownTokenType.Heading1 + (count - 1));
         AddToken(type, start);
      }
      else
      {
         HandleText(start); // Fallback to text if no space
      }
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   private void HandleText(int start)
   {
      if (_currentPosition < _rawInput.Length 
          && IsControlChar(_rawInput[_currentPosition]))
      {
         // make sure
         _currentPosition++;
      }
      
      while (_currentPosition < _rawInput.Length)
      {
         if (IsControlChar(_rawInput[_currentPosition]))
         {
            break;
         }
      
         _currentPosition++;
      }
   
      AddToken(MarkdownTokenType.Text, start);
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   private void HandleCodeBlock(int start)
   {
      _currentPosition += 3;
      
      while (_currentPosition < _rawInput.Length)
      {
         if (IsClosingCodeBlock())
         {
            _currentPosition += 3;
            break;
         }
         _currentPosition++;
      }
      
      AddToken(MarkdownTokenType.CodeBlock, start);
   }
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   private bool IsClosingCodeBlock()
   {
      var isStartOfLine = _currentPosition > 0 
         && (_rawInput[_currentPosition - 1] == '\n' || _rawInput[_currentPosition - 1] == '\r');
   
      return isStartOfLine && 
             Peek(0) == '`' && 
             Peek(1) == '`' && 
             Peek(2) == '`';
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   private void HandleCustomTag(int start, bool isOpen)
   {
      _currentPosition += 2; // Skip [[ or ]]
      AddToken(isOpen ? MarkdownTokenType.CustomTagOpen : MarkdownTokenType.CustomTagClose, start);
   }
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   private void HandleHorizontalRule(int start)
   {
      _currentPosition += 3;
      while (Peek(0) == '-') _currentPosition++; // Consume extra dashes
      AddToken(MarkdownTokenType.HorizontalRule, start);
   }
   
   private void HandleOrderedList(int start)
   {
      var peekOffset = 0;
      while (char.IsAsciiDigit(Peek(peekOffset)))
      {
         peekOffset++;
      }

      if (Peek(peekOffset) == '.' && Peek(peekOffset + 1) == ' ')
      {
         _currentPosition += peekOffset + 1;
         AddToken(MarkdownTokenType.OrderedList, start);
      
         if (Peek(0) == ' ') _currentPosition++;
      }
      else
      {
         HandleText(start);
      }
   }
   
   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   private static bool IsControlChar(char cha)
   {
      return cha switch
      {
         '#' or '*' or '_' or '[' or ']' or '(' or ')' or '!' or '`' or '>' or '\n' or '\r' => true,
         _ => false
      };
   }

   public void Dispose()
   {
      _tokenWriter.Dispose();   
   }
}
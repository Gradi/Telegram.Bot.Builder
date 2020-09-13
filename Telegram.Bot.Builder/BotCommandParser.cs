using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Builder
{
    internal class BotCommandParser
    {
        private readonly string _input;
        private readonly Stack<char> _returnedChars;
        private readonly StringBuilder _inputBuffer;

        private string? _commandName;
        private string? _botName;
        private readonly List<BotCommand.Argument> _arguments;

        private char _currentChar;
        private bool _isEof;
        private string? _argumentName;
        private State _state;
        private State _nextState;

        private BotCommandParser(string input)
        {
            _input = input;
            _returnedChars = new Stack<char>();
            _inputBuffer = new StringBuilder();

            _commandName = null;
            _botName = null;
            _arguments = new List<BotCommand.Argument>();

            _currentChar = default;
            _isEof = false;

            _argumentName = null;
            _state = State.InitialSlash;
            _nextState = default;
        }

        public BotCommand? Parse()
        {
            using var enumerator = _input.GetEnumerator();

            while (_state != State.Error && GetNextChar(enumerator))
            {
                switch (_state)
                {
                    case State.InitialSlash: InitialSlashState(); break;
                    case State.CommandName: CommandNameState(); break;
                    case State.BotName: BotNameState(); break;
                    case State.Whitespace: WhitespaceState(); break;
                    case State.Argument: ArgumentState(); break;
                    case State.QuotedArgument: QuotedArgumentState(); break;
                    case State.Escape: EscapeState(); break;
                }
            }

            return _state != State.Error && _commandName != null ?
                   new BotCommand(_commandName, _botName, _arguments) :
                   null;
        }

        private void InitialSlashState() => _state = _currentChar == '/' ? State.CommandName : State.Error;

        private void CommandNameState()
        {
            if (_isEof || _currentChar == '@' || char.IsWhiteSpace(_currentChar))
            {
                if (_inputBuffer.Length == 0)
                {
                    _state = State.Error;
                }
                else
                {
                    _commandName = GetAndCleanInputBuffer();
                    if (_isEof) return;
                    if (_currentChar == '@') _state = State.BotName;
                    else _state = State.Whitespace;
                }
            }
            else if (char.IsLetterOrDigit(_currentChar) || _currentChar == '_')
                _inputBuffer.Append(_currentChar);
            else
                _state = State.Error;
        }

        private void BotNameState()
        {
            if (_isEof || char.IsWhiteSpace(_currentChar))
            {
                _botName = GetAndCleanInputBuffer();
                _botName = _botName.Length != 0 ? _botName  : null;

                _state = State.Whitespace;
            }
            else if (char.IsLetterOrDigit(_currentChar) || _currentChar == '_')
                _inputBuffer.Append(_currentChar);
            else
                _state = State.Error;
        }

        private void WhitespaceState()
        {
            if (_isEof) return;
            else if (char.IsWhiteSpace(_currentChar)) return;
            else  if (_currentChar == '"')
                _state = State.QuotedArgument;
            else
            {
                _state = State.Argument;
                ReturnChar();
            }
        }

        private void ArgumentState()
        {
           if (_isEof || char.IsWhiteSpace(_currentChar))
           {
               _arguments.Add(new BotCommand.Argument(GetAndCleanInputBuffer(), _argumentName));
               _argumentName = null;
               _state = State.Whitespace;
           }
           else if (_currentChar == '=')
           {
               if (_inputBuffer.Length == 0)
                   _state = State.Error;
               else
                   _argumentName = GetAndCleanInputBuffer();
           }
           else if (_currentChar == '\\')
           {
               _nextState = _state;
               _state = State.Escape;
           }
           else if (_currentChar == '"')
           {
                _state = _inputBuffer.Length != 0  ? State.Error : State.QuotedArgument;
           }
           else
           {
               _inputBuffer.Append(_currentChar);
           }
        }

        private void QuotedArgumentState()
        {
            if (_isEof) _state = State.Error;
            else if (_currentChar == '\\')
            {
                _nextState = _state;
                _state = State.Escape;
            }
            else if (_currentChar == '"')
            {
                _arguments.Add(new BotCommand.Argument(GetAndCleanInputBuffer(), _argumentName));
                _argumentName = null;
                _state = State.Whitespace;
            }
            else
                _inputBuffer.Append(_currentChar);
        }

        private void EscapeState()
        {
            if (_isEof) _state = State.Error;
            else
            {
                _inputBuffer.Append(_currentChar);
                _state = _nextState;
            }
        }

        private string GetAndCleanInputBuffer()
        {
            var result = _inputBuffer.ToString();
            _inputBuffer.Clear();
            return result;
        }

        private void ReturnChar() => _returnedChars.Push(_currentChar);

        private bool GetNextChar(IEnumerator<char> enumerator)
        {
            if (_state == State.Error || _isEof)
                return false;

            if (_returnedChars.TryPop(out var @char))
                _currentChar = @char;
            else if (enumerator.MoveNext())
                _currentChar = enumerator.Current;
            else if (!_isEof)
                _isEof = true;

            return true;
        }

        public static BotCommand? Parse(string? input) =>
            string.IsNullOrWhiteSpace(input) ? null : new BotCommandParser(input).Parse();

        private enum State
        {
            InitialSlash,
            CommandName,
            BotName,
            Whitespace,

            Argument,
            QuotedArgument,

            Escape,

            Error
        }
    }
}

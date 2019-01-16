using System;
using System.Text;

namespace TicTacToeLib
{
    public class Board : IBoard
    {
        private readonly Cell[,] _Cells;
        private int _EmptyCount;
        private readonly BoardState[] _Stack;
        private int _StackPointer;
        private readonly BoardState _CurrentBoardState;

        public Point2D LastPoint
        {
            get
            {
                if (_StackPointer <= 0)
                {
                    return null;
                }

                return _Stack[_StackPointer - 1].Point;
            }
        }

        public PlayerMark? LastPlayerMark
        {
            get
            {
                Point2D point = LastPoint;

                if (point == null)
                {
                    return null;
                }

                Cell cell = _Cells[point.X, point.Y];

                if (cell.Empty)
                {
                    throw new InvalidOperationException();
                }

                return cell.PlayerMark;
            }
        }

        public int CountInRow
        {
            get;
            private set;
        }

        public int Width
        {
            get
            {
                return _Cells.GetLength(0);
            }
        }

        public int Height
        {
            get
            {
                return _Cells.GetLength(1);
            }
        }

        public bool Undo()
        {
            if (_StackPointer <= 0)
            {
                return false;
            }

            BoardState boardState = _Stack[--_StackPointer];

            _Cells[boardState.Point.X, boardState.Point.Y] = Cell.EmptyCell;
            CopyBoardState(boardState, _CurrentBoardState);
            _EmptyCount++;

            return true;
        }

        private void CopyBoardState(BoardState source, BoardState destination)
        {
            Array.Copy(source.MaxCountInRowPerMark, destination.MaxCountInRowPerMark, source.MaxCountInRowPerMark.Length);            
        }

        public bool Mark(int x, int y, PlayerMark playerMark)
        {
            if (!_Cells[x, y].Empty)
            {
                return false;
            }

            BoardState boardState = _Stack[_StackPointer++];
            CopyBoardState(_CurrentBoardState, boardState);
            boardState.Point = new Point2D(x, y);
            _EmptyCount--;
            _Cells[x, y] = Cell.Get(playerMark);

            int index = (int)playerMark;
            int count = _CurrentBoardState.MaxCountInRowPerMark[index];

            if (count >= CountInRow)
            {
                return true;
            }
            
            int primaryDiagonal = 1;
            bool primaryDiagonalInRowBackward = true;
            bool primaryDiagonalInRowForward = true;
            int secondaryDiagonal = 1;
            bool secondaryDiagonalInRowBackward = true;
            bool secondaryDiagonalInRowForward = true;
            int horizontal = 1;
            bool horizontalInRowBackward = true;
            bool horizontalInRowForward = true;
            int vertical = 1;
            bool verticalInRowBackward = true;
            bool verticalInRowForward = true;
            
            for (int i = 1; i < CountInRow; i++)
            {
                int pdx = x + i;
                int pdy = y + i;
                int ndx = x - i;
                int ndy = y - i;

                ProcessCoords(playerMark, ref primaryDiagonal, ref primaryDiagonalInRowBackward, ndx, ndy);
                ProcessCoords(playerMark, ref primaryDiagonal, ref primaryDiagonalInRowForward, pdx, pdy);

                ProcessCoords(playerMark, ref secondaryDiagonal, ref secondaryDiagonalInRowBackward, ndx, pdy);
                ProcessCoords(playerMark, ref secondaryDiagonal, ref secondaryDiagonalInRowForward, pdx, ndy);

                ProcessCoords(playerMark, ref horizontal, ref horizontalInRowBackward, ndx, y);
                ProcessCoords(playerMark, ref horizontal, ref horizontalInRowForward, pdx, y);


                ProcessCoords(playerMark, ref vertical, ref verticalInRowBackward, x, ndy);
                ProcessCoords(playerMark, ref vertical, ref verticalInRowForward, x, pdy);
            }

            int candidateCount = Math.Max(Math.Max(horizontal, vertical), Math.Max(primaryDiagonal, secondaryDiagonal));

            _CurrentBoardState.MaxCountInRowPerMark[index] = Math.Max(candidateCount, count);

            return true;
        }

        private void ProcessCoords(PlayerMark playerMark, ref int counter, ref bool inRow, int x, int y)
        {
            if (inRow && x >= 0 && y >= 0 && x < Width && y < Height)
            {
                if (_Cells[x, y].IsMarked(playerMark))
                {
                    counter++;
                }
                else
                {
                    inRow = false;
                }
            }
        }

        public Cell this[int x, int y]
        {
            get
            {
                return _Cells[x, y];
            }
        }

        public Board(int width, int height, int countInRow)
        {
            CountInRow = countInRow;
            _Cells = new Cell[width, height];

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    _Cells[i, j] = Cell.EmptyCell;
                }
            }
            
            _Stack = new BoardState[Width * Height];
            _StackPointer = 0;           
            
            for (int i = 0; i < _Stack.Length; i++)
            {
                _Stack[i] = new BoardState();
            }

            _EmptyCount = _Stack.Length;
            _CurrentBoardState = new BoardState();
        }

        private Board(Cell[,] cells, int countInRow, int emptyCount, BoardState[] stack, int stackPointer, BoardState currentBoardState)
        {
            _Cells = cells;
            CountInRow = countInRow;
            _EmptyCount = emptyCount;
            _Stack = stack;
            _StackPointer = stackPointer;
            _CurrentBoardState = currentBoardState;
        }

        public bool HasEmpty()
        {
            return _EmptyCount > 0;
        }

        public bool HasInRow(PlayerMark playerMark)
        {
            int playerMarkInt = (int)playerMark;
            return _CurrentBoardState.MaxCountInRowPerMark[playerMarkInt] >= CountInRow;             
        }

        public Board GetCopy()
        {
            Cell[,] cells = new Cell[Width, Height];

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    cells[i, j] = _Cells[i, j];
                }
            }

            BoardState[] stack = new BoardState[_Stack.Length];
            
            for (int i = 0; i < stack.Length; i++)
            {
                stack[i] = _Stack[i].GetCopy();
            }

            return new Board(cells, CountInRow, _EmptyCount, stack, _StackPointer, _CurrentBoardState.GetCopy());
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < _Cells.GetLength(0); i++)
            {
                for (int j = 0; j < _Cells.GetLength(1); j++)
                {
                    if (_Cells[i, j].Empty) {
                        stringBuilder.Append(' ');
                        continue;
                    }

                    switch (_Cells[i, j].PlayerMark)
                    {
                        case PlayerMark.Cross:
                            stringBuilder.Append('X');
                            break;
                        case PlayerMark.Nought:
                            stringBuilder.Append('O');
                            break;
                        default:
                            throw new InvalidOperationException();
                            
                    }
                }

                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }
    }
}

﻿using ChessChallenge.API;
using System;
using System.Linq;

public class MyBot : IChessBot
{
	//                     .  P    K    B    R    Q    K
	int[] kPieceValues = { 0, 100, 300, 310, 500, 900, 10000 };

	bool mIsWhite;

	public Move Think(Board board, Timer timer)
	{
		Move[] legalMoves = board.GetLegalMoves();
		mIsWhite = board.IsWhiteToMove;

		return legalMoves.MaxBy(x => EvaluateMoveMinMax(board, x));
	}

	int EvaluateMoveMinMax(Board board, Move move, int depth = 4)
	{
		bool isUs = board.IsWhiteToMove == mIsWhite;
		int finalScore;

		board.MakeMove(move);
		depth--;

		if (depth == 0)
			finalScore = Evaluate(board);
		else
		{
			finalScore = isUs ? int.MaxValue : int.MinValue;
			foreach (Move candidateMove in board.GetLegalMoves())
			{
				int eval = EvaluateMoveMinMax(board, candidateMove, depth);
				if (eval > finalScore != isUs)
					finalScore = eval;
			}
		}
		board.UndoMove(move);

		return finalScore;
	}


	int Evaluate(Board board)
	{
		int sum = 0;

		if(board.IsInCheckmate())
			return mIsWhite == board.IsWhiteToMove ? int.MinValue : int.MaxValue;

		for (int i = 0; ++i < 7;)
			sum += (board.GetPieceList((PieceType)i, mIsWhite).Count - board.GetPieceList((PieceType)i, !mIsWhite).Count) * kPieceValues[i];

		return sum;
	}
}
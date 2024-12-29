using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Match3Example.GameObjects;
using Match3Example.Scenes.GameSceneBehavior;

namespace Match3Example.Scenes.GameBehavior.GameStates
{
    class Match3Check : GameStateLogic
    {
        private GameScene game;
        private ElementsFall ef;
        private GenerateNewElements gne;

        public Match3Check(GameScene game, ElementsFall ef, GenerateNewElements gne) 
        {
            this.game = game;
            this.ef = ef;
            this.gne = gne;
        }

        public override void Update()
        {
            bool toSE = game.state == GameState.Match3CheckSE ? true : false;
            GameField gameField = game.GameField;

            if (Match3.IsWithoutDelete(gameField))
            {
                if (toSE)
                {
                    game.state = GameState.SwitchElementsREVERSE;
                    return;
                }

                (bool status, bool[,] empty) checkEmptyCells = Match3.CheckEmptyCells(gameField);
                if (checkEmptyCells.status)
                {
                    gne.Start(checkEmptyCells.empty);
                    game.state = GameState.GenerateNewElements;
                    return;
                }
                else
                {
                    game.state = GameState.Interact;
                    return;
                }
            }

            bool[,] deleteCells = Match3.CheckDelete(gameField);

            for (int i = 0; i < deleteCells.GetLength(0); i++)
            {
                for (int j = 0; j < deleteCells.GetLength(1); j++)
                {
                    if (deleteCells[i, j])
                    {
                        gameField.cells[i, j].element = null;
                        game.score++;
                    }
                }
            }

            ef.Start();
            game.state = GameState.ElementsFall;
        }
    }
}

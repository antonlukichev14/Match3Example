using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3Example.Scenes.GameBehavior
{
    public enum GameState
    {
        Interact,
        SwitchElements,
        Match3CheckSE,
        SwitchElementsREVERSE,
        ElementsFall,
        Match3Check,
        GenerateNewElements
    }
}

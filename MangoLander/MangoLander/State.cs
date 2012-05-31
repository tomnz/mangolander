using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MangoLander
{
    public enum GameState
    {
        Active,
        GameOver,
        Paused,
        Menu
    }

    public enum MenuState
    {
        Intro,
        Main,
        Settings,
        Playing
    }
}

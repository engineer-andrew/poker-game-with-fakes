using System.Collections.Generic;

namespace CardGames.Shuffler.Model
{
    public class Deck
    {
        public List<string> Cards { get; set; }

        public bool IsShuffled { get; set; }
    }
}
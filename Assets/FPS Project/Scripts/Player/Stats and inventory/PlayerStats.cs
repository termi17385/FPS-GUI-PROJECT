namespace FPSProject.Player.stats
{
    [System.Serializable]
    public class PlayerStats
    {
        [System.Serializable]
        public struct statsStruct
        {
            public string statNames;
            public int statsNum;
        }
        public statsStruct[] stats; 
        public CharacterClass characterClass;

        public enum CharacterClass
        {
            Stealth,
            Tank,
            Hunter,
            SprintyBoi,
            Mage,
        }
    }
}

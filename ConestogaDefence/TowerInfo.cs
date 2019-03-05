namespace ConestogaDefence
{
    public struct TowerInfo
    {
        public static float GreenTowerDelay => .8f;
        public static float RedTowerDelay => 1.2f;

        public static int GreenTowerCost => 10;
        public static int RedTowerCost => 20;
        public static int SlowDownTowerCost => 15;

        public static int GreenTowerDamage => 30;
        public static int RedTowerDamage => 70;
        public static float SlowDownTowerRatio => .7f;

        public static int GreenTowerRadius => 150;
        public static int RedTowerRadius => 170;
        public static int SlowDownTowerRadius => 120;
    }
}
using UniRx;

namespace _Project.Scripts
{
    public static class PlayerData
    {
        public static ReactiveProperty<decimal> Amount = new(0);
        public static ReactiveProperty<int> WinCount = new(0);
        public static int Id;
    }
}
using UniRx;

namespace _Project.Scripts
{
    public static class PlayerData
    {
        public static ReactiveProperty<decimal> Amount = new(0);
    }
}
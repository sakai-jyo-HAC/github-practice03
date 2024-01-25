using DxLibDLL;
using MyLib; // MyRandomとかInputとか使うのに必要
using MyLIb;

namespace Chiritori
{
    public class Game
    {
        public void Init()
        {
            MyRandomcs.Init(); // MyRandomの初期化
            Input.Init(); // Inputの初期化
        }

        public void Update()
        {
            Input.Update(); // Inputの更新
        }

        public void Draw()
        {
        }
    }
}

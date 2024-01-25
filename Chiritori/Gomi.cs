using DxLibDLL;
using MyLib;
using MyLIb;

namespace Chiritori
{
    // ゴミクラス
    public class Gomi
    {
        public float x; // x座標
        public float y; // y座標

        // コンストラクタ
        public Gomi()
        {
            // 初期位置はランダム
            ResetPosition();
        }

        // 場所をランダムにリセットする
        public void ResetPosition()
        {
            x = MyRandomcs.Range(0, Screen.Width);
            y = MyRandomcs.Range(0, Screen.Height);
        }

        // 描画処理
        public void Draw()
        {
            // 別に回転させるわけではないが、横着してDrawRotaGraphF()を使っている。
            // 勝手に中央揃えしてくれて楽だから。
            DX.DrawRotaGraphF(x, y, 1f, 0f, Image.gomi);
        }
    }
}

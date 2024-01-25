using System; // Math使うのに必要
using DxLibDLL;
using MyLib;
using MyLIb;

namespace Chiritori
{
    // プレイヤーキャラのチリトリークラス
    public class Chiritori
    {
        const float MoveSpeed = 7f; // 移動速度
        const float RotateSpeed = 0.16f; // 回転速度（ラジアン/フレーム）

        public float x; // x座標
        public float y; // y座標
        public float angle; // 向き（ラジアン。右が0で時計回り）

        // コンストラクタ。初期化処理を行う。
        public Chiritori()
        {
            // 位置をランダムに初期化
            x = MyRandomcs.Range(0, Screen.Width);
            y = MyRandomcs.Range(0, Screen.Height);

            // 向きをランダムに初期化
            angle = MyRandomcs.Range(0, (float)Math.PI * 2);
        }

        // 更新処理
        public void Update()
        {
            if (Input.GetButton(DX.PAD_INPUT_1))
            {
                // ボタンが押されていたら、向いている方向へ移動
                x += (float)(Math.Cos(angle) * MoveSpeed);
                y += (float)(Math.Sin(angle) * MoveSpeed);
            }
            else
            {
                // ボタンが押されていなければ、回転
                angle += RotateSpeed;
            }
        }

        // 描画処理
        public void Draw()
        {
            DX.DrawRotaGraphF(x, y, 1f, angle, Image.chiritoriGreen);
        }
    }
}
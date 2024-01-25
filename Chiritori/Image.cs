using DxLibDLL;

namespace Chiritori
{
    // 画像管理クラス
    public static class Image
    {
        // 画像ハンドル
        public static int woodFloor; // 木の床
        public static int chiritoriGreen; // チリトリ 緑
        public static int chiritoriRed; // チリトリ 赤
        public static int chiritoriBlue; // チリトリ 青
        public static int chiritoriYellow; // チリトリ 黄
        public static int gomi; // ゴミ

        // 画像読み込み処理
        public static void Load()
        {
            woodFloor = DX.LoadGraph("Image/wood_floor.jpg");
            chiritoriGreen = DX.LoadGraph("Image/chiritori_green.png");
            chiritoriRed = DX.LoadGraph("Image/chiritori_red.png");
            chiritoriBlue = DX.LoadGraph("Image/chiritori_blue.png");
            chiritoriYellow = DX.LoadGraph("Image/chiritori_yellow.png");
            gomi = DX.LoadGraph("Image/gomi.png");
        }
    }
}
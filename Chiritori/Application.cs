using DxLibDLL;
using System;
using System.Diagnostics;
using System.Threading;

namespace Chiritori
{
    internal class Application
    {
        const int TargetFPS = 60; // �ڕW��FPS(Frame Per Second, 1�b������̃t���[����)
        static readonly bool EnableFrameSkip = true; // �����׎��Ƀt���[���X�L�b�v���邩�ifalse�̏ꍇ�͏��������i�X���[�j�j
        const double MaxAllowSkipTime = 0.2; // �t���[���X�L�b�v����ő�Ԋu�i�b�j�B����ȏ�̊Ԋu���󂢂��ꍇ�̓X�L�b�v�����ɏ��������ɂ���B
        const long IntervalTicks = (long)(1.0 / TargetFPS * 10000000); // �t���[���Ԃ�Tick���B1Tick = 100�i�m�b = 1/10000000�b
        const int MaxAllowSkipCount = (int)(TargetFPS * MaxAllowSkipTime);

        static long nextFrameTicks = IntervalTicks; // ���̃t���[���̖ڕW����
        static Stopwatch stopwatch = new Stopwatch(); // FPS����̂��߂Ɏ��Ԃ��v�邽�߂̍����x�^�C�}�[
        static int skipCount = 0; // ����A���Ńt���[���X�L�b�v������
        static long fpsTicks = 0; // FPS�v���̂��߂�Ticks�B
        static int fpsFrameCount = 0; // FPS�v���̂��߂̃t���[���J�E���g�B60�񐔂��邲�ƂɁA�v�������Ԃ���FPS���Z�o����B

        /// <summary>
        /// ���݂�FPS�iFrame per Second�j
        /// </summary>
        public static float CurrentFPS { get; private set; }

        static Game game;

        [STAThread]
        static void Main(string[] args)
        {
            Thread.CurrentThread.Priority = ThreadPriority.Highest; // �X���b�h�̗D��x���グ�Ă���
            // ��ʃ��t���b�V�����[�g�ƖڕW�t���[�����[�g���������ꍇ�͐���������L���ɁA�������Ȃ��ꍇ�͐��������𖳌��ɂ���
            DX.SetWaitVSyncFlag(DX.GetRefreshRate() == TargetFPS ? DX.TRUE : DX.FALSE);
            DX.SetWindowText("�Q�[���^�C�g��"); // �E�B���h�E�̃^�C�g��
            DX.SetGraphMode(640, 480, 32); // �E�B���h�E�T�C�Y�i��ʉ𑜓x�j�̎w��
            DX.ChangeWindowMode(DX.TRUE); // �E�B���h�E���[�h�ɂ���iDX.FALSE���w�肷��ƃt���X�N���[���ɂȂ�j
            DX.SetAlwaysRunFlag(DX.TRUE); // �E�B���h�E����A�N�e�B�u�ł����삳����

            DX.DxLib_Init(); // DX���C�u�����̏�����

            DX.SetMouseDispFlag(DX.TRUE); // �}�E�X��\������iDX.FALSE���w�肷��Ɣ�\���ɂȂ�j
            DX.SetDrawScreen(DX.DX_SCREEN_BACK); // �`���𗠉�ʂƂ���i�_�u���o�b�t�@�j
            DX.SetUseTransColor(DX.FALSE); // �摜�̎w��F�𓧉߂���@�\�𖳌���

            game = new Game();
            game.Init();

            DX.ScreenFlip();
            stopwatch.Start();

            while (DX.ProcessMessage() == 0) // �E�B���h�E��������܂ŌJ��Ԃ�
            {
                // FPS�̌v��
                fpsFrameCount++;
                if (fpsFrameCount >= 60)
                {
                    long elapsedTicks = stopwatch.Elapsed.Ticks - fpsTicks;
                    float elapsedSec = elapsedTicks / 10000000f;
                    CurrentFPS = fpsFrameCount / elapsedSec;

                    fpsFrameCount = 0;
                    fpsTicks = stopwatch.Elapsed.Ticks;
                }

                game.Update();

                if (DX.GetWaitVSyncFlag() == DX.TRUE)
                {
                    if (EnableFrameSkip)
                    {
                        long waitTicks = nextFrameTicks - stopwatch.Elapsed.Ticks; // �]��������

                        if (waitTicks < 0) // �ڕW�������I�[�o�[���Ă���
                        {
                            if (skipCount < MaxAllowSkipCount) // �A���t���[���X�L�b�v�����ő�X�L�b�v���𒴂��Ă��Ȃ����
                            {
                                skipCount++; // �t���[���X�L�b�v�i�`�揈�����΂��j
                            }
                            else
                            {
                                // �ő�X�L�b�v���𒴂��Ă�̂ŁA�t���[���X�L�b�v���Ȃ��ŕ`��
                                nextFrameTicks = stopwatch.Elapsed.Ticks;
                                Draw();
                            }
                        }
                        else
                        {
                            Draw();
                        }

                        nextFrameTicks += IntervalTicks;
                    }
                    else
                    {
                        Draw();
                    }
                }
                else
                {
                    long waitTicks = nextFrameTicks - stopwatch.Elapsed.Ticks; // �]�������ԁi�ҋ@���K�v�Ȏ��ԁj

                    if (EnableFrameSkip && waitTicks < 0)
                    {
                        if (skipCount < MaxAllowSkipCount)
                        {
                            skipCount++; // �t���[���X�L�b�v�i�`�揈�����΂��j
                        }
                        else
                        {
                            nextFrameTicks = stopwatch.Elapsed.Ticks;
                            Draw();
                        }
                    }
                    else
                    {
                        if (waitTicks > 20000) // ����2�~���b�ȏ�҂K�v������
                        {
                            // Sleep()�͎w�肵�����ԂŃs�b�^���߂��Ă���킯�ł͂Ȃ��̂ŁA
                            // �]�T�������āA�u�҂��Ȃ���΂Ȃ�Ȃ�����-2�~���b�vSleep����
                            int waitMillsec = (int)(waitTicks / 10000) - 2;
                            Thread.Sleep(waitMillsec);
                        }

                        // ���Ԃ�����܂ŉ������Ȃ����[�v���񂵂đҋ@����
                        while (stopwatch.Elapsed.Ticks < nextFrameTicks)
                        {
                        }

                        Draw();
                    }
                    nextFrameTicks += IntervalTicks;
                }
            }

            DX.DxLib_End(); // DX���C�u�����I������
        }

        static void Draw()
        {
            DX.ClearDrawScreen(); // �`���̓��e���N���A����
            game.Draw(); // �Q�[���`��
            DX.ScreenFlip(); // ����ʂƕ\��ʂ����ւ���
            skipCount = 0; // �t���[���X�L�b�v�̃J�E���g�����Z�b�g
        }
    }
}

using DXEngineWrapLib;
using Serilog;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Shiliu.DigitalHuman.Wpf
{
    /// <summary>
    /// 数字人渲染区域 UserControl。
    /// 职责：封装 DXEngineWrap 的初始化/播放/停止/销毁，以及 Loading 占位层。
    /// 周围的对话气泡、录音按钮等业务 UI 由调用方自行实现。
    /// </summary>
    public partial class DigitalHumanView : UserControl
    {
        // ── DependencyProperty ──────────────────────────────────────────────

        public static readonly DependencyProperty RenderWidthProperty =
            DependencyProperty.Register(nameof(RenderWidth), typeof(double), typeof(DigitalHumanView),
                new PropertyMetadata(400.0, OnRenderSizeChanged));

        public static readonly DependencyProperty RenderHeightProperty =
            DependencyProperty.Register(nameof(RenderHeight), typeof(double), typeof(DigitalHumanView),
                new PropertyMetadata(800.0, OnRenderSizeChanged));

        public static readonly DependencyProperty RenderBackColorProperty =
            DependencyProperty.Register(nameof(RenderBackColor), typeof(System.Windows.Media.Color), typeof(DigitalHumanView),
                new PropertyMetadata(System.Windows.Media.Color.FromRgb(0x57, 0x57, 0x6E), OnRenderBackColorChanged));

        public static readonly DependencyProperty LoadingTextProperty =
            DependencyProperty.Register(nameof(LoadingText), typeof(string), typeof(DigitalHumanView),
                new PropertyMetadata("正在初始化数字人 ...", OnLoadingTextChanged));

        public static readonly DependencyProperty LoadingImageSourceProperty =
            DependencyProperty.Register(nameof(LoadingImageSource), typeof(ImageSource), typeof(DigitalHumanView),
                new PropertyMetadata(null, OnLoadingImageSourceChanged));

        // ── CLR 包装 ────────────────────────────────────────────────────────

        public double RenderWidth
        {
            get => (double)GetValue(RenderWidthProperty);
            set => SetValue(RenderWidthProperty, value);
        }

        public double RenderHeight
        {
            get => (double)GetValue(RenderHeightProperty);
            set => SetValue(RenderHeightProperty, value);
        }

        /// <summary>数字人渲染区域背景色（同时应用到 WinForms Panel 和 Border）</summary>
        public System.Windows.Media.Color RenderBackColor
        {
            get => (System.Windows.Media.Color)GetValue(RenderBackColorProperty);
            set => SetValue(RenderBackColorProperty, value);
        }

        /// <summary>Loading 层文字，默认"正在初始化数字人 ..."</summary>
        public string LoadingText
        {
            get => (string)GetValue(LoadingTextProperty);
            set => SetValue(LoadingTextProperty, value);
        }

        /// <summary>Loading 层占位图，不设置则不显示图片</summary>
        public ImageSource LoadingImageSource
        {
            get => (ImageSource)GetValue(LoadingImageSourceProperty);
            set => SetValue(LoadingImageSourceProperty, value);
        }

        // ── 公开状态 ────────────────────────────────────────────────────────

        /// <summary>数字人引擎是否已完成初始化</summary>
        public bool IsInitialized => _isInitialized;

        /// <summary>当前安装路径是否支持数字人（不含中文/非ASCII字符）</summary>
        public bool IsPathSupported => !ContainsNonAscii(AppDomain.CurrentDomain.BaseDirectory);

        // ── 私有状态 ────────────────────────────────────────────────────────

        private DXEngineWrap _engine;
        private bool _isInitialized;
        private bool _isSpeaking;

        // ── 构造 ────────────────────────────────────────────────────────────

        public DigitalHumanView()
        {
            InitializeComponent();
            ApplyRenderSize();
            ApplyRenderBackColor();
            LoadingTextBlock.Text = LoadingText;
        }

        // ── 公开 API ────────────────────────────────────────────────────────

        /// <summary>
        /// 初始化数字人引擎。
        /// 调用方应在确认 IsPathSupported == true 后再调用此方法。
        /// 返回 true 表示初始化成功，返回 false 表示失败（错误信息已写入 Serilog）。
        /// </summary>
        public bool Initialize()
        {
            if (_isInitialized) return true;

            ShowLoading(true);

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string modelPath = Path.Combine(basePath, "DXData", "model", "gj_dh_res");
            string voicePath = Path.Combine(basePath, "DXData", "model", "waiyan_1008");

            Log.Information("[DigitalHumanView] Init — base={Base}, model={Model}, voice={Voice}",
                basePath, modelPath, voicePath);

            if (!Directory.Exists(modelPath))
            {
                Log.Error("[DigitalHumanView] 模型目录不存在: {Path}", modelPath);
                ShowLoading(false);
                return false;
            }
            if (!Directory.Exists(voicePath))
            {
                Log.Error("[DigitalHumanView] 语音模型目录不存在: {Path}", voicePath);
                ShowLoading(false);
                return false;
            }

            try
            {
                var panel = RenderPanel;
                var initFrame = new Rectangle(0, 0,
                    panel.Width > 0 ? panel.Width : (int)RenderWidth,
                    panel.Height > 0 ? panel.Height : (int)RenderHeight);

                if (_engine == null) _engine = new DXEngineWrap();
                _engine.SetInitCallback(OnDXEngineInited);
                _engine.Init(panel.Handle, initFrame, modelPath, voicePath);

                _isInitialized = true;
                Log.Information("[DigitalHumanView] 引擎 Init 调用完成，等待回调");
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[DigitalHumanView] Init 抛出异常");
                ShowLoading(false);
                _isInitialized = false;
                return false;
            }
        }

        /// <summary>播放指定 wav 文件，数字人开始说话动作</summary>
        public void StartSpeaking(string audioFilePath = null)
        {
            if (!_isInitialized || _isSpeaking) return;
            _engine.PlayWav(audioFilePath);
            _isSpeaking = true;
        }

        /// <summary>停止当前播放，数字人回到待机状态</summary>
        public void StopSpeaking()
        {
            if (!_isInitialized || !_isSpeaking) return;
            _engine.Stop();
            _engine.Start();
            _isSpeaking = false;
        }

        /// <summary>释放数字人引擎资源</summary>
        public new void Dispose()
        {
            if (!_isInitialized) return;
            _engine.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            _engine = null;
            _isInitialized = false;
            _isSpeaking = false;
            ShowLoading(false);
            Log.Information("[DigitalHumanView] 引擎已销毁");
        }

        // ── 私有辅助 ────────────────────────────────────────────────────────

        private void OnDXEngineInited(bool isSuccess)
        {
            Log.Information("[DigitalHumanView] onDXEngineInited(isSuccess={Result})", isSuccess);
            if (!isSuccess)
            {
                Log.Error("[DigitalHumanView] 引擎回调失败，请检查模型文件/CUDA/GPU驱动/D3D设备");
                Dispatcher.Invoke(() => ShowLoading(false));
                _isInitialized = false;
                return;
            }
            try
            {
                _engine.Start();
                Dispatcher.Invoke(() => ShowLoading(false));
                Log.Information("[DigitalHumanView] 引擎初始化成功并已 Start");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[DigitalHumanView] Start 抛出异常");
            }
        }

        private void ShowLoading(bool visible)
        {
            LoadingLayer.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ApplyRenderSize()
        {
            RenderBorder.Width = RenderWidth;
            RenderBorder.Height = RenderHeight;
            RenderPanel.Width = (int)RenderWidth;
            RenderPanel.Height = (int)RenderHeight;
        }

        private void ApplyRenderBackColor()
        {
            var c = RenderBackColor;
            RenderBorder.Background = new SolidColorBrush(c);
            RootGrid.Background = new SolidColorBrush(c);
            RenderPanel.BackColor = System.Drawing.Color.FromArgb(c.R, c.G, c.B);
        }

        private static bool ContainsNonAscii(string s) =>
            !string.IsNullOrEmpty(s) && s.Any(c => c > 127);

        // ── DependencyProperty 回调 ──────────────────────────────────────────

        private static void OnRenderSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DigitalHumanView v) v.ApplyRenderSize();
        }

        private static void OnRenderBackColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DigitalHumanView v) v.ApplyRenderBackColor();
        }

        private static void OnLoadingTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DigitalHumanView v)
                v.LoadingTextBlock.Text = e.NewValue as string;
        }

        private static void OnLoadingImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DigitalHumanView v)
                v.LoadingImage.Source = e.NewValue as ImageSource;
        }
    }
}

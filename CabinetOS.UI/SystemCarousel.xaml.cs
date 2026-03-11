using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using CabinetOS.Core.Metadata;

namespace CabinetOS.UI
{
    public partial class SystemCarousel : UserControl
    {
        private const int DuplicateCount = 3;
        private int MiddleOffset => _baseList.Count;

        private List<SystemMetadata> _baseList = new();
        private List<SystemMetadata> _displayList = new();

        private readonly TranslateTransform _transform = new TranslateTransform();

        private DateTime _lastInputTime = DateTime.MinValue;
        private readonly TimeSpan _inputDelay = TimeSpan.FromMilliseconds(120);

        private bool _suppressCallback = false;
        private bool _justWrapped = false;

        public SystemCarousel()
        {
            InitializeComponent();
            Loaded += OnLoaded;

            // Load full metadata instead of folder names
            Systems = MetadataService.GetAvailableSystems()
                .Select(id => MetadataService.GetSystemMetadata(id))
                .ToList();
        }
        
        private void DebugLog(string msg)
        {
            System.Diagnostics.Debug.WriteLine($"[Carousel] {msg}");
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            BuildDisplayList();

            ItemsHost.RenderTransform = _transform;

            SelectedIndex = MiddleOffset;
            Focus();
            UpdateTransformPosition(animate: false);
        }

        // -------------------------
        // SYSTEM LIST DP
        // -------------------------
        public IEnumerable<SystemMetadata> Systems
        {
            get => (IEnumerable<SystemMetadata>)GetValue(SystemsProperty);
            set => SetValue(SystemsProperty, value);
        }

        public static readonly DependencyProperty SystemsProperty =
            DependencyProperty.Register(nameof(Systems), typeof(IEnumerable<SystemMetadata>),
                typeof(SystemCarousel),
                new PropertyMetadata(null, OnSystemsChanged));

        private static void OnSystemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (SystemCarousel)d;
            control.BuildDisplayList();
        }

        // -------------------------
        // DISPLAY LIST
        // -------------------------
        public IEnumerable<SystemMetadata> DisplayItems => _displayList;

        private void BuildDisplayList()
        {
            if (Systems == null)
                return;

            _baseList = Systems.ToList();
            _displayList = Enumerable.Repeat(_baseList, DuplicateCount)
                                     .SelectMany(x => x)
                                     .ToList();

            ItemsHost.ItemsSource = _displayList;
        }

        // -------------------------
        // SELECTED INDEX DP
        // -------------------------
        public int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register(nameof(SelectedIndex), typeof(int),
                typeof(SystemCarousel),
                new PropertyMetadata(0, OnSelectedIndexChanged));

        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (SystemCarousel)d;

            if (control._justWrapped)
            {
                control.DebugLog($"WRAP CALLBACK SKIPPED: {e.OldValue} → {e.NewValue}");
                control._justWrapped = false;
                return;
            }

            if (control._suppressCallback)
            {
                control.DebugLog($"SUPPRESSED CALLBACK: {e.OldValue} → {e.NewValue}");
                return;
            }

            control.DebugLog($"INDEX CHANGED: {e.OldValue} → {e.NewValue}");

            control.WrapIndexIfNeeded();
            control.UpdateTransformPosition(animate: true);
            control.SelectionChanged?.Invoke(control, EventArgs.Empty);
        }

        // -------------------------
        // WRAP LOGIC
        // -------------------------
        private void WrapIndexIfNeeded()
        {
            int baseCount = _baseList.Count;

            int leftBoundary = baseCount;
            int rightBoundary = baseCount * 2 - 1;

            if (SelectedIndex < leftBoundary)
            {
                DebugLog($"WRAP LEFT: {SelectedIndex} < {leftBoundary} → +{baseCount}");

                _transform.BeginAnimation(TranslateTransform.XProperty, null);

                _suppressCallback = true;
                _justWrapped = true;
                SelectedIndex += baseCount;
                _suppressCallback = false;

                UpdateTransformPosition(animate: false);
            }
            else if (SelectedIndex > rightBoundary)
            {
                DebugLog($"WRAP RIGHT: {SelectedIndex} > {rightBoundary} → -{baseCount}");

                _transform.BeginAnimation(TranslateTransform.XProperty, null);

                _suppressCallback = true;
                _justWrapped = true;
                SelectedIndex -= baseCount;
                _suppressCallback = false;

                UpdateTransformPosition(animate: false);
            }
        }

        // -------------------------
        // SELECTED ITEM
        // -------------------------
        public SystemMetadata? SelectedItem
        {
            get
            {
                if (_baseList.Count == 0)
                    return null;

                int index = SelectedIndex % _baseList.Count;
                if (index < 0)
                    index += _baseList.Count;

                return _baseList[index];
            }
        }

        // -------------------------
        // KEYBOARD INPUT
        // -------------------------
        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (DateTime.Now - _lastInputTime < _inputDelay)
            {
                e.Handled = true;
                return;
            }

            if (e.Key == Key.Right)
            {
                DebugLog($"KEY RIGHT → {SelectedIndex} → {SelectedIndex + 1}");
                _lastInputTime = DateTime.Now;
                SelectedIndex++;
                e.Handled = true;
            }
            else if (e.Key == Key.Left)
            {
                DebugLog($"KEY LEFT → {SelectedIndex} → {SelectedIndex - 1}");
                _lastInputTime = DateTime.Now;
                SelectedIndex--;
                e.Handled = true;
            }
        }

        // -------------------------
        // TRANSFORM ANIMATION
        // -------------------------
        private void UpdateTransformPosition(bool animate)
        {
            double itemWidth = 260;
            double targetX = -(SelectedIndex * itemWidth) + (ActualWidth / 2) - (itemWidth / 2);

            DebugLog($"ANIMATE {(animate ? "YES" : "NO")} → targetX = {targetX}");

            if (!animate)
            {
                _transform.X = targetX;
                return;
            }

            var anim = new DoubleAnimation
            {
                To = targetX,
                Duration = TimeSpan.FromMilliseconds(250),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            _transform.BeginAnimation(TranslateTransform.XProperty, anim);
        }

        public event EventHandler? SelectionChanged;
    }
}
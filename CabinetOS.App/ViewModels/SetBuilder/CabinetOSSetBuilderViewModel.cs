using global::CabinetOS.Core.Arcade.SetBuilder;
using global::CabinetOS.Core.Arcade;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CabinetOS.App.ViewModels.SetBuilder
{
    public class CabinetOSSetBuilderViewModel : ViewModelBase
    {
        // ------------------------------------------------------------
        // BUILD OPTIONS (bound to UI)
        // ------------------------------------------------------------
        public BuildOptions Options { get; } = new BuildOptions();

        // ------------------------------------------------------------
        // PROGRESS PROPERTIES
        // ------------------------------------------------------------
        private int _currentGameIndex;
        public int CurrentGameIndex
        {
            get => _currentGameIndex;
            set => SetProperty(ref _currentGameIndex, value);
        }

        private int _totalGames;
        public int TotalGames
        {
            get => _totalGames;
            set => SetProperty(ref _totalGames, value);
        }

        private string _currentGame = "";
        public string CurrentGame
        {
            get => _currentGame;
            set => SetProperty(ref _currentGame, value);
        }

        private string _currentRom = "";
        public string CurrentRom
        {
            get => _currentRom;
            set => SetProperty(ref _currentRom, value);
        }

        private string _currentChd = "";
        public string CurrentChd
        {
            get => _currentChd;
            set => SetProperty(ref _currentChd, value);
        }

        private string _currentSalvage = "";
        public string CurrentSalvage
        {
            get => _currentSalvage;
            set => SetProperty(ref _currentSalvage, value);
        }

        // ------------------------------------------------------------
        // LOG OUTPUT
        // ------------------------------------------------------------
        public ObservableCollection<string> LogEntries { get; } = new();

        private BuildLog _log = new BuildLog();

        // ------------------------------------------------------------
        // SUMMARY
        // ------------------------------------------------------------
        private BuildSummary? _summary;
        public BuildSummary? Summary
        {
            get => _summary;
            set => SetProperty(ref _summary, value);
        }

        // ------------------------------------------------------------
        // CANCELLATION
        // ------------------------------------------------------------
        private CancellationTokenSource? _cts;

        // ------------------------------------------------------------
        // COMMANDS
        // ------------------------------------------------------------
        public ICommand StartBuildCommand { get; }
        public ICommand CancelBuildCommand { get; }

        public CabinetOSSetBuilderViewModel()
        {
            StartBuildCommand = new RelayCommand(async _ => await StartBuildAsync(), _ => _cts == null);
            CancelBuildCommand = new RelayCommand(_ => CancelBuild(), _ => _cts != null);

            // Wire progress callbacks into BuildOptions
            Options.GameProgress = (i, total, name) =>
            {
                CurrentGameIndex = i;
                TotalGames = total;
                CurrentGame = name;
            };

            Options.RomProgress = (game, rom) =>
            {
                CurrentRom = rom;
            };

            Options.ChdProgress = (game, chd) =>
            {
                CurrentChd = chd;
            };

            Options.SalvageProgress = rom =>
            {
                CurrentSalvage = rom;
            };

            Options.Log = _log;
        }

        // ------------------------------------------------------------
        // START BUILD
        // ------------------------------------------------------------
        private async Task StartBuildAsync()
        {
            _cts = new CancellationTokenSource();
            Options.Cancellation = _cts.Token;

            LogEntries.Clear();
            Summary = null;

            // Mirror log entries into ObservableCollection
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Task.Run(async () =>
            {
                int lastCount = 0;
                while (_cts != null)
                {
                    var entries = _log.Entries;
                    while (lastCount < entries.Count)
                    {
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            LogEntries.Add(entries[lastCount]);
                        });
                        lastCount++;
                    }
                    await Task.Delay(100);
                }
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            try
            {
                Summary = await Task.Run(() => ArcadeSetBuilder.BuildSet(Options));
            }
            catch (OperationCanceledException)
            {
                LogEntries.Add("Build canceled.");
            }
            finally
            {
                _cts = null;
            }
        }

        // ------------------------------------------------------------
        // CANCEL BUILD
        // ------------------------------------------------------------
        private void CancelBuild()
        {
            _cts?.Cancel();
        }
    }
}

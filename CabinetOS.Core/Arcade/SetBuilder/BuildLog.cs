using System;
using System.Collections.Generic;
using System.Text;

namespace CabinetOS.Core.Arcade.SetBuilder
{
    // ============================================================
    // BUILD LOG
    // ============================================================
    public class BuildLog
    {
        private readonly List<string> _entries = new();
        public IReadOnlyList<string> Entries => _entries;

        public void Add(string message)
        {
            lock (_entries)
            {
                _entries.Add($"[{DateTime.Now:HH:mm:ss}] {message}");
            }
        }
    }
}

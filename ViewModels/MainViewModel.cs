using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using System.Windows.Forms;
using ParProgrammering.CodeJunk;

namespace ParProgrammering.ViewModels
{
    // Enum Checkbox values
    public enum TimeOptions { timeOption1 = 5000, timerOption2 = 10000, timerOption3 = 30000 }

    class MainViewModel : PropertyChangedBase
    {
        // Images in the selected folder
        private BindableCollection<string> _paths = new BindableCollection<string>();

        public BindableCollection<string> Paths {
            get
            {
                return _paths;
            }
            set
            {
                this._paths = value;
                NotifyOfPropertyChange(() => Paths);
            }
        }

        private string _selectedPath;

        public string SelectedPath
        {
            get { return _selectedPath; }
            set
            {
                _selectedPath = value;

                NotifyOfPropertyChange(() => SelectedPath);
            }
        }

        private bool _timeOptionOne;
        private bool _timeOptionTwo;
        private bool _timeOptionThree;

        public bool TimeOptionOne
        {
            get { return _timeOptionOne; }
            set
            {
                _timeOptionOne = value;
                ChangeTime((int)TimeOptions.timeOption1, value);
                NotifyOfPropertyChange(() => TimeOptionOne);
            }
        }
        public bool TimeOptionTwo
        {
            get { return _timeOptionTwo; }
            set
            {
                _timeOptionTwo = value;
                ChangeTime((int)TimeOptions.timerOption2, value);
                NotifyOfPropertyChange(() => TimeOptionTwo);
            }
        }
        public bool TimeOptionThree
        {
            get { return _timeOptionThree; }
            set
            {
                _timeOptionThree = value;
                ChangeTime((int)TimeOptions.timerOption3, value);
                NotifyOfPropertyChange(() => TimeOptionThree);
            }
        }


        private Timer _timer;

        public MainViewModel()
        {
            _timer = new Timer();
            _timer.Tick += new EventHandler(ChangeWallpaper);
            //ChangeTime((int)TimeOptions.timeOption1, true);
        }


        // Open folder dialog
        public void OpenFolderDialog()
        {
            using (var folder = new FolderBrowserDialog())
            {
                // Show dialog
                DialogResult result = folder.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folder.SelectedPath))
                {
                    Paths.Clear();
                    SelectedPath = folder.SelectedPath;
                    string[] files = Directory.GetFiles(folder.SelectedPath, "*.png");
                    string[] files2 = Directory.GetFiles(folder.SelectedPath, "*.jpg");

                    this.Paths.AddRange(files);
                    Paths.AddRange(files2);
                }
            }
        }

        public void ChangeWallpaper(object sender, EventArgs e)
        {
            if (Paths.Count != 0)
            {
                Random r = new Random();
                int max = this.Paths.Count;
                int pathValueNum = r.Next(0, max);

                CodeJunk.ChangeWallpaper.Set(this.Paths[pathValueNum]);
            }
        }

        private void ChangeTime(int newTime, bool doit)
        {
            if (doit)
            {
                this._timer.Stop();
                _timer.Interval = newTime;
                _timer.Start();
            }
        }

    }
}

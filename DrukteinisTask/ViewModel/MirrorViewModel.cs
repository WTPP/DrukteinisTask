using DrukteinisTask.Helpers;
using System.Linq;
using System.Windows.Input;

namespace DrukteinisTask.ViewModel
{
    public class MirrorViewModel : Framework.ViewModel
    {
        private double _scaleValue;
        private string _input;
        private string _output;

        public MirrorViewModel()
        {
            FlipCommand = new RelayCommand(FlipControls);
            ReverseSentenceCommand = new RelayCommand(ReverseSentence);
            ScaleValue = 1;
        }

        public ICommand ReverseSentenceCommand { get; }
        private void ReverseSentence(object parameter)
        {
            if (string.IsNullOrEmpty(Input))
                return;

            ReverseSentence();
        }

        private void ReverseSentence()
        {
            Output = string.Join(" ", Input.Split(' ').Reverse()).Trim();
        }

        public ICommand FlipCommand { get; }
        private void FlipControls(object parameter)
        {
            ScaleValue *= -1;
        }

        //ScaleX property of ScaleTransform in a control, using to flip controls for mirror effect
        public double ScaleValue
        {
            get { return _scaleValue; }
            set { SetProperty(ref _scaleValue, value); }
        }

        public string Input
        {
            get { return _input; }
            set { SetProperty(ref _input, value); }
        }

        public string Output
        {
            get { return _output; }
            set { SetProperty(ref _output, value); }
        }
    }
}

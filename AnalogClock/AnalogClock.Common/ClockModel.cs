using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace AnalogClock.Common
{
    public class ClockModel : INotifyPropertyChanged
    {
        DateTime dateTime;
        float hourAngle, minuteAngle, secondAngle;

        public event PropertyChangedEventHandler PropertyChanged;

        public ClockModel()
        {
            this.DateTime = DateTime.Now;
            Timer();
        }

        // Two set/get properties without change notifiations
        public bool NeedsRadians { set; get; }
        public bool IsSweepSecondHand { set; get; }

        // Remaining properties are get-only with change notifications
        public DateTime DateTime
        {
            protected set
            {
                if (this.dateTime != value)
                {
                    this.dateTime = value;
                    OnPropertyChanged("DateTime");
                    this.HourAngle = 30 * dateTime.Hour + 0.5f * dateTime.Minute;
                    this.MinuteAngle = 6 * dateTime.Minute + 0.1f * dateTime.Second;
                    this.SecondAngle = 6 * dateTime.Second +
                        (this.IsSweepSecondHand ? 0.006f * dateTime.Millisecond : 0);
                }
            }

            get { return this.dateTime; }
        }

        public float HourAngle
        {
            protected set
            {
                if (this.hourAngle != value)
                {
                    this.hourAngle = value;
                    OnPropertyChanged("HourAngle");
                }
            }

            get { return Convert(this.hourAngle); }
        }

        public float MinuteAngle
        {
            protected set
            {
                if (this.minuteAngle != value)
                {
                    this.minuteAngle = value;
                    OnPropertyChanged("MinuteAngle");
                }
            }

            get { return Convert(this.minuteAngle); }
        }

        public float SecondAngle
        {
            protected set
            {
                if (this.secondAngle != value)
                {
                    this.secondAngle = value;
                    OnPropertyChanged("SecondAngle");
                }
            }

            get { return Convert(this.secondAngle); }
        }

        float Convert(float degrees)
        {
            return this.NeedsRadians ? (float)Math.PI * degrees / 180 : degrees;
        }

        async void Timer()
        {
            while (true)
            {
                await Task.Delay(this.IsSweepSecondHand ? 100 : 1000);
                this.DateTime = DateTime.Now;
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

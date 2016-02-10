using System;
using Pipeline.Configuration;
using Pipeline.Extensions;

namespace DataProfiler {
    public class FieldProfile {
        private readonly int _displayLimit;
        private object _minValue;
        private object _maxValue;

        public FieldProfile(int displayLimit) {
            _displayLimit = displayLimit;
        }

        public Field Field { get; set; }

        public object MinValue
        {
            get { return _minValue ?? string.Empty; }
            set
            {
                if (value != null) {
                    if (_displayLimit < 36 && value is Guid || value is string && value.ToString().Length > _displayLimit) {
                        value = value.ToString().Left(_displayLimit) + "...";
                    }
                    _minValue = value;
                }

            }
        }

        public object MaxValue
        {
            get { return _maxValue ?? string.Empty; }
            set
            {
                if (value != null) {
                    if (_displayLimit < 36 && value is Guid || value is string && value.ToString().Length > _displayLimit) {
                        value = value.ToString().Left(_displayLimit) + "...";
                    }
                    _maxValue = value;
                }
            }
        }

        public int MinLength { get; set; }
        public int MaxLength { get; set; }
        public int Count { get; set; }
        public int Position { get; set; }
    }
}
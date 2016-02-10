using System;
using Pipeline;
using Pipeline.Configuration;

namespace DataProfiler {
    class FieldRow {
        private short _isComparable = 0;
        public Field Field { get; }
        public Row Row { get; }

        public FieldRow(Field f, Row r) {
            Field = f;
            Row = r;
        }

        public bool IsComparable() {
            if (_isComparable == 0) {
                _isComparable = Row[Field] is IComparable ? (short)1 : (short)-1;
            }
            return _isComparable > 0;
        }
    }
}
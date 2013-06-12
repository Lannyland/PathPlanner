using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using rtwmatrix;

namespace TCPIPTest
{
    public class DistPoint
    {
        public int row;
        public int column;

        #region Constructor, Destructor

        // Constructor
        public DistPoint(int _row, int _column)
        {
            row = _row;
            column = _column;
        }

        public DistPoint()
        {
        }

        // Destructor
        ~DistPoint()
        {
        }

        #endregion
    }
}

﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CMcG.CodeAlignment.Business
{
    public class GeneralScopeSelector : IScopeSelector
    {
        public string ScopeSelectorRegex { get; set; }
        public int?   Start              { get; set; }
        public int?   End                { get; set; }

        public IEnumerable<ILine> GetLinesToAlign(IDocument view)
        {
            int start = Start ?? view.StartSelectionLineNumber;
            int end   = End   ?? view.EndSelectionLineNumber;

            if (start == end)
            {
                var blanks = start.DownTo(0).Where(x => IsLineBlank(view, x));
                start      = blanks.Any() ? blanks.First() + 1 : 0;

                blanks     = end.UpTo(view.LineCount - 1).Where(x => IsLineBlank(view, x));
                end        = blanks.Any() ? blanks.First() - 1 : view.LineCount -1;
            }

            return start.UpTo(end).Select(x => view.GetLineFromLineNumber(x));
        }

        bool IsLineBlank(IDocument view, int lineNo)
        {
            return Regex.IsMatch(view.GetLineFromLineNumber(lineNo).Text, ScopeSelectorRegex);
        }
    }
}
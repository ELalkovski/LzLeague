namespace LzLeague.Common.AdminBindingModels
{
    using System;
    using System.Collections.Generic;

    public class MatchGroupDisplayModel
    {
        public DateTime? Date { get; set; }

        public ICollection<MatchBindingModel> Matches { get; set; }
    }
}

using System;
using Xamarin.UITest.Queries;

namespace Tasky.UITest
{
    public class ScreenQuery 
    {

        public static ScreenQuery ForId(string id)
        {
            return new ScreenQuery(id, c => c.Id(id));
        }

        public ScreenQuery(string name, Func<AppQuery, AppQuery> query)
        {
            Name = name.Trim();
            Query = query;
        }
        public string Name { get; private set; }
        public Func<AppQuery, AppQuery> Query { get; private set; }

        public static implicit operator Func<AppQuery, AppQuery>(ScreenQuery scrQuery)
        {
            return scrQuery.Query;
        }
    }
    
}

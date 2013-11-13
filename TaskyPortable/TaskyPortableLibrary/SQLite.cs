//
// Copyright (c) 2009-2011 Krueger Systems, Inc.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;

namespace Tasky.DL.SQLite {
    public class PrimaryKeyAttribute : Attribute {
    }

    public class AutoIncrementAttribute : Attribute {
    }

    public class IndexedAttribute : Attribute {
    }

    public class IgnoreAttribute : Attribute {
    }

    public class MaxLengthAttribute : Attribute {
        public int Value { get; private set; }

        public MaxLengthAttribute(int length)
        {
            Value = length;
        }
    }

    public class CollationAttribute : Attribute {
        public string Value { get; private set; }

        public CollationAttribute(string collation)
        {
            Value = collation;
        }
    }
}
namespace Tasky.DL.SQLiteBase {

    public abstract class SQLiteConnection : IDisposable {
       
        public string DatabasePath { get; private set; }

        public bool TimeExecution { get; set; }

        public bool Trace { get; set; }

        public SQLiteConnection(string databasePath)
        {
            DatabasePath = databasePath;
        }

        public abstract int CreateTable<T>();
 
        public abstract SQLiteCommand CreateCommand(string cmdText, params object[] ps);

        public abstract int Execute(string query, params object[] args);

        public abstract List<T> Query<T>(string query, params object[] args) where T : new();
        
        public abstract IEnumerable<T> DeferredQuery<T>(string query, params object[] args) where T : new();
        
        public abstract List<object> Query(TableMapping map, string query, params object[] args);
        
        public abstract IEnumerable<object> DeferredQuery(TableMapping map, string query, params object[] args);
        
        public abstract TableQuery<T> Table<T>() where T : new();
        
        public abstract T Get<T>(object pk) where T : new();
        
        public bool IsInTransaction { get; protected set; }

        public abstract void BeginTransaction();
        
        public abstract void Rollback();
        
        public abstract void Commit();
        
        public abstract void RunInTransaction(Action action);
        
        public abstract int InsertAll(System.Collections.IEnumerable objects);
        
        public abstract int Insert(object obj);
        
        public abstract int Insert(object obj, Type objType);
        
        public abstract int Insert(object obj, string extra);
        
        public abstract int Insert(object obj, string extra, Type objType);
        
        public abstract int Update(object obj);
        
        public abstract int Update(object obj, Type objType);
        
        public abstract int Delete<T>(T obj);
        
        public void Dispose()
        {
            Close();
        }

        public abstract void Close();
    }

    public abstract class TableMapping {}

    public abstract class SQLiteCommand {}

    public abstract class TableQuery<T> : IEnumerable<T> where T : new() {
        public virtual IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}


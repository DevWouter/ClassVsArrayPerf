using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace ClassVsArrayPerf
{
    static class Program
    {
        static void Main(string[] args)
        {
            var summaryFromIEnumerable = BenchmarkRunner.Run<ListVsArray_FromIEnumerable>();
            var summaryWithSkip = BenchmarkRunner.Run<ListVsArray_FromIEnumerableWithSkipQ>();
            var summaryWithTakeWhile = BenchmarkRunner.Run<ListVsArray_FromIEnumerableWithTakeWhileQ>();
            var summaryWithWhere = BenchmarkRunner.Run<ListVsArray_FromIEnumerableWithWhereQ>();
        }
    }

    public class ListVsArray_FromIEnumerableWithTakeWhileQ
    {
        private const int N = 10000;
        private static readonly IEnumerable<int> _source;
        private static readonly int _take;

        static ListVsArray_FromIEnumerableWithTakeWhileQ()
        {
            _take = new Random().Next(900, 950);
            _source = Enumerable.Range(0, N);
        }

        [Benchmark]
        public int[] ToArray() => _source.TakeWhile(x => x < _take).ToArray();

        [Benchmark]
        public List<int> ToList() => _source.TakeWhile(x => x < _take).ToList();
    }

    public class ListVsArray_FromIEnumerableWithWhereQ
    {
        private const int N = 10000;
        private static readonly IEnumerable<int> _source;
        private static readonly int _take;

        static ListVsArray_FromIEnumerableWithWhereQ()
        {
            _take = new Random().Next(900, 950);
            _source = Enumerable.Range(0, N);
        }

        [Benchmark]
        public int[] ToArray() => _source.Where(x => x < _take).ToArray();

        [Benchmark]
        public List<int> ToList() => _source.Where(x => x < _take).ToList();
    }

    public class ListVsArray_FromIEnumerableWithSkipQ
    {
        private const int N = 10000;
        private static readonly IEnumerable<int> _source;
        private static readonly int _skip;

        static ListVsArray_FromIEnumerableWithSkipQ()
        {
            _skip = new Random().Next(50, 100);
            _source = Enumerable.Range(0, N);
        }

        [Benchmark]
        public int[] ToArray() => _source.Skip(_skip).ToArray();

        [Benchmark]
        public List<int> ToList() => _source.Skip(_skip).ToList();
    }

    public class ListVsArray_FromIEnumerable
    {
        private const int N = 10000;
        private static readonly IEnumerable<int> _source;

        static ListVsArray_FromIEnumerable()
        {
            _source = Enumerable.Range(0, N);
        }

        [Benchmark]
        public int[] ToArray() => _source.ToArray();

        [Benchmark]
        public List<int> ToList() => _source.ToList();
    }
}
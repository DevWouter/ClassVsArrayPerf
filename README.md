# ToArray vs ToList

## Performance testing

``` ini
BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1052 (21H1/May2021Update)
Intel Core i7-10700KF CPU 3.80GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK=5.0.204
  [Host]     : .NET 5.0.7 (5.0.721.25508), X64 RyuJIT
  DefaultJob : .NET 5.0.7 (5.0.721.25508), X64 RyuJIT
```

### ListVsArray_FromIEnumerable

| Method  |      Mean |     Error |    StdDev |
| ------- | --------: | --------: | --------: |
| ToArray |  4.186 μs | 0.0165 μs | 0.0146 μs |
| ToList  | 11.550 μs | 0.0980 μs | 0.0869 μs |

```cs
[Benchmark] public int[] ToArray() => _source.ToArray();
[Benchmark] public List<int> ToList() => _source.ToList();
```

### ListVsArray_FromIEnumerableWithSkipQ

| Method  |      Mean |     Error |    StdDev |
| ------- | --------: | --------: | --------: |
| ToArray |  4.161 μs | 0.0296 μs | 0.0277 μs |
| ToList  | 11.487 μs | 0.1459 μs | 0.1365 μs |

* `_skip` is a static random value between `50` and `100` and set once when the application starts
* `_source` is set using `Enumerable.Range(0, N)` where `N` is `10000`

```cs
[Benchmark] public int[] ToArray() => _source.Skip(_skip).ToArray();
[Benchmark] public List<int> ToList() => _source.Skip(_skip).ToList();
```

### ListVsArray_FromIEnumerableWithTakeWhileQ

| Method  |     Mean |     Error |    StdDev |
| ------- | -------: | --------: | --------: |
| ToArray | 8.694 μs | 0.0413 μs | 0.0387 μs |
| ToList  | 8.310 μs | 0.0850 μs | 0.0754 μs |

* `_take` is a static random value between `900` and `950` and set once when the application starts
* `_source` is set using `Enumerable.Range(0, N)` where `N` is `10000`

```cs
[Benchmark] public int[] ToArray() => _source.TakeWhile(x => x < _take).ToArray();
[Benchmark] public List<int> ToList() => _source.TakeWhile(x => x < _take).ToList();
```

### ListVsArray_FromIEnumerableWithWhereQ

| Method  |     Mean |    Error |   StdDev |
| ------- | -------: | -------: | -------: |
| ToArray | 46.27 μs | 0.169 μs | 0.158 μs |
| ToList  | 45.76 μs | 0.392 μs | 0.347 μs |

* `_take` is a static random value between `900` and `950` and set once when the application starts
* `_source` is set using `Enumerable.Range(0, N)` where `N` is `10000`

```cs
[Benchmark] public int[] ToArray() => _source.Where(x => x < _take).ToArray();
[Benchmark] public List<int> ToList() => _source.Where(x => x < _take).ToList();
```

## Full source code

```cs
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
```

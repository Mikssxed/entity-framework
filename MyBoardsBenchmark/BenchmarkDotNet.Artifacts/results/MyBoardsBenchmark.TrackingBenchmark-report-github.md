```

BenchmarkDotNet v0.15.4, Windows 11 (10.0.26100.6899/24H2/2024Update/HudsonValley)
AMD Ryzen 5 3600 3.60GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.305
  [Host]     : .NET 9.0.9 (9.0.9, 9.0.925.41916), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 9.0.9 (9.0.9, 9.0.925.41916), X64 RyuJIT x86-64-v3


```
| Method          | Mean     | Error     | StdDev    | Median   | Gen0     | Gen1    | Allocated  |
|---------------- |---------:|----------:|----------:|---------:|---------:|--------:|-----------:|
| WithTracking    | 1.633 ms | 0.0436 ms | 0.1252 ms | 1.598 ms | 148.4375 | 70.3125 | 1248.19 KB |
| WithoutTracking | 1.070 ms | 0.0179 ms | 0.0150 ms | 1.070 ms | 101.5625 | 35.1563 |  829.71 KB |

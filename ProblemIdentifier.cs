using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PathCleaner
{
    class ProblemIdentifier
    {

        public static List<PathProblem> FindProblems(List<Func<string, Maybe<string>, Result>> funcs, Path path)
        {
            var pat = path.Folders.Select(x => x.IndexOf('%') >= 0 ? Environment.ExpandEnvironmentVariables(x) : x).ToList();

            var k = new List<Maybe<string>>();

            k.Add(Maybe<string>.None);

            k.AddRange(pat.Select(x => Maybe<string>.From(x)));

            var p = pat.Zip(k, (x, y) => Tuple.Create<string, Maybe<string>>(x, y)).ToList();

            var t = (from a in funcs
                     from b in p
                     select Tuple.Create(b.Item1, b.Item2, a)).ToList();

            var s = t.Where(x => x.Item3(x.Item1, x.Item2).IsFailure).Select(x => new PathProblem
            {
                Path = x.Item1,
                Reason = x.Item3(x.Item1, x.Item2).Error
            }).ToList();

            return s;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.NUnit;

namespace XSharpx.Tests
{
    [NUnit.Framework.TestFixture]
    class OptionTest
    {
        [Property(Arbitrary= new Type[]{ typeof(XSharpxArbs) })]
        public bool MonadLeftIdentityLaw(Func<int, Option<int>> f, int a)
        {
            var left = Option.Some(a).SelectMany(f);
            var right = f(a);
            return left
                .ZipWith<int, bool>(right, x => y => x == y)
                .Fold(x => x, () => left.IsEmpty && right.IsEmpty);
        }

        [Property(Arbitrary= new Type[]{ typeof(XSharpxArbs) })]
        public bool MonadRightIdentityLaw(Option<int> a)
        {
            var left = a.SelectMany(Option.Some);
            return left
                .ZipWith<int, bool>(a, x => y => x == y)
                .Fold(x => x, () => left.IsEmpty && a.IsEmpty);
        }

        [Property(Arbitrary= new Type[]{ typeof(XSharpxArbs) })]
        public bool MonadAssociativityLaw(Func<int, Option<int>> f, Func<int, Option<int>> g, Option<int> m)
        {
            var left = m.SelectMany(f).SelectMany(g);
            var right = m.SelectMany(x => f(x).SelectMany(g));
            return left
                .ZipWith<int, bool>(right, x => y => x == y)
                .Fold(x => x, () => left.IsEmpty && right.IsEmpty);
        }
    }

    public class OptionArb<A> : Arbitrary<Option<A>>
    {
        public override Gen<Option<A>> Generator
        {
            get
            {
                return Gen.OneOf(
                    Arb.From<A>().Generator.Select(Option.Some),
                    Gen.Constant(Option<A>.Empty)
                    );
            }
        }
    }

    public class XSharpxArbs
    {
        public static Arbitrary<Option<A>> Option<A>() { return new OptionArb<A>(); }
    }
}

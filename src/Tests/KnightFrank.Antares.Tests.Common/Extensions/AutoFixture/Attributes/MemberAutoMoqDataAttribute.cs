namespace KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Xunit;
    using Xunit.Sdk;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    [DataDiscoverer("Xunit.Sdk.MemberDataDiscoverer", "xunit.core")]
    public sealed class MemberAutoMoqDataAttribute : MemberDataAttributeBase
    {
        public MemberAutoMoqDataAttribute(string memberName, params object[] parameters)
            : base(memberName, parameters)
        {
        }

        protected override object[] ConvertDataItem(MethodInfo testMethod, object item)
        {
            if (item == null)
            {
                return null;
            }

            var autoMoqData = new InlineAutoMoqDataAttribute(item as object[]);
            return autoMoqData.GetData(testMethod).FirstOrDefault();
        }
    }
}

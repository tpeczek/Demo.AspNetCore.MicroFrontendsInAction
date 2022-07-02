using System.Net;
using System.Text.RegularExpressions;

namespace Demo.AspNetCore.MicroFrontendsInAction.Proxy.Transforms
{
    internal interface ISsiDirective
    {
        string Directive { get; }

        public IReadOnlyDictionary<string, string> Parameters { get; }

        public int Index { get; }

        public int Length { get; }
    }

    internal static class SsiParser
    {
        // <!--#directive parameter1="value1" parameter2="value2" ... -->
        private static readonly Regex SSI_DIRECTIVE_REGEX = new Regex(@"<!--\#([a-z]+)\b([^>]+[^\/>])?-->", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        private const int SSI_DIRECTIVE_GROUP_INDEX = 1;
        private const int SSI_PARAMETERS_GROUP_INDEX = 2;

        private static readonly Regex SSI_PARAMETER_REGEX = new Regex(@"\b([^\s=]+)=""([^""""]*)""", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        private const int SSI_PARAMETER_NAME_INDEX = 1;
        private const int SSI_PARAMETER_VALUE_INDEX = 2;

        private class SsiDirective : ISsiDirective
        {
            private readonly Match _ssiDirectiveMatch;

            public string Directive => _ssiDirectiveMatch.Groups[SSI_DIRECTIVE_GROUP_INDEX].Value;

            public IReadOnlyDictionary<string, string> Parameters { get; }

            public int Index => _ssiDirectiveMatch.Index;

            public int Length => _ssiDirectiveMatch.Length;

            public SsiDirective(Match ssiDirectiveMatch)
            {
                _ssiDirectiveMatch = ssiDirectiveMatch;

                if (_ssiDirectiveMatch.Groups.Count > SSI_PARAMETERS_GROUP_INDEX)
                {
                    Parameters = SSI_PARAMETER_REGEX.Matches(_ssiDirectiveMatch.Groups[SSI_PARAMETERS_GROUP_INDEX].Value).ToDictionary(
                        ssiParameterMatch => ssiParameterMatch.Groups[SSI_PARAMETER_NAME_INDEX].Value,
                        ssiParameterMatch => WebUtility.HtmlDecode(ssiParameterMatch.Groups[SSI_PARAMETER_VALUE_INDEX].Value), StringComparer.OrdinalIgnoreCase
                    );
                }
                else
                {
                    Parameters = new Dictionary<string, string>();
                }
            }
        }

        public static IList<ISsiDirective> ParseDirectives(string content)
        {
            return SSI_DIRECTIVE_REGEX.Matches(content)
                .Select(ssiDirectiveMatch => (ISsiDirective)new SsiDirective(ssiDirectiveMatch))
                .ToList();
        }
    }
}

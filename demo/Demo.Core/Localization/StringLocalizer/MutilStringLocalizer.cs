using Microsoft.Extensions.Localization;

using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Demo.Localization.StringLocalizer
{
    /// <summary>
    /// 拥有多种数据源的StringLocalizer
    /// </summary>
    public class MutilStringLocalizer : IStringLocalizer, IEnumerable<IStringLocalizer>
    {
        private readonly IStringLocalizer[] _localizers;

        public MutilStringLocalizer(IStringLocalizer[] localizers)
        {
            _localizers = localizers;
        }

        public LocalizedString this[string name]
        {
            get
            {
                foreach (var localizer in _localizers)
                {
                    LocalizedString localizedString = localizer.GetString(name);
                    if (localizedString != null && !localizedString.ResourceNotFound)
                        return localizedString;
                }

                return new LocalizedString(name, string.Empty, true);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                foreach (var localizer in _localizers)
                {
                    LocalizedString localizedString = localizer.GetString(name, arguments);
                    if (localizedString != null && !localizedString.ResourceNotFound)
                        return localizedString;
                }

                return new LocalizedString(name, string.Empty, true);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            IEnumerable<LocalizedString> list = null;
            foreach (var localizer in _localizers)
            {
                list = localizer.GetAllStrings(includeParentCultures);
                if (list != null && list.Any())
                    break;
            }

            return list;
        }

        public IEnumerator<IStringLocalizer> GetEnumerator()
        {
            return ((IEnumerable<IStringLocalizer>)_localizers).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _localizers.GetEnumerator();
        }
    }
}

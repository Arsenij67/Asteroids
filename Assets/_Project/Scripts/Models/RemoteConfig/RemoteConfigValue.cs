using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Asteroid.Services.RemoteConfig
{

    public struct ConfigValue
    {
        internal static Regex booleanTruePattern = new Regex("^(1|true|t|yes|y|on)$", RegexOptions.IgnoreCase);

        internal static Regex booleanFalsePattern = new Regex("^(0|false|f|no|n|off|)$", RegexOptions.IgnoreCase);

        public bool BooleanValue
        {
            get
            {
                string stringValue = StringValue;
                if (booleanTruePattern.IsMatch(stringValue))
                {
                    return true;
                }

                if (booleanFalsePattern.IsMatch(stringValue))
                {
                    return false;
                }

                throw new FormatException($"ConfigValue '{stringValue}' is not a boolean value");
            }
        }

        public IEnumerable<byte> ByteArrayValue => Data;

        public double DoubleValue => Convert.ToDouble(StringValue, CultureInfo.InvariantCulture);

        public long LongValue => Convert.ToInt64(StringValue, CultureInfo.InvariantCulture);

        public string StringValue => Encoding.UTF8.GetString(Data);

        internal byte[] Data { get; set; }

        public ValueSource Source { get; internal set; }

        internal ConfigValue(byte[] data, ValueSource source)
        {
            this = default(ConfigValue);
            Data = data;
            Source = source;
        }
    }
}
#if false // Журнал декомпиляции
Элементов в кэше: "310"
------------------
Разрешить: "mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
Найдена одна сборка: "mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
Загрузить из: "C:\Program Files\Unity\Hub\Editor\6000.0.50f1\Editor\Data\NetStandard\compat\2.1.0\shims\netfx\mscorlib.dll"
------------------
Разрешить: "System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
Найдена одна сборка: "System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
Загрузить из: "C:\Program Files\Unity\Hub\Editor\6000.0.50f1\Editor\Data\NetStandard\compat\2.1.0\shims\netfx\System.dll"
------------------
Разрешить: "Firebase.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
Найдена одна сборка: "Firebase.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
Загрузить из: "C:\Users\Asteroids\Assets\Firebase\Plugins\Firebase.App.dll"
------------------
Разрешить: "Microsoft.Win32.Registry, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
Не удалось найти по имени: "Microsoft.Win32.Registry, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
------------------
Разрешить: "netstandard, Version=2.1.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51"
Найдена одна сборка: "netstandard, Version=2.1.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51"
Загрузить из: "C:\Program Files\Unity\Hub\Editor\6000.0.50f1\Editor\Data\NetStandard\ref\2.1.0\netstandard.dll"
------------------
Разрешить: "System.Security.Principal.Windows, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
Не удалось найти по имени: "System.Security.Principal.Windows, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
------------------
Разрешить: "System.Security.AccessControl, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
Не удалось найти по имени: "System.Security.AccessControl, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
------------------
Разрешить: "System.CodeDom, Version=4.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51"
Не удалось найти по имени: "System.CodeDom, Version=4.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51"
#endif

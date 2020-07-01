## VWO .NET SDK Example

VWO .NET SDK allows you to A/B Test your Website at server-side.

This repository provides a basic demo of how FullStack works with VWO NetStandard SDK.

### Requirements

- NetStandard 2.0 or later

### Documentation

Refer [VWO Official FullStack Documentation](https://developers.vwo.com/reference#fullstack-introduction)
### Scripts

1. Install dependencies

```
dotnet restore
```

2. Update your app with essential params inside `VWOConfig.cs`

```c#
using System.Collections.Generic;

namespace VWOSdk.DemoApp
{
    public class VWOConfig
    {
        internal static class SDK {
            public static long AccountId = "";          ////Assign actual value;
            public static string SdkKey = "";
        }
        internal static class ABCampaignSettings {        ////Assign actual value;
            public static string CampaignKey = "";          ////Assign actual value;
            public static string GoalIdentifier = "";          ////Assign actual value;
            public static Dictionary<string, dynamic> Options = new Dictionary<string, dynamic>();
        }

        internal static class PushData {
            public static string TagKey = "";
            public static dynamic TagValue = "";
        }

        internal static class FeatureRolloutData {
            public static string CampaignKey = "";
            public static Dictionary<string, dynamic> Options = new Dictionary<string, dynamic>();
        }

        internal static class FeatureTestData {
            public static string CampaignKey = "";
            public static string GoalIdentifier = "";
            public static Dictionary<string, dynamic> Options = new Dictionary<string, dynamic>();
            public static string StringVariableKey = "";
            public static string IntegerVariableKey = "";
            public static string DoubleVariableKey = "";
            public static string BooleanVariableKey = "";
        }
    }
}

```

3. Run application

```
dotnet run --project VWOSdk.DemoApp/VWOSdk.DemoApp.csproj
```

### LICENSE

```text
    MIT License

    Copyright (c) 2019 Wingify Software Pvt. Ltd.

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
```

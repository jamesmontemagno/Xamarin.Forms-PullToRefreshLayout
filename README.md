PullToRefreshLayout for Xamarin.Forms
===================================

Implementation of pull to refresh layout for Xamarin.Forms targeting iOS and Android.

The goal was to create a cross-platform Xamarin.Forms Layout that could have it’s content set to anything and would enable pull to refresh capabilities. 

A BIG thank you to [Jason Smith](https://twitter.com/jassmith87) for his assistance on some tricky logic.

For a detailed overview of the API and sample please read through my blog:
http://motzcod.es/post/128274430137/pull-to-refresh-anyish-xamarinforms-view

Also available as a NuGet: https://www.nuget.org/packages/Refractored.XamForms.PullToRefresh/

**Important**

If you are using the NuGet ensure that you call “PullToRefreshLayoutRenderer.Init();” in both your MainActivity and AppDelegate. (Similar to Xamarin.Forms.Maps).



![](demo.gif)

### API
Simply create a PullToRefreshLayout like any other Layout and set the Content to a supported view.


```csharp
var scrollView = new ScrollView
{
  VerticalOptions = LayoutOptions.FillAndExpand,
  HorizontalOptions = LayoutOptions.FillAndExpand,
  Content = /* Anything you want in your ScrollView */
};

var refreshView = new PullToRefreshLayout {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Content = scrollView,
                RefreshColor = Color.FromHex("#3498db")
            };

//Set Bindings
refreshView.SetBinding<TestViewModel> (PullToRefreshLayout.IsRefreshingProperty, vm => vm.IsBusy, BindingMode.OneWay);
refreshView.SetBinding<TestViewModel>(PullToRefreshLayout.RefreshCommandProperty, vm => vm.RefreshCommand);

Content = refreshView;
```

Additionally, your content could be anything you want including a StackLayout:

```csharp
Content = new StackLayout
{
  Children = 
  {
    new Label { Text = “Hello Pull to Refresh“ },
    refreshView
  }
};
```

You can of course do it in XAML:
```
<?xml version="1.0" encoding="UTF-8"?>
<ContentPage 
    xmlns="http://xamarin.com/schemas/2014/forms" 
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" 
    x:Class="RefreshSample.Views.ScrollViewXamlPage"
    xmlns:controls="clr-namespace:Refractored.XamForms.PullToRefresh;assembly=Refractored.XamForms.PullToRefresh"
    Title="Xaml Scroll">
   <controls:PullToRefreshLayout
          IsPullToRefreshEnabled="True"
          RefreshCommand="{Binding RefreshCommand}"
          IsRefreshing="{Binding IsBusy}"
          RefreshColor="Blue"> 
          <ScrollView
          HorizontalOptions="FillAndExpand"
          VerticalOptions="FillAndExpand">
            <StackLayout
                  HorizontalOptions="FillAndExpand"
                  VerticalOptions="FillAndExpand">
                <BoxView HeightRequest="100" WidthRequest="100" BackgroundColor="Blue"/>
                <BoxView HeightRequest="100" WidthRequest="100" BackgroundColor="Red"/>
                <BoxView HeightRequest="100" WidthRequest="100" BackgroundColor="Yellow"/>
                <BoxView HeightRequest="100" WidthRequest="100" BackgroundColor="Purple"/>
                <BoxView HeightRequest="100" WidthRequest="100" BackgroundColor="Maroon"/>
                <BoxView HeightRequest="100" WidthRequest="100" BackgroundColor="Blue"/>
                <BoxView HeightRequest="100" WidthRequest="100" BackgroundColor="Red"/>
                <BoxView HeightRequest="100" WidthRequest="100" BackgroundColor="Yellow"/>
                <BoxView HeightRequest="100" WidthRequest="100" BackgroundColor="Purple"/>
                <BoxView HeightRequest="100" WidthRequest="100" BackgroundColor="Maroon"/>
            </StackLayout>
          </ScrollView>
   </controls:PullToRefreshLayout>
</ContentPage>

```

### Officially Supported Views:
* ListView
* ScrollView

### Unofficial Supported Views
* UICollectionView (iOS)
* RecyclerView (Android)
* GridView (Android)
* Potentially all Android Views without a scroll, but it acts a bit odd on some
* ^^You should probably use a ScrollView^^

### License
The MIT License (MIT)

Copyright (c) 2015 Refractored LLC & James Montemagno

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.




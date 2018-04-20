/*
 * Copyright (C) 2015 Refractored LLC & James Montemagno: 
 * http://github.com/JamesMontemagno
 * http://twitter.com/JamesMontemagno
 * http://refractored.com
 * 
 * The MIT License (MIT) see GitHub For more information
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using Xamarin.Forms;
using RefreshSample.ViewModels;
using Refractored.XamForms.PullToRefresh;

namespace RefreshSample.Views
{
    public class GridPage : ContentPage
    {
        public GridPage(bool insideLayout)
        {
            var random = new Random();
            Title = "StackLayout (Pull to Refresh)";

            BindingContext = new TestViewModel(this);

            var grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowSpacing = 0,
                ColumnSpacing = 0
            };

            for (int i = 0; i < 20; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                for (int j = 0; j < 20; j++)
                {

                    if (i == 0)
                    {
                        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                    }

                    grid.Children.Add(new BoxView
                    {
                        HeightRequest = 50,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        BackgroundColor = Color.FromRgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255))
                    }, i, j);
                }

            }

            var refreshView = new PullToRefreshLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Content = grid,
                RefreshColor = Color.FromHex("#3498db")
            };

            refreshView.SetBinding<TestViewModel>(PullToRefreshLayout.IsRefreshingProperty, vm => vm.IsBusy, BindingMode.OneWay);
            refreshView.SetBinding<TestViewModel>(PullToRefreshLayout.RefreshCommandProperty, vm => vm.RefreshCommand);


            if (insideLayout)
            {
                var activity = new ActivityIndicator();
                activity.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy");

                Content = new StackLayout
                {
                    Spacing = 0,
                    Children =
                            {
                                activity,
                                new Label
                                {
                                    TextColor = Color.White,
                                    Text = "In a StackLayout",
                                    FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Label)),
                                    BackgroundColor = Color.FromHex("#3498db"),
                                    XAlign = TextAlignment.Center,
                                    HorizontalOptions = LayoutOptions.FillAndExpand
                                },
                                refreshView
                            }
                };
            }
            else
            {
                Content = refreshView;
            }
        }
    }
}


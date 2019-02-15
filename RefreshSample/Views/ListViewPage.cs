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
using System.Threading.Tasks;

namespace RefreshSample.Views
{
    public class ListViewPage : ContentPage
    {
        public ListViewPage(bool insideLayout)
        {
            Title = "ListView (Pull to Refresh)";

            BindingContext = new TestViewModel(this);

            var listView = new ListView();
            //ListView now has a built in pull to refresh! 
            //You most likely could enable the listview pull to refresh and use it instead of the refresh view
            //listView.IsPullToRefreshEnabled = true;
            //
            //listView.SetBinding<TestViewModel>(ListView.IsRefreshingProperty, vm => vm.IsBusy, BindingMode.OneWay);
            //listView.SetBinding<TestViewModel>(ListView.RefreshCommandProperty, vm => vm.RefreshCommand);



            listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, new Binding("Items"));

            //ListView now has a built in pull to refresh! 
            //You most likely could disable the listview pull to refresh and re-enable this
            //However, I wouldn't recommend that.
            var refreshView = new PullToRefreshLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Content = new StackLayout
                {
                    Spacing = 0,
                    Children =
                            {
                                new Label
                                {
                                    TextColor = Color.White,
                                    Text = "In a StackLayout",
                                    FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Label)),
                                    BackgroundColor = Color.FromHex("#3498db"),
                                    VerticalTextAlignment = TextAlignment.Center,
                                    HorizontalOptions = LayoutOptions.FillAndExpand
                                },
                                listView
                            }
                },
                RefreshColor = Color.FromHex("#3498db")
            };

            refreshView.SetBinding(PullToRefreshLayout.IsRefreshingProperty, new Binding("IsBusy", BindingMode.OneWay));
            refreshView.SetBinding(PullToRefreshLayout.RefreshCommandProperty, new Binding("RefreshCommand"));
            refreshView.SetBinding(PullToRefreshLayout.IsPullToRefreshEnabledProperty, new Binding("CanRefresh"));

            if (insideLayout)
            {
                Content = new StackLayout
                {
                    Spacing = 0,
                    Children =
                            {
                                new Label
                                {
                                    TextColor = Color.White,
                                    Text = "In a StackLayout",
                                    FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Label)),
                                    BackgroundColor = Color.FromHex("#3498db"),
                                    VerticalTextAlignment = TextAlignment.Center,
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


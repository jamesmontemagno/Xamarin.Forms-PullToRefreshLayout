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

using System.Windows.Input;
using Xamarin.Forms;

namespace Refractored.XamForms.PullToRefresh
{
    /// <summary>
    /// Pull to refresh layout.
    /// </summary>
    public class PullToRefreshLayout: ContentView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Refractored.XamForms.PullToRefresh.PullToRefreshLayout"/> class.
        /// </summary>
        public PullToRefreshLayout()
        {
            IsClippedToBounds = true;
            VerticalOptions = LayoutOptions.FillAndExpand;
            HorizontalOptions = LayoutOptions.FillAndExpand;
        }

        /// <summary>
        /// The is refreshing property.
        /// </summary>
        public static readonly BindableProperty IsRefreshingProperty = 
            BindableProperty.Create<PullToRefreshLayout,bool> (
                p => p.IsRefreshing, false,BindingMode.OneWay);

        /// <summary>
        /// Gets or sets a value indicating whether this instance is refreshing.
        /// </summary>
        /// <value><c>true</c> if this instance is refreshing; otherwise, <c>false</c>.</value>
        public bool IsRefreshing {
            get { return (bool)GetValue (IsRefreshingProperty); }
            set { SetValue (IsRefreshingProperty, value); }
        }

        /// <summary>
        /// The is pull to refresh enabled property.
        /// </summary>
        public static readonly BindableProperty IsPullToRefreshEnabledProperty = 
            BindableProperty.Create<PullToRefreshLayout,bool> (
                p => p.IsPullToRefreshEnabled, true);

        /// <summary>
        /// Gets or sets a value indicating whether this instance is pull to refresh enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is pull to refresh enabled; otherwise, <c>false</c>.</value>
        public bool IsPullToRefreshEnabled {
            get { return (bool)GetValue (IsPullToRefreshEnabledProperty); }
            set { SetValue (IsPullToRefreshEnabledProperty, value); }
        }

        /// <summary>
        /// The refresh command property.
        /// </summary>
        public static readonly BindableProperty RefreshCommandProperty = 
            BindableProperty.Create<PullToRefreshLayout,ICommand> (
                p => p.RefreshCommand, null);

        /// <summary>
        /// Gets or sets the refresh command.
        /// </summary>
        /// <value>The refresh command.</value>
        public ICommand RefreshCommand {
            get { return (ICommand)GetValue (RefreshCommandProperty); }
            set { SetValue (RefreshCommandProperty, value); }
        }

        /// <summary>
        /// Color property of refresh spinner color 
        /// </summary>
        public static readonly BindableProperty RefreshColorProperty =
            BindableProperty.Create<PullToRefreshLayout, Color>(
                p => p.RefreshColor, Color.Default);

       
        /// <summary>
        /// Refresh  color
        /// </summary>
        public Color RefreshColor
        {
            get { return (Color)GetValue(RefreshColorProperty); }
            set { SetValue(RefreshColorProperty, value); }
        }



        /// <summary>
        /// Color property of refresh background color
        /// </summary>
        public static readonly BindableProperty RefreshBackgroundColorProperty =
            BindableProperty.Create<PullToRefreshLayout, Color>(
                p => p.RefreshBackgroundColor, Color.Default);

        /// <summary>
        /// Refresh background color
        /// </summary>
        public Color RefreshBackgroundColor
        {
            get { return (Color)GetValue(RefreshBackgroundColorProperty); }
            set { SetValue(RefreshBackgroundColorProperty, value); }
        }


        /// <param name="widthConstraint">The available width for the element to use.</param>
        /// <param name="heightConstraint">The available height for the element to use.</param>
        /// <summary>
        /// Optimization as we can get the size here of our content all in DIP
        /// </summary>
        protected override SizeRequest OnSizeRequest (double widthConstraint, double heightConstraint)
        {
            if (Content == null)
                return new SizeRequest(new Size(100, 100));

            return Content.GetSizeRequest (widthConstraint, heightConstraint);
        }
    }
}


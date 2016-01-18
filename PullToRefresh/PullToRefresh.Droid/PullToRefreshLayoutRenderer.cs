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
using System.ComponentModel;
using System.Reflection;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Views;
using Refractored.XamForms.PullToRefresh;
using Refractored.XamForms.PullToRefresh.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly:ExportRenderer(typeof(PullToRefreshLayout), typeof(PullToRefreshLayoutRenderer))]
namespace Refractored.XamForms.PullToRefresh.Droid
{
    /// <summary>
    /// Pull to refresh layout renderer.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class PullToRefreshLayoutRenderer : SwipeRefreshLayout, 
        IVisualElementRenderer,
        SwipeRefreshLayout.IOnRefreshListener
    {
        /// <summary>
        /// Used for registration with dependency service
        /// </summary>
        public async static void Init()
        {
            var temp = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="Refractored.XamForms.PullToRefresh.Droid.PullToRefreshLayoutRenderer"/> class.
        /// </summary>
        public PullToRefreshLayoutRenderer()
            : base(Forms.Context)
        {
        }

        /// <summary>
        /// Occurs when element changed.
        /// </summary>
        public event EventHandler<VisualElementChangedEventArgs> ElementChanged;


        bool init;
        IVisualElementRenderer packed;

        /// <summary>
        /// Setup our SwipeRefreshLayout and register for property changed notifications.
        /// </summary>
        /// <param name="element">Element.</param>
        public void SetElement(VisualElement element)
        {
            var oldElement = Element;

            //unregister old and re-register new
            if (oldElement != null)
                oldElement.PropertyChanged -= HandlePropertyChanged;

            Element = element;
            if (Element != null)
            {
                UpdateContent();
                Element.PropertyChanged += HandlePropertyChanged;
            }

            if (!init)
            {
                init = true;
                //sizes to match the forms view
                //updates properties, handles visual element properties
                Tracker = new VisualElementTracker(this);
                SetOnRefreshListener(this);
            }

            UpdateColors();
            UpdateIsRefreshing();
            UpdateIsSwipeToRefreshEnabled();

            if (ElementChanged != null)
                ElementChanged(this, new VisualElementChangedEventArgs(oldElement, this.Element));
        }

        /// <summary>
        /// Managest adding and removing the android viewgroup to our actual swiperefreshlayout
        /// </summary>
        void UpdateContent()
        {
            if (RefreshView.Content == null)
                return;

            if (packed != null)
                RemoveView(packed.ViewGroup);

            packed = Platform.CreateRenderer (RefreshView.Content);

            try
            {
                RefreshView.Content.SetValue(RendererProperty, packed);
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Unable to sent renderer property, maybe an issue: " + ex);
            }

            AddView(packed.ViewGroup, LayoutParams.MatchParent);

        }

        BindableProperty rendererProperty = null;

        /// <summary>
        /// Gets the bindable property.
        /// </summary>
        /// <returns>The bindable property.</returns>
        BindableProperty RendererProperty
        {
            get
            {
                if (rendererProperty != null)
                    return rendererProperty;

                var type = Type.GetType("Xamarin.Forms.Platform.Android.Platform, Xamarin.Forms.Platform.Android");
                var prop = type.GetField("RendererProperty", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                var val = prop.GetValue(null);
                rendererProperty = val as BindableProperty;

                return rendererProperty;
            }
        }

        void UpdateColors()
        {
            if (RefreshView == null)
                return;
            if (RefreshView.RefreshColor != Color.Default)
                SetColorSchemeColors(RefreshView.RefreshColor.ToAndroid());
            if (RefreshView.RefreshBackgroundColor != Color.Default)
                SetProgressBackgroundColorSchemeColor(RefreshView.RefreshBackgroundColor.ToAndroid());
        }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="Refractored.XamForms.PullToRefresh.Droid.PullToRefreshLayoutRenderer"/> is refreshing.
        /// </summary>
        /// <value><c>true</c> if refreshing; otherwise, <c>false</c>.</value>
        public override bool Refreshing {
            get {
                return base.Refreshing;
            }
            set {
                //this will break binding :( sad panda we need to wait for next version for this
                //right now you can't update the binding.. so it is 1 way
                if(RefreshView != null && RefreshView.IsRefreshing != value)
                 RefreshView.IsRefreshing = value;

                if (base.Refreshing == value)
                    return;
                
                base.Refreshing = value;
            }
        }

        void UpdateIsRefreshing()
        {
            Refreshing = RefreshView.IsRefreshing;
        }

        void UpdateIsSwipeToRefreshEnabled()
        {
            Enabled = RefreshView.IsPullToRefreshEnabled;
        }


        /// <summary>
        /// Determines whether this instance can child scroll up.
        /// We do this since the actual swipe refresh can't figure it out
        /// </summary>
        /// <returns><c>true</c> if this instance can child scroll up; otherwise, <c>false</c>.</returns>
        public override bool CanChildScrollUp()
        {
            return CanScrollUp(packed.ViewGroup);
        }

        bool CanScrollUp(ViewGroup viewGroup)
        {
            if (viewGroup == null)
                return base.CanChildScrollUp();

            var sdk = (int)global::Android.OS.Build.VERSION.SdkInt;
            if (sdk >= 16)
            {
                #if __ANDROID_16__
               
                //is a scroll container such as listview, scroll view, gridview
                if (viewGroup.IsScrollContainer)
                {
                    return base.CanChildScrollUp();
                }
                #endif
            }

            //if you have something custom and you can't scroll up you might need to enable this
            //for instance on a custom recycler view where the code above isn't working!
            for (int i = 0; i < viewGroup.ChildCount; i++)
            {
                var child = viewGroup.GetChildAt(i);
                if (child is Android.Widget.AbsListView)
                {
                    var list = child as Android.Widget.AbsListView;
                    if (list != null)
                    {
                        if (list.FirstVisiblePosition == 0)
                        {
                            var subChild = list.GetChildAt(0);

                            return subChild != null && subChild.Top != 0;
                        }

                        //if children are in list and we are scrolled a bit... sure you can scroll up
                        return true;
                    }

                }
                else if (child is Android.Widget.ScrollView)
                {
                    var scrollview = child as Android.Widget.ScrollView;
                    return (scrollview.ScrollY <= 0.0);
                }
                else if (child is Android.Support.V4.Widget.SwipeRefreshLayout)
                {
                    return CanScrollUp(child as ViewGroup);
                }
                //else if something else like a recycler view?

            }

            return false;
        }


        /// <summary>
        /// Helpers to cast our element easily
        /// Will throw an exception if the Element is not correct
        /// </summary>
        /// <value>The refresh view.</value>
        public Refractored.XamForms.PullToRefresh.PullToRefreshLayout RefreshView
        {
            get { return this.Element == null ? null : (PullToRefreshLayout)Element; }
        }


        /// <summary>
        /// The refresh view has been refreshed
        /// </summary>
        public void OnRefresh()
        {
            //someone pulled down to refresh or it is done
            if (RefreshView == null)
                return;

            var command = RefreshView.RefreshCommand;
            if (command == null)
                return;

            command.Execute(null);
        }

        /// <summary>
        /// Handles the property changed.
        /// Update the control and trigger refreshing
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Content")
                UpdateContent();
            else if (e.PropertyName == PullToRefreshLayout.IsPullToRefreshEnabledProperty.PropertyName)
                UpdateIsSwipeToRefreshEnabled();
            else if (e.PropertyName == PullToRefreshLayout.IsRefreshingProperty.PropertyName)
                UpdateIsRefreshing();
            else if (e.PropertyName == PullToRefreshLayout.RefreshColorProperty.PropertyName)
                UpdateColors();
            else if (e.PropertyName == PullToRefreshLayout.RefreshBackgroundColorProperty.PropertyName)
                UpdateColors();
        }

        /// <summary>
        /// Gets the size of the desired.
        /// </summary>
        /// <returns>The desired size.</returns>
        /// <param name="widthConstraint">Width constraint.</param>
        /// <param name="heightConstraint">Height constraint.</param>
        public SizeRequest GetDesiredSize(int widthConstraint, int heightConstraint)
        {
            packed.ViewGroup.Measure(widthConstraint, heightConstraint);

            //Measure child here and determine size
            return new SizeRequest(new Size(packed.ViewGroup.MeasuredWidth, packed.ViewGroup.MeasuredHeight));
        }

        /// <summary>
        /// Updates the layout.
        /// </summary>
        public void UpdateLayout()
        {
            if (Tracker == null)
                return;

            Tracker.UpdateLayout();
        }

        /// <summary>
        /// Gets the tracker.
        /// </summary>
        /// <value>The tracker.</value>
        public VisualElementTracker Tracker
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the view group.
        /// </summary>
        /// <value>The view group.</value>
        public Android.Views.ViewGroup ViewGroup
        {
            get{ return this; }
        }

        /// <summary>
        /// Gets the element.
        /// </summary>
        /// <value>The element.</value>
        public VisualElement Element
        {
            get;
            private set;
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace praktika_2;

public partial class TreePage : ContentPage
{
    private BoxView _trunk;
    private Frame _canopy;
    private AbsoluteLayout _absoluteLayout;
    
    public TreePage()
    {
        _trunk = new BoxView
        {
            Color = Color.FromRgb(117, 89 ,62),
            WidthRequest = 70,
            HeightRequest = 150,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };

        _absoluteLayout = new AbsoluteLayout { Children = { _trunk } };

        Content = _absoluteLayout;
    }
}
<img src="./art/repo_header.png" alt="TopTabbedPage - A Xamarin.Forms control" width="728" />

# BottomTabbedPage
A Xamarin.Forms control brings BottomNavigationView for bottom navigation on Android

|![Tab 1](./art/tab1.png)|![Tab 5](./art/tab5.png)|
|:---:|:---:|

## About
This project is maintained by Naxam Co.,Ltd.<br>
We specialize in developing mobile applications using Xamarin and native technology stack.<br>

**Looking for developers for your project?**<br>

<a href="mailto:tuyen@naxam.net"> 
<img src="https://github.com/NAXAM/naxam.github.io/blob/master/assets/img/hire_button.png?raw=true" height="36"></a> <br>

## What's inside
BottomNavigationView is a new control since Android Support Design v25. This source code demonstrate the way to bring it into Xamarin.Forms.

However, BottomNavigationView is very limited from customizing, we have to employ library BottomNavigationViewEx as a patch.

In this code, we have
- A custom BottomTabbedPage
- A custom BottomTabbedRenderer

We could 
- change color of background, text of the bar, each item
- chagne the height of the bar
- change text typeface

# Depedendencies

- BottomNavigationViewEx [nuget](https://www.nuget.org/packages/Naxam.Ittianyu.BottomNavExtension/)

## How to use

### Install NUGET package
```
Install-Package Naxam.BottomTabbedPage
```

### Add XML namespace in your XAML
```xml
xmlns:naxam="clr-namespace:Naxam.Controls.Forms;assembly=Naxam.Controls.Forms"
```

### Change the root element to BottomTabbedPage (change the code behind as well)
```xml
<naxam:BottomTabbedPage
    xmlns="http://xamarin.com/schemas/2014/forms"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:local="clr-namespace:Demo"
    xmlns:naxam="clr-namespace:Naxam.Controls.Forms;assembly=Naxam.Controls.Forms"
    x:Class="Demo.MainPage">
    <local:Page1 />
    <local:Page2 />
    <local:Page3 />
    <local:Page4 />
    <local:Page5 />
</naxam:BottomTabbedPage>
```

### Change the colors/heights in your activity class
```c#
BottomTabbedRenderer.BackgroundColor = new Android.Graphics.Color(23, 31, 50);
BottomTabbedRenderer.FontSize = 10;
BottomTabbedRenderer.IconSize = 20;
BottomTabbedRenderer.ItemTextColor = stateList;
BottomTabbedRenderer.ItemIconTintList = stateList;
BottomTabbedRenderer.Typeface = Typeface.CreateFromAsset(this.Assets, "HiraginoKakugoProNW3.otf");
BottomTabbedRenderer.ItemBackgroundResource = Resource.Drawable.bnv_selector;
BottomTabbedRenderer.ItemSpacing = 8;
BottomTabbedRenderer.ItemPadding = new Xamarin.Forms.Thickness(8);
BottomTabbedRenderer.BottomBarHeight = 80;
BottomTabbedRenderer.ItemAlign = BottomTabbedRenderer.ItemAlignFlags.Center;
```

### Use Iconize for menu item
```c#
BottomTabbedRenderer.MenuItemIconSetter = (menuItem, iconSource) => {
    var iconized = Iconize.FindIconForKey(iconSource.File);
    if (iconized == null)
    {
        BottomTabbedRenderer.DefaultMenuItemIconSetter.Invoke(menuItem, iconSource);

        return;
    }

    var drawable = new IconDrawable(this, iconized).Color(Color.White).SizeDp(20);

    menuItem.SetIcon(drawable);
};
```

## Source Code
- Naxam.BottomNavs.Platform.Droid/*.cs
- Naxam.BottomNavs.Forms/BottomTabbedPage.cs

## Credits
This library currently uses `BottomNavigationViewEx` from @ittianyu [BottomNavigationViewEx](https://github.com/ittianyu/BottomNavigationViewEx) library.

## License

BottmTabbedPage is released under the Apache License license.
See [LICENSE](./LICENSE) for details.
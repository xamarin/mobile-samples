Imports Xamarin.Forms

Public Class App
    Inherits Application

    ''' <summary>
    ''' Have to implement the ctor and set MainPage for Xamarin.Forms to start
    ''' </summary>
    Public Sub New()
        ' *** BUILD UI IN CODE
        Dim label = New Label With {.XAlign = TextAlignment.Center,
                                    .FontSize = Device.GetNamedSize(NamedSize.Medium, GetType(Label)),
                                    .Text = "Welcome to Xamarin.Forms with Visual Basic.NET"}

        Dim stack = New StackLayout With {
            .VerticalOptions = LayoutOptions.Center
        }
        stack.Children.Add(label)

        Dim page = New ContentPage
        page.Content = stack
        'MainPage = page

        ' *** OR test out a separate ContentPage class
        MainPage = New Page2

        ' *** OR USE XAML FROM C# PROJECT
        ' *** include the XamlPages project to try this out
        'Dim xamlpage = New XamlPages.XamlPage1
        'xamlpage.BindingContext = "Hello from Visual Basic.NET"
        'MainPage = xamlpage

    End Sub

End Class

Imports Xamarin.Forms

Public Class Page2
    Inherits ContentPage

    Public Sub New()
        Dim label = New Label With {.XAlign = TextAlignment.Center,
                                    .FontSize = Device.GetNamedSize(NamedSize.Medium, GetType(Label)),
                                    .Text = "Visual Basic ContentPage"}

        Dim button = New Button With {.Text = "Click me"}
        AddHandler button.Clicked, Async Sub(sender, e)
                                       Await DisplayAlert("Hello from VB", "Visual Basic.NET is back!", "Thanks")
                                   End Sub

        Dim stack = New StackLayout With {
            .VerticalOptions = LayoutOptions.Center
        }
        stack.Children.Add(label)
        stack.Children.Add(button)

        Content = stack
    End Sub


End Class

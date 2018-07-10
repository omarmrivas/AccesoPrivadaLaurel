namespace AccesoPrivadaLaurel

open Xamarin.Forms
open Xamarin.Forms.Xaml
open System.Threading
open System.Threading.Tasks
open System.Net
open System.IO

module Model =
    type door = 
        | Exit
        | Enter

module Buttons =
    // Large button Height
    let SCALE = 2.0

    let BUTTON_BORDER_WIDTH = 1.0 * SCALE
    let BUTTON_HEIGHT = 88.0 * SCALE
    let BUTTON_HEIGHT_WP = 144.0 * SCALE
    let BUTTON_HALF_HEIGHT = int (44.0 * SCALE)
    let BUTTON_WIDTH = 88.0 * SCALE

    let getClircleButton color text =
        Button(HorizontalOptions = LayoutOptions.Center,
               BackgroundColor = color,
               BorderColor = Color.Black,
               TextColor = Color.White,
               BorderWidth = BUTTON_BORDER_WIDTH,
               BorderRadius = BUTTON_HALF_HEIGHT,
               HeightRequest = BUTTON_HEIGHT,
               MinimumHeightRequest = BUTTON_HEIGHT,
               WidthRequest = BUTTON_WIDTH,
               MinimumWidthRequest = BUTTON_WIDTH,
               Text = text,
               FontSize = Device.GetNamedSize(NamedSize.Large, typeof<Button>),
               VerticalOptions = LayoutOptions.CenterAndExpand)

module Http =
    let ERROR = "Connection failed!"

    let read_response (response : HttpWebResponse) =
        let receiveStream = response.GetResponseStream()
    //    let encode = System.Text.Encoding.GetEncoding("utf-8")
        let encode = System.Text.Encoding.GetEncoding("ISO-8859-1")
        let readStream = new StreamReader( receiveStream, encode )
        let response = readStream.ReadToEnd()
        readStream.Close()
        response

    let actionString = function
        | Model.door.Exit -> "http://192.168.0.100/exit/"
        | Model.door.Enter -> "http://192.168.0.100/enter/"

    let request_action number door =
        let cookie = new CookieContainer()
        let url = actionString door + number
        let request = WebRequest.CreateHttp(url)
        request.CookieContainer <- cookie
        try
            let response = request.GetResponse() :?> HttpWebResponse
            read_response response
        with | :? WebException as e -> ERROR + ": " + e.Message

    let timeout time def f v =
     try
        let tokenSource = new CancellationTokenSource()
        let token = tokenSource.Token
        let task = Task.Factory.StartNew(fun () -> f v, token)
        if not (task.Wait(time, token))
        then def
        else (fun (x, y) -> x) task.Result
     with e -> def

type MainPage(number : string) as this =
    inherit ContentPage()

//    let _ = base.LoadFromXaml(typeof<MainPage>)

    let requestTimeOut = 10000
    let delay = 1000

    // Create the Label for display.
(*    let labelStatus = Label(Text = "",
                            FontSize = Device.GetNamedSize(NamedSize.Large, typeof<Label>),
                            TextColor = Color.White,
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.CenterAndExpand)*)

    // Create the first Button and attach Clicked handler.
    let exitButton = Buttons.getClircleButton Color.Red "Activar Portón Salida"

    let showStatus msg =
        Device.BeginInvokeOnMainThread (fun () ->
            do this.DisplayAlert("Status", msg, "OK") |> ignore)

    let waitExit () =
        Thread(fun () -> let response = Http.timeout requestTimeOut "Timeout!" (Http.request_action number) Model.door.Exit
                         do Device.BeginInvokeOnMainThread(fun () ->
                                    do showStatus response)
                         if response.StartsWith(Http.ERROR)
                         then Device.BeginInvokeOnMainThread(fun () ->
                                    do exitButton.IsEnabled <- true)
                         else do Thread.Sleep(delay)
                              Device.BeginInvokeOnMainThread(fun () ->
                              do exitButton.IsEnabled <- true)).Start()

    do exitButton.Clicked.Add(fun _ -> do exitButton.IsEnabled <- false
                                       waitExit ())

    // Create the first Button and attach Clicked handler.
    let enterButton = Buttons.getClircleButton Color.Green "Activar Portón Entrada"

    let waitEnter () =
        Thread(fun () -> let response = Http.timeout requestTimeOut "Timeout!" (Http.request_action number) Model.door.Enter
                         do Device.BeginInvokeOnMainThread(fun () ->
                                    do showStatus response)
                         if response.StartsWith(Http.ERROR)
                         then Device.BeginInvokeOnMainThread(fun () ->
                                    do enterButton.IsEnabled <- true)
                         else do Thread.Sleep(delay)
                              Device.BeginInvokeOnMainThread(fun () ->
                              do enterButton.IsEnabled <- true)).Start()

    do enterButton.Clicked.Add(fun _ -> do enterButton.IsEnabled <- false
                                        waitEnter ())

    // Assemble the page.
(*    let buttonStack = StackLayout(Orientation = StackOrientation.Vertical,
                                  VerticalOptions = LayoutOptions.CenterAndExpand)

    do buttonStack.Children.Add enterButton
    do buttonStack.Children.Add exitButton*)

    let mainStack = StackLayout(BackgroundColor = Color.Black)
    do mainStack.Children.Add enterButton
    do mainStack.Children.Add exitButton

    do base.Content <- mainStack

            
